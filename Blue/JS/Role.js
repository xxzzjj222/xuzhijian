layui.use(['table', 'layer'], function () {
    var table = layui.table,
        layer = layui.layer;

    table.render(
        {
            elem: "#RoleTable",
            url: '../../index.ashx?ctrl=Role&action=GetAllRolesForManage',
            page: true,//分页
            limit: 10,//每页条数
            limits: [5, 10, 20],
            parseData: function (result) { //result 即为原始返回的数据
                if (result.url != null) {
                    window.location.href = result.url;//未登录跳转至登录页
                }
            },
            cols: [[
                { field: 'id', title: 'ID', sort: true, fixed: 'left' },
                { field: 'name', title: '角色名称' }
            ]]
        }
    )

})