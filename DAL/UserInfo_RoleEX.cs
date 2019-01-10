using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;


namespace Web.DAL
{
    public class UserInfo_RoleEX
    {
        /// <summary>
        /// 获取所有的用户角色数据，用户角色管理页面用
        /// </summary>
        /// <returns>拼接的json字符串</returns>
        public string GetAllUserInfo_Role(int page, int limit)
        {
            string countText = "select count(distinct UserInfoID) from UserInfo_Role";
            int count = Convert.ToInt32((SqlHelper.ExecuteSaclar(CommandType.Text, countText, null)));//获取所有的行数

            string commandText = "GetUserInfo_RoleGroupDetails";
            int before = limit * (page - 1);
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("limit",limit),
                new SqlParameter("before",before)
            };

            DataTable dt = SqlHelper.ExecuteRead(CommandType.StoredProcedure, commandText, paras);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "\"count\":" + count + ",\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data = data + "{\"userInfo\":" + "\"" + rows[i]["UserName"].ToString().Trim() + "\"" + ",\"role\":" + "\"" + rows[i]["Name"].ToString().Trim() + "\"" + "},";
                }
                data = data + "{\"userInfo\":" + "\"" + rows[rows.Length - 1]["UserName"].ToString().Trim() + "\"" + ",\"role\":" + "\"" + rows[rows.Length - 1]["Name"].ToString().Trim() + "\"" + "}]}";
            }
            else
            {
                data = data + "]}";
            }
                return data;
        }
        /// <summary>
        /// 获取未关联角色的所有的用户用于新增下拉列表显示
        /// </summary>
        /// <returns>返回json字符串</returns>
        public string GetAllUserInfosForSelect()
        {
            string commandText = "select distinct ID,UserName from UserInfo where ID not in(select distinct UserInfoID from UserInfo_Role) ";
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, null);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            StringBuilder data = new StringBuilder("{\"state\":\"success\",\"data\":[");//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data.Append("{\"id\":" + rows[i]["ID"] + ",\"userName\":" + "\"" + rows[i]["UserName"].ToString().Trim() + "\"},");
                }
                data.Append("{\"id\":" + rows[rows.Length - 1]["ID"] + ",\"userName\":" + "\"" + rows[rows.Length - 1]["UserName"].ToString().Trim() + "\"}]}");
            }
            else
            {
                data.Append("]}");
            }
                return data.ToString();
        }
        /// <summary>
        /// 获取所有的角色用于新增多选显示
        /// </summary>
        /// <returns>返回拼接的json字符串</returns>
        public string GetAllRolesForRoleOptions()
        {
            string commandText = "select distinct ID,Name from Role";
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, null);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "{\"state\":\"success\",\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data = data + "{\"id\":" + rows[i]["ID"] + ",\"name\":" + "\"" + rows[i]["Name"].ToString().Trim() + "\"},";
                }
                data = data + "{\"id\":" + rows[rows.Length - 1]["ID"] + ",\"name\":" + "\"" + rows[rows.Length - 1]["Name"].ToString().Trim() + "\"}]}";
            }
            else
            {
                data = data + "]}";
            }
                return data;
        }
        /// <summary>
        /// 新增用户角色信息
        /// </summary>
        /// <param name="userInfoId">用户ID</param>
        /// <param name="roleId">角色ID的拼接字符串</param>
        /// <returns>新增失败或成功信息</returns>
        public string AddUserInfo_Role(int userInfoId,string roleId)
        {
            string data = "{\"state\":";////将返回信息存入data字符串
            try
            {
                string[] roleIdArr = roleId.Split(',');//将roleID字符串转为数组
                for (int i = 0; i < roleIdArr.Length; i++)
                {//循环数组新增用户角色
                    string commandText = "insert into UserInfo_Role values(@UserInfoID,@RoleID)";
                    SqlParameter[] paras = new SqlParameter[]{
                     new SqlParameter("UserInfoID",userInfoId),
                     new SqlParameter("RoleID",int.Parse(roleIdArr[i]))
                    };
                    SqlHelper.ExecuteNonQuery(commandText, paras);
                }
                data = data + "\"success\",\"message\":\"新增成功\"}";
            }
            catch(Exception e)
            {
                data = data + "\"fail\",\"message\":\"新增失败\"}";
            }
            return data;
        }
        /// <summary>
        /// 根据用户名删除用户角色
        /// </summary>
        /// <param name="names">用户名的拼接字符串</param>
        /// <returns>返回拼接的json字符串</returns>
        public string DeleteUserInfo_RoleByNames(string names)
        {
            string data = "{\"state\":";////将返回信息存入data字符串
            try
            {
                string[] nameArray = names.Split(',');//将用户名字符串转为数组
                for (int i = 0; i < nameArray.Length; i++)
                {//循环数组删除用户角色
                    string commandText = "delete from UserInfo_Role where UserInfoID in(select ID from UserInfo where UserName=@UserName)";
                    SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("UserName",nameArray[i])
                    };
                    SqlHelper.ExecuteNonQuery(commandText, paras);
                }
                data = data + "\"success\",\"count\":"+nameArray.Length+"}";
            }
            catch(Exception e)
            {
                data = data + "\"fail\",\"message\":\"删除失败\"}";
            }
            return data;
        }
        /// <summary>
        /// 根据用户名获取选中的角色
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>拼接的json字符串</returns>
        public string GetCheckedRoleByUserInfo(string userName) {
            string commandText = "select RoleID from UserInfo_Role where UserInfoID in (select ID from UserInfo where UserName=@UserName)";
            SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("UserName",userName)
                    };
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, paras);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "{\"data\":[";////将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接返回的json字符串
                {
                    data = data + "{\"roleId\":" + rows[i]["RoleID"] + "},";
                }
                data = data + "{\"roleId\":" + rows[rows.Length - 1]["RoleID"] + "}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="newRoleName">修改后的角色ID拼接字符串</param>
        /// <param name="oldRoleName">修改前的角色ID拼接字符串</param>
        /// <returns>修改完成后的json字符串</returns>
        public string EditUserInfo_Role(string userName, string newRoleName,string oldRoleName)
        {
            string data = "{\"state\":";//拼接返回的json字符串
            try
            {
                string userInfoIDCommand = "select ID from UserInfo where UserName=@UserName";
                SqlParameter[] userInfoIDParas = new SqlParameter[]{
                    new SqlParameter("UserName",userName)
                };
                int userInfoID = (Int32)SqlHelper.ExecuteSaclar(CommandType.Text, userInfoIDCommand, userInfoIDParas);//获取用户名的用户ID

                bool oldExist = false;//判断修改前的RoleID是否还存在
                List<string> deleteRole = new List<string>();//存储修改后不存在的修改前的RoleID用于删除
                string[] arrNew = newRoleName.Split(',');//修改后的角色ID转为数组
                string[] arrOld = oldRoleName.Split(',');//修改前的角色ID转为数组
                for (int i = 0; i < arrOld.Length; i++)
                {
                    oldExist = false;
                    for (int j = 0; j < arrNew.Length; j++)
                    {
                        if (arrOld[i] == arrNew[j])
                        {//修改前后都存在的则从修改后的数组中移除，无需再做处理
                            oldExist = true;
                            List<string> list = arrNew.ToList();
                            list.RemoveAt(j);
                            arrNew = list.ToArray();
                            break;
                        }
                    }
                    if (!oldExist)
                    {
                        deleteRole.Add(arrOld[i]);//修改前存在，修改后不存在的RoleID添加到list中，后面一起删除
                    }
                }
                if (deleteRole.Count > 0)
                {//删除修改后不再存在的角色
                    string deleteRoleID = "(";
                    for (int i = 0; i < deleteRole.Count - 1; i++)
                    {
                        deleteRoleID = deleteRoleID + deleteRole[i] + ",";
                    }
                    deleteRoleID = deleteRoleID + deleteRole.Last() + ")";
                    string deleteCommand = "delete from UserInfo_Role where UserInfoID=@UserInfoID and RoleID in" + deleteRoleID;
                    SqlParameter[] deleteParas = new SqlParameter[]{
                    new SqlParameter("UserInfoID",userInfoID)
                };
                    SqlHelper.ExecuteNonQuery(deleteCommand, deleteParas);
                }
                if (arrNew.Length > 0)
                {//新增修改前没拥有的角色
                    string insertCommand = "";
                    for (int i = 0; i < arrNew.Length; i++)
                    {
                        insertCommand = "insert into UserInfo_Role values(@UserInfoID,@RoleID)";
                        SqlParameter[] insertParas = new SqlParameter[]{
                        new SqlParameter("UserInfoID",userInfoID),
                        new SqlParameter("RoleID",int.Parse(arrNew[i]))
                    };
                        SqlHelper.ExecuteNonQuery(insertCommand, insertParas);
                    }
                }
                data = data + "\"success\",\"message\":\"修改成功\"}";
            }
            catch (Exception e)
            {
                data = data + "\"fail\",\"message\":\"修改失败\"}";
            }
            return data;
        }
        /// <summary>
        /// 获取模板页角色更换的下拉列表值
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>拼接的json字符串</returns>
        public string GetAllRolesByUserInfoForselect(string userName)
        {
            string commandText = "select ID,Name from Role where ID in(select RoleID from UserInfo_Role where UserInfoID in(select ID from UserInfo where UserName=@UserName))";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("UserName",userName)
            };
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, paras);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "{\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接返回的json字符串
                {
                    data = data + "{\"id\":" + rows[i]["ID"] + ",\"name\":\"" + rows[i]["Name"] + "\"},";
                }
                data = data + "{\"id\":" + rows[rows.Length - 1]["ID"] + ",\"name\":\"" + rows[rows.Length - 1]["Name"] + "\"}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
        /// <summary>
        /// 获取各个角色拥有的用户数
        /// </summary>
        /// <returns>拼接的json字符串</returns>
        public string GetUserInfoCountByRole()
        {
            string commandText = "select  count(u.UserInfoID) as UserCount,r.Name as RoleName from UserInfo_Role u inner join Role r on u.RoleID=r.ID group by r.Name";
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, null);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            //string data = "{\"data\":{";//将数据存入data字符串
            //if (rows.Length > 0)
            //{
            //    string userCount = "\"userCount\":[";//用户数字符串
            //    string roleName="\"roleName\":[";//角色名字符串
            //    for (int i = 0; i < rows.Length - 1; i++)//拼接返回的json字符串
            //    {
            //        userCount = userCount + rows[i]["UserCount"] + ",";
            //        roleName=roleName+"\""+rows[i]["RoleName"]+"\",";
            //    }
            //    userCount = userCount + rows[rows.Length - 1]["UserCount"] + "]";
            //    roleName = roleName + "\"" + rows[rows.Length - 1]["RoleName"] + "\"]";
            //    data = data + userCount + "," + roleName + "}}";
            //}
            //else
            //{
            //    data = data + "}}";
            //}
            //return data;
            string data = "{\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接返回的json字符串
                {
                    data = data + "{\"value\":" + rows[i]["UserCount"] + ",\"name\":\"" + rows[i]["RoleName"] + "\"},";
                }
                data = data + "{\"value\":" + rows[rows.Length - 1]["UserCount"] + ",\"name\":\"" + rows[rows.Length - 1]["RoleName"] + "\"}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
    }
}