﻿

$(document).delegate("#payOrder", "click", function () {
    var orderID = $(this).attr("data-id");
    bootbox.dialog({
        size: "small",
        message: "你确定要付款?",
        title: "操作确认",
        buttons: {
            OK: {
                label: '确认',
                className: 'btn btn-myStyle',
                callback: function () {
                    $.post("/RentOrders/PayOrder",
                    { orderID: orderID },
                    function (data) {
                    if (data.Status == 1) {
                        window.location.reload();
                        alert("付款成功");
                    }
                    else if (data.Status == 2) {
                        alert("Sorry!你选择的车辆数量不够，请重新选择其他车辆！");
                    }
                    else {
                        location.href = "/Home/ErrorMessage";
                    }
                    });
                }
            },
            Canael: {
                label: '取消',
                className: 'btn btn-default',
                callback: function () {
                }
            },
        },

    });
})

$(document).on("click", ".delete", function () {
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
                        $.post("/RentOrders/Delete",
                        { recordID: recordID },
                        function (data) {
                            if (data.Status == 1) {
                                window.location.reload();
                                alert("删除成功！");
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
 })

$(document).on("click", ".cancel", function () {
    var orderID = $(this).attr("data-id");
    bootbox.dialog({
        size: "small",
        message: "你确定要取消该订单吗?",
        title: "操作确认",
        buttons: {
            OK: {
                label: '确认',
                className: 'btn btn-myStyle',
                callback: function () {
                    $.post("/RentOrders/CancelOrder",
                    { recordID: orderID },
                    function (result) {
                        if (result.Status == 1) {
                            window.location.reload();
                            alert('你已经取消订单');
                        }
                        else {
                            alert('数据异常，请重新操作！');
                        }
                    });
                }
            },
            Canael: {
                label: '取消',
                className: 'btn btn-default',
                callback: function () {
                }
            },
        },

    });
})

$(document).on("click", "#deleteAll", function () {
    var ids = "";
    $("input:checked[name='selectFlag']").each(function () {
        ids = ids + $(this).val() + ",";
    });
    if (ids.length > 0) {
        if (confirm("确定删除吗？")) {

            var url = "/RentOrders/DeleteAll?ids=" + ids;

            $.getJSON(url, function (data) {
                if (data) {
                    window.location.reload();
                    alert("删除成功！");
                }
                else {
                    alert("操作发生异常,删除失败！");
                }
            });
        }
    } else {

        alert("请选择数据！");
    }

})

$(document).on("click", "#checkAll", function () {
    //debugger;
    if (this.checked) {
        //$("input:checkbox").css("background-color","red");
        var ischecked = this.checked;
        $("input:checkbox[name='selectFlag']").each(function () {
            this.checked = ischecked;

        });
    }
    else if (!this.checked) {
        $("input[name='selectFlag']:checkbox").each(function () { //遍历所有的name为selectFlag的 checkbox  
            $(this).attr("checked", false);
        });
    }
})