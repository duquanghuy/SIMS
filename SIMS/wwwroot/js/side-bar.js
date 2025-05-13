(function () {
    const sb = document.getElementById("sidebarContent");
    const toggle = document.getElementById("sidebarToggle");
    const STORAGE_KEY = "sidebarCollapsed";

    // Apply collapsed or expanded
    function applyState(collapsed) {
        sb.classList.toggle("collapsed", collapsed);
    }

    // Read state from localStorage (default to false)
    function getState() {
        return localStorage.getItem(STORAGE_KEY) === "true";
    }

    // Write state to localStorage
    function setState(collapsed) {
        localStorage.setItem(STORAGE_KEY, collapsed);
    }

    // Wire up the toggle button
    toggle.addEventListener("click", () => {
        const now = !sb.classList.contains("collapsed");
        applyState(now);
        setState(now);
    });

    // On page load, re-apply whatever the user last chose
    document.addEventListener("DOMContentLoaded", () => {
        applyState(getState());
    });
})();