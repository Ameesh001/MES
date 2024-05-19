let tableData;
let rowSelected;

const BASIC_MODEL = {
    idcheque: 0,
    idcheque: 0,
    payeeName: "",
    amount: "",
    amountInWords: "",
    chequeNo: "",
    userID: 0,
    bank: "1",
    states: "",
    depositDate: null,
    nameTop: "",
    nameLeft: "",
    accTop: "",
    accLeft: "",
    CheqNoTop: "",
    CheqNoLeft: "",
    DateTop: "",
    DateLeft: "",
    wordsLeft: "",
    chequeType: "",
    wordsTop: ""
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////
let lblPay = document.getElementById('lblPay');
let lblAmount = document.getElementById('lblAmount');
let lbldate = document.getElementById('lbldate');
let lblInWords = document.getElementById('lblInWords');

dragElement(lblPay);
dragElement(lblAmount);
dragElement(lbldate);
dragElement(lblInWords);

function dragElement(elmnt) {
    let pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
    if (document.getElementById(elmnt.id + "header")) {
        /* if present, the header is where you move the DIV from:*/
        document.getElementById(elmnt.id + "header").onmousedown = dragMouseDown;
    } else {
        /* otherwise, move the DIV from anywhere inside the DIV:*/
        elmnt.onmousedown = dragMouseDown;
    }

    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        // get the mouse cursor position at startup:
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        // call a function whenever the cursor moves:
        document.onmousemove = elementDrag;
    }

    function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        // calculate the new cursor position:
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        // set the element's new position:
        elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
        elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
        // document.getElementById('exampleModalGridTitle').innerHTML = "Top: " + elmnt.style.top + "; Left: " + elmnt.style.left;

    }

    function closeDragElement() {
        /* stop moving when mouse button is released:*/
        document.onmouseup = null;
        document.onmousemove = null;
    }


}
$(document).ready(function () {


    //////////////// Default Date Setting ////////////////
    const date = new Date();

    //let day = date.getDate();
    let month = date.getMonth() + 1;
    //let year = date.getFullYear();

    //let currentDate = `${day}/${month}/${year}`;

    if (month > 6) {
        console.log('Your Trial Has Expired!');
        document.getElementById("btnNewProduct").style.display = "none";
    }
    else {
        console.log('your free version is active');

        fetch("/Cheque/GetBanks")
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data.length > 0) {
                    responseJson.data.forEach((item) => {
                        $("#cbobank").append(
                            $("<option>").val(item.idCategory).text(item.description)
                        )
                    });

                }
            });
    }

    tableData = $("#tbData").DataTable({
        responsive: true,
        "ajax": {
            "url": "/Cheque/GetCheque",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "idcheque"
                , visible: false
            },
            {
                data: 'sysdate',
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('DD/MM/YY,h:mma');
                }
            },
            //  { "data": "idcheque" },
            { "data": "payeeName" },
            { "data": "amount" },
            //{ "data": "amountInWords" },

            { "data": "chequeNo" },

            {
                "data": "depositDate",
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('DD/MM/YYYY');
                }
            },
            { "data": "cheqNoLeft" },
            {
                "data": "states", render: function (data) {
                    if (data == 0)
                        return '<span class="badge badge-success">Issued</span>';
                    else if (data == 1)
                        return '<span class="badge badge-info">Pending</span>';
                    else if (data == 2)
                        return '<span class="badge badge-danger">Cancelled</span>';
                    else
                        return '<span class="badge badge-warning">Edited</span>';
                }
            },
            {
                "data": "idcheque", render: function (data) {
                    return `<div style="white-space: nowrap;" id="button-contianer"><a  href="/Cheque/ShowPDFCheque?chequeID=${data}" target="_blank" class="btn btn-info btn-sm mr-1" />` +
                        `<i class="mdi mdi-eye"></i></a>` +
                        '<button class="btn btn-primary btn-edit btn-sm mr-1"><i class="mdi mdi-pencil"></i></button>' +
                        '<button class="btn btn-danger btn-delete btn-sm "><i class="mdi mdi-trash-can"></i></button></div>';
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
                filename: 'ChequeReport',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }, 'pageLength'
        ]
    });
})

let countSno = 0;
const openModal = (model = BASIC_MODEL) => {

    $("#txtId").val(model.idcheque);

    if (model.idcheque > 0) {
        $("#txtPName").val(model.payeeName);
        $("#txtAmount").val(model.amount.replace(',', '').replace(',', ''));
        $("#txtInWords").val(model.amountInWords);
        $("#txtChequeNo").val(model.chequeNo);
        $("#cboState").val(model.states);
        $("#cbobank").val(model.bank);
        $("#chequeType").val(model.chequeType);
        if (model.depositDate !== null) {
            $("#txtDepositDt").val(model.depositDate.substring(0, 10));
        }
        // setlblmargin(model.nameLeft, model.nameTop, model.dateLeft, model.dateTop, model.accLeft, model.accTop, model.wordsTop, model.wordsLeft);

    }
    OnBankChange();
    $("#modalData").modal("show");

    //lblPay.style.left = model.nameLeft;
    //lblPay.style.top = model.nameTop;
    //lbldate.style.left = model.dateLeft;
    //lbldate.style.top = model.dateTop;
    //lblAmount.style.left = model.accLeft;
    //lblAmount.style.top = model.accTop;
    //lblInWords.style.top = model.wordsTop;
    //lblInWords.style.left = model.wordsLeft;

}

$("#btnNewProduct").on("click", function () {
    openModal()
})

$("#btnSave").on("click", function () {

    //let lblPay = document.getElementById('lblPay');
    //let lblAmount = document.getElementById('lblAmount');
    //let lbldate = document.getElementById('lbldate');
    //let lblInWords = document.getElementById('lblInWords');

    const inputs = $("input.input-validate").serializeArray();
    const inputs_without_value = inputs.filter((item) => item.value.trim() == "")

    if (inputs_without_value.length > 0) {
        const msg = `You must complete the field : "${inputs_without_value[0].name}"`;
        toastr.warning(msg, "");
        $(`input[name="${inputs_without_value[0].name}"]`).focus();
        return;
    }

    //<Cheque BackEnd Alteration>
    let vdateleft = parseInt(lbldate.style.left.replace('px', ''));
    let vdatetop = parseInt(lbldate.style.top.replace('px', ''));
    let vAmounttop = parseInt(lblAmount.style.top.replace('px', ''));
    let vAmountleft = parseInt(lblAmount.style.left.replace('px', ''));
    let vPayetop = parseInt(lblPay.style.top.replace('px', ''));
    let vPayeleft = parseInt(lblPay.style.left.replace('px', ''));
    let vWordstop = parseInt(lblInWords.style.top.replace('px', ''));
    let vWordsleft = parseInt(lblInWords.style.left.replace('px', ''));

    const fdateLeft = vdateleft + 40 - dateLeftB;
    const fdateTop = vdatetop - 9 - dateTopB;
    const faccTop = vAmounttop - 7 - accTopB;
    const faccLeft = vAmountleft + 50 - accLeftB;
    const fPayetop = vPayetop + 0 - nameTopB;
    const fPayeleft = vPayeleft - 0 - nameLeftB;
    const fWordstop = vWordstop - 0 - wordsTopB;
    const fWordsleft = vWordsleft + 0 - wordsLeftB;
    //</Cheque BackEnd Alteration>

    const model = structuredClone(BASIC_MODEL);
    model["idcheque"] = $("#txtId").val();
    model["payeeName"] = $("#txtPName").val();
    model["amount"] = parseInt($("#txtAmount").val()).toLocaleString()
    model["amountInWords"] = $("#txtInWords").val();
    model["chequeNo"] = $("#txtChequeNo").val();
    model["states"] = $("#cboState").val();
    model["bank"] = $("#cbobank").val();
    model["CheqNoLeft"] = $("#cbobank option:selected").text()
    model["depositDate"] = $("#txtDepositDt").val();
    model["userID"] = $("#userid").val();
    model["chequeType"] = $("#chequeType").val();

    model["nameTop"] = fPayetop + 'px'
    model["nameLeft"] = fPayeleft + 'px'
    model["accTop"] = faccTop + 'px'
    model["accLeft"] = faccLeft + 'px'
    model["dateTop"] = fdateTop + 'px'
    model["dateLeft"] = fdateLeft + 'px'
    model["wordsTop"] = fWordstop + 'px'
    model["wordsLeft"] = fWordsleft + 'px'
    model["CheqNoTop"] = parts;


    // const inputPhoto = document.getElementById('txtPhoto');

    const formData = new FormData();
    //formData.append('photo', inputPhoto.files[0]);
    formData.append('model', JSON.stringify(model));

    $("#modalData").find("div.modal-content").LoadingOverlay("show")


    if (model.idcheque == 0) {
        fetch("/Cheque/CreateCheque", {
            method: "POST",
            body: formData
        }).then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {

            if (responseJson.state) {

                tableData.row.add(responseJson.object).draw(false);
                $("#modalData").modal("hide");
                swal("Successful!", "The Cheque was created", "success");

            } else {
                swal("We're sorry", responseJson.message, "error");
            }
        }).catch((error) => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
        })
    } else {

        fetch("/Cheque/EditCheque", {
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
                swal("Successful!", "The Cheque was modified", "success");

            } else {
                swal("We're sorry", responseJson.message, "error");
            }
        }).catch((error) => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide")
        })
    }

})

$("#tbData tbody").on("click", ".btn-edit", function () {
    // $("#linkPrint").attr("href", `/Cheque/ShowPDFSale?saleNumber=000013`);
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
        text: `Delete the Cheque "${data.idcheque}"`,
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

                fetch(`/Cheque/DeleteCheque?idcheque=${data.idcheque}`, {
                    method: "DELETE"
                }).then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide")
                    return response.ok ? response.json() : Promise.reject(response);
                }).then(responseJson => {
                    if (responseJson.state) {

                        tableData.row(row).remove().draw();
                        swal("Successful!", "Cheque was deleted", "success");

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
let v_img = document.getElementById("imgCheque");
let v_alllbl = document.getElementById("alllbl");


function chequeImgSet() {

    const val = $("#cbobank option:selected").text();
    v_img.style.display = "block";
    $("#imgCheque").attr("src", `/img/` + val + '.jpg');
    v_alllbl.style.display = "block";
    setlblvalue();

}

let wordsTopB = 0;
let wordsLeftB = 0;
let dateLeftB = 0;
let dateTopB = 0;
let accLeftB = 0;
let accTopB = 0;
let nameLeftB = 0;
let nameTopB = 0;
function OnBankChange() {

    chequeImgSet();

    let id = $("#cbobank").val();

    fetch(`/Cheque/GetCoordinates?idBank=${id}`, {
        method: "GET"
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        wordsTopB = responseJson.data.wordsTopB;
        wordsLeftB = responseJson.data.wordsLeftB;
        dateLeftB = responseJson.data.dateLeftB;
        dateTopB = responseJson.data.dateTopB;
        accLeftB = responseJson.data.accLeftB;
        accTopB = responseJson.data.accTopB;
        nameLeftB = responseJson.data.nameLeftB;
        nameTopB = responseJson.data.nameTopB;

        setlblmargin(responseJson.data.nameLeft, responseJson.data.nameTop, responseJson.data.dateLeft, responseJson.data.dateTop, responseJson.data.accLeft, responseJson.data.accTop, responseJson.data.wordsTop, responseJson.data.wordsLeft);

    });
}

parts = '';
function setlblvalue() {
    lblPay.innerHTML = $("#txtPName").val();
    let formattedNumber = parseInt($("#txtAmount").val()).toLocaleString();
    lblAmount.innerHTML = formattedNumber;
    //lblAmount.innerHTML = $("#txtAmount").val();
    lblInWords.innerHTML = $("#txtInWords").val();
    //2024-02-15

    parts = $("#txtDepositDt").val();
    if (parts != '') {

        const bankVal = $("#cbobank option:selected").text().toUpperCase();
        if (bankVal.includes("METRO")) {
            lbldate.innerHTML = parts[8] + '&nbsp;&nbsp;&nbsp;' + parts[9] + '&nbsp;&nbsp;&nbsp;&nbsp;' + parts[5] + '&nbsp;&nbsp;&nbsp;' + parts[6] + '&nbsp;&nbsp;&nbsp;&nbsp;' + parts[0] + '&nbsp;&nbsp;&nbsp;' + parts[1] + '&nbsp;&nbsp;&nbsp;' + parts[2] + '&nbsp;&nbsp;&nbsp;' + parts[3];
        } else {
            lbldate.innerHTML = parts[8] + '&nbsp;&nbsp;&nbsp;' + parts[9] + '&nbsp;&nbsp;&nbsp;&nbsp;' + parts[5] + '&nbsp;&nbsp;&nbsp;&nbsp;' + parts[6] + '&nbsp;&nbsp;&nbsp;&nbsp;' + parts[0] + '&nbsp;&nbsp;&nbsp;&nbsp;' + parts[1] + '&nbsp;&nbsp;&nbsp;' + parts[2] + '&nbsp;&nbsp;&nbsp;' + parts[3];

        }
    }
}


function setlblmargin(nameLeft, nameTop, dateLeft, dateTop, accLeft, accTop, wordsTop, wordsLeft) {


    dateLeft = parseInt(dateLeft.replace('px', ''));
    dateTop = parseInt(dateTop.replace('px', ''));
    accTop = parseInt(accTop.replace('px', ''));
    accLeft = parseInt(accLeft.replace('px', ''));
    nameTop = parseInt(nameTop.replace('px', ''));
    nameLeft = parseInt(nameLeft.replace('px', ''));
    wordsTop = parseInt(wordsTop.replace('px', ''));
    wordsLeft = parseInt(wordsLeft.replace('px', ''));

    dateLeft = dateLeft - 40 + dateLeftB; //40
    dateTop = dateTop + 9 + dateTopB; //9
    accTop = accTop + 7 + accTopB; // 7
    accLeft = accLeft - 50 + accLeftB; // 50
    nameTop = nameTop - 0 + nameTopB;
    nameLeft = nameLeft - 0 + nameLeftB;
    wordsTop = wordsTop - 0 + wordsTopB;
    wordsLeft = wordsLeft - 0 + wordsLeftB;

    lblPay.style.left = nameLeft + 'px';
    lblPay.style.top = nameTop + 'px';
    lbldate.style.left = dateLeft + 'px';
    lbldate.style.top = dateTop + 'px';
    lblAmount.style.left = accLeft + 'px';
    lblAmount.style.top = accTop + 'px';
    lblInWords.style.top = wordsTop + 'px';
    lblInWords.style.left = wordsLeft + 'px';

}


//function Imagebool() {

//    const selectedVal = $("#cbobank option:selected").text();
//    if (selectedVal == '') {

//        return false;
//    } else {

//        return true;
//    }
//}

//let a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
//let b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

//function inWords(num) {
//    if ((num = num.toString()).length > 9) return '';
//    n = ('000000000' + num).slice(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
//    if (!n) return; let str = '';
//    str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore ' : '';
//    str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh ' : '';
//    str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
//    str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
//    str += (n[5] != 0) ? ((str != '') ? 'And ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'Only ' : '';
//    return str;
//}

function AmountinWords() {
    inWords(document.getElementById('txtAmount').value);
    setlblvalue();
};

let single_digit = ['', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine']
let double_digit = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen']
let below_hundred = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety']

function inWords(n) {
    if (n < 0)
        return false;

    if (n === 0) return 'Zero'
    function translate(n) {
        word = ""
        if (n < 10) {
            word = single_digit[n] + ' '
        }
        else if (n < 20) {
            word = double_digit[n - 10] + ' '
        }
        else if (n < 100) {
            rem = translate(n % 10)
            word = below_hundred[(n - n % 10) / 10 - 2] + ' ' + rem
        }
        else if (n < 1000) {
            word = single_digit[Math.trunc(n / 100)] + ' Hundred ' + translate(n % 100)
        }
        else if (n < 1000000) {
            word = translate(parseInt(n / 1000)).trim() + ' Thousand ' + translate(n % 1000)
        }
        else if (n < 1000000000) {
            word = translate(parseInt(n / 1000000)).trim() + ' Million ' + translate(n % 1000000)
        }
        else {
            word = translate(parseInt(n / 1000000000)).trim() + ' Billion ' + translate(n % 1000000000)
        }
        return word
    }
    result = translate(n)
    document.getElementById('txtInWords').value = result.trim() + ' Only /-';
};


