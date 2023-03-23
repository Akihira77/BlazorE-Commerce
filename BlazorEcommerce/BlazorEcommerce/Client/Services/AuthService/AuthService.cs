namespace BlazorEcommerce.Client.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/change-password", request.Password);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }

	public async Task<ServiceResponse<bool>> ChangeRole(User user)
	{
        var result = await _http.PutAsJsonAsync("api/v1/auth/change-role", user);

		return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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

	public async Task<IEnumerable<UserDto>> GetUserList()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<UserDto>>>("api/v1/auth/get-all-user");

        return result.Data;
    }

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/login", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

    public async Task<ServiceResponse<int>> Register(UserRegister request)
    {
        var result = await _http.PostAsJsonAsync("api/v1/auth/register", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
    }
}
