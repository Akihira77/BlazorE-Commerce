using BlazorEcommerce.Server.Features.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Features.Admins;
[Route("api/v1/[controller]")]
[ApiController]
//[Authorize(Roles = "Admin")]
public class LogsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public LogsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Logs>>>> GetAllLogs()
    {
        var result = await _unitOfWork.Log.GetAll();
        return Ok(new ServiceResponse<IEnumerable<Logs>>
        {
            Data = result.OrderBy(logs => logs.LogCreated)
        });
    }
}
