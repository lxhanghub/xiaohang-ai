
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

/// <summary>
/// 仓储
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// 用户
    /// </summary>
    DbSet<User> Users { get; }


    /// <summary>
    /// 工作单元
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
