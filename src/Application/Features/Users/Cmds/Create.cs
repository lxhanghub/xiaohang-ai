using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.Dto;
using Domain.DomainServices;

namespace Application.Features.Users.Cmds;

/// <summary>
/// 创建
/// </summary>
/// <param name="LoginName">账号</param>
/// <param name="Password">密码</param>
public record CreateCmd(string LoginName, string Password, string nickName) : IRequest<UserDto>;

public class UserLoginQueryValidator : AbstractValidator<CreateCmd>
{
    public UserLoginQueryValidator()
    {
        RuleFor(v => v.LoginName).NotNull().NotEmpty().WithMessage("登录名为空");
        RuleFor(v => v.Password).NotNull().NotEmpty().WithMessage("密码为空");
    }
}
public class CreateHandler(IApplicationDbContext context, UserManager um) : IRequestHandler<CreateCmd, UserDto>
{
    public async Task<UserDto> Handle(CreateCmd cmd, CancellationToken cancellationToken)
    {
        var user = await um.CreateAsync
        (
            loginName: cmd.LoginName,
            password: cmd.Password,
            nickName: cmd.nickName
        );

        context.Users.Add(user);

        return user.Adapt<UserDto>();
    }
}
