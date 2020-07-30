$(document).ready(function () {

    fnTransferencia()

    $("#ncode_timovi").change(function () {
        fnTransferencia();
    })

    var ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": [0,6]  //0,1,2,8,9
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
                console.log(resultado);
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

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, data.Precio, data.Precio, data.ncode_umed]).draw();
        //Totales();
    });

    $("#btncerrar").click(function () {
        mattable.destroy();
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

    var moviViewDetas = {
        "ncode_arti": "", "ncant_movidet": "", "npu_movidet": "", "ncode_movi": "", "ncode_umed": ""
    };

    var moviView = {
        "ncode_movi":"","dfemov_movi":"", "smone_movi":"", "ntc_movi":"", "sobse_movi":"", "ncode_timovi":"",
        "ncode_alma":"", "ndestino_alma":"", "stipo_movi":"", "moviViewDetas": []
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
        url: urlMoviCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(moviView) },
        success: function (result) {
            if (result.Success == "1") {
                window.location.href = urlMoviLista;
            }
            else {
                alert('No se puede registrar movimiento');
            }
        },
        error: function (ex) {
            alert('No se pueden registrar movimientos' + ex);
        }
    });

}