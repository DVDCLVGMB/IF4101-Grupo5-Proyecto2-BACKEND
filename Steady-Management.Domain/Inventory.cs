using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Inventory
    {
        private int productId;
        private string size;
        private int itemQuantity;
        private int limitUntilRestock;

        public Inventory(int productId, string size, int itemQuantity, int limitUntilRestock)
        {
            this.productId = productId;
            this.size = size;
            this.itemQuantity = itemQuantity;
            this.limitUntilRestock = limitUntilRestock;
        }

        public int ProductId { get => productId; set => productId = value; }
        public string Size { get => size; set => size = value; }
        public int ItemQuantity { get => itemQuantity; set => itemQuantity = value; }
        public int LimitUntilRestock { get => limitUntilRestock; set => limitUntilRestock = value; }
    }
}
