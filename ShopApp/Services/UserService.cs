using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Services;

public class UserService : IUserService
{
    private List<User> _users = new();

    public void AddUser(User user)
    {
        _users.Add(user);
    }
}
