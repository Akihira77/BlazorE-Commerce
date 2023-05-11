using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Auths;

public interface IAuthRepository : IRepository<User>
{
    Task<User> Register(User user, string password);

    Task<bool> UserExists(string email);

    Task<string> Login(string email, string password);

    Task<User> ChangePassword(int userId, string newPassword);

    void Update(User user);

    int GetUserId();

    string GetUserEmail();

    Task<User> GetUserByEmail(string email);

    Task<IEnumerable<User>> GetUsers();
}