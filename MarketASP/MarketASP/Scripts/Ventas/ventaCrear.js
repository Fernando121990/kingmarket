$(document).ready(function () {


    var ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": []  //0,1,2,8,9
        },
        {
            "sClass": "my_class",
            "aTargets": [5,7]
        }],
        "drawCallback": function () {
            this.$('td.my_class').editable(urlEditar, {
                "callback": function (sValue, y) {
                    var aPos = ofunciones.row(this).index();
                    var idx = ofunciones.column(this).index();
                    switch (idx) {
                        case 7:
                            var xvalue = ofunciones.cell(aPos, 8).data();
                            console.log(xvalue);
                            var yValue = ComparaPrecio(sValue, xvalue);
                            console.log(yValue);
                            ofunciones.cell(aPos, idx).data(yValue).draw;
                            console.log('Precio insertado');
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

    $(".delMat").click(function () {

        ofunciones.rows('.selected').remove().draw(false);
    });

    var mattable;

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
                        { "data": "Medida" },
                        { "data": "Precio" },
                        { "data": "Stock" },
                        { "data": "ncode_umed" }],
                    "aoColumnDefs": [{
                        "bVisible": false,
                        "aTargets": [0]
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

        ofunciones.row.add([data.Cod,0,0, data.Cod2, data.DescArt, xcan, data.Medida, data.Precio, data.Precio, data.ncode_umed]).draw();
        Totales();
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });

});



function Sales_save() {

    var moviViewDetas = {
        "ncode_arti": "", "ncant_movidet": "", "npu_movidet": "", "ncode_movi": "", "ncode_umed": ""
    };

    var moviView = {
        "ncode_movi": "", "dfemov_movi": "", "smone_movi": "", "ntc_movi": "", "sobse_movi": "", "ncode_timovi": "",
        "ncode_alma": "", "ndestino_alma": "", "stipo_movi": "", "moviViewDetas": []
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

    //console.log(profView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlventaCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(moviView) },
        success: function (result) {
            if (result.Success == "1") {
                window.location.href = urlventaLista;
            }
            else {
                alert('No se puede registrar venta');
            }
        },
        error: function (ex) {
            alert('No se puede registrar venta' + ex);
        }
    });

}
function Totales() {
    //console.log('calculo de totales');

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

        var AFECTO_ART = oTable[i][1].toString();
        var COL_TISC = oTable[i][2];
        var CANT_DV = oTable[i][5];
        var PU_DV = oTable[i][7];
        var DSCTO_DV = 0;

        TOT = 0;
        TOT_AFECTO = 0;
        TOT_EXON = 0;
        TOT = CANT_DV * PU_DV;
        TOT = TOT - (TOT * DSCTO_DV / 100);

        //console.log(AFECTO_ART);

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

    $("#ndsctoaf_venta").val(DSCTO_AFECTO.toFixed(2));
    $("#ndctoex_venta").val(DSCTO_EXON.toFixed(2));
    $("#nbrutoaf_venta").val(SUBT_AFECTO.toFixed(2));
    $("#nbrutoex_venta").val(SUBT_EXON.toFixed(2));
    $("#ntotaex_venta").val(SUBT_EXON.toFixed(2));

    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    $("#nsubex_venta").val(SUBT_EX.toFixed(2));
    $("#ntotaaf_venta").val(TOTAL_AFEC.toFixed(2));
    $("#nsubaf_venta").val(SUBT_AFEC.toFixed(2));

    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    $("#nigvaf_venta").val(IGV_AF.toFixed(2));
    $("#nigvex_venta").val(0);

    var TOTAL = TOTAL_AFEC + SUBT_EX;

    $("#ntotal_venta").val(TOTAL.toFixed(2));
}
function ComparaPrecio(Precio, PrecioOrigen) {

    var xprecio = parseFloat(Precio);

    if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
        xprecio = parseFloat(PrecioOrigen);
    }

    return xprecio;

}