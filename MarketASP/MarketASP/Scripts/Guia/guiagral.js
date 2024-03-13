var mattable;
var olotes;
var ofunciones;
var lotetable;
var ordentable;
var cuotastable = null;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0;
var stipo;
var conf_PrecioIGV;
var conf_poretencion;
var beditar = false;
var bhasOP = false;

$(document).ready(function () {

    var code = 0;

    $('#btnpro').hide();

    code = $("#ncode_guia").val();
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
                        "column": ofunciones.column(this).index()
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

    $("#btnopedido").prop("disabled", true);

    if (typeof code === 'undefined') {
        //  console.log('series');
        var cod = $("#sserie_guia option:selected").val();

        fnDocumentoSerieNumero(cod);
        ////fnDocumentoSerieNumero($("#ncode_docu").val());
    }

    if (code > 0) {

        beditar = true;

        fnclienteDire();
        
        $("#NRO_DCLIENTE").val($('#ncode_clidire').val());

        bhasOP = true;
        if ($("#sserienume_orpe").val().length < 1) {
            
            bhasOP = false;
        };

    }

    fnMovimiento()
    fnTransferencia()

    $("#ncode_tiguia").change(function () {

        fnMovimiento();
        fnTransferencia();
    })


    $("#ncode_docu").change(function () {

        $("#snume_guia").val('');

        var cod = $("#ncode_docu option:selected").val();

        fnDocumentoSerie(cod);

        $("#btnopedido").prop("disabled", true);
        if (cod == 1076) {
            $("#btnopedido").prop("disabled", false);
        }
    });

    $("#sserie_guia").change(function () {
        var cod = $("#sserie_guia option:selected").val();
        fnDocumentoSerieNumero(cod);
    });

    /*cambio de fecha*/
    $('#dfemov_guia').change(function () {

        var codpago = $("#nro_fopago option:selected").val();

        if (typeof codpago !== 'undefined') {

            fnFormaPagoDiasFecha(codpago);
        }

        //console.log(this.value);

        fnTipoCambioFecha('GU', 'venta', this.value)

    });

    /*forma de pago*/

    $("#ncode_fopago").change(function () {


        var codpago = $("#ncode_fopago option:selected").val();
        var sfpago = $("#ncode_fopago option:selected").text();

        $("#ncuotas_guia").prop("disabled", true);
        $("#ncuotadias_guia").prop("disabled", true);
        $("#ncuotas_guia").val(0);
        $("#ncuotadias_guia").val(0);

        if (sfpago.indexOf("FNEG") != -1 || sfpago.indexOf("LETRA") != -1) {
            $("#ncuotas_guia").prop("disabled", false);
            $("#ncuotadias_guia").prop("disabled", false);
        }
        else {
            cuotastable.rows().remove().draw(false);
        }


        fnFormaPagoDiasFecha(codpago);

    });

    /*CUOTAS*/
    $("#ncuotas_guia").change(function () {
        beditar = false
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#ncuotadias_guia").change(function () {
        beditar = false
        Totales(conf_igv, conf_decimal, conf_icbper);
    });


    olotes = $('#tblLote').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0, 7]
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
        "ordering":false,
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0, 6, 7, 8, 9, 10, 11, 12,16]
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
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            var cantorigen = ofunciones.cell(aPos, 13).data();
                            var descarga = ofunciones.cell(aPos, 16).data();
                            
                            var cantvalida = 0;
                            var cantfaltante = 0;
                            var subto = 0;

                            if (descarga.toString() == 'true' || descarga.toString() == 'True')
                            {
                                cantvalida = ComparaCantidad(sValue, cantorigen)
                                fnStockValida(codarticulo, cantvalida, almacen);

                                ofunciones.cell(aPos, idx).data(cantvalida).draw;

                                cantfaltante = fnActualizaCtdadRestanteLote(codarticulo, cantvalida);
                                subto = cantvalida * parseFloat(xvalue);

                            }
                            else {
                                ofunciones.cell(aPos, idx).data(sValue).draw;
                                subto = sValue * parseFloat(xvalue);
                            }

                            ofunciones.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            ofunciones.cell(aPos, 15).data(cantfaltante).draw;

                            break;

                        case 5: //price column
                            //console.log('subtotal');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var yValue = ComparaPrecio(sValue, xvalue);
                            var subto = xcant * yValue
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
                            { "data": "bicbper_arti" },
                            { "data": "blote_arti" },
                            { "data": "bdescarga" }

                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0, 7, 8, 9, 10, 11,12]
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
            data.bafecto_arti, data.bisc_arti, data.bdscto_arti, data.bicbper_arti, xcan * data.Precio, xcan, null, xcan, data.bdescarga]).draw();

        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btnguia").click(function () {

        //Validaciones

        if ($("#COD_CLIENTE").val().length < 1) {
            alert("Seleccione Cliente");
            return false;
        };

        if ($("#NRO_DCLIENTE option:selected").text().length < 1) {
            alert("Seleccione Ubicacion");
            return false;
        };

        if ($("#NRO_DCLIENTE option:selected").text().length < 1) {
            alert("Seleccione Ubicacion");
            return false;
        };

        if ($("#ncode_alma option:selected").text().length < 1) {
            alert("Seleccione Almacen");
            return false;
        };

        if ($("#ncode_tiguia option:selected").text().substring(0, 1) == 'T') {

            if ($("#ndestino_alma option:selected").text().length < 1) {
                alert("Seleccione Almacen destino");
                return false;
            };
        }


        if ($("#ntc_guia").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_guia').val();


        if ($("#sserie_guia").val().length < 1) {
            alert("Ingrese serie del documento");
            return false;
        }

        if ($("#snume_guia").val().length < 1) {
            alert("Ingrese número del documento");
            return false;
        }

        if (ncode_docu == 10 && $("#sruc_cliente").val().length < 1) {
            alert("Ingrese RUC");
            return false;
        }

        if (ncode_docu == 11 && $("#sdni_cliente").val().length < 1) {
            alert("Ingrese DNI");
            return false;
        }


        var otbly = $('#tbl').dataTable();
        var nrowsy = otbly.fnGetData().length;
        //console.log(nrowsy);
        if (nrowsy < 1) {
            alert("Seleccione Articulos");
            return false;
        }

        $('#btnguia').hide();
        $('#btnpro').show();

        Sales_save();
    });

    //lista de almacenes
    $(".btnalma").click(function () {
        var data = ofunciones.row('.selected').data();
        var xcodarticulo = data[0];

        $.ajax({
            type: 'POST',
            url: urlKardex,
            dataType: 'json',
            data: { ncode_arti: xcodarticulo },
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito almacen');

                almatable = $('#almatabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "sdesc_alma" },
                        { "data": "sdesc1_arti" },
                        { "data": "STOCK" },
                        { "data": "TRANSITO" }
                        ],
                    footerCallback: function (row, data, start, end, display) {
                        var api = this.api();

                        // Remove the formatting to get integer data for summation
                        var intVal = function (i) {
                            return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                        };

                        // Total over all pages
                        totstock = api
                            .column(2)
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);

                        // Total over this page
                        totDispo = api
                            .column(3)
                            .data()
                            .reduce(function (a, b) {
                                return intVal(a) + intVal(b);
                            }, 0);

                        // Update footer
                        $(api.column(2).footer()).html(totstock);
                        $(api.column(3).footer()).html(totDispo);
                    },
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

    $("#btnalmacerrarx").click(function () {
        almatable.destroy();
    });

    $("#btnalmacerrar").click(function () {
        almatable.destroy();
    });

    /* lote de salida */

    $(".delLote").click(function () {
        var data = olotes.row('.selected').data();
        olotes.rows('.selected').remove().draw(false);

        fnActualizarCtdadLote(data[0], data[3]);

        //olotes.rows('.selected').remove().draw(false);
    });

    $(".addLote").click(function () {

        var data = ofunciones.row('.selected').data();
        var xcodart = data[0];
        var xalma = $("#ncode_alma option:selected").val();
        var xctdad = data[15];

        //console.log(xctdad);

        $("#xctdad").val(xctdad);
        $("#xctdadfaltaS").val(xctdad);
        $("#xctdadlote").val(xctdad);

        $.ajax({
            type: "Post",
            url: urlGetLoteDisponible,
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { ncode_alma: xalma, ncode_arti: xcodart, fvenci_lote: '', sdesc_lote: '' },
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito');

                lotetable = $('#lotetabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "ncode_lote" },
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

        /*VALIDAR QUE LA CANTIDAD A SOLICITAR SEA MAYOR QUE CERO */

        var xcan = parseFloat($("#xctdadlote").val());

        if (xcan <= 0) {

            alert("La cantidad a solicitar es cero");
            return;
        }


        var data = lotetable.row('.selected').data();
        var idtx = lotetable.row('.selected').index();
        var idt = ofunciones.row('.selected').index();

        //console.log(data);
        
        var xcanlote = data.ncantrestante_lote;
        var xesta = 0;
        var xcontrol = $("#xctdadfaltaS").val();

        //console.log(xcan);
        //console.log(xcanlote);

        if (parseFloat(xcanlote) >= parseFloat(xcan)) {

            olotes.row.add([data.ncode_arti, data.scode_arti, data.sdesc1_arti, xcan, '-', data.sdesc_lote, data.fvenci_lote, data.ncode_lote]).draw();

            xcontrol = xcontrol - xcan;
            xcanlote = xcanlote - xcan;

            $("#xctdadfaltaS").val(xcontrol);
            $("#xctdadlote").val(xcontrol);

            lotetable.cell({ row: idtx, column: 6 }).data(xcanlote).draw(false);
            ofunciones.cell({ row: idt, column: 15 }).data(xcontrol).draw(false);
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

    /* lote de ingreso*/

    $("#btnLoteI").click(function () {

        var xcod = $('#xcodearti').val();
        var xcodlocal = $('#xcodelocal').val();
        var xdesc = $('#xdescarti').val();
        var xfvenci = $('#dfvenci_lote').val();
        var xlote = $('#sdesc_lote').val();
        var xcant = $('#ncant_lote').val();
        var xund = $("#xund").val();
        var xcontrol = $("#xctdadfalta").val();

        if (parseFloat(xcontrol) >= parseFloat(xcant) && parseFloat(xcontrol) > 0) {

            olotes.row.add([xcod, xcodlocal, xdesc, xcant, xund, xlote, xfvenci, 0]).draw();

            xcontrol = xcontrol - xcant

            $("#xctdadfalta").val(xcontrol);

            ofunciones.cell({ row: idtx, column: 15 }).data(xcontrol).draw(false);

            console.log('cantidad de lote actualizada')

        }
        else {
            alert("La cantidad es mayor a la solicitada o se asignaron todos los lotes ");
        }


    });

    $(".addLoteI").click(function () {

        var data = ofunciones.row('.selected').data();
        idtx = ofunciones.row('.selected').index();

        console.log('agregar lote')
        console.log(data);
        console.log(idtx);

        var xcod = data[0];
        var xcodlocal = data[1];
        var xdes = data[2];
        var xund = data[4];
        var xcontrol = data[15];

        $("#xcodearti").val(xcod);
        $("#xcodelocal").val(xcodlocal);
        $("#xdescarti").val(xdes);
        $("#xund").val(xund);
        $("#xcontrol").val(xcontrol);
        $("#xctdadfalta").val(xcontrol);

    });

    /*orden de pedido*/
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
                        [   { "data": "ncode_orpe" },
                            { "data": "fecha" },
                            { "data": "numeracion" },
                            { "data": "srazon_cliente" },
                            { "data": "ncode_alma" }
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0,4]
                    },
                    {
                        "sClass": "my_class",
                        "aTargets": []
                        }],
                    select: {
                        style: 'multi'
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

        bhasOP = true;

        //var data = ordentable.row('.selected').data();
        let rowsData = ordentable.rows({ selected: true }).data();

        if (rowsData.length == 0) {
            alert("Seleccionar Orden de Pedido");
            return;
        }
        var scode_orpe = '';
        var sdocumentos = '';
        var razons = rowsData[0].srazon_cliente;
        var almas = rowsData[0].ncode_alma;
        /*validar */
        for (var i = 0; i < rowsData.length; i++) {

            if (razons != rowsData[i].srazon_cliente || almas != rowsData[i].ncode_alma ) {
                alert("Las ordenes de pedido deben pertenecer al mismo cliente y almacen");
                return;
            }

            scode_orpe += rowsData[i].ncode_orpe + '|';
            sdocumentos += rowsData[i].numeracion + '|';

        }

        //console.log(scode_orpe);

        fnCargaOrdenPedido(scode_orpe,sdocumentos);

    });

    $("#btnpedidocerrarx").click(function () {
        ordentable.destroy();
    });

    $("#btnpedidocerrar").click(function () {
        ordentable.destroy();
    });



    //$(".delLoteI").click(function () {

    //    var data = olotes.row('.selected').data();
    //    olotes.rows('.selected').remove().draw(false);

    //    fnActualizarCtdadLote(data[0], data[3]);

    //});
});



function Sales_save() {

    //console.log('nueva guia');
    var guiaViewCuotas = {
        "ncode_guiacu": "", "sfecharegistro": "", "nvalor_guiacu": ""
    };

    var guiaViewLotes = {
        "ncode_arti": "", "ncant_guialote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
    };


    var guiaViewDetas = {
        "ncode_arti": "", "ncant_guiadet": "", "npu_guiadet": "","ncode_umed":"","ncode_orpe":"",
        //"ndscto2_guiadet": "", "nexon_guiadet": "", "nafecto_guiadet": "", "besafecto_guiadet": "",
        //"ncode_alma": "", "ndsctomax_guiadet": "", "ndsctomin_guiadet": "", "ndsctoporc_guiadet": "", "bicbper_guiadet": "",
        "sdesc": ""
    };

    var guiaView = {
        "ncode_guia": "", "sfemov_guia": "", "smone_guia": "", "ntc_guia": "", "sobse_guia": "","ncode_tran":"","ncode_dose":"",
        "sserie_guia": "", "snume_guia": "", "ncode_alma": "", "ncode_cliente": "", "ncode_docu": "","scode_orpe":"","sserienume_orpe":"",
        "ncode_clidire": "", "ncode_mone": "", "ncode_tiguia": "", "ndestino_alma": "", "stipo_guia": "", "guiaViewDetas": [],
        "guiaViewLotes": [], "guiaViewCuotas": [],
        "sfeguia_guia": "", "sfevenci_guia": "", 
        "nbrutoex_guia": "", "nbrutoaf_guia": "",
        "ndctoex_guia": "", "ndsctoaf_guia": "", "nsubex_guia": "",
        "nsubaf_guia": "", "nigvex_guia": "", "nigvaf_guia": "", "ntotaex_guia": "",
        "ntotaaf_guia": "", "ntotal_guia": "", "ntotalMN_guia": "", "ntotalUs_guia": "",
        "ncode_vende": "", "ncode_orpe": "","ncode_fopago":"","ncode_venzo":"",
        "nvalIGV_guia": "", "nicbper_guia": "", "scode_compra": "",

    };

    guiaView.ncode_guia = $('#ncode_guia').val();
    guiaView.ncode_docu = $("#ncode_docu option:selected").val();
    guiaView.ncode_dose = $("#sserie_guia option:selected").val();
    guiaView.sserie_guia = $("#sserie_guia option:selected").text();
    guiaView.snume_guia = $('#snume_guia').val();
    guiaView.sfemov_guia = $('#dfemov_guia').val();
    guiaView.ncode_cliente = $('#COD_CLIENTE').val();
    guiaView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    guiaView.smone_guia = $('#smone_guia').val();
    guiaView.ntc_guia = $('#ntc_guia').val();
    guiaView.sobse_guia = $('#sobse_guia').val();
    guiaView.scode_compra = $('#scode_compra').val();
    guiaView.ncode_tiguia = $("#ncode_tiguia option:selected").val();
    guiaView.ncode_alma = $("#ncode_alma option:selected").val();
    guiaView.ndestino_alma = $("#ndestino_alma option:selected").val();
    guiaView.stipo_guia = $("#ncode_tiguia option:selected").text().substring(0, 1);
    guiaView.ncode_mone = $("#ncode_mone option:selected").val();
    guiaView.scode_orpe = $("#ncode_orpe").val();
    guiaView.sserienume_orpe = $("#sserienume_orpe").val();
    guiaView.nbrutoex_guia = $('#nbrutoex_guia').val();
    guiaView.nbrutoaf_guia = $('#nbrutoaf_guia').val();
    guiaView.ndctoex_guia = $('#ndctoex_guia').val();
    guiaView.ndsctoaf_guia = $('#ndsctoaf_guia').val();
    guiaView.nsubex_guia = $('#nsubex_guia').val();
    guiaView.nsubaf_guia = $('#nsubaf_guia').val();
    guiaView.nigvex_guia = $('#nigvex_guia').val();
    guiaView.nigvaf_guia = $('#nigvaf_guia').val();
    guiaView.ntotaex_guia = $('#ntotaex_guia').val();
    guiaView.ntotaaf_guia = $('#ntotaaf_guia').val();
    guiaView.ntotal_guia = $('#ntotal_guia').val();
    guiaView.ntotalMN_guia = $('#ntotalMN_guia').val();
    guiaView.ntotalUs_guia = $('#ntotalUs_guia').val();
    guiaView.nvalIGV_guia = $('#nvalIGV_guia').val();
    //guiaView.ncode_vende = $("#ncode_vende option:selected").val();
    guiaView.bclienteagretencion = $('input:checkbox[name=bclienteagretencion]:checked').val();
    guiaView.ncuotas_guia = $('#ncuotas_guia').val();
    guiaView.ncuotavalor_guia = $('#ncuotavalor_guia').val();
    guiaView.ncuotadias_guia = $('#ncuotadias_guia').val();
    guiaView.sglosadespacho_guia = $('#sglosadespacho_guia').val();
    guiaView.bflete_guia = $('input:checkbox[name=bflete_guia]:checked').val();
    guiaView.ncode_fopago = $("#ncode_fopago option:selected").val();
    guiaView.ncode_vende = $("#ncode_vende option:selected").val();
    guiaView.ncode_venzo = $("#ncode_vende option:selected").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        guiaViewDetas.ncode_arti = oTable[i][0];
        guiaViewDetas.sdesc = oTable[i][2];
        guiaViewDetas.ncant_guiadet = oTable[i][3];
        guiaViewDetas.npu_guiadet = oTable[i][5];
        guiaViewDetas.ncode_umed = oTable[i][7];
        guiaViewDetas.ncode_orpe = oTable[i][14];
        guiaView.guiaViewDetas.push(guiaViewDetas);

        var guiaViewDetas = {
            "ncode_arti": "", "ncant_guiadet": "", "npu_guiadet": "", "ncode_umed": "", "sdesc": "","ncode_orpe":""
        };

    }

    var otbly = $('#tblLote').dataTable();
    var nrowsy = otbly.fnGetData().length;
    var oTabley = otbly.fnGetData();

    for (var i = 0; i < nrowsy; i++) {

        console.log(oTabley[i][3]);

        guiaViewLotes.ncode_arti = oTabley[i][0];
        guiaViewLotes.sdesc = oTabley[i][2];
        guiaViewLotes.ncant_guialote = oTabley[i][3];
        guiaViewLotes.sdesc_lote = oTabley[i][5];
        guiaViewLotes.sfvenci_lote = oTabley[i][6]
        guiaViewLotes.ncode_lote = oTabley[i][7]
        guiaViewLotes.ncode_alma = $("#ncode_alma option:selected").val();

        guiaView.guiaViewLotes.push(guiaViewLotes);

        guiaViewLotes = {
            "ncode_arti": "", "ncant_guialote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
        };


    }

    var otblc = $('#tblcuota').dataTable();
    var nrowsc = otblc.fnGetData().length;
    var oTablec = otblc.fnGetData();

    for (var i = 0; i < nrowsc; i++) {

        guiaViewCuotas.ncode_guiacu = oTablec[i][0];
        guiaViewCuotas.sfecharegistro = oTablec[i][1];
        guiaViewCuotas.nvalor_guiacu = oTablec[i][2];
        guiaView.guiaViewCuotas.push(guiaViewCuotas);

        guiaViewCuotas = {
            "ncode_guiacu": "", "sfecharegistro": "", "nvalor_guiacu": ""
        };
    }

     console.log(guiaView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlguiaCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(guiaView) },
        success: function (result) {
            console.log(result.Mensaje)
            switch (result.Success) {
                case 1:
                    window.location.href = urlguiaLista;
                    alert(result.Mensaje);
                    break;
                case 2:
                    //console.log(urlguiaCobro);
                    //urlguiaCobro = urlguiaCobro.replace("param-id", encodeURIComponent(result.CtaCo))
                    //    .replace("param-mensaje", encodeURIComponent(result.Mensaje));
                    ////console.log(urlguiaCobro);
                    //window.location.href = urlguiaCobro;
                    break;
                case 3:
                    alert(result.Mensaje);
                    $('#btnguia').show();
                    $('#btnpro').hide();
                    break;
                default:
                    window.location.href = urlguiaLista;
                    alert(result.Mensaje);
            }
        },
        error: function (ex) {
            alert('No se puede registrar guia' + ex);
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
    var monedaVenta = $('#smone_guia').val();
    var TCambio = parseFloat($('#ntc_guia').val());

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


    $("#ndsctoaf_guia").val(DSCTO_AFECTO);
    $("#ndctoex_guia").val(DSCTO_EXON);
    $("#nbrutoaf_guia").val(SUBT_AFECTO);
    $("#nbrutoex_guia").val(SUBT_EXON);

    $("#nsubaf_guia").val(SUBT_AFECTO);
    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    $("#nsubex_guia").val(SUBT_EX);
    $("#ntotaex_guia").val(SUBT_EXON);

    var TOTAL_AFEC = 0, SUBT_AFEC = 0, IGV_AF = 0;


    if (conf_PrecioIGV == "on") {
        TOTAL_AFEC = parseFloat(SUBT_AFECTO);
        SUBT_AFEC = parseFloat(TOTAL_AFEC) / (1 + (parseInt(CONFIG_IGV) / 100));
        IGV_AF = parseFloat(TOTAL_AFEC) - parseFloat(SUBT_AFEC);

        $("#ntotaaf_guia").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nsubaf_guia").val(SUBT_AFEC.toFixed(conf_decimal));
        $("#nigvaf_guia").val(IGV_AF.toFixed(conf_decimal));

    }
    else {
        IGV_AF = (parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV) / parseInt(100)));
        TOTAL_AFEC = (parseFloat(SUBT_AFECTO) + parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV) / parseInt(100)));

        $("#ntotaaf_guia").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nigvaf_guia").val(IGV_AF.toFixed(conf_decimal));

    }

    var TOTAL = parseFloat(TOTAL_AFEC) + parseFloat(SUBT_EX);

    $("#ntotal_guia").val(TOTAL.toFixed(conf_decimal));

    //valor de retencion
    var retencion = 1;
    $("#ntotaretencion_guia").val(0);
    if ($('input:checkbox[name=bclienteagretencion]:checked').val()) {
        retencion = conf_poretencion;
        if (TOTAL > 700) {

            TOTAL = TOTAL * (1 - retencion);
            $("#ntotaretencion_venta").val(TOTAL.toFixed(conf_decimal));

        }
    }

    //numero de cuotas - valor de cuota
    var xncuotas_venta = $('#ncuotas_guia').val();
    $('#ncuotavalor_guia').val(0);
    if (xncuotas_venta != '' && xncuotas_venta != '0') {
        var xncuotavalor_venta = TOTAL / xncuotas_venta;
        $('#ncuotavalor_guia').val(xncuotavalor_venta.toFixed(conf_decimal));
    }
    CuotasLista(xncuotas_venta, xncuotavalor_venta);


}

function CuotasLista(nrocuotas, valorcuota) {

    cuotastable.rows().remove().draw(false);

    if (nrocuotas == '' || nrocuotas == '0' || beditar == true) {
        return;
    }

    var options = { year: 'numeric', month: '2-digit', day: '2-digit' };

    var sfecha = $("#dfemov_guia").val();
    var dias = $('#ncuotadias_guia').val();
    var valorcuota = $('#ncuotavalor_guia').val();
    var nrocuotas = $('#ncuotas_guia').val();

    //obteniendo nueva fecha
    var parts = sfecha.split("/");
    var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
    fecha.setDate(fecha.getDate() + parseInt(dias));
    sfecha = new Date(fecha).toLocaleDateString("es-PE", options);


    for (var i = 0; i < nrocuotas; i++) {
        cuotastable.row.add([i, sfecha, valorcuota]).draw();
        parts = sfecha.split("/");
        fecha = new Date(parts[2], parts[1] - 1, parts[0]);
        fecha.setDate(fecha.getDate() + parseInt(dias));
        sfecha = new Date(fecha).toLocaleDateString("es-PE", options);
    }
}


function ComparaPrecio(Precio, PrecioOrigen) {

    var xprecio = parseFloat(Precio);

    if (bhasOP) {

        if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
            xprecio = parseFloat(PrecioOrigen);
        }
    }

    return xprecio.toFixed(conf_decimal);

}
function ComparaCantidad(Cantidad, CantidadOrigen) {
    console.log('valida cantidad');

    var xcantidad = parseFloat(Cantidad);
    //Verificar la cantidad origen sino viene de pedido no hay control de cantidades
    if (stipo == "S") {

        if (bhasOP) {

            if (CantidadOrigen > 0) {

                if (parseFloat(Cantidad) > parseFloat(CantidadOrigen)) {
                    xcantidad = parseFloat(CantidadOrigen);
                }
            }
        }
    }

    return xcantidad.toFixed(conf_decimal);
}
function fnFormaPagoDiasFecha(cod_pago) {

    $.ajax({
        type: 'POST',
        url: urlGetDiasFormaPago,
        dataType: 'json',
        data: { ncode_fopago: cod_pago },
        success: function (fopago) {
            //console.log(fopago);
            $.each(fopago, function (i, dias) {

                var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
                var m = $("#dfemov_guia").val();
                var parts = m.split("/");
                var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
                fecha.setDate(fecha.getDate() + parseInt(dias.dias));
                var xfecha = new Date(fecha).toLocaleDateString("es-PE", options);
            //    $('#dfevenci_venta').val(xfecha);
            //    console.log(xfecha);
            });

        },
        error: function (ex) {
            alert('No se pueden recuperar las areas.' + ex);
        }
    });
    return false;

}


function fnlimpiar() {

    $('#COD_CLIENTE').val();
    $("#NRO_DCLIENTE").empty();
    $('#sobse_guia').val();
    $('#ncode_compra').val();
}


function fnStockValida(codigo, cantidad, almacen) {
    //console.log(codigo);
    console.log(stipo);
    if (stipo == 'S') {

    $.ajax({
        type: 'POST',
        url: urlStockValida,
        dataType: 'json',
        data: { ncode_arti: codigo, ncode_alma: almacen },
        success: function (resudisponible) {

            if (resudisponible < cantidad) {
                var mensaje = 'El articulo no tiene stock disponible, solo hay  ' + resudisponible + 'unidades'
                alert(mensaje);
            }

            //console.log(rpta);
            //callback(rpta);
        },
        error: function (ex) {
            alert('No se puede recuperar el stock disponible' + ex);
        }
    });

    }
}
function fnmensaje(rpta) {
    if (rpta == false) {
        alert('el articulo no tiene disponible');
    }
}

function fnTransferencia() {

    $('.destino').hide();
    
    var stipo = $("#ncode_tiguia option:selected").text().substring(0, 1);

    if (stipo == 'T') {
        $('.destino').show();
    }
    else {
        $('#ndestino_alma').val('');
    }
}

function fnMovimiento() {
    $('.addLoteI').hide();
    $('.addLote').hide();

    stipo = $("#ncode_tiguia option:selected").text().substring(0, 1);

    if (stipo == 'I') {
        $('.addLoteI').show();
    }
    if (stipo == 'S') {
        $('.addLote').show();
    }
}
function fnActualizarCtdadLote(codArt, ctdad) {
    console.log(codArt);

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    var codigo = '';
    var ctdadx = '0';
    for (var i = 0; i < nrowsx; i++) {

        codigo = oTable[i][0];
        ctdadx = oTable[i][15];

        if (ctdadx == null || ctdadx == '') {
            ctdadx = 0;
        }

        console.log(ctdadx);
        console.log(ctdad);

        if (codigo == codArt) {
            ctdadx = parseFloat(ctdadx) + parseFloat(ctdad);
            console.log(ctdadx);
            ofunciones.cell({ row: i, column: 15 }).data(ctdadx).draw(false);
            break;
        }
    }
}

function fnActualizaCtdadRestanteLote(xcodart, ctdadactual) {

    var otbly = $('#tblLote').dataTable();
    var nrowsy = otbly.fnGetData().length;
    var oTabley = otbly.fnGetData();
    var sumx = 0; 

    for (var i = 0; i < nrowsy; i++) {

        if (xcodart === oTabley[i][0]) {

            sumx = parseFloat(sumx) + parseFloat(oTabley[i][3])
        }

    }

    var cantfaltante = ctdadactual - sumx
   //console.log('calcula lote')
   // console.log(sumx);
   // console.log(cantfaltante);
    return cantfaltante

}

function fnCargaOrdenPedido(codigo,sdocumentos) {

    fnlimpiar();
    //console.log(codigo);
    $.ajax({
        type: 'POST',
        url: urlGetOrdenPedidoVenta,
        dataType: 'json',
        data: { scode_orpe: codigo,documentos:sdocumentos },
        success: function (venta) {
            console.log(venta);

            $('#sdesc_cliente').val(venta.scliente);
            $('#COD_CLIENTE').val(venta.ncode_cliente);
            $('#sruc_cliente').val(venta.sruc);
            $('#sdni_cliente').val(venta.sdni);
            $('#sobse_guia').val(venta.sobse_venta);
            $("#ncode_mone").val(venta.ncode_mone);
            $("#ncode_orpe").val(codigo);
            $("#ncode_alma").val(venta.ncode_alma);
            $("#ncode_vende").val(venta.ncode_venzo);
            $("#scode_compra").val(venta.scode_compra);
            $("#sserienume_orpe").val(venta.sserienumero);
            $('#bclienteagretencion').prop('checked', false);
            if (venta.bclienteagretencion) {
                $('#bclienteagretencion').prop('checked', true);
            }
            $('#ncuotas_guia').val(venta.ncuotas_venta);
            $('#ncuotavalor_guia').val(venta.ncuotavalor_venta);
            $('#nretencionvalor_guia').val(venta.nretencionvalor_venta);

            $('#bflete_guia').prop('checked', false);
            if (venta.bflete_guia) {
                $('#bflete_guia').prop('checked', true);
            }

            $('#ncuotadias_guia').val(venta.ncuotadias_venta);
            $('#sglosadespacho_guia').val(venta.sglosadespacho_venta);



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
                    , venta.ventaViewDetas[i].ncant_vedeta
                    , venta.ventaViewDetas[i].ncode_orpe
                    , venta.ventaViewDetas[i].ncant_vedeta
                    , venta.ventaViewDetas[i].bdescarga_vedeta
                ]).draw();
            }

            fnclienteDire();
            $("#NRO_DCLIENTE").val(venta.ncode_clidire);
            $("#ncode_fopago").val(venta.ncode_fopago);
            fnFormaPagoDiasFecha(venta.ncode_fopago);

        },
        error: function (ex) {
            alert('No se puede recuperar datos de la orden de pedido' + ex);
        }
    });

    Totales(conf_igv, conf_decimal, conf_icbper);

    return false;


}

function fnDocumentoSerie(ncode) {
    //console.log($("#ncode_docu").val());
    //var ncode = $("#ncode_docu option:selected").val();
    var xcode = 0;
    $("#sserie_guia").empty();
    $.ajax({
        type: 'POST',
        url: urlGetDocuSerie,
        dataType: 'json',
        data: { ncode_docu: ncode },
        success: function (areas) {

            console.log(areas);

            if (areas.length == 0) {

                return;
            }

            var code = areas[0].ncode_dose;
            var des = areas[0].serie;

            $.each(areas, function (i, area) {
                $("#sserie_guia").append('<option value="'
                    + area.ncode_dose + '">'
                    + area.serie + '</option>');
            });

            $("#sserie_guia").val(code);

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
                $('#snume_guia').val(doc);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}

