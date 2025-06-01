using System;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Application.Features.Chat.Queries;

public record FunctionCallQuery(string local) : IRequest<string>;
public class FunctionCallHandler(Kernel kernel) : IRequestHandler<FunctionCallQuery, string>
{
    public async Task<string> Handle(FunctionCallQuery request, CancellationToken cancellationToken)
    {
        var prompt = "The weather today in {{ $location }} is {{GetForecast}}.";

        var arguments = new KernelArguments
        {
            ["location"] = request.local,

            ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
            {
                ["deepseek-chat"] = new OpenAIPromptExecutionSettings
                {
                    ServiceId = "deepseek-chat", // 替换成你注册时的 serviceId
                    Temperature = 0.7,
                    MaxTokens = 1000
                }
            }
        };
        
        var res = await kernel.InvokePromptAsync(prompt, arguments);

        return res.ToString() ?? string.Empty;
    }
}
