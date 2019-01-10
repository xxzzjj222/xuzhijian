layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = layui.layer;
    var roleName = GetQueryString("roleName");//获取列表页面传来的角色名参数
    var oldNavigateTitle;//修改前选中的多选框角色
    //页面打开加载内容
    $(document).ready(function () {
        //加载用户名input
        $("input[name='roleName']").val(roleName);
        //加载导航多选选项
        $.post('../../index.ashx', { ctrl: "RoleNavigate", action: "GetAllNavigatesForNavigateOptions" }, function (result) {
            if (result.state == "success") {
                var data = result.data;
                var div = $("#navigateOptions")//获取多选外层div元素
                var child;
                div.empty();//清空下拉列表的所有子元素
                for (var i = 0; i < data.length; i++) {//添加子元素
                    child = $("<input type='checkbox' name='navigate' value=" + data[i].id + " title=" + data[i].navigateTitle + " >");
                    div.append(child);
                }
                //获取该用户拥有的角色
                $.post('../../index.ashx', { ctrl: "RoleNavigate", action: "GetCheckedNavigateByRoleName", roleName: roleName }, function (result) {
                    oldNavigateTitle = result.data;
                    for (var i = 0; i < oldNavigateTitle.length; i++) {//设置多选框的选中
                        $("input:checkbox[value=" + oldNavigateTitle[i].navigateId + "]").prop("checked", "true");
                    }
                    layui.form.render(); //重新渲染显示效果
                })
            }
        });

    });

    form.on('submit(formDemo)', function (data) {
        data.field.roleName = roleName;//用户名
        var arr = new Array()
        $("input:checkbox[name=navigate]:checked").each(function (i) {
            arr[i] = $(this).val();
        });//修改后选中的多选框的value
        data.field.newNavigateTitle = arr.join(",");
        var arrOld = new Array();
        for (var i = 0; i < oldNavigateTitle.length; i++) {
            arrOld[i] = oldNavigateTitle[i].navigateId;//修改前的多选框的value
        }
        data.field.oldNavigateTitle = arrOld.join(",");
        $.post('../../index.ashx', { ctrl: "RoleNavigate", action: "EditRoleNavigate", roleName: data.field.roleName, newNavigateTitle: data.field.newNavigateTitle, oldNavigateTitle: data.field.oldNavigateTitle }, function (result) {
            if (result.url != null) {
                window.location.href = result.url;//未登录跳转至登录页
            }
            else if (result.state = "success") {
                layer.open({//修改成功
                    content: result.message,
                    btn: '确定',
                    shadeClose: false,
                    yes: function () {
                        var contentIframe = window.parent.parent.document.getElementById("changeIframe");
                        contentIframe.src = "RoleNavigateManage.html";//设置iframeURL
                        var index = parent.layer.getFrameIndex(window.name);
                        parent.layer.close(index); //关闭新增弹框
                    }
                });
            }
            else if (result.state == "fail") {
                layer.open({//修改失败
                    content: result.message,
                    btn: '确定',
                    shadeClose: false
                });
            }
        });
        return false;
    });
    $("#cancelBtn").click(function () {
        var index = parent.layer.getFrameIndex(window.name);
        parent.layer.close(index); //关闭修改弹框
    });

});