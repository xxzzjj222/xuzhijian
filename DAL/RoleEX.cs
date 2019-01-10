using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Web.DAL
{
    public class RoleEX
    {
        /// <summary>
        /// 获取所有的角色数据，角色管理页面用
        /// </summary>
        /// <returns>拼接的json字符串</returns>
        public string GetAllRolesForManage(int page, int limit)
        {
            string countText = "select count(*) from Role";
            int count = Convert.ToInt32((SqlHelper.ExecuteSaclar(CommandType.Text, countText, null)));//获取所有的行数

            string commandText = "select top (@limit) * from Role where ID not in(select top (@before) ID from Role)";
            int before = limit * (page - 1);
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("limit",limit),
                new SqlParameter("before",before)
            };

            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, paras);//获取所有的数据存入datatable
            DataRow[] rows = dt.Select();//将所有的行数据存入datarow数组中
            string data = "\"count\":" + count + ",\"data\":[";//将数据存入data字符串
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data = data + "{\"id\":" + rows[i]["ID"].ToString().Trim() + ",\"name\":" + "\"" + rows[i]["Name"].ToString().Trim() + "\"" + "},";
                }
                data = data + "{\"id\":" + rows[rows.Length - 1]["ID"].ToString().Trim() + ",\"name\":" + "\"" + rows[rows.Length - 1]["Name"].ToString().Trim() + "\"" + "}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
    }
}