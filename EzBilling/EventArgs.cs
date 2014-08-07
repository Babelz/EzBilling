using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EzBilling
{
    public sealed class XmlDatabaseEventArgs : EventArgs
    {
        #region Properties
        public XElement Item
        {
            get;
            private set;
        }
        public string Key
        {
            get;
            private set;
        }
        #endregion

        public XmlDatabaseEventArgs(XElement item)
            : this(item, string.Empty)
        {
        }
        public XmlDatabaseEventArgs(XElement item, string key)
            : base()
        {
            Item = item;
            Key = key;
        }
    }
}
