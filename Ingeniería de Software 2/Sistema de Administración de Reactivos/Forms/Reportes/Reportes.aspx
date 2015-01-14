<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="SAR.Forms.Reportes.Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">

    <!--Encabezado de la página y botones de la funcionalidad-->
    <div class="page-header">
        <div class="row">
            <div class="col-lg-11">
                <h1><i class="fa fa-file-text"></i>Reportes</h1>
            </div>
            <div class="col-lg-1">
                <h2><a id="informacion" href="#modalInformacion" data-toggle="modal" runat="server"><i class="fa fa-question-circle text-info"></i></a></h2>
            </div>
        </div>
        <div class="row">
            <div id="alertAlerta" class="alert alert-danger fade in" runat="server" hidden="hidden">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                <strong>
                    <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label></strong><asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
            </div>
        </div>
    </div>

    <!-- Filtros -->
    <div class="row">
        <div class="col-md-8">
            <h3>Reporte de Inventario</h3>
            <hr />
                <div class="col-md-4">
                    <fieldset>
                        <legend>Filtros</legend>
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:CheckBox ID="checkCodigo" runat="server" />
                            </span>
                            <input runat="server" type="text" class="form-control" id="textCodigo" placeholder="Código" />
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:CheckBox ID="checkNombre" runat="server" />
                            </span>
                            <input runat="server" type="text" class="form-control" id="textNombre" placeholder="Nombre" />
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">
                                <label>&nbsp;</label>
                            </span>
                            <asp:DropDownList ID="comboPrueba" class="form-control" runat="server">
                                <asp:ListItem>--Seleccione--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">
                                <a id="A1" href="#modalCroquis" data-toggle="modal" runat="server"><i class="fa fa-search fa-fw linkBlanco"></i></a>
                            </span>
                            <asp:DropDownList ID="comboUbicacion" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:CheckBox ID="checkActivo" runat="server" Checked="true" />
                            </span>
                            <input runat="server" type="text" class="form-control" disabled="disabled" id="text1" placeholder="Activo" />
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:CheckBox ID="checkMostrarTodos" runat="server" Checked="false" />
                            </span>
                            <input runat="server" type="text" class="form-control" disabled="disabled" id="text2" placeholder="Mostrar todos" />
                        </div>
                        <br />
                    </fieldset>
                </div>

                <!-- Filtros para columnas -->
                <div class="col-md-4" id="columnas" runat="server">
                    <div id="columnas2" runat="server">
                        <fieldset id="fieldset" runat="server">
                            <legend>Campos Deseados</legend>
                            <label class="checkbox">
                                <input type="checkbox" id="checkTodos" value="0" runat="server" onserverchange="setEstadoCheckboxesRestantes" checked="checked" />
                                Seleccionar todos
                            </label>
                            <label class="checkbox" id="labelCodigoReactivo" runat="server">
                                <input type="checkbox" id="checkCodigoReactivo" value="1" runat="server" />
                                Código
                            </label>
                            <label class="checkbox" id="labelNombreReactivo" runat="server">
                                <input type="checkbox" id="checkNombreReactivo" value="2" runat="server" />
                                Nombre
                            </label>
                            <label class="checkbox" id="labelDescripcionReactivo" runat="server">
                                <input type="checkbox" id="checkDescripcionReactivo" value="3" runat="server" />
                                Descripción
                            </label>
                            <label class="checkbox" id="labelUbicacionReactivo" runat="server">
                                <input type="checkbox" id="checkUbicacionReactivo" value="4" runat="server" />
                                Ubicación
                            </label>
                            <label class="checkbox" id="labelCantidadMuestraReactivo" runat="server">
                                <input type="checkbox" id="checkCantidadMuestraReactivo" value="5" runat="server" />
                                Cantidad de muestra
                            </label>
                            <label class="checkbox" id="labelUnidadMuestraReactivo" runat="server">
                                <input type="checkbox" id="checkUnidadMuestraReactivo" value="6" runat="server" />
                                Unidad de muestra
                            </label>
                            <label class="checkbox" id="labelCantidadReactivo" runat="server">
                                <input type="checkbox" id="checkCantidadReactivo" value="7" runat="server" />
                                Cantidad de reactivo
                            </label>
                            <label class="checkbox" id="labelUnidadReactivo" runat="server">
                                <input type="checkbox" id="checkUnidadReactivo" value="8" runat="server" />
                                Unidad de reactivo
                            </label>
                            <label class="checkbox" id="labelRestanteReactivo" runat="server">
                                <input type="checkbox" id="checkRestanteReactivo" value="9" runat="server" />
                                Restante
                            </label>
                            <label class="checkbox" id="labelVenceReactivo" runat="server">
                                <input type="checkbox" id="checkVenceReactivo" value="10" runat="server" />
                                Vencimiento
                            </label>
                            <label class="checkbox" id="labelActivoReactivo" runat="server">
                                <input type="checkbox" id="checkActivoReactivo" value="11" runat="server" />
                                Activo
                            </label>
                        </fieldset>
                    </div>
                </div>

                <!-- Agrupacion -->
                <div class="col-md-4">
                    <div class="row">
                        <fieldset>
                            <legend>Agrupar por</legend>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="optionsRadios" id="radioAgruparNinguno" value="0" runat="server" />
                                    Ninguno
                                </label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="optionsRadios" id="radioAgruparProveedor" value="1" runat="server" />
                                    Proveedor
                                </label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="optionsRadios" id="radioAgruparPrueba" value="2" runat="server" />
                                    Prueba
                                </label>
                            </div>
                        </fieldset>
                    </div>
                    <br />
                    <div class="row">
                        <div class="text-center">
                            <button type="submit" id="boton" runat="server" class="btn btn-primary" onserverclick="generarReporte"><i class="fa fa-download"></i> Reporte de Inventario</button>
                        </div>
                    </div>
                </div>
            </div>

        <!--Reporte Historico-->
        <div class="col-md-4">
            <h3>Reporte Histórico</h3>
            <hr />
            <fieldset>
                <legend>Rango de fechas</legend>
                <div class="input-group">
                    <label for="textFechaInicio" class="col-sm-4 control-label">Inicio: </label>
                    <div class="col-sm-8">
                        <div class=" input-group margin-bottom-sm">
                            <input id="textFechaInicio" class="form-control" runat="server" type="text" placeholder="mm/aaaa"/>
                            <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                        </div>
                    </div>
                </div>
                <br />
                <div class="input-group">
                    <label for="textFechaFinal" class="col-sm-4 control-label">Final: </label>
                    <div class="col-sm-8">
                        <div class=" input-group margin-bottom-sm">
                            <input id="textFechaFinal" class="form-control" runat="server" type="text" placeholder="mm/aaaa"/>
                            <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="text-center">
                        <button type="submit" id="botonReporteHistorico" runat="server" class="btn btn-primary" onserverclick="generarReporteHistorico"><i class="fa fa-download"></i> Reporte Histórico</button>
                    </div>
                </div>
                <br /><br /><br />
                <div class="form-group">
                    <span class="label label-primary pull-right"><i class="fa fa-check fa-fw"></i>Espacio requerido</span>
                </div>
            </fieldset>
        </div>
    </div>


    <!--Modal Información-->
    <div class="modal fade" id="modalInformacion" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="H2"><i class="fa fa-question-circle text-info fa-2x"></i>¿Cómo usar Reporte de Inventario?</h4>
                </div>
                <div class="modal-body">

                    <div class="process">
                        <div class="process-row">
                            <div class="process-step">
                                <button type="button" class="btn btn-info btn-circle" disabled="disabled"><i class="fa fa-search fa-3x"></i></button>
                                <p>Filtre los reactivos dependiendo de lo que desee ver en el reporte</p>
                            </div>

                            <div class="process-step">
                                <button type="button" class="btn btn-info  btn-circle" disabled="disabled"><i class="fa fa-check-square-o fa-3x"></i></button>
                                <p>Seleccione que campos desea incluir en el reporte</p>
                            </div>
                            <div class="process-step">
                                <button type="button" class="btn btn-info  btn-circle" disabled="disabled"><i class="fa fa-th-large fa-3x"></i></button>
                                <p>Seleccione el tipo de agrupación de los reactivos</p>
                            </div>
                            <div class="process-step">
                                <button type="button" class="btn btn-success btn-circle" disabled="disabled"><i class="fa fa-download fa-3x"></i></button>
                                <p>Presione generar para obtener el reporte</p>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="Button3" class="btn btn-success" data-dismiss="modal">Aceptar</button>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal ubicación-->
    <div class="modal fade" id="modalCroquis" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Mapa del Laboratorio</h4>
                </div>
                <div class="modal-body modal-body-croquis">
                    <div>
                        <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 2115 1060">
                            <style type="text/css">
                                polygon, rect {
                                    cursor: pointer;
                                    pointer-events: visiblePainted;
                                    fill: #4e5d6c;
                                    stroke-width: 2,5;
                                    stroke: white;
                                    stroke-linecap: round;
                                    -webkit-transition: all 0.5s;
                                    -moz-transition: all 2s;
                                    transition: all 0.5s;
                                }

                                .text-blanco {
                                    fill: #ffffff;
                                    font-size: 48px;
                                }

                                text {
                                    fill: white;
                                    font-size: 48px;
                                    cursor: pointer;
                                    pointer-events: visiblePainted;
                                    -webkit-transition: all 0.5s;
                                    -moz-transition: all 0.5s;
                                    transition: all 0.5s;
                                }

                                g:hover text {
                                    font-size: 56px;
                                }

                                g:hover rect {
                                    fill: #bf5a16;
                                }

                                g:hover polygon {
                                    fill: #bf5a16;
                                }

                                g:active rect {
                                }
                            </style>
                            <g>
                                <polygon points="1,1 1,454 390,454 390,1055, 2105,1055, 2105,1" style="fill: #2b3e50; stroke-width: 5; stroke: black;" />
                            </g>
                            <g>
                                <rect id="1" x="7" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="70" y="90">1</text>
                            </g>
                            <g>
                                <rect id="2" x="270" y="7" width="240" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="380" y="90">2</text>
                            </g>
                            <g>
                                <rect id="3" x="510" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="575" y="90">3</text>
                            </g>
                            <g>
                                <rect id="4" x="660" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="725" y="90">4</text>
                            </g>
                            <g>
                                <rect id="5" x="810" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="875" y="90">5</text>
                            </g>
                            <g>
                                <rect id="6" x="960" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1025" y="90">6</text>
                            </g>
                            <g>
                                <rect id="7" x="1110" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1175" y="90">7</text>
                            </g>
                            <g>
                                <rect id="8" x="1260" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1325" y="90">8</text>
                            </g>
                            <g>
                                <rect id="9" x="1410" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1475" y="90">9</text>
                            </g>
                            <g>
                                <rect id="10" x="1560" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1615" y="90">10</text>
                            </g>
                            <g>
                                <rect id="11" x="1710" y="7" width="240" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1800" y="90">11</text>
                            </g>
                            <g>
                                <rect id="12" x="1950" y="7" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="2000" y="90">12</text>
                            </g>
                            <g>
                                <rect id="13" x="5" y="300" width="270" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="110" y="385">13</text>
                            </g>
                            <g>
                                <rect x="275" y="375" width="120" height="75" />
                            </g>
                            <g>
                                <rect id="21" x="510" y="300" width="150" height="450" onclick="elegirUbicacion(this.id)" />
                                <text x="565" y="555">21</text>
                            </g>
                            <g>
                                <polygon id="22" points="660,300 660,750 885,750 885,600 810,600 810,300" onclick="elegirUbicacion(this.id)" />
                                <text x="715" y="555">22</text>
                            </g>
                            <g>
                                <polygon id="23" points="885,600, 885,750, 1110,750, 1110,300 960,300 960,600" onclick="elegirUbicacion(this.id)" />
                                <text x="1015" y="555">23</text>
                            </g>
                            <g>
                                <polygon id="24" points="1110,300 1110,750 1335,750 1335,600 1260,600 1260,300" onclick="elegirUbicacion(this.id)" />
                                <text x="1165" y="555">24</text>
                            </g>
                            <g>
                                <polygon id="25" points="1335,600, 1335,750, 1560,750, 1560,300 1410,300 1410,600" onclick="elegirUbicacion(this.id)" />
                                <text x="1465" y="555">25</text>
                            </g>
                            <g>
                                <rect id="14" x="395" y="975" width="120" height="75" onclick="elegirUbicacion(this.id)" />
                                <text x="430" y="1025">14</text>
                            </g>
                            <g>
                                <rect id="15" x="660" y="900" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="715" y="985">15</text>
                            </g>
                            <g>
                                <rect id="16" x="810" y="900" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="865" y="985">16</text>
                            </g>
                            <g>
                                <rect id="17" x="960" y="900" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1015" y="985">17</text>
                            </g>
                            <g>
                                <rect id="18" x="1110" y="900" width="150" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1165" y="985">18</text>
                            </g>
                            <g>
                                <rect id="19" x="1260" y="900" width="300" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1375" y="985">19</text>
                            </g>
                            <g>
                                <rect x="1560" y="900" width="150" height="150" />
                            </g>
                            <g>
                                <rect id="ubicacionOficina" x="1710" y="650" width="390" height="250" onclick="elegirUbicacion(this.id)" />
                                <text x="1825" y="800">Oficina</text>
                            </g>
                            <g>
                                <rect id="20" x="1710" y="900" width="390" height="150" onclick="elegirUbicacion(this.id)" />
                                <text x="1875" y="985">20</text>
                            </g>
                            <text class="text-blanco" x="10" y="220">Entrada </text>
                            <text class="text-blanco" x="10" y="260">Principal</text>
                            <text class="text-blanco" x="515" y="1000">Entrada </text>
                            <text class="text-blanco" x="530" y="1040">Norte</text>
                        </svg>
                    </div>
                </div>
                <div class="modal-footer">
                    <br />
                </div>
            </div>
        </div>
    </div>

    <!--Permite mapear la ubicación seleccionada en el croquis a el combobox-->
    <script>
        function elegirUbicacion(idUbicacion) {
            document.getElementById('<%=comboUbicacion.ClientID %>').selectedIndex = idUbicacion;
            $('#modalCroquis').modal('hide')
        }
    </script>
    <script>
        $(function () {
            var hoy = new Date();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();
            if (mm < 10) {
                mm = '0' + mm
            }
            today = mm + '/' + yyyy;
            $("#ContenidoPrincipal_textFechaInicio").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'mm/yy',
                showAnim: 'show',
                monthNamesShort: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
                onClose: function (dateText, inst) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1));
                }
            });

            $("#ContenidoPrincipal_textFechaFinal").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'mm/yy',
                showAnim: 'show',
                monthNamesShort: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
                onClose: function (dateText, inst) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).datepicker('setDate', new Date(year, month, 1));
                }
            });
        });
    </script>
    <style>
        .ui-datepicker-calendar {
            display: none;
        }
</style>
</asp:Content>
