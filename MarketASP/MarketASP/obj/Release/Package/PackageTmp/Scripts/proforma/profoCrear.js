$(document).ready(function () {
    var code = 0;
    var conf_igv = 0;
    var conf_decimal = 0;

    code = $("#ncode_prof").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();

    console.log(conf_igv);
    console.log(conf_decimal);
    console.log(conf_icbper);

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
    }

    $("#ncode_docu").change(function () {
        fnDocumentoSerieNumero();
    });

    $("#ncode_fopago").change(function () {
        fnFormaPagoDiasFecha()
    });

    var ofunciones = $('#tbl').DataTable({
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
                            { "data": "Stock" },
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
                        "aTargets": [0, 6, 7, 8, 9,10]
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
        data.bafecto_arti, data.bisc_arti, data.bdscto_arti, xcan * data.Precio]).draw();
        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btnprof").click(function () {

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

        if ($("#ntc_prof").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_venta').val();

        //console.log(ncode_docu);
        //console.log(ntotal);

        if ($("#sseri_prof").val().length < 1) {
            alert("Ingrese serie del documento");
            return false;
        }

        if ($("#snume_prof").val().length < 1) {
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
});



function Sales_save() {

    //console.log('nueva venta');

    var proformaViewDetas = {
        "ncode_arti": "", "ncant_profdeta": "", "npu_profdeta": "", "ndscto_profdeta": "",
        "ndscto2_profdeta": "", "nexon_profdeta": "", "nafecto_profdeta": "", "besafecto_profdeta": "",
        "ncode_alma": "", "ndsctomax_profdeta": "", "ndsctomin_profdeta": "", "ndsctoporc_profdeta": ""
    };

    var proformaView = {
        "ncode_prof": "", "ncode_alma": "", "ncode_mone": "",
        "ncode_docu": "", "sseri_prof": "", "snume_prof": "",
        "sfeprofo_prof": "", "sfevenci_prof": "", "ncode_cliente": "",
        "ncode_clidire": "", "smone_prof": "", "ntc_prof": "",
        "ncode_fopago": "", "sobse_prof": "", "ncode_compra": "",
        "nbrutoex_prof": "", "nbrutoaf_prof": "",
        "ndctoex_prof": "", "ndsctoaf_prof": "", "nsubex_prof": "",
        "nsubaf_prof": "", "nigvex_prof": "", "nigvaf_prof": "", "ntotaex_prof": "",
        "ntotaaf_prof": "", "ntotal_prof": "", "ntotalMN_prof": "", "ntotalUs_prof": "",
        "nvalIGV_prof": "", "proformaViewDetas": []

    };

    proformaView.ncode_prof = $('#ncode_prof').val();
    proformaView.ncode_docu = $("#ncode_docu option:selected").val();
    proformaView.sseri_prof = $('#sseri_prof').val();
    proformaView.snume_prof = $('#snume_prof').val();
    proformaView.sfeprofo_prof = $('#dfeprofo_prof').val();
    proformaView.sfevenci_prof = $('#dfevenci_prof').val();
    proformaView.ncode_cliente = $('#COD_CLIENTE').val();
    proformaView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    proformaView.smone_prof = $('#smone_prof').val();
    proformaView.ntc_prof = $('#ntc_prof').val();
    proformaView.ncode_fopago = $("#ncode_fopago option:selected").val();
    proformaView.sobse_prof = $('#sobse_prof').val();
    proformaView.ncode_compra = $('#ncode_compra').val();
    proformaView.nbrutoex_prof = $('#nbrutoex_prof').val();
    proformaView.nbrutoaf_prof = $('#nbrutoaf_prof').val();
    proformaView.ndctoex_prof = $('#ndctoex_prof').val();
    proformaView.ndsctoaf_prof = $('#ndsctoaf_prof').val();
    proformaView.nsubex_prof = $('#nsubex_prof').val();
    proformaView.nsubaf_prof = $('#nsubaf_prof').val();
    proformaView.nigvex_prof = $('#nigvex_prof').val();
    proformaView.nigvaf_prof = $('#nigvaf_prof').val();
    proformaView.ntotaex_prof = $('#ntotaex_prof').val();
    proformaView.ntotaaf_prof = $('#ntotaaf_prof').val();
    proformaView.ntotal_prof = $('#ntotal_prof').val();
    proformaView.ntotalMN_prof = $('#ntotalMN_prof').val();
    proformaView.ntotalUs_prof = $('#ntotalUs_prof').val();
    proformaView.nvalIGV_prof = $('#nvalIGV_prof').val();
    proformaView.ncode_alma = $("#ncode_alma option:selected").val();
    proformaView.ncode_mone = $("#ncode_mone option:selected").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        proformaViewDetas.ncode_arti = oTable[i][0];
        proformaViewDetas.ncant_profdeta = oTable[i][3];
        proformaViewDetas.npu_profdeta = oTable[i][5];
        proformaViewDetas.ndscto_profdeta = oTable[i][6] - oTable[i][5];
        proformaViewDetas.nexon_profdeta = oTable[i][5] * oTable[i][3];
        proformaViewDetas.nafecto_profdeta = oTable[i][5] * oTable[i][3];
        proformaViewDetas.besafecto_profdeta = oTable[i][8];
        proformaViewDetas.ndsctoporc_profdeta = oTable[i][6];
        proformaViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        proformaView.proformaViewDetas.push(proformaViewDetas);

        var proformaViewDetas = {
            "ncode_arti": "", "ncant_profdeta": "", "npu_profdeta": "", "ndscto_profdeta": "",
            "ndscto2_profdeta": "", "nexon_profdeta": "", "nafecto_profdeta": "", "besafecto_profdeta": "",
            "ncode_alma": "", "ndsctomax_profdeta": "", "ndsctomin_profdeta": "", "ndsctoporc_profdeta": ""
        };

    }

    // console.log(proformaView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlprofCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(proformaView) },
        success: function (result) {
            console.log(result.Success)
            switch (result.Success) {
                case 1:
                    window.location.href = urlprofLista;
                    break;
                default:
                    window.location.href = urlprofLista;
            }
        },
        error: function (ex) {
            alert('No se puede registrar proforma' + ex);
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

    $("#ndsctoaf_prof").val(DSCTO_AFECTO.toFixed(conf_decimal));
    $("#ndctoex_prof").val(DSCTO_EXON.toFixed(conf_decimal));
    $("#nbrutoaf_prof").val(SUBT_AFECTO.toFixed(conf_decimal));
    $("#nbrutoex_prof").val(SUBT_EXON.toFixed(conf_decimal));
    $("#ntotaex_prof").val(SUBT_EXON.toFixed(conf_decimal));

    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    $("#nsubex_prof").val(SUBT_EX.toFixed(conf_decimal));
    $("#ntotaaf_prof").val(TOTAL_AFEC.toFixed(conf_decimal));
    $("#nsubaf_prof").val(SUBT_AFEC.toFixed(conf_decimal));

    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    $("#nigvaf_prof").val(IGV_AF.toFixed(conf_decimal));
    $("#nigvex_prof").val(0);

    var TOTAL = TOTAL_AFEC + SUBT_EX;

    $("#ntotal_prof").val(TOTAL.toFixed(2));
}
function ComparaPrecio(Precio, PrecioOrigen) {

    var xprecio = parseFloat(Precio);

    if (parseFloat(Precio) < parseFloat(PrecioOrigen)) {
        xprecio = parseFloat(PrecioOrigen);
    }

    return xprecio;

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

                var m = $("#dfeprofo_prof").val();
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
                var xfecha = new Date(fecha).toLocaleDateString()
                $('#dfevenci_prof').val(xfecha);
                //console.log(fecha);
                //console.log(xfecha);
            });

        },
        error: function (ex) {
            alert('No se pueden recuperar las areas.' + ex);
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
                $('#sseri_prof').val(doc.serie);
                $('#snume_prof').val(doc.numero);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}


