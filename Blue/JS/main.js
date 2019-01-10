// 基于准备好的dom，初始化echarts实例
var myChart = echarts.init(document.getElementById('main'));

$.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "GetUserInfoCountByRole" }, function (result) {
    // 填入数据
    myChart.setOption({
        roseType: 'angle',
        series: [{
            // 根据名字对应到相应的系列
            name: '用户',
            type: 'pie',
            radius: '75%',
            data: result.data
        }],
        backgroundColor: '#2c343c',
        textStyle: {
            color: 'rgba(255, 255, 255, 0.3)'
        },
        label: {
            textStyle: {
                color: 'rgba(255, 255, 255, 0.3)'
            }
        },
        labelLine: {
            lineStyle: {
                color: 'rgba(255, 255, 255, 0.3)'
            }
        }
        
    });
})
