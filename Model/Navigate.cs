using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Model
{
    public class Navigate
    {
        public Navigate() { }
        #region Model
        private int _id;
        private string _navigatetitle;
        private string _navigateUrl;
        private int _sort;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string NavigateTitle
        {
            get
            {
                return _navigatetitle;
            }
            set
            {
                _navigatetitle = value;
            }
        }
        public string NavigateUrl
        {
            get
            {
                return _navigateUrl;
            }
            set 
            {
                _navigateUrl = value;            
            }
        }
        public int Sort
        {
            get 
            {
                return _sort;
            }
            set
            {
                _sort = value;
            }
        }
        #endregion
    }
}