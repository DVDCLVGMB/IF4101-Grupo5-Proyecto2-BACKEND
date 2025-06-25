using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Role
    {
        private int roleId;
        private string roleName;

        public Role(int roleId, string roleName)
        {
            this.roleId = roleId;
            this.roleName = roleName;
        }
        public int RoleId { get => roleId; set => roleId = value; }
        public string RoleName { get => roleName; set => roleName = value; }
    }
}
