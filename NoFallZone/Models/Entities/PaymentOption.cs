﻿namespace NoFallZone.Models.Entities;
public class PaymentOption
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Fee { get; set; } = decimal.Zero;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}