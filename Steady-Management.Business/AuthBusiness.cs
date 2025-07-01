using System;
using System.Collections.Generic;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    /// <summary>
    /// Lógica de autenticación: inicio y cierre de sesión.
    /// </summary>
    public class AuthBusiness
    {
        private readonly UserData _userData;

        // Ejemplo simple de “sesiones” en memoria (thread‑safe)
        // ➜ Reemplázalo por JWT, Redis, BD, etc. cuando lo necesites.
        private static readonly HashSet<int> _activeUsers = new HashSet<int>();
        private static readonly object _lock = new object();

        public AuthBusiness(string connectionString)
        {
            _userData = new UserData(connectionString);
        }

        /// <summary>
        /// Valida credenciales.
        /// Devuelve un WebAppUser si son correctas; null si no coinciden.
        /// </summary>
        public WebAppUser? ValidateCredentials(string username, string password)
        {
            try
            {
                WebAppUser? user = _userData.GetUserByCredentials(username, password);

                if (user != null)
                {
                    // Marcar sesión como activa
                    lock (_lock)
                    {
                        _activeUsers.Add(user.UserId);
                    }
                }

                return user;   // null ⇒ usuario o contraseña incorrectos
            }
            catch (Exception)
            {
                // Aquí puedes loggear la excepción. Re‑lanzar según política.
                throw;
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario.
        /// </summary>
        public void Logout(int userId)
        {
            // Quitar de la lista de usuarios activos
            lock (_lock)
            {
                _activeUsers.Remove(userId);
            }

            // (Opcional) registrar la fecha de logout en la BD
            // _userData.UpdateLastLogout(userId);   // si implementas ese método
        }

        /// <summary>
        /// (Opcional) Comprueba si un usuario tiene sesión activa.
        /// </summary>
        public bool IsLoggedIn(int userId)
        {
            lock (_lock)
            {
                return _activeUsers.Contains(userId);
            }
        }
    }
}



