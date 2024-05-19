let tableData;
let rowSelected;

const BASIC_MODEL = {
    idGatePass: 0,
    name: "",
    companyName: "",
    contactNo: "",
    nic: "",
    vechicleType: "",
    vechicleNo: "",
    remarks: "",
    userID: 0,
    item: "",
    checkIn: null,
    checkOut: null,
    status: "Non-Returnable",
    isReceived: 2
}


$(document).ready(function () {

    //fetch("/Setup/GetBanks")
    //    .then(response => {
    //        return response.ok ? response.json() : Promise.reject(response);
    //    }).then(responseJson => {
    //        if (responseJson.data.length > 0) {

    //            responseJson.data.forEach((item) => {
    //                $("#cboBank").append(
    //                    // $("<option>").val(item.idBank).text(item.description)
    //                    //$("<option>").val("Bank not defined").text("Select Bank")
    //                    $("<option>").val(item.idCategory).text(item.description)
    //                )
    //            });

    //        }
    //    })


    tableData = $("#tbData").DataTable({
        responsive: true,
        "ajax": {
            "url": "/Setup/GetGatePass",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                data: 'dateGP',
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('DD/MM/YY, h:mm a');
                }
            },
            { "data": "idGatePass" },
            { "data": "name" },
            { "data": "companyName" },
            { "data": "contactNo" },
            //{ "data": "nic" },
            { "data": "vechicleType" },
            /*  { "data": "vechicleNo" },*/

            { "data": "status" },
            {
                "data": "isReceived", render: function (data) {
                    if (data == 1)
                        return '<span class="badge badge-success">Received</span>';
                    else if (data == 2)
                        return '<span class="badge badge-info">No Need</span>';
                    else
                        return '<span class="badge badge-danger">Pending</span>';
                }
            },


            {
                "data": "idGatePass", render: function (data) {
                    return `<a  href="/Setup/ShowPDFSale?saleNumber=${data}" target="_blank" class="btn btn-info btn-sm mr-1" /><i class="mdi mdi-eye"></i></a>` +
                        '<button class="btn btn-primary btn-edit btn-sm mr-1"><i class="mdi mdi-pencil"></i></button>' +
                        '<button class="btn btn-danger btn-delete btn-sm "><i class="mdi mdi-trash-can"></i></button>';
                }
            }


        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Export Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Report Products',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }, 'pageLength'
        ]
    });
})

var countSno = 0;
const openModal = (model = BASIC_MODEL) => {

    $("#childTable tr").remove();
    countSno = 0;
    $('#childTable').find('thead').append('<tr><th scope="col" style="width:4%;">No.</th>    <th scope="col" style="width:15%;">Name</th>   <th scope="col" style="width:22%;">Description</th>    <th scope="col" style="width:22%;">Remarks</th>       <th scope="col" style="width:10%;"><select class="form-control form-control-sm" id="Q_P"><option value="Quantity">Quantity</option><option value="Pieces">Pieces</option></select></th>      <th scope="col" style="width:10%;"><select class="form-control form-control-sm" id="K_L"><option value="KG">KG</option><option value="LBS">LBS</option></select></th>    <th scope="col" style="width:10%;"><select class="form-control form-control-sm" id="C_C_B"><option value="Cone">Cone</option><option value="Carton">Carton</option><option value="Bags">Bags</option></select></th>    <th scope="" style="width:5%;"></th>    </tr>');



    if (model.item === null || model.item === '') {
        childrenRow();
    }
    else {
        //<Column data Binding according to data>
        var noOfRows = model.item.split("|");
        for (let i = 1; i < noOfRows.length; i++) {
            var value = noOfRows[i - 1].replace("|", "");
            var array = value.split(";");

            countSno++;
            $('#childTable').find('tbody').append('<tr><th scope="row" style="width:4%;">' + countSno + '</th><td style="width:15%;"><input type="text" name="name" class="form-control" value="' + array[1] + '" /></td><td style="width:22%;"><input type="text" name="school" class="form-control" value="' + array[2] + '" /></td><td style="width:22%;"><input type="text" name="year" class="form-control" value="' + array[3] + '" /></td><td style="width:10%;"><input type="text" name="age"  class="form-control" value="' + array[4] + '" /></td><td style="width:10%;"><input type="text" name="age"  class="form-control" value="' + array[5] + '" /></td><td style="width:10%;"><input type="text" name="age"  class="form-control" value="' + array[6] + '" /></td><td style="width:5%;"><input type="button" class="btn btn-block btn-default" id="addrow" onclick="childrenRow()" value="+" /></td></tr>');

        }
       

        //<Column Binding according to data>
        //var itemD = model.itemDetail.split("|");
        //for (let i = 0; i < 1; i++) {

        //    var itemDValue = itemD[i].replace("|", "");
        //    var itemDArray = itemDValue.split(";");
        //    var Q_P = itemDArray[3].split(":");
        //    var K_L = itemDArray[4].split(":");
        //    var C_C_B = itemDArray[5].split(":");
        //}

        //$("#Q_P").val(Q_P[0].trim().replace("Qty", "Quantity"));
        //$("#K_L").val(K_L[0].trim());
        //$("#C_C_B").val(C_C_B[0].trim());

        //</Column data Binding according to data>
        var Q_P = '';
        var K_L = '';
        var C_C_B = '';
        if (model.itemDetail.includes("Qty:")) {
            Q_P = 'Quantity';
        } else {
            Q_P = 'Pieces';
        }

        if (model.itemDetail.includes("LBS:")) {
            K_L = 'LBS';
        }
        else {
            K_L = 'KG';
        }

        if (model.itemDetail.includes("Bags:")) {
            C_C_B = 'Bags';
        }
        else if (model.itemDetail.includes("Cone:")) {
            C_C_B = 'Cone';
        }
        else {
            C_C_B = 'Carton';
        }

        $("#Q_P").val(Q_P);
        $("#K_L").val(K_L);
        $("#C_C_B").val(C_C_B);
        //</Column Binding according to data>
    }


    $("#txtId").val(model.idGatePass);
    $("#txtName").val(model.name);
    $("#txtCompany").val(model.companyName);
    $("#txtContact").val(model.contactNo);
    $("#txtVType").val(model.vechicleType);
    $("#txtVTNo").val(model.vechicleNo);
    $("#txtRemarks").val(model.remarks);
    $("#cboStatus").val(model.status);
    $("#txtNIC").val(model.nic);
    $("#cboreceive").val(model.isReceived);
    if (model.checkIn !== null) {
        $("#txtEndDt").val(model.checkIn.substring(0, 10));
    }
    if (model.checkOut !== null) {

        $("#txtStartDt").val(model.checkOut.substring(0, 10));
    }

    checkvalue();
    $("#modalData").modal("show")

}

$("#btnNewProduct").on("click", function () {
    openModal()
})

$("#btnSave").on("click", function () {

    ///////////////////////////
    let targetTable = document.getElementById('childTable');
    let targetTableRows = targetTable.rows;
    let tableHeaders = targetTableRows[0];
    let dt = '';
    let valWithName = '';
    let ddlQ_P = $("#Q_P").val();
    let ddlK_L = $("#K_L").val();
    let ddlC_C_B = $("#C_C_B").val();
    let val = '';

    // start from the second row as the first one only contains the table's headers
    for (let i = 1; i < targetTableRows.length; i++) {
        // loop over the contents of each row
        for (let j = 0; j < targetTableRows[i].cells.length; j++) {
            // something we could use to identify a given item
            let currColumn = tableHeaders.cells[j].innerHTML;
            // the current <td> element
            let currData = targetTableRows[i].cells[j];
            // the input field in the row
            let currDataInput = currData.querySelector('input');

            // is the current <td> element containing an input field? print its value.
            // Otherwise, print whatever is insside
            if (!currData.innerHTML.includes("+")) {

                if (currDataInput) {
                    if (currDataInput.value === "/" || currDataInput.value === '') {
                        dt = 0;
                    } else {
                        dt = currDataInput.value;
                    }
                }
                else {
                    dt = currData.innerHTML;
                }

                if (j > 3) {

                    if (j === 4) {
                        currColumn = ddlQ_P;
                    }
                    else if (j === 5) {

                        currColumn = ddlK_L;
                    }
                    else if (j === 6) {

                        currColumn = ddlC_C_B;
                    }
                }

                valWithName += currColumn + ":" + dt + "; ";
                val += dt + ";";

            }
            //console.log(`${currColumn}: ${currDataInput.value}`);
        }
        valWithName += "| ";
        val += "|";
    }
    ///////////////////////////

    const inputs = $("input.input-validate").serializeArray();
    const inputs_without_value = inputs.filter((item) => item.value.trim() == "")

    if (inputs_without_value.length > 0) {
        const msg = `You must complete the field : "${inputs_without_value[0].name}"`;
        toastr.warning(msg, "");
        $(`input[name="${inputs_without_value[0].name}"]`).focus();
        return;
    }
    if (valWithName.includes(" No.")) {
        valWithName = valWithName.replace(" Description", " Descr").replace(" Quantity", " Qty").replace(" No.", " Item");
    }

    const model = structuredClone(BASIC_MODEL);
    model["idGatePass"] = $("#txtId").val();
    model["name"] = $("#txtName").val();
    model["companyName"] = $("#txtCompany").val();
    model["contactNo"] = $("#txtContact").val();
    model["nic"] = $("#txtNIC").val();
    model["vechicleType"] = $("#txtVType").val();
    model["vechicleNo"] = $("#txtVTNo").val();
    model["remarks"] = $("#txtRemarks").val();
    model["userID"] = $("#userid").val();
    model["status"] = $("#cboStatus").val();
    model["itemDetail"] = valWithName.replace("No.", "Item").replace("Description", "Descr").replace("Quantity", "Qty");
    model["item"] = val;
    model["isReceived"] = $("#cboreceive").val();
    // model["checkOut"] = $("#txtStartDt").val();
    // model["checkIn"] = $("#txtEndDt").val();
    if ($("#cboStatus").val() === "Returnable") {

        if ($("#txtStartDt").val() === "") {

            swal("Error", "Please Select Start Date", "error");
            return;
        }
        else if ($("#txtEndDt").val() === "") {

            swal("Error", "Please Select End Date", "error");
            return;
        }
        else if ($("#cboreceive").val() === "2") {

            model["isReceived"] = 0;
        }

        model["checkOut"] = $("#txtStartDt").val();
        model["checkIn"] = $("#txtEndDt").val();

    }
    else {
        model["isReceived"] = 2;
        model["checkOut"] = null;
        model["checkIn"] = null;
    }


    // const inputPhoto = document.getElementById('txtPhoto');

    const formData = new FormData();
    //formData.append('photo', inputPhoto.files[0]);
    formData.append('model', JSON.stringify(model));

    $("#modalData").find("div.modal-content").LoadingOverlay("show")


    if (model.idGatePass == 0) {
        fetch("/Setup/CreateGatePass", {
            method: "POST",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {

            if (responseJson.state) {

                tableData.row.add(responseJson.object).draw(false);
                $("#modalData").modal("hide");
                swal("Successful!", "The GatePass was created", "success");

            } else {
                swal("We're sorry", responseJson.message, "error");
            }
        }).catch((error) => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
        })
    } else {

        fetch("/Setup/EditGatePass", {
            method: "PUT",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {
            if (responseJson.state) {

                tableData.row(rowSelected).data(responseJson.object).draw(false);
                rowSelected = null;
                $("#modalData").modal("hide");
                swal("Successful!", "The GatePass was modified", "success");

            } else {
                swal("We're sorry", responseJson.message, "error");
            }
        }).catch((error) => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
        })
    }

})

$("#tbData tbody").on("click", ".btn-edit", function () {
    $("#linkPrint").attr("href", `/Setup/ShowPDFSale?saleNumber=000013`);
    if ($(this).closest('tr').hasClass('child')) {
        rowSelected = $(this).closest('tr').prev();
    } else {
        rowSelected = $(this).closest('tr');
    }

    const data = tableData.row(rowSelected).data();

    openModal(data);
})

$("#tbData tbody").on("click", ".btn-delete", function () {


    let row;

    if ($(this).closest('tr').hasClass('child')) {
        row = $(this).closest('tr').prev();
    } else {
        row = $(this).closest('tr');
    }
    const data = tableData.row(row).data();

    swal({
        title: "Are you sure?",
        text: `Delete the GatePass "${data.idGatePass}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete",
        cancelButtonText: "No, cancel",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (respuesta) {

            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show")

                fetch(`/Setup/DeleteGatePass?idGatePass=${data.idGatePass}`, {
                    method: "DELETE"
                }).then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide")
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(responseJson => {
                    if (responseJson.state) {

                        tableData.row(row).remove().draw();
                        swal("Successful!", "GatePass was deleted", "success");

                    } else {
                        swal("We're sorry", responseJson.message, "error");
                    }
                })
                    .catch((error) => {
                        $(".showSweetAlert").LoadingOverlay("hide")
                    })
            }
        });
})

function checkvalue() {
    v = $("#cboStatus").val();
    var d1 = document.getElementById("divdate1");
    var d2 = document.getElementById("divdate2");
    var d3 = document.getElementById("divdate3");
    if (v === "Returnable") {
        d1.style.display = "block";
        d2.style.display = "block";
        d3.style.display = "block";
    }
    else {

        d1.style.display = "none";
        d2.style.display = "none";
        d3.style.display = "none";
    }
}
function childrenRow() {
    countSno++;
    $('#childTable').find('tbody').append('<tr><th scope="row" style="width:4%;">' + countSno + '</th><td style="width:15%;"><input type="text" name="name" class="form-control" /></td><td style="width:22%;"><input type="text" name="description" class="form-control" /></td><td style="width:22%;"><input type="text" name="qty" class="form-control" /></td><td style="width:10%;"><input type="text" name="kg" class="form-control" /></td><td style="width:10%;"><input type="text" name="lbs" class="form-control" /></td><td style="width:10%;"><input type="text" name="pieces" class="form-control" /></td><td style="width:5%;"><input type="button" class="btn btn-block btn-default" id="addrow" onclick="childrenRow()" value="+" /></td></tr>');

}