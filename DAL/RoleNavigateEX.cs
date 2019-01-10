using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.DAL
{
    public class RoleNavigateEX
    {
        /// <summary>
        /// 获取所有的角色导航
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页条数</param>
        /// <returns>拼接的数据json字符串</returns>
        public string GetAllRoleNavigate(int page, int limit)
        {
            string countText = "select count(distinct RoleID) from RelateNavigateRole";
            int count = Convert.ToInt32((SqlHelper.ExecuteSaclar(CommandType.Text, countText, null)));//获取所有的行数

            string commandText = "GetRoleNavigateGroupDetails";
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
                    data = data + "{\"roleName\":" + "\"" + rows[i]["RoleName"].ToString().Trim() + "\"" + ",\"navigateTitle\":" + "\"" + rows[i]["NavigateTitle"].ToString().Trim() + "\"" + "},";
                }
                data = data + "{\"roleName\":" + "\"" + rows[rows.Length - 1]["RoleName"].ToString().Trim() + "\"" + ",\"navigateTitle\":" + "\"" + rows[rows.Length - 1]["NavigateTitle"].ToString().Trim() + "\"" + "}]}";
            }
            else
            {
                data = data + "]}";
            }
                return data;
        }
        /// <summary>
        /// 获取未关联导航的所有的角色用于新增下拉列表显示
        /// </summary>
        /// <returns>返回json字符串</returns>
        public string GetAllRolesForselect()
        {
            string commandText = "select distinct ID,Name from Role where ID not in(select distinct RoleID from RelateNavigateRole) ";
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, null);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            StringBuilder data = new StringBuilder("{\"state\":\"success\",\"data\":[");//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data.Append("{\"id\":" + rows[i]["ID"] + ",\"roleName\":" + "\"" + rows[i]["Name"].ToString().Trim() + "\"},");
                }
                data.Append("{\"id\":" + rows[rows.Length - 1]["ID"] + ",\"roleName\":" + "\"" + rows[rows.Length - 1]["Name"].ToString().Trim() + "\"}]}");
            }
            else
            {
                data.Append("]}");
            }
            return data.ToString();
        }
        /// <summary>
        /// 获取所有导航用于新增多选
        /// </summary>
        /// <returns>返回拼接的json字符串</returns>
        public string GetAllNavigatesForNavigateOptions()
        {
            string commandText = "select distinct ID,NavigateTitle from Navigate";
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, null);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "{\"state\":\"success\",\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data = data + "{\"id\":" + rows[i]["ID"] + ",\"navigateTitle\":" + "\"" + rows[i]["NavigateTitle"].ToString().Trim() + "\"},";
                }
                data = data + "{\"id\":" + rows[rows.Length - 1]["ID"] + ",\"navigateTitle\":" + "\"" + rows[rows.Length - 1]["NavigateTitle"].ToString().Trim() + "\"}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
        /// <summary>
        /// 新增角色导航信息
        /// </summary>
        /// <param name="userInfoId">角色ID</param>
        /// <param name="roleId">导航ID的拼接字符串</param>
        /// <returns>新增失败或成功信息</returns>
        public string AddRoleNavigate(int roleId, string navigateIds)
        {
            string data = "{\"state\":";////将返回信息存入data字符串
            try
            {
                string[] navigateIdArr = navigateIds.Split(',');//将roleID字符串转为数组
                for (int i = 0; i < navigateIdArr.Length; i++)
                {//循环数组新增用户角色
                    string commandText = "insert into RelateNavigateRole values(@NavigateID,@RoleID)";
                    SqlParameter[] paras = new SqlParameter[]{
                     new SqlParameter("RoleID",roleId),
                     new SqlParameter("NavigateID",int.Parse(navigateIdArr[i]))
                    };
                    SqlHelper.ExecuteNonQuery(commandText, paras);
                }
                data = data + "\"success\",\"message\":\"新增成功\"}";
            }
            catch (Exception e)
            {
                data = data + "\"fail\",\"message\":\"新增失败\"}";
            }
            return data;
        }
        /// <summary>
        /// 根据角色获取选中的导航
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns>拼接的json字符串</returns>
        public string GetCheckedNavigateByRoleName(string roleName)
        {
            string commandText = "select NavigateID from RelateNavigateRole where RoleID in (select ID from Role where Name=@RoleName)";
            SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("RoleName",roleName)
                    };
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, paras);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "{\"data\":[";////将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接返回的json字符串
                {
                    data = data + "{\"navigateId\":" + rows[i]["NavigateID"] + "},";
                }
                data = data + "{\"navigateId\":" + rows[rows.Length - 1]["NavigateID"] + "}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
        /// <summary>
        /// 修改角色导航
        /// </summary>
        /// <param name="userName">角色</param>
        /// <param name="newRoleName">修改后的导航ID拼接字符串</param>
        /// <param name="oldRoleName">修改前的导航ID拼接字符串</param>
        /// <returns>修改完成后的json字符串</returns>
        public string EditRoleNavigate(string roleName, string newNavigateTitle, string oldNavigateTitle)
        {
            string data = "{\"state\":";//拼接返回的json字符串
            try
            {
                string roleIDCommand = "select ID from Role where Name=@RoleName";
                SqlParameter[] roleIDParas = new SqlParameter[]{
                    new SqlParameter("RoleName",roleName)
                };
                int roleID = (Int32)SqlHelper.ExecuteSaclar(CommandType.Text, roleIDCommand, roleIDParas);//获取角色的ID

                bool oldExist = false;//判断修改前的RoleID是否还存在
                List<string> deleteNavigate = new List<string>();//存储修改后不存在的修改前的NavigateID用于删除
                string[] arrNew = newNavigateTitle.Split(',');//修改后的导航ID转为数组
                string[] arrOld = oldNavigateTitle.Split(',');//修改前的导航ID转为数组
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
                        deleteNavigate.Add(arrOld[i]);//修改前存在，修改后不存在的NavigateID添加到list中，后面一起删除
                    }
                }
                if (deleteNavigate.Count > 0)
                {//删除修改后不再存在的导航
                    string deleteNavigateID = "(";
                    for (int i = 0; i < deleteNavigate.Count - 1; i++)
                    {
                        deleteNavigateID = deleteNavigateID + deleteNavigate[i] + ",";
                    }
                    deleteNavigateID = deleteNavigateID + deleteNavigate.Last() + ")";
                    string deleteCommand = "delete from RelateNavigateRole where RoleID=@RoleID and NavigateID in" + deleteNavigateID;
                    SqlParameter[] deleteParas = new SqlParameter[]{
                    new SqlParameter("RoleID",roleID)
                };
                    SqlHelper.ExecuteNonQuery(deleteCommand, deleteParas);
                }
                if (arrNew.Length > 0)
                {//新增修改前没拥有的导航
                    string insertCommand = "";
                    for (int i = 0; i < arrNew.Length; i++)
                    {
                        insertCommand = "insert into RelateNavigateRole values(@NavigateID,@RoleID)";
                        SqlParameter[] insertParas = new SqlParameter[]{
                        new SqlParameter("NavigateID",int.Parse(arrNew[i])),
                        new SqlParameter("RoleID",roleID)
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
        /// 根据角色删除导航
        /// </summary>
        /// <param name="roles">角色名的拼接字符串</param>
        /// <returns>返回拼接的json字符串</returns>
        public string DeleteRoleNavigateByRoles(string roles)
        {
            string data = "{\"state\":";////将返回信息存入data字符串
            try
            {
                string[] roleArray = roles.Split(',');//将角色名字符串转为数组
                for (int i = 0; i < roleArray.Length; i++)
                {//循环数组删除用户角色
                    string commandText = "delete from RelateNavigateRole where RoleID in(select ID from Role where Name=@RoleName)";
                    SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("RoleName",roleArray[i])
                    };
                    SqlHelper.ExecuteNonQuery(commandText, paras);
                }
                data = data + "\"success\",\"count\":" + roleArray.Length + "}";
            }
            catch (Exception e)
            {
                data = data + "\"fail\",\"message\":\"删除失败\"}";
            }
            return data;
        }
    }
}