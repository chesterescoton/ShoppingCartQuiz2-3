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
        public string Category;

        public void Show()
        {
            Console.WriteLine(Id + ". " + Name + " - ₱" + Price + " (Stock: " + Stock + ")");
        }

        public double GetItemTotal(int qty) => qty * Price;
        public bool HasEnoughStock(int qty) => qty <= Stock;
        public void DeductStock(int qty) => Stock -= qty;
        public void AddStock(int qty) => Stock += qty;
    }

    class CartItem
    {
        public Product Product;
        public int Quantity;

        public double GetTotal() => Product.GetItemTotal(Quantity);
    }

    class Order
    {
        public int ReceiptNo;
        public double Total;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Product[] products = {
                new Product { Id = 1, Name = "Laptop", Price = 30000, Stock = 5, Category="Electronics"},
                new Product { Id = 2, Name = "Mouse", Price = 500, Stock = 10, Category="Electronics"},
                new Product { Id = 3, Name = "Keyboard", Price = 1000, Stock = 7, Category="Electronics"},
                new Product { Id = 4, Name = "Monitor", Price = 7000, Stock = 4, Category="Electronics"}
            };

            CartItem[] cart = new CartItem[5];
            int cartCount = 0;

            Order[] history = new Order[10];
            int historyCount = 0;
            int receiptNo = 1;

            while (true)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. View Cart");
                Console.WriteLine("3. Checkout");
                Console.WriteLine("4. Order History");
                Console.WriteLine("5. Exit");

                if (!int.TryParse(Console.ReadLine(), out int menu))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                if (menu == 1)
                {
                    foreach (var p in products) p.Show();

                    Console.Write("Enter product ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id) || id < 1 || id > products.Length)
                    {
                        Console.WriteLine("Invalid ID.");
                        continue;
                    }

                    Product selected = products[id - 1];

                    Console.Write("Enter quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                    {
                        Console.WriteLine("Invalid quantity.");
                        continue;
                    }

                    if (selected.HasEnoughStock(qty))
                    {
                        cart[cartCount++] = new CartItem { Product = selected, Quantity = qty };
                        selected.DeductStock(qty);
                        Console.WriteLine("Added to cart!");
                    }
                    else
                    {
                        Console.WriteLine("Not enough stock.");
                    }
                }

                else if (menu == 2)
                {
                    Console.WriteLine("\n=== CART ===");

                    if (cartCount == 0)
                    {
                        Console.WriteLine("Cart is empty.");
                        continue;
                    }

                    for (int i = 0; i < cartCount; i++)
                        Console.WriteLine($"{i + 1}. {cart[i].Product.Name} x{cart[i].Quantity}");
                }

                else if (menu == 3)
                {
                    double total = 0;

                    for (int i = 0; i < cartCount; i++)
                        total += cart[i].GetTotal();

                    double discount = total >= 5000 ? total * 0.10 : 0;
                    double final = total - discount;

                    Console.WriteLine("Total: ₱" + final);

                    history[historyCount++] = new Order
                    {
                        ReceiptNo = receiptNo++,
                        Total = final
                    };

                    cartCount = 0;
                }

                else if (menu == 4)
                {
                    Console.WriteLine("\n=== ORDER HISTORY ===");

                    for (int i = 0; i < historyCount; i++)
                        Console.WriteLine("Receipt #" + history[i].ReceiptNo + " - ₱" + history[i].Total);
                }

                else if (menu == 5)
                {
                    break;
                }
            }
        }
    }
}
