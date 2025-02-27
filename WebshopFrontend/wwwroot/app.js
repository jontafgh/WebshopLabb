function AddProductToLocalStorage(product) {
    var products = GetProductsFromLocalStorage();
    products.push(product);
    localStorage.setItem("products", JSON.stringify(products));
}

function GetProductsFromLocalStorage() {
    var products = localStorage.getItem("products");
    if (products === null) {
        return [];
    }
    return JSON.parse(products);
}

function GetNumberOfProductsInLocalStorage() {
    return GetProductsFromLocalStorage().length;
}