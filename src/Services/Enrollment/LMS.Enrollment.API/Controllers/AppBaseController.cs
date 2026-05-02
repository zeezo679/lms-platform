using LMS.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Enrollment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppBaseController : ControllerBase
    {
        // Standardized with data success response
        protected ActionResult Success<T>(T data, string message = "Operation Successfully")
        {
            return Ok(new ApiResponse<T>(true,data, message,200));
        }

        // Standardized without data success response
        protected ActionResult Success(string message = "Operation Successfully")
        {
            return Ok(new ApiResponse<string>(true,null!, message, 200));
        }

        // Standardized Created response
        protected ActionResult Created<T>(T data, string message = "Created Successfully")
        {
            return StatusCode(201, new ApiResponse<T>(true, data, message, 201));
        }

        protected ActionResult BadRequestError(string message)
        {
            return StatusCode(400, new ApiResponse<object>(false, null!, message, 400));
        }

        // 404 Not Found
        protected ActionResult NotFoundError(string message = "Resource not found")
        {
            return StatusCode(404, new ApiResponse<object>(false, null!, message, 404));
        }

        // 401 Unauthorized 
        protected ActionResult UnauthorizedError(string message = "You are not authorized")
        {
            return StatusCode(401, new ApiResponse<object>(false, null!, message, 401));
        }
    }
}
