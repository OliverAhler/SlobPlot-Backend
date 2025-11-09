namespace Domain.Common.Authorization;

public enum AuthorizationPolicy
{
    MustBeOwner,
    MustBeOwnerOrModerator,
    MustBeAdmin,
    Public // Anyone can access
}