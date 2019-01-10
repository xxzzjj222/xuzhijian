
layui.use(['form', 'layer', 'table'], function () {
    var form = layui.form,
        table = layui.table,
        layer = layui.layer;

    document.getElementsByName("userName")[0].value = GetQueryString("userName");
    document.getElementsByName("password")[0].value = GetQueryString("password");

    //监听提交
    form.on('submit(formDemo)', function (data) {
        $.post('../../index.ashx', { ctrl: "UserInfo", action: "EditUserInfo", userName: data.field.userName, password: data.field.password }, function (result) {
            if (result.url != null) {
                window.location.href = result.url;//未登录跳转至登录页
            }
            else if (result.state == "fail") {
                layer.open({//修改失败
                    content: result.message,
                    btn: '确定',
                    shadeClose: false
                });
            }
            else if (result.state == "success") {//修改成功
                layer.open({
                    content: result.message,
                    btn: '确定',
                    shadeClose: false,
                    yes: function () {
                        var contentIframe = window.parent.parent.document.getElementById("changeIframe");
                        contentIframe.src = "UserInfoManage.html";//设置iframeURL
                        var index = parent.layer.getFrameIndex(window.name);
                        parent.layer.close(index); //关闭修改弹框
                    }
                });
            }
        })
        return false;
    });
    $("#cancelBtn").click(function () {
        var index = parent.layer.getFrameIndex(window.name);
        parent.layer.close(index); //关闭修改弹框
    });
})
