﻿$(document).ready(function () {

    $('.dfech').datepicker({
        dateFormat: "dd/mm/yy"
    });

    //listado de clientes por autocomplete
    $("#sdesc_cliente").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetCliente, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { sdescCliente: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.srazon_cliente, value: item.srazon_cliente, id: item.ncode_cliente
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#COD_CLIENTE').val(ui.item.id);
            fnclienteDire();
        }
    });

    $("#ncode_gere").change(function () {
        $("#ncode_orar").empty();
        $.ajax({
            type: 'POST',
            url: url,
            dataType: 'json',
            data: { ncode_gere: $("#ncode_gere").val() },
            success: function (areas) {
                $.each(areas, function (i, area) {
                    $("#ncode_orar").append('<option value="'
                        + area.ncode_orar + '">'
                        + area.sdesc_orar + '</option>');
                });
            },
            error: function (ex) {
                alert('No se pueden recuperar las areas.' + ex);
            }
        });
        return false;
    });

    $("#subigeo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetUbigeo, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { sdescUbigeo: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.direccion, value: item.direccion, id: item.codigo
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#scode_ubigeo').val(ui.item.id);
        }
    });

    $("#btnCliente").click(function () {
        fnclienteNuevo();
    });

    $("#btnCliCierre").click(function () {
        $("#srazon_cliente").val("");
        $("#sruc_cliente").val("");
        $("#sdnice_cliente").val("");
        $("#sdire_cliente").val("");
        $("#subigeo_cliente").val("");
        $("#sfono1_cliente").val("");
        $("#smail_cliente").val("");
        $("#srepre_cliente").val("");
        $("#sfono2_cliente").val("");
        $("#sappa_cliente").val("");
        $("#sapma_cliente").val("");
        $("#snomb_cliente").val("");
    });




});

function fnclienteDire() {
    $("#NRO_DCLIENTE").empty();
    $.ajax({
        type: 'POST',
        url: urlGetClienteDire,
        dataType: 'json',
        data: { scodCliente: $("#COD_CLIENTE").val() },
        success: function (areas) {
            $.each(areas, function (i, area) {
                $("#NRO_DCLIENTE").append('<option value="'
                    + area.ncode_clidire + '">'
                    + area.sdesc_clidire + '</option>');
            });
        },
        error: function (ex) {
            alert('No se pueden recuperar las direcciones.' + ex);
        }
    });
    return false;

}

function fnclienteNuevo() {

    var valor = $('input:radio[name=stipo_cliente]:checked').val();

    if (valor === 'J') {

        if ($("#srazon_cliente").val().length < 1) {
            alert("Ingrese Razon Social");
            return false;
        };

        if ($("#sruc_cliente").val().length < 1) {
            alert("Ingrese RUC");
            return false;
        };

    }
    else {
        if ($("#sappa_cliente").val().length < 1) {
            alert("Ingrese A.Paterno");
            return false;
        };

        if ($("#sdnice_cliente").val().length < 1) {
            alert("Ingrese DNI");
            return false;
        };

    }

    if ($("#sdire_cliente").val().length < 1) {
        alert("Ingrese Direccion");
        return false;
    };

    if ($("#scode_ubigeo").val().length < 1) {
        alert("Ingrese Ubicacion");
        return false;
    };


    var clienteView = {
        "srazon_cliente": "", "sruc_cliente": "", "sdnice_cliente": "",
        "sdire_cliente": "", "subigeo_cliente": "", "sfono1_cliente":"",
        "smail_cliente": "", "srepre_cliente": "", "sfono2_cliente": "",
        "stipo_cliente": "", "sappa_cliente": "", "sapma_cliente" : "",
        "snomb_cliente" : ""
    };

    // Setear valores
    clienteView.srazon_cliente = $("#srazon_cliente").val();
    clienteView.sruc_cliente = $("#sruc_cliente").val();
    clienteView.sdnice_cliente = $("#sdnice_cliente").val();
    clienteView.sdire_cliente = $("#sdire_cliente").val();
    clienteView.subigeo_cliente = $("#subigeo_cliente").val();
    clienteView.sfono1_cliente = $("#sfono1_cliente").val();
    clienteView.smail_cliente = $("#smail_cliente").val();
    clienteView.srepre_cliente = $("#srep_cliente").val();
    clienteView.sfono2_cliente = $("#sfono2_cliente").val();
    clienteView.stipo_cliente = $('input:radio[name=stipo_cliente]:checked').val();
    clienteView.sappa_cliente = $("#sappa_cliente").val();
    clienteView.sapma_cliente = $("#sapma_cliente").val();
    clienteView.snomb_cliente = $("#snomb_cliente").val();

    if (clienteView.tipo_contribuyente == 'N') {
        clienteView.srazon_cliente = clienteView.snomb_cliente + ' ' + clienteView.sappa_cliente + ' ' + clienteView.sapma_cliente;
    }

     ///alert($('input:radio[name=tipo_contribuyente]:checked').val());

    //console.log(profView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlClienteNuevo,
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(clienteView) },
        success: function (result) {
            if (result.Success == "1") {
                alert('Registro Exitoso');
            }
            else {
                alert('No se puede registrar cliente.');
            }
        },
        error: function (ex) {
            alert('No se puede registrar cliente.' + ex);
        }
    });


}

