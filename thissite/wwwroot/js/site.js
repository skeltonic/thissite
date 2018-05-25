// Write your JavaScript code.
$(document).ready(() => {
    $.getJSON("/Home/CartSummary", (response) => {
        $("#cartBadge").text(response.cartItems.reduce((accumulator, item) => { return accumulator + item.quantity }, 0));
    })
})