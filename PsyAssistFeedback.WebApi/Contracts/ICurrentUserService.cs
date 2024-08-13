namespace PsyAssistFeedback.WebApi.Contracts;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
