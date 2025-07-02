using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteadyManagement.Domain
{
    public class Parameter
    {
        private string nombre_empresa;
        private string telefono_empresa;
        private string correo_empresa;
        private string tipo_pago;
        private string cedula_juridica;
        private int numero_factura;

        public Parameter()
        {
        }

        public Parameter(string nombre_empresa, string telefono_empresa, string correo_empresa,
                         string tipo_pago, string cedula_juridica)
        {
            this.nombre_empresa = nombre_empresa;
            this.telefono_empresa = telefono_empresa;
            this.correo_empresa = correo_empresa;
            this.tipo_pago = tipo_pago;
            this.cedula_juridica = cedula_juridica;
        }

        // Constructor actualizado para incluir NumeroFactura
        public Parameter(int numero_factura, string nombre_empresa, string telefono_empresa, string correo_empresa,
                         string tipo_pago, string cedula_juridica)
        {
            this.numero_factura = numero_factura; // Inicializamos el nuevo campo
            this.nombre_empresa = nombre_empresa;
            this.telefono_empresa = telefono_empresa;
            this.correo_empresa = correo_empresa;
            this.tipo_pago = tipo_pago;
            this.cedula_juridica = cedula_juridica;
        }
        public string NombreEmpresa { get => nombre_empresa; set => nombre_empresa = value; }
        public string TelefonoEmpresa { get => telefono_empresa; set => telefono_empresa = value; }
        public string CorreoEmpresa { get => correo_empresa; set => correo_empresa = value; }
        public string TipoPago { get => tipo_pago; set => tipo_pago = value; }
        public string CedulaJuridica { get => cedula_juridica; set => cedula_juridica = value; }
        public int NumeroFactura { get => numero_factura; set => numero_factura = value; }
    }
}
