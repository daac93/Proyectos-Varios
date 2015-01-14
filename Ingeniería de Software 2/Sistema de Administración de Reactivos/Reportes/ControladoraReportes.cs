using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SAR.App_Code.Reportes {
    public class ControladoraReportes {
        private ControladoraBDReportes controladoraBD;

        /**
         *  Constructor. 
         */
        public ControladoraReportes() {
            controladoraBD = new ControladoraBDReportes();
        }


        /**
         * Ejecuta una query en la base de datos y genera un reporte, sobre los reactivos, en formato .xslx con los datos obtenidos de la query.
         * 
         * @param Object [] datosQuery : los datos que se desean mostrar en el reporte.  Tiene la siguiente estructura:
         *                                  datosQuery[0] : Boolean [] -> contiene cada una de las columnas que se van a mostrar.  
         *                                                                  Tiene mismo orden que "listaCamposDisponibles" en "Reportes.aspx.cs".
         *                                  datosQuery[1] : String [] -> contiene cada uno de los filtros que se va a aplicar a la query.
         *                                  datosQuery[2] : Criterio de agrupación ("", "proveedor", "prueba").
         * 
         * @return ExcelPackage : el paquete Excel que contiene el reporte.
         */
        public ExcelPackage generarReporte(Object [] datosQuery) {
            DataTable datos = controladoraBD.generarReporte(datosQuery);

            if (datos != null) {
                return convertirAExcel(datos, (String)datosQuery[2]);
            } else {
                return null;
            }
        }

        /**
         * Ejecuta una query en la base de datos y genera un reporte, sobre el historial de uso de reactivos, en formato .xslx con los datos obtenidos de la query.
         * 
         * @param fechaInicio : fecha donde comienzan el uso de reactivos.
         * @param fechaFinal : fecha donde termina el uso de reactivos.
         * 
         * @return ExcelPackage : el paquete Excel que contiene el reporte.
         */
        public ExcelPackage generarReporteHistorico(String fechaInicio, String fechaFinal) {
            DataTable datos = controladoraBD.generarReporteHistorico(fechaInicio, fechaFinal);

            if (datos != null) {
                return convertirAExcel(datos, "mes");
            } else {
                return null;
            }
        }

        /**
         * Genera un reporte en formato .xslx con un formato predeterminado.
         * 
         * @param DataTable datos : datos obtenidos de la base de datos que se van a mostrar en el reporte.
         * @param String criterioAgrupacion : criterio mediante el cuál se van a agrupar las tuplas de reactivos (ninguno, por proveedor o por prueba).
         * 
         * @return ExcelPackage : el excel creado.
         */
        private ExcelPackage convertirAExcel(DataTable datos, String criterioAgrupacion) {
            ExcelPackage paquete;
            ExcelWorksheet hojaDeCalculo;
            int cantidadColumnas, cantidadFilas;
            String nombreUltimaColumna, stringUltimaFila;

            cantidadColumnas = datos.Columns.Count;
            cantidadFilas = datos.Rows.Count;
            nombreUltimaColumna = obtenerNombreColumna(cantidadColumnas);
            stringUltimaFila = cantidadFilas.ToString();

            //Crear paquete Excel.
            paquete = new ExcelPackage();

            //Crear hoja de cálculo.
            hojaDeCalculo = paquete.Workbook.Worksheets.Add("Reporte");

            //Cargar "datos" en la hoja de cálculo.
            hojaDeCalculo.Cells ["A1"].LoadFromDataTable(datos, true);

            //Formatear encabezado.
            formatearEncabezado(hojaDeCalculo, nombreUltimaColumna);
            
            //Agrupar tuplas.
            if(criterioAgrupacion != "")    {
                agruparCeldas(hojaDeCalculo, datos, criterioAgrupacion);
            }

            //Agregar bordes.
            aplicarBordesACeldas(hojaDeCalculo, nombreUltimaColumna, cantidadFilas);

            //Autoajustar al contenido todas las celdas.
            aplicarAutoAjusteAContenido(hojaDeCalculo, nombreUltimaColumna, cantidadFilas);

            //Aplicar formato de texto.
            aplicarFormatoTextoAHoja(hojaDeCalculo, nombreUltimaColumna, cantidadFilas);

            return paquete;
        }

        /**
         * Modifica una hoja de Excel y le aplica un formato predeterminado a la primera fila de la hoja.
         * 
         * @param ExcelWorksheet hojaDeCalculo : la hoja de Excel a la que se le va a aplicar el estilo en su encabezado.
         * @param String nombreUltimaColumna : nombre de la última columna donde hay datos en el Excel.
         */
        private void formatearEncabezado(ExcelWorksheet hojaDeCalculo, String nombreUltimaColumna) {
            ExcelRange encabezado;

            encabezado = hojaDeCalculo.Cells ["A1:" + nombreUltimaColumna + "1"];

            encabezado.Style.Font.Bold = true;
            encabezado.Style.Fill.PatternType = ExcelFillStyle.Solid;                      
            encabezado.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            encabezado.Style.Font.Color.SetColor(Color.White);
        }

        /**
         * Modifica una hoja de excel y la aplica un merge a las celdas con el mismo valor en la primero columna,
         * dependiendo de un criterio de agrupación.
         * 
         * @param ExcelWorksheet hojaDeCalculo : la hoja Excel sobre la que se trabaja.
         * @param DataTable datosEnExcel : los datos que se tienen en la hoja de Excel.
         * @param String criterioAgrupacion : el criterio mediante el cuál las celdas se agrupan (Prueba o Proveedor).
         */
        private void agruparCeldas(ExcelWorksheet hojaDeCalculo, DataTable datosEnExcel, String criterioAgrupacion) {
            String columnaParaAgrupar;
            String rangoCeldas;
            int cantidadFilas, ultimaFilaAgrupada;

            ultimaFilaAgrupada = 2;

            if (criterioAgrupacion == "proveedor") {
                columnaParaAgrupar = "Proveedor";
            } else if (criterioAgrupacion == "prueba") {
                columnaParaAgrupar = "Prueba";
            } else {
                columnaParaAgrupar = "Mes";
            }

            var grupos = from tuplas in datosEnExcel.AsEnumerable()
                        group tuplas by tuplas.Field<string>(columnaParaAgrupar)
                            into tuplasAgrupadas
                            orderby tuplasAgrupadas.Key
                            select new { agrupado = tuplasAgrupadas.Key, cantidad = tuplasAgrupadas.Count() };

            foreach (var grupo in grupos) {
                cantidadFilas = grupo.cantidad - 1;
                rangoCeldas = "A" + ultimaFilaAgrupada.ToString() +":" + "A" + (ultimaFilaAgrupada + cantidadFilas).ToString();

                hojaDeCalculo.Cells [rangoCeldas].Merge = true;
                hojaDeCalculo.Cells [rangoCeldas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hojaDeCalculo.Cells [rangoCeldas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ultimaFilaAgrupada += cantidadFilas + 1; 
            }
        }

        /**
         * Aplica la opción "Todos los bordes" a las celdas con datos en un hoja de Excel.
         * 
         * @param ExcelWorksheet hojaDeCalculo : la hoja de Excel sobre la que se trabaja.
         * @param String nombreUltimaColumna : el nombre de la última columna con datos.
         * @param int cantidadFilas : el número de la última fila con datos.
         */
        private void aplicarBordesACeldas(ExcelWorksheet hojaDeCalculo, String nombreUltimaColumna, int cantidadFilas) {
            String cantidadFilasString;
            ExcelRange celdas;

            cantidadFilasString = (cantidadFilas + 1).ToString();

            celdas = hojaDeCalculo.Cells ["A1:" + nombreUltimaColumna + cantidadFilasString];
            celdas.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            celdas.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            celdas.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            celdas.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        /**
         * Aplica el formato "General" a todas las celdas con datos en una hoja de Excel.
         * 
         * @param ExcelWorksheet hojaDeCalculo : la hoja de Excel sobre la que se trabaja.
         * @param String nombreUltimaColumna : el nombre de la última columna con datos.
         * @param int cantidadFilas : el número de la última fila con datos.
         */
        private void aplicarFormatoTextoAHoja(ExcelWorksheet hojaDeCalculo, String nombreUltimaColumna, int cantidadFilas) {
            String cantidadFilasString;
            cantidadFilasString = (cantidadFilas + 1).ToString();
            
            hojaDeCalculo.Cells ["A1:A2"].Style.Numberformat.Format = "@";
        }

        /**
         * Aplica la opción "Autoajustar al contenido" a todas las celdas con datos en una hoja de Excel.
         * 
         * @param ExcelWorksheet hojaDeCalculo : la hoja de Excel sobre la que se trabaja.
         * @param String nombreUltimaColumna : el nombre de la última columna con datos.
         * @param int cantidadFilas : el número de la última fila con datos.
         */
        private void aplicarAutoAjusteAContenido(ExcelWorksheet hojaDeCalculo, String nombreUltimaColumna, int cantidadFilas) {
            String cantidadFilasString;

            cantidadFilasString = (cantidadFilas + 1).ToString();

            hojaDeCalculo.Cells ["A1:" + nombreUltimaColumna + cantidadFilasString].AutoFitColumns();
        }

        /**
         * Transforma un número de columna en un nombre de columna de Excel. 
         * (1 = A, 2 = B, 3 = C, ...)
         * 
         * @param int numeroColumna : el número de la columna a transformar.
         * @return String : elnombre de la columna.
         */
        private String obtenerNombreColumna(int numeroColumna) {
            int dividendo = numeroColumna;
            String nombreColumna = "";
            int modulo;

            while (dividendo > 0) {
                modulo = (dividendo - 1) % 26;
                nombreColumna = Convert.ToChar(65 + modulo).ToString() + nombreColumna;
                dividendo = (int)((dividendo - modulo) / 26);
            }

            return nombreColumna;
        }
    }
} 