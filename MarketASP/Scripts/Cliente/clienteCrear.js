$(document).ready(function () {

    var valor = $('input:radio[name=stipo_cliente]:checked').val();

    //seleccion de tipo de cliente
    fnClienteNatural(valor);

    $("input[name$='stipo_cliente']").click(function () {
        var test = $(this).val();

        fnClienteNatural(test);

    });


    $("#btncliSave").click(function () {

        valor = $('input:radio[name=stipo_cliente]:checked').val();

        if (valor === 'J') {

            if ($("#srazon_cliente").val().length < 1) {
                alert("Ingrese Razon Social");
                return false;
            };

            if ($("#sruc_cliente").val().length < 1) {
                alert("Ingrese RUC");
                return false;
            };

        }
        else {

            if ($("#sdnice_cliente").val().length < 1) {
                alert("Ingrese DNI");
                return false;
            };

            if ($("#snomb_cliente").val().length < 1) {
                alert("Ingrese Nombres");
                return false;
            };

            if ($("#sappa_cliente").val().length < 1) {
                alert("Ingrese A.Paterno");
                return false;
            };

            if ($("#sapma_cliente").val().length < 1) {
                alert("Ingrese A.Materno");
                return false;
            };


        }

        if ($("#sdire_cliente").val().length < 1) {
            alert("Ingrese Direccion");
            return false;
        };

        if ($("#scode_ubigeo").val().length < 1) {
            alert("Ingrese Ubicacion");
            return false;
        };
    });
})


function fnClienteNatural(test) {
    //console.log(test);
    //$("#stipo_cliente").val(test)

    if (test === "N") {
        $(".natural").show();
        $(".juridica").hide();
    }
    else {

        $(".natural").hide();
        $(".juridica").show();
    }

}
