using System.ComponentModel;
using System.Security.Claims;

using Application.Common.Interfaces;

namespace Web.Services;

public class CurrentUser<TKey> : IUser<TKey>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public TKey? Id
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return default;

            var converter = TypeDescriptor.GetConverter(typeof(TKey));

            return (TKey)converter.ConvertFromInvariantString(userId)!;
        }
    }

    public List<string> Roles => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)?.Split(",")?.ToList() ?? [];
}
