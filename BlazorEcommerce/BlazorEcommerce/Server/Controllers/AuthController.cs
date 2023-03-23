using BlazorEcommerce.Server.Services.EmailService;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IEmailSender _emailSender;

	public AuthController(IUnitOfWork unitOfWork, IEmailSender emailSender)
	{
		_unitOfWork = unitOfWork;
		_emailSender = emailSender;
	}

	[HttpPost("register")]
	public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
	{
		var user = await _unitOfWork.Auth
			.Register(new User
			{
				Email = request.Email,
			}, request.Password);

		var response = new ServiceResponse<int>()
		{
			Message = user.Message
		};

		if(user.Email.IsNullOrEmpty())
		{
			response.Success = false;
			return BadRequest(response);
		}
		await _unitOfWork.Auth.Add(user);
		await _unitOfWork.Save();

		response.Data = user.Id;
		return Ok(response);
	}

	[HttpPost("login")]
	public async Task<ActionResult<ServiceResponse<string?>>> Login(UserLogin request)
	{
		var result = await _unitOfWork.Auth.Login(request.Email, request.Password);
		var response = new ServiceResponse<string>()
		{
			Message = result,
			Success = false,
		};

		if(!(result.Equals("Wrong password")
					|| result.Equals("User not found")))
		{
			response.Success = true;
			response.Message = DateTime.Now.AddHours(1).ToString();
			response.Data = result;
		}

		return Ok(response);
	}

	[HttpPost("change-password")]
	public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
	{
		var response = new ServiceResponse<bool>()
		{
			Success = false,
			Message = "User not found."
		};


		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		var result = await _unitOfWork.Auth.ChangePassword(int.Parse(userId), newPassword);


		if(result == null)
		{
			return BadRequest(response);
		}

		_unitOfWork.Auth.Update(result);
		await _unitOfWork.Save();
		response.Success = true;
		response.Message = "Password has been changed.";

		return Ok(response);
	}

	[HttpPost("forgot-password")]
	public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword([FromBody] string email)
	{
		var response = new ServiceResponse<string>();
		
		var user = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Email.Equals(email)));

		if (user == null)
		{
			response.Success = false;
			response.Message = "User is not found";

			return NotFound(response);
		}
		response.Data = CreateRandomToken();
		var message = new Message(new string[] { user.Email }, "otp", response.Data);
		await _emailSender.SendEmailAsync(message);

		return Ok(response);
	}

	[HttpGet("get-role/{email}")]
	public async Task<ActionResult<ServiceResponse<string>>> GetRole(string email)
	{
		var response = new ServiceResponse<string>(); 
		var result = await _unitOfWork.Auth.GetUserByEmail(email);

		if (result == null)
		{
			response.Success = false;
			response.Message = "User is not found";

			return BadRequest(response);
		} 
		response.Data = result.Role;
		response.Success = true;
		response.Message = $"User {email} is found";

		return Ok(response);
	}
	[HttpGet("get-all-user")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<UserDto>>>> GetAllUsers()
	{
		var result = await _unitOfWork.Auth.GetUsers();
		var userList = new List<UserDto>();
		int idx = 1;
		foreach(var user in result)
		{
			userList.Add(new UserDto { Nr = idx++, User = user });
		}
		return Ok(new ServiceResponse<IEnumerable<UserDto>>() { Data = userList });	
	}
	[HttpGet("get-user/{userId}")]
	public async Task<ActionResult<ServiceResponse<User>>> GetUser(int userId)
	{
		var user = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == userId));
		var response = new ServiceResponse<User>();
		
		if (user == null)
		{
			response.Success = false;
			response.Message = "User is not found";
		} 

		response.Data = user;
		return Ok(response);
	}
	[HttpPut("change-role")]
	public async Task<ActionResult<ServiceResponse<bool>>> ChangeRole(User user)
	{
		var result = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == user.Id));
		var response = new ServiceResponse<bool>
		{
			Success = false,
			Message = "User is not found"
		};

		if(result != null)
		{
			_unitOfWork.Auth.Update(user);
			await _unitOfWork.Save();
			response.Success = true;
		}
		return Ok(response);
	}
	private static string CreateRandomToken()
	{
		return Convert.ToHexString(RandomNumberGenerator.GetBytes(3));
	}
}
