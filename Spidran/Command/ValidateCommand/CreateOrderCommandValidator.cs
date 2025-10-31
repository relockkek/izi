using FluentValidation;
using Spidran.DB;
using Spidran.DTO;
namespace Spidran.Command.ValidateCommand
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        private readonly SpidranContext _context;

        public CreateOrderCommandValidator(SpidranContext context)
        {
            _context = context;

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId обязателен")
                .Must(UserExists).WithMessage("Пользователь не найден");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Заказ должен содержать товары")
                .Must(AllProductsExist).WithMessage("Один или несколько товаров не найдены")
                .Must(AllProductsInStock).WithMessage("Недостаточно товаров на складе");

            RuleFor(x => x.ShippingAddress.House)
                .NotEmpty().WithMessage("Номер дома обязателен");


            RuleFor(x => x.ShippingAddress.Street)
                .NotEmpty().WithMessage("Номер улицы обязателен");

            RuleFor(x => x.ShippingAddress.City)
                .NotEmpty().WithMessage("Город обязателен");

            RuleFor(x => x.ShippingAddress.PostalCode)
                .NotEmpty().WithMessage("Почтовый индекс обязателен");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Способ оплаты обязателен")
                .Must(BeValidPaymentMethod).WithMessage("Недопустимый способ оплаты");
        }

        private bool UserExists(int  userId) => _context.Users.Any(u => u.Id == userId);
        private bool AllProductsExist(List<OrderItemDTO> items)
        {
            var productIds = items.Select(i => i.ProductId).Distinct();
            var existingProducts = _context.Products.Where(p => productIds.Contains(p.Id)).Select(p => p.Id);
            return productIds.All(id => existingProducts.Contains(id));
        }

        private bool AllProductsInStock(List<OrderItemDTO> items) {
            var productStocks = _context.Products
                .Where(p => items.Select(i => i.ProductId).Contains(p.Id))
                .ToDictionary(p => p.Id, p => p.StockQuantity);

            return items.All(item =>
            productStocks.ContainsKey(item.ProductId) &&
            productStocks[item.ProductId] >= item.Count);

        }
        private bool BeValidPaymentMethod(string method) =>
            method == "credit_card" || method == "SBP" || method == "cash"; 
    }
}
