var mattable;
var ordentable;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0
var oLotes;
var ofunciones;
var idtx;


$(document).ready(function () {

    $('#btnpro').hide();
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
                            //console.log('cantidad');
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                            var xcant = ofunciones.cell(aPos, 3).data();
                            //console.log(xcant);
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            //console.log(xvalue);
                            var subto = xcant * parseFloat(xvalue);
                            ofunciones.cell(aPos, 11).data(subto).draw;
                            break;
                        case 5: //price column
                            //console.log('subtotal');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var yValue = ComparaPrecio(sValue, xvalue);
                            var subto = xcant * yValue
                            ofunciones.cell(aPos, idx).data(yValue).draw;
                            ofunciones.cell(aPos, 11).data(subto).draw;
                            break;
                        default:
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                    }

                    Totales();
                },
                "submitdata": function (value, settings) {
                    return {
                        "column": ofunciones.column(this).index()
                    };

                },
                "height": "20px",
                "width": "100%"
            });
            Totales();
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
    oLotes = $('#tblLote').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0,7]  //0,1,2,8,9
        },
        {
            "sClass": "my_class",
            "aTargets": []
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlEditar, {
                "callback": function (sValue, y) {
                    var aPos = ofunciones.row(this).index();
                    var idx = ofunciones.column(this).index();
                    switch (idx) {
                        case 3: //quantity column
                            //console.log('cantidad');
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                            var xcant = ofunciones.cell(aPos, 3).data();
                            //console.log(xcant);
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            //console.log(xvalue);
                            var subto = xcant * parseFloat(xvalue);
                            ofunciones.cell(aPos, 11).data(subto).draw;
                            break;
                        case 5: //price column
                            //console.log('subtotal');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var yValue = ComparaPrecio(sValue, xvalue);
                            var subto = xcant * yValue
                            ofunciones.cell(aPos, idx).data(yValue).draw;
                            ofunciones.cell(aPos, 11).data(subto).draw;
                            break;
                        default:
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                    }

                },
                "submitdata": function (value, settings) {
                    return {
                        "column": ofunciones.column(this).index()
                    };

                },
                "height": "20px",
                "width": "100%"
            });
            Totales();
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
                    data: resultado, 
                    "columns":
                        [{ "data": "Cod" },
                        { "data": "Cod2" },
                        { "data": "DescArt" },
                        { "data": "Stock" },
                        { "data": "Medida" },
                        { "data": "Costo" },
                        { "data": "ncode_umed" },
                        { "data": "bafecto_arti" },
                        { "data": "bisc_arti" },
                        { "data": "bdscto_arti" }
                        ],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0, 6, 7, 8, 9]
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

    $('#matetabla tbody').on('click', 'tr', function () {
        var data = table.row(this).data();
        alert('You clicked on ' + data[0] + "'s row");
    });

    $("#btnmate").click(function () {
        var data = mattable.row('.selected').data();
        var xcan = 1;
        var xesta = 0;

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Costo, data.Costo, data.ncode_umed,
            data.bafecto_arti, data.bisc_arti, data.bdscto_arti, xcan * data.Costo,xcan]).draw();
        Totales();
    });

    $("#btncerrar","btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btncompra").click(function () {

        if ($("#sdesc_prove").val().length < 1) {
            alert("Seleccione Proveedor");
            return false;
        };

        var otbly = $('#tbl').dataTable();
        var nrowsy = otbly.fnGetData().length;

        if (nrowsy < 1) {
            alert("Seleccione Articulos");
            return false;
        }

        $('#btncompra').hide();
        $('#btnpro').show();

        Sales_save();
    });

    $(".btnpedidos").click(function () {

        $.ajax({
            type: 'POST',
            url: urlPedidoCompra,
            dataType: 'json',
            data: {},
            success: function (resultado) {

                ordentable = $('#pedidotabla').DataTable({
                    data: resultado, ///JSON.parse(data.d),
                    "columns":
                        [{ "data": "ncode_orco" },
                        { "data": "fecha" },
                        { "data": "numeracion" },
                        { "data": "sdesc_prove" }
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

        console.log(data.ncode_orco);

        fnCargaOrdenPedido(data.ncode_orco);

    });

    $("#btnpedidocerrarx", "#btnpedidocerrar").click(function () {
        ordentable.destroy();
    });

    /*manejo de lotes*/

    $("#btnLote").click(function () {

        var xcod = $('#xcodearti').val();
        var xcodlocal = $('#xcodelocal').val();
        var xdesc = $('#xdescarti').val();
        var xfvenci = $('#dfvenci_lote').val();
        var xlote = $('#sdesc_lote').val();
        var xcant = $('#ncant_lote').val();
        var xund = $("#xund").val();
        var xcontrol = $("#xctdadfalta").val();

        if (parseFloat(xcontrol) >= parseFloat(xcant) && parseFloat(xcontrol) > 0) {

            oLotes.row.add([xcod, xcodlocal, xdesc, xcant, xund, xlote, xfvenci, 0]).draw();

            xcontrol = xcontrol - xcant

            $("#xctdadfalta").val(xcontrol);

            ofunciones.cell({ row: idtx, column: 12 }).data(xcontrol).draw(false);

            console.log('cantidad de lote actualizada')

        }
        else {
            alert("La cantidad es mayor a la solicitada o se asignaron todos los lotes ");
        }

        
    });

    $(".addLote").click(function () {
        var data = ofunciones.row('.selected').data();
        idtx = ofunciones.row('.selected').index();

        console.log('agregar lote')
        console.log(data);
        console.log(idtx);

        var xcod = data[0];
        var xcodlocal = data[1];
        var xdes = data[2];
        var xund = data[4];
        var xcontrol = data[12];

        $("#xcodearti").val(xcod);
        $("#xcodelocal").val(xcodlocal);
        $("#xdescarti").val(xdes);
        $("#xund").val(xund);
        $("#xcontrol").val(xcontrol);
        $("#xctdadfalta").val(xcontrol);

    });

    $(".delLote").click(function () {

        var data = oLotes.row('.selected').data();
        oLotes.rows('.selected').remove().draw(false);

        fnActualizarCtdadLote(data[0], data[3]);

    });
});



function Sales_save() {

    console.log('nueva compra');

    var loteViewDeta = {
        "ncode_arti": "", "ncant_lote": "", "ncode_compra": "", "dfvenci_lote": "",
        "sdesc_lote": "", "sfechalote": ""
    };

    var compraViewDetas = {
        "ncode_arti": "", "ncant_comdeta": "", "npu_comdeta": "", "ndscto_comdeta": "",
        "ndscto2_comdeta": "", "nexon_comdeta": "", "nafecto_comdeta": "", "besafecto_comdeta": "",
        "ncode_alma": "" 
    };

    var compraView = {
        "ncode_compra": "", "ncode_alma": "",
        "ncode_docu": "", "sseri_compra": "", "snume_compra": "",
        "sfecompra_compra": "", "sfevenci_compra": "", "ncode_provee": "",
        "smone_compra": "", "ntc_compra": "", "sguia_compra": "","sproforma_compra":"",
        "ncode_fopago": "", "sobs_compra": "",
        "nbrutoex_compra": "", "nbrutoaf_compra": "",
        "ndsctoex_compra": "", "ndsctoaf_compra": "", "nsubex_compra": "",
        "nsubaf_compra": "", "nigvex_compra": "", "nigvaf_compra": "", "ntotaex_compra": "",
        "ntotaaf_compra": "", "ntotal_compra": "", "ntotalMN_compra": "", "ntotalUs_compra": "",
        "nvalIGV_compra": "", "ncode_orco": "", "sserie_orco": "", "stipo_orco": "", "compraViewDetas": [], "loteViewDeta": []

    };

    compraView.ncode_orco = $('#ncode_orco').val();
    compraView.ncode_compra = $('#ncode_compra').val();
    compraView.ncode_docu = $("#ncode_docu option:selected").val();
    compraView.sseri_compra = $('#sseri_compra').val();
    compraView.snume_compra = $('#snume_compra').val();
    compraView.sfecompra_compra = $('#dfecompra_compra').val();
    compraView.sfevenci_compra = $('#dfevenci_compra').val();
    compraView.ncode_provee = $('#ncode_provee').val();
    compraView.smone_compra = $('#smone_compra').val();
    compraView.ntc_compra = $('#ntc_compra').val();
    compraView.ncode_fopago = $("#ncode_fopago option:selected").val();
    compraView.sobs_compra = $('#sobs_compra').val();
    compraView.nbrutoex_compra = $('#nbrutoex_compra').val();
    compraView.nbrutoaf_compra = $('#nbrutoaf_venta').val();
    compraView.ndsctoex_compra = $('#ndsctoex_compra').val();
    compraView.ndsctoaf_compra = $('#ndsctoaf_compra').val();
    compraView.nsubex_compra = $('#nsubex_compra').val();
    compraView.nsubaf_compra = $('#nsubaf_compra').val();
    compraView.nigvex_compra = $('#nigvex_compra').val();
    compraView.nigvaf_compra = $('#nigvaf_compra').val();
    compraView.ntotaex_compra = $('#ntotaex_compra').val();
    compraView.ntotaaf_compra = $('#ntotaaf_compra').val();
    compraView.ntotal_compra = $('#ntotal_compra').val();
    compraView.ntotalMN_compra = $('#ntotalMN_compra').val();
    compraView.ntotalUs_compra = $('#ntotalUs_compra').val();
    compraView.nvalIGV_compra = $('#nvalIGV_compra').val();
    compraView.ncode_alma = $("#ncode_alma option:selected").val();
    compraView.sguia_compra = $('#sguia_compra').val();
    compraView.sproforma_compra = $('#sproforma_compra').val();
    compraView.sserie_orco = $('#sserie_orco').val();
    compraView.stipo_orco = $('#stipo_orco').val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        compraViewDetas.ncode_arti = oTable[i][0];
        compraViewDetas.ncant_comdeta = oTable[i][3];
        compraViewDetas.npu_comdeta = oTable[i][5];
        compraViewDetas.ndscto_comdeta = oTable[i][6] - oTable[i][5];
        compraViewDetas.nexon_comdeta = oTable[i][5] * oTable[i][3];
        compraViewDetas.nafecto_comdeta = oTable[i][5] * oTable[i][3];
        compraViewDetas.besafecto_comdeta = oTable[i][8];
        compraViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        compraView.compraViewDetas.push(compraViewDetas);

        compraViewDetas = {
            "ncode_arti": "", "ncant_comdeta": "", "npu_comdeta": "", "ndscto_comdeta": "",
            "ndscto2_comdeta": "", "nexon_comdeta": "", "nafecto_comdeta": "", "besafecto_comdeta": "",
            "ncode_alma": "" 
        };

    }

    var otblx = $('#tblLote').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        loteViewDeta.ncode_arti = oTable[i][0];
        loteViewDeta.ncant_lote = oTable[i][3];
        loteViewDeta.sdesc_lote = oTable[i][5];
        loteViewDeta.sfechalote = oTable[i][6];
        loteViewDeta.ncode_compra = compraView.ncode_compra;

        compraView.loteViewDeta.push(loteViewDeta);

        loteViewDeta = {
            "ncode_arti": "", "ncant_lote": "", "ncode_compra": "", "dfvenci_lote": "",
            "sdesc_lote": "", "sfechalote": ""
        };

    }
    console.log(compraView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlcompraCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(compraView) },
        success: function (result) {
            if (result.Success == "1") {
                window.location.href = urlcompraLista;
            }
            else {
                $('#btncompra').show();
                $('#btnpro').hide();
                alert('No se puede registrar compra');
            }
        },
        error: function (ex) {
            $('#btncompra').show();
            $('#btnpro').hide();
            alert('No se puede registrar compra' + ex);
        }
    });

}
function Totales() {
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
    var CONFIG_IGV = 18;

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

        if (AFECTO_ART == 'true' || AFECTO_ART == 'True') {
            TOT_AFECTO = TOT_AFECTO + Math.round(TOT, 2);
            //  console.log('afecto');
        }

        if (AFECTO_ART == 'false' || AFECTO_ART == 'False') {

            TOT_EXON = TOT_EXON + Math.round(TOT, 2);
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

    $("#ndsctoaf_compra").val(DSCTO_AFECTO.toFixed(2));
    $("#ndsctoex_compra").val(DSCTO_EXON.toFixed(2));
    $("#nbrutoaf_compra").val(SUBT_AFECTO.toFixed(2));
    $("#nbrutoex_compra").val(SUBT_EXON.toFixed(2));
    $("#ntotaex_compra").val(SUBT_EXON.toFixed(2));

    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    $("#nsubex_compra").val(SUBT_EX.toFixed(2));
    $("#ntotaaf_compra").val(TOTAL_AFEC.toFixed(2));
    $("#nsubaf_compra").val(SUBT_AFEC.toFixed(2));

    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    $("#nigvaf_compra").val(IGV_AF.toFixed(2));
    $("#nigvex_compra").val(0);

    var TOTAL = TOTAL_AFEC + SUBT_EX;

    $("#ntotal_compra").val(TOTAL.toFixed(2));
}
function ComparaPrecio(Precio, PrecioOrigen) {

    var xprecio = parseFloat(Precio);

    if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
        xprecio = parseFloat(PrecioOrigen);
    }

    return xprecio;

}

function fnCargaOrdenPedido(codigo) {

    fnlimpiar();
    console.log(codigo);
    $.ajax({
        type: 'POST',
        url: urlGetOrdenCompra,
        dataType: 'json',
        data: { ncode_orco: codigo },
        success: function (compra) {
            //console.log(compra);

            $('#sdesc_prove').val(compra.sproveedor);
            $('#ncode_provee').val(compra.ncode_provee);
            $('#stipo_orco').val(compra.stipo_orco);
            $('#sserie_orco').val(compra.sserie_orco);
            //$('#sruc_provee').val(compra.sruc);
            $('#sobse_compra').val(compra.ncode_cliente);
            $("#ncode_mone").val(compra.ncode_mone);
            $("#ncode_orco").val(codigo);
            //$("#ncode_fopago").val(compra.ncode_fopago);
            var num = parseInt(compra.compraViewDetas.length);
            var oprof = $('#tbl').DataTable();
            oprof.clear();
            for (var i = 0; i < num; i++) {

                oprof.row.add([compra.compraViewDetas[i].ncode_arti
                    , compra.compraViewDetas[i].scod2
                    , compra.compraViewDetas[i].sdesc
                    , compra.compraViewDetas[i].ncant_comdeta
                    , compra.compraViewDetas[i].sund
                    , compra.compraViewDetas[i].npu_comdeta
                    , compra.compraViewDetas[i].npu_comdeta
                    , 1
                    , compra.compraViewDetas[i].besafecto_comdeta
                    , compra.compraViewDetas[i].bisc_comdeta
                    , 0
                    , compra.compraViewDetas[i].npu_comdeta * compra.compraViewDetas[i].ncant_comdeta
                    , compra.compraViewDetas[i].ncant_comdeta]).draw();
            }
        },
        error: function (ex) {
            alert('No se puede recuperar datos de la orden de pedido' + ex);
        }
    });

    Totales();

    return false;
}

function fnlimpiar() {

    $('#ncode_provee').val();
    //$('#sobse_venta').val();
    //$('#ncode_compra').val();
}
function fnActualizarCtdadLote(codArt,ctdad) {
    console.log(codArt);

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    var codigo = '';
    var ctdadx = '';
    for (var i = 0; i < nrowsx; i++) {

        codigo = oTable[i][0];
        ctdadx = oTable[i][12];

        if (codigo == codArt) {
            ctdadx = parseFloat(ctdadx) + parseFloat(ctdad);
            ofunciones.cell({ row: i, column: 12 }).data(ctdadx).draw(false);
            break;
        }
    }
}