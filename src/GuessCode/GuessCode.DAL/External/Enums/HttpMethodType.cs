using System.ComponentModel;

namespace GuessCode.DAL.External.Enums;

public enum HttpMethodType
{
    [Description("GET")]
    Get,
    
    [Description("POST")]
    Post,
    
    [Description("PUT")]
    Put,
    
    [Description("DELETE")]
    Delete
}