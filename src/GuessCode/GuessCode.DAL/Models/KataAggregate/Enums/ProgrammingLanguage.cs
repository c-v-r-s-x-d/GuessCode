using System.ComponentModel;

namespace GuessCode.DAL.Models.Enums;

public enum ProgrammingLanguage
{
    [Description("cpp")]
    Cpp = 1,
    
    [Description("csharp")]
    Csharp = 2,
    
    [Description("python")]
    Python = 3,
    
    [Description("java")]
    Java = 4
}