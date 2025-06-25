using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class WebAppUser
    {
        private int userId;
        private int roleId;
        private string userLogin;
        private string userPassword;

        public WebAppUser(int userId, int roleId, string userLogin, string userPassword)
        {
            this.userId = userId;
            this.roleId = roleId;
            this.userLogin = userLogin;
            this.userPassword = userPassword;
        }

        public int UserId { get => userId; set => userId = value; }
        public int RoleId { get => roleId; set => roleId = value; }
        public string UserLogin { get => userLogin; set => userLogin = value; }
        public string UserPassword { get => userPassword; set => userPassword = value; }
    }
}
