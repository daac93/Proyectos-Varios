using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NotifiacionesOfflineSAR {
    public sealed class ManejadorXml {
        static readonly ManejadorXml _instance = new ManejadorXml();
        private static XmlWriter writer;

        public static ManejadorXml Instance {
            get {
                return _instance;
            }
        }

        /**
         * Constructor.
         */
        ManejadorXml() {
            crearXmlWriter();
        }

        /**
         * Crea un objeto de configuración para un XmlWriter que cuenta con indentación y atributos en líneas individuales.
         * @return XmlWriterSettings: objeto de configuración.
         */
        private static XmlWriterSettings configurarXmlWriter() {
            XmlWriterSettings settings;

            settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            return settings;
        }

        /**
         * Crea un objeto XmlWriter con la configuración dada por "configurarXmlWriter".
         */
        private static void crearXmlWriter() {
            XmlWriterSettings settings = configurarXmlWriter();

            XmlWriter writer = XmlWriter.Create("Notificaciones.xml", settings);
        }

        /**
         * Agrega la etiqueta de apertura al archivo Xml.
         */
        public void iniciarXml() {
            writer.WriteStartDocument();
            abrirNuevoElementoEnXml("data");
        }

        /**
         * Abre un nuevo elemento en el Xml.
         */
        public void abrirNuevoElementoEnXml(String etiquetaElemento)   {
            writer.WriteStartElement(etiquetaElemento);
        }
    
        /**
         * Cierra el último elemento abierto en el Xml.
         */
        public void cerrarElementoEnXml()   {
            writer.WriteEndElement();
        }

        /**
         * Agrega la etiqueta de cierre al archivo Xml.
         */
        public void cerrarArchivoXml() {
            cerrarElementoEnXml();

            writer.WriteEndDocument();
        }

        /**
         * Cierra el objeto XmlWriter. 
         */
        public void cerrarXmlWriter() {
            writer.Close();
        }

        /**
         * Agrega un nuevo hijo al XML.
         * @param etiquetaElemento etiqueta para el hijo en el archivo XML.
         * @param codigoID Identificador único a nivel de base de datos para el reactivo.
         * @param codigo Código del reactivo.
         * @param nombreReactivo Nombre del reactivo.
         * @param detalle Fecha de vencimiento o cantidad restantes, dependiendo del caso.
         */
        public void agregarEntradaXML(String etiquetaElemento, String codigoId, String codigo, String nombreReactivo, String detalle) {
            writer.WriteStartElement(etiquetaElemento);

            writer.WriteElementString("codigoId", codigoId);
            writer.WriteElementString("codigo", codigo);
            writer.WriteElementString("nombre", nombreReactivo);
            writer.WriteElementString("detalle", detalle);

            writer.WriteEndElement();
        }
    }
}
