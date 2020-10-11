if (!sra) {
    var sra = {};
}

function initializeSpeedRunEdit(isReadOnly) {
    initializeSpeedRunEditEvents(isReadOnly);
}

function initializeSpeedRunEditEvents(isReadOnly) {
    if (isReadOnly) {
        $('#divSpeedRunEdit :text').prop("readonly", true);
        $('#divSpeedRunEdit :radio, select').prop("disabled", true);
    }

    $('#divSpeedRunEdit').find('.select2').select2({ dropdownAutoWidth: true, width: "auto", dropdownParent: "#editModal" });
    $('#divSpeedRunEdit').find('.date').datepicker();
    //$modalBody.find('.time').timepicker();

    $('#divSpeedRunEdit').find('.game-search').autocomplete({
        minlength: 3,
        source: '../SpeedRun/SearchGames',
        select: function (event, ui) {
            var obj = JSON.parse(ui.item.value);

            $(this).data("value", obj.value);
            $(this).val(obj.label);
        },
        search: function (event, ui) {
            $(this).parent().addClass("loading-icon");
        },
        response: function (event, ui) {
            $(this).parent().removeClass("loading-icon");
        }
    });

    $('#divSpeedRunEdit').find('.user-search').select2({
        dropdownAutoWidth: true,
        width: "300px",
        dropdownParent: "#editModal",
        minimumInputLength: 3,
        ajax: {
            url: '../SpeedRun/SearchUsers',
            dataType: 'json',
            processResults: function (data, params) {
                var results = $(data).map(function () { return { id: this.value, text: this.label } }).get();
                return { results: results, more: false };
            }
        }
    });
}














