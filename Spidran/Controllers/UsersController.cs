using Microsoft.AspNetCore.Mvc;
using MyMediator.Interfaces;
using Spidran.DB;
using Spidran.Command;
using Spidran.Behaviors;
using Spidran.DTO;
using Spidran.Handlers;

namespace Spidran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Result>> Register(RegisterUserCommand request)
        {
            var command = new RegisterUserCommand
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Phone = request.Phone,
                Info = request.Info
            };

            var result = await _mediator.SendAsync(command);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }


}
