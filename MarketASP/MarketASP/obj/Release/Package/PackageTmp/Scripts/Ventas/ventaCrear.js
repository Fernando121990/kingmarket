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
var conf_poretencion;
var beditar = false;

$(document).ready(function () {

    var code = 0;

    $('#btnpro').hide();

    code = $("#ncode_venta").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();
    conf_PrecioIGV = $("input[type=checkbox][name=bprecioconigv]:checked").val();
    conf_poretencion = $("#poretencion").val();

    /*DATATABLES*/
    cuotastable = $('#tblcuota').DataTable({
        "dom": 'T<"clear">rtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": []
        },
        {
            "sClass": "my_class",
            "aTargets": [1,2]
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlcuotaEditar, {
                "callback": function (sValue, y) {
                    var aPos = cuotastable.row(this).index();
                    var idx = cuotastable.column(this).index();
                    cuotastable.cell(aPos, idx).data(sValue).draw;
                },
                "submitdata": function (value, settings) {
                    return {
                        "column": cuotastable.column(this).index()
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

    if (typeof code === 'undefined') {
      //  console.log('series');

        var cod = $("#sseri_venta option:selected").val();

        fnDocumentoSerieNumero(cod);

        //fnDocumentoSerie   SerieNumero($("#ncode_docu").val(),"V");
    }

    if (code > 0) {
        beditar = true;

        fnclienteDire();
        //console.log($('#ncode_clidire').val());
        $("#NRO_DCLIENTE").val($('#ncode_clidire').val());

        fnclienteFPago();

        $("#nro_fopago").val($('#ncode_fopago').val());
    }

    $("#ncode_docu").change(function () {

        $("#snume_venta").val('');

        var cod = $("#ncode_docu option:selected").val();

        fnDocumentoSerie(cod);

    });

    $("#sseri_venta").change(function () {
        var cod = $("#sseri_venta option:selected").val();
        fnDocumentoSerieNumero(cod);
    });

    /*CLIENTE DIRECCION*/
    $("#btndireccion").on("click", function () {

        var codcliente = $('#COD_CLIENTE').val();

        if (codcliente == '') {

            return
        }

        var ruta = urlclientedireccion;

        // urlclientedireccion.replace("param-id", JSON.stringify(codcliente));
        urlclientedireccion = urlclientedireccion.replace("param-id", codcliente);

        ///window.location.href = urlclientedireccion;
        window.open(urlclientedireccion, "_blank");
        urlclientedireccion = ruta;
    })

    /*CUOTAS*/
    $("#ncuotas_venta").change(function () {
        beditar = false
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#ncuotadias_venta").change(function () {
        beditar = false
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#bclienteagretencion").change(function () {
        Totales(conf_igv, conf_decimal, conf_icbper);
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

        if ($("#ncode_fopago option:selected").text().length < 1) {
            alert("Seleccione forma de pago");
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

    /* ordendes de pedido **/
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

    /* guia de venta **/
    $(".btnguiaventa").click(function () {

        $.ajax({
            type: 'POST',
            url: urlGuiaVenta,
            dataType: 'json',
            data: {},
            success: function (resultado) {

                guiatable = $('#guiaventatabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "ncode_guia" },
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

    $("#btnguia").click(function () {

        var data = guiatable.row('.selected').data();

        console.log(data.ncode_guia);

        fnCargaGuiaVenta(data.ncode_guia);

    });

    $("#btnguiaventacerrarx").click(function () {
        guiatable.destroy();
    });

    $("#btnguiaventacerrar").click(function () {
        guiatable.destroy();
    });


    /**lotes */

    $(".delLote").click(function () {

        var data = olotes.row('.selected').data();
        olotes.rows('.selected').remove().draw(false);

        fnActualizarCtdadLote(data[0], data[3]);

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
                console.log(resultado);

                if (resultado.length == 0) {
                    alert("No hay lotes disponibles para venta")
                    return;
                }

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

        var xcan = $("#xctdadlote").val(); //1;
        var xesta = 0;
        var xcontrol = $("#xctdadfalta").val();


        if (xcan == 0) {
            alert("La cantidad ha agregar es cero, no se asignaran lotes");
            return;
        }

        var data = lotetable.row('.selected').data();
        var idtx = lotetable.row('.selected').index();
        var idt = ofunciones.row('.selected').index();

        //console.log(data);
        
        var xcanlote = data.ncantrestante_lote;

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

    var ventaViewCuotas = {
        "ncode_vedecu": "", "sfecharegistro": "", "nvalor_vedecu": ""
    };

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
        "ncode_vende": "", "ncode_orpe": "", "sserienume_orpe": "", "ncode_guiaAsociadas_venta": "", "ncode_dose": "",
        "ncuotas_venta": "", "ncuotavalor_venta": "", "nretencionvalor_venta": "", "ncuotadias_venta": "", "sglosadespacho_venta": "",
        "bflete_venta": "",
        "nvalIGV_venta": "", "nicbper_venta": "", "bclienteagretencion": "", "ventaViewDetas": [], "ventaViewLotes": [],
        "ventaViewCuotas": []
    };

    ventaView.ncode_venta = $('#ncode_venta').val();
    ventaView.ncode_docu = $("#ncode_docu option:selected").val();
    ventaView.ncode_dose = $("#sseri_venta option:selected").val();
    ventaView.sseri_venta = $("#sseri_venta option:selected").text();
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
    ventaView.bclienteagretencion = $('input:checkbox[name=bclienteagretencion]:checked').val();
    ventaView.sserienume_orpe = $('#sserienume_orpe').val();
    ventaView.ncode_guiaAsociadas_venta = $("#ncode_guiaAsociadas_venta").val();
    ventaView.ncuotas_venta = $('#ncuotas_venta').val();
    ventaView.ncuotavalor_venta = $('#ncuotavalor_venta').val();
    ventaView.nretencionvalor_venta = $('#nretencionvalor_venta').val();
    ventaView.ncuotadias_venta = $('#ncuotadias_venta').val();
    ventaView.sglosadespacho_venta = $('#sglosadespacho_venta').val();
    ventaView.bflete_venta = $('input:checkbox[name=bflete_venta]:checked').val();


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
        ventaViewDetas.ncantLote_vedeta = oTable[i][13];
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

    var otblc = $('#tblcuota').dataTable();
    var nrowsc = otblc.fnGetData().length;
    var oTablec = otblc.fnGetData();

    for (var i = 0; i < nrowsc; i++) {

        ventaViewCuotas.ncode_vedecu = oTablec[i][0];
        ventaViewCuotas.sfecharegistro = oTablec[i][1];
        ventaViewCuotas.nvalor_orpecu = oTablec[i][2];
        ventaView.ventaViewCuotas.push(ventaViewCuotas);

        ventaViewCuotas = {
            "ncode_vedecu": "", "sfecharegistro": "", "nvalor_vedecu": ""
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


    //valor de retencion
    var retencion = 1;
    $("#ntotaretencion_venta").val(0);
    if ($('input:checkbox[name=bclienteagretencion]:checked').val()) {
        retencion = conf_poretencion;
        TOTAL = TOTAL * (1 - retencion);
        $("#ntotaretencion_venta").val(TOTAL.toFixed(conf_decimal));
    }

    //numero de cuotas - valor de cuota
    var xncuotas_venta = $('#ncuotas_venta').val();
    $('#ncuotavalor_venta').val(0);
    if (xncuotas_venta != '' && xncuotas_venta != '0') {
        var xncuotavalor_venta = TOTAL / xncuotas_venta;
        $('#ncuotavalor_venta').val(xncuotavalor_venta.toFixed(conf_decimal));
    }
    CuotasLista(xncuotas_venta, xncuotavalor_venta);

}

function CuotasLista(nrocuotas, valorcuota) {

    

    if (nrocuotas == '' || nrocuotas == '0' || beditar == true) {
        return;
    }

    cuotastable.rows().remove().draw(false);

    var options = { year: 'numeric', month: '2-digit', day: '2-digit' };


    var sfecha = $("#dfeventa_venta").val();
    var dias = $('#ncuotadias_venta').val();
    var valorcuota = $('#ncuotavalor_venta').val();
    var nrocuotas = $('#ncuotas_venta').val();

    for (var i = 0; i < nrocuotas; i++) {
        cuotastable.row.add([i, sfecha, valorcuota]).draw();
        var parts = sfecha.split("/");
        var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
        fecha.setDate(fecha.getDate() + parseInt(dias));
        sfecha = new Date(fecha).toLocaleDateString("es-PE", options);
    }


}


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
    beditar = true
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
            $("#sserienume_orpe").val(venta.sserienumero);
            $('#bclienteagretencion').prop('checked', false);
            if (venta.bclienteagretencion) {
                $('#bclienteagretencion').prop('checked', true);
            }
            $('#ncuotas_venta').val(venta.ncuotas_venta);
            $('#ncuotavalor_venta').val(venta.ncuotavalor_venta);
            $('#nretencionvalor_venta').val(venta.nretencionvalor_venta);

            $('#bflete_venta').prop('checked', false);
            if (venta.bflete_venta) {
                $('#bflete_venta').prop('checked', true);
            }

            $('#ncuotadias_venta').val(venta.ncuotadias_venta);
            $('#sglosadespacho_venta').val(venta.sglosadespacho_venta);

            var num = parseInt(venta.ventaViewDetas.length);
            var oprof = $('#tbl').DataTable();
            oprof.clear();
            for (var i = 0; i < num; i++) {
                oprof.row.add([venta.ventaViewDetas[i].ncode_arti
                ,venta.ventaViewDetas[i].scod2
                ,venta.ventaViewDetas[i].sdesc
                ,venta.ventaViewDetas[i].ncant_vedeta
                , venta.ventaViewDetas[i].sumed
                ,venta.ventaViewDetas[i].npu_vedeta
                ,venta.ventaViewDetas[i].npu_vedeta
                , venta.ventaViewDetas[i].ncode_umed
                ,venta.ventaViewDetas[i].besafecto_vedeta
                ,venta.ventaViewDetas[i].bisc_vedeta
                    ,venta.ventaViewDetas[i].npu_vedeta
                    ,false
                    ,venta.ventaViewDetas[i].nafecto_vedeta
                    ,venta.ventaViewDetas[i].ncant_vedeta]).draw();
            }

            var numc = parseInt(venta.ventaViewCuotas.length);
            var oprofc = $('#tblcuota').DataTable();
            oprofc.clear();
            for (var i = 0; i < numc; i++) {
                oprofc.row.add([venta.ventaViewCuotas[i].ncode_vedecu
                    , venta.ventaViewCuotas[i].sfecharegistro
                    , venta.ventaViewCuotas[i].nvalor_vedecu]).draw();
            }


            fnclienteDire();
            $("#NRO_DCLIENTE").val(venta.ncode_clidire);
            $("#ncode_fopago").val(venta.ncode_fopago);
        },
        error: function (ex) {
            alert('No se puede recuperar datos de la orden de pedido' + ex);
        }
    });

    Totales(conf_igv, conf_decimal, conf_icbper);

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

function fnActualizarCtdadLote(codArt, ctdad) {
    console.log(codArt);

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    var codigo = '';
    var ctdadx = '';
    for (var i = 0; i < nrowsx; i++) {

        codigo = oTable[i][0];
        ctdadx = oTable[i][13];

        if (codigo == codArt) {
            ctdadx = parseFloat(ctdadx) + parseFloat(ctdad);
            ofunciones.cell({ row: i, column: 13 }).data(ctdadx).draw(false);
            break;
        }
    }
}

function fnCargaGuiaVenta(codigo) {

    fnlimpiar();
    console.log(codigo);
    $.ajax({
        type: 'POST',
        url: urlGuiaVentaAsociar,
        dataType: 'json',
        data: { ncode_guia: codigo },
        success: function (venta) {
            console.log(venta);

            $('#sdesc_cliente').val(venta.scliente);
            $('#COD_CLIENTE').val(venta.ncode_cliente);
            $('#sruc_cliente').val(venta.sruc);
            $('#sdni_cliente').val(venta.sdni);
            $('#sobse_venta').val(venta.ncode_cliente);
            $("#ncode_mone").val(venta.ncode_mone);
            $("#ncode_guiaAsociadas_venta").val(codigo);
            $("#ncode_alma").val(venta.ncode_alma);
            $("#sserienume_orpe").val(venta.sserienumero);
            $('#bclienteagretencion').prop('checked', false);
            if (venta.bclienteagretencion) {
                $('#bclienteagretencion').prop('checked', true);
            }
            $('#ncuotas_venta').val(venta.ncuotas_venta);
            $('#ncuotavalor_venta').val(venta.ncuotavalor_venta);
            $('#nretencionvalor_venta').val(venta.nretencionvalor_venta);

            $('#bflete_venta').prop('checked', false);
            if (venta.bflete_venta) {
                $('#bflete_venta').prop('checked', true);
            }

            $('#ncuotadias_venta').val(venta.ncuotadias_venta);
            $('#sglosadespacho_venta').val(venta.sglosadespacho_venta);



            var num = parseInt(venta.ventaViewDetas.length);
            var oprof = $('#tbl').DataTable();
            oprof.clear();
            for (var i = 0; i < num; i++) {
                oprof.row.add([venta.ventaViewDetas[i].ncode_arti
                    , venta.ventaViewDetas[i].scod2
                    , venta.ventaViewDetas[i].sdesc
                    , venta.ventaViewDetas[i].ncant_vedeta
                    , venta.ventaViewDetas[i].sumed
                    , venta.ventaViewDetas[i].npu_vedeta
                    , venta.ventaViewDetas[i].npu_vedeta
                    , venta.ventaViewDetas[i].ncode_umed
                    , venta.ventaViewDetas[i].besafecto_vedeta
                    , venta.ventaViewDetas[i].bisc_vedeta
                    , venta.ventaViewDetas[i].npu_vedeta
                    , false
                    , venta.ventaViewDetas[i].nafecto_vedeta
                    , venta.ventaViewDetas[i].ncant_vedeta]).draw();
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

function fnDocumentoSerie(ncode) {
    //console.log($("#ncode_docu").val());
    //var ncode = $("#ncode_docu option:selected").val();
    var xcode = 0;
    $("#sseri_venta").empty();
    $.ajax({
        type: 'POST',
        url: urlGetDocuSerie,
        dataType: 'json',
        data: { ncode_docu: ncode },
        success: function (areas) {

            var code = areas[0].ncode_dose;
            var des = areas[0].serie;

            $.each(areas, function (i, area) {
                $("#sseri_venta").append('<option value="'
                    + area.ncode_dose + '">'
                    + area.serie + '</option>');
            });

            $("#sseri_venta").val(code);

            fnDocumentoSerieNumero(code);
            
        },
        error: function (ex) {
            alert('No se pueden recuperar las series del documento.' + ex);
        }
    });
    return false;

}

function fnDocumentoSerieNumero(ncode) {
    console.log(ncode);
    ///var ncode = $("#sseri_venta option:selected").val();

    $.ajax({
        type: 'POST',
        url: urlGetDocuNumero,
        dataType: 'json',
        data: { ncode_dose: ncode },
        success: function (docu) {
            console.log(docu);
            $.each(docu, function (i, doc) {
                ///$('#sseri_orpe').val(doc.serie);
                $('#snume_venta').val(doc);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}