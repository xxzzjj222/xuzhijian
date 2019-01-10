
function getCookie(c_name) {//根据key获取cookie的value
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}

function setCookie(c_name, value, expiredays) {//通过key value 及过期时间设置cookie
    var cookieName = getCookie(c_name)
    if (cookieName != null && cookieName != "") {
        return;
    }
    else {
        var exdate = new Date()
        exdate.setDate(exdate.getDate() + expiredays)
        document.cookie = c_name + "=" + escape(value) +
        ((expiredays == null) ? "" : "; expires=" + exdate.toGMTString())
    }
}
//删除cookie
function delCookie(c_name) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() - 1);
    var c_val = getCookie(c_name);
    if (c_val != null) {
        document.cookie = c_name + "=" + escape(c_val) + ";expires=" + exdate.toGMTString();
    }
}

//获取地址栏参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}