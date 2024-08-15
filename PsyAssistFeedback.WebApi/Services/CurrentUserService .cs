using PsyAssistFeedback.WebApi.Contracts;
using System.Security.Claims;

namespace PsyAssistFeedback.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid UserId
        => Guid.TryParse(_contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) 
            ? id 
            : Guid.Empty;
}
