using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Department
    {
        private int deptId;
        private string deptName;

        public Department() { }
        public Department(int deptId, string deptName)
        {
            this.deptId = deptId;
            this.deptName = deptName;
        }

        public int DeptId { get => deptId; set => deptId = value; }
        public string DeptName { get => deptName; set => deptName = value; }
    }
}
