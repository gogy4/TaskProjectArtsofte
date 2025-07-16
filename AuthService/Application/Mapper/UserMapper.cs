using Application.Models;
using Domain.Entity;

namespace Application.Mapper;

public static class UserMapper
{
    public static UserModel ToModel(User user)
    {
        return new UserModel(user.Id, user.Email, user.HashedPassword, user.Salt);
    }

    public static User ToEntity(UserModel userModel)
    {
        return new User(userModel.Id, userModel.Email, userModel.HashedPassword, userModel.Salt);
    }
}