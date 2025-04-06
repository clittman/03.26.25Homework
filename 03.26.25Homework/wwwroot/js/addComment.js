$(() => {
    $("#text").on('input', function () {
        CheckTexboxes()
    })

    $("#name").on('input', function () {
        CheckTexboxes()
    })
});

function CheckTexboxes() {
    console.log("checking");
    const name = $("#name").val().trim();
    const text = $("#text").val().trim();

    const formValidity = String(name) && String(text);
    $("#submit").prop('disabled', !formValidity);
}