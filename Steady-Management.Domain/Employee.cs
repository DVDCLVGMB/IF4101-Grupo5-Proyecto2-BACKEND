using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Employee
    {
        private int employeeId;
        private int deptId;
        private int roleId;
        private string extension;
        private string employeeName;
        private string employeeSurname;
        private string workPhoneNumber;

        public Employee(int employeeId, int deptId, int roleId, string extension, string employeeName, string employeeSurname, string workPhoneNumber)
        {
            this.employeeId = employeeId;
            this.deptId = deptId;
            this.roleId = roleId;
            this.extension = extension;
            this.employeeName = employeeName;
            this.employeeSurname = employeeSurname;
            this.workPhoneNumber = workPhoneNumber;
        }

        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public int DeptId { get => deptId; set => deptId = value; }
        public int RoleId { get => roleId; set => roleId = value; }
        public string Extension { get => extension; set => extension = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
        public string EmployeeSurname { get => employeeSurname; set => employeeSurname = value; }
        public string WorkPhoneNumber { get => workPhoneNumber; set => workPhoneNumber = value; }
    }
}
