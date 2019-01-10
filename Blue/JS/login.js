
layui.use(['layer', 'form', 'element'], function () {
    var form = layui.form,
        element=layui.element,
        layer=layui.layer;

    form.on('submit(formDemo)', function (data) {
        var index = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
        $.post('../../index.ashx', { ctrl: "UserInfo", action: "GetLoginStateByUserName", UserName: data.field.userName, Password: data.field.password }, function (result) {
            if (result.url != null) {//未登录验证，返回login页
                window.location.href = result.url;
            }
            else if (result.state == "fail") {
                layer.closeAll('loading');//关闭加载图标
                layer.msg(result.message);//弹框提示登录失败信息
            }
            else if (result.state == "success")
            {//登录成功，跳转至默认页面
                setCookie("userName", data.field.userName);
                window.location.href = result.message;//跳转至主页面
            }
        });       
        return false;
    });
});