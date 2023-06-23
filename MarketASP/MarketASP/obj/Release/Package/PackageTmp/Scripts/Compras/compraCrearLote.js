var mattable;
var ordentable;
var conf_igv = 0;
var conf_decimal = 0;
var conf_icbper = 0
$(document).ready(function () {


    var ofunciones = $('#tbl').DataTable({
        "dom": 'T<"clear">lfrtip',
        "aoColumnDefs": [{
            "bVisible": false,
            "aTargets": []  //0,1,2,8,9
        },
        {
            "sClass": "my_class",
            "aTargets": [3,5,6]
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

        var codigo = $("#ncode_compra").val();
        console.log(codigo);

        $.ajax({
            type: 'POST',
            url: urlArticulos,
            dataType: 'json',
            data: { ncode_compra: codigo },
            success: function (resultado) {
                //console.log(resultado);
                //alert('exito');

                mattable = $('#matetabla').DataTable({
                    data: resultado, 
                    "columns":
                        [{"data": "Cod" },
                        {"data": "Cod2" },
                        {"data": "DescArt" },
                            {"data": "Ctdad" },
                            {"data": "Medida" },
                        {"data": "ncode_umed" },
                        {"data": "CtdadLoteControl" }
                        ],
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

        ofunciones.row.add([data.Cod, data.Cod2, data.DescArt, xcan, data.Medida, "", "", data.CtdadLoteControl]).draw();
        //Totales();
    });

    $("#btncerrar","btnmatcerrar").click(function () {
        mattable.destroy();
    });

    $("#btncompra").click(function () {

        if ($("#ncode_compra").val().length < 1) {
            alert("Seleccione Compra");
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

    console.log('asignar lotes');

    var loteViewDeta = {
        "ncode_arti": "", "ncant_lote": "", "ncode_compra": "", "dfvenci_lote": "",
        "sdesc_lote": "" 
    };

    var loteView = {
        "ncode_compra":"","loteViewDeta": []

    };

    loteView.ncode_compra = $('#ncode_compra').val();

    var otblx = $('#tbl').dataTable();
    var nrowsx = otblx.fnGetData().length;
    var oTable = otblx.fnGetData();

    for (var i = 0; i < nrowsx; i++) {

        loteViewDeta.ncode_arti = oTable[i][0];
        loteViewDeta.ncant_lote = oTable[i][3];
        loteViewDeta.sdesc_lote = oTable[i][5];
        loteViewDeta.dfvenci_lote = oTable[i][6];
        loteViewDeta.ncode_compra = loteView.ncode_compra;

        loteView.loteViewDeta.push(loteViewDeta);

        loteViewDeta = {
            "ncode_arti": "", "ncant_lote": "", "ncode_compra": "", "dfvenci_lote": "",
            "sdesc_lote": ""
        };

    }

    console.log(loteView);

    var token = $('[name=__RequestVerificationToken]').val();

    $.ajax({
        url: urlloteCrea, // '/MOFs/Create',
        type: 'POST',
        dataType: 'json',
        data: { '__RequestVerificationToken': token, 'model_json': JSON.stringify(loteView) },
        success: function (result) {
            if (result.Success == "1") {
                window.location.href = urlloteLista;
            }
            else {
                alert('No se puede registrar lotes');
            }
        },
        error: function (ex) {
            alert('No se puede registrar lotes' + ex);
        }
    });

}
function fnlimpiar() {

    $('#ncode_provee').val();
    //$('#sobse_venta').val();
    //$('#ncode_compra').val();
}