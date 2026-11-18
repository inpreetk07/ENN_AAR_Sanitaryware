
/* =========================================
   ENN AAR SANITARYWARE - FRONTEND LOGIC
========================================= */

// DEMO PRODUCTS
// Later, replace this array with products from SQL Server.

const products = [

    {
        id: 1,
        name: "Premium Countertop Basin",
        brand: "CERA",
        category: "Basins",
        model: "CB-101",
        price: 4500,
        image: "https://images.unsplash.com/photo-1620626011761-996317b8d101?w=700",
        description: "Elegant ceramic countertop wash basin for modern bathrooms.",
        badge: "POPULAR"
    },

    {
        id: 2,
        name: "Modern Wall Hung Toilet",
        brand: "HINDWARE",
        category: "Toilets",
        model: "HT-202",
        price: 8500,
        image: "https://images.unsplash.com/photo-1584622650111-993a426fbf0a?w=700",
        description: "Contemporary design with a clean and elegant finish.",
        badge: "FEATURED"
    },

    {
        id: 3,
        name: "Luxury Chrome Faucet",
        brand: "JAQUAR",
        category: "Faucets",
        model: "JF-303",
        price: 3200,
        image: "https://images.unsplash.com/photo-1620626011761-996317b8d101?w=700",
        description: "Premium chrome finish bathroom faucet.",
        badge: "NEW"
    },

    {
        id: 4,
        name: "Rainfall Shower Set",
        brand: "KOHLER",
        category: "Showers",
        model: "KS-404",
        price: 6500,
        image: "https://images.unsplash.com/photo-1620626011761-996317b8d101?w=700",
        description: "Modern rainfall shower set for a luxurious experience.",
        badge: "POPULAR"
    },

    {
        id: 5,
        name: "Designer Ceramic Basin",
        brand: "PARRYWARE",
        category: "Basins",
        model: "PB-505",
        price: 5200,
        image: "https://images.unsplash.com/photo-1620626011761-996317b8d101?w=700",
        description: "Stylish basin with a premium ceramic finish.",
        badge: ""
    },

    {
        id: 6,
        name: "Premium Bathroom Faucet",
        brand: "JAQUAR",
        category: "Faucets",
        model: "JF-606",
        price: 2800,
        image: "https://images.unsplash.com/photo-1620626011761-996317b8d101?w=700",
        description: "Elegant and durable bathroom fitting.",
        badge: "NEW"
    },

    {
        id: 7,
        name: "Designer Floor Mounted Toilet",
        brand: "CERA",
        category: "Toilets",
        model: "CT-707",
        price: 7200,
        image: "https://images.unsplash.com/photo-1584622650111-993a426fbf0a?w=700",
        description: "Elegant floor-mounted sanitaryware.",
        badge: ""
    },

    {
        id: 8,
        name: "Luxury Overhead Shower",
        brand: "KOHLER",
        category: "Showers",
        model: "KS-808",
        price: 4800,
        image: "https://images.unsplash.com/photo-1620626011761-996317b8d101?w=700",
        description: "Premium overhead shower for modern bathrooms.",
        badge: ""
    }

];


// CURRENT FILTER
let selectedCategory = "All";


// FORMAT PRICE
function formatPrice(price) {

    return new Intl.NumberFormat("en-IN", {
        style: "currency",
        currency: "INR",
        maximumFractionDigits: 0
    }).format(price);

}


// RENDER PRODUCTS
function renderProducts() {

    const grid = document.getElementById("productGrid");

    if (!grid) return;

    const search = document
        .getElementById("productSearch")
        .value.toLowerCase()
        .trim();

    const filteredProducts = products.filter(product => {

        const matchesCategory =
            selectedCategory === "All" ||
            product.category === selectedCategory;

        const matchesSearch =
            product.name.toLowerCase().includes(search) ||
            product.brand.toLowerCase().includes(search) ||
            product.model.toLowerCase().includes(search) ||
            product.category.toLowerCase().includes(search);

        return matchesCategory && matchesSearch;

    });


    grid.innerHTML = filteredProducts.map(product => `

        <div class="col-6 col-md-4 col-lg-3">

            <div class="product-card">

                <div class="product-image">

                    <img src="${product.image}"
                         alt="${product.name}"
                         loading="lazy"
                         onerror="this.src='https://placehold.co/600x450/f0eee9/333333?text=Product+Image'">

                    ${product.badge
                        ? `<span class="product-badge">${product.badge}</span>`
                        : ""}

                </div>

                <div class="product-content">

                    <span class="product-brand">
                        ${product.brand}
                    </span>

                    <h5>${product.name}</h5>

                    <p>Model: ${product.model}</p>

                    <div class="product-price">
                        ${formatPrice(product.price)}
                    </div>

                    <div class="product-actions">

                        <button class="view-btn"
                                onclick="showProduct(${product.id})">
                            View Details
                        </button>

                        <button class="enquiry-btn"
                                onclick="enquireProduct(${product.id})">
                            <i class="bi bi-whatsapp"></i>
                        </button>

                    </div>

                </div>

            </div>

        </div>

    `).join("");


    document.getElementById("noProducts")
        .classList.toggle("d-none", filteredProducts.length > 0);

}


// CATEGORY FILTER
function filterCategory(category) {

    selectedCategory = category;

    document.querySelectorAll(".filter-btn").forEach(button => {

        button.classList.toggle(
            "active",
            button.dataset.category === category
        );

    });

    renderProducts();

}


// CATEGORY BUTTON EVENTS
document.querySelectorAll(".filter-btn").forEach(button => {

    button.addEventListener("click", function () {

        filterCategory(this.dataset.category);

    });

});


// SEARCH EVENTS
document.getElementById("productSearch")
    ?.addEventListener("input", renderProducts);


// SHOW PRODUCT DETAILS
function showProduct(id) {

    const product = products.find(p => p.id === id);

    if (!product) return;

    document.getElementById("productModalBody").innerHTML = `

        <div class="row g-4">

            <div class="col-md-6">

                <img src="${product.image}"
                     class="modal-product-image"
                     alt="${product.name}">

            </div>

            <div class="col-md-6">

                <span class="product-brand">
                    ${product.brand}
                </span>

                <h3 class="mt-3">${product.name}</h3>

                <p class="text-secondary">
                    ${product.description}
                </p>

                <hr>

                <p><strong>Category:</strong> ${product.category}</p>

                <p><strong>Model:</strong> ${product.model}</p>

                <h3 class="product-price">
                    ${formatPrice(product.price)}
                </h3>

                <button class="btn btn-dark rounded-pill w-100 mt-3 py-3"
                        onclick="enquireProduct(${product.id})">

                    <i class="bi bi-whatsapp me-2"></i>
                    Enquire On WhatsApp

                </button>

            </div>

        </div>

    `;

    const modal = new bootstrap.Modal(
        document.getElementById("productModal")
    );

    modal.show();

}


// WHATSAPP ENQUIRY
function enquireProduct(id) {

    const product = products.find(p => p.id === id);

    if (!product) return;

    // Replace with the actual store WhatsApp number.
    const phone = "919815172233";

    const message =
        `Hello, I am interested in ${product.name}. ` +
        `Brand: ${product.brand}. ` +
        `Model: ${product.model}. ` +
        `Price: ${formatPrice(product.price)}. ` +
        `Please provide more details.`;

    const url =
        `https://wa.me/${phone}?text=${encodeURIComponent(message)}`;

    window.open(url, "_blank");

}


// INITIALIZE CATALOGUE
document.addEventListener("DOMContentLoaded", function () {

    renderProducts();

});
