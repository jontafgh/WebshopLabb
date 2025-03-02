function AddProductToLocalStorage(product) {
    var products = GetProductsFromLocalStorage();
    products.push(product);
    localStorage.setItem("products", JSON.stringify(products));
}

function EmptyLocalStorage() {
    localStorage.clear();
}

function GetProductsFromLocalStorage() {
    var products = localStorage.getItem("products");
    if (products === null) {
        return [];
    }
    return JSON.parse(products);
}

function GetProductsFromLocalStorageAsString() {
    var products = localStorage.getItem("products");
    return products;
}


function GetNumberOfProductsInLocalStorage() {
    return GetProductsFromLocalStorage().length;
}