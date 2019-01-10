using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.DAL;

namespace Web.Control
{
    public class RoleNavigateController:BaseController
    {
        /// <summary>
        /// 获取所有的角色导航
        /// </summary>
        public void GetAllRoleNavigate()
        {
            RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
            int page = int.Parse(ctx.Request["page"]);//页码
            int limit = int.Parse(ctx.Request["limit"]);//每页的数据
            string roleNavigates = roleNavigateEx.GetAllRoleNavigate(page, limit);
            string result = "{\"code\":0,\"msg\":\"\"," + roleNavigates;//拼接字符串为layui-table规范的格式
            WriteJsonBack(result);//返回context
        }
        /// <summary>
        /// 获取未关联导航的所有的角色用于新增下拉列表显示
        /// </summary>
        public void GetAllRolesForselect()
        {
            RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
            string roles = roleNavigateEx.GetAllRolesForselect();//获取所有未关联导航的角色
            WriteJsonBack(roles);//返回context
        }
        /// <summary>
        /// 获取所有导航用于新增多选
        /// </summary>
        public void GetAllNavigatesForNavigateOptions()
        {
            RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
            string navigates = roleNavigateEx.GetAllNavigatesForNavigateOptions();//获取所有导航
            WriteJsonBack(navigates);//返回context
        }
        /// <summary>
        /// 新增角色导航
        /// </summary>
        public void AddRoleNavigate()
        {
            string addMessage;
            try
            {
                RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
                int roleId = int.Parse(ctx.Request["roleName"]);//RoleId
                string navigateIds = ctx.Request["navigateTitle"];//NavigateID的拼接字符串
                addMessage = roleNavigateEx.AddRoleNavigate(roleId, navigateIds);
            }
            catch(Exception e)
            {
                addMessage = "{\"state\":\"fail\",\"message\":\"新增失败\"}";
            }
            WriteJsonBack(addMessage);//返回context
        }
        /// <summary>
        /// 根据角色获取选中的导航
        /// </summary>
        public void GetCheckedNavigateByRoleName()
        {
            string roleName = ctx.Request["roleName"];//获取需要删除的导航数组
            RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
            string checkedNavigateIds = roleNavigateEx.GetCheckedNavigateByRoleName(roleName);
            WriteJsonBack(checkedNavigateIds);//返回信息至context
        }
        /// <summary>
        /// 修改角色导航
        /// </summary>
        public void EditRoleNavigate()
        {
            string roleName = ctx.Request["roleName"];//角色
            string newNavigateTitle = ctx.Request["newNavigateTitle"];//修改后选中的导航ID的拼接字符串
            string oldNavigateTitle = ctx.Request["oldNavigateTitle"];//修改前选中的导航ID

            RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
            string editMessage = roleNavigateEx.EditRoleNavigate(roleName, newNavigateTitle, oldNavigateTitle);
            WriteJsonBack(editMessage);//返回修改的信息值context
        }
        /// <summary>
        /// 根据角色删除导航
        /// </summary>
        public void DeleteRoleNavigateByRoles()
        {
            string roles = ctx.Request["roles"];//获取需要删除的角色数组
            RoleNavigateEX roleNavigateEx = new RoleNavigateEX();//创建EX对象
            string deleteMessage = roleNavigateEx.DeleteRoleNavigateByRoles(roles);
            WriteJsonBack(deleteMessage);//返回删除的信息至context
        }
    }

}