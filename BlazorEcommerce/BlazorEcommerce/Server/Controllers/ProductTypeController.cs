﻿using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ProductTypeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("get-all-producttype")]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetAllProductTypes()
    {
        var response = new ServiceResponse<List<ProductType>>
        {
            Data = (await _unitOfWork.ProductType.GetAll()).ToList(),
            Message = "Product type list"
        };

        return Ok(response);
    }

    [HttpPost("add-producttype")]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType)
    {
        productType.Editing = productType.IsNew = false;
        await _unitOfWork.ProductType.Add(productType);
        await _unitOfWork.Save();

		var response = new ServiceResponse<List<ProductType>>
		{
			Data = (await _unitOfWork.ProductType.GetAll()).ToList(),
			Message = "Adding Product Type is success"
		};

		return Ok(response);
	}

    [HttpPut("update-producttype")]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductType(ProductType productType)
    {
        var result = await _unitOfWork.ProductType.GetFirstOrDefault((pt => pt.Id == productType.Id), track: false);

        var response = new ServiceResponse<List<ProductType>>();

		if (result == null)
        {
            response.Success = false;
            response.Message = "Product type is not found";

        } else
        {
            productType.Editing = false;
            _unitOfWork.ProductType.Update(productType);
            await _unitOfWork.Save();
            response.Data = (await _unitOfWork.ProductType.GetAll()).ToList();
            response.Message = "Updating product type is success";
        }

		return Ok(response);
	}

    [HttpDelete("delete-producttype/{id}")]
    public async Task<ActionResult<ServiceResponse<bool>>> DeleteProductType(int id)
    {
        var productType = await _unitOfWork.ProductType.GetFirstOrDefault((pt => pt.Id == id));

        var response = new ServiceResponse<bool>();
        if (productType != null)
        {
            response.Data = true;
            response.Message = "Deleting Product Type is success";
        } else
        {
            response.Data = false;
            response.Message = "Product Type is not found";
        }

        return Ok(response);
    }
}