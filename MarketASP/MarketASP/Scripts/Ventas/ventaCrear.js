$(document).ready(function () {

    var code = 0;
    var conf_igv = 0;
    var conf_decimal = 0;

    code = $("#ncode_venta").val();
    conf_igv = $("#cnfigv").val();
    conf_decimal = $("#cnfdeci").val();
    conf_icbper = $("#cnficbper").val();

    console.log(conf_igv);
    console.log(conf_decimal);
    console.log(conf_icbper);

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
            "aTargets": [0,6,7,8,9,10] 
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
                            ofunciones.cell(aPos, idx).data(sValue).draw;
                            var xcant = ofunciones.cell(aPos, 3).data();
                            //console.log(xcant);
                            var xvalue = ofunciones.cell(aPos, 5).data();
                            //console.log(xvalue);
                            var subto = xcant * parseFloat(xvalue);
                            ofunciones.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            break;
                        case 5: //price column
                            //console.log('subtotal');
                            var xcant = ofunciones.cell(aPos, 3).data();
                            var xvalue = ofunciones.cell(aPos, 6).data();
                            var yValue = ComparaPrecio(sValue, xvalue);
                            var subto = xcant*yValue
                            ofunciones.cell(aPos, idx).data(yValue).draw;
                            ofunciones.cell(aPos, 12).data(subto.toFixed(conf_decimal)).draw;
                            break;
                        default:
                            ofunciones.cell(aPos, idx).data(sValue).draw;

                    }

                    Totales(conf_igv, conf_decimal, conf_icbper);;
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

    $("#sdesc_profo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: urlGetProforma, // "/Encuesta/GetCliente",
                type: "POST", dataType: "json",
                data: { snume: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.numeracion, value: item.numeracion, id: item.ncode_prof
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#ncode_profo').val(ui.item.id);
            fnCargaProforma();
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
                        "aTargets": [] //0,6,7,8,9
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
            data.bafecto_arti, data.bisc_arti, data.bdscto_arti,data.bicbper_arti,xcan*data.Precio]).draw();

        Totales(conf_igv, conf_decimal, conf_icbper);
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
    });

    $("#btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btnventa").click(function () {

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

        if ($("#ntc_venta").val().length < 1) {
            alert("Ingrese tipo de cambio");
            return false;
        }

        var ncode_docu = $("#ncode_docu option:selected").val();
        var ntotal = $('#ntotal_venta').val();


        if ($("#sseri_venta").val().length < 1) {
            alert("Ingrese serie del documento");
            return false;
        }

        if ($("#snume_venta").val().length < 1) {
            alert("Ingrese número del documento");
            return false;
        }

        if(ncode_docu == 10 && $("#sruc_cliente").val().length < 1){
            alert("Ingrese RUC");
            return false;
        }

        if (ncode_docu == 11 && $("#sdni_cliente").val().length < 1) {
            alert("Ingrese DNI");
            return false;
        }

        if (ncode_docu = 11 && $("#COD_CLIENTE").val() == 5 && ntotal > 700 ) {
            alert("La boleta supera los S/ 700, debe registrar al cliente.");
            return false;
        }

        //If Val(Me.TXT_TC.Text) = 0 Then vrpta = False : MsgBox("Ingrese Tipo de Cambio", MsgBoxStyle.Information) : Exit Function

        //If Combo_DOC.SelectedValue = "001" And DBLISTAR.Rows.Count - 1 > CONFIG_ITEM_FACTURA Then
        //vrpta = False
        //MessageBox.Show("Solo puede ingresar " & CONFIG_ITEM_FACTURA & " Items.", "GRABAR", MessageBoxButtons.OK, MessageBoxIcon.Information)
        //Exit Function
        //End If

        //If Combo_DOC.SelectedValue = "002" And DBLISTAR.Rows.Count - 1 > CONFIG_ITEM_BOLETA Then
        //vrpta = False
        //MessageBox.Show("Solo puede ingresar " & CONFIG_ITEM_BOLETA & " Items.", "GRABAR", MessageBoxButtons.OK, MessageBoxIcon.Information)
        //Exit Function
        //End If
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

    var ventaViewDetas = {
        "ncode_arti": "", "ncant_vedeta": "", "npu_vedeta": "", "ndscto_vedeta": "",
        "ndscto2_vedeta": "", "nexon_vedeta": "", "nafecto_vedeta": "", "besafecto_vedeta": "",
        "ncode_alma": "","ndsctomax_vedeta":"","ndsctomin_vedeta":"","ndsctoporc_vedeta":"","bicbper_vedeta" :""
    };

    var ventaView = {
        "ncode_venta": "", "ncode_alma": "","ncode_mone":"",
        "ncode_docu":"","sseri_venta":"", "snume_venta":"",
        "sfeventa_venta":"","sfevenci_venta":"","ncode_cliente":"",
        "ncode_clidire":"","smone_venta":"","ntc_venta":"",
        "ncode_fopago":"","sobse_venta":"","ncode_compra":"",
        "ncode_profo":"","nbrutoex_venta":"","nbrutoaf_venta":"",
        "ndctoex_venta":"","ndsctoaf_venta":"","nsubex_venta":"",
        "nsubaf_venta":"","nigvex_venta":"","nigvaf_venta":"","ntotaex_venta":"",
        "ntotaaf_venta":"","ntotal_venta":"","ntotalMN_venta":"","ntotalUs_venta":"",
        "nvalIGV_venta": "","nicbper_venta":"","ventaViewDetas": []

    };

    ventaView.ncode_venta = $('#ncode_venta').val();
    ventaView.ncode_docu = $("#ncode_docu option:selected").val();
    ventaView.sseri_venta = $('#sseri_venta').val();
    ventaView.snume_venta = $('#snume_venta').val();
    ventaView.sfeventa_venta = $('#dfeventa_venta').val();
    ventaView.sfevenci_venta = $('#dfevenci_venta').val();
    ventaView.ncode_cliente = $('#COD_CLIENTE').val(); 
    ventaView.ncode_clidire = $("#NRO_DCLIENTE option:selected").val();
    ventaView.smone_venta = $('#smone_venta').val();
    ventaView.ntc_venta = $('#ntc_venta').val();
    ventaView.ncode_fopago = $("#ncode_fopago option:selected").val();
    ventaView.sobse_venta = $('#sobse_venta').val();
    ventaView.ncode_compra = $('#ncode_compra').val();
    ventaView.ncode_profo = $('#ncode_profo').val();
    ventaView.nbrutoex_venta = $('#nbrutoex_venta').val();
    ventaView.nbrutoaf_venta = $('#nbrutoaf_venta').val();
    ventaView.ndctoex_venta = $('#ndctoex_venta').val();
    ventaView.ndsctoaf_venta = $('#ndsctoaf_venta').val();
    ventaView.nsubex_venta = $('#nsubex_venta').val();
    ventaView.nsubaf_venta = $('#nsubaf_venta').val();
    ventaView.nigvex_venta = $('#nigvex_venta').val();
    ventaView.nigvaf_venta = $('#nigvaf_venta').val();
    ventaView.ntotaex_venta = $('#ntotaex_venta').val();
    ventaView.ntotaaf_venta = $('#ntotaaf_venta').val();
    ventaView.ntotal_venta = $('#ntotal_venta').val();
    ventaView.ntotalMN_venta = $('#ntotalMN_venta').val();
    ventaView.ntotalUs_venta = $('#ntotalUs_venta').val();
    ventaView.nicbper_venta = $('#nicbper_venta').val();
    ventaView.nvalIGV_venta = $('#nvalIGV_venta').val();
    ventaView.ncode_alma = $("#ncode_alma option:selected").val();
    ventaView.ncode_mone = $("#ncode_mone option:selected").val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        ventaViewDetas.ncode_arti = oTable[i][0];
        ventaViewDetas.ncant_vedeta = oTable[i][3];
        ventaViewDetas.npu_vedeta = oTable[i][5];
        ventaViewDetas.ndscto_vedeta = oTable[i][6] - oTable[i][5];
        ventaViewDetas.nexon_vedeta = oTable[i][5] * oTable[i][3];
        ventaViewDetas.nafecto_vedeta = oTable[i][5] * oTable[i][3];
        ventaViewDetas.besafecto_vedeta = oTable[i][8];
        ventaViewDetas.ndsctoporc_vedeta = oTable[i][6];
        ventaViewDetas.bicbper_vedeta = oTable[i][11];
        ventaViewDetas.ncode_alma = $("#ncode_alma option:selected").val();

        ventaView.ventaViewDetas.push(ventaViewDetas);

        ventaViewDetas = {
            "ncode_arti": "", "ncant_vedeta": "", "npu_vedeta": "", "ndscto_vedeta": "",
            "ndscto2_vedeta": "", "nexon_vedeta": "", "nafecto_vedeta": "", "besafecto_vedeta": "",
            "ncode_alma": "", "ndsctomax_vedeta": "", "ndsctomin_vedeta": "", "ndsctoporc_vedeta": "",
            "bicbper_vedeta" : ""
        };

    }

   // console.log(ventaView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlventaCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(ventaView) },
        success: function (result) {
            console.log(result.Mensaje)
            switch (result.Success) {
                case 1:
                    window.location.href = urlventaLista;
                    alert(result.Mensaje);
                    break;
                case 2:
                    console.log(urlventaCobro);
                    urlventaCobro = urlventaCobro.replace("param-id", encodeURIComponent(result.CtaCo))
                        .replace("param-mensaje", encodeURIComponent(result.Mensaje));
                    console.log(urlventaCobro);
                    window.location.href = urlventaCobro;
                    break;
                default:
                    window.location.href = urlventaLista;
                    alert(result.Mensaje);
            }
        },
        error: function (ex) {
            alert('No se puede registrar venta' + ex);
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


    for (var i = 0; i < nrowsx; i++) {

        //console.log(i);
        //console.log(nrowsx);

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

    $("#ndsctoaf_venta").val(DSCTO_AFECTO.toFixed(conf_decimal));
    $("#ndctoex_venta").val(DSCTO_EXON.toFixed(conf_decimal));
    $("#nbrutoaf_venta").val(SUBT_AFECTO.toFixed(conf_decimal));
    $("#nbrutoex_venta").val(SUBT_EXON.toFixed(conf_decimal));
    $("#ntotaex_venta").val(SUBT_EXON.toFixed(conf_decimal));
    $("#nicbper_venta").val(TOT_ICBPER.toFixed(conf_decimal));

    var SUBT_EX = SUBT_EXON - DSCTO_EXON;
    var TOTAL_AFEC = SUBT_AFECTO - DSCTO_AFECTO;
    var SUBT_AFEC = TOTAL_AFEC / (1 + (CONFIG_IGV / 100));

    $("#nsubex_venta").val(SUBT_EX.toFixed(conf_decimal));
    $("#ntotaaf_venta").val(TOTAL_AFEC.toFixed(conf_decimal));
    $("#nsubaf_venta").val(SUBT_AFEC.toFixed(conf_decimal));

    var IGV_AF = TOTAL_AFEC - SUBT_AFEC;
    $("#nigvaf_venta").val(IGV_AF.toFixed(conf_decimal));
    $("#nigvex_venta").val(0);

    var TOTAL = TOTAL_AFEC + SUBT_EX;

    $("#ntotal_venta").val(TOTAL.toFixed(conf_decimal));
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

                var m = $("#dfeventa_venta").val();
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
                $('#dfevenci_venta').val(xfecha);
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
                $('#sseri_venta').val(doc.serie);
                $('#snume_venta').val(doc.numero);
                //console.log(doc.serie);
            });

        },
        error: function (ex) {
            alert('No se puede recuperar el número y serie' + ex);
        }
    });
    return false;
}
function fnCargaProforma() {

    //console.log($("#ncode_profo").val());
    fnlimpiar();

    $.ajax({
        type: 'POST',
        url: urlGetProfoVenta,
        dataType: 'json',
        data: { ncode_prof: $("#ncode_profo").val() },
        success: function (venta) {
            console.log(venta);

            $('#sdesc_cliente').val(venta.scliente);
            $('#COD_CLIENTE').val(venta.ncode_cliente);
            $('#sruc_cliente').val(venta.sruc);
            $('#sdni_cliente').val(venta.sdni);
            $('#sobse_venta').val(venta.ncode_cliente);
            $("#ncode_mone").val(venta.ncode_mone);

            var num = parseInt(venta.ventaViewDetas.length);
            var oprof = $('#tbl').DataTable();
            oprof.clear();
            for (var i = 0; i < num; i++) {
                oprof.row.add([venta.ventaViewDetas[i].ncode_arti,
                    venta.ventaViewDetas[i].scod2,
                    venta.ventaViewDetas[i].sdesc,
                    venta.ventaViewDetas[i].ncant_vedeta,
                    1,
                    venta.ventaViewDetas[i].npu_vedeta,
                    venta.ventaViewDetas[i].npu_vedeta,
                    1,
                    venta.ventaViewDetas[i].besafecto_vedeta,
                    venta.ventaViewDetas[i].bisc_vedeta,
                    venta.ventaViewDetas[i].npu_vedeta,
                    venta.ventaViewDetas[i].nafecto_vedeta]).draw();
            }

            fnclienteDire();
            $("#NRO_DCLIENTE").val(venta.ncode_clidire);
        },
        error: function (ex) {
            alert('No se puede recuperar datos de proforma' + ex);
        }
    });

    Totales();

    return false;

    
}
function fnlimpiar() {

    $('#COD_CLIENTE').val();
    $("#NRO_DCLIENTE").empty();
    $('#sobse_venta').val();
    $('#ncode_compra').val();
}

