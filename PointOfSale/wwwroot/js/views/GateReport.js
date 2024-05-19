let tableData;

$(document).ready(function () {

    $("#txtStartDate").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#txtEndDate").datepicker({ dateFormat: 'dd/mm/yy' });

   

    tableData = $('#tbdata').DataTable({
        bAutoWidth: false, 
        "processing": true,
        "ajax": {
            "url": "/Reports/GatePassReport?startDate=01/01/1991&endDate=01/01/1991",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                data: 'dateGP',
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('DD/MM/YYYY');
                }
            },
            {
                data: 'dateGP',
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('h:mm:ss a');
                }
            },
            { "data": "idGatePass" },
            { "data": "name" },
            { "data": "companyName" },
            { "data": "contactNo" },
            { "data": "nic" },
            { "data": "vechicleType" },
            { "data": "vechicleNo" },
            { "data": "status" },
            { "data": "remarks" },
            { "data": "item" },
            { "data": "itemDetail"},
            {
                data: 'checkOut',
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('DD/MM/YYYY');
                }
            },
            {
                data: 'checkIn',
                render: function (data, type, row) {
                    return moment(new Date(data).toString()).format('DD/MM/YYYY');
                }
            },
            
        ],
        order: [[1, "desc"]],
        "scrollX": true,
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Export Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'GatePass Report',
            }, 'pageLength'
        ]
    });

    /*-------------Start Ameesh Work---------------*/
    //////////////// Default Date Setting ////////////////
    const date = new Date();

    let day = date.getDate();
    let month = date.getMonth() + 1;
    let year = date.getFullYear();
    if (`${month}`.length == 1) {
        month = '0' + `${month}`

    }
    if (`${day}`.length == 1) {
        day = '0' + `${day}`

    }
    let currentDate = `${day}/${month}/${year}`;

    document.getElementById("txtStartDate").value = currentDate;
    document.getElementById("txtEndDate").value = currentDate;


    var new_url = `/Reports/GatePassReport?startDate=${$("#txtStartDate").val().trim()}&endDate=${$("#txtEndDate").val().trim()}`

    tableData.ajax.url(new_url).load();

    //////////////// Default Date Setting ////////////////

    /*-----------End Ameesh Work-----------*/

})



$("#btnSearch").click(function () {

    if ($("#txtStartDate").val().trim() == "" || $("#txtEndDate").val().trim() == "") {
        toastr.warning("", "You must enter start and end date");
        return;
    }

    var new_url = `/Reports/GatePassReport?startDate=${$("#txtStartDate").val().trim()}&endDate=${$("#txtEndDate").val().trim()}`

    tableData.ajax.url(new_url).load();
})
