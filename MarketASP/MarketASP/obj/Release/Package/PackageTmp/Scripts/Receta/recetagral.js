var mattable;
var ofunciones;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0;

$(document).ready(function () {

    var code = 0;

    $('#btnpro').hide();

    code = $("#ncode_movi").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();

    console.log(conf_igv);
    console.log(conf_decimal);


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
                            ofunciones.cell(aPos, idx).data(cantvalida).draw;
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

        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });


    $("#btnmovi").click(function () {

        if ($("#Rec_tipo option:selected").text().length < 1) {
            alert("Seleccione tipo de receta");
            return false;
        };

        if ($("#Rec_almacen option:selected").text().length < 1) {
            alert("Seleccione Almacen");
            return false;
        };

        if ($("#Rec_codProd option:selected").text().length < 1) {
            alert("Seleccione Articulo a producir");
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

});



function Sales_save() {


    var recetaViewDetas = {
        "RecD_CodProd": "", "RecD_Cantidad": "", "RecD_precio": "", "RecD_Almacen": ""
    };

    var recetaView = {
        "Rec_codigo": "", "Rec_descripcion": "", "Rec_codProd": "", "Rec_cantidad": "", "Rec_almacen": "", "Rec_tipo":"",
        "Rec_costoOperativo": "", "recetaViewDetas": []
    };

    recetaView.Rec_codigo = $('#Rec_codigo').val();
    recetaView.Rec_descripcion = $('#Rec_descripcion').val();
    recetaView.Rec_cantidad = $('#Rec_cantidad').val();
    recetaView.Rec_costoOperativo = $('#Rec_costoOperativo').val();
    recetaView.Rec_almacen = $("#Rec_almacen option:selected").val();
    recetaView.Rec_codProd = $("#Rec_codProd option:selected").val();
    recetaView.Rec_tipo = $("#Rec_tipo option:selected").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        recetaViewDetas.RecD_CodProd = oTable[i][0];
        recetaViewDetas.RecD_Cantidad = oTable[i][3];
        recetaViewDetas.RecD_precio = oTable[i][5];
        recetaViewDetas.RecD_Almacen = oTable[i][6];

        recetaView.recetaViewDetas.push(recetaViewDetas);

        recetaViewDetas = {
            "RecD_CodProd": "", "RecD_Cantidad": "", "RecD_precio": "", "RecD_Almacen": ""
        };
    }


    console.log(recetaView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlrecetaCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(recetaView) },
        success: function (result) {
            console.log(result.Mensaje)
            switch (result.Success) {
                case 1:
                    window.location.href = urlrecetaLista;
                    alert(result.Mensaje);
                    break;
                case 3:
                    alert(result.Mensaje);
                    $('#btnmovi').show();
                    $('#btnpro').hide();
                    break;
                default:
                    window.location.href = urlrecetaLista;
                    alert(result.Mensaje);
            }
        },
        error: function (ex) {
            alert('No se puede registrar la receta' + ex);
        }
    });

}



function ComparaCantidad(Cantidad, CantidadOrigen) {
    console.log('valida cantidad');

    var xcantidad = parseFloat(Cantidad);

    return xcantidad.toFixed(conf_decimal);

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

        //var AFECTO_ART = oTable[i][8].toString();
        //var COL_TISC = oTable[i][9];
        //var COL_ICBPER = oTable[i][11].toString();
        var CANT_DV = oTable[i][3];
        var PU_DV = oTable[i][5];
        //var DSCTO_DV = 0;

        TOT = 0;
        //TOT_AFECTO = 0;
        //TOT_EXON = 0;
        TOT = CANT_DV * PU_DV;
        TOT_AFECTO = TOT_AFECTO + Math.round(TOT, conf_decimal);
        //TOT = TOT - (TOT * DSCTO_DV / 100);

//        console.log(COL_ICBPER);

        //if (AFECTO_ART.toUpperCase() == 'TRUE' || AFECTO_ART == 'true' || AFECTO_ART == 'True') {
        //    TOT_AFECTO = TOT_AFECTO + Math.round(TOT, conf_decimal);
        //    //  console.log('afecto');
        //}

        //if (AFECTO_ART.toUpperCase() == 'FALSE' || AFECTO_ART == 'false' || AFECTO_ART == 'False') {

        //    TOT_EXON = TOT_EXON + Math.round(TOT, conf_decimal);
        //    //console.log('exone');
        //}

        //if (COL_ICBPER.toUpperCase() == 'TRUE') {
        //    TOT_ICBPER = TOT_ICBPER + conf_icbper * CANT_DV;
        //}

        //SUBT = CANT_DV * PU_DV;
        //console.log('totafecto');
        //console.log(TOT_AFECTO);
        //console.log('totexone');
        //console.log(TOT_EXON);

    //    if (TOT_AFECTO !== 0) {
    //        SUBT_AFECTO = SUBT_AFECTO + SUBT;
    //        DSCTO = (SUBT * (DSCTO_DV / 100));
    //        DSCTO_AFECTO = DSCTO_AFECTO + DSCTO;
    //        //      console.log('subafecto');
    //        //    console.log(SUBT_AFECTO);
    //    }

    //    if (TOT_EXON !== 0) {
    //        SUBT_EXON = SUBT_EXON + SUBT;
    //        DSCTO = (SUBT * (DSCTO_DV / 100));
    //        DSCTO_EXON = DSCTO_EXON + DSCTO;
    //        //  console.log(TOT_EXON);
    //        //console.log('subtexon');
    //        //console.log(SUBT_EXON);
    //    }

    //    if (COL_TISC == true) {
    //        TOT_ISC = TOT_ISC + (TOT_AFECTO / (1 + COL_TISC / 100));
    //        TOT_ISC = TOT_ISC + (TOT_EXON / (1 + COL_TISC / 100));
    //    }
    }

    //$("#ndsctoaf_guia").val(DSCTO_AFECTO.toFixed(conf_decimal));
    //$("#ndctoex_guia").val(DSCTO_EXON.toFixed(conf_decimal));
    //$("#nbrutoaf_guia").val(SUBT_AFECTO.toFixed(conf_decimal));
    //$("#nbrutoex_guia").val(SUBT_EXON.toFixed(conf_decimal));
    //$("#ntotaex_guia").val(SUBT_EXON.toFixed(conf_decimal));
    //$("#nicbper_guia").val(TOT_ICBPER.toFixed(conf_decimal));

    //var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    //var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    //var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    //$("#nsubex_guia").val(SUBT_EX.toFixed(conf_decimal));
    //$("#ntotaaf_guia").val(TOTAL_AFEC.toFixed(conf_decimal));
    //$("#nsubaf_guia").val(SUBT_AFEC.toFixed(conf_decimal));

    //var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    //$("#nigvaf_guia").val(IGV_AF.toFixed(conf_decimal));
    //$("#nigvex_guia").val(0);

    var TOTAL = TOT_AFECTO; //+ SUBT_EX;

    $("#Rec_costoOperativo").val(TOTAL.toFixed(conf_decimal));

    return false;
}