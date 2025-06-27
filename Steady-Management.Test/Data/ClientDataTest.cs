using Microsoft.Data.SqlClient;
using NUnit.Framework;
using Steady_Management.DataAccess;
using SteadyManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steady_Management.Test
{
    [TestFixture]
    public class ClientDataTest
    {
        private string connectionString = "";

        [SetUp]
        public void Setup()
        {
            connectionString =
            "Data Source=163.178.173.130;Initial Catalog=PedidosSteadyManagement;Persist Security Info=True;User ID=Lenguajes;Password=lenguajesparaiso2025;Trust Server Certificate=True;";

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            // Asegurar que exista una ciudad de prueba
            using var cmd = new SqlCommand(@"
                IF NOT EXISTS (SELECT 1 FROM City WHERE city_name = 'TestCity')
                BEGIN
                    INSERT INTO City (city_name, province, country)
                    VALUES ('TestCity', 1, 'Costa Rica');
                END
            ", connection);
            cmd.ExecuteNonQuery();
        }

        private int GetTestCityId()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var cmd = new SqlCommand("SELECT TOP 1 city_id FROM City WHERE city_name = 'TestCity'", connection);
            return (int)cmd.ExecuteScalar();
        }

        private void CleanDatabase()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var delete = new SqlCommand("DELETE FROM Client", connection);
            delete.ExecuteNonQuery();
        }

        private Client CreateSampleClient()
        {
            return new Client
            {
                CityId = GetTestCityId(),
                CompanyName = "TestCorp",
                ContactName = "Ana",
                ContactSurname = "Ramirez",
                ContactRank = "Gerente",
                ClientAddress = "Calle 5",
                ClientPhoneNumber = "88887777",
                ClientFaxNumber = "22224444",
                ClientPostalCode = "10101"
            };
        }

        [Test]
        public void CreateClient_OK()
        {
            CleanDatabase();
            var data = new ClientData(connectionString);
            var client = CreateSampleClient();

            data.Create(client);

            Assert.That(client.ClientId, Is.GreaterThan(0));
        }

        [Test]
        public void GetAllClients_OK()
        {
            CleanDatabase();
            var data = new ClientData(connectionString);
            var client = CreateSampleClient();
            data.Create(client);

            var result = data.GetAll();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().CompanyName, Is.EqualTo("TestCorp"));
        }

        [Test]
        public void UpdateClient_OK()
        {
            CleanDatabase();
            var data = new ClientData(connectionString);
            var client = CreateSampleClient();
            data.Create(client);

            client.ContactName = "Marco";
            client.ContactSurname = "Pérez";
            var updated = data.Update(client);

            var updatedClient = data.GetById(client.ClientId);

            Assert.That(updated, Is.True);
            Assert.That(updatedClient!.ContactName, Is.EqualTo("Marco"));
            Assert.That(updatedClient.ContactSurname, Is.EqualTo("Pérez"));
        }

        [Test]
        public void DeleteClient_OK()
        {
            CleanDatabase();
            var data = new ClientData(connectionString);
            var client = CreateSampleClient();
            data.Create(client);

            var deleted = data.Delete(client.ClientId);
            var shouldBeNull = data.GetById(client.ClientId);

            Assert.That(deleted, Is.True);
            Assert.That(shouldBeNull, Is.Null);
        }
    }
}
