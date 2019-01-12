"use strict";
var g_size = 0;
var g_total = 0;
var g_Choices = [];
var pastaType;
(function (pastaType) {
    pastaType[pastaType["Bolognese"] = 0] = "Bolognese";
    pastaType[pastaType["Veggie"] = 1] = "Veggie";
    pastaType[pastaType["Vegan"] = 2] = "Vegan";
})(pastaType || (pastaType = {}));
var Choice = /** @class */ (function () {
    function Choice(id, amount, type, kids) {
        this.Id = id;
        this.Amount = amount;
        this.Type = type;
        this.Kids = kids;
    }
    return Choice;
}());
var Order = /** @class */ (function () {
    function Order(amount, name, email, takeAway, timeArrival) {
        this.Id = 0;
        this.Amount = amount;
        this.Choices = [];
        this.Name = name;
        this.Email = email;
        this.TakeAway = takeAway;
        this.TimeArrival = timeArrival;
    }
    return Order;
}());
function finish(id) {
    $("#amountToPay").text("€ " + g_total);
    $("#subId").text(id);
    $("#contentForm").hide();
    $("#finishForm").show();
    $("#contentForm").unblock();
}
function error() {
    $("#contentForm").unblock();
    $("#contentForm").hide();
    $("#errorForm").show();
}
function store() {
    $("#contentForm").block();
    var order = new Order(g_total, $("#fullName").val(), $("#email").val(), $("input[id='takeAway']:checked").val() === "1" ? true : false, $("#arrival").val());
    order.Choices = g_Choices;
    callFunctionApp("POST", "https://bprsubscribe.azurewebsites.net/api/Subscribe", finish, error, JSON.stringify(order));
}
function createSubscriptions() {
    if ($("#groupsize").val() && g_size !== $("#groupsize").val()) {
        $("#subscriptionPlaceholder").empty();
        g_size = Number($("#groupsize").val());
        createSubscriptionLines();
    }
}
function sum(e) {
    var id = e.id.slice(-1);
    var choice = _.find(g_Choices, function (o) {
        return o.Id === Number(id);
    });
    var discount = $("input[id='kidsMeal" + id + "']:checked").val() !== undefined;
    if (choice)
        g_Choices = _.without(g_Choices, choice);
    switch (e.value) {
        case "bolognese":
            g_Choices.push(new Choice(Number(id), discount ? 6 : 8, pastaType.Bolognese, discount));
            break;
        case "veggie":
            g_Choices.push(new Choice(Number(id), discount ? 6 : 8, pastaType.Veggie, discount));
            break;
        default:
            g_Choices.push(new Choice(Number(id), discount ? 8 : 10, pastaType.Vegan, discount));
            break;
    }
    g_total = 0;
    _.each(g_Choices, function (o) {
        g_total += o.Amount;
    });
    $("#totalAmount").text(g_total);
    $("#total").show();
}
function discount(e) {
    var id = e.id.slice(-1);
    var choice = _.find(g_Choices, function (o) {
        return o.Id === Number(id);
    });
    if (!choice)
        return;
    var radio = {};
    radio.id = e.id;
    radio.value = $("input[name='inlineRadioOptions" + id + "']:checked").val();
    sum(radio);
}
function createSubscriptionLines() {
    for (var index = 1; index < g_size + 1; index++) {
        var html = '<div>' +
            '<label for="fullName' +
            index +
            '">Keuze persoon ' +
            index +
            "</label>" +
            "</div>" +
            '<div class="form-group custom-control custom-radio custom-control-inline">' +
            '<input class="custom-control-input" type="radio" name="inlineRadioOptions' +
            index +
            '" id="bolognese' +
            index +
            '" value="bolognese" required onClick=sum(this)>' +
            '<label class="custom-control-label" for="bolognese' +
            index +
            '">Bolognese: 8€</label>' +
            "</div>" +
            '<div class="form-group custom-control custom-radio custom-control-inline">' +
            '<input class="custom-control-input" type="radio" name="inlineRadioOptions' +
            index +
            '" id="veggie' +
            index +
            '" value="veggie" required onClick=sum(this)>' +
            '<label class="custom-control-label" for="veggie' +
            index +
            '">Vegetarisch: 8€</label>' +
            "</div>" +
            '<div class="form-group custom-control custom-radio custom-control-inline">' +
            '<input class="custom-control-input" type="radio" name="inlineRadioOptions' +
            index +
            '" id="vegan' +
            index +
            '" value="vegan" required onClick=sum(this)>' +
            '<label class="custom-control-label" for="vegan' +
            index +
            '">Vegan: 10€</label>' +
            "</div>" +
            '<div class="form-check custom-control-inline custom-checkbox">' +
            '<input class="custom-control-input" type="checkbox" id="kidsMeal' +
            index +
            '" value="1" onClick=discount(this)>' +
            '<label class="custom-control-label" for="kidsMeal' +
            index +
            '">Kinderportie (-€2)</label>' +
            '</div>';
        $("#subscriptionPlaceholder").append(html);
    }
}
$(function () {
    $("#groupsize").change(function () {
        createSubscriptions();
    });
    $('#contactForm').submit(function () {
        store();
        return false;
    });
});
//# sourceMappingURL=subscribe.js.map