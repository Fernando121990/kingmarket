$(document).ready(function () {

    var cambioUS = 0;
    var cambioMN = 0;
    var saldoMN = 0; vueltoMN = 0; saldoUS = 0;

    var xtc = $('#ntc_ctacode').val();
    var valor = $('input:radio[name=smone_ctacode]:checked').val();
    saldoMN = $('#nsaldoMN').val();
    saldoUS = $('#nsaldoUS').val();

    //seleccion de moneda de pago
    fnSaldoLimpia(valor);

    $("input[name$='smone_ctacode']").click(function () {
        fnSaldoLimpia(this.value);
    });

    $("#npagoMN").on("input", function () {

        cambioUS = this.value / xtc
        $('#ncambioUS').val(cambioUS);
        $('#nmonto_ctacode').val(saldoMN);

        //calculando vuelto en moneda nacional
        vueltoMN = this.value - saldoMN;

        if (vueltoMN <= 0) {
            vueltoMN = 0;
            $('#nmonto_ctacode').val(this.value);
        };

        $('#nvuelto_ctacode').val(vueltoMN);
    })

    $("#npagoUS").on("input", function () {

        cambioMN = this.value * xtc

        $('#ncambioMN').val(cambioMN);

        $('#nmonto_ctacode').val(saldoUS);

        //calculando vuelto en moneda nacional
        vueltoMN = cambioMN - saldoMN;
        if (vueltoMN <= 0) {
            vueltoMN = 0;
            $('#nmonto_ctacode').val(this.value);
        };

        $('#nvuelto_ctacode').val(vueltoMN);

    })

    $("#npagoMN").keypress(function (e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == 13) {
            $('#sobse_ctacode').focus();
            return false;
        }
    })

    $("#npagoUS").keypress(function (e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == 13) {
            $('#sobse_ctacode').focus();
            return false;
        }
    })

    $("#btnPago").click(function () {

        if ($("#nmonto_ctacode").val().length < 1) {
            alert("Ingrese Pago");
            return false;
        };

    });

})

function fnSaldoLimpia(moneda) {

    //console.log('limpiando');

    $("#npagoMN").val('');
    $("#npagoUS").val('');
    $('#ncambioMN').val('');
    $('#ncambioUS').val('');
    $('#nmonto_ctacode').val('');
    $('#nvuelto_ctacode').val('');

    if (moneda === "MN") {
        $(".MN").removeAttr("disabled", "disable");
        $(".US").attr("disabled", "disable");
        $('#smone').text('PAGO S/ :');
        $("#npagoMN").focus();
    }
    else {
        $(".MN").attr("disabled", "disable");
        $(".US").removeAttr("disabled", "disable");
        $('#smone').text('PAGO US$ :');
        $("#npagoUS").focus();
    }

}