using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Web
{
    public class SqlHelper
    {
        //获取连接字符串
        public static string GetSqlConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["connString"].ConnectionString.ToString();
        }
        
        /// <summary>
        /// ExecuteNonQuery封装，执行sql返回受影响的行数
        /// </summary>
        /// <param name="sqlText">执行的sql脚本</param>
        /// <param name="parameters">sql脚本的参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlText,params SqlParameter[] parameters)
        { 
            using(SqlConnection conn=new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sqlText;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();              
                }
            }
        }
        

        /// <summary>
        /// ExecuteScalar封装，返回查询结果中的第一行的第一列
        /// </summary>
        /// <param name="cmdType">sql命令类别</param>
        /// <param name="cmdText">执行的sql脚本</param>
        /// <param name="parameters">sql脚本的参数</param>
        /// <returns></returns>
        public static object ExecuteSaclar(CommandType cmdType, string cmdText, params SqlParameter[] parameters) 
        {
            using (SqlConnection conn = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandType = cmdType;
                    cmd.CommandText = cmdText;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// SqlDataAdapter封装
        /// </summary>
        /// <param name="cmdType">sql命令类别</param>
        /// <param name="cmdText">执行的sql脚本</param>
        /// <param name="parameters">sql脚本的参数</param>
        /// <param name="startRow">可选参数：分页的起始行</param>
        /// <param name="maxRows">可选参数：分页的最大行数</param>
        /// <param name="tableName">可选参数：表名</param>
        /// <returns></returns>
        public static DataTable ExecuteRead(CommandType cmdType, string cmdText, SqlParameter[] parameters, int startRow = -1, int maxRows = -1, string tableName = "defaultTable")
        {
            using (SqlConnection conn = new SqlConnection(GetSqlConnectionString()))
            {
                using (SqlCommand cmd=conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds=new DataSet();
                    if (startRow == -1 || maxRows == -1)
                    {
                        da.Fill(ds, tableName);//不分页
                    }
                    else
                    {
                        da.Fill(ds, startRow, maxRows, tableName);//分页
                    }
                    return ds.Tables[0];
                }
            }
        }
    }
}
