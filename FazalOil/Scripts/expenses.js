$(document).ready(function () {
    var d = new Date();
    var strDate = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
    getAllExpenses(strDate);
});

function saveExpense() {
    var date = $('#datePurchase').val();
    var amt = parseInt($('#txtAmount').val());
    var personName = $('#txtPerson').val();
    var purpose = $('#txtPurpose').val();

    var json = {
        PersonName: personName,
        Purpose: purpose,
        Amount: amt,
        Date : date
    }
    $("#alertdiv").html('<div class="alert alert-warning">Processing request...</div>');

    addExpense(json, function (data) {
        if (data != "-1") {
            $("#alertdiv").html('<div class="alert alert-success">Successfully added</div>');
            $('#datePurchase').val('');
            $('#txtAmount').val('');
            $('#txtPerson').val('');
            $('#txtPurpose').val('');
            setTimeout(
              function () {
                  //do something special
                  $("#alertdiv").html('');
                  clearProduct();
              }, 2000);
            
        }
    });
}

$("#dtPicker").change(function () {
    var dt = $(this).val();
    getAllExpenses(dt);
});

function getAllExpenses(date) {

    var dt = $('#expenseTable').DataTable();

    getExpenses(date,function (data) {
        dt.clear();
        var arr = JSON.parse(data);

        if (arr.length > 0) {
            $.each(arr, function (index, item) {
                dt.row.add([
                    (index + 1),
                    item.PersonName,
                    item.Purpose,
                    item.Amount,
                    item.Date.split('T')[0]
                ]).draw();
            });
        }
        else {
            alert("No expenses on " + date.split().reverse().join());
        }
        
    });

}