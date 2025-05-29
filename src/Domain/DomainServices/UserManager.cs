using System.Diagnostics.CodeAnalysis;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.DomainServices;
public class UserManager(IApplicationDbContext context) : DomainService
{
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="password"></param>
    /// <param name="nickName"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public async Task<User> CreateAsync(
        [NotNull] string loginName,
        [NotNull] string password,
        string nickName)
    {
        var user = new User
        (
            Guid.NewGuid(),
            loginName,
            password,
            nickName
        );

        var exist = await context.Users.AnyAsync(s => s.LoginName.Equals(loginName));

        return !exist ? user : throw new BusinessException(ExceptionMessage.UserExist);
    }
}
