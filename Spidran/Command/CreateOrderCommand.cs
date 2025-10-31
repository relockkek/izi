using Spidran.DB;
using Spidran.DTO;

namespace Spidran.Command
{
    public class CreateOrderCommand : CommandBase
    {
        public int UserId { get; set; }
        public List<OrderItemDTO> Items { get; set; }
        public ShippingAddressDTO ShippingAddress { get; set; } = new();
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
