using System;
using Application.Common.AI.ModelSelector;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Application.Features.Chat.Queries;

public record PluginsQuery(string Message) : IRequest<string>;


public class PluginsQueryHandler(Kernel kernel,KernelPlugin plugin) : IRequestHandler<PluginsQuery, string>
{


    public async Task<string> Handle(PluginsQuery request, CancellationToken cancellationToken)
    {
        var arguments = new KernelArguments
        {
            ["article"] = request.Message,
            // 设置使用哪个服务 ID（你注册模型服务时设置的名字）
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

        var result = await kernel.InvokeAsync(plugin["RewriteRedBookStyle"], arguments);

        return result.ToString() ?? string.Empty;
    }
}
