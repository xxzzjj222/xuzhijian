using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.DAL;

namespace Web.Control
{
    public class NavigateController:BaseController
    {
        /// <summary>
        /// 获取所有的导航数据，用于导航栏
        /// </summary>
        public void GetAllNavigates()
        {
            NavigateEX navigateEx = new NavigateEX();//创建NavigateEX对象
            int roleID = int.Parse(ctx.Request["roleID"]);//角色ID
            string result = navigateEx.GetAllNavigates(roleID);
            WriteJsonBack(result);//返回context
        }
        /// <summary>
        /// 获取所有的导航数据，用于导航管理
        /// </summary>
        public void GetAllNavigatesForManage()
        {
            NavigateEX navigateEx = new NavigateEX();//创建NavigateEX对象
            int page = int.Parse(ctx.Request["page"]);//页码
            int limit = int.Parse(ctx.Request["limit"]);//每页的数据

            string navigates = navigateEx.GetAllNavigatesForManage(page, limit);
            string result = "{\"code\":0,\"msg\":\"\"," + navigates;//拼接字符串为layui-table规范的格式
            WriteJsonBack(result);//返回context
        }
    }
}