using BlazorEcommerce.Shared.Dto.UserDTO;

namespace BlazorEcommerce.Client.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public List<UserDto> AdminUsers { get; set; } = new List<UserDto>();
    public string Name { get; set; }

    public event Action? OnChange;
    public AuthService(HttpClient http)
    {
        _http = http;
    }


	public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/change-password", request.Password);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }

	public async Task<ServiceResponse<string>> ChangeRole(User user)
	{
        var result = await _http.PutAsJsonAsync("api/v1/auth/change-role", user);

		return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
	}

	public async Task<string> GetRole(string email)
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/v1/auth/get-role/{email}");

        return result.Data;
    }

    public async Task<ServiceResponse<string>> GetToken(string email)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/forgot-password", email);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

	public async Task<User> GetUser(int userId)
	{
        var result = await _http.GetFromJsonAsync<ServiceResponse<User>>($"api/v1/auth/get-user/{userId}");

        return result.Data;
	}

	public async Task GetUserList()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<UserDto>>>("api/v1/auth/get-all-user");

        AdminUsers = result.Data.ToList();
    }

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/login", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

    public async Task<ServiceResponse<bool>> Register(UserRegister request)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/register", request);
        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

		return response;
    }

    public async Task DeleteUser(User user)
    {
        var result = await _http.DeleteAsync($"api/v1/auth/delete/{user.Id}");

        await GetUserList();
        OnChange?.Invoke();
    }
}
