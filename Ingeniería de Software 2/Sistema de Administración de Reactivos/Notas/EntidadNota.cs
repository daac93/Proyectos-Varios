using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAR.App_Code.Notas
{
    public class EntidadNota    {
    //Atributos
        private String titulo;
        private String contenido;
        private String idNota;
      
        /*Constructor.
         * recibe un vector de tipo Object
         * con los datos de la nota que se
         * van a encapsular.
         */
        public EntidadNota(Object [] datos) {
            this.idNota = datos[0].ToString();
            this.titulo = datos[1].ToString();
            this.contenido = datos[2].ToString();
        }

        public String Titulo {
            get { return titulo; }
            set { titulo = value; }
        }


        public String Contenido{
            get { return contenido; }
            set { contenido = value; }
        }

        public String IdNota {
            get { return idNota; }
            set { idNota = value; }
        }
    }
}