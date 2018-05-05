//***********************GLOBAL VARIABLES***************************//

var BASE_URL = "../../";
var ADD_PRODUCT = "Data/AddProduct";

//***********************END GLOBAL VARIABLES***********************//


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
}

function categories() {
    var category = $("#selectCategory option:selected").text();
    if(category === "Oil")
    {
        document.getElementById('txtQuantity').disabled = true;
        document.getElementById('txtCanSize').disabled = false;
        document.getElementById('txtNoOfCans').disabled = false;
    }
    else
    {
        document.getElementById('txtQuantity').disabled = false;
        document.getElementById('txtCanSize').disabled = true;
        document.getElementById('txtNoOfCans').disabled = true;
    }
}
    
function totalPrice() {
    var category = document.getElementById('selectCategory').value;
    var canPrice = document.getElementById('txtPrice').value;
    var numberOfCans = document.getElementById('txtNoOfCans').value;
    var quantity = document.getElementById('txtQuantity').value;
    var totalPrice1 = document.getElementById('txtTotalPrice');
    
    if(category === "Oil")
    {
        var result = canPrice * numberOfCans;
        totalPrice1.value = result;
    }
    else
    {
        var result = canPrice * quantity;
        totalPrice1.value = result;
    }
}

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

    var category = $("#selectCategory").val();

    var json = ""

    if (category == "1") {
        json = {
            "ProductName": productName,
            "CanSize": canSize,
            "totalCans": totalCans,
            "totalLiters":totalLiters,
            "BrandID": brandID,
            "PurchasingPrice": purchasingPrice,
            "DropoffID": dropoffID,
            "SupplierID": supplierID,
            "DateOfPurchase": dateOfPurchase
        }
    }
    else {
        json = {
            "ProductName": productName,
            "Quantity":quantity,
            "BrandID": brandID,
            "PurchasingPrice": purchasingPrice,
            "DropoffID": dropoffID,
            "SupplierID": supplierID,
            "DateOfPurchase": dateOfPurchase
        }
    }
    
    $("#alertdiv").html('<div class="alert alert-warning"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>Please wait...</div>');


    $.ajax({
        url: BASE_URL + ADD_PRODUCT,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(json),
        success:function(data){
            $("#alertdiv").html('<div class="alert alert-success">Successfully added</div>');
            $('input').val('');
            $('option').attr('selected', false);
            setTimeout(
              function () {
                  //do something special
                  $("#alertdiv").html('');
              }, 3000);

            
        },
        error:function(err){

        }
    });
}