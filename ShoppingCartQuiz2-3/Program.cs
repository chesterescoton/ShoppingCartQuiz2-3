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
        }
}
