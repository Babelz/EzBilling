using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace EzBilling.Database.Serialization
{
    public sealed class DatabaseObjectSerializer
    {
        public DatabaseObjectSerializer()
        {
        }

        private DatabaseObjectAttribute GetDatabaseObjectAttribute(Type type)
        {
            object attribute = type.GetCustomAttributes(typeof(DatabaseObjectAttribute), true).FirstOrDefault();

            if (attribute == null)
            {
                throw new SerializationException(type);
            }

            return attribute as DatabaseObjectAttribute;
        }
        private List<PropertyInfo> GetDataProperties(Type type)
        {
            List<PropertyInfo> dataProperties = new List<PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                object attribute = properties[i].GetCustomAttributes(typeof(DatabaseObjectFieldAttribute), true);

                if (attribute == null)
                {
                    continue;
                }

                dataProperties.Add(properties[i]);
            }

            return dataProperties;
        }

        public List<T> Deserialize<T>(List<XElement> xElements) where T : class, new ()
        {
            List<T> deserializedObjects = new List<T>();

            for (int i = 0; i < xElements.Count; i++)
            {
                T deserializedObject = Deserialize<T>(xElements[i]);
                deserializedObjects.Add(deserializedObject);
            }

            return deserializedObjects;
        }
        public List<XElement> Serialize<T>(List<T> databaseObjects) where T : class
        {
            List<XElement> serializedObjects = new List<XElement>();

            for (int i = 0; i < databaseObjects.Count; i++)
            {
                XElement serializedObject = Serialize<T>(databaseObjects[i]);
                serializedObjects.Add(serializedObject);
            }

            return serializedObjects;
        }
        public T Deserialize<T>(XElement xElement) where T : class, new()
        {
            Type type = typeof(T);

            DatabaseObjectAttribute attribute = GetDatabaseObjectAttribute(type);
            List<PropertyInfo> dataProperties = GetDataProperties(type);

            T deserializedObject = new T();

            for (int i = 0; i < dataProperties.Count; i++)
            {
                XAttribute xAttribute = xElement.Attributes().FirstOrDefault(a => a.Name == dataProperties[i].Name);

                if (xAttribute == null)
                {
                    continue;
                }

                dataProperties[i].SetValue(deserializedObject, xAttribute.Value, null);
            }

            return deserializedObject;
        }
        public XElement Serialize<T>(T databaseObject) where T : class
        {
            Type type = typeof(T);

            DatabaseObjectAttribute attribute = GetDatabaseObjectAttribute(type);
            List<PropertyInfo> dataProperties = GetDataProperties(type);

            XElement xElement = new XElement(attribute.ObjectName);

            for (int i = 0; i < dataProperties.Count; i++)
            {
                xElement.SetAttributeValue(dataProperties[i].Name, dataProperties[i].GetValue(databaseObject, null).ToString());
            }

            return xElement;
        }
    }
}
