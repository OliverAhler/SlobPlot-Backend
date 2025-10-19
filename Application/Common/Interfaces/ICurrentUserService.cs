namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    bool IsAuthenticated { get; }
}