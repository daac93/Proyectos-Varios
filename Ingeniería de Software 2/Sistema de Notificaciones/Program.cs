using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using System.Threading;
using System.Xml;

namespace NotifiacionesOfflineSAR {
    class Program {
        private static ManejadorXml writer;
        private static ManejadorBaseDatos manejadorBD;
        private static ManejadorEmail manejadorEmail;

        /**
         * Método principal del programa.
         * @param string[] args: argumentos para el programa.
         */
        static void Main(string[] args) {
            String resultado;

            writer.iniciarXml();

            resultado = manejadorBD.barridoBD();

            writer.cerrarArchivoXml();
            writer.cerrarXmlWriter();

            if (args.Length > 0) {
                manejadorEmail.enviarEmail("Notificaciones Sistema Administración de Reactivos", resultado);
            }
        }

        private void obtenerInstancias() {
            writer = ManejadorXml.Instance;
            manejadorBD = ManejadorBaseDatos.Instance;
            manejadorEmail = ManejadorEmail.Instance;
        }

        

        
    }
}
