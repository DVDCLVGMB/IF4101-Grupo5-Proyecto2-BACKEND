using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steady_Management.DataAccess;
using SteadyManagement.Domain;

namespace SteadyManagement.Business
{
    public class ClientBusiness
    {
        private readonly ClientData _data;
        public ClientBusiness(string conn) => _data = new ClientData(conn);

        public Client AddClient(Client c) { _data.Create(c); return c; }
        public List<Client> GetAll() => _data.GetAll();
        public Client? GetById(int id) => _data.GetById(id);
        public bool Update(Client c) => _data.Update(c);
        public bool Delete(int id) => _data.Delete(id);
    }
}
