if (!sra) {
    var sra = {};
}

var throttleTimer = null;
var throttleDelay = 500;

function initializeClient(statusTypes, categoryTypes, platforms, elementsPerPage) {
    initalizeConstants(statusTypes, categoryTypes, platforms, elementsPerPage);
    initializeEvents();
    getSpeedRunList();
}

function initalizeConstants(statusTypes, categoryTypes, platforms, elementsPerPage) {
    sra['statusTypes'] = statusTypes;
    sra['categoryTypes'] = categoryTypes;
    sra['platforms'] = platforms;
    sra['elementsPerPage'] = elementsPerPage;
}

function initializeEvents() {
    $('.categoryGroup input').change(onCategoryChange);

    $(window).off('scroll', OnWindowScroll).on('scroll', OnWindowScroll);
}

function onCategoryChange() {
    $('#divSpeedRunList').empty();
    getSpeedRunList();
}

function OnWindowScroll() {
    clearTimeout(throttleTimer);
    throttleTimer = setTimeout(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
            getSpeedRunList();
        }
    }, throttleDelay);
}

function getSpeedRunList() {
    var speedRunCount = $('.speedRunSummary').length;
    var categoryID = $('.categoryGroup input:checked').val();

    $('#loading').show();
    $.get('../templates/SpeedRunSummary.html?_t=' + (new Date()).getTime(), function (summaryTemplate, status) {
        $.get('../SpeedRun/GetLatestSpeedRuns', { category: categoryID, elementsPerPage: sra.elementsPerPage, elementsOffset: speedRunCount },
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
    var $modal = $('#editModal');
    var $modalTitle = $('#editModal').find('.modal-title');
    var $modalBody = $('#editModal').find('.modal-body');
    var $modalLoading = $('#editModal').find('.modal-loading');
    $modalTitle.text("Details");

    $modalBody.hide();
    $modalLoading.show();
    $modal.modal('show');
    $.get('../templates/SpeedRunEdit.html?_t=' + (new Date()).getTime(), function (detailsTemplate, status) {
        $.get('SpeedRun/GetEditSpeedRun?speedRunID=' + speedRunID + '&gameID=' + gameID + '&isReadOnly=true', function (data, status) {
            renderTemplate($modalBody, detailsTemplate, data).then(function () {
                initializeSpeedRunEdit(data.isReadOnly);
                $modalBody.show();
                $modalLoading.hide();
            });
        });
    });
}












