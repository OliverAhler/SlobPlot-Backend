namespace Application.Features.Auth.Queries.GetUserBySub;

public record GetUserBySubResponse(
    Guid Id,
    Guid SubUid,
    string UserName,
    DateTime CreatedAt
);