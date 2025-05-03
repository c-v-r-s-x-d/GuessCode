namespace GuessCode.API.Models.V1.Settings;

public class CodeExecutionSettings
{
    public string PythonImage { get; set; } = null!;
    
    public string PythonFileName { get; set; } = null!;
    
    public string CppImage { get; set; } = null!;
    
    public string CppFileName { get; set; } = null!;
    
    public string JavaImage { get; set; } = null!;
    
    public string JavaFileName { get; set; } = null!;
    
    public string CsharpImage { get; set; } = null!;
    
    public string CsharpFileName { get; set; } = null!;
}