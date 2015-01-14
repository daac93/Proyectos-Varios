using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SAR.App_Code.DataSets.NotasDataSetsTableAdapters;

namespace SAR.App_Code.Notas
{

    public class ControladoraBDNotas
    {
        NotasTableAdapter adapterNotas;

        //constructor
        public ControladoraBDNotas()
        {
            /*se inicializa una instancia adapter notas
             para poder obtener información de la base de datos.*/
            adapterNotas = new NotasTableAdapter();
        }

        /*Inserta un nota en la base de datos
         * Recibe una entidadProvedor que contiene todos los datos
         * necesarios para insertar un nota.
         * 
         * Retorna un vector de String con información del éxito de la inserción.
         */
        public String[] insertarNota(EntidadNota nuevaNota)
        {

            //para retornar un mensaje de notificación después de intentar un insert en la base de datos.
            String[] resultado = new String[3];

            try
            {
                /* se accesan los atributos de nuevoNota para insertarlos en 
                 * la base de datos usando el Insert del adapterNota
                 */
                this.adapterNotas.Insert(nuevaNota.Titulo, nuevaNota.Contenido);

                /* Al insertar exitosamente, se llena el vector
                 * de resultado con los menajes pertinentes.
                 */
                resultado[0] = "success";
                resultado[1] = "Exito. ";
                resultado[2] = "El nota se ha ingresado exitosamente";
            }
            catch (SqlException e)
            {
                int r = e.Number;

                if (r == 2627)
                {
                    //error = "La cedula esta repetida."
                    resultado[0] = "danger";
                    resultado[1] = "Error. ";
                    resultado[2] = "La cedula ingresada ya existe";
                }
                else
                {
                    /*en cualquier otro caso no se pudo insertar el nota*/
                    resultado[0] = "danger";
                    resultado[1] = "Error. ";
                    resultado[2] = "No se pudo agregar nota";
                }
            }

            //se devuelve el vector que lleva la información del resultado de la inserción.
            return resultado;
        }

        /* Modifica un nota en la base de datos.
         * Recibe dos parámetros del tipo EntidadNota: 
         *      - notaNuevo, contiene los datos nuevos del nota
         *      - notaViejo, contiene los datos viejos del nota
         *      
         * Retorna un vector de String con información del éxito de la inserción.
         */
        public String[] modificarNota(EntidadNota notaNueva, EntidadNota notaVieja)
        {
            //para retornar un mensaje de notificación después de intentar un update en la base de datos.
            String[] resultado = new String[3];

            try
            {
                /* se accesan los atributos de notaNuevo y de notaViejo para actualizarlo en 
                 * la base de datos usando el Update del adapterNota
                 */
                //UPDATE [dbo].[Notas] SET [titulo] = @titulo, [contenido] = @contenido WHERE (([id] = @Original_id) AND ((@IsNull_titulo = 1 AND [titulo] IS NULL) OR ([titulo] = @Original_titulo)) AND ((@IsNull_contenido = 1 AND [contenido] IS NULL) OR ([contenido] = @Original_contenido)))
                this.adapterNotas.Update(notaNueva.Titulo, notaNueva.Contenido, Convert.ToInt64(notaVieja.IdNota), notaVieja.Titulo, notaVieja.Contenido);

                /* Al modificar exitosamente, se llena el vector
                 * de resultado con los menajes pertinentes.
                 */
                resultado[0] = "success";
                resultado[1] = "Exito. ";
                resultado[2] = "Se ha modificado nota";
            }
            catch (SqlException e)
            {
                //error = "No se ha podido modificar el nota.";
                resultado[0] = "danger";
                resultado[1] = "Error. ";
                resultado[2] = "No se pudo modificar el nota";
            }

            //se devuelve el vector que lleva la información del resultado de la modifiación.
            return resultado;
        }

        /* Elimina un nota en la base de datos.
         * Recibe un parámetro del tipo Entidad nota con los datos
         * del nota a eliminar.
         * 
         * Retorna un vector de String con información del éxito de la inserción.
         */
        public String[] eliminarNota(EntidadNota notaBorrar)
        {
            //para retornar un mensaje de notificación después de intentar un update en la base de datos.
            String[] resultado = new String[3];

            try
            {
                /* se accesan los atributos de notaNuevo y de notaViejo para eliminarlo de 
                 * la base de datos usando el Delete del adapterNota
                 */
                this.adapterNotas.Delete(Convert.ToInt64(notaBorrar.IdNota), notaBorrar.Titulo, notaBorrar.Contenido);

                /* Al eliminar exitosamente, se llena el vector
                 * de resultado con los menajes pertinentes.
                 */
                resultado[0] = "success";
                resultado[1] = "Exito. ";
                resultado[2] = "Se elimino exitosamente el nota";

            }
            catch (Exception e)
            {
                // error = "No ha sido posible eliminar el nota.";
                resultado[0] = "danger";
                resultado[1] = "Error. ";
                resultado[2] = "No se pudo eliminar el nota";

            }

            //se devuelve el vector que lleva la información del resultado de la modifiación.
            return resultado;
        }

        /*Consulta todas las notaes existentes en la base de datos.
         retorna un DataTable con la información de todos los nota
         existentes en la base de datos*/
        public DataTable consultarNotas()
        {
            DataTable resultado = new DataTable();

            try
            {
                //utiliza el GetData del adapterNota para hacer la consulta.
                resultado = adapterNotas.GetData();
            }
            catch (Exception e) { }
            //finalmente retorna los datos obtenidos.
            return resultado;
        }

        public DataTable consultarNota(String id)
        {
            DataTable resultado = new DataTable();

            try
            {
                //utiliza consultarFila del adapterNota para consultar el
                resultado = adapterNotas.consultarNota(Convert.ToInt64(id));
            }
            catch (Exception e) { }
            return resultado;
        }
    }
}