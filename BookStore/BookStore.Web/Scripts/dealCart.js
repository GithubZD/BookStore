

$(document).ready(function () {
    $(".count").bind("update", function () {
        var ids = "";
        $("input:checked[name='selectFlag']").each(function () {
            ids = ids + $(this).val() + ",";
        });
        var recordID = $(this).attr("data-id");
        var count = $(this).val();
        $.post("/ShopCart/UpdateCount",
            { recordID: recordID, count: count, ids: ids },
            function (data) {
                $("#Total_count").text(data.TotalCount);
                $("#TotilPrice").text(data.TotalPrice);
            });
    });
    $("#btnDelete").click(function () {
        var ids = "";
        $("input:checked[name='selectFlag']").each(function () {
            ids = ids + $(this).val() + ",";
        });
        var recordID = $("#delete_id").val();
        $.post("/ShopCart/Delete", { recordID: recordID, ids: ids },
                function (data) {
                    if (data.Status == 1) {
                        $("#myModal").modal("hide")
                        $("#record_" + recordID).fadeOut();
                        $("#Total_count").text(data.TotalCount);
                        $("#TotilPrice").text(data.TotalPrice);
                    }
                });

    });
    $(".delete").click(function () {
        $("#myModal").modal();
        $("#delete_id").val($(this).attr("data-id"));
    });
})
// 提交到后台操作删除
//删除购物车中全部货物
function OnDelete() {
    var ids = "";
    $("input:checked[name='selectFlag']").each(function () {
        ids = ids + $(this).val() + ",";
    });
    if (ids.length > 0) {
        if (confirm("确定删除吗？")) {

            var url = "/ShopCart/DeleteAll?ids=" + ids;

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
}
// 标题行的checkbox, name为checkAll,数据行的的checkbox,name为selectFlag
$(function () {
//选中全部
$("#checkAll").click(function () {
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

    var ids = "";
    $("input:checked[name='selectFlag']").each(function () {
        ids = ids + $(this).val() + ",";
    });
    if (ids.length > 0) {
        $.post("/ShopCart/UpdateTotalPrice",
        { ids: ids },
        function (data) {
            $("#TotilPrice").text(data.TotalPrice);
        })
    }
    else {
        $("#TotilPrice").text(0.00);
    }

                
})

$(".check-box").click(function () {

    var ids = "";
    $("input:checked[name='selectFlag']").each(function () {
        ids = ids + $(this).val() + ",";
    });
    if (ids.length > 0) {
        $.post("/ShopCart/UpdateTotalPrice",
        { ids: ids },
        function (data) {
            $("#TotilPrice").text(data.TotalPrice);
        })
    }
    else {
        $("#TotilPrice").text(0.00);
    }


})
});


//添加订单
$(function () {
    $("#addOrder").click(function () {
        var ids = "";
        $("input:checked[name='selectFlag']").each(function () {
            ids = ids + $(this).val() + ",";
        });
        if (ids.length > 0) {
            var url = "/Order/CreateOrder?ids=" + ids;
            location.href = url;
        }
        else {

            alert("请选择数据！");
        }

    });
})