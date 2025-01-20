$(function () {

    $("#snomc_pers").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: 'POST',
                url: Url,
                dataType: 'json',
                data: { tipo: 0, trabajador: request.term, ncode_orar: 0 },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.trabajador,
                            value: item.trabajador,
                            id : item.scode_pers
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#scode_pers').val(ui.item.id);
        }
    });

});

