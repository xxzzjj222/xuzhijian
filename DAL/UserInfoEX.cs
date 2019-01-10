using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Web.Model;
using Web.Plugin;

namespace Web.DAL
{
    public class UserInfoEX
    {
        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="userInfo">userinfo对象</param>
        /// <returns></returns>
        public int InsertUserInfo(UserInfo userInfo, string role)
        {
            string commandText = "InsertUserInfo";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@userName",userInfo.UserName),
                new SqlParameter("@password",userInfo.Password)
            };
            try
            {
                int userInfoID = (Int32)SqlHelper.ExecuteSaclar(CommandType.StoredProcedure, commandText, paras);
                Workflow.ActionTrigger("UserInfo", "Insert", role, userInfoID);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        /// 根据用户名，判断用户是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>密码</returns>
        public string GetUserInfoByUserName(string userName)
        {
            string commandText = "select * from UserInfo where UserName=@userName";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@userName",userName)
            };
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, paras, 0, 9, "UserInfoTable");
            DataRow[] rows = dt.Select();//将返回的table存入DataRow数组
            if (rows.Length == 0)
            {//无用户则返回NoUser
                return "NoUser";
            }
            else
            {//有则返回第一行的密码
                return rows[0]["Password"].ToString().Trim();
            }
        }
        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns>拼接的json字符串</returns>
        public string GetAllUserInfo(int page, int limit,int role)
        {
            string countText = "select count(*) from UserInfo";
            int count = Convert.ToInt32((SqlHelper.ExecuteSaclar(CommandType.Text, countText, null)));//获取所有的行数

            string commandText = @"select  b.ID,b.UserName,b.Password,b.State,b.Description,stuff((select ','+bb.Action 
                                  from  (select top (@limit) u.*,s.Description,a.Description as Action  from UserInfo u 
                                  left join WorkflowDefinitionStates s on u.State=s.ID  
                                  left join WorkflowDefinitionTransits t on u.State=t.StartState and t.Role=@role
                                  left join WorkflowDefinitionActions a on t.Action=a.ID  
                                  where u.ID not in(select top (@before) ID from UserInfo) and s.DefinitionID in 
                                  (select ID from WorkflowDefinitions w where w.Name='UserInfo')) bb  
                                            where b.ID=bb.ID 
                                            for xml path('')),1,1,'') as Action 
                                 from (select top (@limit) u.*,s.Description,a.Description as Action  from UserInfo u 
                                  left join WorkflowDefinitionStates s on u.State=s.ID  
                                  left join WorkflowDefinitionTransits t on u.State=t.StartState and t.Role=@role
                                  left join WorkflowDefinitionActions a on t.Action=a.ID  
                                  where u.ID not in(select top (@before) ID from UserInfo) and s.DefinitionID in 
                                  (select ID from WorkflowDefinitions w where w.Name='UserInfo'))  b 
                                  group by b.ID,b.UserName,b.Password,b.State,b.Description";
            int before = limit * (page - 1);
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("limit",limit),
                new SqlParameter("before",before),
                new SqlParameter("role",role)
            };
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text,commandText, paras);
            DataRow[] rows = dt.Select();//将返回的table存入DataRow数组
            string data = "\"count\":" + count + ",\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)
                {
                    data = data + "{\"id\":" + rows[i]["ID"].ToString().Trim() + ",\"userName\":" + "\"" + rows[i]["UserName"].ToString().Trim() + "\"" + ",\"password\":" + "\"" + rows[i]["Password"].ToString().Trim() + "\"" + ",\"state\":" + "\"" + rows[i]["Description"] + "\"" + ",\"action\":" + "\"" + rows[i]["Action"] + "\"" + "},";
                }
                data = data + "{\"id\":" + rows[rows.Length - 1]["ID"].ToString().Trim() + ",\"userName\":" + "\"" + rows[rows.Length - 1]["UserName"].ToString().Trim() + "\"" + ",\"password\":" + "\"" + rows[rows.Length - 1]["Password"].ToString().Trim() + "\"" + ",\"state\":" + "\"" + rows[rows.Length - 1]["Description"] + "\"" + ",\"action\":" + "\"" + rows[rows.Length - 1]["Action"] + "\"" + "}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteUserInfoByIds(string ids)
        {
            string commandText = "delete from UserInfo where ID in(" + ids + ")";
            return SqlHelper.ExecuteNonQuery(commandText, null);

        }
        /// <summary>
        /// 根据id 修改用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public int EditUserInfo(UserInfo userInfo)
        {
            string commandText = "update UserInfo set Password=@Password where UserName=@userName";
            SqlParameter[] paras = new SqlParameter[]{//sql参数
                new SqlParameter("UserName",userInfo.UserName),
                new SqlParameter("Password",userInfo.Password)
            };
            return SqlHelper.ExecuteNonQuery(commandText, paras);
        }
    }
}
