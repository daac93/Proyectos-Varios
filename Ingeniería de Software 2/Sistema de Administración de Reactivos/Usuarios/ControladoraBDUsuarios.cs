using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SAR.App_Code.DataSets.UsuariosDataSetsTableAdapters;
using Microsoft.Owin.Security;

namespace SAR.App_Code.Usuarios {
    public class ControladoraBDUsuarios {

        private UsuariosTableAdapter adapterUsuarios;

        /**
         * Constructor.
         */
        public ControladoraBDUsuarios() {
            adapterUsuarios = new UsuariosTableAdapter();
        }

        /**
         * Inserta un usuario a la base de datos, en la tabla "Usuarios"
         * 
         * @param EntidadUsuario nuevoUsuario : la entidad con los datos del nuevo usuario.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] insertarUsuarioBD(EntidadUsuario nuevoUsuario) {
            String [] resultado = new String[3];
            try {
                this.adapterUsuarios.Insert(
                                            nuevoUsuario.Username,
                                            nuevoUsuario.Rol,
                                            nuevoUsuario.Nombre,
                                            nuevoUsuario.Apellido1,
                                            nuevoUsuario.Apellido2,
                                            nuevoUsuario.Telefono,
                                            nuevoUsuario.Email);

                resultado[0] = "success";
                resultado[1] = "Éxito. ";
                resultado[2] = "El usuario se ha ingresado exitosamente.";

            } catch (SqlException e) {
                int r = e.Number;

                resultado[0] = "danger";
                resultado[1] = "Error. ";
                resultado[2] = "No se pudo agregar el usuario.";

            }

            return resultado;
        }

        /**
         * Inserta un usuario en la base de datos.  Tanto en la tabla "Usuarios" como en la tabla "AspNetUsers"
         * del componente "Identity" de .NET.
         * 
         * @param EntidadUsuario nuevoUsuario : la entidad con los datos del nuevo usuario.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] insertarUsuario(EntidadUsuario nuevoUsuario) {
            //Constructor por defecto que va a usar la DefaultConnectionString
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            String [] error = new String[3];

            var user = new IdentityUser() { UserName = nuevoUsuario.Username };
            IdentityResult resultado = manager.Create(user, nuevoUsuario.Password);

            if (resultado.Succeeded) {
                //Se creó correctamente y lo agrego a la tabla de usarios y lo agrego al rol.
                manager.AddToRole(manager.FindByName(nuevoUsuario.Username).Id, nuevoUsuario.Rol);
                return this.insertarUsuarioBD(nuevoUsuario);
            } else {
                

                error[0] = "danger";
                error[1] = "Error. ";
                if(resultado.Errors.FirstOrDefault().Contains("taken"))  {
                    error[2] = "El nombre de usuario ya está siendo utilizado.";
                }   else  {
                    error[2] = resultado.Errors.FirstOrDefault();
                }
            }

            return error;
        }

        /**
         * Modifica un usuario existente tanto en la tabla "Usuarios" como en las tablas relacionadas con "Identity".
         * 
         * @param EntidadUsuario usuarioNuevo : la entidad con los nuevos datos del usuario.
         * @param EntidadUsuario usuarioViejo : la entidad con los datos viejos del usuario.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] modificarUsuario(EntidadUsuario usuarioNuevo, EntidadUsuario usuarioViejo) {
            //Constructor por defecto que va a usar la DefaultConnectionString
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);
            IdentityResult resultado = null;
            String[] error = new String[3];

            
            if (usuarioNuevo.Password != "") {
                resultado = manager.RemovePassword(manager.FindByName(usuarioViejo.Username).Id);
                if (resultado.Succeeded) {
                    resultado = manager.AddPassword(manager.FindByName(usuarioViejo.Username).Id, usuarioNuevo.Password);
                }
            }

            if (usuarioNuevo.Rol != usuarioViejo.Rol) {
                resultado = manager.RemoveFromRole(manager.FindByName(usuarioViejo.Username).Id, usuarioViejo.Rol);

                if (resultado.Succeeded) {
                    resultado = manager.AddToRole(manager.FindByName(usuarioViejo.Username).Id, usuarioNuevo.Rol);
                }
            }

            if (resultado != null && resultado.Succeeded) {
                return modificarUsuarioBD(usuarioNuevo, usuarioViejo);
            } else {
                error[0] = "danger";
                error[1] = "Error. ";
                error[2] = resultado.Errors.FirstOrDefault();
            }

            return error;
        }

        /**
         * Modifica los datos de un usuario existente en la tabla "Usuarios".
         * 
         * @param EntidadUsuario usuarioNuevo : la entidad con los nuevos datos del usuario.
         * @param EntidadUsuario usuarioViejo : la entidad con los datos viejos del usuario.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] modificarUsuarioBD(EntidadUsuario usuarioNuevo, EntidadUsuario usuarioViejo) {
            String [] resultado = new String[3];

            try {
                this.adapterUsuarios.Update(usuarioNuevo.Username, usuarioNuevo.Rol, usuarioNuevo.Nombre, usuarioNuevo.Apellido1, usuarioNuevo.Apellido2,
                                              usuarioNuevo.Telefono, usuarioNuevo.Email,
                                              usuarioViejo.Username, usuarioViejo.Rol, usuarioViejo.Nombre, usuarioViejo.Apellido1, usuarioViejo.Apellido2,
                                              usuarioViejo.Telefono, usuarioViejo.Email);

                resultado[0] = "success";
                resultado[1] = "Éxito. ";
                resultado[2] = "El usuario se ha modificado exitosamente.";

            } catch (SqlException e) {
                resultado[0] = "danger";
                resultado[1] = "Error. ";
                resultado[2] = "No se pudo modificar el proveedor";
            }

            return resultado;
        }

        /**
         * Elimina un usuario tanto de la tabla "Usuarios" como de las tablas relacionadas con "Identity".
         * 
         * @param EntidadUsuario usuarioABorrar : entidad con todos los datos del usuario que se va a borrar.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] eliminarUsuario(EntidadUsuario usuarioABorrar) {
            String[] error = new String[3];

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            IdentityResult resultado = manager.Delete(manager.FindByName(usuarioABorrar.Username));
            
            if (resultado.Succeeded) {
                try {
                    this.adapterUsuarios.Delete(usuarioABorrar.Username, usuarioABorrar.Rol, usuarioABorrar.Nombre, usuarioABorrar.Apellido1, usuarioABorrar.Apellido2,
                                                  usuarioABorrar.Telefono, usuarioABorrar.Email);

                    error[0] = "success";
                    error[1] = "Exito. ";
                    error[2] = "Se eliminó exitosamente el usuario.";
                } catch (SqlException) {
                    error[0] = "danger";
                    error[1] = "Error. ";
                    error[2] = "No se pudo eliminar el usuario.";
                }
            } else {
                error[0] = "danger";
                error[1] = "Error. ";
                error[2] = resultado.Errors.FirstOrDefault();
            }

            return error;
        }

        /**
         * Lista todos los usuarios presentes en la base de datos.
         * 
         * @return DataTable : datos de todos los usuarios presentes en la base de datos.
         */
        public DataTable consultarUsuarios() {
            DataTable resultado = new DataTable();
            resultado = adapterUsuarios.GetData();
            return resultado;
        }

        /**
         * Consulta un usuario específico en la base de datos.
         * 
         * @parama String nombreUsuario : el nombre de usuario a consultar.
         * @return DataTable : datos de usuario consultado.
         */
        public DataTable consultarUsuario(String nombreUsuario) {
            DataTable resultado = new DataTable();

            resultado = adapterUsuarios.consultarUsuario(nombreUsuario);

            return resultado;
        }

        /**
         * Utiliza la interfas de "Identity" para verificar que los datos de login sean correctos.
         * 
         * @param String username : nombre de usuario ingresado.
         * @param String password : contraseña ingresada.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] logIn(String username, String password) {
            String[] error = new String[3];
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            try {
                var user = userManager.Find(username, password);

                if (user != null) {
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                    error[0] = "success";
                    error[1] = "Exito. ";
                    error[2] = "Se logueo exitosamente el usuario.";
                } else {
                    error[0] = "warning";
                    error[1] = "Alerta. ";
                    error[2] = "Nombre de usuario y/o contraseña incorrectos.";
                }

                
            } catch (Exception e) {
                error[0] = "warning";
                error[1] = "Alerta. ";
                error[2] = "No hay conexión con la base de datos.\nInténtelo más tarde.";
            }

            return error;
        }

        /**
         * Obtiene los permisos que posee un determinado rol.
         * 
         * @param String rol : el nombre del rol al que se le van a obtener los permisos.
         * @return String : los permisos del rol dado.
         */
        public String obtenerPermisos(String rol) {
            try {
                PermisosTableAdapter adapterPermisos = new PermisosTableAdapter();

                DataTable resultado = adapterPermisos.consultarPermisosRol(rol);

                return resultado.Rows[0][1].ToString();
            } catch (Exception e) {

            }
            
            return null;
        }

        /**
         * Obtiene el rol de un usuario dado.
         * 
         * @param String username : nombre de usuario al que se le va a obtener el rol.
         * @return String : el nombre del rol al que pertenece el usuario.
         */
        public String obtenerRolUsuario(String username) {
            DataTable resultado = new DataTable();

            try {
                resultado = adapterUsuarios.consultarUsuario(username);
            } catch (Exception e) {
                return null;
            }
            
            return resultado.Rows[0][1].ToString();
        }
    }
}