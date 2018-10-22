$(function () {
    window.$.datetimepicker.setLocale('en');

    window.$(".timepicker_input").datetimepicker({
        format: "H:i",
        timepicker: true,
        datepicker: false
    });

    window.$(".datepicker_input").datetimepicker({
        startDate: new Date(),
        format: "Y-F-d",
        timepicker: false
    });
})