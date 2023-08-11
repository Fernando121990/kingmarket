var mattable;
var lotetable;
var ordentable;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0

$(document).ready(function () {

    var code = 0;

    $('#btnpro').hide();

    code = $("#ncode_guia").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();

    console.log(conf_igv);
    console.log(conf_decimal);
    console.log(conf_icbper);

    if (typeof code === 'undefined') {
        //  console.log('series');
        fnDocumentoSerieNumero($("#ncode_docu").val());
    }

    if (code > 0) {
        fnclienteDire();
        //console.log($('#ncode_clidire').val());
        $("#NRO_DCLIENTE").val($('#ncode_clidire').val());

        fnclienteFPago();

        $("#nro_fopago").val($('#ncode_fopago').val());
    }

    fnTransferencia()

    $("#ncode_tiguia").change(function () {
        fnTransferencia();
    })

    $("#ncode_docu").change(function () {
        fnDocumentoSerieNumero(this.val());
    });

    $("#ncode_fopago").change(function () {
        fnFormaPagoDiasFecha()
    });

    var olotes = $('#tblLote').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0, 7]
        },
        {
            "sClass": "my_class",
            "aTargets": [3]
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlEditar, {
                "callback": function (sValue, y) {
                    var aPos = olotes.row(this).index();
                    var idx = olotes.column(this).index();
                    switch (idx) {
                        case 3: //quantity column
                            //    //console.log('cantidad');
                            //    var almacen = $("#ncode_alma option:selected").val();
                            //    var codarticulo = olotes.cell(aPos, 0).data();
                            //    var cantorigen = olotes.cell(aPos, 13).data();
                            //    var cantvalida = ComparaCantidad(sValue, cantorigen)
                            //    fnStockValida(codarticulo, cantvalida, almacen);
                            //    olotes.cell(aPos, idx).data(cantvalida).draw;
                            //    var xvalue = olotes.cell(aPos, 5).data();
                            //    var subto = cantvalida * parseFloat(xvalue);
                            //    olotes.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            olotes.cell(aPos, idx).data(sValue).draw;
                            break;
                        //case 5: //price column
                        //    //console.log('subtotal');
                        //    var xcant = olotes.cell(aPos, 3).data();
                        //    var xvalue = olotes.cell(aPos, 6).data();
                        //    var yValue = ComparaPrecio(sValue, xvalue);
                        //    var subto = xcant * yValue
                        //    olotes.cell(aPos, idx).data(yValue).draw;
                        //    olotes.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                        //    break;
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

    var ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0,5,6,7,8,9,10,11,12] //0, 6, 7, 8, 9, 10, 11]
        },
        {
            "sClass": "my_class",
            "aTargets": [3]
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
        var xcan = 1;
        var xesta = 0;

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Precio, data.Precio, data.ncode_umed,
        data.bafecto_arti, data.bisc_arti, data.bdscto_arti, data.bicbper_arti, xcan * data.Precio, 0]).draw();

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
        ////*******

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

        //if (ncode_docu = 11 && $("#COD_CLIENTE").val() == 5 && ntotal > 700) {
        //    alert("La boleta supera los S/ 700, debe registrar al cliente.");
        //    return false;
        //}

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

    /** */

    $(".delLote").click(function () {

        olotes.rows('.selected').remove().draw(false);
    });

    $(".addLote").click(function () {

        $.ajax({
            type: "Post",
            url: urlGetLoteDisponible,
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { ncode_alma: 1, ncode_arti: 0, fvenci_lote: '', sdesc_lote: '' },
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
        var data = lotetable.row('.selected').data();
        var xcan = 1;
        var xesta = 0;

        olotes.row.add([data.ncode_arti, data.scode_arti, data.sdesc1_arti, xcan, '-', data.sdesc_lote, data.fvenci_lote, data.ncode_lote]).draw();

    });

    $("#btncerrarL").click(function () {
        lotetable.destroy();
    });

    $("#btnlotecerrar").click(function () {
        lotetable.destroy();
    });



});



function Sales_save() {

    //console.log('nueva guia');

    var guiaViewLotes = {
        "ncode_arti": "", "ncant_guialote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
    };


    var guiaViewDetas = {
        "ncode_arti": "", "ncant_guiadet": "", "npu_guiadet": "","ncode_umed":"",
        //"ndscto2_guiadet": "", "nexon_guiadet": "", "nafecto_guiadet": "", "besafecto_guiadet": "",
        //"ncode_alma": "", "ndsctomax_guiadet": "", "ndsctomin_guiadet": "", "ndsctoporc_guiadet": "", "bicbper_guiadet": "",
        "sdesc": ""
    };

    var guiaView = {
        "ncode_guia": "", "sfemov_guia": "", "smone_guia": "", "ntc_guia": "", "sobse_guia": "",
        "sserie_guia": "", "snume_guia": "", "ncode_alma": "", "ncode_cliente": "", "ncode_docu": "",
        "ncode_clidire": "", "ncode_mone": "", "ncode_tiguia": "", "ndestino_alma": "", "stipo_guia": "", "guiaViewDetas": [],
        "guiaViewLotes": []

        //"sfeguia_guia": "", "sfevenci_guia": "", 
        //"nbrutoex_guia": "", "nbrutoaf_guia": "",
        //"ndctoex_guia": "", "ndsctoaf_guia": "", "nsubex_guia": "",
        //"nsubaf_guia": "", "nigvex_guia": "", "nigvaf_guia": "", "ntotaex_guia": "",
        //"ntotaaf_guia": "", "ntotal_guia": "", "ntotalMN_guia": "", "ntotalUs_guia": "",
        //"ncode_vende": "", "ncode_orpe": "",
        //"nvalIGV_guia": "", "nicbper_guia": "", 

    };

    guiaView.ncode_guia = $('#ncode_guia').val();
    guiaView.ncode_docu = $("#ncode_docu option:selected").val();
    guiaView.sserie_guia = $('#sserie_guia').val();
    guiaView.snume_guia = $('#snume_guia').val();
    guiaView.sfemov_guia = $('#dfemov_guia').val();
    guiaView.ncode_cliente = $('#COD_CLIENTE').val();
    guiaView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    guiaView.smone_guia = $('#smone_guia').val();
    guiaView.ntc_guia = $('#ntc_guia').val();
    guiaView.sobse_guia = $('#sobse_guia').val();
    guiaView.ncode_tiguia = $("#ncode_tiguia option:selected").val();
    guiaView.ncode_alma = $("#ncode_alma option:selected").val();
    guiaView.ndestino_alma = $("#ndestino_alma option:selected").val();
    guiaView.stipo_guia = $("#ncode_tiguia option:selected").text().substring(0, 1);
    guiaView.ncode_mone = $("#ncode_mone option:selected").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        guiaViewDetas.ncode_arti = oTable[i][0];
        guiaViewDetas.sdesc = oTable[i][2];
        guiaViewDetas.ncant_guiadet = oTable[i][3];
        guiaViewDetas.npu_guiadet = oTable[i][5];
        guiaViewDetas.ncode_umed = oTable[i][7];
        guiaView.guiaViewDetas.push(guiaViewDetas);

        var guiaViewDetas = {
            "ncode_arti": "", "ncant_guiadet": "", "npu_guiadet": "", "ncode_umed": "", "sdesc": ""
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
    var CONFIG_IGV = 0;
    var TOT_ICBPER = 0;

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    CONFIG_IGV = conf_igv;

    console.log(oTable);

    for (var i = 0; i < nrowsx; i++) {


        console.log(nrowsx);

        var AFECTO_ART = oTable[i][8].toString();
        var COL_TISC = oTable[i][9];
        var COL_ICBPER = oTable[i][11].toString();
        var CANT_DV = oTable[i][3];
        var PU_DV = oTable[i][5];
        var DSCTO_DV = 0;

        TOT = 0;
        TOT_AFECTO = 0;
        TOT_EXON = 0;
        TOT = CANT_DV * PU_DV;
        TOT = TOT - (TOT * DSCTO_DV / 100);

        console.log(COL_ICBPER);

        if (AFECTO_ART.toUpperCase() == 'TRUE' || AFECTO_ART == 'true' || AFECTO_ART == 'True') {
            TOT_AFECTO = TOT_AFECTO + Math.round(TOT, conf_decimal);
            //  console.log('afecto');
        }

        if (AFECTO_ART.toUpperCase() == 'FALSE' || AFECTO_ART == 'false' || AFECTO_ART == 'False') {

            TOT_EXON = TOT_EXON + Math.round(TOT, conf_decimal);
            //console.log('exone');
        }

        if (COL_ICBPER.toUpperCase() == 'TRUE') {
            TOT_ICBPER = TOT_ICBPER + conf_icbper * CANT_DV;
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

    $("#ndsctoaf_guia").val(DSCTO_AFECTO.toFixed(conf_decimal));
    $("#ndctoex_guia").val(DSCTO_EXON.toFixed(conf_decimal));
    $("#nbrutoaf_guia").val(SUBT_AFECTO.toFixed(conf_decimal));
    $("#nbrutoex_guia").val(SUBT_EXON.toFixed(conf_decimal));
    $("#ntotaex_guia").val(SUBT_EXON.toFixed(conf_decimal));
    $("#nicbper_guia").val(TOT_ICBPER.toFixed(conf_decimal));

    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    $("#nsubex_guia").val(SUBT_EX.toFixed(conf_decimal));
    $("#ntotaaf_guia").val(TOTAL_AFEC.toFixed(conf_decimal));
    $("#nsubaf_guia").val(SUBT_AFEC.toFixed(conf_decimal));

    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    $("#nigvaf_guia").val(IGV_AF.toFixed(conf_decimal));
    $("#nigvex_guia").val(0);

    var TOTAL = TOTAL_AFEC + SUBT_EX;

    $("#ntotal_guia").val(TOTAL.toFixed(conf_decimal));

    return false;
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
    //$.ajax({
    //    type: 'POST',
    //    url: urlGetDiasFormaPago,
    //    dataType: 'json',
    //    data: { ncode_fopago: $("#ncode_fopago").val() },
    //    success: function (fopago) {
    //        console.log(fopago);
    //        $.each(fopago, function (i, dias) {

    //            var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
    //            var m = $("#dfeguia_venta").val();
    //            var parts = m.split("/");
    //            var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
    //            //var fecha = new Date($('#dfeventa_venta').val());
    //            //console.log(fecha);
    //            //console.log(fecha.getDate());
    //            //console.log(fecha.getFullYear());
    //            //console.log(fecha.getMonth());
    //            //console.log(dias.dias);
    //            //var xdias = parseInt(fecha.getDate()) + parseInt(dias.dias); // Número de días a agregar
    //            //console.log(xdias);
    //            fecha.setDate(fecha.getDate() + parseInt(dias.dias));
    //            var xfecha = new Date(fecha).toLocaleDateString("es-PE", options);
    //            $('#dfevenci_venta').val(xfecha);
    //            //console.log(fecha);
    //            console.log(xfecha);
    //        });

    //    },
    //    error: function (ex) {
    //        alert('No se pueden recuperar las areas.' + ex);
    //    }
    //});
    return false;

}
function fnDocumentoSerieNumero(docu) {
    //console.log($("#ncode_docu").val());

    $.ajax({
        type: 'POST',
        url: urlGetDocuNumero,
        dataType: 'json',
        data: { ndocu: docu },
        success: function (docu) {
            //console.log(docu.length);
            $.each(docu, function (i, doc) {
                $('#sserie_guia').val(doc.serie);
                $('#snume_guia').val(doc.numero);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
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
    //console.log(almacen);
    $.ajax({
        type: 'POST',
        url: urlStockValida,
        dataType: 'json',
        data: { ncode_arti: codigo, ncode_alma: almacen },
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

function fnTransferencia() {
    $('.destino').hide();
    $('#ndestino_alma').val('');
    var stipo = $("#ncode_tiguia option:selected").text().substring(0, 1);

    if (stipo == 'T') {
        $('.destino').show();
    }
}