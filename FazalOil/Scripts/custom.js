//***********************GLOBAL VARIABLES***************************//

var BASE_URL = "../../";
var ADD_PRODUCT = "Data/AddProduct";
var UPDATE_PRODUCT = "Data/UpdateProduct";
var GET_PRODUCTS = "Data/GetProducts";
var GET_BRANDS = "Data/GetBrands";
var GET_CATEGORIES = "Data/GetCategories";
var GET_SUPPLIER = "Data/GetSuppliers";
var GET_DROPOFF = "Data/GetDropoffs";
var ADD_INVOICE = "Data/AddInvoice";
var GET_AVAIL_STOCK = "Data/GetAvailableStock";

//***********************END GLOBAL VARIABLES***********************//

//***********************API FUNCTIONS******************************//

function saveProduct() {

    var productName = $("#txtProductName").val();
    var canSize = $("#txtCanSize").val();
    var totalCans = $("#txtNoOfCans").val();
    var totalLiters = $("#txtNoOfLitres").val();
    var quantity = $("#txtQuantity").val();
    var brandID = $("#selectBrand").val();
    var purchasingPrice = $("#txtPrice").val();
    var dropoffID = $("#selectLocation").val();
    var supplierID = $("#selectSupplier").val();
    var dateOfPurchase = $("#datePurchase").val();
    var productId = $("#modal_pid").html();

    if (productNamesList.indexOf(productName) > -1) {
        productId = indices[productNamesList.indexOf(productName)];
    }

    var category = $("#selectCategory").val();
    var categoryName = $("#selectCategory option:selected").text();

    var json = ""

    if (categoryName.toLowerCase() == "engine oil") {
        json = {
            "ProductID": productId,
            "CategoryID":category,
            "ProductName": productName,
            "CanSize": canSize,
            "totalCans": totalCans,
            "totalLiters": totalLiters,
            "BrandID": brandID,
            "PurchasingPrice": purchasingPrice,
            "DropoffID": dropoffID,
            "SupplierID": supplierID,
            "DateOfPurchase": dateOfPurchase
        }
    }
    else if (categoryName.toLowerCase() == "raw oil") {
        json = {
            "ProductID": productId,
            "CategoryID": category,
            "ProductName": productName,
            "totalLiters": totalLiters,
            "BrandID": brandID,
            "PurchasingPrice": purchasingPrice,
            "DropoffID": dropoffID,
            "SupplierID": supplierID,
            "DateOfPurchase": dateOfPurchase
        }
    }
    else {
        json = {
            "ProductID": productId,
            "CategoryID": category,
            "ProductName": productName,
            "TotalQuantity": quantity,
            "BrandID": brandID,
            "PurchasingPrice": purchasingPrice,
            "DropoffID": dropoffID,
            "SupplierID": supplierID,
            "DateOfPurchase": dateOfPurchase
        }
    }

    if (category == "-1") {
        $("#alertdiv").html('<div class="alert alert-danger">Please select a category<div>');
    }
    else if (dateOfPurchase == "") {
        $("#alertdiv").html('<div class="alert alert-danger">Please select a date<div>');
    }
    else if (dropoffID == "-1") {
        $("#alertdiv").html('<div class="alert alert-danger">Please select a location<div>');
    }
    else if (supplierID == "-1") {
        $("#alertdiv").html('<div class="alert alert-danger">Please select a supplier<div>');
    }
    else {
        $("#alertdiv").html('<div class="alert alert-warning">Processing your request...</div>');

        if ($("#btnSubmit").text().toLowerCase() == "save")
        {
            $.ajax({
                url: BASE_URL + ADD_PRODUCT,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    $("#alertdiv").html('<div class="alert alert-success">Successfully added</div>');
                    $('input').val('');
                    $('option').attr('selected', false);
                    SHOULD_LOAD = true;
                    setTimeout(
                      function () {
                          //do something special
                          $("#alertdiv").html('');
                      }, 2000);


                },
                error: function (err) {

                }
            });
        }
        else
        {
            $.ajax({
                url: BASE_URL + UPDATE_PRODUCT,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    $("#alertdiv").html('<div class="alert alert-success">Successfully added</div>');
                    $('input').val('');
                    $('option').attr('selected', false);
                    SHOULD_LOAD = true;
                    setTimeout(
                      function () {
                          //do something special
                          $("#alertdiv").html('');
                      }, 2000);
                },
                error: function (err) {

                }
            });
        }

        
    }
}

function getProductName(callback) {
    $.ajax({
        url: BASE_URL + GET_PRODUCTS,
        type: "GET",
        success: function (data) {
            callback(data);
        },
        error: function (err) {

        }
    });
}

function getAvailStock(callback) {
    $.ajax({
        url: BASE_URL + GET_AVAIL_STOCK,
        type: "GET",
        success: function (data) {
            callback(data);
        },
        error: function (err) {

        }
    });
}

function getCategories() {
    var cat = $("#selectCategory");
    cat.html('<option value="-1">-Select Category-</option>');

    $.ajax({
        url: BASE_URL + GET_CATEGORIES,
        type: "GET",
        success: function (data) {

            var categories = $.parseJSON(data);

            $.each(categories, function (index, item) {
                cat.append(new Option(item.CategoryName,item.CategoryID))
            });
        },
        error: function (err) {

        }
    });
}

function getBrands() {
    var br = $("#selectBrand");
    br.html('<option value="-1">-Select Brand-</option>');

    $.ajax({
        url: BASE_URL + GET_BRANDS,
        type: "GET",
        success: function (data) {

            var brands = $.parseJSON(data);

            $.each(brands, function (index, item) {
                br.append(new Option(item.BrandName, item.BrandID))
            });
        },
        error: function (err) {

        }
    });
}

function getSuppliers() {
    var br = $("#selectSupplier");
    br.html('<option value="-1">-Select supplier-</option>');

    $.ajax({
        url: BASE_URL + GET_SUPPLIER,
        type: "GET",
        success: function (data) {

            var brands = $.parseJSON(data);

            $.each(brands, function (index, item) {
                br.append(new Option(item.SupplierName, item.SupplierID))
            });
        },
        error: function (err) {

        }
    });
}

function getDropoffs() {
    var br = $("#selectLocation");
    br.html('<option value="-1">-Select location-</option>');

    $.ajax({
        url: BASE_URL + GET_DROPOFF,
        type: "GET",
        success: function (data) {

            var brands = $.parseJSON(data);

            $.each(brands, function (index, item) {
                br.append(new Option(item.DropoffName, item.DropoffID))
            });
        },
        error: function (err) {

        }
    });
}

function addInvoice(model,callback) {
    $.ajax({
        url: BASE_URL + ADD_INVOICE,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(model),
        success: function (data) {
            callback(data);
        },
        error: function (err) {
            callback("-1");
        }
    });
}

//***********************END API FUNCTIONS**************************//

function supllier() {
    var location = document.getElementById('selectLocation').value;
    if(location === "Depot")
    {
        document.getElementById('selectSupplier').disabled = true;
    }
    else
    {
        document.getElementById('selectSupplier').disabled = false;
    }
}

function nextChange() {
    var currentReading = document.getElementById('txtCurrentReading').value;
    var expectedKm = document.getElementById('txtExpectedkm').value;
    var nextChange = document.getElementById('txtNextChange');
    var result = parseInt(currentReading) + parseInt(expectedKm);
    nextChange.value = result;
}

function calculateLitres() {
    var canSize = document.getElementById('txtCanSize').value;
    var numberOfCans = document.getElementById('txtNoOfCans').value;
    var numberOfLitres = document.getElementById('txtNoOfLitres');
    var result = canSize * numberOfCans;
    numberOfLitres.value = result;
    totalPrice();
}

function categories() {
    var category = $("#selectCategory option:selected").text();

    if (category.toLowerCase() == "engine oil")
    {
        rawOilField("none",'0');
        filterFields("none");
        engineOilFields("inline-block");
    }
    else if (category.toLowerCase() == "filters" || category.toLowerCase() == "others")
    {
        rawOilField("none",'0');
        engineOilFields("none");
        filterFields("inline-block");
    }
    else if (category.toLowerCase() == "raw oil")
    {
        engineOilFields("none");
        filterFields("none");
        rawOilField("inline-block",'1');
    }
}

function engineOilFields(val) {

    $('#totalCansDiv').css({ display: val });
    $('#canSizeDiv').css({ display: val });
    $('#totalLitersDiv').css({ display: val });
    $('#priceDiv').css({ display: val });
    $('#totalPriceDiv').css({ display: val });
}

function filterFields(val) {

    $('#totalQuantityDiv').css({ display: val });
    $('#priceDiv').css({ display: val });
    $('#totalPriceDiv').css({ display: val });
    
}

function rawOilField(val,status) {
    $('#totalLitersDiv').css({ display: val });
    if (status == '0') {
        $('#txtNoOfLitres').prop('disabled',true);
    }
    else {
        $('#txtNoOfLitres').prop('disabled', false);
    }
    
    $('#priceDiv').css({ display: val });
    $('#totalPriceDiv').css({ display: val });
}
    
function totalPrice() {
    var category = $("#selectCategory option:selected").text();
    var canPrice = document.getElementById('txtPrice').value;
    var numberOfCans = document.getElementById('txtNoOfCans').value;
    var quantity = document.getElementById('txtQuantity').value;
    var totalPrice1 = document.getElementById('txtTotalPrice');
    var totalLiters = document.getElementById('txtNoOfLitres').value;
    
    if(category.toLowerCase() == "engine oil")
    {
        var result = canPrice * numberOfCans;
        totalPrice1.value = result;
    }
    else if (category.toLowerCase() == "raw oil")
    {
        var result = canPrice * totalLiters;
        totalPrice1.value = result;
    }
    else{
        var result = canPrice * quantity;
        totalPrice1.value = result;
    }
}