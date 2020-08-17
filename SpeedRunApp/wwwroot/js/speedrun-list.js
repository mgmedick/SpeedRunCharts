if (!sra) {
    var sra = {};
}

var throttleTimer = null;
var throttleDelay = 500;

function initializeClient(elementsPerPage) {
    initalizeConstants(elementsPerPage);
    initializeEvents();
}

function initalizeConstants(elementsPerPage) {
    sra['speedRunListElementsPerPage'] = elementsPerPage;
}

function initializeEvents() {
    $('.categoryGroup input').change(onCategoryChange);

    $(window).off('scroll', OnWindowScroll).on('scroll', OnWindowScroll);
}

function onCategoryChange() {
    $('#divSpeedRunList').empty();
    getSpeedRunList();
}

function getSpeedRunList() {
    var speedRunCount = $('.speedRunSummary').length;
    var categoryID = $('.categoryGroup input:checked').val();

    $('#loading').show();
    $.get("SpeedRun/SpeedRunSummaryList",
        { category: categoryID, elementsPerPage: sra.speedRunListElementsPerPage, elementsOffset: speedRunCount },
        function (data) {
            if (data != null) {
                $('#divSpeedRunList').append(data);
            }
        },
        "html"
    ).fail(function () {
        alert("error");
    }).always(function () {
        $('#loading').hide();
    });
}

function OnWindowScroll() {
    clearTimeout(throttleTimer);
    throttleTimer = setTimeout(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
            getSpeedRunList();
        }
    }, throttleDelay);
}








