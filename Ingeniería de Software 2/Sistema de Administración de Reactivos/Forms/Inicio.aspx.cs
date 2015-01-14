using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SAR.App_Code.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAR.Forms {
    public partial class Inicio : System.Web.UI.Page {
        private static ControladoraUsuarios controladoraUsuario;

        protected void Page_Init(object sender, EventArgs e) {
            //Si el usuario ya esta logueado
            if (HttpContext.Current.User.Identity.IsAuthenticated) {
                //Revisamos el rol, para redirigirlo según el rol.
                if ((HttpContext.Current.User.IsInRole("Administrador"))) {
                    Response.Redirect("Dashboard.aspx");
                } else {
                    Response.Redirect("/Forms/Reactivos/ConsultarReactivos.aspx");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                controladoraUsuario = new ControladoraUsuarios();
            }
        }

        protected void logIn(object sender, EventArgs e) {
            String[] loginCorrecto = controladoraUsuario.logIn(textNombreUsuario.Value, textContraseña.Value);

            if (loginCorrecto[0].Equals("success")) {
                //Si el usuario es asistente, solamente tiene acceso al consultar reactivos.
                if (controladoraUsuario.obtenerRolUsuario(textNombreUsuario.Value).Equals("Administrador")) {
                    Response.Redirect("Dashboard.aspx");
                } else {
                    Response.Redirect("/Forms/Reactivos/ConsultarReactivos.aspx");
                }
            } else {
                //Mensaje de error
                mostrarMensaje(loginCorrecto[0], loginCorrecto[1], loginCorrecto[2]);
            }
        }

        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje) {
            alertAlerta.Attributes["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            alertAlerta.Attributes.Remove("hidden");
        }

        protected void ocultarMensaje() {
            alertAlerta.Attributes.Add("hidden", "hidden");
        }
    }
}