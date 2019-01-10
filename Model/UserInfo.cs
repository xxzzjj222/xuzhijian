using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model
{
    public class UserInfo
    {
        public UserInfo() { }
        #region Model
        private int? _id;
        private string _username;
        private string _password;


        /// <summary>
        /// ID
        /// </summary>
        public int? ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion
    }
}
