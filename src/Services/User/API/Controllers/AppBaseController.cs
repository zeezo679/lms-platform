using LMS.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        protected ActionResult Success<T>(T data, string message = "Operation successful.")
            => Ok(new ApiResponse<T>(true, data, message, 200));

        protected ActionResult Success(string message = "Operation successful.")
            => Ok(new ApiResponse<string>(true, null!, message, 200));

        protected ActionResult Created<T>(T data, string message = "Created successfully.")
            => StatusCode(201, new ApiResponse<T>(true, data, message, 201));

        protected ActionResult NotFoundError(string message = "Resource not found.")
            => StatusCode(404, new ApiResponse<object>(false, null!, message, 404));

        protected ActionResult UnauthorizedError(string message = "You are not authorized.")
            => StatusCode(401, new ApiResponse<object>(false, null!, message, 401));
    }
}
