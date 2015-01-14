<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="SAR.Forms.Usuarios.Usuarios" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <script src="../../Estilo/js/validator.js"></script>

    <!--Encabezado de la página y botones de la funcionalidad-->
    <div class="page-header">
        <div class="row">
            <div class="col-lg-11">
                <h1><i class="fa fa-users"></i>Usuarios</h1>
            </div>
            <div class="col-lg-1">
                <h2><a id="informacion" href="#modalInformacion" data-toggle="modal" runat="server"><i class="fa fa-question-circle text-info"></i></a></h2>
            </div>
        </div>
    </div>
    <div class="row row-botones">
        <div class="col-lg-5">
            <button runat="server" onserverclick="clickInsertar" id="botonAgregar" class="btn btn-primary" type="button"><i class="fa fa-plus"></i>Agregar</button>
            <button runat="server" onserverclick="clickModificar" id="botonModificar" class="btn btn-primary" type="button"><i class="fa fa-pencil-square-o"></i>Modificar</button>
            <a id="botonEliminar" href="#modalEliminar" class="btn btn-primary" role="button" data-toggle="modal" runat="server"><i class="fa fa-trash-o fa-lg"></i>Eliminar</a>
            <a id="botonConsultar" href="#modalConsultar" data-toggle="modal" runat="server" class="btn btn-primary"><i class="fa fa-search fa-lg"></i>Consultar</a>
        </div>
        <div class="col-lg-8">
            <div id="alertAlerta" class="alert alert-danger fade in" runat="server" hidden="hidden">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                <strong>
                    <asp:Label ID="labelTipoAlerta" runat="server" Text="Alerta! "></asp:Label></strong><asp:Label ID="labelAlerta" runat="server" Text="Mensaje de alerta"></asp:Label>
            </div>
        </div>
    </div>

    <!--Datos personales-->
    <div class="row">
        <div class="col-lg-6">
            <div class="well bs-component">
                <fieldset>
                    <legend>Datos Personales</legend>
                    <div class="form-group">
                        <label for="textNombre" class="col-sm-3 control-label">Nombre: </label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <input runat="server" id="textNombre" class="form-control" type="text" placeholder="Nombre" data-error="Nombre inválido" title="Nombre" pattern="^([A-Za-zÁÉÍÓÚáéíóúñÑ]+[\x20]?)*$" data-minlength="3" maxlength="12" required="required" />
                                <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="textApellido1" class="col-sm-3 control-label">Apellidos: </label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <input runat="server" id="textApellido1" class="form-control" type="text" placeholder="Primer Apellido" data-error="Apellido inválido" title="Primer Apellido" pattern="^([A-Za-zÁÉÍÓÚáéíóúñÑ]+[\x20]?)*$" data-minlength="4" maxlength="20" required="required" />
                                <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="textApellido2" class="col-sm-3 control-label"></label>
                        <div class="col-sm-8">
                            <input runat="server" id="textApellido2" class="form-control" type="text" placeholder="Segundo Apellido" data-error="Apellido inválido" title="Segundo Apellido" pattern="^([A-Za-zÁÉÍÓÚáéíóúñÑ]+[\x20]?)*$" data-minlength="4" maxlength="20" />
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="textTelefono" class="col-sm-3 control-label">Teléfono: </label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <input runat="server" id="textTelefono" class="form-control" type="tel" placeholder="Teléfono" data-error="Número de teléfono inválido" title="telefono" pattern="^[0-9]*$" data-minlength="8" maxlength="12" required="required" />
                                <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="textEmail" class="col-sm-3 control-label">E-mail: </label>
                        <div class="col-sm-8">
                            <input runat="server" id="textEmail" class="form-control" type="email" placeholder="E-mail" data-error="Correo inválido" title="email" pattern="[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?" maxlength="50"/>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <span class="label label-primary pull-right"><i class="fa fa-check fa-fw"></i>Espacio requerido</span>
                    </div>
                </fieldset>
            </div>
        </div>
        <!--Datos de la cuenta-->
        <div class="col-lg-6">
            <div class="well bs-component">
                <fieldset>
                    <legend>Datos de la Cuenta</legend>
                    <div class="form-group">
                        <label for="textUsername" class="col-sm-3 control-label">Usuario: </label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <input runat="server" id="textUsername" class="form-control" type="text" placeholder="Nombre de Usuario" data-error="Nombre de usuario inválido." title="Nombre de Usuario" pattern="^[a-zA-Z\.\-0-9]*$" data-minlength="4" maxlength="12" required="required" />
                                <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="textPassword" class="col-sm-3 control-label">Contraseña: </label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <input runat="server" id="textPassword" data-toogle="validator" class="form-control" type="password" placeholder="Contraseña" title="Contraseña" pattern="(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$" data-error="La contraseña debe contener al menos 6 caracteres y contener una mayúscula, minúscula y un número." data-minlength="6" maxlength="20" required="required" />
                                <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="textConfirmarPassword" class="col-sm-3 control-label"></label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <input runat="server" id="textConfirmarPassword" class="form-control" type="password" placeholder="Confirme la contraseña" data-match="#ContenidoPrincipal_textPassword" data-match-error="Las contraseña no coinciden." required="required" />
                                <span class="input-group-addon"><i class="fa fa-check fa-fw"></i></span>
                            </div>
                            <div class="help-block with-errors"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="comboRol" class="col-sm-3 control-label">Rol: </label>
                        <div class="col-sm-9">
                            <div class=" input-group margin-bottom-sm">
                                <asp:DropDownList ID="comboRol" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="text-center">
                    <button runat="server" id="botonAceptar" class="btn btn-success" type="submit" onserverclick="clickAceptar">Aceptar</button>
                    <button runat="server" id="botonCancelar" class="btn btn-danger" type="reset" onserverclick="clickCancelar">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
    <!--Modal Consultar-->
    <div class="modal fade" id="modalConsultar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="H1"><i class="fa fa-search"></i>Consultar Pruebas</h4>
                </div>
                <div class="modal-body">
                    <div class="col-lg-12">
                        <asp:UpdatePanel ID="UpdatePanelPruebas" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gridViewUsuarios" CssClass="table able-responsive table-condensed" OnRowCommand="gridViewUsuarios_RowCommand" OnPageIndexChanging="gridViewUsuarios_PageIndexChanging" runat="server" AllowPaging="true" PageSize="15" BorderColor="Transparent">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn-default" CommandName="Select" Text="Consultar" />
                                    </Columns>
                                    <RowStyle Font-Size="small" BackColor="White" ForeColor="Black" />
                                    <PagerStyle CssClass="paging" HorizontalAlign="Center" />
                                    <AlternatingRowStyle BackColor="#EBEBEB" />
                                    <SelectedRowStyle CssClass="info" Font-Bold="true" ForeColor="White" />
                                    <HeaderStyle CssClass="active" Font-Size="Medium" Font-Bold="true" />
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gridViewUsuarios" EventName="RowCommand" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="Button1" class="btn btn-danger" data-dismiss="modal" onserverclick="cancelarConsultar">Cancelar</button>
                </div>
            </div>
        </div>
    </div>


    <!--Modal Eliminar-->
    <div class="modal fade" id="modalEliminar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel"><i class="fa fa-exclamation-triangle text-danger fa-2x"></i>Confirmar eliminación</h4>
                </div>
                <div class="modal-body">
                    ¿Está seguro que desea eliminar el usuario seleccionado?
                </div>
                <div class="modal-footer">
                    <button type="button" id="botonCancelarModal" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="button" id="botonAceptarModal" class="btn btn-success" runat="server" onserverclick="clickAceptarEliminar" data-dismiss="modal">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <!--Modal Información-->
    <div class="modal fade" id="modalInformacion" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="H2"><i class="fa fa-question-circle text-info fa-2x"></i>¿Cómo usar Usuarios?</h4>
                </div>
                <div class="modal-body">

                    <div class="process">
                        <div class="process-row">
                            <div class="process-step">
                                <button type="button" class="btn btn-info  btn-circle" disabled="disabled"><i class="fa fa-user fa-3x"></i></button>
                                <p>Ingrese los datos personales de la persona</p>
                            </div>
                            <div class="process-step">
                                <button type="button" class="btn btn-info  btn-circle" disabled="disabled"><i class="fa fa-key fa-3x"></i></button>
                                <p>Ingrese los datos de la cuenta de usuario</p>
                            </div>
                            <div class="process-step">
                                <button type="button" class="btn btn-success btn-circle" disabled="disabled"><i class="fa fa-users fa-3x"></i></button>
                                <p>Presione aceptar para guardar la cuenta de usuario</p>
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
</asp:Content>

