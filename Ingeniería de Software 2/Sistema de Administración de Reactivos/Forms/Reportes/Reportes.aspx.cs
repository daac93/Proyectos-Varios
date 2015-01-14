using OfficeOpenXml;
using SAR.App_Code.Pruebas;
using SAR.App_Code.Reportes;
using SAR.App_Code.Reactivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Data;

namespace SAR.Forms.Reportes {
    public partial class Reportes : System.Web.UI.Page {
        private static int CANTIDAD_FILTROS = 6;
        private static ControladoraReportes controladoraReportes;
        private static ControladoraPruebas controladoraPruebas;
        private static ControladoraReactivo controladoraReactivos;
        private static String [] ubicaciones = { "--Ubicación--", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "Oficina", "Refrigeradora" };
        private static String [] listaCamposDisponibles = { "CodigoReactivo", "NombreReactivo", "DescripcionReactivo", "UbicacionReactivo",
                                                    "CantidadMuestraReactivo", "UnidadMuestraReactivo", "CantidadReactivo",
                                                    "UnidadReactivo", "RestanteReactivo", "VenceReactivo", "ActivoReactivo"};


        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                controladoraReportes = new ControladoraReportes();
                controladoraReactivos = new ControladoraReactivo();
                controladoraPruebas = new ControladoraPruebas(controladoraReactivos);

                establecerAgrupacionDefault();

                cargarComboBoxPruebas();
                cargarComboBoxUbicaciones();


            }
        }

        protected void generarReporte(object sender, EventArgs e) {
            Object [] datosQuery = new Object [3];

            datosQuery [0] = obtenerCamposDeseados();
            datosQuery [1] = obtenerFiltros();
            datosQuery [2] = obtenerCriterioAgrupacion();

            ExcelPackage reporte = controladoraReportes.generarReporte(datosQuery);

            if (reporte != null) {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Reporte.xlsx");
                Response.BinaryWrite(reporte.GetAsByteArray());
                Response.End();
            } else {
                String [] resultado = new String [3];
                resultado [0] = "danger";
                resultado [1] = "Error. ";
                resultado [2] = "Ha sucedido un problema, no se pudo generar el reporte.";
                mostrarMensaje(resultado [0], resultado [1], resultado [2]);
            }
        }

        /**
         *  Genera un reporte histórico. 
         */
        protected void generarReporteHistorico(object sender, EventArgs e) {
            if (textFechaInicio.Value.ToString() != "" && textFechaFinal.Value.ToString() != "") {

                String fechaInicio, fechaFinal;

                fechaInicio = "01/" + textFechaInicio.Value.ToString();
                fechaFinal = "01/" + textFechaFinal.Value.ToString();

                ExcelPackage reporte = controladoraReportes.generarReporteHistorico(fechaInicio, fechaFinal);

                if (reporte != null) {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Reporte.xlsx");
                    Response.BinaryWrite(reporte.GetAsByteArray());
                    Response.End();
                } else {
                    String [] resultado = new String [3];
                    resultado [0] = "danger";
                    resultado [1] = "Error. ";
                    resultado [2] = "Ha sucedido un problema, no se pudo generar el reporte.";
                    mostrarMensaje(resultado [0], resultado [1], resultado [2]);
                }
            } else {
                String [] resultado = new String [3];
                resultado [0] = "danger";
                resultado [1] = "Error. ";
                resultado [2] = "Favor seleccione una fecha de inicio y una final para el reporte histórico.";
                mostrarMensaje(resultado [0], resultado [1], resultado [2]);
            }
        }

        /**
         * Revisa cada uno de los checkboxes presentes en la pantalla, en la sección "Campos deseados" y devuelve un vector con valores
         * booleanos indicando si cada campo fue seleccionado.
         * Cada checkbox debe tener un id como: "check<nombreColumna>".
         * Además, cada Id de los checkboxes debe estar declarado en el vector "listaCamposDisponibles".
         * 
         * @return Boolean[] : Vector cuya posicion n indica si el n-ésimo campo de la "listaCamposDisponibles" fue seleccionado. 
         */
        protected Boolean [] obtenerCamposDeseados() {
            String checkboxId = "";
            Boolean [] camposSeleccionados;
            HtmlInputCheckBox noSeUsa = null;
            HtmlControl labelNoSeUsa = null;
            var checkboxActual = noSeUsa;
            var labelContenedor = labelNoSeUsa;
            camposSeleccionados = new Boolean [listaCamposDisponibles.Length + 1];

            //Bloque para el checbox "todo"
            if (checkTodos.Checked) {
                camposSeleccionados [0] = true;
                return camposSeleccionados;
            }

            for (int i = 0; i < listaCamposDisponibles.Length; i++) {
                checkboxId = listaCamposDisponibles [i];

                labelContenedor = (HtmlControl)fieldset.FindControl("label" + checkboxId);

                checkboxActual = (HtmlInputCheckBox)labelContenedor.FindControl("check" + checkboxId);

                if (checkboxActual.Checked) {
                    camposSeleccionados [i + 1] = true;
                }
            }

            return camposSeleccionados;
        }

        /**
         * Revisa cada uno de los checkboxes presentes en la pantalla, en la sección "Filtros".
         * Cada checkbox debe tener un id como: "check<nombreColumna>".
         * 
         * @return String [] : Vector con cada uno de los criterios de búsqueda.
         */
        protected String [] obtenerFiltros() {
            String [] filtros = new String [CANTIDAD_FILTROS];

            if (checkCodigo.Checked) {
                filtros [0] = textCodigo.Value;
            }
            if (checkNombre.Checked) {
                filtros [1] = textNombre.Value;
            }
            if ((comboPrueba.SelectedItem).Text != "--Pruebas--") {
                filtros [2] = (comboPrueba.SelectedItem).Text;
            }
            if ((comboUbicacion.SelectedItem).Text != "--Ubicación--") {
                filtros [3] = (comboUbicacion.SelectedItem).Text;
            }
            if (checkActivo.Checked) {
                filtros [4] = "1";
            }
            if (checkMostrarTodos.Checked) {
                filtros [5] = "todos";
            }

            return filtros;
        }

        protected String obtenerCriterioAgrupacion() {
            if (radioAgruparNinguno.Checked) {
                return "";
            } else if (radioAgruparProveedor.Checked) {
                return "proveedor";
            } else {
                return "prueba";
            }
        }

        protected void cargarComboBoxPruebas() {
            DataTable pruebas = controladoraPruebas.consultarPruebas();
            if (pruebas != null) {
                String [] datos = new String [pruebas.Rows.Count + 1];
                datos [0] = "--Pruebas--";
                for (int i = 1; i <= pruebas.Rows.Count; i++) {
                    datos [i] = pruebas.Rows [i - 1] [0].ToString();
                }
                comboPrueba.DataSource = datos;
                comboPrueba.DataBind();
            } else {
                String [] resultado = new String [3];
                resultado [0] = "danger";
                resultado [1] = "Error. ";
                resultado [2] = "Ha sucedido un problema, no se pudo establecer conexión con la base de datos.";
                mostrarMensaje(resultado [0], resultado [1], resultado [2]);
            }
        }

        protected void cargarComboBoxUbicaciones() {
            comboUbicacion.DataSource = ubicaciones;
            comboUbicacion.DataBind();
        }

        /**
         * Activa/desactiva checkbox restantes del apartado "Campos Deseados" (cuando se marca el checkbox "Seleccionar todos").
         */
        protected void setEstadoCheckboxesRestantes(object sender, EventArgs e) {
            if (checkTodos.Checked) {
                setEstadoCheckboxesCamposDeseados(true);
            } else {
                setEstadoCheckboxesCamposDeseados(false);
            }
        }

        protected void setEstadoCheckboxesCamposDeseados(Boolean desactivado) {
            String checkboxId = "";
            HtmlInputCheckBox noSeUsa = null;
            HtmlControl labelNoSeUsa = null;
            var checkboxActual = noSeUsa;
            var labelContenedor = labelNoSeUsa;

            for (int i = 0; i < listaCamposDisponibles.Length; i++) {
                checkboxId = listaCamposDisponibles [i];

                labelContenedor = (HtmlControl)fieldset.FindControl("label" + checkboxId);

                checkboxActual = (HtmlInputCheckBox)labelContenedor.FindControl("check" + checkboxId);

                checkboxActual.Disabled = desactivado;
            }
        }

        /**
         * Establece la agrupación por defecto en "Ninguno".
         */
        protected void establecerAgrupacionDefault() {
            this.radioAgruparNinguno.Checked = true;
        }

        protected void mostrarMensaje(String tipoAlerta, String alerta, String mensaje) {
            alertAlerta.Attributes ["class"] = "alert alert-" + tipoAlerta + " alert-dismissable fade in";
            labelTipoAlerta.Text = alerta + " ";
            labelAlerta.Text = mensaje;
            alertAlerta.Attributes.Remove("hidden");
        }
    }
}