using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Web.Control;
using Web.Plugin;
using System.Web.SessionState;
using System.Text;


namespace Web
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            
            if (CacheHelper.GetCache("UserName")!=null || (context.Request["ctrl"] == "UserInfo" && context.Request["action"] == "GetLoginStateByUserName"))
            {//已登录或请求为登录验证
                CallController(context);//调用控制器处理请求
            }
            else
            { //其他请求未登录则重定向至登录页
                string url = context.Request.Url.ToString();
                string rawUrl = context.Request.Url.PathAndQuery.ToString();
                string getUrl = url.Substring(0, url.IndexOf(rawUrl));
                //string uu=getUrl+"/View/login.html";
                string redirectUrl = "{\"url\":\"" + getUrl + "/View/login.html\"}";//拼接登录界面的url
                //context.Response.Write("<script language='javascript'> window.location.href = '" + uu + "';console.log(1);</script>");
                //context.Response.Redirect(uu);
                context.Response.Clear();
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.ContentType = "application/json";
                context.Response.Write(redirectUrl);
                context.Response.Flush();
                context.ApplicationInstance.CompleteRequest();//将拼接的url返回至context

                return;
            }
        }

        private static void CallController(HttpContext context)
        {
            string controller_name = "";
            if (context.Request["ctrl"] == null)
            {
                controller_name = "default" + "Controller";
            }
            else
            {
                controller_name = context.Request["ctrl"] + "Controller";//获取控制器
            }
            string nspace = "Web.Control";//可在web.config中配置
            Assembly res = null;//命名空间程序集对象
            IController controller = null;//用于接收控制器
            res = GetNamespace();//获取程序集对象
            if (res != null)
            {
                controller = (IController)res.CreateInstance(nspace + "." + controller_name);//通过借口创建控制器实例
                if (controller != null)
                {
                    controller.ActionPerform(context);
                }
            }

        }
        private static Assembly GetNamespace()//获取当前执行代码的程序集，只能获取自己写的命名空间对象程序集，不能获取加载的
        {
            Assembly ns = System.Reflection.Assembly.GetExecutingAssembly();
            return ns;
        }
        private static Assembly GetNamespace(string nspace, Assembly res)//穷举所有的加载类，获得程序集对象，灵活，可以动态从dll加载
        {
            try
            {
                res = (Assembly)CacheHelper.GetCache("nss");
                if (res != null)//有缓存直接获取缓存对象
                {
                    return res;
                }
                else
                {
                    Assembly[] nss = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var e in nss)
                    {
                        foreach (Type t in e.GetTypes())
                        {
                            if (t.Namespace == nspace)
                            {
                                res = t.Assembly;
                                break;
                            }
                        }
                    }
                    if (res != null)
                    {
                        CacheHelper.SetCache("nss", res);//将值插入缓存，避免再次穷举
                    }
                    return res;
                }
            }
            catch (Exception)
            {
                return res;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}