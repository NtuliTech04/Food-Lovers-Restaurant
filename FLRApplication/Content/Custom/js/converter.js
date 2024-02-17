
var zeroAmt = Number(0).toFixed(2);
var available = Number($('#Points_Available').val());
var orderTot = Number($('#Payment_OrderTotal').val());

var spendInput = document.getElementById("toSpend");
var getDiscount = document.getElementById("Discount_DiscountAmount");

$("#toSpend").keyup(function () {
    var spendPoint = Number($('#toSpend').val());

    if (spendPoint <= available && spendPoint >= 0) {

        var calcDisc = (spendPoint * 10 / 100).toFixed(2);

        if (calcDisc > orderTot) {
            getDiscount.value = zeroAmt;
            alert("Points to spend have exceeded your order total");
            return false;
        }
    }
    else {
        getDiscount.value = zeroAmt;
        spendInput.value = null;
        alert("Invalid Input");

        return false;
    }
    getDiscount.value = calcDisc;
});