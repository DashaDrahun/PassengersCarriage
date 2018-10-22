function makeSelectRed() {
    $("select").addClass("colorforerror");
}

function makeOptionSelected(routeId) {
    $("option[value='" + routeId + "']").attr("selected", "selected");
}

function removeErrorStyle(element) {
    $(element).removeClass("colorforerror");
}