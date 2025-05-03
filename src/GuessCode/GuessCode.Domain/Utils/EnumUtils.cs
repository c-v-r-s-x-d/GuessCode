using System.ComponentModel;
using System.Reflection;

namespace GuessCode.Domain.Utils;

public static class EnumUtils
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (name == null)
            return value.ToString();

        var field = type.GetField(name);
        if (field == null)
            return value.ToString();

        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
}