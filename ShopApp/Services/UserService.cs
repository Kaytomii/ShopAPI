using ShopApi.Interfaces;
using ShopDomain.Models;

namespace ShopApi.Services;

public class UserService : IUserService
{
    private List<User> _users = new();

    public void AddUser(User user)
    {
        _users.Add(user);
    }
}
