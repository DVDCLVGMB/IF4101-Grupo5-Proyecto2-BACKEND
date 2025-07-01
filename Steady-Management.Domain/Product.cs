using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Product
    {
        private int productId;
        private int categoryId;
        private string productName;
        private decimal price;
        private bool isTaxable;

        public Product(int productId, int categoryId, string productName, decimal price, bool isTaxable)
        {
            this.productId = productId;
            this.categoryId = categoryId;
            this.productName = productName;
            this.price = price;
            this.isTaxable = isTaxable;
        }

        public int ProductId { get => productId; set => productId = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public decimal Price { get => price; set => price = value; }
        public bool IsTaxable { get => isTaxable; set => isTaxable = value; }
    }
}
