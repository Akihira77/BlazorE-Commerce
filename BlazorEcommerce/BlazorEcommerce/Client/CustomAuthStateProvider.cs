using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorEcommerce.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
	private readonly ILocalStorageService _localStorage;
	private readonly HttpClient _http;

	public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
	{
		_localStorage = localStorage;
		_http = http;
	}
	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		string authToken = await _localStorage.GetItemAsStringAsync("authToken");

		// if there are not a authToken
		var identity = new ClaimsIdentity();
		_http.DefaultRequestHeaders.Authorization = null;

		// else 
		if(!string.IsNullOrEmpty(authToken))
		{
			try
			{
				// create
				identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
				_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
			} catch(Exception)
			{
				// if get error while create/parsing remove
				await _localStorage.RemoveItemAsync("authToken");
				identity = new ClaimsIdentity();
			}
		}

		var user = new ClaimsPrincipal(identity);
		var state = new AuthenticationState(user);

		NotifyAuthenticationStateChanged(Task.FromResult(state));

		return state;
	}

	private static byte[] ParseBase64WithoutPadding(string base64)
	{
		switch(base64.Length % 4)
		{
			case 2:
				base64 += "==";
				break;
			case 3:
				base64 += "=";
				break;
		}

		return Convert.FromBase64String(base64);
	}

	private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
	{
		var payload = jwt.Split('.')[1];
		var jsonBytes = ParseBase64WithoutPadding(payload);
		var keyValuePairs = JsonSerializer
			.Deserialize<Dictionary<string, object>>(jsonBytes);

		var claims = keyValuePairs
			.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

		return claims;
	}
}
