
function AddItemToLocalStorage(item) {
    var cart = GetCartFromLocalStorage();

    if (ItemExistsInLocalStorage(item.artNr)) {
        var index = cart.findIndex(p => p.artNr === item.artNr);
        cart[index].quantity += item.quantity;
        localStorage.setItem("cart", JSON.stringify(cart));
    }
    else {
        
        cart.push(item);
        localStorage.setItem("cart", JSON.stringify(cart));
    }
}

function GetCartFromLocalStorage() {
    var cart = localStorage.getItem("cart");
    if (cart === null) {
        return [];
    }
    return JSON.parse(cart);
}

function RemoveCartFromLocalStorage() {
    localStorage.removeItem("cart");
}

function ItemExistsInLocalStorage(artNr) {
    var cart = GetCartFromLocalStorage();
    return cart.some(p => p.artNr === artNr);
}
