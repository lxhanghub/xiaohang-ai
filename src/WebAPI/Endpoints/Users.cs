using System.Security.Claims;
using Application.Features.Users.Queries;

namespace Web;
public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .AddEndpointFilter<ApiResponseFilter>()
           .MapPost(UserLogin, "Login");
    }

    /// <summary>
    /// 登录
    /// </summary>
    public async Task<object> UserLogin(ISender sender, TokenBuilder tokenBuilder, UserLogin query)
    {
        var userDto = await sender.Send(query);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
        };

        string token = tokenBuilder.Build(claims);

        return new { User = userDto, Token = token };
    }
}
