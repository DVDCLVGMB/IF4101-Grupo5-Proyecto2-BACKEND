using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class EmployeeBusiness
    {
        private readonly EmployeeData _employeeData;

        public EmployeeBusiness(EmployeeData employeeData)
        {
            _employeeData = employeeData;
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync() => _employeeData.GetAllEmployeesAsync();

        public Task<Employee> GetEmployeeByIdAsync(int id) => _employeeData.GetEmployeeByIdAsync(id);

        public Task<int> CreateEmployeeAsync(Employee employee) => _employeeData.CreateEmployeeAsync(employee);

        public Task UpdateEmployeeAsync(Employee employee) => _employeeData.UpdateEmployeeAsync(employee);

        public Task DeleteEmployeeAsync(int id) => _employeeData.DeleteEmployeeAsync(id);
    }
}
