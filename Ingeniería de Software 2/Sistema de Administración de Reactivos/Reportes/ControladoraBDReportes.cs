using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using SAR.App_Code.DataSets.ReportesDataSetTableAdapters;

namespace SAR.App_Code.Reportes {
    public class ControladoraBDReportes {
        private SqlConnection conexion;
        private SqlDataReader lector;
        private int CANTIDAD_FILTROS = 6;

        //Diccionario que contiene cada SQL statement para seleccionar una columna especifica de la tabla Reactivos.
        private Dictionary<int, string> diccionarioColumnas = new Dictionary<int, string>()   {
	            {1, "Reactivos.codigoReactivo as 'Código'"},
                {2, "Reactivos.nombre as 'Nombre'"},
                {3, "Reactivos.descripcion as 'Descripción'"},
                {4, "Reactivos.ubicacion as 'Ubicación'"},
                {5, "Reactivos.cantidadMuestra as 'Cantidad Muestra'"},
                {6, "Reactivos.unidadMuestra as 'Unidad Muestra'"},
                {7, "Reactivos.cantidadReactivo as 'Cantidad Reactivo'"},
                {8, "Reactivos.unidadReactivo as 'Unidad Reactivo'"},
                {9, "Reactivos.restante as 'Restante'"},
                {10, "Reactivos.vence as 'Fecha Vencimiento'"},
                {11, "(dbo.transformarActivo(activo)) as 'Estado'"},
	        };

        //Diccionario que contiene el SQL statement para agrupar tuplas.
        private Dictionary<string, string> diccionarioAgrupacion = new Dictionary<string, string>()   {
	            {"proveedor", "Proveedores.compania as 'Proveedor', "},
                {"prueba", "Pruebas.nombre as 'Prueba', "},
                {"", ""}
	        };

        //Diccionario que contiene el SQL statement para poder aplicar cada uno de los filtros a las tuplas.
        private Dictionary<int, string> diccionarioFiltros = new Dictionary<int, string>()   {
	            {0, "Reactivos.codigoReactivo like '%*$%'"},
                {1, "Reactivos.nombre like '%*$%'"},
                {2, "Pruebas.nombre = '*$'"},
                {3, "Reactivos.ubicacion = "},
                {4, "Reactivos.activo = "}
	        };

        /**
         * Constructor.
         */
        public ControladoraBDReportes() {
            crearConexion();
        }

        /**
         * Inicializa el objeto conexion para poder conectarse a la base de datos. 
         */
        private void crearConexion() {
            //Obtener la "DefaultConnection" del archivo "Web.config" para conectarse a la Base de Datos.
            conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings ["DefaultConnection"].ConnectionString);
        }

        /**
         * Cierra la conexión con la base de datos.
         */
        private void cerrarConexion() {
            conexion.Close();
        }

        /**
         * Ejecuta una query en la base de datos.
         * 
         * @param Object [] datosQuery : los datos que se desean mostrar en el reporte.  Tiene la siguiente estructura:
         *                                  datosQuery[0] : Boolean [] -> contiene cada una de las columnas que se van a mostrar.  
         *                                                                  Tiene mismo orden que "listaCamposDisponibles" en "Reportes.aspx.cs".
         *                                  datosQuery[1] : String [] -> contiene cada uno de los filtros que se va a aplicar a la query.
         *                                  datosQuery[2] : Criterio de agrupación ("", "proveedor", "prueba").
         * 
         * @return DataTable : los datos obtenidos de la query.
         */
        public DataTable generarReporte(Object [] datosQuery) {
            String query = "";

            Boolean [] columnasSolicitadas = (Boolean []) datosQuery[0];
            String [] filtros = (String[]) datosQuery[1];
            String criterioAgrupacion = (String)datosQuery[2];              

            //Agregar Select
            query += generarSelect(columnasSolicitadas, criterioAgrupacion);

            //Agregar From
            query += " " + generarFrom(criterioAgrupacion, filtros[2]);

            //Agregar where
            query += " " + generarWhere(filtros, criterioAgrupacion);

            //Agregar el order by
            query += generarOrderBy(criterioAgrupacion);

            //Agregar ;
            query += ";";
            
            try {
                conexion.Open();

                DataTable datos = new DataTable();

                //Ejecuta el comando SQL
                lector = new SqlCommand(query, conexion).ExecuteReader();

                //Carga los datos obtenidos al DataTable
                datos.Load(lector);

                cerrarConexion();

                return datos;

            } catch (Exception e) {
                //Hubo un problema generando el reporte.
                return null;
            }
        }

        /**
         * Ejecuta una query en la base de datos que permite obtener datos de un reportes histórico.
         * 
         * @param fechaInicio : fecha donde comienza el uso de reactivos.
         * @param fechaFinal : fecha donde termina el uso de reactivos.
         * 
         * @return DataTable : los datos obtenidos de la query.
         */
        public DataTable generarReporteHistorico(String fechaInicio, String fechaFinal) {
            DataTable datos = new DataTable();
            ReportesTableAdapter adapterReportes = new ReportesTableAdapter();

            try {
                datos = adapterReportes.GetData(fechaInicio, fechaFinal);

                return datos;
            } catch (Exception e) {
                //Hubo un problema generando el reporte.
                return null;
            }
        }

        /**
         * Genera el "Select statement" para obtener solamente las columnas que se solicitaron en la interfaz.
         * El orden de las columnas en el vector debe corresponder con el orden en que se definen en el diccionario "diccionarioColumnas.
         * 
         * @param Boolean[] columnas : valor booleano que indica si la columna en la posición columnas[i] fue seleccionada en la interfaz. La posición 0 indica si se selecciono la casilla "Todos".
         * @return String : Select statement para la consulta SQL con la que se va a generar el reporte
         */
        protected String generarSelect(Boolean [] columnas, String criterioAgrupamiento) {
            String select = "select ";
            Boolean todasLasColumnas = false, unaColumna = false;


            select += diccionarioAgrupacion [criterioAgrupamiento];

            //Si se checkearon todos los checkboxes
            if (columnas [0]) {
                todasLasColumnas = true;
            }


            foreach (var entrada in diccionarioColumnas) {
                if (columnas [entrada.Key] || todasLasColumnas) {
                    select += entrada.Value + ", ";
                    unaColumna = true;
                }
            }

            if (unaColumna) {
                select = select.Remove(select.Length - 2);
            }

            return select;
        }

        /**
         * Genera la claúsula "from" para la query que se va a ejecutar.
         * El criterio de agrupación determina si se deben agregar más tablas.
         * De igual manera se deben agregar más tablas si se filtró por prueba.
         * 
         * @return String : "from" statement para query.
         */
        protected String generarFrom(String criterioAgrupacion, String filtradoPorPrueba) {
            String from = "from Reactivos ";

            switch (criterioAgrupacion) {
                case "proveedor":
                    from = @"from Provee FULL OUTER JOIN 
                                Proveedores ON Provee.idProveedor = Proveedores.idProveedor FULL OUTER JOIN 
                                Reactivos ON Provee.codigoIDReactivo = Reactivos.codigoID";
                    break;
                case "prueba":
                    from = @"from Reactivos FULL OUTER JOIN 
                                SeUsaEn ON Reactivos.codigoID = SeUsaEn.idReactivo FULL OUTER JOIN 
                                Pruebas ON SeUsaEn.nombrePrueba = Pruebas.nombre";
                    break;
            }

            if (criterioAgrupacion != "prueba"  && filtradoPorPrueba != null) {
                from += @"FULL OUTER JOIN
                            SeUsaEn ON Reactivos.codigoID = SeUsaEn.idReactivo FULL OUTER JOIN
                            Pruebas ON SeUsaEn.nombrePrueba = Pruebas.nombre";
            }

            return from;
        }

        /**
         * Genera el "where" statement para la query basado en los filtros que se escogieron para el reprote.
         * 
         * @return String : "where" statement para la consulta SQL.
         */
        protected String generarWhere(String [] filtros, String criterioAgrupamiento) {
            String where = "where 1 = 1";

            for (int i = 0; i < CANTIDAD_FILTROS - 2; i++) {
                if ((i >= 0 && i <= 2) && filtros[i] != null) {
                    where += " and " + (diccionarioFiltros [i].Replace("*$", filtros[i]));
                } else if (filtros [i] != null) {
                    where += " and " + diccionarioFiltros [i] + filtros [i];
                }
            }

            if(filtros[4] != "" && filtros[5] != "todos")    {
                where += " and " + diccionarioFiltros [4] + filtros [4];
            }

            switch (criterioAgrupamiento) {
                case "":
                    //Sin criterio de agrupamiento.
                    break;
                case "proveedor":
                    where += " and Provee.codigoIDReactivo = codigoID ";
                    break;
                case "prueba":
                    where += " and Pruebas.nombre = SeUsaEn.nombrePrueba and SeUsaEn.idReactivo = Reactivos.codigoID ";
                    break;
            }

            return where;
        }

        /**
         * Genera la claúsula "order by" en caso de que se haya especificado un criterio de agrupación para las tuplas.
         * 
         * @return String : "order by" statement para agrupar las tuplas de la query.
         */
        protected String generarOrderBy(String criterioAgrupacion) {
            String orderBy = " order by Reactivos.nombre";

            switch (criterioAgrupacion) {
                case "proveedor":
                    orderBy = " order by 'Proveedor'";
                    break;
                case "prueba":
                    orderBy = " order by 'Prueba'";
                    break;
            }

            return orderBy;
        }
    }
}