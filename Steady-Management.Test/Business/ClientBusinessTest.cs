using Microsoft.Data.SqlClient;
using SteadyManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steady_Management.Business;


namespace Steady_Management.Test.Business
{

    [TestFixture]
    public class ClientBusinessTest
    {
        private string connectionString;
        private ClientBusiness clientBusiness;

        [SetUp]
        public void Setup()
        {
            connectionString =
                            "Data Source=163.178.173.130;Initial Catalog=PedidosSteadyManagement;Persist Security Info=True;User ID=Lenguajes;Password=lenguajesparaiso2025;Trust Server Certificate=True;";


            clientBusiness = new ClientBusiness(connectionString);

            // Insertar ciudad si no existe
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var cmd = new SqlCommand(@"
            IF NOT EXISTS (SELECT 1 FROM City WHERE city_name = 'TestCity')
            BEGIN
                INSERT INTO City (city_name, province, country)
                VALUES ('TestCity', 1, 'Costa Rica');
            END", connection);
            cmd.ExecuteNonQuery();
        }

        private int GetTestCityId()
        {
            using var cn = new SqlConnection(connectionString);
            cn.Open();
            using var cmd = new SqlCommand("SELECT TOP 1 city_id FROM City WHERE city_name = 'TestCity'", cn);
            return (int)cmd.ExecuteScalar();
        }

        [Test]
        public void CreateClient_WithMissingName_ThrowsException()
        {
            var client = new Client
            {
                CityId = GetTestCityId(),
                CompanyName = "", //  inválido
                ContactName = "Mario",
                ContactSurname = "Cerdas",
                ClientPhoneNumber = "88887777",
                ClientAddress = "Av 5",
                ClientFaxNumber = "22223333",
                ClientPostalCode = "10101"
            };

            Assert.Throws<ArgumentException>(() => clientBusiness.Create(client));
        }

        [Test]
        public void CreateClient_OK()
        {
            var client = new Client
            {
                CityId = GetTestCityId(),
                CompanyName = "Empresa S.A.",
                ContactName = "Luis",
                ContactSurname = "Ramírez",
                ContactRank = "Vendedor", 
                ClientPhoneNumber = "88887777",
                ClientAddress = "Centro",
                ClientFaxNumber = "22445511",
                ClientPostalCode = "11001"
            };


            clientBusiness.Create(client);

            Assert.That(client.ClientId, Is.GreaterThan(0));
        }
    }

}



