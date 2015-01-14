using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SAR.App_Code.Notas
{
    public class ControladoraNotas
    {
        ControladoraBDNotas controladoraBDNotas;

        /* Constructor.
         * Inicializa un objeto del Tipo ControladoraBDNotas
         * el cual va a usar para poder enviarl y solicitar información
         * de la base de datos.
         */
        public ControladoraNotas()
        {
            controladoraBDNotas = new ControladoraBDNotas();
        }

        /*Envía un mensaje a la ControladoraBDNotas para insertar un nota.
         * Recibe un vector del tipo Object con los datos del nota que se van a insertar.
         * Retorna un vector de String con la información del éxito o no de la inserción.
         */
        public String[] insertarNota(Object[] datos)
        {
            EntidadNota nota = new EntidadNota(datos);

            //utiliza el método insertarNota de la ControladoraBdNota.
            return controladoraBDNotas.insertarNota(nota);
        }

        /* Envía un mensaje a la ControladoraBDNotas para modifiar un nota.
         * Recibe:
         *      - datosNuevos de tipo Object[], que contiene los datos nuevos con los
         *          que se va a actualizar la información del nota.
         *      - notaViejo de tipo EntidadNota, con los datos viejos del nota.
         * Retorna un vector de String con la información del éxito o no de la modificación.
         */
        public String[] modificarNota(Object[] datosNuevos, EntidadNota notaVieja)
        {
            EntidadNota notaNueva = new EntidadNota(datosNuevos);

            //utiliza el método modificarNota de la ControladoraBdNota.
            return controladoraBDNotas.modificarNota(notaNueva, notaVieja);
        }

        /* Envía un mensaje a la ControladoraBDNotas para eliminar un nota.
         * Recibe:
         *      - datosNuevos de tipo Object[], que contiene los datos nuevos con los
         *          que se va a actualizar la información del nota.
         *      - notaViejo de tipo EntidadNota, con los datos viejos del nota.
         * Retorna un vector de String con la información del éxito o no de la modificación.
         */
        public String[] eliminarNota(EntidadNota eliminado)
        {
            return controladoraBDNotas.eliminarNota(eliminado);
        }

        /* Envía un mensaje a la ControladoraBDNotas para consultar todos los notas.
         * Retorna un DataTable con la información de todos los notas en la base de datos.
         */
        public DataTable consultarNotas()
        {
            return controladoraBDNotas.consultarNotas();
        }



        public EntidadNota consultarNota(String id)
        {
            EntidadNota notaConsultada = null; //para encpasular los datos consultados.
            Object[] datosConsultados = new Object[3]; //para guardar los datos obtenidos de la consulta temporalmente

            //utiliza el método consultarNota de la ControladoraBdNota, el cual recibe la cédula.
            DataTable filaNota = controladoraBDNotas.consultarNota(id);

            if (filaNota.Rows.Count == 1)
            { // si hay un valor
                for (int i = 0; i < 3; i++)
                {
                    // obtiene los atributos y los guarda en datosConsultados
                    datosConsultados[i] = filaNota.Rows[0][i].ToString();
                }

                //Se encapsulan los datos utilizando la clase entidadNota
                notaConsultada = new EntidadNota(datosConsultados);
            }

            //retorna los datos del nota
            return notaConsultada;
        }
    }
}