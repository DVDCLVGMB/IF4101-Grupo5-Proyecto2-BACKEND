// Steady_Management.Business/CityBusiness.cs
using Steady_Management.DataAccess;
using SteadyManagement.Domain;
using System.Collections.Generic;

namespace Steady_Management.Business
{
    public class CityBusiness
    {
        private readonly CityData _data;
        public CityBusiness(string cs) => _data = new CityData(cs);

        // Nuevo Create que devuelve el City completo
        public City Create(City city)
        {
            // Inserta y obtiene el nuevo ID
            int newId = _data.Create(city);
            // Asigna al objeto
            city.CityId = newId;
            return city;
        }

        public List<City> GetAll() => _data.GetAll();
        public City GetById(int id) => _data.GetById(id);
        public bool Update(City city) => _data.Update(city);
        public bool Delete(int id) => _data.Delete(id);
    }
}
