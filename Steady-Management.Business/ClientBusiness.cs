using System;
using System.Collections.Generic;
using Steady_Management.DataAccess;
using SteadyManagement.Domain;

namespace Steady_Management.Business
{
    public class ClientBusiness
    {
        private readonly ClientData clientData;

        public ClientBusiness(string connectionString)
        {
            clientData = new ClientData(connectionString);
        }

        // CREATE
        public void Create(Client client)
        {
            ValidateClient(client);
            clientData.Create(client);
        }

        // READ
        public List<Client> GetAll()
        {
            return clientData.GetAll();
        }

        public Client? GetById(int id)
        {
            return clientData.GetById(id);
        }

        // UPDATE
        public bool Update(Client client)
        {
            ValidateClient(client);
            return clientData.Update(client);
        }

        // DELETE
        public bool Delete(int id)
        {
            return clientData.Delete(id);
        }

        // VALIDACIONES
        private void ValidateClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.CompanyName))
                throw new ArgumentException("El nombre de la compañía es obligatorio.");
            if (string.IsNullOrWhiteSpace(client.ContactName))
                throw new ArgumentException("El nombre del contacto es obligatorio.");
            if (string.IsNullOrWhiteSpace(client.ClientPhoneNumber))
                throw new ArgumentException("Debe ingresar el número de teléfono.");
            if (client.CityId <= 0)
                throw new ArgumentException("Debe asignarse una ciudad válida.");
        }
    }
}
