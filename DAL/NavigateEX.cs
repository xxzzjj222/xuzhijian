using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Web.DAL
{
    public class NavigateEX
    {
        /// <summary>
        /// 获取角色所有的导航数据,导航栏用
        /// </summary>
        /// <returns>拼接的json字符串</returns>
        public string GetAllNavigates(int roleID)
        {
            string commandText = "select * from Navigate where ID in (select NavigateID from RelateNavigateRole where RoleID =@RoleID ) order by Sort asc";
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("RoleID",roleID),
            };
            DataTable dt = SqlHelper.ExecuteRead(CommandType.Text, commandText, paras);//获取所有数据存入datatable
            DataRow[] rows = dt.Select();//将所有行数据存入datarow数组中
            string data = "[";
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length - 1; i++)//拼接为json字符串格式
                {
                    data = data + "{\"NavigateTitle\":" + "\"" + rows[i]["NavigateTitle"].ToString().Trim() + "\"" + ",\"NavigateUrl\":" + "\"" + rows[i]["NavigateUrl"].ToString().Trim() + "\"" + "},";
                }
                data = data + "{\"NavigateTitle\":" + "\"" + rows[rows.Length - 1]["NavigateTitle"].ToString().Trim() + "\"" + ",\"NavigateUrl\":" + "\"" + rows[rows.Length - 1]["NavigateUrl"].ToString().Trim() + "\"" + "}]";
            }
            else
            {
                data = data + "]";
            }
            return data;
        }
        /// <summary>
        /// 获取所有的导航数据，导航管理页面用
        /// </summary>
        /// <returns>拼接的json字符串</returns>
        public string GetAllNavigatesForManage(int page, int limit)
        {
            string countText = "select count(*) from Role";
            int count = Convert.ToInt32((SqlHelper.ExecuteSaclar(CommandType.Text, countText, null)));//获取所有的行数

            string commandText = "select top (@limit) * from Navigate where ID not in(select top (@before) ID from Navigate)";
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
                    data = data + "{\"id\":" + rows[i]["ID"].ToString().Trim() + ",\"navigateTitle\":" + "\"" + rows[i]["NavigateTitle"].ToString().Trim() + "\"" + ",\"navigateUrl\":" + "\"" + rows[i]["NavigateUrl"].ToString().Trim() + "\"" + ",\"sort\":" + rows[i]["sort"].ToString().Trim() + "},";
                }
                data = data + "{\"id\":" + rows[rows.Length - 1]["ID"].ToString().Trim() + ",\"navigateTitle\":" + "\"" + rows[rows.Length - 1]["NavigateTitle"].ToString().Trim() + "\"" + ",\"navigateUrl\":" + "\"" + rows[rows.Length - 1]["NavigateUrl"].ToString().Trim() + "\"" + ",\"sort\":" + rows[rows.Length - 1]["Sort"].ToString().Trim() + "}]}";
            }
            else
            {
                data = data + "]}";
            }
            return data;
        }
    }
}