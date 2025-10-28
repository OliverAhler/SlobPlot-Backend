namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    Task<Guid> GetUserIdAsync(CancellationToken cancellationToken);
    Guid GetSubId();
    bool IsAuthenticated { get; }
}