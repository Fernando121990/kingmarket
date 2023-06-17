var ofunciones=null;
var mattable;
var preciotable;
var almatable;
var conf_decimal = 2;
var conf_moneda = 0;
$(document).ready(function () {
    var code = 0;
    var conf_igv = 0;

    code = $("#ncode_orco").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();
    

    //console.log(conf_igv);
    //console.log(conf_decimal);
    //console.log(conf_icbper);

    //console.log(code);
    //console.log('xx')

    if (typeof code === 'undefined') {
        //  console.log('series');
        fnDocumentoSerieNumero();
    }

    if (code > 0) {
        fnclienteDire();
        //console.log($('#ncode_clidire').val());
        $("#NRO_DCLIENTE").val($('#ncode_clidire').val());

        fnclienteFPago();

        $("#nro_fopago").val($('#ncode_fopago').val());
    }

    $("#ncode_docu").change(function () {
        fnDocumentoSerieNumero();
    });

    //$("#ncode_fopago").change(function () {
    //    fnFormaPagoDiasFecha()
    //});

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
                            console.log('columna cantidad');
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                            var xcant = ofunciones.cell(aPos, 3).data();

                            //console.log(typeof (xcant));
                            //console.log(ofunciones.cell(aPos, 3).data());
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            //console.log(xvalue);
                            var subto = xcant * parseFloat(xvalue);
                            ofunciones.cell(aPos, 11).data(subto.toFixed(conf_decimal)).draw;
                            break;
                        case 5: //price column
                           console.log('columna precio');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var yValue = ComparaPrecio(sValue, xvalue);
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


        $.ajax({
            type: "Post",
            url: urlArticulos,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
                            { "data": "Disponible" },
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
                        "aTargets": [0, 7, 8, 9, 10,11]
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
        data.bafecto_arti, data.bisc_arti, data.bdscto_arti, xcan * data.Precio]).draw();
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

    $("#btnorco").click(function () {

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

        if ($("#ntc_orco").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_venta').val();

        //console.log(ncode_docu);
        //console.log(ntotal);

        if ($("#sseri_orco").val().length < 1) {
            alert("Ingrese serie del documento");
            return false;
        }

        if ($("#snume_orco").val().length < 1) {
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
                            { "data": "ncant_orcodeta" },
                            { "data": "npu_orcodeta" },
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



});



function Sales_save() {

    //console.log('nueva venta');

    var ordenpedidoViewDetas = {
        "ncode_arti": "", "ncant_orcodeta": "", "npu_orcodeta": "", "ndscto_orcodeta": "",
        "ndscto2_orcodeta": "", "nexon_orcodeta": "", "nafecto_orcodeta": "", "besafecto_orcodeta": "",
        "ncode_alma": "", "ndsctomax_orcodeta": "", "ndsctomin_orcodeta": "", "ndsctoporc_orcodeta": "",
        "npuorigen_orcodeta": ""
    };

    var ordenpedidoView = {
        "ncode_orco": "", "ncode_alma": "", "ncode_mone": "",
        "ncode_docu": "", "sseri_orco": "", "snume_orco": "",
        "sfeordenpedido_orco": "", "sfevenci_orco": "", "ncode_cliente": "",
        "ncode_clidire": "", "smone_orco": "", "ntc_orco": "",
        "ncode_fopago": "", "sobse_orco": "", "scode_compra": "",
        "nbrutoex_orco": "", "nbrutoaf_orco": "",
        "ndctoex_orco": "", "ndsctoaf_orco": "", "nsubex_orco": "",
        "nsubaf_orco": "", "nigvex_orco": "", "nigvaf_orco": "", "ntotaex_orco": "",
        "ntotaaf_orco": "", "ntotal_orco": "", "ntotalMN_orco": "", "ntotalUs_orco": "",
        "nvalIGV_orco": "", "ncode_vende": "", "ordenpedidoViewDetas": []

    };

    ordenpedidoView.ncode_orco = $('#ncode_orco').val();
    ordenpedidoView.ncode_docu = $("#ncode_docu option:selected").val();
    ordenpedidoView.sseri_orco = $('#sseri_orco').val();
    ordenpedidoView.snume_orco = $('#snume_orco').val();
    ordenpedidoView.sfeordenpedido_orco = $('#dfeorcoo_orco').val();
    ordenpedidoView.sfevenci_orco = $('#dfevenci_orco').val();
    ordenpedidoView.ncode_cliente = $('#COD_CLIENTE').val();
    ordenpedidoView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    ordenpedidoView.smone_orco = $('#smone_orco').val();
    ordenpedidoView.ntc_orco = $('#ntc_orco').val();
    ordenpedidoView.ncode_fopago = $("#nro_fopago option:selected").val();
    ordenpedidoView.sobse_orco = $('#sobse_orco').val();
    ordenpedidoView.scode_compra = $('#scode_compra').val();
    ordenpedidoView.nbrutoex_orco = $('#nbrutoex_orco').val();
    ordenpedidoView.nbrutoaf_orco = $('#nbrutoaf_orco').val();
    ordenpedidoView.ndctoex_orco = $('#ndctoex_orco').val();
    ordenpedidoView.ndsctoaf_orco = $('#ndsctoaf_orco').val();
    ordenpedidoView.nsubex_orco = $('#nsubex_orco').val();
    ordenpedidoView.nsubaf_orco = $('#nsubaf_orco').val();
    ordenpedidoView.nigvex_orco = $('#nigvex_orco').val();
    ordenpedidoView.nigvaf_orco = $('#nigvaf_orco').val();
    ordenpedidoView.ntotaex_orco = $('#ntotaex_orco').val();
    ordenpedidoView.ntotaaf_orco = $('#ntotaaf_orco').val();
    ordenpedidoView.ntotal_orco = $('#ntotal_orco').val();
    ordenpedidoView.ntotalMN_orco = $('#ntotalMN_orco').val();
    ordenpedidoView.ntotalUs_orco = $('#ntotalUs_orco').val();
    ordenpedidoView.nvalIGV_orco = $('#nvalIGV_orco').val();
    ordenpedidoView.ncode_alma = $("#ncode_alma option:selected").val();
    ordenpedidoView.ncode_mone = $("#ncode_mone option:selected").val();
    ordenpedidoView.ncode_vende = $("#ncode_vende option:selected").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        ordenpedidoViewDetas.ncode_arti = oTable[i][0];
        ordenpedidoViewDetas.ncant_orcodeta = oTable[i][3];
        ordenpedidoViewDetas.npu_orcodeta = oTable[i][5];
        ordenpedidoViewDetas.npuorigen_orcodeta = oTable[i][6];
        ordenpedidoViewDetas.ndscto_orcodeta = oTable[i][6] - oTable[i][5];
        ordenpedidoViewDetas.nexon_orcodeta = oTable[i][5] * oTable[i][3];
        ordenpedidoViewDetas.nafecto_orcodeta = oTable[i][5] * oTable[i][3];
        ordenpedidoViewDetas.besafecto_orcodeta = oTable[i][8];
        ordenpedidoViewDetas.ndsctoporc_orcodeta = oTable[i][6];
        ordenpedidoViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        ordenpedidoView.ordenpedidoViewDetas.push(ordenpedidoViewDetas);

        var ordenpedidoViewDetas = {
            "ncode_arti": "", "ncant_orcodeta": "", "npu_orcodeta": "", "ndscto_orcodeta": "",
            "ndscto2_orcodeta": "", "nexon_orcodeta": "", "nafecto_orcodeta": "", "besafecto_orcodeta": "",
            "ncode_alma": "", "ndsctomax_orcodeta": "", "ndsctomin_orcodeta": "", "ndsctoporc_orcodeta": ""
        };

    }

    // console.log(ordenpedidoView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlorcoCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(ordenpedidoView) },
        success: function (result) {
            console.log(result.Success)
            switch (result.Success) {
                case 1:
                    window.location.href = urlorcoLista;
                    break;
                default:
                    window.location.href = urlorcoLista;
            }
        },
        error: function (ex) {
            alert('No se puede registrar ordenpedido' + ex);
        }
    });

}
function Totales(conf_igv, conf_decimal, conf_icbper) {
    console.log('calculo de totales');
    console.log(conf_igv);
    console.log(conf_decimal);
    console.log(conf_icbper);

    var TOT_AFECTO = 0;
    var TOT_EXON = 0;
    var TOT = 0;
    var SUBT = 0;
    var SUBT_AFECTO = 0;
    var SUBT_EXON = 0;
    var DSCTO = 0;
    var DSCTO_AFECTO = 0;
    var DSCTO_EXON = 0;
    var TOT_ISC = 0;
    CONFIG_IGV = conf_igv;

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();



    for (var i = 0; i < nrowsx; i++) {

        //console.log(i);
        //console.log(nrowsx);

        var AFECTO_ART = oTable[i][8].toString();
        var COL_TISC = oTable[i][9];
        var CANT_DV = oTable[i][3];
        var PU_DV = oTable[i][5];
        var DSCTO_DV = 0;

        TOT = 0;
        TOT_AFECTO = 0;
        TOT_EXON = 0;
        TOT = CANT_DV * PU_DV;
        TOT = TOT - (TOT * DSCTO_DV / 100);

        console.log(AFECTO_ART);

        if (AFECTO_ART.toUpperCase() == 'TRUE' || AFECTO_ART == 'true' || AFECTO_ART == 'True') {
            TOT_AFECTO = TOT_AFECTO + Math.round(TOT, conf_decimal);
            //  console.log('afecto');
        }

        if (AFECTO_ART.toUpperCase() == 'FALSE' || AFECTO_ART == 'false' || AFECTO_ART == 'False') {

            TOT_EXON = TOT_EXON + Math.round(TOT, conf_decimal);
            //console.log('exone');
        }

        SUBT = CANT_DV * PU_DV;
        //console.log('totafecto');
        //console.log(TOT_AFECTO);
        //console.log('totexone');
        //console.log(TOT_EXON);

        if (TOT_AFECTO !== 0) {
            SUBT_AFECTO = SUBT_AFECTO + SUBT;
            DSCTO = (SUBT * (DSCTO_DV / 100));
            DSCTO_AFECTO = DSCTO_AFECTO + DSCTO;
            //      console.log('subafecto');
            //    console.log(SUBT_AFECTO);
        }

        if (TOT_EXON !== 0) {
            SUBT_EXON = SUBT_EXON + SUBT;
            DSCTO = (SUBT * (DSCTO_DV / 100));
            DSCTO_EXON = DSCTO_EXON + DSCTO;
            //  console.log(TOT_EXON);
            //console.log('subtexon');
            //console.log(SUBT_EXON);
        }

        if (COL_TISC == true) {
            TOT_ISC = TOT_ISC + (TOT_AFECTO / (1 + COL_TISC / 100));
            TOT_ISC = TOT_ISC + (TOT_EXON / (1 + COL_TISC / 100));
        }
    }

    $("#ndsctoaf_orco").val(DSCTO_AFECTO.toFixed(conf_decimal));
    $("#ndctoex_orco").val(DSCTO_EXON.toFixed(conf_decimal));
    $("#nbrutoaf_orco").val(SUBT_AFECTO.toFixed(conf_decimal));
    $("#nbrutoex_orco").val(SUBT_EXON.toFixed(conf_decimal));
    $("#ntotaex_orco").val(SUBT_EXON.toFixed(conf_decimal));

    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    $("#nsubex_orco").val(SUBT_EX.toFixed(conf_decimal));
    $("#ntotaaf_orco").val(TOTAL_AFEC.toFixed(conf_decimal));
    $("#nsubaf_orco").val(SUBT_AFEC.toFixed(conf_decimal));

    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    $("#nigvaf_orco").val(IGV_AF.toFixed(conf_decimal));
    $("#nigvex_orco").val(0);

    var TOTAL = TOTAL_AFEC + SUBT_EX;

    $("#ntotal_orco").val(TOTAL.toFixed(2));
}
function ComparaPrecio(Precio, PrecioOrigen) {
    console.log('compra precio');
    console.log(Precio);
    console.log(PrecioOrigen);

    var xprecio = parseFloat(Precio);

    if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
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
                var m = $("#dfeorcoo_orco").val();
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
                $('#dfevenci_orco').val(xfecha);
                //console.log(fecha);
                console.log(xfecha);
            });

        },
        error: function (ex) {
            alert('No se puede obtener la fecha de pago .' + ex);
        }
    });
    return false;

}

function fnDocumentoSerieNumero() {
    //console.log($("#ncode_docu").val());

    $.ajax({
        type: 'POST',
        url: urlGetDocuNumero,
        dataType: 'json',
        data: { ndocu: $("#ncode_docu").val() },
        success: function (docu) {
            //console.log(docu.length);
            $.each(docu, function (i, doc) {
                $('#sseri_orco').val(doc.serie);
                $('#snume_orco').val(doc.numero);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}


