document.addEventListener("DOMContentLoaded", function () {
    // DOM elements
    const searchInput = document.getElementById("search-input");
    const categoryCheckboxes = document.querySelectorAll(".category-checkbox");
    const menuTabs = document.querySelectorAll("#menu-tabs .nav-link");
    const menuItems = document.querySelectorAll(".menu-item");

    // Filter states
    let selectedCategories = new Set();
    let selectedMenu = "all";
    let searchTerm = "";

    // Functions
    // Filtering logic for each menu item
    function filterItems() {
        menuItems.forEach(item => {
            const categoryId = item.dataset.categoryId;
            const menuName = item.dataset.menuName;
            const itemName = item.querySelector(".card-title")?.textContent.toLowerCase() || "";

            const matchesCategory = selectedCategories.size === 0 || selectedCategories.has(categoryId);
            const matchesMenu = selectedMenu === "all" || menuName === selectedMenu;
            const matchesSearch = searchTerm === "" || itemName.includes(searchTerm);

            if (matchesCategory && matchesMenu && matchesSearch) {
                item.style.display = "block";
            } else {
                item.style.display = "none";
            }
        });
        // Hide entire menu sections if there all menu items have display none
        document.querySelectorAll(".menu-section").forEach(section => {
            const visibleItems = section.querySelectorAll(".menu-item:not([style*='display: none'])");
            if (visibleItems.length === 0) {
                section.style.display = "none";
            } else {
                section.style.display = "block";
            }
        });
    }
    function saveFiltersToCookie() {
        const filters = {
            selectedCategories: Array.from(selectedCategories),
            selectedMenu,
            searchTerm
        };
        document.cookie = "menuFilters=" + encodeURIComponent(JSON.stringify(filters)) + "; path=/";
    }
    function loadFiltersFromCookie() {
        const cookies = document.cookie.split("; ");
        const cookie = cookies.find(c => c.startsWith("menuFilters="));
        if (!cookie) return;

        try {
            const data = JSON.parse(decodeURIComponent(cookie.split("=")[1]));
            if (data.selectedCategories) {
                selectedCategories = new Set(data.selectedCategories);
                data.selectedCategories.forEach(catId => {
                    const cb = document.querySelector(`.category-checkbox[value="${catId}"]`);
                    if (cb) cb.checked = true;
                });
            }

            if (data.searchTerm) {
                searchTerm = data.searchTerm;
                if (searchInput) searchInput.value = searchTerm;
            }

            if (data.selectedMenu) {
                selectedMenu = data.selectedMenu;
                menuTabs.forEach(tab => {
                    tab.classList.remove("active");
                    if (tab.dataset.menuFilter.toLowerCase() === selectedMenu) {
                        tab.classList.add("active");
                    }
                });
            }
        } catch (e) {
            console.error("Failed to parse filters from cookie", e);
        }
    }

    // Load cookie values
    loadFiltersFromCookie();
    filterItems();

    // Event listeners for filters
    categoryCheckboxes.forEach(checkbox => {
        checkbox.addEventListener("change", () => {
            selectedCategories = new Set(
                Array.from(categoryCheckboxes)
                    .filter(cb => cb.checked)
                    .map(cb => cb.value)
            );
            filterItems();
            saveFiltersToCookie();
        });
    });

    searchInput?.addEventListener("input", () => {
        searchTerm = searchInput.value.toLowerCase();
        filterItems();
        saveFiltersToCookie();
    });

    menuTabs.forEach(tab => {
        tab.addEventListener("click", (e) => {
            e.preventDefault();
            menuTabs.forEach(t => t.classList.remove("active"));
            tab.classList.add("active");
            selectedMenu = tab.dataset.menuFilter.toLowerCase();
            filterItems();
            saveFiltersToCookie();
        });
    });

    const clearBtn = document.querySelector(".clear-filter-btn");
    clearBtn?.addEventListener("click", () => {
        categoryCheckboxes.forEach(cb => cb.checked = false);
        selectedCategories.clear();
        searchInput.value = "";
        searchTerm = "";
        selectedMenu = "all";
        menuTabs.forEach(t => t.classList.remove("active"));
        menuTabs[0].classList.add("active");
        filterItems();
        saveFiltersToCookie();
    });
});