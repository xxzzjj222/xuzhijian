using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.DAL;
using Web.Model;
using Web.Plugin;
using System.Web;
using System.Web.SessionState;

namespace Web.Control
{
    public class UserInfoController : BaseController, IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 插入用户
        /// </summary>
        public void AddUserInfo()
        {
            string result;//存储返回的json字符串
            UserInfoEX userInfoEX = new UserInfoEX();//定义userInfoEX对象
            string user = userInfoEX.GetUserInfoByUserName(ctx.Request["userName"]);//判断是否已存在该用户
            if (user == "NoUser")
            {//不存在，进行新增
                UserInfo userInfo = new UserInfo();//定义userinfo对象    
                userInfo.UserName = ctx.Request["userName"];
                userInfo.Password = ctx.Request["password"];
                string role = ctx.Request["role"];
                int num = userInfoEX.InsertUserInfo(userInfo,role);
                if (num > 0)
                {//返回值大于0,插入成功
                    result = "{\"state\":\"success\",\"message\":\"新增成功\"}";
                }
                else
                {//插入失败
                    result = "{\"state\":\"fail\",\"message\":\"新增失败，请重试\"}";
                }
            }
            else
            {//存在，返回提示
                result = "{\"state\":\"fail\",\"message\":\"该用户名已存在\"}";
            }
            WriteJsonBack(result);//返回至context
        }
        /// <summary>
        /// 根据用户名称查找用户
        /// </summary>
        public void GetLoginStateByUserName()
        {
            string result;//存储返回的json字符串
            UserInfoEX userInfoEX = new UserInfoEX();//定义userInfoEX对象
            var userName = ctx.Request["UserName"];
            string password = userInfoEX.GetUserInfoByUserName(userName);
            if (password == "NoUser")
            {//不存在
                result = "{\"state\":\"fail\",\"message\":\"该用户不存在\"}";
            }
            else if (password == ctx.Request["Password"])
            {//判断密码是否正确
                CacheHelper.SetCache("UserName", ctx.Request["UserName"]);//将用户名存入cache
                string url = ctx.Request.Url.ToString();
                string rawUrl = ctx.Request.Url.PathAndQuery.ToString();
                string getUrl = url.Substring(0, url.IndexOf(rawUrl));
                result = "{\"state\":\"success\",\"message\":\"" + getUrl + "/View/Template.html\"}";
            }
            else
            {
                result = "{\"state\":\"fail\",\"message\":\"密码错误\"}";
            }
            WriteJsonBack(result);//返回context
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        public void GetAllUserInfo()
        {
            UserInfoEX userInfoEX = new UserInfoEX();
            int page = int.Parse(ctx.Request["page"]);//页码
            int limit = int.Parse(ctx.Request["limit"]);//每页的数据
            int role = int.Parse(ctx.Request["role"]);//当前角色

            string users = userInfoEX.GetAllUserInfo(page, limit,role);
            string result = "{\"code\":0,\"msg\":\"\"," + users;//拼接字符串为layui-table规范的格式
            WriteJsonBack(result);//返回context
        }
        /// <summary>
        /// 根据id删除用户
        /// </summary>
        public void DeleteUserInfoByIds()
        {
            string ids = ctx.Request["ids[]"];
            UserInfoEX userInfoEX = new UserInfoEX();
            int deleteCount = userInfoEX.DeleteUserInfoByIds(ids);
            string result = "{\"count\":" + deleteCount + "}";
            WriteJsonBack(result);//返回删除的条数至context
        }
        /// <summary>
        /// 根据用户名修改用户信息
        /// </summary>
        public void EditUserInfo()
        {
            UserInfo userInfo = new UserInfo();//创建一个用户实例
            userInfo.UserName = ctx.Request["userName"];
            userInfo.Password = ctx.Request["password"];

            UserInfoEX userInfoEX = new UserInfoEX();//创建UserInfoEX实例
            int num = userInfoEX.EditUserInfo(userInfo);
            string result;
            if (num > 0)
            {
                result = "{\"state\":\"success\",\"message\":\"修改成功\"}";
            }
            else
            {
                result = "{\"state\":\"fail\",\"message\":\"修改失败\"}";
            }
            WriteJsonBack(result);//返回修改的条数至context
        }
        /// <summary>
        /// 工作流触发
        /// </summary>
        public void WorkflowTrigger()
        {
            string workflowAction=ctx.Request["workflowAction"];//工作流动作
            string role=ctx.Request["role"];//工作流角色
            int entityID = int.Parse(ctx.Request["entityID"]);//数据ID
            string state = ctx.Request["state"];//工作流当前状态
            Workflow.ActionTrigger("UserInfo",workflowAction,role,entityID,state);
        }
    }
}
