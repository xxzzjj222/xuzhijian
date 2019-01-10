using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.DAL;
using Web.Plugin;

namespace Web.Control
{
    public class UserInfo_RoleController:BaseController
    {
        /// <summary>
        /// 获取所有用户角色
        /// </summary>
        public void GetAllUserInfo_Role()
        {
            UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建RoleEX对象
            int page = int.Parse(ctx.Request["page"]);//页码
            int limit = int.Parse(ctx.Request["limit"]);//每页的数据

            string userInfo_Roles = userInfo_RoleEx.GetAllUserInfo_Role(page, limit);
            string result = "{\"code\":0,\"msg\":\"\"," + userInfo_Roles;//拼接字符串为layui-table规范的格式
            WriteJsonBack(result);//返回context
        }
        /// <summary>
        /// 获取所有的用户名用于新增下拉列表
        /// </summary>
        public void GetAllUserInfosForselect()
        {
            UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string userInfos = userInfo_RoleEx.GetAllUserInfosForSelect();//获取所有的用户名
            WriteJsonBack(userInfos);//返回context
        }
        /// <summary>
        /// 获取所有角色名用于新增多选
        /// </summary>
        public void GetAllRolesForRoleOptions()
        {
            UserInfo_RoleEX uerInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string roles = uerInfo_RoleEx.GetAllRolesForRoleOptions();//获取所有角色名
            WriteJsonBack(roles);//返回context
        }
        /// <summary>
        /// 新增用户角色
        /// </summary>
        public void AddUserInfo_Role()
        {
            string addMessage;
            try
            {
                UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
                int userInfoId = int.Parse(ctx.Request["userName"]);//UserInfoID
                string roleId = ctx.Request["name"];//RoleID的拼接字符串
                addMessage = userInfo_RoleEx.AddUserInfo_Role(userInfoId, roleId);
            }
            catch(Exception e)
            {
                addMessage = "{\"state\":\"fail\",\"message\":\"新增失败\"}";
            }
            WriteJsonBack(addMessage);//返回context
        }
        /// <summary>
        /// 根据用户民删除用户角色
        /// </summary>
        public void DeleteUserInfo_RoleByNames()
        {
            string names = ctx.Request["names"];//获取需要删除的用户名数组
            UserInfo_RoleEX userInf_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string deleteMessage = userInf_RoleEx.DeleteUserInfo_RoleByNames(names);
            WriteJsonBack(deleteMessage);//返回删除的信息至context
        }
        /// <summary>
        /// 根据用户名获取选中的角色
        /// </summary>
        public void GetCheckedRoleByUserInfo()
        {
            string userName = ctx.Request["userName"];//获取需要删除的用户名数组
            UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string checkedRoleIds = userInfo_RoleEx.GetCheckedRoleByUserInfo(userName);
            WriteJsonBack(checkedRoleIds);//返回删除的信息至context
        }
        /// <summary>
        /// 修改用户角色
        /// </summary>
        public void EditUserInfo_Role()
        {
            string userName = ctx.Request["userName"];//用户名
            string newRoleName = ctx.Request["newRoleName"];//修改后选中的角色ID的拼接字符串
            string oldRoleName = ctx.Request["oldRoleName"];//修改前选中的角色ID

            UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string editMessage = userInfo_RoleEx.EditUserInfo_Role(userName, newRoleName,oldRoleName);
            WriteJsonBack(editMessage);//返回修改的信息值context
        }
        /// <summary>
        /// 获取模板页角色更换的下拉列表值
        /// </summary>
        public void GetAllRolesByUserInfoForselect()
        {
            string userName = Plugin.CacheHelper.GetCache("UserName").ToString();//用户名
            UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string data=userInfo_RoleEx.GetAllRolesByUserInfoForselect(userName);
            WriteJsonBack(data);//返回修改的信息值context
        }
        /// <summary>
        /// 获取各角色绑定的用户数量
        /// </summary>
        public void GetUserInfoCountByRole()
        {
            UserInfo_RoleEX userInfo_RoleEx = new UserInfo_RoleEX();//创建UserInfo_RoleEX对象
            string data = userInfo_RoleEx.GetUserInfoCountByRole();
            WriteJsonBack(data);//返回信息至context
        }
    }
}