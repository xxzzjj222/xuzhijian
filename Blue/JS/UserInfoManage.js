//工作流点击调用函数
function workflowClick(role, state, workflowAction, entityID) {
    $.post('../../index.ashx', { ctrl: "UserInfo", action: "WorkflowTrigger", role: role, state: state, workflowAction: workflowAction, entityID: entityID }, function (result) {
        var contentIframe = window.parent.document.getElementById("changeIframe");
        contentIframe.src = "UserInfoManage.html";//设置iframeURL
    })
}
layui.use(['table', 'layer','upload'], function () {
    var table = layui.table,
        upload = layui.upload,
        layer = layui.layer;

    var uploadInst = upload.render({
        elem: '#upload' //绑定元素
        , url: '../../index.ashx?action=UploadImage' //上传接口
        , done: function (res) {
            //上传完毕回调
            if (res.code == 0) {
                layer.msg(res.message);
            }
            else if (res.code = 1)
                layer.msg(res.message);
        }
        , error: function () {
            //请求异常回调
        },
        acceptMime:'image/'
    });
    var select = $("select[name='userName']", parent.document).children('option:selected').val();
    var selectName = $("select[name='userName']", parent.document).children('option:selected').text();

    table.render(
        {//--------渲染表格
            elem: "#userInfoTable",
            url: '../../index.ashx?ctrl=UserInfo&action=GetAllUserInfo&role=' + select,//请求数据的url
            page: true,//分页
            limit: 10,//每页条数
            limits: [5, 10, 20],
            parseData: function (result) { //result 即为原始返回的数据
                if (result.url != null) {
                    window.location.href = result.url;//未登录跳转至登录页
                }
            },
            cols: [[//绑定数据
                { fixed: 'left', type: 'checkbox' },
                { title: '序号', fixed: 'left', type: 'numbers' },
                { field: 'id', title: 'ID', sort: true, fixed: 'left' },
                { field: 'userName', title: '用户名' },
                { field: 'password', title: '密码' },
                {
                    field: 'state', title: '状态', templet: function (d) {
                        var arrAction = d.action.split(',');
                        var str = "";
                        for (var i = 0; i < arrAction.length; i++)
                        {
                            str = str + '<a onclick=workflowClick("' + selectName + '","' + d.state + '","' + arrAction[i] + '",' + d.id + ') class="layui-table-link margin-left-right-ten">' + arrAction[i] + '</a>';
                        }
                        return d.state + str;
                    }
                },
                { toolbar: '#toolbar-tr', title: '操作', fixed: 'right' }
            ]],
            id: 'id',//设置table的id
        });
    
    $("#addUserInfo").click(function () {//-------新增按钮
        layer.open({//弹框一个iframe框用于新增
            type: 2,
            title: '新增用户信息',
            shadeClose: true,
            shade: 0,
            area: ['60%', '30%'],
            content: '../View/UserInfoAdd.html' //iframe的url
        });
    })
    $("#deleteUserInfo").click(function () {//-------删除按钮
        var checkStatus = table.checkStatus("id");//获取勾选的行
        if (checkStatus.data.length > 0) {
            layer.confirm('确认删除！', function (index) {
                layer.close(index);
                var idArray = new Array();
                for (var i = 0; i < checkStatus.data.length; i++) {//将勾选行的id存入数组
                    idArray[i] = checkStatus.data[i].id;
                }
                //根据传入的id数组删除数据
                $.post('../../index.ashx', { ctrl: "UserInfo", action: "DeleteUserInfoByIds", ids: idArray }, function (result) {
                    if (result.count != null) {
                        layer.open({
                            content: "成功删除" + result.count + "条数据",
                            btn: '确定',
                            shadeClose: false,
                            yes: function () {
                                var lis = parent.$("#ul-nav")[0].children;//获取所有的导航栏li
                                for (var i = 0; i < lis.length; i++) {
                                    if (lis[i].classList.contains("layui-this")) {//获取选中的导航栏li
                                        lis[i].click();//调用点击函数,刷新页面
                                    }
                                }
                            }
                        })
                    }
                })
            })
        }
        else {//为选则数据则提示
            layer.msg("未选择数据");
        }
    })
    table.on('tool(userInfo)', function (obj) {//-----------列工具条的按钮触发事件
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象

        if (layEvent === 'delete') { //删除
            layer.confirm('确认删除！', function (index) {
                obj.del(); //删除对应行（tr）的DOM结构，并更新缓存
                layer.close(index);
                //向服务端发送删除指令
                var idArray = new Array();
                idArray[0] = data.id;

                $.post('../../index.ashx', { ctrl: "UserInfo", action: "DeleteUserInfoByIds", ids: idArray }, function (result) {
                    if (result.count != null) {
                        layer.msg("删除" + result.count + "条数据");
                    }
                })
            })
        }
        else if (layEvent == 'edit') {//编辑
            layer.open({//弹框一个iframe框用于编辑
                type: 2,
                title: '修改用户信息',
                shadeClose: true,
                shade: 0,
                area: ['60%', '30%'],
                content: '../View/UserInfoEdit.html?userName=' + data.userName + '&password=' + data.password //iframe的url
            });
        }
    })

})
