using Microsoft.EntityFrameworkCore;
using MyMediator.Interfaces;
using Spidran.Command;
using Spidran.DB;
using Spidran.DTO;

namespace Spidran.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result>
    {
        private readonly SpidranContext _context;

        public CreateOrderCommandHandler(SpidranContext context)
        {
            _context = context;
        }

        public async Task<Result> HandleAsync(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            var order = new Order
            {
                UserId = request.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "pending",
                Address = new ShippingAddress
                {
                    House = request.ShippingAddress.House,
                    Street = request.ShippingAddress.Street,
                    City = request.ShippingAddress.City,
                    PostalCode = request.ShippingAddress.PostalCode,
                },
                PaymentMethod = request.PaymentMethod
            };

            foreach (var item in request.Items)
            {
                var product = products[item.ProductId];
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    UnitPrice = product.Price
                });

                product.StockQuantity -= item.Count;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok("Заказ успешно создан", new { orderId = order.Id });
        }
    }
}
