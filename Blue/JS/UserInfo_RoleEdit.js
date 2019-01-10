layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = layui.layer;
    var userName = GetQueryString("userName");//获取列表页面传来的用户名参数
    var oldRoleName;//修改前选中的多选框角色
    //页面打开加载内容
    $(document).ready(function () {
        //加载用户名input
        $("input[name='userName']").val(userName);
        //加载角色名多选选项
        $.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "GetAllRolesForRoleOptions" }, function (result) {
            if (result.state == "success") {
                var data = result.data;
                var div = $("#roleOptions")//获取多选外层div元素
                //var div = document.getElementById("roleOptions");
                var child;
                div.empty();//清空下拉列表的所有子元素
                for (var i = 0; i < data.length; i++) {//添加子元素
                    child = $("<input type='checkbox' name='role' value=" + data[i].id + " title=" + data[i].name + " >");
                    div.append(child);
                }     
                //获取该用户拥有的角色
                $.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "GetCheckedRoleByUserInfo", userName: userName }, function (result) {
                    oldRoleName = result.data;
                    for (var i = 0; i < oldRoleName.length; i++) {//设置多选框的选中
                        $("input:checkbox[value=" + oldRoleName[i].roleId + "]").prop("checked", "true");
                    }
                    layui.form.render(); //重新渲染显示效果
                })  
            }
        });
        
    });

    form.on('submit(formDemo)', function (data) {
        data.field.userName = userName;//用户名
        var arr = new Array()
        $("input:checkbox[name=role]:checked").each(function (i) {
            arr[i] = $(this).val();
        });//修改后选中的多选框的value
        data.field.newRoleName = arr.join(",");
        var arrOld = new Array();
        for (var i = 0; i < oldRoleName.length; i++)
        {
            arrOld[i] = oldRoleName[i].roleId;//修改前的多选框的value
        }
        data.field.oldRoleName = arrOld.join(",");
        $.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "EditUserInfo_Role", userName: data.field.userName, newRoleName: data.field.newRoleName, oldRoleName: data.field.oldRoleName }, function (result) {
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
                        contentIframe.src = "UserInfo_RoleManage.html";//设置iframeURL
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