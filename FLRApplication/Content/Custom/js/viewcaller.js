const overlay = document.querySelector(".overlay");

$(function () {
    $("a[data-modal]").on("click", function (e) {
        overlay.style.display = "block";
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal({ keyboard: true }, 'show');
        });
        return false;
    });
});

$('#myModal').on('hidden.bs.modal', function () {
    overlay.style.display = "none";
});