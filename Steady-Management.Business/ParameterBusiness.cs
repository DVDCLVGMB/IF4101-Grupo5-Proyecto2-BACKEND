using Steady_Management.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteadyManagement.Domain;

namespace Steady_Management.Business
{
    public class ParameterBusiness
    {
        private readonly ParameterData _parameterData;

        public ParameterBusiness(string connectionString)
        {
            _parameterData = new ParameterData(connectionString);
        }

        public Parameter GetParameter() => _parameterData.GetParameter();

        public void UpdateParameter(Parameter parameterToUpdate)
        {
            try
            {
                _parameterData.UpdateParameter(parameterToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la capa de negocio al actualizar el parámetro: {ex.Message}");
                throw;
            }
        }
    }
}