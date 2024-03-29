﻿using BlazorEcommerce.Server.Features.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Features.Products;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("Get-all-categories")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> GetCategories()
    {
        var response = new ServiceResponse<IEnumerable<Category>>()
        {
            Data = await _unitOfWork.Category
                    .GetAll(c => c.Visible && !c.Deleted),
            Message = "Category list",
        };

        return Ok(response);
    }

    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> AdminGetAllCategories()
    {
        var response = new ServiceResponse<IEnumerable<Category>>()
        {
            Data = await _unitOfWork.Category
            .GetAll(c => !c.Deleted),
            Message = "Admin - Category list",
        };

        return Ok(response);
    }

    [HttpPost("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> AddCategory(Category category)
    {
        if (await _unitOfWork.Category.GetFirstOrDefault((c => c.Name.ToLower().Equals(category.Name.ToLower()))) != null)
        {
            return BadRequest(new ServiceResponse<string>
            {
                Message = "Name of The Category is Already Exists",
                Success = false
            });
        } 

        category.Editing = category.IsNew = false;
        await _unitOfWork.Category.Add(category);

        await _unitOfWork.Log.Add(new Logs
        {
            LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Adding new 'Category : {category.Name}'",
        });

        await _unitOfWork.SaveAsync();
        var response = new ServiceResponse<IEnumerable<Category>>
        {
            Data = await _unitOfWork.Category
            .GetAll(c => !c.Deleted),
            Message = $"Adding Category {category.Name} is success"
        };
        return Ok(response);
    }

    [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> DeleteCategory(int id)
    {
        var category = await _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
        var response = new ServiceResponse<IEnumerable<Category>>();

        if (category != null)
        {
            response.Message = "Deleting category is success";
            category.Deleted = true;
            //_unitOfWork.Category.Remove(category);

            await _unitOfWork.Log.Add(new Logs
            {
                LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Deleting 'Category : {category.Name}'",
            });

            await _unitOfWork.SaveAsync();
        }
        else
        {
            response.Success = false;
            response.Message = "Category is not found";
        }
        response.Data = await _unitOfWork.Category
            .GetAll(c => !c.Deleted);
        return Ok(response);
    }

    [HttpPut("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> UpdateCategory(Category category)
    {
        var dbCategory = await _unitOfWork.Category.GetFirstOrDefault(c => c.Id == category.Id, track: false);

        var response = new ServiceResponse<IEnumerable<Category>>();
        if (category != null)
        {
            response.Message = "Updating category is success";
            category.Editing = false;
            _unitOfWork.Category.Update(category);

            await _unitOfWork.Log.Add(new Logs
            {
                LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Updating 'Category : {category.Name}'",
            });

            await _unitOfWork.SaveAsync();
        }
        else
        {
            response.Success = false;
            response.Message = "Category is not found";
        }
        response.Data = await _unitOfWork.Category
            .GetAll(c => !c.Deleted);
        return Ok(response);
    }
}