using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string? Size { get; set; }
        public int ItemQuantity { get; set; }
        public int LimitUntilRestock { get; set; }

        public Inventory(int inventoryId,
                         int productId,
                         string? size,
                         int itemQuantity,
                         int limitUntilRestock)
        {
            InventoryId = inventoryId;
            ProductId = productId;
            Size = size;
            ItemQuantity = itemQuantity;
            LimitUntilRestock = limitUntilRestock;
        }
    }
}
