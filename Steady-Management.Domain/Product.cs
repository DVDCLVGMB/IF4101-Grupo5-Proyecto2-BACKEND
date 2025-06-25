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
        private float price;

        public Product(int productId, int categoryId, string productName, float price)
        {
            this.productId = productId;
            this.categoryId = categoryId;
            this.productName = productName;
            this.price = price;
        }

        public int ProductId { get => productId; set => productId = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public float Price { get => price; set => price = value; }
    }
}
