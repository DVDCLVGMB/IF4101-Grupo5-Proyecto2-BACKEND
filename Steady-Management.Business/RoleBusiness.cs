using System.Collections.Generic;
using System.Threading.Tasks;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class RoleBusiness
    {
        private readonly RoleData _roleData;

        public RoleBusiness(RoleData roleData)
        {
            _roleData = roleData;
        }

        public Task<IEnumerable<Role>> GetAllRolesAsync() => _roleData.GetAllRolesAsync();

        public Task<Role> GetRoleByIdAsync(int id) => _roleData.GetRoleByIdAsync(id);

        public Task<int> CreateRoleAsync(Role role) => _roleData.CreateRoleAsync(role);

        public Task UpdateRoleAsync(Role role) => _roleData.UpdateRoleAsync(role);

        public Task DeleteRoleAsync(int id) => _roleData.DeleteRoleAsync(id);
    }
}
