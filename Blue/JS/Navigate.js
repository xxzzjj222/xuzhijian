layui.use(['table', 'layer'], function () {
    var table = layui.table,
        layer = layui.layer;

    table.render(
        {
            elem: "#NavigateTable",
            url: '../../index.ashx?ctrl=Navigate&action=GetAllNavigatesForManage',
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
                { field: 'navigateTitle', title: '导航' },
                { field: 'navigateUrl', title: '链接' },
                { field: 'sort', title: '排序',sort: true }
            ]]
        }
    )

})