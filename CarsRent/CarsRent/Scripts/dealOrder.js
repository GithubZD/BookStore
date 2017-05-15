

$(document).delegate(".payOrder", "click", function () {
    var orderID = $(this).attr("data-id");
    var Pay = $("li.active").find("a").attr("data-pay");
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
                        alert("付款成功");
                        IsPay(Pay);
                    }
                    else{
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
                        $.post("/RentOrders/Delete",
                        { recordID: recordID },
                        function (data) {
                            if (data.Status == 1) {
                                //window.location.reload();
                                $(".record_" + recordID).fadeOut();
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

$(document).ready(function () {
    $("#allOrder").click(function () {
        $.ajax({
            type: "POST",
            url: "/RentOrders/AllOrder",
            datatype: "json",
            success: function (date) {
                $("#orderList").html(date);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        })
    })
    $(".checkByPay").click(function () {
        var PayYesNo = $(this).attr("data-pay");
        $.ajax({
            type: "POST",
            url: "/RentOrders/checkOrderByPay",
            data: { Pay: PayYesNo },
            datatype: "json",
            success: function (date) {
                $("#orderList").html(date);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        })
    })
    $(".checkByStatus").click(function () {
        var carStatus = $(this).attr("date-status");
        $.ajax({
            type: "POST",
            url: "/RentOrders/checkOrderByStatus",
            data: { CarStatus: carStatus },
            datatype: "json",
            success: function (date) {
                $("#orderList").html(date);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        })
    })
    $(".checkByEvaluate").click(function () {
        var noEvaluate = $(this).attr("date-evaluate");
        $.ajax({
            type: "POST",
            url: "/RentOrders/checkOrderByStatus",
            data: { noEvaluate: noEvaluate },
            datatype: "json",
            success: function (date) {
                $("#orderList").html(date);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        })
    })
})

function IsPay(Pay) {
    $.ajax({
        type: "POST",
        url: "/RentOrders/checkOrderByPay",
        data: { Pay: Pay },
        datatype: "json",
        success: function (date) {
            $("#orderList").html(date);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}