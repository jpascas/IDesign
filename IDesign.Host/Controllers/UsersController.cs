using IDesign.Manager;
using IDesign.Manager.Models;
using MapsterMapper;
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
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginRequestModel)
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
    }
}
