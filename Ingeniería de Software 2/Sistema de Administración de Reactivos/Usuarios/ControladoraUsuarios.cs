using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SAR.App_Code.Usuarios {
    public class ControladoraUsuarios {
        ControladoraBDUsuarios controladoraBDUsuario;

        /**
         * Constructor.
         */
        public ControladoraUsuarios() {
            controladoraBDUsuario = new ControladoraBDUsuarios();
        }

        /**
         * Inserta un usuario en el sistema.
         * 
         * @param Object [] datos : vector con los datos de nuevo usuario.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] insertarUsuario(Object[] datos) {
            EntidadUsuario usuario = new EntidadUsuario(datos);
            return controladoraBDUsuario.insertarUsuario(usuario);
        }

        /**
         * Modificar un usuario existente en el sistema.
         * 
         * @param Object datosNuevos : vector que contiene los nuevos datos del usuario.
         * @param EntidadUsuario usuarioViejo : datos viejos del usuario.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] modificarUsuario(Object[] datosNuevos, EntidadUsuario usuarioViejo) {
            EntidadUsuario usuarioNuevo = new EntidadUsuario(datosNuevos);
            return controladoraBDUsuario.modificarUsuario(usuarioNuevo, usuarioViejo);
        }

        /**
         * Elimina un usuario del sistema.
         * 
         * @parama EntidadUsuario eliminado : entidad del usuario que se va a eliminar.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación. 
         */
        public String[] eliminarUsuario(EntidadUsuario eliminado) {
            return controladoraBDUsuario.eliminarUsuario(eliminado);
        }

        /**
         * Lista los datos de todos los usuarios en el sistema. 
         */
        public DataTable consultarUsuarios() {
            return controladoraBDUsuario.consultarUsuarios();
        }

        /**
         * Consulta los datos de un usuario específico en el sistema.
         * 
         * @param String username : el nombre del usuario a consultar.
         * @return EntidadUsuario : la entidad con los datos del usuario consultado.
         */
        public EntidadUsuario consultarUsuario(String username) {
            EntidadUsuario usuarioConsultado = null;
            Object[] datosConsultados = new Object[8];
            datosConsultados[7] = "";

            DataTable filaUsuario = controladoraBDUsuario.consultarUsuario(username); // pide la fila a la contBD

            if (filaUsuario.Rows.Count == 1) { // si hay un valor
                for (int i = 0; i < 7; i++) { // obtiene los atributos
                    datosConsultados[i] = filaUsuario.Rows[0][i].ToString();
                }
                usuarioConsultado = new EntidadUsuario(datosConsultados);
            }

            return usuarioConsultado;
        }

        /**
         * Verifica los credenciales de autenticación dados.
         * 
         * @param String username : nombre de usuario ingresado.
         * @param String password : contraseña ingresada.
         * @return String [] : mensaje indicando la correctitud o el fallo de la operación.
         */
        public String[] logIn(String username, String password) {
            return controladoraBDUsuario.logIn(username, password);
        }

        /**
         * Cierra la sesión de un usuario.
         */
        public void logOff() {
            
        }

        /**
         * Obtiene los permisos que posee un determinado rol.
         * 
         * @param String rol : el nombre del rol al que se le van a obtener los permisos.
         * @return String : los permisos del rol dado.
         */
        public String obtenerPermisosRol(String rol) {
            return controladoraBDUsuario.obtenerPermisos(rol);
        }

        /**
         * Obtiene el rol de un usuario dado.
         * 
         * @param String username : nombre de usuario al que se le va a obtener el rol.
         * @return String : el nombre del rol al que pertenece el usuario.
         */
        public String obtenerRolUsuario(String username) {
            return controladoraBDUsuario.obtenerRolUsuario(username);
        }
    }
}