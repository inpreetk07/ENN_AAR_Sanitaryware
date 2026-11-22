
document.addEventListener("DOMContentLoaded", function () {

    const productGrid = document.getElementById("productGrid");
    const searchInput = document.getElementById("productSearch");
    const noProducts = document.getElementById("noProducts");
    const filterButtons = document.querySelectorAll("#categoryFilters .filter-btn");

    if (!productGrid) return;

    let selectedCategory = "all";

    function filterProducts() {
        const search = searchInput
            ? searchInput.value.toLowerCase().trim()
            : "";

        const items = document.querySelectorAll(
            "#productGrid .product-item, .showroom-item"
        );
        let visibleCount = 0;

        items.forEach(item => {
            const category = (item.dataset.category || "").trim().toLowerCase();
            const text = item.textContent.toLowerCase();

            const matchesCategory =
                selectedCategory === "all" ||
                category === selectedCategory;

            const matchesSearch = text.includes(search);

            const visible = matchesCategory && matchesSearch;

            item.style.display = visible ? "" : "none";

            if (visible) visibleCount++;
        });

        if (noProducts) {
            noProducts.classList.toggle("d-none", visibleCount > 0);
        }
    }

    filterButtons.forEach(button => {
        button.addEventListener("click", function () {
            selectedCategory = (this.dataset.category || "all")
                .trim().toLowerCase();

            filterButtons.forEach(btn =>
                btn.classList.toggle("active", btn === this)
            );

            filterProducts();
        });
    });

    if (searchInput) {
        searchInput.addEventListener("input", filterProducts);
    }

    filterProducts();
});
