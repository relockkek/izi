using System;
using System.Collections.Generic;

namespace Spidran.DB;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public string? ClientIp { get; set; }

    public string? UserAgent { get; set; }

    public DateTime? CommandCreatedAt { get; set; }

    public int? AddressId { get; set; }

    public virtual ShippingAddress? Address { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User User { get; set; } = null!;
}
