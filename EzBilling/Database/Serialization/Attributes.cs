using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzBilling.Database.Serialization
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DatabaseObjectAttribute : Attribute
    {
        #region Properties
        public string RootKey
        {
            get;
            private set;
        }
        public string ObjectName
        {
            get;
            private set;
        }
        #endregion

        public DatabaseObjectAttribute(string rootKey, string objectName)
            : base()
        {
            RootKey = rootKey;
            ObjectName = objectName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DatabaseObjectFieldAttribute : Attribute
    {
        public DatabaseObjectFieldAttribute()
            : base()
        {
        }
    }
}
