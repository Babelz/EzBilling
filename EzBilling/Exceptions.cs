using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzBilling
{
    public class EzBillingException : Exception
    {
        public EzBillingException(string message)
            : base(message)
        {
        }
    }

    public sealed class XmlDatabaseException : Exception
    {
        public XmlDatabaseException(string message)
            : base(message)
        {
        }
    }

    public sealed class SerializationException : Exception
    {
        public SerializationException(Type type)
            : base(string.Format("Type {0} does not implement DatabaseObject attribute.", type.FullName))
        {
        }
    }
}
