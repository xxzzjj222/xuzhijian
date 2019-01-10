using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Plugin
{
    /// <summary>
    /// 获取文件物理路径类
    /// </summary>
    public class MapPath
    {
        public static string GetPath(string strPath)
        {
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Server.MapPath(strPath);
            }
            else//非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                strPath = strPath.Replace("~", "");
                if(strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
    }
}