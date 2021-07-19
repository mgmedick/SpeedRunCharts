////if (!sra) {
////    var sra = {};
////}

//var throttleTimer = null;
//var throttleDelay = 500;

function initializeClient(elementsPerPage) {
    initalizeConstants(elementsPerPage);
    initializeEvents();
    getSpeedRunList();
}

function initalizeConstants(elementsPerPage) {
    sra['elementsPerPage'] = elementsPerPage;
}

function initializeEvents() {
    //$('.categoryGroup input').change(onCategoryChange);
    $('[data-toggle="tooltip"]').tooltip();

//    $(window).off('scroll', OnWindowScroll).on('scroll', OnWindowScroll);
}

/*
function onCategoryChange() {
    $('#divSpeedRunList').empty();
    getSpeedRunList();

    var categoryID = $('.categoryGroup input:checked').val();
    $('.speedrunlist').attr("categoryid", categoryID);
}

function OnWindowScroll() {
    clearTimeout(throttleTimer);
    throttleTimer = setTimeout(function () {
        if ($(window).scrollTop() + $(window).height() > $('body').height() - 200) {
            getSpeedRunList();
        }
    }, throttleDelay);
}

function getSpeedRunList() {
    var categoryID = $('.categoryGroup input:checked').val();
    var orderValues = $('.orderValue').map(function () { return parseInt($(this).val()) });
    var offset = orderValues.length > 0 ? Math.min.apply(null, orderValues) : null;
    $('.speedrunlist').attr("categoryid", categoryID);
    $('.speedrunlist').attr("topamt", sra.elementsPerPage);
    $('.speedrunlist').attr("offset", offset);

    $('#loading').show();
    $.get('../templates/SpeedRunSummary.html?_t=' + (new Date()).getTime(), function (summaryTemplate, status) {
        $.get('../SpeedRun/GetLatestSpeedRuns', { category: categoryID, topAmount: sra.elementsPerPage, orderValueOffset: offset },
            function (data, status) {
                var requests = [];
                $(data).each(function () {
                    requests.push(renderTemplate(null, summaryTemplate, this));
                });
                $.when.apply(null, requests).then(function () {
                    var html = "";
                    $(arguments).each(function () {
                        html += this;
                    })
                    $('#divSpeedRunList').append(html);
                    $('#loading').hide();
                });
            });
    });
}

function showSpeedRunDetails(speedRunID, gameID) {
    var $modal = $('#detailsModal');
    var $modalTitle = $modal.find('.modal-title');
    var $modalBody = $modal.find('.modal-body');
    var $modalLoading = $modal.find('.modal-loading');

    $modalBody.hide();
    $modalLoading.show();
    $modal.modal('show');

    $.get('../templates/SpeedRunEdit.html?_t=' + (new Date()).getTime(), function (detailsTemplate, status) {
        $.get('SpeedRun/GetEditSpeedRun?gameID=' + gameID + '&speedRunID=' + speedRunID + '&isReadOnly=true', function (data, status) {
            renderTemplate($modalBody, detailsTemplate, data).then(function () {
                initializeSpeedRunEdit(data.isReadOnly);
                $modalBody.show();
                $modalLoading.hide();
            });
        });
    });
}
*/















