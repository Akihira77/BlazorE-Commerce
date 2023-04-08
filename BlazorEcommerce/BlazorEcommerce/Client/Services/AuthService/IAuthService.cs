namespace BlazorEcommerce.Client.Services.AuthService;

public interface IAuthService
{
    event Action? OnChange;
    List<UserDto> AdminUsers { get; set; }
    Task<bool> Register(UserRegister request);
    Task<ServiceResponse<string>> Login(UserLogin request);
    Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
    Task<ServiceResponse<string>> GetToken(string email);
    Task<string> GetRole(string email);
    Task GetUserList();
    Task<User> GetUser(int userId);
    Task<ServiceResponse<string>> ChangeRole(User user);
    Task DeleteUser(User user);
}
