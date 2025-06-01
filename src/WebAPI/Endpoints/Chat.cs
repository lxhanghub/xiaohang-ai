using System;
using System.Runtime.CompilerServices;
using Application.Features.Chat.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace WebAPI.Endpoints;

public class Chat : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
         .AddEndpointFilter<ApiResponseFilter>()
         .MapPost(ChatAsync)
         .MapPost(ChatStreamAsync, "stream")
         .MapPost(PluginsAsync, "plugins")
         .MapPost(FunctionCallAsync, "functioncall");

    }

    /// <summary>
    /// 登录
    /// </summary>
    public async Task<string> ChatAsync(ISender sender, [FromBody] ChatQuery query)
    {
        var result = await sender.Send(query);

        return result ?? "No response from chat model.";
    }

    /// <summary>
    /// 流式聊天响应
    /// </summary>
    public async Task ChatStreamAsync(
            HttpContext context,
            ISender sender,
            [FromBody] ChatStreamQuery query,
            CancellationToken cancellationToken)
    {

        context.Response.ContentType = "text/event-stream";
        context.Response.Headers.CacheControl = "no-cache";
        context.Response.Headers.Connection = "keep-alive";

        await foreach (var chunk in sender.CreateStream(query, cancellationToken))
        {
            if (!string.IsNullOrEmpty(chunk))
            {
                await context.Response.WriteAsync($"data: {chunk}\n\n", cancellationToken);
                await context.Response.Body.FlushAsync(cancellationToken); // 关键：立即发送
            }
        }
    }

    public async Task<string> PluginsAsync(ISender sender, [FromBody] PluginsQuery query)
    {
        var result = await sender.Send(query);

        return result ?? "No response from plugins.";
    }

    public async Task<string> FunctionCallAsync(ISender sender, [FromBody] FunctionCallQuery query)
    {
        var result = await sender.Send(query);

        return result ?? "No response from function call.";
    }
}
