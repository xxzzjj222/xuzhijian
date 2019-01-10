﻿layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = layui.layer;

    //页面打开加载内容
    $(document).ready(function () {
        //加载用户名下拉列表选项
        $.post('../../index.ashx', { ctrl: "RoleNavigate", action: "GetAllRolesForselect" }, function (result) {
            if (result.state == "success") {
                var data = result.data;
                var select = $("select[name='roleName']")//获取下拉列表元素
                var child;
                select.empty();//清空下拉列表的所有子元素
                for (var i = 0; i < data.length; i++) {//添加子元素
                    child = $("<option value=" + data[i].id + ">");
                    child.text(data[i].roleName);
                    select.append(child);
                }
                form.render('select');//重新渲染下拉列表
            }
        });
        //加载角色名多选选项
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
                form.render('checkbox');//重新渲染下拉列表
            }
        });
    });

    form.on('submit(formDemo)', function (data) {
        data.field.roleName = $("select[name='roleName'] option:selected").val();
        var arr = new Array()
        $("input:checkbox[name=navigate]:checked").each(function (i) {
            arr[i] = $(this).val();
        });
        data.field.navigateTitle = arr.join(",");
        $.post('../../index.ashx', { ctrl: "RoleNavigate", action: "AddRoleNavigate", roleName: data.field.roleName, navigateTitle: data.field.navigateTitle }, function (result) {
            if (result.url != null) {
                window.location.href = result.url;//未登录跳转至登录页
            }
            else if (result.state = "success") {
                layer.open({//新增成功
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
                layer.open({//新增失败
                    content: result.message,
                    btn: '确定',
                    shadeClose: false
                });
            }
        });
        return false;
    })

});