using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifiacionesOfflineSAR {
    public sealed class ManejadorBaseDatos {
        static readonly ManejadorBaseDatos _instance = new ManejadorBaseDatos();
        private static SqlConnection conexion;
        private static ManejadorXml writer;

        public static ManejadorBaseDatos Instance   {
            get {
                return _instance;
            }
        }

        /**
         * Constructor
         */
        ManejadorBaseDatos() {
            conexion = new SqlConnection(Properties.Settings.Default.connectionString);
            writer = ManejadorXml.Instance;
        }

        /**
         * Realiza un barrido a la base de datos determinando los reactivos por vencer (menos de un mes de la fech actual)
         * y aquellos por acabarse (menos del 10% en existencia).
         * @return String: representación HTML del correo a enviar.
         */
        public String barridoBD() {
            String email = "";

            email += barridoReactivosPorVencer();

            email += barridoReactivosPorAcabarse();

            return email;
        }

        /**
         * Realiza el barrido a la base de datos buscando los reactivos por vencer. 
         * @return String String que representa el HTML que se va a enviar por correo con los datos de los reactivos por vencer.
         */
        private String barridoReactivosPorVencer() {
            String[] resultado = new String[2];
            String reactivosPorVencer = "";
            SqlDataReader lector;

            writer.abrirNuevoElementoEnXml("reactivosPorVencer");

            try {
                
                conexion.Open();

                lector = crearLectorReactivosPorVencer();

                reactivosPorVencer = procesarReactivosPorVencer(lector);

                lector.Close();

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            //Cierra el tag <reactivosPor Vencer>
            writer.cerrarElementoEnXml();

            return reactivosPorVencer;
        }

        /**
         * Crea un lector SQL con el query necesario para encontrar los reactivos cercanos a vencer. 
         * @return SqlDataReader : lector que contiene la consulta necesaria para encontrar los reactivos mencionados.
         */
        private SqlDataReader crearLectorReactivosPorVencer() {
            SqlDataReader lector;
            lector = new SqlCommand(@"select codigoId, codigoReactivo, nombre, vence 
                                            from reactivos 
                                            where (CONVERT(datetime, vence, 103)) < DATEADD(M, 1, GETDATE())
                                            and (CONVERT(datetime, vence, 103)) > DATEADD(M, -1, GETDATE())
                                            and activo = '1'", conexion).ExecuteReader();
            return lector;
        }

        /**
         * Procesa las tuplas obtenidas correspondientes a reactivos cercanos a vencer.
         * @param SqlDataReader lector: lector SQL que contiene las tuplas que cumplen con el criterio dado.
         * @return String : representación HTML de los datos de los reactivos prontos a vencer.
         */
        private string procesarReactivosPorVencer(SqlDataReader lector) {
            String htmlReactivosPorVencer = "";

            if (lector.HasRows) {
                htmlReactivosPorVencer += @"<h3>Rectivos prontos a Vencer:</h3>
                                            <table>
                                                <tbody>
                                                    <tr>
		                                                <th>Código</th>
		                                                <th>Nombre</th>		
		                                            <th>Fecha de Vencimiento</th>
	                                            </tr>";

                while (lector.Read()) {
                    htmlReactivosPorVencer += "<tr>\n" + "<td>" + lector.GetString(1) + "</td>\n" +
                                                    "<td>" + lector.GetString(2) + "</td>\n" +
                                                    "<td>" + lector.GetString(3) + "</td>\n</tr>";


                    writer.agregarEntradaXML("reactivoPorVencer", lector.GetInt64(0).ToString(), lector.GetString(1), lector.GetString(2), lector.GetString(3));

                }
                htmlReactivosPorVencer += "</tbody>\n</table>";
            } else {
                //No hay reactivos que cumplan las características.
                htmlReactivosPorVencer = "";
            }
            return htmlReactivosPorVencer;
        }

        /**
         * Realiza el barrido a la base de datos buscando los reactivos por acabarse. 
         * @return String String que representa el HTML que se va a enviar por correo con los datos de los reactivos por acabarse.
         */
        private String barridoReactivosPorAcabarse() {
            String reactivosPorAcabarse = "";
            SqlDataReader lector;

            writer.abrirNuevoElementoEnXml("reactivosPorAcabarse");

            try {
                conexion.Open();

                lector = crearLectorReactivosPorAcabarse();

                reactivosPorAcabarse = procesarReactivosPorAcabarse(lector);

                lector.Close();


            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            //Cierra el tag <reactivosPor Vencer>
            writer.cerrarElementoEnXml();

            return reactivosPorAcabarse;
        }

        /**
         * Crea un lector SQL con el query necesario para encontrar los reactivos cercanos a acabarse. 
         * @return SqlDataReader : lector que contiene la consulta necesaria para encontrar los reactivos mencionados.
         */
        private SqlDataReader crearLectorReactivosPorAcabarse() {
            SqlDataReader lector;
            lector = new SqlCommand(@"select codigoId, codigoReactivo, nombre, vence 
                                            from reactivos 
                                            where (cantidadReactivo*.1) > restante
                                            and activo = '1'", conexion).ExecuteReader();
            return lector;
        }

        /**
         * Procesa las tuplas obtenidas correspondientes a reactivos cercanos a acabarse.
         * @param SqlDataReader lector: lector SQL que contiene las tuplas que cumplen con el criterio dado.
         * @return String : representación HTML de los datos de los reactivos prontos a acabarse.
         */
        private string procesarReactivosPorAcabarse(SqlDataReader lector) {
            String htmlReactivosPorAcabarse = "";
            
            if (lector.HasRows) {
                htmlReactivosPorAcabarse += @"<h3>Rectivos prontos a Acabarse:</h3>
                                            <table>
                                                <tbody>
                                                    <tr>
		                                                <th>Código</th>
		                                                <th>Nombre</th>		
		                                            <th>Cantidad</th>
	                                            </tr>";

                while (lector.Read()) {
                    htmlReactivosPorAcabarse += "<tr>\n" + "<td>" + lector.GetString(1) + "</td>\n" +
                                                    "<td>" + lector.GetString(2) + "</td>\n" +
                                                    "<td>" + lector.GetString(3) + "</td>\n</tr>";

                    writer.agregarEntradaXML("reactivoPorAcabarse", lector.GetInt64(0).ToString(), lector.GetString(1), lector.GetString(2), lector.GetString(3));

                }

                htmlReactivosPorAcabarse += "</tbody>\n</table>";
            } else {
                //No hay reactivos que cumplan las características.
                htmlReactivosPorAcabarse = "";
            }
            return htmlReactivosPorAcabarse;
        }
    }
}
