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
        public int RemainingStock;

        public void DisplayProduct()
        {
            Console.WriteLine(Id + ". " + Name + " - ₱" + Price + " (Stock: " + RemainingStock + ")");
        }

        public bool HasEnoughStock(int quantity)
        {
            return quantity <= RemainingStock;
        }

        public double GetItemTotal(int quantity)
        {
            return Price * quantity;
        }

        public void DeductStock(int quantity)
        {
            RemainingStock -= quantity;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Product[] products = {
                new Product { Id = 1, Name = "Laptop", Price = 30000, RemainingStock = 5 },
                new Product { Id = 2, Name = "Mouse", Price = 500, RemainingStock = 10 },
                new Product { Id = 3, Name = "Keyboard", Price = 1000, RemainingStock = 7 },
                new Product { Id = 4, Name = "Monitor", Price = 7000, RemainingStock = 4 },
                new Product { Id = 5, Name = "Headset", Price = 1500, RemainingStock = 6 },
                new Product { Id = 6, Name = "USB Flash Drive", Price = 300, RemainingStock = 15 },
                new Product { Id = 7, Name = "Webcam", Price = 1200, RemainingStock = 5 }
            };

            int[] cartQty = new int[products.Length];
            double[] cartTotal = new double[products.Length];

            while (true)
            {
                Console.WriteLine("\n=== STORE MENU ===");

                for (int i = 0; i < products.Length; i++)
                {
                    products[i].DisplayProduct();
                }

                Console.Write("\nEnter product number: ");
                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > products.Length)
                {
                    Console.WriteLine("Invalid product number.");
                    continue;
                }

                Product selectedProduct = products[choice - 1];

                if (selectedProduct.RemainingStock == 0)
                {
                    Console.WriteLine("Product is out of stock.");
                    continue;
                }

                Console.Write("Enter quantity: ");
                int quantity;

                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity.");
                    continue;
                }

                if (!selectedProduct.HasEnoughStock(quantity))
                {
                    Console.WriteLine("Not enough stock available.");
                    continue;
                }

                cartQty[choice - 1] += quantity;
                cartTotal[choice - 1] += selectedProduct.GetItemTotal(quantity);

                selectedProduct.DeductStock(quantity);

                Console.WriteLine("Added to cart!");

                Console.Write("Add more? (Y/N): ");
                string answer = Console.ReadLine().ToUpper();

                if (answer == "N")
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
                Console.WriteLine(products[i].Name + ": " + products[i].RemainingStock);
            }
        }
    }
}
