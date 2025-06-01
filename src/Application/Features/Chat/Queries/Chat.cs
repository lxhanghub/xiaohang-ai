using System;
using Application.Common.AI.ModelSelector;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Application.Features.Chat.Queries;

public record ChatQuery(string Message) : IRequest<string?>;


public class ChatQueryHandler(IModelSelector selector) : IRequestHandler<ChatQuery, string?>
{

    private readonly string prompt = "你是一个编程领域智能助手，擅长回答各种编程相关的问题,除了编程相关的问题是不能回答的,并提示用户你的作用。请根据用户的输入提供准确和有用的回答。";
    public Task<string?> Handle(ChatQuery request, CancellationToken cancellationToken)
    {
        var ChatCompletion = selector.GetTextModel();

        var history = new ChatHistory(prompt, AuthorRole.System);

        history.AddUserMessage(request.Message);

        var response = ChatCompletion.GetChatMessageContentAsync(history, new OpenAIPromptExecutionSettings()
        {
            MaxTokens = 1000, // 可根据需要调整
            Temperature = 0.7f, // 可根据需要调整
            TopP = 1.0f // 可根据需要调整

        }, cancellationToken: cancellationToken).Result;

        return Task.FromResult(response.Content);
    }
}