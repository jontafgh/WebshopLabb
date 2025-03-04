function AddItemToLocalStorage(item) {
    var cart = GetCartFromLocalStorage();
    cart.push(item);
    localStorage.setItem("cart", JSON.stringify(cart));
}

function GetCartFromLocalStorage() {
    var cart = localStorage.getItem("cart");
    if (cart === null) {
        return [];
    }
    return JSON.parse(cart);
}

function GetItemFromLocalStorage(itemId) {
    var cart = GetCartFromLocalStorage();
    var item = cart.find(p => p.Id === itemId);
    return JSON.parse(item);
}

function UpdateItemInLocalStorage(item) {
    var cart = GetCartFromLocalStorage();
    var index = cart.findIndex(p => p.Id === item.Id);
    var newItem = null;
    if (index > -1) {
        cart[index].Quantity += item.Quantity;
        newItem = cart[index];
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    return newItem;
}

function RemoveItemFromLocalStorage(itemId) {
    var cart = GetCartFromLocalStorage();
    var index = cart.findIndex(p => p.Id === itemId);
    var removedItem = null;
    if (index > -1) {
        removedItem = cart[index];
        cart.splice(index, 1);
    }
    localStorage.setItem("cart", JSON.stringify(cart));
    return removedItem;
}

function RemoveCartFromLocalStorage() {
    localStorage.removeItem("cart");
}