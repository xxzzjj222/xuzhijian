using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Model
{
    public class Role
    {
        public Role() { }

        #region Model

        private int? _id;
        private string _name;
        /// <summary>
        /// ID
        /// </summary>
        public int? ID
        {
            get { return _id;}
            set { this._id = value; }
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { this._name = value; }
        }

        #endregion
    }
}