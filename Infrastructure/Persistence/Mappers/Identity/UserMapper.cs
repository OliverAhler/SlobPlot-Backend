using System.Linq.Expressions;
using Domain.UserManagement.Aggregates;
using Infrastructure.Persistence.Entities.Identity;

namespace Infrastructure.Persistence.Mappers.Identity;

public static class UserMapper
{
    // Expression that EF Core CAN translate to SQL
    private static readonly Expression<Func<DbUser, User>> ToDomainExpression = dbUser =>
        User.Reconstitute(
            dbUser.Id,
            dbUser.SubUid,
            dbUser.UserName,
            dbUser.CreatedAt,
            dbUser.UpdatedAt
        );
    
    /// <summary>
    /// Projects DbUser to Domain User in a queryable (translates to SQL).
    /// Use this for queries that haven't been executed yet.
    /// </summary>
    public static IQueryable<User> ProjectToDomain(this IQueryable<DbUser> query)
    {
        return query.Select(ToDomainExpression);
    }
    
    /// <summary>
    /// Maps a materialized DbUser to Domain User (in-memory).
    /// Use this when you already have a DbUser object.
    /// </summary>
    public static User ToDomain(this DbUser dbUser)
    {
        return User.Reconstitute(
            dbUser.Id,
            dbUser.SubUid,
            dbUser.UserName,
            dbUser.CreatedAt,
            dbUser.UpdatedAt
        );
    }
    
    // Domain User → DbUser
    public static DbUser ToDb(this User user)
    {
        return new DbUser
        {
            Id = user.Id.Value,
            SubUid = user.SubUid,
            UserName = user.UserName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsDeleted = false
        };
    }
    
    // Collection mapping
    public static List<User> ToDomain(this IEnumerable<DbUser> dbUsers)
    {
        return dbUsers.Select(ToDomain).ToList();
    }
}