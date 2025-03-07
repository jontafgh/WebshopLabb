function AddItemToLocalStorage(item) {
    var cart = GetCartFromLocalStorage();

    if (ItemExistsInLocalStorage(item.id)) {
        var index = cart.findIndex(p => p.id === item.id);
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

function GetItemFromLocalStorage(itemId) {
    var cart = GetCartFromLocalStorage();
    var item = cart.find(p => p.id === itemId);
    return item;
}

function UpdateItemInLocalStorage(item) {
    var cart = GetCartFromLocalStorage();
    var index = cart.findIndex(p => p.id === item.id);
    cart[index].quantity = item.quantity;
    var newItem = GetItemFromLocalStorage(item.id);
    localStorage.setItem("cart", JSON.stringify(cart));
    return newItem;
}

function RemoveItemFromLocalStorage(itemId) {
    var cart = GetCartFromLocalStorage();
    var index = cart.findIndex(p => p.id === itemId);
    var removedItem = GetItemFromLocalStorage(itemId);
    cart.splice(index, 1);
    localStorage.setItem("cart", JSON.stringify(cart));
    return removedItem;
}

function RemoveCartFromLocalStorage() {
    localStorage.removeItem("cart");
}

function ItemExistsInLocalStorage(itemId) {
    var cart = GetCartFromLocalStorage();
    return cart.some(p => p.id === itemId);
}

function GetCartTotalQuantityFromLocalStorage() {
    return GetCartFromLocalStorage().length;
}