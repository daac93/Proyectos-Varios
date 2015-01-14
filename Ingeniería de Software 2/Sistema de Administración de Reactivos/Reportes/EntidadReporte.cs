using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SAR.App_Code.Reportes {
    
    public class EntidadReporte {
        private String nombre;
        private DataTable datos;

        public EntidadReporte(Object[] datos) {
            this.nombre = datos[0].ToString();
            this.datos = (DataTable)datos[1];
        }

        public String Nombre {
            get { return nombre; }
            set { nombre = value; }
        }

        public DataTable Datos {
            get { return datos; }
            set { datos = value; }
        }
    }
}