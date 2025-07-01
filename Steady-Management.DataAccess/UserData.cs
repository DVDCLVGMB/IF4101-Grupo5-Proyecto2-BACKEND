using System;
using System.Configuration;          // Para leer la cadena de conexión
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

namespace Steady_Management.DataAccess
{
    public class UserData
    {
        // Lee la cadena de conexión desde appsettings.json o app.config
        private readonly string _connectionString;

        public UserData(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Devuelve un WebAppUser si las credenciales coinciden; null en caso contrario.
        /// Lanza excepción si ocurre un error de base de datos.
        /// </summary>
        public WebAppUser? GetUserByCredentials(string username, string password)
        {
            WebAppUser? user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Para esta consulta de solo lectura la transacción no es estrictamente necesaria,
                // pero la incluimos porque lo solicitaste y para mantener un patrón homogéneo.
                using (SqlTransaction tx = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("dbo.usp_WebAppUser_GetByCredentials", conn, tx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@p_user_login", SqlDbType.VarChar, 25).Value = username;
                            cmd.Parameters.Add("@p_user_password", SqlDbType.VarChar, 25).Value = password;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    user = new WebAppUser(
                                        userId: dr.GetInt32(dr.GetOrdinal("user_id")),
                                        roleId: dr.GetInt32(dr.GetOrdinal("role_id")),
                                        userLogin: dr.GetString(dr.GetOrdinal("user_login")),
                                        userPassword: dr.GetString(dr.GetOrdinal("user_password"))
                                    );
                                }
                            }
                        }

                        tx.Commit();   // Éxito
                    }
                    catch (SqlException ex)
                    {
                        tx.Rollback();
                        // Re‑lanzamos la excepción con contexto. 
                        // En UI/servicio puedes capturarla y registrar/loggear.
                        throw new Exception("Error al obtener el usuario por credenciales.", ex);
                    }
                }
            }

            return user;   // null si no existe usuario
        }
    }
}
