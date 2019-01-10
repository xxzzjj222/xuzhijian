using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SqlSugar;

namespace Web.DAL
{
    public class SugarDao
    {
        //禁止实例化，静态实例方便直接引用，避免反复创建实例
        private SugarDao() { }

        private static string _connectionString;
        public static string ConnectionString
        {
            get 
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.AppSettings["sqlProvider"].ToString();
                }
                return _connectionString;
            }
        }

        private static SqlSugarClient _db;
        public static SqlSugarClient Instance
        {
            get 
            {
                if (_db == null)
                {
                    _db = new SqlSugarClient(new ConnectionConfig() 
                    { 
                        ConnectionString=ConnectionString,
                        DbType=DbType.SqlServer,
                        IsAutoCloseConnection=true
                    });
                }
                return _db;
            }
        }
    }
}