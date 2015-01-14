using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using SAR.App_Code.Notas;
using System.Data;

namespace SAR.Forms{
    

    public partial class Dashboard : System.Web.UI.Page {
        private static ControladoraNotas controladoraNotas;
        private static EntidadNota nota;
        protected void Page_Load(object sender, EventArgs e) {
            controladoraNotas = new ControladoraNotas();
            crearNotificaciones();
            agregarNotasAlDashboard();
        }

        protected void crearNotificaciones() {
            XmlNodeList data, reactivos;
            XmlDocument xml;
                
            xml = new XmlDocument();

            xml.Load(Server.MapPath("../Notificaciones/Notificaciones.xml"));

            data = xml.GetElementsByTagName("data");

            reactivos = ((XmlElement)data[0]).GetElementsByTagName("reactivosPorVencer");
            agregarNotificaciones(notificacionesReactivosVencer, reactivos, "reactivoPorVencer");

            reactivos = ((XmlElement)data[0]).GetElementsByTagName("reactivosPorAcabarse");
            agregarNotificaciones(notificacionesReactivosAcabarse, reactivos, "reactivoPorAcabarse");

        }

        protected void agregarNotificaciones(HtmlGenericControl listaInterfaz, XmlNodeList reactivos, String tipoNodo) {
            XmlNodeList codigoId, codigo, nombre, detalles, lista;
            HtmlGenericControl elementoLista;

            lista = ((XmlElement)reactivos[0]).GetElementsByTagName(tipoNodo);

            foreach (XmlElement i in lista) {
                codigoId = i.GetElementsByTagName("codigoId");
                codigo = i.GetElementsByTagName("codigo");
                nombre = i.GetElementsByTagName("nombre");
                detalles = i.GetElementsByTagName("detalle");

                elementoLista = crearEntradaNotificacion(codigoId[0].InnerText, codigo[0].InnerText, nombre[0].InnerText, detalles[0].InnerText);

                listaInterfaz.Controls.Add(elementoLista);
            }
        }

        protected HtmlGenericControl crearEntradaNotificacion(String codigoId, String codigo, String nombre, String detalle) {
            HtmlGenericControl nuevaEntrada = new HtmlGenericControl();

            /*<a href="URL" class="list-group-item">
                    <span class="pull-right text-muted small"><em>4 minutes ago</em>
                </span>
              </a>*/

            //<em>DETALLE</em>
            HtmlGenericControl em = new HtmlGenericControl();
            em.InnerText = detalle;
            em.TagName = "em";

            //<span class="pull-right text-muted small">EM</span>
            HtmlGenericControl span = new HtmlGenericControl();
            span.Attributes["class"] = "pull-right text-muted small";
            span.TagName = "span";
            span.Controls.Add(em);

            //<a href="URL" class="list-group-item">
            nuevaEntrada.Attributes["class"] = "list-group-item";
            nuevaEntrada.Attributes["href"] = "Reactivos/Reactivos.aspx?c=" + codigoId + "&m=4";
            nuevaEntrada.TagName = "a";
            nuevaEntrada.InnerText = codigo + " | " + nombre;
            nuevaEntrada.Controls.Add(span);

            return nuevaEntrada;
        }


        //-------------------Código para las notas-------------------------
        protected void clickNota(object sender, EventArgs e){
            LinkButton theButton = (LinkButton)sender;
            nota = consultarNota(textIdNota.Value.ToString());
            textTitulo.Value = nota.Titulo;
            textCuerpoNota.Value = nota.Contenido;
            //Código para mostrar la ventana modal...
        }

        protected void botonAceptarAgregarNota(object sender, EventArgs e){
            Object [] datos = new Object [3];
            datos[0] = "0";
            datos[1] = textTitulo.Value.ToString();
            datos[2] = textCuerpoNota.Value.ToString();
            controladoraNotas.insertarNota(datos);
            textTitulo.Value = "";
            textCuerpoNota.Value = "";
            agregarNotasAlDashboard();
        }

        protected void botonAceptarEliminarNota(object sender, EventArgs e){
            nota = consultarNota(textIdNota.Value.ToString());
            controladoraNotas.eliminarNota(nota); //Se supone que la nota ya está consultada al hacer click en el botón eliminar...
            agregarNotasAlDashboard();
        }

        protected void botonAceptarEditarNota(object sender, EventArgs e){
            nota = consultarNota(textIdNota.Value.ToString());
            Object[] datos = new Object[3];
            datos[0] = textIdNota.Value.ToString(); //No debería de hace falta
            datos[1] = textModificarTitulo.Value.ToString();
            datos[2] = textAreaModificarCuerpo.Value.ToString();
            //nota = this.consultarNota(datos[0].ToString()); No debería de hacer falta ya que se consultó al hacer click en el link
            controladoraNotas.modificarNota(datos, nota);
            agregarNotasAlDashboard();
        }

        protected void botonConsultarNota(object sender, EventArgs e) {
            nota = consultarNota(textIdNota.Value.ToString());
            textModificarTitulo.Value = nota.Titulo;
            textAreaModificarCuerpo.Value = nota.Contenido;
            modalModificarNota.Attributes.Remove("hidden");
        }


        protected void agregarNotasAlDashboard() {
            DataTable notas = controladoraNotas.consultarNotas();
            HtmlGenericControl notaNueva;
            notasDash.InnerHtml = "";
            if (notas.Rows.Count > 0) {
                foreach (DataRow nota in notas.Rows) {
                    notaNueva = agregarNotaAlHTML(nota[0].ToString(), nota[1].ToString(), nota[2].ToString());
                    notasDash.Controls.Add(notaNueva);
                }
            }
        }

        protected void botonCancelarModificar (object sender, EventArgs e){
            modalModificarNota.Attributes["hidden"] = "hidden";
        }

        protected EntidadNota consultarNota(String id) {
            nota = controladoraNotas.consultarNota(id);
            return nota;
        }

        protected HtmlGenericControl agregarNotaAlHTML(string id, string titulo, string cuerpo) {
            HtmlGenericControl nota = new HtmlGenericControl();
            HtmlGenericControl boton = new HtmlGenericControl();
            HtmlGenericControl link = new HtmlGenericControl();
            HtmlGenericControl tituloNota = new HtmlGenericControl();
            HtmlGenericControl cuerpoNota = new HtmlGenericControl();
            HtmlGenericControl icono = new HtmlGenericControl();
            //<p>Cuerpo</p>
            cuerpoNota.TagName = "p";
            cuerpoNota.InnerText = cuerpo;
            //<h2>Titulo</h2>
            tituloNota.TagName = "h2";
            tituloNota.InnerText = titulo;
            //<a href="#modalNota" role="button" data-toggle="modal" runat="server">..</a>
            link.TagName = "a";
            link.Attributes["href"] = "#modalModificar";
            link.Attributes["id"] = id;
            link.Attributes["role"] = "button";
            link.Attributes["onclick"] = "obtenerId(id)"; 

            link.Controls.Add(tituloNota);
            link.Controls.Add(cuerpoNota);
            //<i class="fa fa-times"></i>
            icono.TagName = "i";
            icono.Attributes["class"] = "fa fa-thumb-tack";
            //<button type="button" class="close boton-eliminar-nota" data-toggle="modal" data-target="#modalEliminarNota">...</button>
            boton.TagName = "button";
            boton.Attributes["type"] = "button";
            boton.Attributes["id"] = (Convert.ToInt32(id)*(-1)).ToString(); //El botón de eliminar tendrá un id negativo para diferenciarlo del link
            boton.Attributes["class"] = "close boton-eliminar-nota";
            boton.Attributes["data-toggle"] = "modal";
            boton.Attributes["data-target"] = "#modalEliminarNota";
            boton.Attributes["onclick"] = "obtenerId(id)"; 
            boton.Controls.Add(icono);
            //<li>..</li>
            nota.TagName = "li";
            nota.Controls.Add(boton);
            nota.Controls.Add(link);
            return nota;
        }

    }
}