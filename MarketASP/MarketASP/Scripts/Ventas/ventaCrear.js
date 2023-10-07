var ofunciones;
var olotes;
var mattable;
var lotetable;
var ordentable;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0
var conf_moneda = 0;
var CONFIG_dscto = 'NO';
var conf_PrecioIGV;

$(document).ready(function () {

    var code = 0;

    $('#btnpro').hide();

    code = $("#ncode_venta").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();
    conf_PrecioIGV = $("input[type=checkbox][name=bprecioconigv]:checked").val();
    //console.log(conf_igv);
    //console.log(conf_decimal);
    //console.log(conf_icbper);

    if (typeof code === 'undefined') {
      //  console.log('series');
        fnDocumentoSerieNumero($("#ncode_docu").val(),"V");
    }

    if (code > 0) {
        fnclienteDire();
        //console.log($('#ncode_clidire').val());
        $("#NRO_DCLIENTE").val($('#ncode_clidire').val());

        fnclienteFPago();

        $("#nro_fopago").val($('#ncode_fopago').val());
    }

    $("#ncode_docu").change(function () {

        var cod = $("#ncode_docu option:selected").val();
        //console.log('docuyuki');
        //console.log(cod);
        fnDocumentoSerieNumero(cod,"V");
    });

    $("#bitguia_venta").click(function () {

        if ($("#bitguia_venta").is(':checked')) {
            //console.log('checkedyuki');
            fnDocumentoSerieNumero(1076, "G");
        }
        else {
           // console.log('uncheckedyuki');
            $("#sserie_guia").val('');  // unchecked
            $("#snumero_guia").val('');  // unchecked
        }
    });


    $("#ncode_fopago").change(function () {
        fnFormaPagoDiasFecha()
    });

    olotes = $('#tblLote').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0,7]
        },
        {
            "sClass": "my_class",
            "aTargets": []
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlEditar, {
                "callback": function (sValue, y) {
                    var aPos = olotes.row(this).index();
                    var idx = olotes.column(this).index();
                    switch (idx) {
                        case 3: //quantity column
                            olotes.cell(aPos, idx).data(sValue).draw;
                            break;
                        default:
                            olotes.cell(aPos, idx).data(sValue).draw;

                    }

                },
                "submitdata": function (value, settings) {
                    return {
                        "column": olotes.column(this).index()
                    };

                },
                "height": "20px",
                "width": "100%"
            });
        },
        select: {
            style: 'single'
        },
        "paging": false,
        "keys": true,
        "info": false,
        "searching": false,
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por pagina",
            "zeroRecords": "No hay datos disponibles",
            "info": "Mostrando pagina _PAGE_ of _PAGES_",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(Filtrado de _MAX_ total registros)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": " Siguiente",
                "previous": "Anterior "
            }
        }
    });


    ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0,6,7,8,9,10,11] 
        },
        {
            "sClass": "my_class",
            "aTargets": [3,5]
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlEditar, {
                "callback": function (sValue, y) {
                    var aPos = ofunciones.row(this).index();
                    var idx = ofunciones.column(this).index();
                    switch (idx) {
                        case 3: //quantity column
                            //console.log('cantidad');
                            var almacen = $("#ncode_alma option:selected").val();
                            var codarticulo = ofunciones.cell(aPos, 0).data();
                            var cantorigen = ofunciones.cell(aPos, 13).data();
                            var cantvalida = ComparaCantidad(sValue, cantorigen)
                            fnStockValida(codarticulo, cantvalida, almacen);
                            ofunciones.cell(aPos, idx).data(cantvalida).draw;
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            var subto = cantvalida * parseFloat(xvalue);
                            ofunciones.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            ofunciones.cell(aPos, 13).data(cantvalida).draw;
                            break;
                        case 5: //price column
                            //console.log('subtotal');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var yValue = ComparaPrecio(sValue, xvalue);
                            var subto = xcant*yValue
                            ofunciones.cell(aPos, idx).data(yValue).draw;
                            ofunciones.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            break;
                        default:
                            ofunciones.cell(aPos, idx).data(sValue).draw;

                    }

                    Totales(conf_igv, conf_decimal, conf_icbper);
                },
                "submitdata": function (value, settings) {
                    return {
                        "column": ofunciones.column(this).index()
                    };

                },
                "height": "20px",
                "width": "100%"
            });
            Totales(conf_igv, conf_decimal, conf_icbper);
        },
        select: {
            style: 'single'
        },
        "paging": false,
        "keys":true,
        "info": false,
        "searching": false,
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por pagina",
            "zeroRecords": "No hay datos disponibles",
            "info": "Mostrando pagina _PAGE_ of _PAGES_",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": "(Filtrado de _MAX_ total registros)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": " Siguiente",
                "previous": "Anterior "
            }
        }
    });

    $("#sdesc_profo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetProforma, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { snume: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.numeracion, value: item.numeracion, id: item.ncode_prof
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#ncode_profo').val(ui.item.id);
            fnCargaProforma();
        }
    });
        
    $(".delMat").click(function () {
        var data = ofunciones.row('.selected').data();
        var xcodart = data[0];
        ofunciones.rows('.selected').remove().draw(false);


        ///console.log(xcodart);

        var indexes = olotes
            .rows()
            .indexes()
            .filter(function (value, index) {
                return xcodart === olotes.row(value).data()[0];
            });

        olotes.rows(indexes).remove().draw();

    });

     $(".addMat").click(function () {
         var codalmacen = $("#ncode_alma option:selected").val();
         console.log(codalmacen);

        $.ajax({
            type: "Post",
            url: urlArticulos,
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { ncode_alma: codalmacen },
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito');

                mattable = $('#matetabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "Cod" },
                        { "data": "Cod2" },
                        { "data": "DescArt" },
                        { "data": "Stock" },
                        { "data": "StockReservado" },
                        { "data": "StockTransito" },
                        { "data": "Medida" },
                        { "data": "Precio" },
                        { "data": "ncode_umed" },
                        { "data": "bafecto_arti" },
                        { "data": "bisc_arti" },
                        { "data": "bdscto_arti" },
                        { "data": "bicbper_arti" }
                            
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0,8,9,10,11,12]
                    },
                    {
                        "sClass": "my_class",
                        "aTargets": []
                    }],
                    select: {
                        style: 'single'
                    },
                    "scrollY": "300px",
                    "scrollCollapse": true,
                    "paging": false,
                    "info": false,
                    "language": {
                        "lengthMenu": "Mostrar _MENU_ registros por pagina",
                        "zeroRecords": "No hay datos disponibles",
                        "info": "Mostrando pagina _PAGE_ of _PAGES_",
                        "infoEmpty": "No hay registros disponibles",
                        "infoFiltered": "(Filtrado de _MAX_ total registros)",
                        "search": "Buscar:",
                        "paginate": {
                            "first": "Primero",
                            "last": "Ultimo",
                            "next": ">>",
                            "previous": "<<"
                        }
                    }
                });

            },
            error: function (err) {
                alert(err);
            }
        });

    });

    $("#btnmate").click(function () {
        var data = mattable.row('.selected').data();
        var xcan = 0;
        var xesta = 0;

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Precio, data.Precio, data.ncode_umed,
            data.bafecto_arti, data.bisc_arti, data.bdscto_arti,data.bicbper_arti,xcan*data.Precio,xcan]).draw();

        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btnventa").click(function () {

        //Validaciones

        if ($("#COD_CLIENTE").val().length < 1) {
            alert("Seleccione Cliente");
            return false;
        };

        if ($("#NRO_DCLIENTE option:selected").text().length < 1) {
            alert("Seleccione Ubicacion");
            return false;
        };
        ////*******

        if ($("#ntc_venta").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_venta').val();


        if ($("#sseri_venta").val().length < 1) {
            alert("Ingrese serie del documento");
            return false;
        }

        if ($("#snume_venta").val().length < 1) {
            alert("Ingrese número del documento");
            return false;
        }

        if(ncode_docu == 10 && $("#sruc_cliente").val().length < 1){
            alert("Ingrese RUC");
            return false;
        }

        if (ncode_docu == 11 && $("#sdni_cliente").val().length < 1) {
            alert("Ingrese DNI");
            return false;
        }

        if (ncode_docu = 11 && $("#COD_CLIENTE").val() == 5 && ntotal > 700 ) {
            alert("La boleta supera los S/ 700, debe registrar al cliente.");
            return false;
        }

        var otbly = $('#tbl').dataTable();
        var nrowsy = otbly.fnGetData().length;
        //console.log(nrowsy);
        if (nrowsy < 1) {
            alert("Seleccione Articulos");
            return false;
        }

        $('#btnventa').hide();
        $('#btnpro').show();

        Sales_save();
    });

    $(".btnpedidos").click(function () {

        $.ajax({
            type: 'POST',
            url: urlPedidoVenta,
            dataType: 'json',
            data: {},
            success: function (resultado) {

               ordentable = $('#pedidotabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "ncode_orpe" },
                            { "data": "fecha" },
                            { "data": "numeracion" },
                            { "data": "srazon_cliente" }
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": []
                    },
                    {
                        "sClass": "my_class",
                        "aTargets": []
                    }],
                    select: {
                        style: 'single'
                    },
                    "scrollY": "300px",
                    "scrollCollapse": true,
                    "paging": false,
                    "info": false,
                    "bDestroy": true,
                    "language": {
                        "lengthMenu": "Mostrar _MENU_ registros por pagina",
                        "zeroRecords": "No hay datos disponibles",
                        "info": "Mostrando pagina _PAGE_ of _PAGES_",
                        "infoEmpty": "No hay registros disponibles",
                        "infoFiltered": "(Filtrado de _MAX_ total registros)",
                        "search": "Buscar:",
                        "paginate": {
                            "first": "Primero",
                            "last": "Ultimo",
                            "next": ">>",
                            "previous": "<<"
                        }
                    }
                });

            },
            error: function (err) {
                alert(err);
            }
        });

    });

    $("#btnorden").click(function () {
        var data = ordentable.row('.selected').data();

        console.log(data.ncode_orpe);

        fnCargaOrdenPedido(data.ncode_orpe);

    });

    $("#btnpedidocerrarx").click(function () {
        ordentable.destroy();
    });

    $("#btnpedidocerrar").click(function () {
        ordentable.destroy();
    });



    /** */

    $(".delLote").click(function () {

        olotes.rows('.selected').remove().draw(false);
    });



    $(".addLote").click(function () {

        var data = ofunciones.row('.selected').data();
        var xctdad = data[13];

        //console.log(xctdad);

        $("#xctdad").val(xctdad);
        $("#xctdadfalta").val(xctdad);
        $("#xctdadlote").val(xctdad);

        var ncodealma = $("#ncode_alma option:selected").val();
        

        $.ajax({
            type: "Post",
            url: urlGetLoteDisponible,
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {ncode_alma: ncodealma,ncode_arti: data[0],fvenci_lote: '',sdesc_lote:''},
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito');

                lotetable = $('#lotetabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [   { "data": "ncode_lote" },
                            { "data": "sdesc_lote" },
                            { "data": "fvenci_lote" },
                            { "data": "ncode_arti" },
                            { "data": "scode_arti" },
                            { "data": "sdesc1_arti" },
                            { "data": "ncantrestante_lote" },
                            { "data": "ncodeDoc_lote" },
                            { "data": "ncode_alma" }
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0,3,7,8]
                    },
                    {
                        "sClass": "my_class",
                        "aTargets": []
                    }],
                    select: {
                        style: 'single'
                    },
                    "scrollY": "300px",
                    "scrollCollapse": true,
                    "paging": false,
                    "info": false,
                    "language": {
                        "lengthMenu": "Mostrar _MENU_ registros por pagina",
                        "zeroRecords": "No hay datos disponibles",
                        "info": "Mostrando pagina _PAGE_ of _PAGES_",
                        "infoEmpty": "No hay registros disponibles",
                        "infoFiltered": "(Filtrado de _MAX_ total registros)",
                        "search": "Buscar:",
                        "paginate": {
                            "first": "Primero",
                            "last": "Ultimo",
                            "next": ">>",
                            "previous": "<<"
                        }
                    }
                });

            },
            error: function (err) {
                alert(err);
            }
        });

    });

    $("#btnLote").click(function () {
        var data = lotetable.row('.selected').data();
        var idtx = lotetable.row('.selected').index();
        var idt = ofunciones.row('.selected').index();

        //console.log(data);
        var xcan = $("#xctdadlote").val(); //1;
        var xcanlote = data.ncantrestante_lote;
        var xesta = 0;
        var xcontrol = $("#xctdadfalta").val();

        //console.log(xcan);
        //console.log(xcanlote);

        if (parseFloat(xcanlote) >= parseFloat(xcan)) {

            olotes.row.add([data.ncode_arti, data.scode_arti, data.sdesc1_arti, xcan, '-', data.sdesc_lote, data.fvenci_lote, data.ncode_lote]).draw();

            xcontrol = xcontrol - xcan;
            xcanlote = xcanlote - xcan;

            $("#xctdadfalta").val(xcontrol);
            $("#xctdadlote").val(xcontrol);

            lotetable.cell({ row: idtx, column: 6 }).data(xcanlote).draw(false);
            ofunciones.cell({ row: idt, column: 13 }).data(xcontrol).draw(false);
        }
        else {
            alert("La cantidad es mayor a la cantidad disponible del lote");
        }
        

    });

    $("#btncerrarL").click(function () {
        lotetable.destroy();
    });

    $("#btnlotecerrar").click(function () {
        lotetable.destroy();
    });

});



function Sales_save() {

    //console.log('nueva venta');

    var ventaViewLotes = {
        "ncode_arti": "", "ncant_velote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
    };


    var ventaViewDetas = {
        "ncode_arti": "", "ncant_vedeta": "", "npu_vedeta": "", "ndscto_vedeta": "",
        "ndscto2_vedeta": "", "nexon_vedeta": "", "nafecto_vedeta": "", "besafecto_vedeta": "",
        "ncode_alma": "", "ndsctomax_vedeta": "", "ndsctomin_vedeta": "", "ndsctoporc_vedeta": "", "bicbper_vedeta": "",
        "sdesc": "", "ncantLote_vedeta":""
    };

    var ventaView = {
        "ncode_venta": "", "ncode_alma": "","ncode_mone":"",
        "ncode_docu":"","sseri_venta":"", "snume_venta":"",
        "sfeventa_venta":"","sfevenci_venta":"","ncode_cliente":"",
        "ncode_clidire":"","smone_venta":"","ntc_venta":"",
        "ncode_fopago":"","sobse_venta":"","scode_compra":"",
        "ncode_profo":"","nbrutoex_venta":"","nbrutoaf_venta":"",
        "ndctoex_venta":"","ndsctoaf_venta":"","nsubex_venta":"",
        "nsubaf_venta":"","nigvex_venta":"","nigvaf_venta":"","ntotaex_venta":"",
        "ntotaaf_venta": "", "ntotal_venta": "", "ntotalMN_venta": "", "ntotalUs_venta": "",
        "ncode_vende": "", "ncode_orpe": "",
        "nvalIGV_venta": "", "nicbper_venta": "", "ventaViewDetas": [], "ventaViewLotes": []
    };

    ventaView.ncode_venta = $('#ncode_venta').val();
    ventaView.ncode_docu = $("#ncode_docu option:selected").val();
    ventaView.sseri_venta = $('#sseri_venta').val();
    ventaView.snume_venta = $('#snume_venta').val();
    ventaView.sfeventa_venta = $('#dfeventa_venta').val();
    ventaView.sfevenci_venta = $('#dfevenci_venta').val();
    ventaView.ncode_cliente = $('#COD_CLIENTE').val(); 
    ventaView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    ventaView.smone_venta = $('#smone_venta').val();
    ventaView.ntc_venta = $('#ntc_venta').val();
    ventaView.ncode_fopago = $("#ncode_fopago option:selected").val();
    ventaView.sobse_venta = $('#sobse_venta').val();
    ventaView.scode_compra = $('#scode_compra').val();
    ventaView.ncode_profo = $('#ncode_profo').val();
    ventaView.nbrutoex_venta = $('#nbrutoex_venta').val();
    ventaView.nbrutoaf_venta = $('#nbrutoaf_venta').val();
    ventaView.ndctoex_venta = $('#ndctoex_venta').val();
    ventaView.ndsctoaf_venta = $('#ndsctoaf_venta').val();
    ventaView.nsubex_venta = $('#nsubex_venta').val();
    ventaView.nsubaf_venta = $('#nsubaf_venta').val();
    ventaView.nigvex_venta = $('#nigvex_venta').val();
    ventaView.nigvaf_venta = $('#nigvaf_venta').val();
    ventaView.ntotaex_venta = $('#ntotaex_venta').val();
    ventaView.ntotaaf_venta = $('#ntotaaf_venta').val();
    ventaView.ntotal_venta = $('#ntotal_venta').val();
    ventaView.ntotalMN_venta = $('#ntotalMN_venta').val();
    ventaView.ntotalUs_venta = $('#ntotalUs_venta').val();
    ventaView.nicbper_venta = $('#nicbper_venta').val();
    ventaView.nvalIGV_venta = $('#nvalIGV_venta').val();
    ventaView.ncode_alma = $("#ncode_alma option:selected").val();
    ventaView.ncode_mone = $("#ncode_mone option:selected").val();
    ventaView.ncode_vende = $("#ncode_vende option:selected").val();
    ventaView.ncode_orpe = $("#ncode_orpe").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        ventaViewDetas.ncode_arti = oTable[i][0];
        ventaViewDetas.sdesc = oTable[i][2];
        ventaViewDetas.ncant_vedeta = oTable[i][3];
        ventaViewDetas.npu_vedeta = oTable[i][5];
        ventaViewDetas.ndscto_vedeta = oTable[i][6] - oTable[i][5];
        ventaViewDetas.nexon_vedeta = oTable[i][5] * oTable[i][3];
        ventaViewDetas.nafecto_vedeta = oTable[i][5] * oTable[i][3];
        ventaViewDetas.besafecto_vedeta = oTable[i][8];
        ventaViewDetas.ndsctoporc_vedeta = oTable[i][6];
        ventaViewDetas.bicbper_vedeta = oTable[i][11];
        ventaViewDetas.ncantLote_vedeta = oTable[i][12];
        ventaViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        ventaView.ventaViewDetas.push(ventaViewDetas);

        ventaViewDetas = {
            "ncode_arti": "", "ncant_vedeta": "", "npu_vedeta": "", "ndscto_vedeta": "",
            "ndscto2_vedeta": "", "nexon_vedeta": "", "nafecto_vedeta": "", "besafecto_vedeta": "",
            "ncode_alma": "", "ndsctomax_vedeta": "", "ndsctomin_vedeta": "", "ndsctoporc_vedeta": "",
            "bicbper_vedeta": "", "sdesc": "", "ncantLote_vedeta": ""
        };

    }

    var otbly = $('#tblLote').dataTable();
    var nrowsy = otbly.fnGetData().length;
    var oTabley = otbly.fnGetData();

    for (var i = 0; i < nrowsy; i++) {

        //console.log(oTabley[i][3]);

        ventaViewLotes.ncode_arti = oTabley[i][0];
        ventaViewLotes.sdesc = oTabley[i][2];
        ventaViewLotes.ncant_velote = oTabley[i][3];
        ventaViewLotes.sdesc_lote = oTabley[i][5];
        ventaViewLotes.sfvenci_lote = oTabley[i][6]
        ventaViewLotes.ncode_lote = oTabley[i][7]
        ventaViewLotes.ncode_alma = $("#ncode_alma option:selected").val();

        ventaView.ventaViewLotes.push(ventaViewLotes);

        ventaViewLotes = {
            "ncode_arti": "", "ncant_velote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
        };


    }



    //console.log(ventaView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlventaCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(ventaView) },
        success: function (result) {
            console.log(result.Mensaje)
            switch (result.Success) {
                case 1:
                    window.location.href = urlventaLista;
                    alert(result.Mensaje);
                    break;
                case 2:
                    console.log(urlventaCobro);
                    urlventaCobro = urlventaCobro.replace("param-id", encodeURIComponent(result.CtaCo))
                        .replace("param-mensaje", encodeURIComponent(result.Mensaje));
                    //console.log(urlventaCobro);
                    window.location.href = urlventaCobro;
                    break;
                case 3:
                    alert(result.Mensaje);
                    $('#btnventa').show();
                    $('#btnpro').hide();
                    break;
                default:
                    window.location.href = urlventaLista;
                    alert(result.Mensaje);
            }
        },
        error: function (ex) {
            alert('No se puede registrar venta' + ex);
        }
    });

}

function Totales(conf_igv, conf_decimal, conf_icbper) {
    console.log('calculo de totales');
    console.log(conf_igv);
    console.log(conf_decimal);
    console.log(conf_icbper);

    var TOT_AFECTO = parseFloat(0).toFixed(conf_decimal);
    var TOT_EXON = parseFloat(0).toFixed(conf_decimal);
    var TOT = parseFloat(0).toFixed(conf_decimal);
    var SUBT = parseFloat(0).toFixed(conf_decimal);
    var SUBT_AFECTO = parseFloat(0).toFixed(conf_decimal);
    var SUBT_EXON = parseFloat(0).toFixed(conf_decimal);
    var DSCTO = parseFloat(0).toFixed(conf_decimal);
    var DSCTO_AFECTO = parseFloat(0).toFixed(conf_decimal);
    var DSCTO_EXON = parseFloat(0).toFixed(conf_decimal);
    var TOT_ISC = parseFloat(0).toFixed(conf_decimal);
    var IGVTOTAL = parseFloat(0).toFixed(conf_decimal);

    CONFIG_IGV = parseInt(conf_igv);
    var monedaVenta = $('#smone_venta').val();
    var TCambio = parseFloat($('#ntc_venta').val());

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        TOT = 0; CANT = 0; PU = 0; CDSCTO = 0; CDSCTO2 = 0; PUADESCONTAR = 0; PUSINIGV = 0; IGVDELPRECIO = 0; IGVPORDETALLE = 0; TCambio = 0

        CANT = parseFloat(oTable[i][3]).toFixed(conf_decimal);
        PU = parseFloat(oTable[i][5]).toFixed(conf_decimal);
        CDSCTO = parseFloat(0).toFixed(conf_decimal); //REDONDEAR(Val(DBLISTAR.Item(F, COL_DSCTO)), CONFIG_GE_NRODECIMALES)
        CDSCTO2 = parseFloat(0).toFixed(conf_decimal); //REDONDEAR(Val(DBLISTAR.Item(F, COL_DSCTO2)), CONFIG_GE_NRODECIMALES)
        var AFECTO_ART = oTable[i][8].toString();
        var COL_TISC = oTable[i][9];

        //validar venta en soles o dolares
        if (monedaVenta == "D") {
            PU = (PU / TCambio).toFixed(conf_decimal);
        }


        if (CONFIG_dscto = "SI") {
            PUADESCONTAR = (PU * (DSCTO / 100)).toFixed(conf_decimal);
            TOT = (CANT * (PU - PUADESCONTAR)).toFixed(conf_decimal);
        }
        else {
            PUADESCONTAR = CDSCTO
            TOT = (CANT * (PU - PUADESCONTAR)).toFixed(conf_decimal);
        }

        TOT = (TOT - (TOT * (CDSCTO2 / 100))).toFixed(conf_decimal);

        if (AFECTO_ART.toUpperCase() == 'TRUE' || AFECTO_ART == 'true' || AFECTO_ART == 'True') {
            TOT_AFECTO = parseFloat(TOT_AFECTO) + Math.round(TOT, conf_decimal);
        }

        if (AFECTO_ART.toUpperCase() == 'FALSE' || AFECTO_ART == 'false' || AFECTO_ART == 'False') {
            TOT_EXON = parsefloat(TOT_EXON) + Math.round(TOT, conf_decimal);
        }

        SUBT = ((PU - PUADESCONTAR) * CANT).toFixed(conf_decimal);
        PUSINIGV = ((PU - PUADESCONTAR) / (1 + (CONFIG_IGV / 100))).toFixed(conf_decimal);
        IGVDELPRECIO = ((PU - PUADESCONTAR) - PUSINIGV).toFixed(conf_decimal);

        //DSCTO
        if (parseFloat(TOT_AFECTO) > 0) {
            SUBT_AFECTO = parseFloat(SUBT_AFECTO) + parseFloat(SUBT);

            if (CONFIG_dscto == "SI") {
                DSCTO = (PUADESCONTAR * CANT).toFixed(conf_decimal);
            }
            else {
                DSCTO = (PUADESCONTAR * CANT).toFixed(conf_decimal);
            }

            DSCTO_AFECTO = parseFloat(DSCTO_AFECTO) + parseFloat(DSCTO);
        }

        if (parseFloat(TOT_EXON) > 0) {
            SUBT_EXON = parseFloat(SUBT_EXON) + parseFloat(SUBT);
            if (CONFIG_dscto == "SI") {
                DSCTO = (PUADESCONTAR * CANT).toFixed(conf_decimal);
            }
            else {
                DSCTO = (PUADESCONTAR * CANT).toFixed(conf_decimal);
            }
            DSCTO_EXON = parseFloat(DSCTO_EXON) + parseFloat(DSCTO);
        }

        if (COL_TISC == true) {
            TOT_ISC = TOT_ISC + (TOT_AFECTO / (1 + COL_TISC / 100));
            TOT_ISC = TOT_ISC + (TOT_EXON / (1 + COL_TISC / 100));
        }

        IGVPORDETALLE = (IGVDELPRECIO * CANT).toFixed(2);

        IGVTOTAL = IGVTOTAL + IGVPORDETALLE

    }

    console.log('valores totales');
    console.log(DSCTO_AFECTO);
    console.log(DSCTO_EXON);
    console.log(SUBT_AFECTO);
    console.log(SUBT_EXON);


    $("#ndsctoaf_venta").val(DSCTO_AFECTO);
    $("#ndctoex_venta").val(DSCTO_EXON);
    $("#nbrutoaf_venta").val(SUBT_AFECTO);
    $("#nbrutoex_venta").val(SUBT_EXON);

    $("#nsubaf_venta").val(SUBT_AFECTO);
    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    $("#nsubex_venta").val(SUBT_EX);
    $("#ntotaex_venta").val(SUBT_EXON);

    var TOTAL_AFEC = 0, SUBT_AFEC = 0, IGV_AF = 0;


    if (conf_PrecioIGV == "on") {
        TOTAL_AFEC = parseFloat(SUBT_AFECTO);
        SUBT_AFEC = parseFloat(TOTAL_AFEC) / (1 + (parseInt(CONFIG_IGV) / 100));
        IGV_AF = parseFloat(TOTAL_AFEC) - parseFloat(SUBT_AFEC);

        $("#ntotaaf_venta").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nsubaf_venta").val(SUBT_AFEC.toFixed(conf_decimal));
        $("#nigvaf_venta").val(IGV_AF.toFixed(conf_decimal));

    }
    else {
        IGV_AF = (parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV) / parseInt(100)));
        TOTAL_AFEC = (parseFloat(SUBT_AFECTO) + parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV) / parseInt(100)));

        $("#ntotaaf_venta").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nigvaf_venta").val(IGV_AF.toFixed(conf_decimal));

    }

    var TOTAL = parseFloat(TOTAL_AFEC) + parseFloat(SUBT_EX);

    $("#ntotal_venta").val(TOTAL.toFixed(conf_decimal));
}


//function Totales(conf_igv, conf_decimal, conf_icbper) {
//    console.log('calculo de totales');
//    console.log(conf_igv);
//    console.log(conf_decimal);
//    console.log(conf_icbper);

//    var TOT_AFECTO = 0;
//    var TOT_EXON = 0;
//    var TOT = 0;
//    var SUBT = 0;
//    var SUBT_AFECTO = 0;
//    var SUBT_EXON = 0;
//    var DSCTO = 0;
//    var DSCTO_AFECTO = 0;
//    var DSCTO_EXON = 0;
//    var TOT_ISC = 0;
//    var CONFIG_IGV = 0;
//    var TOT_ICBPER = 0;

//    var otblx = $('#tbl').dataTable();
//    var nrowsx = otblx.fnGetData().length;
//    var oTable = otblx.fnGetData();

//    CONFIG_IGV = conf_igv;

//    console.log(oTable);

//    for (var i = 0; i < nrowsx; i++) {

        
//        console.log(nrowsx);

//        var AFECTO_ART = oTable[i][8].toString();
//        var COL_TISC = oTable[i][9];
//        var COL_ICBPER = oTable[i][11].toString();
//        var CANT_DV = oTable[i][3];
//        var PU_DV = oTable[i][5];
//        var DSCTO_DV = 0;

//        TOT = 0;
//        TOT_AFECTO = 0;
//        TOT_EXON = 0;
//        TOT = CANT_DV * PU_DV;
//        TOT = TOT - (TOT * DSCTO_DV / 100);

//        console.log(COL_ICBPER);

//        if (AFECTO_ART.toUpperCase() == 'TRUE' || AFECTO_ART == 'true' || AFECTO_ART == 'True') {
//            TOT_AFECTO = TOT_AFECTO + Math.round(TOT, conf_decimal);
//            //  console.log('afecto');
//        }

//        if (AFECTO_ART.toUpperCase() == 'FALSE' || AFECTO_ART == 'false' || AFECTO_ART == 'False') {

//            TOT_EXON = TOT_EXON + Math.round(TOT, conf_decimal);
//            //console.log('exone');
//        }

//        if (COL_ICBPER.toUpperCase() == 'TRUE') {
//            TOT_ICBPER = TOT_ICBPER + conf_icbper * CANT_DV;
//        }

//        SUBT = CANT_DV * PU_DV;
//        //console.log('totafecto');
//        //console.log(TOT_AFECTO);
//        //console.log('totexone');
//        //console.log(TOT_EXON);

//        if (TOT_AFECTO !== 0) {
//            SUBT_AFECTO = SUBT_AFECTO + SUBT;
//            DSCTO = (SUBT * (DSCTO_DV / 100));
//            DSCTO_AFECTO = DSCTO_AFECTO + DSCTO;
//            //      console.log('subafecto');
//            //    console.log(SUBT_AFECTO);
//        }

//        if (TOT_EXON !== 0) {
//            SUBT_EXON = SUBT_EXON + SUBT;
//            DSCTO = (SUBT * (DSCTO_DV / 100));
//            DSCTO_EXON = DSCTO_EXON + DSCTO;
//            //  console.log(TOT_EXON);
//            //console.log('subtexon');
//            //console.log(SUBT_EXON);
//        }

//        if (COL_TISC == true) {
//            TOT_ISC = TOT_ISC + (TOT_AFECTO / (1 + COL_TISC / 100));
//            TOT_ISC = TOT_ISC + (TOT_EXON / (1 + COL_TISC / 100));
//        }
//    }

//    $("#ndsctoaf_venta").val(DSCTO_AFECTO.toFixed(conf_decimal));
//    $("#ndctoex_venta").val(DSCTO_EXON.toFixed(conf_decimal));
//    $("#nbrutoaf_venta").val(SUBT_AFECTO.toFixed(conf_decimal));
//    $("#nbrutoex_venta").val(SUBT_EXON.toFixed(conf_decimal));
//    $("#ntotaex_venta").val(SUBT_EXON.toFixed(conf_decimal));
//    $("#nicbper_venta").val(TOT_ICBPER.toFixed(conf_decimal));

//    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
//    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
//    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

//    $("#nsubex_venta").val(SUBT_EX.toFixed(conf_decimal));
//    $("#ntotaaf_venta").val(TOTAL_AFEC.toFixed(conf_decimal));
//    $("#nsubaf_venta").val(SUBT_AFEC.toFixed(conf_decimal));

//    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
//    $("#nigvaf_venta").val(IGV_AF.toFixed(conf_decimal));
//    $("#nigvex_venta").val(0);

//    var TOTAL = TOTAL_AFEC + SUBT_EX;

//    $("#ntotal_venta").val(TOTAL.toFixed(conf_decimal));

//    return false;
//}

function ComparaPrecio(Precio, PrecioOrigen) {

    var xprecio = parseFloat(Precio);

    if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
        xprecio = parseFloat(PrecioOrigen);
    }

    return xprecio.toFixed(conf_decimal);

}
function ComparaCantidad(Cantidad, CantidadOrigen) {
    console.log('valida cantidad');

    var xcantidad = parseFloat(Cantidad);
    //Verificar la cantidad origen sino viene de pedido no hay control de cantidades
    if (CantidadOrigen > 0) {

        if (parseFloat(Cantidad) > parseFloat(CantidadOrigen)) {
            xcantidad = parseFloat(CantidadOrigen);
        }


    }


    return xcantidad.toFixed(conf_decimal);

}
function fnFormaPagoDiasFecha() {
    $.ajax({
        type: 'POST',
        url: urlGetDiasFormaPago,
        dataType: 'json',
        data: { ncode_fopago: $("#ncode_fopago").val() },
        success: function (fopago) {
            console.log(fopago);
            $.each(fopago, function (i, dias) {

                var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
                var m = $("#dfeventa_venta").val();
                var parts = m.split("/");
                var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
                //var fecha = new Date($('#dfeventa_venta').val());
                //console.log(fecha);
                //console.log(fecha.getDate());
                //console.log(fecha.getFullYear());
                //console.log(fecha.getMonth());
                //console.log(dias.dias);
                //var xdias = parseInt(fecha.getDate()) + parseInt(dias.dias); // Número de días a agregar
                //console.log(xdias);
                fecha.setDate(fecha.getDate() + parseInt(dias.dias));
                var xfecha = new Date(fecha).toLocaleDateString("es-PE", options);
                $('#dfevenci_venta').val(xfecha);
                //console.log(fecha);
                console.log(xfecha);
            });

        },
        error: function (ex) {
            alert('No se pueden recuperar las areas.' + ex);
        }
    });
    return false;

}
function fnDocumentoSerieNumero(docu,tipo) {
    //console.log($("#ncode_docu").val());

    $.ajax({
        type: 'POST',
        url: urlGetDocuNumero,
        dataType: 'json',
        data: { ndocu: docu },
        success: function (docu) {
            //console.log(docu.length);
            $.each(docu, function (i, doc) {
                
                switch (tipo) {
                    case "V":
                        $('#sseri_venta').val(doc.serie);
                        $('#snume_venta').val(doc.numero);
                        break
                    case "G":
                        $('#sserie_guia').val(doc.serie);
                        $('#snumero_guia').val(doc.numero);
                        break
                }
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}
function fnCargaProforma() {

    //console.log($("#ncode_profo").val());
    fnlimpiar();

    $.ajax({
        type: 'POST',
        url: urlGetProfoVenta,
        dataType: 'json',
        data: { ncode_prof: $("#ncode_profo").val() },
        success: function (venta) {
            console.log(venta);

            $('#sdesc_cliente').val(venta.scliente);
            $('#COD_CLIENTE').val(venta.ncode_cliente);
            $('#sruc_cliente').val(venta.sruc);
            $('#sdni_cliente').val(venta.sdni);
            $('#sobse_venta').val(venta.ncode_cliente);
            $("#ncode_mone").val(venta.ncode_mone);

            var num = parseInt(venta.ventaViewDetas.length);
            var oprof = $('#tbl').DataTable();
            oprof.clear();
            for (var i = 0; i < num; i++) {
                oprof.row.add([venta.ventaViewDetas[i].ncode_arti,
                    venta.ventaViewDetas[i].scod2,
                    venta.ventaViewDetas[i].sdesc,
                    venta.ventaViewDetas[i].ncant_vedeta,
                    1,
                    venta.ventaViewDetas[i].npu_vedeta,
                    venta.ventaViewDetas[i].npu_vedeta,
                    1,
                    venta.ventaViewDetas[i].besafecto_vedeta,
                    venta.ventaViewDetas[i].bisc_vedeta,
                    venta.ventaViewDetas[i].npu_vedeta,
                    venta.ventaViewDetas[i].nafecto_vedeta]).draw();
            }

            fnclienteDire();
            $("#NRO_DCLIENTE").val(venta.ncode_clidire);
        },
        error: function (ex) {
            alert('No se puede recuperar datos de proforma' + ex);
        }
    });

    Totales();

    return false;

    
}
function fnlimpiar() {

    $('#COD_CLIENTE').val();
    $("#NRO_DCLIENTE").empty();
    $('#sobse_venta').val();
    $('#ncode_compra').val();
}
function fnCargaOrdenPedido(codigo) {

    fnlimpiar();
    console.log(codigo);
    $.ajax({
        type: 'POST',
        url: urlGetOrdenPedidoVenta,
        dataType: 'json',
        data: { ncode_orpe: codigo },
        success: function (venta) {
            console.log(venta);

            $('#sdesc_cliente').val(venta.scliente);
            $('#COD_CLIENTE').val(venta.ncode_cliente);
            $('#sruc_cliente').val(venta.sruc);
            $('#sdni_cliente').val(venta.sdni);
            $('#sobse_venta').val(venta.ncode_cliente);
            $("#ncode_mone").val(venta.ncode_mone);
            $("#ncode_orpe").val(codigo);
            $("#ncode_alma").val(venta.ncode_alma);


            var num = parseInt(venta.ventaViewDetas.length);
            var oprof = $('#tbl').DataTable();
            oprof.clear();
            for (var i = 0; i < num; i++) {
                oprof.row.add([venta.ventaViewDetas[i].ncode_arti
                ,venta.ventaViewDetas[i].scod2
                ,venta.ventaViewDetas[i].sdesc
                ,venta.ventaViewDetas[i].ncant_vedeta
                ,1
                ,venta.ventaViewDetas[i].npu_vedeta
                ,venta.ventaViewDetas[i].npu_vedeta
                ,1
                ,venta.ventaViewDetas[i].besafecto_vedeta
                ,venta.ventaViewDetas[i].bisc_vedeta
                    ,venta.ventaViewDetas[i].npu_vedeta
                    ,false
                    ,venta.ventaViewDetas[i].nafecto_vedeta
                    ,venta.ventaViewDetas[i].ncant_vedeta]).draw();
            }

            fnclienteDire();
            $("#NRO_DCLIENTE").val(venta.ncode_clidire);
            $("#ncode_fopago").val(venta.ncode_fopago);
        },
        error: function (ex) {
            alert('No se puede recuperar datos de la orden de pedido' + ex);
        }
    });

    Totales();

   return false;


}

//function fnStockValida(codigo, cantidad, almacen, callback) {
function fnStockValida(codigo, cantidad, almacen) {
    //console.log(codigo);
    //console.log(almacen);
    $.ajax({
        type: 'POST',
        url: urlStockValida,
        dataType: 'json',
        data: { ncode_arti: codigo, ncode_alma:almacen },
        success: function (resudisponible) {

            if (resudisponible < cantidad) {
                alert('El articulo no tiene stock disponible ');
            }

            //console.log(rpta);
            //callback(rpta);
        },
        error: function (ex) {
            alert('No se puede recuperar el stock disponible' + ex);
        }
    });

}
function fnmensaje(rpta) {
    if (rpta == false) {
        alert('el articulo no tiene disponible');
    }
}

