using Application.Common.AI.ModelSelector;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Runtime.CompilerServices;

namespace Application.Features.Chat.Queries;

public record ChatStreamQuery(string Message) : MediatR.IStreamRequest<string>;

public class ChatStreamQueryHandler : MediatR.IStreamRequestHandler<ChatStreamQuery, string>
{
    private readonly IModelSelector _selector;

    public ChatStreamQueryHandler(IModelSelector selector)
    {
        _selector = selector;
    }

    public async IAsyncEnumerable<string> Handle(ChatStreamQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var chatCompletion = _selector.GetTextModel();
        var history = new ChatHistory(request.Message, AuthorRole.User);

        var stream = chatCompletion.GetStreamingChatMessageContentsAsync(history, cancellationToken: cancellationToken);

        await foreach (var chunk in stream.WithCancellation(cancellationToken))
        {
            if (chunk.Content != null)
            {
                yield return chunk.Content;
            }
        }
    }
}
