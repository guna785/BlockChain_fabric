var addDonationUrl = "/InsertData/AddDonation";
var addRequestUrl = "/InsertData/AddRequest";

function JsonPOST(url, data) {
    $.ajax({
        url: url,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify(data),
        processData: false,
        async: true,
        success: function (response) {
            sweetAlert('Congratulations!', response.status, 'success');
            $(".modal").modal("hide");
        },
        error: function (e) {
            swal("Oops", e.responseText, "error");
            // $(".modal").modal("hide");
        }
    });
}

function DoAction(action, data) {

    if (action === "AddDonation") {
        JsonPOST(addDonationUrl, data);
        LoadData();
    }
    else if (action === "AddRequest") {
        JsonPOST(addRequestUrl, data);
        LoadData();
    }
   

}

