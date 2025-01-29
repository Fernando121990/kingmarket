var mattable;
var olotes;
var ofunciones;
var lotetable;
var ordentable;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0;
var stipo;


$(document).ready(function () {

    var code = 0;

    $('#btnpro').hide();

    code = $("#ncode_movi").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();

    console.log(conf_igv);
    console.log(conf_decimal);
    //console.log(conf_icbper);

    //if (typeof code === 'undefined') {
    //    //  console.log('series');
    //    fnDocumentoSerieNumero($("#ncode_docu").val());
    //}
    fnMovimiento()
    fnTransferencia()

    $("#ncode_timovi").change(function () {
        fnTransferencia();
    })

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
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0,6,7] 
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
                    //console.log('Precios');
                    //console.log(sValue);
                    //console.log(aPos);
                    //console.log(idx);
                    //console.log(ofunciones.cell(apos,idx+1).data());
                    switch (idx) {
                        case 3: //quantity column
                            //console.log('cantidad');
                            var almacen = $("#ncode_alma option:selected").val();
                            var codarticulo = ofunciones.cell(aPos, 0).data();
                            var cantorigen = ofunciones.cell(aPos, 8).data();
                            var cantvalida = ComparaCantidad(sValue, cantorigen)
                            //fnStockValida(codarticulo, cantvalida, almacen);
                            ofunciones.cell(aPos, idx).data(cantvalida).draw;
                            var cantfaltante = fnActualizaCtdadRestanteLote(codarticulo, cantvalida);
                            //var xvalue = ofunciones.cell(aPos, 5).data();
                            //var subto = cantvalida * parseFloat(xvalue);
                            //ofunciones.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            ofunciones.cell(aPos, 8).data(cantfaltante).draw;
                            break;
                        //case 7:
                        //    var xvalue = ofunciones.cell(aPos, 8).data();
                        //    console.log(xvalue);
                        //    var yValue = ComparaPrecio(sValue, xvalue);
                        //    console.log(yValue);
                        //    ofunciones.cell(aPos, idx).data(yValue).draw;
                        //    console.log('Precio insertado');
                        //    break;
                        default:
                            ofunciones.cell(aPos, idx).data(sValue).draw;

                    }

                    //Totales();
                },
                "submitdata": function (value, settings) {
                    return {
                        "column": ofunciones.column(this).index()
                    };

                },
                "height": "20px",
                "width": "100%"
            });
            //Totales();
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
                        "aTargets": [0, 7, 8, 9, 10, 11, 12]
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

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Precio, data.Precio, data.ncode_umed,xcan]).draw();
        //Totales();
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });


    $("#btnmovi").click(function () {

        if ($("#ncode_timovi option:selected").text().length < 1) {
            alert("Seleccione Movimiento");
            return false;
        };

        if ($("#ncode_alma option:selected").text().length < 1) {
            alert("Seleccione Almacen");
            return false;
        };

        var otbly = $('#tbl').dataTable();
        var nrowsy = otbly.fnGetData().length;

        if (nrowsy < 1) {
            alert("Seleccione Articulos");
            return false;
        }

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
        var xctdad = data[13];

        //console.log(xctdad);

        $("#xctdad").val(xctdad);
        $("#xctdadfalta").val(xctdad);
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

            ofunciones.cell({ row: idtx, column: 8 }).data(xcontrol).draw(false);

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
        var xcontrol = data[8];

        $("#xcodearti").val(xcod);
        $("#xcodelocal").val(xcodlocal);
        $("#xdescarti").val(xdes);
        $("#xund").val(xund);
        $("#xcontrol").val(xcontrol);
        $("#xctdadfalta").val(xcontrol);

    });

});

function fnTransferencia() {
    $('.destino').hide();
    $('#ndestino_alma').val('');
    var stipo = $("#ncode_timovi option:selected").text().substring(0, 1);

    if (stipo == 'T') {
        $('.destino').show();
    }
}

function Sales_save() {

    var moviViewLotes = {
        "ncode_arti": "", "ncant_movilote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
    };


    var moviViewDetas = {
        "ncode_arti": "", "ncant_movidet": "", "npu_movidet": "", "ncode_movi": "", "ncode_umed": ""
    };

    var moviView = {
        "ncode_movi":"","dfemov_movi":"", "smone_movi":"", "ntc_movi":"", "sobse_movi":"", "ncode_timovi":"",
        "ncode_alma": "", "ndestino_alma": "", "stipo_movi": "", "moviViewDetas": [], "moviViewLotes": []
    };

    moviView.ncode_movi = $('#ncode_movi').val();
    moviView.dfemov_movi = $("#dfemov_movi").val();
    moviView.smone_movi = $("#smone_movi").val();
    moviView.ntc_movi = $("#ntc_movi").val();
    moviView.sobse_movi = $("#sobse_movi").val();
    moviView.ncode_timovi = $("#ncode_timovi").val();
    moviView.ncode_alma = $("#ncode_alma").val();
    moviView.ndestino_alma = $("#ndestino_alma").val();
    moviView.stipo_movi = $("#ncode_timovi option:selected").text().substring(0, 1);

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        moviViewDetas.ncode_arti = oTable[i][0];
        moviViewDetas.ncant_movidet = oTable[i][3];
        moviViewDetas.npu_movidet = oTable[i][5];
        moviViewDetas.ncode_umed = oTable[i][7];

        moviView.moviViewDetas.push(moviViewDetas);

        moviViewDetas = {
            "ncode_arti": "", "ncant_movidet": "", "npu_movidet": "", "ncode_movi": "", "ncode_umed": ""
        };
    }

    var otbly = $('#tblLote').dataTable();
    var nrowsy = otbly.fnGetData().length;
    var oTabley = otbly.fnGetData();

    for (var i = 0; i < nrowsy; i++) {

        console.log(oTabley[i][3]);

        moviViewLotes.ncode_arti = oTabley[i][0];
        moviViewLotes.sdesc = oTabley[i][2];
        moviViewLotes.ncant_movilote = oTabley[i][3];
        moviViewLotes.sdesc_lote = oTabley[i][5];
        moviViewLotes.sfvenci_lote = oTabley[i][6]
        moviViewLotes.ncode_lote = oTabley[i][7]
        moviViewLotes.ncode_alma = $("#ncode_alma option:selected").val();

        moviView.moviViewLotes.push(moviViewLotes);

        moviViewLotes = {
            "ncode_arti": "", "ncant_movilote": "", "ncode_alma": "", "sdesc_lote": "", "sfvenci_lote": "", "ncode_lote": ""
        };


    }


    console.log(moviView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlMoviCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(moviView) },
        success: function (result) {
            console.log(result.Mensaje)
            switch (result.Success) {
                case 1:
                    window.location.href = urlMoviLista;
                    alert(result.Mensaje);
                    break;
                case 3:
                    alert(result.Mensaje);
                    $('#btnmovi').show();
                    $('#btnpro').hide();
                    break;
                default:
                    window.location.href = urlmoviLista;
                    alert(result.Mensaje);
            }
        },
        error: function (ex) {
            alert('No se pueden registrar movimientos' + ex);
        }
    });

}

function fnMovimiento() {
    $('.addLoteI').hide();
    $('.addLote').hide();

    stipo = $("#ncode_timovi option:selected").text().substring(0, 1);

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
        ctdadx = oTable[i][8];

        if (ctdadx == null || ctdadx == '') {
            ctdadx = 0;
        }

        console.log(ctdadx);
        console.log(ctdad);

        if (codigo == codArt) {
            ctdadx = parseFloat(ctdadx) + parseFloat(ctdad);
            console.log(ctdadx);
            ofunciones.cell({ row: i, column: 8 }).data(ctdadx).draw(false);
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
    console.log('calcula lote')
    console.log(sumx);
    console.log(cantfaltante);
    return cantfaltante

}

function ComparaCantidad(Cantidad, CantidadOrigen) {
    console.log('valida cantidad');

    var xcantidad = parseFloat(Cantidad);
    //Verificar la cantidad origen sino viene de pedido no hay control de cantidades
    if (stipo == "S") {

        if (CantidadOrigen > 0) {

            if (parseFloat(Cantidad) > parseFloat(CantidadOrigen)) {
                xcantidad = parseFloat(CantidadOrigen);
            }


        }

    }


    return xcantidad.toFixed(conf_decimal);

}