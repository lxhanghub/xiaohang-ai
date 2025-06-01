using System;
using Application.Common.AI.ModelSelector;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Application.Features.Chat.Queries;

public record ChatQuery(string Message) : IRequest<string?>;


public class ChatQueryHandler(IModelSelector selector) : IRequestHandler<ChatQuery, string?>
{
    public Task<string?> Handle(ChatQuery request, CancellationToken cancellationToken)
    {
        var ChatCompletion = selector.GetTextModel();

        var history = new ChatHistory(request.Message, AuthorRole.User);

        var response = ChatCompletion.GetChatMessageContentAsync(history, cancellationToken: cancellationToken).Result;

        return Task.FromResult(response.Content);
    }
}