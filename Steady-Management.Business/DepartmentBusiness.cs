using System;
using System.Collections.Generic;
using Steady_Management.DataAccess;   // DepartmentData
using Steady_Management.Domain;       // Department

namespace SteadyManagement.Business
{
    public class DepartmentBusiness
    {
        private readonly DepartmentData _data;

        /// <summary>
        /// Recibe el connection-string de la BD y crea el data-layer.
        /// </summary>
        public DepartmentBusiness(string conn) => _data = new DepartmentData(conn);

        // -------------------  CREATE  -------------------
        public Department AddDepartment(Department d)
        {
            Validate(d);
            _data.Create(d);
            return d;                         // Devuelve el objeto con el ID ya asignado
        }

        // -------------------  READ ALL  -----------------
        public List<Department> GetAll() => _data.GetAll();

        // -------------------  READ BY ID  ---------------
        public Department? GetById(int id) => _data.GetById(id);

        // -------------------  UPDATE  -------------------
        public bool Update(Department d)
        {
            Validate(d);
            return _data.Update(d);
        }

        // -------------------  DELETE  -------------------
        public bool Delete(int id) => _data.Delete(id);

        // -------------------  RULES / VALIDATIONS -------        
        private static void Validate(Department d)
        {
            if (string.IsNullOrWhiteSpace(d.DeptName))
                throw new ArgumentException("El nombre del departamento no puede estar vacío.");

            if (d.DeptName.Length > 25)       // coincide con el VARCHAR(25) de la tabla
                throw new ArgumentException("El nombre del departamento no debe exceder 25 caracteres.");
        }
    }
}
