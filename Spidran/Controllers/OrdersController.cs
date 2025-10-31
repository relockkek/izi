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
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("new")]
        public async Task<ActionResult<Result>> CreateOrder(CreateOrderCommand request)
        {
            var command = new CreateOrderCommand()
            {
                UserId = request.UserId,
                Items = request.Items.Select(i => new OrderItemDTO
                {
                    ProductId = i.ProductId,
                    Count = i.Count
                }).ToList(),
                ShippingAddress = new ShippingAddressDTO
                {
                    House = request.ShippingAddress.House,
                    Street = request.ShippingAddress.Street,
                    City = request.ShippingAddress.City,
                    PostalCode = request.ShippingAddress.PostalCode,
                },
                PaymentMethod = request.PaymentMethod
            };

            var result = await _mediator.SendAsync(command);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
