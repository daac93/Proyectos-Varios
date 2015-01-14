using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAR.App_Code.Usuarios {

    public class EntidadUsuario {
        private String username;
        private String rol;
        private String nombre;
        private String apellido1;
        private String apellido2;
        private String telefono;
        private String email;
        private String password;    //Solamente si usa cuando se va insertar a la BD, no cuando se consulta.

        /**
         * Constructor.
         */
        public EntidadUsuario(Object[] datos)   {
            this.username = datos[0].ToString();
            this.rol = datos[1].ToString();
            this.nombre = datos[2].ToString();
            this.apellido1 = datos[3].ToString();
            this.apellido2 = datos[4].ToString();
            this.telefono = datos[5].ToString();
            this.email = datos[6].ToString();
            this.password = datos[7].ToString();
        }

        public String Username {
            get { return username; }
            set { username = value; }
        }

        public String Rol {
            get { return rol; }
            set { rol = value; }
        }

        public String Nombre {
            get { return nombre; }
            set { nombre = value; }
        }

        public String Apellido1 {
            get { return apellido1; }
            set { apellido1 = value; }
        }

        public String Apellido2 {
            get { return apellido2; }
            set { apellido2 = value; }
        }

        public String Telefono {
            get { return telefono; }
            set { telefono = value; }
        }

        public String Email {
            get { return email; }
            set { email = value; }
        }

        public String Password {
            get { return password; }
            set { password = value; }
        }
    }
}