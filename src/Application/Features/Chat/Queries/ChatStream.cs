using Application.Common.AI.ModelSelector;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Runtime.CompilerServices;

namespace Application.Features.Chat.Queries;

public record ChatStreamQuery(string Message) : MediatR.IStreamRequest<string>;

public class ChatStreamQueryHandler : MediatR.IStreamRequestHandler<ChatStreamQuery, string>
{
    private readonly string prompt = "你是一个编程智能助手，擅长回答各种编程问题。请根据用户的输入提供准确和有用的回答。";
    private readonly IModelSelector _selector;

    public ChatStreamQueryHandler(IModelSelector selector)
    {
        _selector = selector;
    }

    public async IAsyncEnumerable<string> Handle(ChatStreamQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var chatCompletion = _selector.GetTextModel();
        var history = new ChatHistory(prompt, AuthorRole.System);

        history.AddUserMessage(request.Message);

        var stream = chatCompletion.GetStreamingChatMessageContentsAsync(history,  new OpenAIPromptExecutionSettings()
        {
            MaxTokens = 1000, // 可根据需要调整
            Temperature = 0.7f, // 可根据需要调整
            TopP = 1.0f // 可根据需要调整
         }, cancellationToken: cancellationToken);

        await foreach (var chunk in stream.WithCancellation(cancellationToken))
        {
            if (chunk.Content != null)
            {
                yield return chunk.Content;
            }
        }
    }
}
