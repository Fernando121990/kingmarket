var ofunciones=null;
var mattable;
var preciotable;
var almatable;
var conf_decimal = 2;
var conf_moneda = 0;
var CONFIG_dscto = 'NO';
var conf_PrecioIGV;

$(document).ready(function () {
    var code = 0;
    var conf_igv = 0;

    $('#btnpro').hide();
    code = $("#ncode_orco").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();
    conf_PrecioIGV = $("input[type=checkbox][name=bprecioconigv]:checked").val();

    //console.log(conf_igv);
    //console.log(conf_decimal);
    //console.log(conf_icbper);

    //console.log(code);
    //console.log('xx')

    if (typeof code === 'undefined') {
        //  console.log('series');
        fnDocumentoSerieNumero();
    }

  //  if (code > 0) {
//        $("#nro_fopago").val($('#ncode_fopago').val());
//    }

    $("#ncode_docu").change(function () {
        fnDocumentoSerieNumero();
    });

    $("#ncode_fopago").change(function () {
        fnFormaPagoDiasFecha($('#ncode_fopago').val());
    });

     ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": []///[0, 6, 7, 8, 9, 10]
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
                            { "data": "Costo" },
                            { "data": "ncode_umed" },
                            { "data": "bafecto_arti" },
                            { "data": "bisc_arti" },
                            { "data": "bdscto_arti" },
                            { "data": "bicbper_arti" }
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets":[0, 8, 9, 10,11,12]
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

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Costo, data.Costo, data.ncode_umed,
            data.bafecto_arti, data.bisc_arti, data.bdscto_arti, xcan * data.Costo]).draw();
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

        if ($("#ncode_provee").val().length < 1) {
            alert("Seleccione Proveedor");
            return false;
        };


        if ($("#ntc_orco").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_orco').val();

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


        /////******
        var otbly = $('#tbl').dataTable();
        var nrowsy = otbly.fnGetData().length;
        //console.log(nrowsy);
        if (nrowsy < 1) {
            alert("Seleccione Articulos");
            return false;
        }

        $('#btnorco').hide();
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
            url: urlOCompraPrecio,
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
                            { "data": "sdesc_prove" },
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
                            { "data": "TRANSITO"}
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

    var ordencompraViewDetas = {
        "ncode_arti": "", "ncant_orcodeta": "", "npu_orcodeta": "", "ndscto_orcodeta": "",
        "ndscto2_orcodeta": "", "nexon_orcodeta": "", "nafecto_orcodeta": "", "besafecto_orcodeta": "",
        "ncode_alma": "", "ndsctomax_orcodeta": "", "ndsctomin_orcodeta": "", "ndsctoporc_orcodeta": "",
        "npuorigen_orcodeta": ""
    };

    var ordenpedidoView = {
        "ncode_orco": "", "ncode_alma": "", "ncode_mone": "",
        "ncode_docu": "", "sseri_orco": "", "snume_orco": "",
        "sfeordencompra_orco": "", "sfevenci_orco": "", "sfentrega_orco": "", "ncode_cliente": "",
        "ncode_clidire": "", "smone_orco": "", "ntc_orco": "",
        "ncode_fopago": "", "sobse_orco": "", "scode_compra": "",
        "nbrutoex_orco": "", "nbrutoaf_orco": "",
        "ndctoex_orco": "", "ndsctoaf_orco": "", "nsubex_orco": "",
        "nsubaf_orco": "", "nigvex_orco": "", "nigvaf_orco": "", "ntotaex_orco": "",
        "ntotaaf_orco": "", "ntotal_orco": "", "ntotalMN_orco": "", "ntotalUs_orco": "",
        "nvalIGV_orco": "", "ncode_provee": "", "stipo_orco": "", "ntipo_orco": "", "ordencompraViewDetas": []

    };

    ordenpedidoView.ncode_orco = $('#ncode_orco').val();
    ordenpedidoView.ncode_docu = $("#ncode_docu option:selected").val();
    ordenpedidoView.sseri_orco = $('#sseri_orco').val();
    ordenpedidoView.snume_orco = $('#snume_orco').val();
    ordenpedidoView.sfeordencompra_orco = $('#dfeorco_orco').val();
    ordenpedidoView.sfevenci_orco = $('#dfevenci_orco').val();
    ordenpedidoView.sfentrega_orco = $('#dfentrega_orco').val();
    ordenpedidoView.ncode_provee = $('#ncode_provee').val();
    ordenpedidoView.smone_orco = $("#smone_orco option:selected").val(); 
    ordenpedidoView.ntc_orco = $('#ntc_orco').val();
    ordenpedidoView.ncode_fopago = $("#ncode_fopago option:selected").val();
    ordenpedidoView.sobse_orco = $('#sobse_orco').val();
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

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        ordencompraViewDetas.ncode_arti = oTable[i][0];
        ordencompraViewDetas.ncant_orcodeta = oTable[i][3];
        ordencompraViewDetas.npu_orcodeta = oTable[i][5];
        ordencompraViewDetas.npuorigen_orcodeta = oTable[i][6];
        ordencompraViewDetas.ndscto_orcodeta = oTable[i][6] - oTable[i][5];
        ordencompraViewDetas.nexon_orcodeta = oTable[i][5] * oTable[i][3];
        ordencompraViewDetas.nafecto_orcodeta = oTable[i][5] * oTable[i][3];
        ordencompraViewDetas.besafecto_orcodeta = oTable[i][8];
        ordencompraViewDetas.ndsctoporc_orcodeta = oTable[i][7];
        ordencompraViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        ordenpedidoView.ordencompraViewDetas.push(ordencompraViewDetas);

        var ordencompraViewDetas = {
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
            $('#btnorco').show();
            $('#btnpro').hide();
            alert('No se puede registrar orden de compra' + ex);
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
    var monedaorco = $('#smone_orco').val();
    var TCambio = parseFloat($('#ntc_orco').val());

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

        //validar orco en soles o dolares
        if (monedaorco == "D") {
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


    $("#ndsctoaf_orco").val(DSCTO_AFECTO);
    $("#ndctoex_orco").val(DSCTO_EXON);
    $("#nbrutoaf_orco").val(SUBT_AFECTO);
    $("#nbrutoex_orco").val(SUBT_EXON);

    $("#nsubaf_orco").val(SUBT_AFECTO);
    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    $("#nsubex_orco").val(SUBT_EX);
    $("#ntotaex_orco").val(SUBT_EXON);

    var TOTAL_AFEC = 0, SUBT_AFEC = 0, IGV_AF = 0;


    if (conf_PrecioIGV == "on") {
        TOTAL_AFEC = parseFloat(SUBT_AFECTO);
        SUBT_AFEC = parseFloat(TOTAL_AFEC) / (1 + (parseInt(CONFIG_IGV) / 100));
        IGV_AF = parseFloat(TOTAL_AFEC) - parseFloat(SUBT_AFEC);

        $("#ntotaaf_orco").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nsubaf_orco").val(SUBT_AFEC.toFixed(conf_decimal));
        $("#nigvaf_orco").val(IGV_AF.toFixed(conf_decimal));

    }
    else {
        IGV_AF = (parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV) / parseInt(100)));
        TOTAL_AFEC = (parseFloat(SUBT_AFECTO) + parseFloat(SUBT_AFECTO) * (parseFloat(CONFIG_IGV) / parseInt(100)));

        $("#ntotaaf_orco").val(TOTAL_AFEC.toFixed(conf_decimal));
        $("#nigvaf_orco").val(IGV_AF.toFixed(conf_decimal));

    }

    var TOTAL = parseFloat(TOTAL_AFEC) + parseFloat(SUBT_EX);

    $("#ntotal_orco").val(TOTAL.toFixed(conf_decimal));

}
function ComparaPrecio(Precio, PrecioOrigen) {
    //console.log('compra precio');
    //console.log(Precio);
    //console.log(PrecioOrigen);

    var xprecio = parseFloat(Precio);

   // if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
   //     xprecio = parseFloat(PrecioOrigen);
   // }

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
                var m = $("#dfeorco_orco").val();
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


