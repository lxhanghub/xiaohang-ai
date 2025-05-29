using Domain.Constants;
using Ardalis.GuardClauses;

namespace Domain.Common;

/// <summary>
/// 审计实体
/// </summary>
public class AuditableEntity : IAuditableEntity
{

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// 最后修改人
    /// </summary>
    public string? LastModifiedBy { get; set; }
}


/// <summary>
/// 审计实体
/// </summary>
public class AuditableEntity<TKey> : AuditableEntity, IAuditableEntity<TKey>
{
    /// <summary>
    /// 主键
    /// </summary>
    public TKey Id { get; init; }

    public AuditableEntity(TKey id)
    {
        Id = Guard.Against.Null
        (
            input: id,
            parameterName: nameof(id),
            exceptionCreator: () => new BusinessException(ExceptionMessage.IdNull)
        );
    }
}
