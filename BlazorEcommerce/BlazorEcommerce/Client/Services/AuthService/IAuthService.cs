namespace BlazorEcommerce.Client.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<int>> Register(UserRegister request);
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
    Task<ServiceResponse<string>> GetToken(string email);
    Task<string> GetRole(string email);
    Task<IEnumerable<UserDto>> GetUserList();
    Task<User> GetUser(int userId);
    Task<ServiceResponse<bool>> ChangeRole(User user);
}
