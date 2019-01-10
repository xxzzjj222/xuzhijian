using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.Runtime;
using Commons.Collections;
using System.Text;
using System.IO;

namespace Web.Plugin
{
    /// <summary>
    /// NVelocity模板工具类
    /// </summary>
    public class VelocityHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templateDir">模板文件夹路径</param>
        public VelocityHelper(string templateDir)
        {
            Init(templateDir);
        }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public VelocityHelper() 
        { }
        /// <summary>
        /// 初始化NVelocity模板
        /// </summary>
        /// <param name="templateDir"></param>
        public void Init(string templateDir)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH,MapPath.GetPath(templateDir));
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");

            //模板的缓存设置
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true);//是否缓存
            props.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)30);//缓存时间(秒)

            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }
        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(string key, object value)
        {
            if (context == null)
            {
                context = new VelocityContext();
            }
            if (value == null)
            {
                value = " ";
            }
            context.Put(key, value);
        }
        /// <summary>
        /// 显示模板
        /// </summary>
        /// <param name="httpcontext"></param>
        /// <param name="templateFileName"></param>
        public void Display(HttpContext httpcontext,string templateFileName)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templateFileName);
            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            //输出
            httpcontext.Response.Clear();
            httpcontext.Response.Write(writer.ToString());
            httpcontext.Response.Flush();
            httpcontext.ApplicationInstance.CompleteRequest();
        }
        /// <summary>
        /// 根据模板生成静态页面
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateHtml(string templateFileName, string htmlpath)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templateFileName);
            //合并模板
            StringWriter write = new StringWriter();
            template.Merge(context, write);
            using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
            {
                write2.Write(write);
                write2.Flush();
                write2.Close();
            }
        }
        /// <summary>
        /// 向前端ui返回信息
        /// </summary>
        /// <param name="httpcontext"></param>
        /// <param name="strJson"></param>
        public void WriteJson(HttpContext httpcontext, string strJson)
        {
            httpcontext.Response.Clear();
            httpcontext.Response.ContentEncoding = Encoding.UTF8;
            httpcontext.Response.ContentType = "application/json";
            httpcontext.Response.Write(strJson);
            httpcontext.Response.Flush();
            httpcontext.ApplicationInstance.CompleteRequest();
        }
        /// <summary>
        /// 向前端ui返回信息,按照内容类型排列的 Mime 类型列表中的写法来进行
        /// 参考格式：http://www.cnblogs.com/huanhuan86/archive/2012/06/12/2546362.html
        /// </summary>
        /// <param name="httpcontext"></param>
        /// <param name="strHtml"></param>
        public void WriteHtml(HttpContext httpcontext, string strHtml)
        {
            httpcontext.Response.Clear();
            httpcontext.Response.ContentEncoding = Encoding.UTF8;
            httpcontext.Response.ContentType = "text/html";
            httpcontext.Response.Write(strHtml);
            httpcontext.Response.Flush();
            httpcontext.ApplicationInstance.CompleteRequest();
        }
    }
}