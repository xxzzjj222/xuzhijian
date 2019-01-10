layui.use(['table', 'layer'], function () {
    var table = layui.table,
        layer = layui.layer;

    table.render(
        {//--------渲染表格
            elem: "#userInfo_RoleTable",
            url: '../../index.ashx?ctrl=UserInfo_Role&action=GetAllUserInfo_Role',//请求数据的url
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
                { field: 'userInfo', title: '用户名' },
                { field: 'role', title: '角色' },
                { toolbar: '#toolbar-tr', title: '操作', fixed: 'right' }
            ]],
            id: 'userInfo',//设置table的id
        });
    $("#addUserInfo_Role").click(function () {//-------新增按钮
        layer.open({//弹框一个iframe框用于新增
            type: 2,
            title: '新增用户角色信息',
            shadeClose: true,
            shade: 0,
            area: ['60%', '60%'],
            content: '../View/UserInfo_RoleAdd.html' //iframe的url
        });
    })
    $("#deleteUserInfo_Role").click(function () {//-------删除按钮
        var checkStatus = table.checkStatus("userInfo");//获取勾选的行
        if (checkStatus.data.length > 0) {
            layer.confirm('确认删除！', function (index) {
                layer.close(index);
                var nameArray = new Array();
                for (var i = 0; i < checkStatus.data.length; i++) {//将勾选行的name存入数组
                    nameArray[i] = checkStatus.data[i].userInfo;
                }
                var userNames = nameArray.join(',');//将数组转为字符串
                //根据传入的name数组删除数据
                $.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "DeleteUserInfo_RoleByNames", names: userNames }, function (result) {
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
    table.on('tool(userInfo_Role)', function (obj) {//-----------列工具条的按钮触发事件
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象

        if (layEvent === 'delete') { //删除
            layer.confirm('确认删除！', function (index) {
                obj.del(); //删除对应行（tr）的DOM结构，并更新缓存
                layer.close(index);
                //向服务端发送删除指令
                var nameArray = new Array();
                nameArray[0] = data.userInfo;
                var userNames = nameArray.join(',');//将数组转为字符串

                $.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "DeleteUserInfo_RoleByNames", names: userNames }, function (result) {
                    if (result.count != null) {
                        layer.msg("删除" + result.count + "条数据");
                    }
                })
            })
        }
        else if (layEvent == 'edit') {//编辑
            layer.open({//弹框一个iframe框用于编辑
                type: 2,
                title: '修改用户角色信息',
                shadeClose: true,
                shade: 0,
                area: ['60%', '60%'],
                content: '../View/UserInfo_RoleEdit.html?userName=' + data.userInfo //iframe的url
            });
        }
    })

})
