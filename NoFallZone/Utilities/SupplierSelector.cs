using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Models;

namespace NoFallZone.Utilities;
public static class SupplierSelector
{
    public static Supplier? ChooseSupplier(NoFallZoneContext db)
    {
        var suppliers = db.Suppliers.ToList();

        if (suppliers.Count == 0)
        {
            Console.WriteLine("No suppliers found! Returning to main menu...");
            return null;
        }

        Console.WriteLine("\nChoose a supplier:");
        for (int i = 0; i < suppliers.Count; i++)
            Console.WriteLine($"{i + 1}. {suppliers[i].Name}");

        int index = InputHelper.PromptInt("Enter supplier number", 1, suppliers.Count,
            $"Please enter a number from 1 to {suppliers.Count}");

        return suppliers[index - 1];
    }
}
