$(() => {
    $("#text").on('input', function () {
        CheckTexboxes()
    })

    $("#title").on('input', function () {
        CheckTexboxes()
    })
});

function CheckTexboxes() {
    const title = $("#title").val().trim();
    const text = $("#text").val().trim();

    const formValidity = String(title) && String(text);
    $("button").prop('disabled', !formValidity);
}