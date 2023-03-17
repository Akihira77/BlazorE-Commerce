using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

    public AuthController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
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
}
