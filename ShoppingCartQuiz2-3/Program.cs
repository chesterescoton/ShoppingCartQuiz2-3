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
        public string Category;
        public double Price;
        public int RemainingStock;

        public void DisplayProduct()
        {
            Console.WriteLine(Id + ". " + Name + " (" + Category + ") - ₱" + Price + " (Stock: " + RemainingStock + ")");
        }
    }

    class Order
    {
        public int ReceiptNumber;
        public double FinalTotal;
        public DateTime Date;
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Product[] products = {
                new Product { Id = 1, Name = "Laptop", Category="Electronics", Price = 30000, RemainingStock = 5 },
                new Product { Id = 2, Name = "Mouse", Category="Accessories", Price = 500, RemainingStock = 10 },
                new Product { Id = 3, Name = "Keyboard", Category="Accessories", Price = 1000, RemainingStock = 7 },
                new Product { Id = 4, Name = "Monitor", Category="Electronics", Price = 7000, RemainingStock = 4 },
                new Product { Id = 5, Name = "Headset", Category="Accessories", Price = 1500, RemainingStock = 6 },
                new Product { Id = 6, Name = "USB Flash Drive", Category="Accessories", Price = 300, RemainingStock = 15 },
                new Product { Id = 7, Name = "Webcam", Category="Electronics", Price = 1200, RemainingStock = 5 }
            };

            int[] cartQty = new int[products.Length];
            double[] cartTotal = new double[products.Length];

            List<Order> history = new List<Order>();
            int receiptCounter = 1;

            while (true)
            {
                Console.WriteLine("\n=== STORE MENU ===");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Search Product");
                Console.WriteLine("3. Filter by Category");
                Console.WriteLine("4. Manage Cart");
                Console.WriteLine("5. View Order History");
                Console.WriteLine("6. Exit");

                int menu;
                Console.Write("Choose: ");
                if (!int.TryParse(Console.ReadLine(), out menu))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                if (menu == 1)
                {
                    foreach (var p in products)
                        p.DisplayProduct();

                    AddToCart(products, cartQty, cartTotal);
                }
                else if (menu == 2)
                {
                    Console.Write("Search: ");
                    string search = Console.ReadLine().ToLower();

                    foreach (var p in products)
                        if (p.Name.ToLower().Contains(search))
                            p.DisplayProduct();
                }
                else if (menu == 3)
                {
                    Console.WriteLine("1. Electronics\n2. Accessories");
                    string cat = Console.ReadLine();

                    foreach (var p in products)
                    {
                        if ((cat == "1" && p.Category == "Electronics") ||
                            (cat == "2" && p.Category == "Accessories"))
                        {
                            p.DisplayProduct();
                        }
                    }
                }
                else if (menu == 4)
                {
                    CartMenu(products, cartQty, cartTotal, history, ref receiptCounter);
                }
                else if (menu == 5)
                {
                    Console.WriteLine("\n=== ORDER HISTORY ===");
                    foreach (var o in history)
                    {
                        Console.WriteLine("Receipt #" + o.ReceiptNumber +
                                          " | ₱" + o.FinalTotal +
                                          " | " + o.Date);
                    }
                }
                else if (menu == 6)
                {
                    break;
                }
            }
        }

        static void AddToCart(Product[] products, int[] cartQty, double[] cartTotal)
        {
            while (true)
            {
                int choice;
                Console.Write("Enter product #: ");
                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > products.Length)
                {
                    Console.WriteLine("Invalid product.");
                    continue;
                }

                int qty;
                Console.Write("Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                {
                    Console.WriteLine("Invalid quantity.");
                    continue;
                }

                if (products[choice - 1].RemainingStock < qty)
                {
                    Console.WriteLine("Not enough stock.");
                    continue;
                }

                cartQty[choice - 1] += qty;
                cartTotal[choice - 1] += products[choice - 1].Price * qty;
                products[choice - 1].RemainingStock -= qty;

                Console.WriteLine("Added to cart!");

                string ans;
                while (true)
                {
                    Console.Write("Add more? (Y/N): ");
                    ans = Console.ReadLine().ToUpper();
                    if (ans == "Y" || ans == "N") break;
                    Console.WriteLine("Invalid input.");
                }

                if (ans == "N") break;
            }
        }

        static void CartMenu(Product[] products, int[] cartQty, double[] cartTotal, List<Order> history, ref int receiptCounter)
        {
            while (true)
            {
                Console.WriteLine("\n=== CART MENU ===");
                Console.WriteLine("1. View Cart");
                Console.WriteLine("2. Remove Item");
                Console.WriteLine("3. Update Quantity");
                Console.WriteLine("4. Clear Cart");
                Console.WriteLine("5. Checkout");
                Console.WriteLine("6. Back");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                if (choice == 1)
                {
                    bool empty = true;
                    for (int i = 0; i < products.Length; i++)
                    {
                        if (cartQty[i] > 0)
                        {
                            Console.WriteLine(products[i].Name + " x" + cartQty[i] + " = ₱" + cartTotal[i]);
                            empty = false;
                        }
                    }
                    if (empty) Console.WriteLine("Cart is empty.");
                }
                else if (choice == 2)
                {
                    int i;
                    Console.Write("Enter product #: ");
                    if (!int.TryParse(Console.ReadLine(), out i) || i < 1 || i > products.Length)
                    {
                        Console.WriteLine("Invalid.");
                        continue;
                    }

                    i--;

                    products[i].RemainingStock += cartQty[i];

                    cartQty[i] = 0;
                    cartTotal[i] = 0;

                    Console.WriteLine("Item removed.");
                }
                else if (choice == 3)
                {
                    int i;
                    Console.Write("Enter product #: ");
                    if (!int.TryParse(Console.ReadLine(), out i) || i < 1 || i > products.Length)
                    {
                        Console.WriteLine("Invalid.");
                        continue;
                    }

                    i--;

                    int newQty;
                    Console.Write("New quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out newQty) || newQty < 0)
                    {
                        Console.WriteLine("Invalid.");
                        continue;
                    }

                    // restore old stock first
                    products[i].RemainingStock += cartQty[i];

                    if (products[i].RemainingStock < newQty)
                    {
                        Console.WriteLine("Not enough stock.");
                        // revert
                        products[i].RemainingStock -= cartQty[i];
                        continue;
                    }

                    cartQty[i] = newQty;
                    cartTotal[i] = products[i].Price * newQty;
                    products[i].RemainingStock -= newQty;

                    Console.WriteLine("Updated.");
                }
                else if (choice == 4)
                {
                    for (int i = 0; i < products.Length; i++)
                    {
                        products[i].RemainingStock += cartQty[i];
                        cartQty[i] = 0;
                        cartTotal[i] = 0;
                    }

                    Console.WriteLine("Cart cleared.");
                }
                else if (choice == 5)
                {
                    Checkout(products, cartQty, cartTotal, history, ref receiptCounter);
                    break;
                }
                else if (choice == 6)
                {
                    break;
                }
            }
        }

        static void Checkout(Product[] products, int[] cartQty, double[] cartTotal, List<Order> history, ref int receiptCounter)
        {
            double total = 0;
            bool empty = true;

            foreach (var q in cartQty)
                if (q > 0) empty = false;

            if (empty)
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            Console.WriteLine("\n=== RECEIPT ===");
            Console.WriteLine("Receipt No: " + receiptCounter);
            Console.WriteLine("Date: " + DateTime.Now);

            for (int i = 0; i < products.Length; i++)
            {
                if (cartQty[i] > 0)
                {
                    Console.WriteLine(products[i].Name + " x" + cartQty[i] + " = ₱" + cartTotal[i]);
                    total += cartTotal[i];
                }
            }

            double discount = total >= 5000 ? total * 0.10 : 0;
            double finalTotal = total - discount;

            Console.WriteLine("Grand Total: ₱" + total);
            Console.WriteLine("Discount: ₱" + discount);
            Console.WriteLine("Final Total: ₱" + finalTotal);

            double payment;
            while (true)
            {
                Console.Write("Enter payment: ");
                if (double.TryParse(Console.ReadLine(), out payment) && payment >= finalTotal)
                    break;
                Console.WriteLine("Invalid or insufficient payment.");
            }

            double change = payment - finalTotal;

            Console.WriteLine("Payment: ₱" + payment);
            Console.WriteLine("Change: ₱" + change);

            history.Add(new Order
            {
                ReceiptNumber = receiptCounter,
                FinalTotal = finalTotal,
                Date = DateTime.Now
            });

            receiptCounter++;

            bool hasLow = false;
            foreach (var p in products)
            {
                if (p.RemainingStock <= 5)
                {
                    if (!hasLow)
                    {
                        Console.WriteLine("\nLOW STOCK ALERT:");
                        hasLow = true;
                    }
                    Console.WriteLine(p.Name + " has only " + p.RemainingStock + " left.");
                }
            }

            for (int i = 0; i < cartQty.Length; i++)
            {
                cartQty[i] = 0;
                cartTotal[i] = 0;
            }
        }
    }
}
