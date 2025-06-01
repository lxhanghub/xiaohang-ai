using System;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Application.Common.AI.FunctionCalls;

public class WeatherPlugin
{    
    [KernelFunction("GetForecast")]
    [Description("获取给定位置的天气预报")]
    [return: Description("指定位置的天气预报")]
    public static string GetForecast( string  location )
    {
        return $"Sunny, 23℃";
    }
}
