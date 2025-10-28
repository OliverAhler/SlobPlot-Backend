namespace API.Contracts.Users.GetAuthenticatedUser;

public record AuthenticatedUserResponse(
    string UserName,
    DateTime CreatedAt
);