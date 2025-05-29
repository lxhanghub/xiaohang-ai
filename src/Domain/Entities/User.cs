using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;
using Domain.Constants;
namespace Domain.Entities;

/// <summary>
/// 用户
/// </summary>
public class User : AggregateRoot<Guid>
{
    /// <summary>
    /// 账号
    /// </summary>
    public string LoginName { get; init; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; private set; } = default!;

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; } = default!;

    private User() { }

    internal User(
        [NotNull] Guid id,
        [NotNull] string loginName,
        [NotNull] string password,
        string nickName) : base(id)
    {
        //校验登录名称为空，或者赋值
        LoginName = Guard.Against.NullOrWhiteSpace
        (
            input: loginName,
            parameterName: nameof(loginName),
            exceptionCreator: () => new BusinessException(ExceptionMessage.UserNameNull)
        );

        //校验昵称为空，或者赋值
        NickName = Guard.Against.NullOrWhiteSpace
        (
            input: nickName,
            parameterName: nameof(nickName),
            exceptionCreator: () => new BusinessException(ExceptionMessage.UserNameNull)
        );

        //校验密码为空，或者赋值
        Password = HashPassword(Guard.Against.NullOrWhiteSpace
        (
            input: password,
            parameterName: nameof(password),
            exceptionCreator: () => new BusinessException(ExceptionMessage.UserNameNull)
        ));
    }

    /// <summary>
    /// 更新密码
    /// </summary>
    public User UpdatePassword([NotNull] string password)
    {
        //校验密码为空，或者赋值
        Password = HashPassword(Guard.Against.NullOrWhiteSpace
        (
            input: password,
            parameterName: nameof(password),
            exceptionCreator: () => new BusinessException(ExceptionMessage.UserPasswordNull)
        ));

        //添加用户密码修改事件
        AddDomainEvent(new UserPasswordUpdateEvent { UserId = Id });

        return this;
    }

    /// <summary>
    /// 密码hash加密
    /// </summary>
    private string HashPassword([NotNull] string password)
    {
        //校验密码为空
        Guard.Against.NullOrWhiteSpace
        (
            input: password,
            parameterName: nameof(password),
            exceptionCreator: () => new BusinessException(ExceptionMessage.UserPasswordNull)
        );

        // 生成一个随机的盐
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[32]; // 生成一个32字节的盐
        rng.GetBytes(salt);

        // 使用HMAC-SHA256算法创建一个哈希对象
        using var hmac = new HMACSHA256(salt);

        // 计算密码的哈希值
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // 将盐和哈希值组合成一个单一的字节数组
        byte[] saltAndHash = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, saltAndHash, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, saltAndHash, salt.Length, hash.Length);

        // 将组合后的字节数组转换为Base64字符串
        return Convert.ToBase64String(saltAndHash);
    }

    /// <summary>
    /// 校验密码
    /// </summary>
    public bool ValidatePassword([NotNull] string password)
    {
        //校验密码为空
        Guard.Against.NullOrWhiteSpace
        (
            input: password,
            parameterName: nameof(password),
            exceptionCreator: () => new BusinessException(ExceptionMessage.UserPasswordNull)
        );

        byte[] saltAndHash = Convert.FromBase64String(Password);
        byte[] salt = new byte[32];
        byte[] hash = new byte[saltAndHash.Length - 32];

        // 从存储的哈希值中分离出盐和哈希值
        Buffer.BlockCopy(saltAndHash, 0, salt, 0, 32);
        Buffer.BlockCopy(saltAndHash, 32, hash, 0, hash.Length);

        // 使用相同的盐重新计算密码的哈希值
        using var hmac = new HMACSHA256(salt);
        byte[] testHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // 比较原始哈希值和新计算的哈希值
        return testHash.SequenceEqual(hash);
    }
}
