using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Category
    {
        private int categoryId;
        private string description;

        public Category(int categoryId, string description)
        {
            this.categoryId = categoryId;
            this.description = description;
        }

        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string Description { get => description; set => description = value; }
    }
}
