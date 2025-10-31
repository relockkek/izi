using System;
using System.Collections.Generic;

namespace Spidran.DB;

public partial class ShippingAddress
{
    public int Id { get; set; }

    public string? House { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
