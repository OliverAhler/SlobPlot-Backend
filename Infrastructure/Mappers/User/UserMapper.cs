using System.Linq.Expressions;
using Application.Features.Auth.DTOs;
using Infrastructure.Models.IDM;

namespace Infrastructure.Mappers.User;

public static class UserMapper
{
    // Expression that EF Core CAN translate to SQL
    public static Expression<Func<DbUser, UserDto>> ToDto => user => new UserDto(
        user.Id,
        user.UserName,
        user.CreatedAt
    );
    
    // Extension for materialized objects (when you already have DbUser in memory)
    public static UserDto ToDtoInstance(this DbUser user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.CreatedAt
        );
    }
    
    public static List<UserDto> ToDtoInstance(this IEnumerable<DbUser> users)
    {
        return users.Select(ToDtoInstance).ToList();
    }
}