layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = layui.layer;

    //监听提交
    form.on('submit(formDemo)', function (data) {
        var select = $("select[name='userName']",parent.parent.document).children('option:selected').text();
        $.post('../../index.ashx', { ctrl: "UserInfo", action: "AddUserInfo", userName: data.field.userName, password: data.field.password,role:select }, function (result) {
            if (result.url != null) {
                window.location.href = result.url;//未登录跳转至登录页
            }
            else if (result.state == "fail") {
                layer.open({//新增失败
                    content: result.message,
                    btn: '确定',
                    shadeClose: false
                });    
            }
            else if (result.state == "success")
            {//新增成功
                layer.open({
                    content: result.message,
                    btn: '确定',
                    shadeClose: false,
                    yes: function () {
                        var contentIframe = window.parent.parent.document.getElementById("changeIframe");
                        contentIframe.src = "UserInfoManage.html";//设置iframeURL
                        var index = parent.layer.getFrameIndex(window.name);
                        parent.layer.close(index); //关闭新增弹框
                    }
                });
            }
        })
        return false;
    });
});