document
    .getElementById("sidebarToggle")
    .addEventListener("click", function () {
        var sb = document.getElementById("sidebarContent");
        var main = document.getElementById("mainContent");
        sb.classList.toggle("collapsed");
    });