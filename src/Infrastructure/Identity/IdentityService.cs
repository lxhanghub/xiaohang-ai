using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;
internal class IdentityService(IApplicationDbContext context) : IIdentityService
{
    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        //TODO:未实现
        await Task.CompletedTask;
        return true;
    }
    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

        return user?.NickName;
    }

    public Task<bool> IsInRoleAsync(string userId, string role)
    {
        //TODO:暂未实现
        return Task.FromResult(true);
    }
}
