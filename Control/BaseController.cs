using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;
using Web.Plugin;
using System.Text;
using System.Reflection;


namespace Web.Control
{
    public class BaseController:IController,IHttpHandler
    {
        protected static SqlSugarClient db = Web.DAL.SugarDao.Instance;//获取数据库静态连接
        protected static VelocityHelper vh = new VelocityHelper(@"~/Templates");//静态模板对象
        protected HttpContext ctx = null;//http上下文对象

        public ActionResult result = new ActionResult();//返回结果集
        private string errormsg = "";//错误提示信息
        private string noauthmsg = "";//无授权提示信息
        public virtual void ProcessRequest(HttpContext context)//只是预留方便统一可以直接处理结果，一般不用
        {
        }
        public BaseController()
        {
        }
        public virtual void View()
        {
            //一般这里什么都没有
        }
        public virtual void DefaultAction()//默认动作
        {
            View();
        }
        protected virtual void ErrorAction()//出错发生动作
        {
            WriteBackHtml(errormsg, false);
        }
        public virtual ActionResult ActionPerform(HttpContext context)//控制器调用的入口方法
        {
            try
            {
                if (context != null)
                {
                    ctx = context;//获取http内容上下文
                }
                Type t = this.GetType();
                string actionName = context.Request["action"];//获取动作信息
                if (string.IsNullOrEmpty(actionName))
                {
                    actionName = "DefaultAction";
                }
                MethodInfo me = t.GetMethod(actionName);//查询控制器中是否有对应的action
                if (me != null)
                {
                    me.Invoke(this, null);//有则通过反射调用对应的action
                }
                else
                {
                    errormsg = string.Format("没有{0}这个动作方法", actionName);//没有则输出错误信息
                    ErrorAction();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// 调用WriteJsonBack
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="flag"></param>
        protected static void WriteBackHtml(HttpContext context,string msg,bool flag)
        {
            string success = "true";
            if (!flag)
            {
                success = "false";
            }
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("success", success);
            result.Add("msg", msg);
            string strJson = Json.ToJson(result);
            WriteJsonBack(context, strJson);
        }
        protected virtual void WriteBackHtml(string msg, bool flag)
        {
            WriteBackHtml(ctx, msg, flag);
        }
        /// <summary>
        /// 向前端ui返回信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="strJson"></param>
        protected static void WriteJsonBack(HttpContext context,string strJson)
        {
            context.Response.Clear();
            context.Response.ContentEncoding=Encoding.UTF8;
            context.Response.ContentType="application/json";
            context.Response.Write(strJson);
            context.Response.Flush();
            context.ApplicationInstance.CompleteRequest();
        }
        protected virtual void WriteJsonBack(string strJson)
        {
            WriteJsonBack(ctx, strJson);
        }
        /// <summary>
        /// 向前端ui返回信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="strHtml"></param>
        protected static void WriteHtmlBack(HttpContext context, string strHtml)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/html";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(strHtml);
            context.ApplicationInstance.CompleteRequest();
        }
        protected virtual void WriteHtmlBack(string strHtml)
        {
            WriteHtmlBack(ctx, strHtml);
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