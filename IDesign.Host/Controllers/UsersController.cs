using IDesign.Manager;
using IDesign.Manager.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDesign.Host.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly ILoginManager _manager;
        private readonly IMapper _mapper;
        public UsersController(ILoginManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        [HttpPost("login")]        
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginRequestModel)
        {            
            var tokenResult = await _manager.HandleLoginAsync(loginRequestModel);            

            if (tokenResult.Success)
            {
                return Ok(tokenResult.Result);
            }
            else
            {
                return ToFailureResult(tokenResult);
            }
        }

        [Authorize]
        [HttpGet("usersonly")]
        public async Task<ActionResult> ActionForUsersOnly()
        {
            return Ok($"You are an authenticated user with id {this.GetCurrentUserId()} with role {this.GetCurrentUserRole()}");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminonly")]
        public async Task<ActionResult> ActionForAdminsOnly()
        {
            return Ok($"You are an authenticated user with id {this.GetCurrentUserId()} with role {this.GetCurrentUserRole()}");
        }
    }
}
