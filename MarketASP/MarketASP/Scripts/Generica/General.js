$(document).ready(function () {

    $('.dfech').datepicker({
        dateFormat: "dd/mm/yy"
    });

    //carga en proforma
    fnClienteNatural($("#tipo_contribuyente").val());
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
                            label: item.DESC_CLIENTE, value: item.DESC_CLIENTE, id: item.COD_CLIENTE
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
            $('#subigeo_cliente').val(ui.item.id);
        }
    });

    $("#btnCliente").click(function () {
        fnclienteNuevo();
    });

    $("#btnCliCierre").click(function () {
        $("#srazon_cliente").val("");
        $("#sruc_cliente").val("");
        $("#sdni_cliente").val("");
        $("#sdire_cliente").val("");
        $("#subigeo_cliente").val("");
        $("#sfono_cliente").val("");
        $("#semail_cliente").val("");
        $("#srep_cliente").val("");
        $("#sfono2_cliente").val("");
        $("#ape_pat_cliente").val("");
        $("#ape_mat_cliente").val("");
        $("#nombres_cliente").val("");
    });



    $("input[name$='tipo_contribuyente']").click(function () {
        var test = $(this).val();

        fnClienteNatural(test);

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
                    + area.NRO_DCLIENTE + '">'
                    + area.DIR_DCLIENTE + '</option>');
            });
        },
        error: function (ex) {
            alert('No se pueden recuperar las direcciones.' + ex);
        }
    });
    return false;

}

function fnclienteNuevo() {

    var valor = $('input:radio[name=tipo_contribuyente]:checked').val();

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
        if ($("#ape_pat_cliente").val().length < 1) {
            alert("Ingrese A.Paterno");
            return false;
        };

        if ($("#sdni_cliente").val().length < 1) {
            alert("Ingrese DNI");
            return false;
        };

    }

    if ($("#sdire_cliente").val().length < 1) {
        alert("Ingrese Direccion");
        return false;
    };

    if ($("#subigeo_cliente").val().length < 1) {
        alert("Ingrese Ubicacion");
        return false;
    };


    var clienteView = {
        "srazon_cliente": "", "sruc_cliente": "", "sdni_cliente": "",
        "sdire_cliente": "", "subigeo_cliente": "", "sfono_cliente":"",
        "semail_cliente": "", "srep_cliente": "", "sfono2_cliente": "",
        "tipo_contribuyente": "","ape_pat_cliente" : "", "ape_mat_cliente" : "",
        "nombres_cliente" : ""
    };

    // Setear valores
    clienteView.srazon_cliente = $("#srazon_cliente").val();
    clienteView.sruc_cliente = $("#sruc_cliente").val();
    clienteView.sdni_cliente = $("#sdni_cliente").val();
    clienteView.sdire_cliente = $("#sdire_cliente").val();
    clienteView.subigeo_cliente = $("#subigeo_cliente").val();
    clienteView.sfono_cliente = $("#sfono_cliente").val();
    clienteView.semail_cliente = $("#semail_cliente").val();
    clienteView.srep_cliente = $("#srep_cliente").val();
    clienteView.sfono2_cliente = $("#sfono2_cliente").val();
    clienteView.tipo_contribuyente = $('input:radio[name=tipo_contribuyente]:checked').val();
    clienteView.ape_pat_cliente = $("#ape_pat_cliente").val();
    clienteView.ape_mat_cliente = $("#ape_mat_cliente").val();
    clienteView.nombres_cliente = $("#nombres_cliente").val();

    if (clienteView.tipo_contribuyente == 'N') {
        clienteView.srazon_cliente = clienteView.nombres_cliente + ' ' + clienteView.ape_pat_cliente + ' ' + clienteView.ape_mat_cliente;
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

function fnClienteNatural(test) {
    //console.log(test);

    if (test === "N") {
        $(".natural").show();
        $(".juridica").hide();
    }
    else {
        $(".natural").hide();
        $(".juridica").show();
    }

}