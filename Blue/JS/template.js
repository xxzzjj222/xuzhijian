
layui.use(['layer', 'form', 'element', 'carousel'], function () {
    var element = layui.element,
        layer = layui.layer,
        form=layui.form,
        carousel = layui.carousel;
    //form.on('select(roles)', function (data) {
    //    if (data.value != null) {
    //        getNavigateChangeRole(data.value);
    //    }
    //    form.render('select');
    //})
    $(document).ready(function () {
        $("#a-loginUserName").text("欢迎" + getCookie("userName"));//设置span的值为"欢迎+userName"
        carousel.render({//设置轮播
            elem: '#carouel',
            width: '100%', //设置容器宽度
            height: '100px',//设置容器高度
            arrow: 'none' //始终显示箭头
        });
        $("#a-logout").click(function () {
            delCookie("userName");
            $.post('../../index.ashx', { action: "RemoveAllCache" }, function (result) {

            })
        });
        //加载角色下拉列表选项
        $.post('../../index.ashx', { ctrl: "UserInfo_Role", action: "GetAllRolesByUserInfoForselect" }, function (result) {
                var data = result.data;
                var select = $("select[name='userName']")//获取下拉列表元素
                var child;
                select.empty();//清空下拉列表的所有子元素
                for (var i = 0; i < data.length; i++) {//添加子元素
                    child = $("<option value=" + data[i].id + ">");
                    child.text(data[i].name);
                    select.append(child);
                }
                form.render('select');//重新渲染下拉列表
                $("#roleSelect").change(function () {
                    var selectedRoleID = $(this).children('option:selected').val();//这就是selected的值 
                    getNavigateChangeRole(selectedRoleID);//重新加载角色拥有的导航
                })
                getNavigateChangeRole(data[0].id);
        });
        
    })
    function getNavigateChangeRole(roleID)//角色切换重新加载导航
    {
        $.post('../../index.ashx', { ctrl: "Navigate", action: "GetAllNavigates", roleID: roleID }, function (result) {
            if (result.url != null) {//--------未登录，跳转至登录页
                window.location.href = result.url;
            }
            else {//--------动态添加导航
                $("#ul-nav").find("li").remove();
                for (var i = 0; i < result.length; i++) {//循环获取的数据
                    var liParams = {//创建li标签的参数
                        className: "layui-nav-item",//样式
                        elementType: "li",//标签
                        parentId: "ul-nav",//上级元素
                        index: i,
                        text: null,//显示文本
                        clickFun: clickFun(result[i].NavigateUrl)//点击执行的函数
                    }
                    var li = createChildNode(liParams);//创建li标签
                    if (result[i].NavigateUrl == "Main.html") {//若为用户管理导航，给加上选中样式
                        li.classList.add("layui-this");
                    }
                    var aParams = {//创建a标签样式
                        className: null,
                        elementType: 'a',
                        parentId: li.id,
                        index: i,
                        text: result[i].NavigateTitle
                    }
                    createChildNode(aParams);//创建a标签
                    var contentIframe = document.getElementById("changeIframe");
                    contentIframe.src = "Main.html";//设置iframeURL为用户管理
                }
            }
        });
    }
    function clickFun(navigateUrl) {//--------导航点击
        return function () {
            var preSelected = document.getElementsByClassName("layui-this");//获取前一个选中的导航
            var preSelectedNav=null;
            for (var i = 0; i < preSelected.length; i++)
            {
                if (preSelected[i].classList.contains("layui-nav-item")) {
                    preSelectedNav = preSelected[i];
                    break;
                }
            }
            if (preSelectedNav.id != this.id) {
                var contentIframe = document.getElementById("changeIframe");
                contentIframe.src = navigateUrl;//设置iframeURL
                var lis = this.parentNode.children;//获取导航所有的li节点
                for (var i = 0; i < lis.length; i++) {
                    if (lis[i].classList.contains("layui-this")) {//移除包含选中样式的节点的选中样式
                        lis[i].classList.remove("layui-this");
                    }
                }
                this.classList.add("layui-this");//给点击导航添加选中样式
            }
        }
    }
    //----------添加子节点（）
    var createChildNode = function (params) {
        var element = document.createElement(params.elementType);//创建子节点元素
        if (params.className != null) {
            element.className = params.className;//设置节点样式
        }
        if (params.text != null) {
            element.innerHTML = params.text;//设置文本
        }
        if (params.clickFun != null) {
            element.onclick = params.clickFun;
        }
        element.id = params.elementType + '-' + params.text + '-' + params.index;
        var parentNode = document.getElementById(params.parentId);//获取父节点元素
        parentNode.appendChild(element);//将节点加入父节点
        return element;
    }
});