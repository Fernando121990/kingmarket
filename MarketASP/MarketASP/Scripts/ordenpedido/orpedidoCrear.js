
var ofunciones=null;
var mattable;
var lotetable;
var preciotable;
var almatable;
var cuotastable = null;
var conf_decimal = 2;
var conf_moneda = 0;
var CONFIG_dscto = 'NO';
var conf_PrecioIGV;
var conf_poretencion;
var beditar = false;

$(document).ready(function () {
    var code = 0;
    var conf_igv = 0;

    $('#btnpro').hide();

    code = $("#ncode_orpe").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();
    conf_PrecioIGV = $("input[type=checkbox][name=bprecioconigv]:checked").val();
    conf_poretencion = $("#poretencion").val();

    /*DESHABILITANDO CUOTAS*/

    $("#ncuotas_orpe").prop("disabled", true);
    $("#ncuotadias_orpe").prop("disabled", true);

    /*DATATABLES*/
    cuotastable = $('#tblcuota').DataTable({
        "dom": 'T<"clear">rtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0]
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
        fnDocumentoSerieNumero();
    }

    if (code > 0) {

        var cod_cliente = $('#COD_CLIENTE').val();
        var cod_fpago = $('#ncode_fopago').val();
        var cod_clidire = $('#ncode_clidire').val();


        fnclienteDire();

        $("#NRO_DCLIENTE").val(cod_clidire);

        fnclienteFPago(cod_cliente,cod_fpago);

        beditar = true;
    }

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

    $("#sseri_orpe").change(function () {
        
        fnDocumentoSerieNumero();
    });

    /*CUOTAS*/
    $("#ncuotas_orpe").change(function () {
        beditar = false
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#ncuotadias_orpe").change(function () {
        beditar = false
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    /*cambio de fecha*/
    $('#dfeorpeo_orpe').change(function () {

        var codpago = $("#nro_fopago option:selected").val();

        if (typeof codpago !== 'undefined') {

            fnFormaPagoDiasFecha(codpago);
        }

        //console.log(this.value);

        fnTipoCambioFecha('OP','venta',this.value)

    });

    /*formas de pago*/
    $('#nro_fopago').change(function () {
        
        var codpago = $("#nro_fopago option:selected").val();

        fnFormaPagoDiasFecha(codpago);

        var snpago = $("#nro_fopago option:selected").text();

        $("#ncuotas_orpe").prop("disabled", true);
        $("#ncuotadias_orpe").prop("disabled", true);
        $("#ncuotas_orpe").val(1);
        $("#ncuotadias_orpe").val('');

        ///console.log(snpago.indexOf("FNEG"));

        if (snpago.indexOf("FNEG") != -1 || snpago.indexOf("LETRA") != -1) {
            $("#ncuotas_orpe").prop("disabled", false);
            $("#ncuotadias_orpe").prop("disabled", false);
        }

        Totales(conf_igv, conf_decimal, conf_icbper);
        
    })


     ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0, 6, 7, 8, 9, 10]
        },
        {
            "sClass": "my_class",
            "aTargets": [3, 5]
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlEditar, {
                "callback": function (sValue, y) {
                    var aPos = ofunciones.row(this).index();
                    var idx = ofunciones.column(this).index();
                    switch (idx) {
                        case 3: //quantity column
                            //console.log('columna cantidad');
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            //console.log(xvalue);
                            var subto = xcant * parseFloat(xvalue);
                            ofunciones.cell(aPos, 11).data(subto.toFixed(conf_decimal)).draw;
                            break;
                        case 5: //price column
                           console.log('columna precio');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var xtope = ofunciones.cell(aPos, 12).data();
                            var yValue = ComparaPrecio(sValue, xvalue,xtope);
                            var subto = xcant * yValue
                            ofunciones.cell(aPos, idx).data(yValue).draw;
                            ofunciones.cell(aPos, 11).data(subto.toFixed(conf_decimal)).draw;
                            break;
                        default:
                            console.log('el valor por default')
                            ofunciones.cell(aPos, idx).data(sValue).draw;

                    }

                    Totales(conf_igv, conf_decimal, conf_icbper);
                },
                "submitdata": function (value, settings) {
                    console.log('submiit data');
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

        ofunciones.rows('.selected').remove().draw(false);
    });

    $(".addMat").click(function () {

        var codalmacen = $("#ncode_alma option:selected").val();
        //console.log(codalmacen);
        if (typeof codalmacen === 'undefined') {
            alert('Seleccionar Almacen');
            return
        }

        $.ajax({
            type: "Post",
            url: urlArticulos,
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { ncode_alma: codalmacen},
            success: function (resultado) {
                console.log(resultado);
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
                            { "data": "npreciotope_arti" }
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0, 8, 9, 10,11,12]
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

    $("#btnmate").click(function () {
        var data = mattable.row('.selected').data();
        var xcan = 1;
        var xesta = 0;

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Precio, data.Precio, data.ncode_umed,
            data.bafecto_arti, data.bisc_arti, data.bdscto_arti, xcan * data.Precio, data.npreciotope_arti]).draw();
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });
    $("#btnalmacerrarx").click(function () {
        almatable.destroy();
    });

    $("#btnalmacerrar").click(function () {
        almatable.destroy();
    });
    $("#btnpreciocerrarx").click(function () {
        preciotable.destroy();
    });

    $("#btnpreciocerrar").click(function () {
        preciotable.destroy();
    });

    $("#btnorpe").click(function () {

        //Validaciones

        if ($("#COD_CLIENTE").val().length < 1) {
            alert("Seleccione Cliente");
            return false;
        };

        if ($("#NRO_DCLIENTE option:selected").text().length < 1) {
            alert("Seleccione Ubicacion");
            return false;
        };


        if ($("#ncode_vende option:selected").text().length < 1) {
            alert("Seleccione Vendedor");
            return false;
        };

        if ($("#nro_fopago option:selected").text().length < 1) {
            alert("Seleccione Forma de pago");
            return false;
        };

        if ($("#ncode_alma option:selected").text().length < 1) {
            alert("Seleccione Almacen");
            return false;
        };

        if ($("#ntc_orpe").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        if ($("#sseri_orpe").val().length < 1) {
            alert("Ingrese serie del documento");
            return false;
        }

        if ($("#snume_orpe").val().length < 1) {
            alert("Ingrese número del documento");
            return false;
        }



        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_venta').val();


        //console.log(ncode_docu);
        //console.log(ntotal);


        if (ncode_docu == 10 && $("#sruc_cliente").val().length < 1) {
            alert("Ingrese RUC");
            return false;
        }

        if (ncode_docu == 11 && $("#sdni_cliente").val().length < 1) {
            alert("Ingrese DNI");
            return false;
        }

        if (ncode_docu = 11 && $("#COD_CLIENTE").val() == 5 && ntotal > 700) {
            alert("La boleta supera los S/ 700, debe registrar al cliente.");
            return false;
        }

        /////******
        var otbly = $('#tbl').dataTable();
        var nrowsy = otbly.fnGetData().length;
        //console.log(nrowsy);
        if (nrowsy < 1) {
            alert("Seleccione Articulos");
            return false;
        }

        $('#btnorpe').hide();
        $('#btnpro').show();

        Sales_save();
    });


    var tblpedido;
///lista de pedidos
    $(".btnpedidos").click(function () {
        var data = ofunciones.row('.selected').data();
        //console.log(data[0]);
        var xcodarticulo = data[0];
        //console.log(xcodarticulo);

        $.ajax({
            type: 'POST',
            url: urlPedidoPrecio,
            dataType: 'json',
            data: { ncode_arti: xcodarticulo },
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito pedido');

               preciotable = $('#preciotabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "fecha" },
                            { "data": "documento" },
                            { "data": "ncant_orpedeta" },
                            { "data": "npu_orpedeta" },
                            { "data": "sdesc_vende" },
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
//lista de almacenes
    $(".btnalma").click(function () {
        var data = ofunciones.row('.selected').data();
        var xcodarticulo = data[0];

        $.ajax({
            type: 'POST',
            url: urlKardex,
            dataType: 'json',
            data: { ncode_arti: xcodarticulo},
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito almacen');

                almatable = $('#almatabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "sdesc_alma" },
                        { "data": "sdesc1_arti" },
                            { "data": "STOCK" },
                            { "data": "DISPONIBLE"}
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

/*lista de lotes*/

    $(".btnverlotes").click(function () {

        var data = ofunciones.row('.selected').data();

        var ncodealma = $("#ncode_alma option:selected").val();

        $.ajax({
            type: "Post",
            url: urlGetLoteDisponible,
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { ncode_alma: ncodealma, ncode_arti: data[0], fvenci_lote: '', sdesc_lote: '' },
            success: function (resultado) {
                console.log(resultado);

                if (resultado.length == 0) {
                    alert("No hay lotes disponibles para venta")
                    return;
                }

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
                        "aTargets": [0, 3, 7, 8]
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

    $("#btnverlotecerrar").click(function () {
        lotetable.destroy();
    });
    $("#btnverlotecerrarx").click(function () {
        lotetable.destroy();
    });

});



function Sales_save() {

    //console.log('nueva venta');

    var ordenpedidoViewCuotas = {
        "ncode_orpecu": "", "sfecharegistro": "", "nvalor_orpecu": ""
    };


    var ordenpedidoViewDetas = {
        "ncode_arti": "", "ncant_orpedeta": "", "npu_orpedeta": "", "ndscto_orpedeta": "",
        "ndscto2_orpedeta": "", "nexon_orpedeta": "", "nafecto_orpedeta": "", "besafecto_orpedeta": "",
        "ncode_alma": "", "ndsctomax_orpedeta": "", "ndsctomin_orpedeta": "", "ndsctoporc_orpedeta": "",
        "npuorigen_orpedeta": ""
    };

    var ordenpedidoView = {
        "ncode_orpe": "", "ncode_alma": "", "ncode_mone": "","ncode_dose":"",
        "ncode_docu": "", "sseri_orpe": "", "snume_orpe": "",
        "sfeordenpedido_orpe": "", "sfevenci_orpe": "", "sfedespacho_orpe": "", "ncode_cliente": "",
        "ncode_clidire": "", "smone_orpe": "", "ntc_orpe": "",
        "ncode_fopago": "", "sobse_orpe": "", "scode_compra": "",
        "nbrutoex_orpe": "", "nbrutoaf_orpe": "",
        "ndctoex_orpe": "", "ndsctoaf_orpe": "", "nsubex_orpe": "",
        "nsubaf_orpe": "", "nigvex_orpe": "", "nigvaf_orpe": "", "ntotaex_orpe": "",
        "ntotaaf_orpe": "", "ntotal_orpe": "", "ntotalMN_orpe": "", "ntotalUs_orpe": "",
        "ncuotas_orpe": "", "ncuotavalor_orpe": "", "ncuotadias_orpe": "", "sglosadespacho_orpe": "",
        "bflete_orpe":"",
        "nvalIGV_orpe": "", "ncode_vende": "", "bclienteagretencion": "", "ordenpedidoViewDetas": [],
        "ordenpedidoViewCuotas": []

    };

    ordenpedidoView.ncode_orpe = $('#ncode_orpe').val();
    ordenpedidoView.ncode_docu = $("#ncode_docu option:selected").val();
    ordenpedidoView.ncode_dose = $("#sseri_orpe option:selected").val();
    ordenpedidoView.sseri_orpe = $("#sseri_orpe option:selected").text();
    ordenpedidoView.snume_orpe = $('#snume_orpe').val();
    ordenpedidoView.sfeordenpedido_orpe = $('#dfeorpeo_orpe').val();
    ordenpedidoView.sfevenci_orpe = $('#dfevenci_orpe').val();
    ordenpedidoView.sfedespacho_orpe = $('#dfdespacho_orpe').val();
    ordenpedidoView.ncode_cliente = $('#COD_CLIENTE').val();
    ordenpedidoView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    ordenpedidoView.smone_orpe = $('#smone_orpe').val();
    ordenpedidoView.ntc_orpe = $('#ntc_orpe').val();
    ordenpedidoView.ncode_fopago = $("#nro_fopago option:selected").val();
    ordenpedidoView.sobse_orpe = $('#sobse_orpe').val();
    ordenpedidoView.scode_compra = $('#scode_compra').val();
    ordenpedidoView.nbrutoex_orpe = $('#nbrutoex_orpe').val();
    ordenpedidoView.nbrutoaf_orpe = $('#nbrutoaf_orpe').val();
    ordenpedidoView.ndctoex_orpe = $('#ndctoex_orpe').val();
    ordenpedidoView.ndsctoaf_orpe = $('#ndsctoaf_orpe').val();
    ordenpedidoView.nsubex_orpe = $('#nsubex_orpe').val();
    ordenpedidoView.nsubaf_orpe = $('#nsubaf_orpe').val();
    ordenpedidoView.nigvex_orpe = $('#nigvex_orpe').val();
    ordenpedidoView.nigvaf_orpe = $('#nigvaf_orpe').val();
    ordenpedidoView.ntotaex_orpe = $('#ntotaex_orpe').val();
    ordenpedidoView.ntotaaf_orpe = $('#ntotaaf_orpe').val();
    ordenpedidoView.ntotal_orpe = $('#ntotal_orpe').val();
    ordenpedidoView.ntotalMN_orpe = $('#ntotalMN_orpe').val();
    ordenpedidoView.ntotalUs_orpe = $('#ntotalUs_orpe').val();
    ordenpedidoView.nvalIGV_orpe = $('#nvalIGV_orpe').val();
    ordenpedidoView.ncode_alma = $("#ncode_alma option:selected").val();
    ordenpedidoView.ncode_mone = $("#ncode_mone option:selected").val();
    ordenpedidoView.ncode_vende = $("#ncode_vende option:selected").val();
    ordenpedidoView.bclienteagretencion = $('input:checkbox[name=bclienteagretencion]:checked').val(); 
    ordenpedidoView.ncuotas_orpe = $('#ncuotas_orpe').val();
    ordenpedidoView.ncuotavalor_orpe = $('#ncuotavalor_orpe').val();
    ordenpedidoView.ncuotadias_orpe = $('#ncuotadias_orpe').val();
    ordenpedidoView.sglosadespacho_orpe = $('#sglosadespacho_orpe').val();
    ordenpedidoView.bflete_orpe = $('input:checkbox[name=bflete_orpe]:checked').val();


    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        ordenpedidoViewDetas.ncode_arti = oTable[i][0];
        ordenpedidoViewDetas.ncant_orpedeta = oTable[i][3];
        ordenpedidoViewDetas.npu_orpedeta = oTable[i][5];
        ordenpedidoViewDetas.npuorigen_orpedeta = oTable[i][6];
        ordenpedidoViewDetas.ndscto_orpedeta = oTable[i][6] - oTable[i][5];
        ordenpedidoViewDetas.nexon_orpedeta = oTable[i][5] * oTable[i][3];
        ordenpedidoViewDetas.nafecto_orpedeta = oTable[i][5] * oTable[i][3];
        ordenpedidoViewDetas.besafecto_orpedeta = oTable[i][8];
        //ordenpedidoViewDetas.ndsctoporc_orpedeta = oTable[i][7];
        ordenpedidoViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        ordenpedidoView.ordenpedidoViewDetas.push(ordenpedidoViewDetas);

        ordenpedidoViewDetas = {
            "ncode_arti": "", "ncant_orpedeta": "", "npu_orpedeta": "", "ndscto_orpedeta": "",
            "ndscto2_orpedeta": "", "nexon_orpedeta": "", "nafecto_orpedeta": "", "besafecto_orpedeta": "",
            "ncode_alma": "", "ndsctomax_orpedeta": "", "ndsctomin_orpedeta": "", "ndsctoporc_orpedeta": ""
        };

    }

    var otblc = $('#tblcuota').dataTable();
    var nrowsc = otblc.fnGetData().length;
    var oTablec = otblc.fnGetData();

    for (var i = 0; i < nrowsc; i++) {

        ordenpedidoViewCuotas.ncode_orpecu = oTablec[i][0];
        ordenpedidoViewCuotas.sfecharegistro = oTablec[i][1];
        ordenpedidoViewCuotas.nvalor_orpecu = oTablec[i][2];
        ordenpedidoView.ordenpedidoViewCuotas.push(ordenpedidoViewCuotas);

        ordenpedidoViewCuotas = {
            "ncode_orpecu": "", "sfecharegistro": "", "nvalor_orpecu": ""
        };
    }



    console.log(ordenpedidoView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlorpeCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(ordenpedidoView) },
        success: function (result) {
            console.log(result.Success)
            switch (result.Success) {
                case 1:
                    window.location.href = urlorpeLista;
                    break;
                default:
                    window.location.href = urlorpeLista;
            }
        },
        error: function (ex) {
            $('#btnorpe').show();
            $('#btnpro').hide();

            alert('No se puede registrar orden de pedido' + ex);
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
    var monedaVenta = $('#smone_orpe').val();
    var TCambio = parseFloat($('#ntc_orpe').val());

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
        var COL_TISC =  oTable[i][9];

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


    $("#ndsctoaf_orpe").val(DSCTO_AFECTO);
    $("#ndctoex_orpe").val(DSCTO_EXON);
    $("#nbrutoaf_orpe").val(SUBT_AFECTO);
    $("#nbrutoex_orpe").val(SUBT_EXON);

    $("#nsubaf_orpe").val(SUBT_AFECTO);
    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    $("#nsubex_orpe").val(SUBT_EX);
    $("#ntotaex_orpe").val(SUBT_EXON);

    var TOTAL_AFEC = 0, SUBT_AFEC = 0, IGV_AF = 0;


    if (conf_PrecioIGV == "on") {
        TOTAL_AFEC = parseFloat(SUBT_AFECTO);
        SUBT_AFEC = parseFloat(TOTAL_AFEC) / (1 + (parseInt(CONFIG_IGV) / 100));
        IGV_AF = parseFloat(TOTAL_AFEC) - parseFloat(SUBT_AFEC);

        $("#ntotaaf_orpe").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nsubaf_orpe").val(SUBT_AFEC.toFixed(conf_decimal));
        $("#nigvaf_orpe").val(IGV_AF.toFixed(conf_decimal));

    }
    else {
        IGV_AF = (parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV)/parseInt(100)));
        TOTAL_AFEC = (parseFloat(SUBT_AFECTO) + parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV)/parseInt(100)));

        $("#ntotaaf_orpe").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nigvaf_orpe").val(IGV_AF.toFixed(conf_decimal));

    }

    var TOTAL = parseFloat(TOTAL_AFEC) + parseFloat(SUBT_EX);

    $("#ntotal_orpe").val(TOTAL.toFixed(conf_decimal));

    //valor de retencion
    var retencion = 1;
    $("#ntotaretencion_orpe").val(0);
    if ($('input:checkbox[name=bclienteagretencion]:checked').val()) {
        retencion = conf_poretencion;
        if (parseFloat(TOTAL) > 700) {

            TOTAL = TOTAL * (1 - retencion);
            $("#ntotaretencion_orpe").val(TOTAL.toFixed(conf_decimal));

        }

    }

    //numero de cuotas - valor de cuota
    var xncuotas_orpe = $('#ncuotas_orpe').val();
    $('#ncuotavalor_orpe').val(0);
    if (xncuotas_orpe != '' && xncuotas_orpe != '0') {
        var xncuotavalor_orpe = TOTAL / xncuotas_orpe;
        $('#ncuotavalor_orpe').val(xncuotavalor_orpe.toFixed(conf_decimal));
    }
    CuotasLista(xncuotas_orpe, xncuotavalor_orpe);

}

function CuotasLista(nrocuotas,valorcuota) {

    

    if (nrocuotas == '' || nrocuotas == '0' || beditar == true) {
        return;
    }

    cuotastable.rows().remove().draw(false);

    var options = { year: 'numeric', month: '2-digit', day: '2-digit' };


    var sfecha = $("#dfeorpeo_orpe").val(); 
    var dias = $('#ncuotadias_orpe').val();
    var valorcuota = $('#ncuotavalor_orpe').val();
    var nrocuotas = $('#ncuotas_orpe').val();

    for (var i = 0; i < nrocuotas; i++) {
        cuotastable.row.add([i, sfecha, valorcuota]).draw();
        var parts = sfecha.split("/");
        var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
        fecha.setDate(fecha.getDate() + parseInt(dias));
        sfecha = new Date(fecha).toLocaleDateString("es-PE", options);
    }
        

}

function ComparaPrecio(Precio, PrecioOrigen,preciotope) {
    console.log('compra precio');
    console.log(Precio);
    console.log(PrecioOrigen);

    var xprecio = parseFloat(Precio);

    if (parseFloat(Precio) < parseFloat(preciotope)) {
        xprecio = parseFloat(PrecioOrigen);
    }

    return xprecio.toFixed(conf_decimal);

}
function fnFormaPagoDiasFecha(codfopago) {
    console.log('dias');
    console.log(codfopago);
    $.ajax({
        type: 'POST',
        url: urlGetDiasFormaPago,
        dataType: 'json',
        data: { ncode_fopago: codfopago },
        //data: { ncode_fopago: $("#ncode_fopago").val() },
        success: function (fopago) {
            //console.log(fopago);
            $.each(fopago, function (i, dias) {

                var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
                var m = $("#dfeorpeo_orpe").val();
                var parts = m.split("/");
                var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
                fecha.setDate(fecha.getDate() + parseInt(dias.dias));
                var xfecha = new Date(fecha).toLocaleDateString("es-PE", options);
                $('#dfevenci_orpe').val(xfecha);
            });

        },
        error: function (ex) {
            alert('No se puede obtener la fecha de pago .' + ex);
        }
    });
    return false;

}
//function fnTipoCambioFecha(saccion,sfecha) {
//    console.log('tipo cambio');
//    console.log(sfecha);
//    $.ajax({
//        type: 'POST',
//        url: urlGetTipoCambioFecha,
//        dataType: 'json',
//        data: { accion:saccion, sFecha: sfecha },
//        success: function (foTipoCambio) {

//            console.log(foTipoCambio);

//            $('#ntc_orpe').val(foTipoCambio[0]);

            

//        },
//        error: function (ex) {
//            alert('No se puede obtener la fecha de pago .' + ex);
//        }
//    });
//    return false;

//}

function fnDocumentoSerieNumero() {
    //console.log($("#ncode_docu").val());
    var ncode = $("#sseri_orpe option:selected").val();

    $.ajax({
        type: 'POST',
        url: urlGetDocuNumero,
        dataType: 'json',
        data: { ncode_dose: ncode },
        success: function (docu) {
            console.log(docu);
            $.each(docu, function (i, doc) {
                ///$('#sseri_orpe').val(doc.serie);
                $('#snume_orpe').val(doc);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}


function fnAgenteRetencion() {
    console.log('retencion agente')
    Totales(conf_igv, conf_decimal, conf_icbper);
}