using System.Security.Claims;
using IDesign.Manager;
using IDesign.Manager.Models;
using Microsoft.AspNetCore.Mvc;

namespace IDesign.Host.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ApiController() { }

        protected ObjectResult ToFailureResult<T>(OperationResult<T> operationResult, int defaultCode = 500)
        {
            ErrorResult result = new ErrorResult();
            result.Messages.AddRange(operationResult.FailureMessages);
            return StatusCode(operationResult.FailureCode ?? defaultCode, result);
        }
        protected long GetCurrentUserId()
        {
            return  Convert.ToInt64(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected string GetCurrentUserRole()
        {
            return this.User.FindFirst(ClaimTypes.Role).Value;
        }
    }
}
