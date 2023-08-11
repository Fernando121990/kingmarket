var editor;

$(document).ready(function () {

    $('.dfech').datepicker({
        language: 'es',
        format: 'dd/mm/yyyy'
//        dateFormat: "dd/mm/yy"
    });

    $('.dtbl').DataTable();

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
                            label: item.srazon_cliente, value: item.srazon_cliente, id: item.ncode_cliente,
                            rucid: item.sruc_cliente, dnid: item.sdnice_cliente, fopagoid: item.ncode_fopago,
                            vendeid : item.ncode_vende, almaid : item.ncode_alma
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            //console.log('cliente');
            //console.log(ui.item.vendeid);
            //console.log(ui.item.almaid);
            //console.log(ui.item.fopagoid);

            $('#COD_CLIENTE').val(ui.item.id);
            $("#sruc_cliente").val(ui.item.rucid);
            $('#sdni_cliente').val(ui.item.dnid);
            //$('#ncode_fopago').val(ui.item.fopagoid);
            $('#ncode_vende').val(ui.item.vendeid);
            $('#ncode_alma').val(ui.item.almaid);
            fnclienteDire();
            fnclienteFPago();
            fnFormaPagoDiasFecha(ui.item.fopagoid);
        }
    });

    $("#sruc_cliente").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetRucCliente, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { srucCliente: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.sruc_cliente, value: item.sruc_cliente, id: item.ncode_cliente,
                            razon: item.srazon_cliente, dnid: item.sdnice_cliente, fopagoid: item.ncode_fopago,
                            vendeid: item.ncode_vende, almaid: item.ncode_alma
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#COD_CLIENTE').val(ui.item.id);
            $('#sdesc_cliente').val(ui.item.razon);
            $('#sdni_cliente').val(ui.item.dnid);
            //$('#ncode_fopago').val(ui.item.fopagoid);
            $('#ncode_vende').val(ui.item.vendeid);
            $('#ncode_alma').val(ui.item.almaid);
            fnclienteDire();
            fnclienteFPago();
            fnFormaPagoDiasFecha(ui.item.fopagoid);
        }
    });

    $("#sdni_cliente").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetDniCliente, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { sdniCliente: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.sdnice_cliente, value: item.sdnice_cliente, id: item.ncode_cliente,
                            razon: item.srazon_cliente, rucid: item.sruc_cliente, fopagoid: item.ncode_fopago,
                            vendeid: item.ncode_vende, almaid: item.ncode_alma
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#COD_CLIENTE').val(ui.item.id);
            $('#sdesc_cliente').val(ui.item.razon);
            $('#sruc_cliente').val(ui.item.rucid);
            //$('#ncode_fopago').val(ui.item.fopagoid);
            $('#ncode_vende').val(ui.item.vendeid);
            $('#ncode_alma').val(ui.item.almaid);
            fnclienteDire();
            fnclienteFPago();
            fnFormaPagoDiasFecha(ui.item.fopagoid);
        }
    });

    //listado de proveedores
    $("#sdesc_prove").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetProveedor, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { sdescProvee: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.sdesc_prove, value: item.sdesc_prove, id: item.ncode_provee,
                            ruc: item.sruc_prove
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            //$('#COD_PROVE').val(ui.item.id);
            $('#ncode_provee').val(ui.item.id);
            $('#sruc_prove').val(ui.item.ruc);
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
        $("#scode_ubigeo").val("");
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

function fnclienteFPago() {
    $("#nro_fopago").empty();
    $.ajax({
        type: 'POST',
        url: urlgetClienteFoPago,
        dataType: 'json',
        data: { scodCliente: $("#COD_CLIENTE").val() },
        success: function (areas) {
            $.each(areas, function (i, area) {
                $("#nro_fopago").append('<option value="'
                    + area.ncode_fopago + '">'
                    + area.sdesc_fopago + '</option>');
            });
        },
        error: function (ex) {
            alert('No se pueden recuperar las formas de pago del cliente.' + ex);
        }
    });
    //$("#ncode_fopago").val(codfpago);
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
        "sdire_cliente": "", "scode_ubigeo": "", "sfono1_cliente":"",
        "smail_cliente": "", "srepre_cliente": "", "sfono2_cliente": "",
        "stipo_cliente": "", "sappa_cliente": "", "sapma_cliente" : "",
        "snomb_cliente" : ""
    };

    // Setear valores
    clienteView.srazon_cliente = $("#srazon_cliente").val();
    clienteView.sruc_cliente = $("#sruc_cliente").val();
    clienteView.sdnice_cliente = $("#sdnice_cliente").val();
    clienteView.sdire_cliente = $("#sdire_cliente").val();
    clienteView.scode_ubigeo = $("#scode_ubigeo").val();
    clienteView.sfono1_cliente = $("#sfono1_cliente").val();
    clienteView.smail_cliente = $("#smail_cliente").val();
    clienteView.srepre_cliente = $("#srep_cliente").val();
    clienteView.sfono2_cliente = $("#sfono2_cliente").val();
    clienteView.stipo_cliente = $('input:radio[name=stipo_cliente]:checked').val();
    clienteView.sappa_cliente = $("#sappa_cliente").val();
    clienteView.sapma_cliente = $("#sapma_cliente").val();
    clienteView.snomb_cliente = $("#snomb_cliente").val();

    if (clienteView.stipo_cliente == 'N') {
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
                alert(result.mensaje);
            }
        },
        error: function (ex) {
            alert('No se puede registrar cliente.' + ex);
        }
    });


}

