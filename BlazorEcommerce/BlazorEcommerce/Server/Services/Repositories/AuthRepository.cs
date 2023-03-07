using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.Repositories;

public class AuthRepository : Repository<User>, IAuthRepository
{
	private readonly AppDbContext _db;
	private readonly IConfiguration _configuration;

	public AuthRepository(AppDbContext db, IConfiguration configuration) : base(db)
	{
		_db = db;
		_configuration = configuration;
	}

	public async Task<string> Login(string email, string password)
	{
		string message = "";
		var user = await _db.Users
			.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

		if(user == null)
		{
			message = "User not found";
		} else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
		{
			message = "Wrong password";
		} else
		{
			message = CreateToken(user);
		}

		return message;
	}

	public async Task<User> Register(User user, string password)
	{
		if(await UserExists(user.Email))
		{
			return new User() { Id = 0, Message = "User email is already exists." };
		}

		CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

		user.PasswordHash = passwordHash;
		user.PasswordSalt = passwordSalt;
		user.Message = "User registration is successful!";
		return user;
	}

	public async Task<bool> UserExists(string email)
	{

		return await _db.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));
	}

	private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512();
		passwordSalt = hmac.Key;
		passwordHash = hmac
					.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
	}

	private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512(passwordSalt);
		var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

		return computeHash.SequenceEqual(passwordHash);
	}

	private string CreateToken(User user)
	{
		List<Claim> claims = new List<Claim>
		{
			// will use fo changePassword
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Email)
		};

		var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.Now.AddDays(1),
			//expires: DateTime.Now,
			signingCredentials: creds
		);

		var jwt = new JwtSecurityTokenHandler().WriteToken(token);

		return jwt;
	}

	public async Task<User> ChangePassword(int userId, string newPassword)
	{
		var user = await _db.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.Id == userId);

		if(user == null)
		{
			return null;
		}

		CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

		user.PasswordHash = passwordHash;
		user.PasswordSalt = passwordSalt;

		return user;
	}

	public void Update(User user)
	{
		_db.Users.Update(user);
	}
}
