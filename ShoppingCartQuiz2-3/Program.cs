using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingCartQuiz2_3
{
   class Product
    {
        public int Id;
        public string Name;
        public double Price;
        public int Stock;

        public void Show()
        {
            Console.WriteLine(Id + ". " + Name + " - ₱" + Price + " (Stock: " + Stock + ")");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Product[] products = {
                new Product { Id = 1, Name = "Laptop", Price = 30000, Stock = 5 },
                new Product { Id = 2, Name = "Mouse", Price = 500, Stock = 10 },
                new Product { Id = 3, Name = "Keyboard", Price = 1000, Stock = 7 }
            };

            int[] cartQty = new int[3];
            double[] cartTotal = new double[3];

            while (true)
            {
                Console.WriteLine("\n=== STORE MENU ===");
                for (int i = 0; i < products.Length; i++)
                {
                    products[i].Show();
                }

                Console.Write("\nEnter product number: ");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > products.Length)
                {
                    Console.WriteLine("Invalid product number.");
                    continue;
                }

                Product p = products[choice - 1];

                if (p.Stock == 0)
                {
                    Console.WriteLine("Product is out of stock.");
                    continue;
                }

                Console.Write("Enter quantity: ");
                int qty;
                if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                {
                    Console.WriteLine("Invalid quantity.");
                    continue;
                }

                if (qty > p.Stock)
                {
                    Console.WriteLine("Not enough stock available.");
                    continue;
                }

                cartQty[choice - 1] += qty;
                cartTotal[choice - 1] += qty * p.Price;

                p.Stock -= qty;

                Console.WriteLine("Added to cart!");

                Console.Write("Add more? (Y/N): ");
                string ans = Console.ReadLine().ToUpper();
                if (ans == "N")
                    break;
            }

            double grandTotal = 0;
            Console.WriteLine("\n=== RECEIPT ===");

            for (int i = 0; i < products.Length; i++)
            {
                if (cartQty[i] > 0)
                {
                    Console.WriteLine(products[i].Name + " x" + cartQty[i] + " = ₱" + cartTotal[i]);
                    grandTotal += cartTotal[i];
                }
            }

            Console.WriteLine("Grand Total: ₱" + grandTotal);

            double discount = 0;
            if (grandTotal >= 5000)
            {
                discount = grandTotal * 0.10;
                Console.WriteLine("Discount (10%): ₱" + discount);
            }

            double finalTotal = grandTotal - discount;
            Console.WriteLine("Final Total: ₱" + finalTotal);

            Console.WriteLine("\n=== UPDATED STOCK ===");
            for (int i = 0; i < products.Length; i++)
            {
                Console.WriteLine(products[i].Name + ": " + products[i].Stock);
            }

        }
    }
}
