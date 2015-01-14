using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SAR.App_Code.Usuarios;
using System.Data;

namespace SAR.Forms.Usuarios {
    public partial class Usuarios : System.Web.UI.Page {
        private static String[] roles = { "Asistente", "Técnico de Laboratorio", "Administrador" };
        private static Boolean seConsulto = false;

        private static ControladoraUsuarios controladora;
        private static int modo = 0;
        private static EntidadUsuario usuarioConsultado;
        private static String permisos = "1111";
        private static int resultadosPorPagina;

        protected void Page_Load(object sender, EventArgs e) {
            controladora = new ControladoraUsuarios();

            resultadosPorPagina = gridViewUsuarios.PageSize;
            llenarGrid();
            ocultarMensaje();

            if (!IsPostBack) {
                permisos = ((SiteMaster)Page.Master).getPermisos("USUARIOS");
                setComboRoles(roles);
                if (!seConsulto) {
                    modo = 0; 
                } else {
                    if (usuarioConsultado == null) {
                        mostrarMensaje("warning", "Alerta. ", "No se pudo consultar el usuario.");
                    } else {
                        setDatosConsultados();
                    }
                    
                    seConsulto = false;
                }    
            }

            irAModo();
        }

        /**
         * Llena el combobox de roles en la interfaz.
         */
        protected void setComboRoles(String[] datos) {
            comboRol.DataSource = datos;
            comboRol.DataBind();
        }

        /**
         * Activa y desactiva los botones dela interfaz según los permisos y la acción que se vaya a realizar.
         */
        protected void irAModo() {
            if (modo == 0) { // resetear
                botonAceptar.Disabled = true;
                botonCancelar.Disabled = true;
                botonModificar.Disabled = true;
                botonEliminar.Disabled = true;
                botonAgregar.Disabled = false;
                habilitarCampos(false);
            } else if (modo == 1) { //insertar
                botonAceptar.Disabled = false;
                botonCancelar.Disabled = false;
                botonModificar.Disabled = true;
                botonEliminar.Disabled = true;
                botonAgregar.Disabled = true;

            } else if (modo == 2) { //modificar
                botonAceptar.Disabled = false;
                botonCancelar.Disabled = false;
                botonModificar.Disabled = true;
                botonEliminar.Disabled = true;
                botonAgregar.Disabled = true;
            } else if (modo == 3) { // eliminar
                botonModificar.Disabled = false;
                botonEliminar.Disabled = false;
                botonAceptar.Disabled = true;
                botonCancelar.Disabled = false;
                botonAgregar.Disabled = false;
                habilitarCampos(false);
            } else if (modo == 4) { //consultar
                botonModificar.Disabled = false;
                botonEliminar.Disabled = false;
                botonAceptar.Disabled = true;
                botonCancelar.Disabled = false;
                botonAgregar.Disabled = false;
                habilitarCampos(false);
            }

            aplicarPermisos();
        }

        /**
         * Desactiva las funciones que el usuario no puede utilizar.
         */
        protected void aplicarPermisos() {
            char[] estados = permisos.ToCharArray();
            if (estados[0] == '0') {
                this.botonAgregar.Disabled = true;
            }
            if (estados[1] == '0') {
                this.botonModificar.Disabled = true;
            }
            if (estados[2] == '0') {
                this.botonEliminar.Disabled = true;
            }
            if (estados[3] == '0') {
                this.botonConsultar.Disabled = true;
            }
        }

        protected void habilitarCampos(Boolean habilitar) {
            this.textUsername.Disabled = !habilitar;
            this.textPassword.Disabled = !habilitar;
            this.textConfirmarPassword.Disabled = !habilitar;
            this.comboRol.Enabled = habilitar;
            this.textNombre.Disabled = !habilitar;
            this.textApellido1.Disabled = !habilitar;
            this.textApellido2.Disabled = !habilitar;
            this.textTelefono.Disabled = !habilitar;
            this.textEmail.Disabled = !habilitar;
        }

        /**
         * Evento que se dispara cuando el usuario hace clic en el botón "Insertar" de la interfaz.
         */
        protected void clickInsertar(object sender, EventArgs e) {
            usuarioConsultado = null;
            modo = 1;
            irAModo();
            limpiarCampos();
            habilitarCampos(true);

            //Reiniciar los validadores JS.
            textPassword.Attributes.Add("required", "required");
            textPassword.Attributes.Add("data-toogle", "validator");

            textConfirmarPassword.Attributes.Add("required", "required");
            textConfirmarPassword.Attributes.Add("data-match", "#ContenidoPrincipal_textPassword");
        }

        /**
         * Evento que se dispara cuando el usuario hace clic en el botón "Modificar" de la interfaz.
         */
        protected void clickModificar(object sender, EventArgs e) {
            modo = 2;
            habilitarCampos(true);
            /*Excepto:*/
            this.textUsername.Disabled = true;
            //this.textConfirmarPassword.Disabled = true;

            textPassword.Attributes.Remove("required");
            textPassword.Attributes.Remove("data-toogle");

            textConfirmarPassword.Attributes.Remove("required");
            textConfirmarPassword.Attributes.Remove("data-match");

            irAModo();
        }

        /**
         * Evento que se dispara cuando el usuario hace clic en el botón "Acpetar" del modal de confirmación para eliminar de la interfaz.
         */
        protected void clickAceptarEliminar(object sender, EventArgs e) {
            textPassword.Attributes.Remove("required");
            textPassword.Attributes.Remove("data-toogle");

            textConfirmarPassword.Attributes.Remove("required");
            textConfirmarPassword.Attributes.Remove("data-toogle");

            if (!(HttpContext.Current.User.Identity.Name.Equals(usuarioConsultado.Username)) && !(HttpContext.Current.User.Identity.Name.Equals("admin"))) {
                String[] error = controladora.eliminarUsuario(usuarioConsultado);
                mostrarMensaje(error[0], error[1], error[2]);
                if (error[0].Contains("success")) {
                    modo = 0;
                    usuarioConsultado = null;
                    irAModo();
                    llenarGrid();
                    limpiarCampos();
                }
            } else {
                mostrarMensaje("warning", "Alerta.", "No es posible eliminar el usuario con el que se encuentra conectado.");
            }
        }

        /**
         * Evento que se dispara cuando el usuario hace clic en "Aceptar".
         * Su comportamiento varía según el modo en el que se encuentre la interfaz.
         */
        protected void clickAceptar(object sender, EventArgs e) {

            Boolean operacionCorrecta = true;

            if (modo == 1) {
                operacionCorrecta = insertarUsuario();

                if (operacionCorrecta) { // si inserto el usuario : va a modo 4
                    usuarioConsultado = controladora.consultarUsuario(this.textUsername.Value.ToString());
                    modo = 4;
                    habilitarCampos(false);
                } // si no lo inserto no debe cambiar de modo ni limpiar la pantalla
            } else if (modo == 2) {
                Object[] datosNuevos = obtenerDatosFormulario();

                String[] error = controladora.modificarUsuario(datosNuevos, usuarioConsultado);

                mostrarMensaje(error[0], error[1], error[2]);

                usuarioConsultado = controladora.consultarUsuario(this.textUsername.Value.ToString());
                llenarGrid();
                modo = 4;
            } else if (modo == 3) {
                limpiarCampos();
                modo = 0;
            }
            if (operacionCorrecta) {
                irAModo();
                llenarGrid();
            }
        }

        /**
         * Obtiene todos los datos ingresados en el formulario en pantalla.
         * 
         * @return Object [] : todos los datos ingresados como Object.
         */
        protected Object[] obtenerDatosFormulario() {
            Object[] datosNuevos = new Object[8];
            datosNuevos[0] = this.textUsername.Value.ToString();
            datosNuevos[1] = this.comboRol.SelectedValue.ToString();
            datosNuevos[2] = this.textNombre.Value.ToString();
            datosNuevos[3] = this.textApellido1.Value.ToString();
            datosNuevos[4] = this.textApellido2.Value.ToString();
            datosNuevos[5] = this.textTelefono.Value.ToString();
            datosNuevos[6] = this.textEmail.Value.ToString();
            datosNuevos[7] = this.textPassword.Value.ToString();
            return datosNuevos;
        }

        /**
         * Evento que se dispara cuando el usuario hace clic en el botón "Cancelar" de la interfaz.
         */
        protected void clickCancelar(object sender, EventArgs e) {
            modo = 0;
            irAModo();
            limpiarCampos();
            habilitarCampos(false);
            usuarioConsultado = null;
        }

        /**
         * Inserta un usuario en el sistema.
         * Se llama luego de "clickAceptar" en modo 1.
         */
        protected Boolean insertarUsuario() {
            Boolean res = true;
            Object[] usuario;

            usuario = obtenerDatosFormulario();

            String[] error = controladora.insertarUsuario(usuario);

            mostrarMensaje(error[0], error[1], error[2]);
            //mostrarMensajeError(error);
            if (error[0].Contains("success")) {
                llenarGrid();
            } else { //creo q no!:)
                res = false;
                modo = 1;
            }
            return res;
        }

        /**
         * Consulta un usuario en el sistema.
         *
         * @param String username :nombre del usuario a consultar.
         */
        protected void consultarUsuario(String username) {
            seConsulto = true;
            try {
                usuarioConsultado = controladora.consultarUsuario(username); // se intenta consultar el usuario con la controladora y llenar los datos en pantalla
                //Los datos de la prueba consultada se setean en el page_load, puesto que se debe hacer un redirect debido a la presencia del grid en un modal.
                modo = 4;
            } catch {
                //Falló la consulta del usuario, se setea la entidad en null.
                usuarioConsultado = null;
                modo = 0;
            }

            irAModo();// se cambia a modo de consulta
            //Fuera de este método se redirecciona, a nivel de row command para no interrumpir la ejecución del método.
        }

        /**
         * Llena el formulario con los datos del usuario consultado.
         */
        protected void setDatosConsultados() {
            this.textUsername.Value = usuarioConsultado.Username;
            this.comboRol.SelectedValue = usuarioConsultado.Rol;
            this.textNombre.Value = usuarioConsultado.Nombre;
            this.textApellido1.Value = usuarioConsultado.Apellido1;
            this.textApellido2.Value = usuarioConsultado.Apellido2;
            this.textTelefono.Value = usuarioConsultado.Telefono;
            this.textEmail.Value = usuarioConsultado.Email;
        }

        /**
         * Evento que se llama al hacer click en el botón "Consultar" del grid de usuarios.
         */ 
        protected void gridViewUsuarios_RowCommand(object sender, GridViewCommandEventArgs e) {
            switch (e.CommandName) {
                case "Select":
                    GridViewRow filaSeleccionada = this.gridViewUsuarios.Rows[Convert.ToInt32(e.CommandArgument) + (this.gridViewUsuarios.PageIndex * resultadosPorPagina)];
                    String username = Convert.ToString(filaSeleccionada.Cells[1].Text);
                    consultarUsuario(username);
                    Response.Redirect("/Forms/Usuarios/Usuarios.aspx");
                    break;
            }

        }

        /**
         * Paginación del grid de usuarios. 
         */
        protected void gridViewUsuarios_PageIndexChanging(Object sender, GridViewPageEventArgs e) {
            this.gridViewUsuarios.PageIndex = e.NewPageIndex;
            this.gridViewUsuarios.DataBind();
        }

        /**
         * Limpia el formulario de la interfaz.
         */
        protected void limpiarCampos() {
            this.textUsername.Value = "";
            this.comboRol.SelectedIndex = 0;
            this.textNombre.Value = "";
            this.textApellido1.Value = "";
            this.textApellido2.Value = "";
            this.textTelefono.Value = "";
            this.textEmail.Value = "";
        }

        /**
         * Carga el grid con los datos de todos los usuarios en el sistema.
         */
        protected void llenarGrid() {
            DataTable tabla = crearTablaUsuarios();

            try {
                Object[] datos = new Object[4];
                DataTable usuarios = controladora.consultarUsuarios();

                if (usuarios.Rows.Count > 0) {
                    foreach (DataRow fila in usuarios.Rows) {
                        datos[0] = fila[0].ToString();
                        datos[1] = fila[2].ToString() + " " + fila[3].ToString() + " " + fila[4].ToString();
                        datos[2] = fila[1].ToString();
                        datos[3] = fila[6].ToString();
                        tabla.Rows.Add(datos);
                    }
                } else {
                    datos[0] = "-";
                    datos[1] = "-";
                    datos[2] = "-";
                    datos[3] = "-";
                    tabla.Rows.Add(datos);
                }


                this.gridViewUsuarios.DataSource = tabla;
                this.gridViewUsuarios.DataBind();
            } catch (Exception e) {
                mostrarMensaje("warning", "Alerta", "No hay conexión a la base de datos.");
            }
        }

        /**
         * Crea el DataTable personalizado para mostrar los datos de los usuarios.
         * 
         * @return DataTable : estructura con los datos de usuarios a mostrar en pantalla.
         */ 
        protected DataTable crearTablaUsuarios() {
            DataTable tabla = new DataTable();
            DataColumn columna;

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Nombre de Usuario";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Dueño";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Rol";
            tabla.Columns.Add(columna);

            columna = new DataColumn();
            columna.DataType = System.Type.GetType("System.String");
            columna.ColumnName = "Email";
            tabla.Columns.Add(columna);

            return tabla;
        }

        /**
         * Oculta el mensaje de error, advertencia y éxito de la interfaz.
         */
        protected void ocultarMensaje() {
            alertAlerta.Attributes.Add("hidden", "hidden");
        }

        /**
         * Muestra el mensaje de error, advertencia y éxito de la interfaz.
         * 
         * @param String tipoAlerta : tipo del mensaje a mostrar.
         * @param String alerta : título del mensaje.
         * @param String mensaje : mensaje a mostrar.
         */
        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje) {
            alertAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            alertAlerta.Attributes.Remove("hidden");
        }

        /*
         * Este metodo se llama cuando se cancela una consulta.
         * Es necesario por tecnología.
         */
        protected void cancelarConsultar(object sender, EventArgs e) {
        }
    }
}