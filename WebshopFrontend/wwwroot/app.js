function AddProductToLocalStorage(product) {
    var products = GetProductsFromLocalStorage();
    products.push(product);
    localStorage.setItem("products", JSON.stringify(products));
}

function RemoveProductFromLocalStorage(product) {
    var products = GetProductsFromLocalStorage();
    var index = products.indexOf(product);
    if (index > -1) {
        products.splice(index, 1);
    }
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