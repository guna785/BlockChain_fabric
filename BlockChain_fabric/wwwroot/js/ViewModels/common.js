var InsertDeviceUrl = "/InsertData/InsertDevice";
var EditDeviceUrl = "/EditData/EditDevice";

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

    if (action === "AddDevice") {
        JsonPOST(InsertDeviceUrl, data);
        LoadData();
    }
    else if (action === "EditDevice") {
        JsonPOST(EditDeviceUrl, data);
        LoadData();
    }
   

}

