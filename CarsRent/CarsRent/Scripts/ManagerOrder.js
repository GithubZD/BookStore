

$(document).delegate(".delete", "click", function () {
    var recordID = $(this).attr("data-id");
    bootbox.dialog({
        size: "small",
        onEscape: "true",
        message: "你确定要删除该订单吗?",
        title: "操作确认",
        buttons: {
            OK: {
                label: " 确定 ",
                className: "btn btn-myStyle",
                callback: function () {
                    $.post("/ManagerOrder/Delete",
                    { recordID: recordID },
                    function (data) {
                        if (data.Status == 1) {
                            //window.location.reload();
                            $("#record_" + recordID).fadeOut();
                        }
                        else {
                            alert("操作发生异常,删除失败！");
                        }
                    });
                }
            },
            Cancel: {
                label: " 取消 ",
                className: "btn btn-default",
                callback: function () {
                }
            }
        }
    });
});

$(document).delegate(".GetCar", "click", function () {
    var orderId = $(this).attr("data-id");
    var Pay = $("li.active").find("a").attr("data-pay");
    var CarStatus = $("li.active").find("a").attr("data-status");
    $.post("/ManagerOrder/UserGetCar",
        { OrderId: orderId },
        function (date) {
            if (date.State == 1) {
                reload(Pay, CarStatus);
            }
            else if (date.State == 0) {
                location.href = "/Home/ErrorMessage";
            }
        })
})
$(document).delegate(".ReturnCar", "click", function () {
    var orderId = $(this).attr("data-id");
    var Pay = $("li.active").find("a").attr("data-pay");
    var CarStatus = $("li.active").find("a").attr("data-status");
    $.post("/ManagerOrder/UserReturnCar",
        { OrderId: orderId },
        function (date) {
            if (date.State == 1) {
                reload(Pay, CarStatus);
            }
            else if (date.State == 0) {
                location.href = "/Home/ErrorMessage";
            }
        })
})
function reload(Pay, CarStatus) {
    $.ajax({
        type: "POST",
        url: "/ManagerOrder/OrderCategory",
        data: { Pay: Pay, CarStatus: CarStatus },
        datatype: "json",
        success: function (date) {
            $("#order_list").html(date);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}