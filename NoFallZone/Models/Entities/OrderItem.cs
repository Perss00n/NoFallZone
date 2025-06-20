﻿namespace NoFallZone.Models.Entities;
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }

    public bool WasDeal { get; set; } = false;

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
