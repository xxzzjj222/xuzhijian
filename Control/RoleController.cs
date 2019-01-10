using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.DAL;

namespace Web.Control
{
    public class RoleController:BaseController
    {
        /// <summary>
        /// 获取所有的角色数据，用于角色管理
        /// </summary>
        public void GetAllRolesForManage()
        {
            RoleEX roleEx = new RoleEX();//创建RoleEX对象
            int page = int.Parse(ctx.Request["page"]);//页码
            int limit = int.Parse(ctx.Request["limit"]);//每页的数据

            string roles = roleEx.GetAllRolesForManage(page, limit);
            string result = "{\"code\":0,\"msg\":\"\"," + roles;//拼接字符串为layui-table规范的格式
            WriteJsonBack(result);//返回context
        }
    }
}