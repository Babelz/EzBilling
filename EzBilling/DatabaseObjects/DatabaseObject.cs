using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EzBilling.DatabaseObjects
{
    public abstract class DatabaseObject
    {
        #region Vars
        private string id;
        #endregion

        #region Properties
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        #endregion

        protected DatabaseObject()
        {
        }

        public abstract void Fill(DataRow info);
    }
}
