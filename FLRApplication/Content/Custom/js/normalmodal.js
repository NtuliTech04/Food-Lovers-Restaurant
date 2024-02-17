const modal = document.querySelector(".modal");
const overlay = document.querySelector(".overlay");
const openModalBtn = document.querySelector(".btn-open");
const closeModalBtn = document.querySelector(".btn-close");

// close modal function
const closeModal = function () {
    modal.style.display = "none";
    overlay.style.display = "none";
};

// close the modal when the close button and overlay is clicked
closeModalBtn.addEventListener("click", closeModal);
overlay.addEventListener("click", closeModal);

// close modal when the Esc key is pressed
document.addEventListener("keydown", function (e) {
    if (e.key === "Escape" && !modal.style.display == "none") {
        closeModal();
    }
});

// open modal function
const openModal = function () {
    modal.style.display = "block";
    overlay.style.display = "block";
};

// open modal event
openModalBtn.addEventListener("click", openModal);

