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

        public double GetItemTotal(int qty)
        {
            return qty * Price;
        }

        public bool HasEnoughStock(int qty)
        {
            return qty <= Stock;
        }

        public void DeductStock(int qty)
        {
            Stock -= qty;
        }
    }

    class CartItem
    {
        public Product Product;
        public int Quantity;

        public double GetTotal()
        {
            return Product.GetItemTotal(Quantity);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Product[] products = {
                new Product { Id = 1, Name = "Laptop", Price = 30000, Stock = 5 },
                new Product { Id = 2, Name = "Mouse", Price = 500, Stock = 10 },
                new Product { Id = 3, Name = "Keyboard", Price = 1000, Stock = 7 },
                new Product { Id = 4, Name = "Monitor", Price = 7000, Stock = 4 },
                new Product { Id = 5, Name = "Headset", Price = 1500, Stock = 6 },
                new Product { Id = 6, Name = "USB Flash Drive", Price = 300, Stock = 15 },
                new Product { Id = 7, Name = "Webcam", Price = 1200, Stock = 5 }
             }
            ;

            CartItem[] cart = new CartItem[5];
            int cartCount = 0;

            while (true)
            {
                Console.WriteLine("\n=== STORE MENU ===");
                foreach (var product in products)
                {
                    product.Show();
                }

                if (cartCount == cart.Length)
                {
                    Console.WriteLine("Cart is full! Cannot add more items.");
                    break;
                }

                Console.Write("\nEnter product number: ");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > products.Length)
                {
                    Console.WriteLine("Invalid product number.");
                    continue;
                }

                Product selected = products[choice - 1];

                if (selected.Stock == 0)
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

                if (!selected.HasEnoughStock(qty))
                {
                    Console.WriteLine("Not enough stock available.");
                    continue;
                }

                bool found = false;
                for (int i = 0; i < cartCount; i++)
                {
                    if (cart[i].Product == selected)
                    {
                        cart[i].Quantity += qty;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    cart[cartCount] = new CartItem
                    {
                        Product = selected,
                        Quantity = qty
                    };
                    cartCount++;
                }

                selected.DeductStock(qty);

                Console.WriteLine("Added to cart!");

                Console.Write("Add more? (Y/N): ");
                string ans = Console.ReadLine().ToUpper();
                if (ans == "N")
                    break;
            }

            double grandTotal = 0;
            Console.WriteLine("\n=== RECEIPT ===");

            for (int i = 0; i < cartCount; i++)
            {
                double total = cart[i].GetTotal();
                Console.WriteLine(cart[i].Product.Name + " x" + cart[i].Quantity + " = ₱" + total);
                grandTotal += total;
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
            foreach (var product in products)
            {
                Console.WriteLine(product.Name + ": " + product.Stock);
            }
        }
    }
}
