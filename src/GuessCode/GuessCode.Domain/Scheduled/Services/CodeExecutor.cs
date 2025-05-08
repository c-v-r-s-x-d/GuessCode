using System.ComponentModel.DataAnnotations;
using GuessCode.API.Models.V1.Settings;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.Domain.Scheduled.Models;
using GuessCode.Domain.Utils;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Options;

namespace GuessCode.Domain.Scheduled.Services;

public class CodeExecutor
{
    private const string ImagePrefix = "guesscode/";
    private const string WrapperDirectory = "/app/compilers/";
    private const string TestFileDirectory = "/app/local/";
    private const string TestFileExtension = ".txt";

    private const string UserCodeReplaceKey = @"{{USER_CODE}}";
    private const string FuncNameReplaceKey = @"{{FUNC_NAME}}";
    private const string ArgsReplaceKey = @"{{ALL_ARGS_HERE}}";
    
    private readonly CodeExecutionSettings _settings;
    private readonly Dictionary<string, Func<CodeExecutionTask, Task<string>>> _runners;

    public CodeExecutor(IOptions<CodeExecutionSettings> settings)
    {
        _settings = settings.Value;
        _runners = new Dictionary<string, Func<CodeExecutionTask, Task<string>>>
        {
            {
                ProgrammingLanguage.Python.GetDescription(),
                task => RunContainerAsync(_settings.PythonImage, _settings.PythonFileName, task.SourceCode, task.InputFile)
            },
            {
                ProgrammingLanguage.Cpp.GetDescription(),
                task => RunContainerAsync(_settings.CppImage, _settings.CppFileName, task.SourceCode, task.InputFile)
            },
            {
                ProgrammingLanguage.Java.GetDescription(),
                task => RunContainerAsync(_settings.JavaImage, _settings.JavaFileName, task.SourceCode, task.InputFile)
            },
            {
                ProgrammingLanguage.Csharp.GetDescription(),
                task => RunContainerAsync(_settings.CsharpImage, _settings.CsharpFileName, task.SourceCode, task.InputFile)
            },
        };
    }

    public async Task<string> ExecuteAsync(CodeExecutionTask task)
    {
        if (_runners.TryGetValue(task.Language, out var runner))
            return await runner(task);

        throw new ValidationException($"Unknown language: {task.Language}");
    }

    private async Task<string> RunContainerAsync(string image, string fileName, string code, Guid fileId)
    {
        var executableFile = BuildExecutableFile(code, fileId.ToString(), image);
        var input = GetTestFileContent(fileId.ToString());
        
        Console.WriteLine(executableFile);
        Console.WriteLine(input);
        
        image = ImagePrefix + image;
        
        var uniqueId = Guid.NewGuid().ToString("N");
        var podName = $"runner-{uniqueId}";

        // 1. Init Kubernetes client
        var config = KubernetesClientConfiguration.InClusterConfig();
        var client = new Kubernetes(config);

        // 2. Create config map with user code and input
        var configMap = new V1ConfigMap
        {
            Metadata = new V1ObjectMeta
            {
                Name = $"runner-cm-{uniqueId}",
                NamespaceProperty = "dev"
            },
            Data = new Dictionary<string, string>
            {
                { fileName, executableFile },
                { "input.txt", input }
            }
        };
        await client.CreateNamespacedConfigMapAsync(configMap, "dev");

        // 3. Create pod with the specified image
        var pod = new V1Pod
        {
            Metadata = new V1ObjectMeta
            {
                Name = podName,
                NamespaceProperty = "dev",
                Labels = new Dictionary<string, string> { { "app", "code-runner" } }
            },
            Spec = new V1PodSpec
            {
                RestartPolicy = "Never",
                Containers = new List<V1Container>
                {
                    new()
                    {
                        Name = "runner",
                        Image = image,
                        VolumeMounts = new List<V1VolumeMount>
                        {
                            new()
                            {
                                Name = "input-volume",
                                MountPath = "/app/userdata/input",
                                ReadOnlyProperty = true
                            },
                            new()
                            {
                                Name = "output-volume",
                                MountPath = "/app/userdata/output"
                            }
                        }
                    }
                },
                Volumes = new List<V1Volume>
                {
                    new()
                    {
                        Name = "input-volume",
                        ConfigMap = new V1ConfigMapVolumeSource
                        {
                            Name = configMap.Metadata.Name
                        }
                    },
                    new()
                    {
                        Name = "output-volume",
                        EmptyDir = new V1EmptyDirVolumeSource()
                    }
                }
            }
        };

        await client.CreateNamespacedPodAsync(pod, "dev");

        // 4. Ждем завершения
        while (true)
        {
            var status = await client.ReadNamespacedPodStatusAsync(podName, "dev");
            if (status.Status.Phase == "Succeeded" || status.Status.Phase == "Failed")
                break;

            await Task.Delay(500);
        }

        // 5. Читаем логи
        var logs = await client.ReadNamespacedPodLogAsync(podName, "dev");

        // 6. Чистим ресурсы
        await client.DeleteNamespacedPodAsync(podName, "dev");
        await client.DeleteNamespacedConfigMapAsync(configMap.Metadata.Name, "dev");

        using var streamReader = new StreamReader(logs);
        return await streamReader.ReadToEndAsync();
    }

    private static string BuildExecutableFile(string userCode, string testFileId, string directory)
    {
        var templateFile = GetTemplateFileContent(directory);
        var testFile = GetTestFileContent(testFileId);
        
        var lines = testFile.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        
        var functionName = lines[2].Trim();
        var argumentCount = int.Parse(lines[3]);
            
        var executableFile = templateFile
            .Replace(UserCodeReplaceKey, userCode)
            .Replace(FuncNameReplaceKey, functionName)
            .Replace(ArgsReplaceKey, string.Join(", ", Enumerable.Range(0, argumentCount).Select(x => $"args[{x}]")));
            
        return executableFile;
    }

    private static string GetTemplateFileContent(string directory)
    {
        var path = WrapperDirectory + directory + "/wrapper.txt";
        return System.IO.File.ReadAllText(path);
    }

    private static string GetTestFileContent(string fileName)
    {
        var path = TestFileDirectory + fileName + TestFileExtension;
        return System.IO.File.ReadAllText(path);
    }
}