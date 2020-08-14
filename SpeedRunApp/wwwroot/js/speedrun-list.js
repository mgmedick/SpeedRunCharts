var throttleTimer = null;
var throttleDelay = 500;

function InitializeClient() {
    InitializeEvents();
}

function InitializeEvents() {
    $(window).off('scroll', OnWindowScroll).on('scroll', OnWindowScroll);
}

function GetSpeedRunList() {
    var speedRunCount = $('.speedRunSummary').length;

    $('#loading').show();
    $.get("SpeedRun/SpeedRunListMore",
        { "elementsOffset": speedRunCount },
        function (data) {
            if (data != null) {
                $('#divSpeedRunList').append(data);
            }
        },
        "html"
    ).fail(function () {
        alert("error");
    }).always(function() {
        $('#loading').hide();
  });
}

function OnWindowScroll() {
    clearTimeout(throttleTimer);
    throttleTimer = setTimeout(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
            GetSpeedRunList();
        }
    }, throttleDelay);
}







