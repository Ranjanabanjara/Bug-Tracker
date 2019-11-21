$(function submit() {
    $("[data-role=submit]").click(function () {
        $(this).closest("form").submit();
    });
});