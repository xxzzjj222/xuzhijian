using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Plugin;


namespace Web.Control
{
    public class defaultController:BaseController
    {
        //点击注销按钮，移除所有的cache
        public void RemoveAllCache()
        {
            Plugin.CacheHelper.RemoveAllCache();
        }
    }
}