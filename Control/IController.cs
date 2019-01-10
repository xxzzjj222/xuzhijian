using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Control
{
    interface IController
    {
        ActionResult ActionPerform(HttpContext context);
    }

    public class ActionResult
    {
        public Dictionary<string,string> strresult=new Dictionary<string,string>();
        public Dictionary<string,object> strobject=new Dictionary<string,object>();
    }
}