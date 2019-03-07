using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Plugin
{
    public class Upload
    {
        public static string UploadImage(HttpPostedFile file)
        {
            string result;
            var filecombin = file.FileName.Split('.');
            if (file == null || String.IsNullOrEmpty(file.FileName) || file.ContentLength == 0 || filecombin.Length < 2)
            {
                result = "{\"code\":1,\"message\":\"上传失败\"}";
                return result;
            }
            //定义本地路径位置
            string local = "Blue\\Images";
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, local);

            var tmpName =  System.Web.HttpContext.Current.Server.MapPath("~/Blue/Images/");
            var tmp = file.FileName;
            var tmpIndex = 0;
            //判断是否存在相同文件名的文件 相同累加1继续判断
            while (System.IO.File.Exists(tmpName + tmp))
            {
                tmp = filecombin[0] + "_" + ++tmpIndex + "." + filecombin[1];
            }

            //不带路径的最终文件名
            string filePathName = tmp;

            if (!System.IO.Directory.Exists(localPath))
                System.IO.Directory.CreateDirectory(localPath);
            file.SaveAs(Path.Combine(localPath, filePathName));   //保存图片（文件夹

            result = "{\"code\":0,\"message\":\"上传成功\"}";
            return result;
        }
    }
}