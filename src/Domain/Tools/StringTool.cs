using System.Runtime.CompilerServices;

namespace Domain.Tools;
public static class StringTool
{
    public static string Format(this string str, params object[] objects)
    {
        return string.Format(str, objects);
    }

}
