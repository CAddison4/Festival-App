// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const ticketRequests = [];
const qtyAvailableById = [];

function updateShoppingCart(event) {
    var clickedId = event.target.id;
    var elementIdSplit = clickedId.split('-');
    var id = elementIdSplit[0]; // id of the TicketOptionVM item
    var action = elementIdSplit[1]; // add or remove
    var ticketType = event.target.dataset.type;
    var ticketPrice = event.target.dataset.price;
    const qty = event.target.dataset.qty * 1; // initial qty available for this ticket type at the time of clicking the button
    const index = qtyAvailableById.findIndex(obj => obj.id === id);
    if (index === -1) {
        qtyAvailableById.push({ id, qty });
        console.log("qtyAvailableById", qtyAvailableById)
    }
    ChangeCart(id, action, ticketPrice, ticketType);
}

function ChangeCart(id, action, ticketPrice, ticketType) {
    var cartQtyId = "#" + id + "-quantity";
    var cartAmtId = "#" + id + "-amount";

    var quantity = $(cartQtyId).html();
    const index = qtyAvailableById.findIndex(obj => obj.id === id);
    const qtyObj = qtyAvailableById[index];
    console.log("qtyAvailable before conditionals", qtyObj.qty)

    if (action == "add") {
        if (qtyObj.qty === 0) {
            console.log("Quantity available is 0");
            return;
        }
        quantity++;
        qtyObj.qty--;
        console.log("Quantity available: " + qtyObj.qty);
    } else {
        if (quantity * 1 === 0) {
            console.log("Nothing to remove from cart");
            return;
        }
        quantity--;
        qtyObj.qty++;
        console.log("Quantity available: " + qtyObj.qty);
        if (quantity * 1 < 0) {
            quantity = 0;
        }
    }

    const ticketRequestIndexOfCurrentType = ticketRequests.findIndex(request => request.ticketType === ticketType);

    if (ticketRequestIndexOfCurrentType === -1) {
        ticketRequests.push({
            ticketType,
            quantity
        })
    } else {
        ticketRequests[ticketRequestIndexOfCurrentType].quantity = quantity;
    }

    $(cartQtyId).text(quantity);

    //Calculate new amount
    const quantityNum = quantity * 1;
    const ticketPriceNum = ticketPrice * 1;
    var newAmount = (ticketPriceNum * quantityNum).toFixed(2);

    $(cartAmtId).text(newAmount);

    var totalItemsId = "#totalItems";
    var totalAmountId = "#totalAmount";

    //Calculate totals
    var totalQuantity = 0;
    $('.quantity').each(function () {
        var thisQuantity = $(this).html();
        totalQuantity += parseInt(thisQuantity);
    });
    var totalAmount = 0.00;
    $('.amount').each(function () {
        var thisAmount = $(this).html();
        totalAmount += parseFloat(thisAmount);
    });

    $("#totalItems").text(totalQuantity);
    $("#totalAmount").text(totalAmount.toFixed(2));
}

paypal.Button.render({
    env: 'sandbox',
    style: {
        size: 'small',
        color: 'gold',
        shape: 'pill',
        label: 'checkout'
    },
    client: {
        // You need to change this to your client ID
        sandbox: 'AY6FwsqKZKOJ2wSYZM6Idv2oinvnUMKMepprrSKvg_JpwRo_r2PkuTp0O5yBpNM2uZqWc6UUnHXUho3W',
        // production: '3W8F5EEJKUJP4KSY'  // Switch to ‘production’ when live.
    },

    commit: true,

    payment: function (data, actions) {
        return actions.payment.create({
            payment: {
                transactions: [{

                    custom: 'Custom data goes here!',
                    amount: {
                        total:
                            document.getElementById("totalAmount").innerHTML
                        , currency:
                            'CAD'
                    }
                }]
            }
        });
    },

    onAuthorize: function (data, actions) {
        return actions.payment.execute().then(function (payment) {
            //console.log("This is what comes back from Paypal: ")
            //console.log(payment); 
            //console.log("data1: " + JSON.stringify({ payerEmail: payment.payer.payer_info.email }));
            //console.log("data2: " + JSON.stringify({ ticketRequests: ticketRequests }));

            var dataObject = {
                "PayerEmail": payment.payer.payer_info.email,
                "TicketRequests": "ticketRequests",
            }


            $.ajax({
                type: "POST",
                url: "/Ticket/PaySuccess",
                data: JSON.stringify({
                    PayerEmail: payment.payer.payer_info.email
                    , TicketRequests: ticketRequests
                }),
                contentType: "application/json",
                success: function (response) {
                    console.log("success", response);
                    window.location.href = "/Ticket/PurchaseConfirmation?id=" + response;
                },
                error: function (response) {
                    console.log("failure", response);
                }
            });
        })
    },

    onCancel: function (data, actions) {
    },
}, '#paypal-button');