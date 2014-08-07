using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace EzBilling.Database
{
    public sealed class XmlDatabaseConnection
    {
        #region Vars
        private readonly string databasePath;
        
        private XDocument database;
        #endregion

        #region Properties
        public bool Connected
        {
            get
            {
                return database != null;
            }
        }
        #endregion

        #region Events
        public event XmlDatabaseEventHandler ItemRemoved;
        public event XmlDatabaseEventHandler ItemAdded;
        #endregion

        public XmlDatabaseConnection(string databasePath)
        {
            this.databasePath = databasePath;
        }

        private void OnItemAdded(object sender, XmlDatabaseEventArgs e)
        {
            if (ItemAdded != null)
            {
                ItemAdded(sender, e);
            }
        }
        private void OnItemRemoved(object sender, XmlDatabaseEventArgs e)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(sender, e);
            }
        }

        private void CheckFile()
        {
            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException("Database not found.");
            }
        }
        private XmlDatabaseException InvalidRootKey(string rootKey)
        {
            return new XmlDatabaseException(string.Format("Root key '{0}' was not found in database.", rootKey));
        }
        private XmlDatabaseException ItemNotFound()
        {
            return new XmlDatabaseException("Database does not contain given item.");
        }

        public void Remove(string rootKey, XElement item)
        {
            // Get the root.
            XElement rootElement = FindItemRoot(rootKey);

            if (rootElement == null)
            {
                throw InvalidRootKey(rootKey);
            }

            if (!rootElement.Elements().Contains(item))
            {
                throw ItemNotFound();
            }

            // Remove the object.
            item.Remove();

            OnItemRemoved(this, new XmlDatabaseEventArgs(item, rootKey));
        }
        /// <summary>
        /// Adds the given element to the database. No validation needed if element has been
        /// created using serializer.
        /// </summary>
        /// <param name="rootKey">Where to add this element.</param>
        /// <param name="item">Element to add.</param>
        public void Add(string rootKey, XElement item)
        {
            XElement rootElement = FindItemRoot(rootKey);

            if (rootElement == null)
            {
                throw InvalidRootKey(rootKey);
            }

            rootElement.Add(item);

            OnItemAdded(this, new XmlDatabaseEventArgs(item));
        }
        public XElement FindItemRoot(string rootKey)
        {
            return database.Root.Elements()
                .FirstOrDefault(e => e.Name == rootKey);
        }
        public XElement FindItem(string rootKey, Predicate<XElement> predicate)
        {
            return FindItemRoot(rootKey).Elements()
                .FirstOrDefault(e => predicate(e));
        }

        public void OpenConnection()
        {
            if (Connected)
            {
                return;
            }

            CheckFile();

            database = XDocument.Load(databasePath);
        }
        public void CloseConnection()
        {
            if (!Connected)
            {
                return;
            }

            CheckFile();

            database.Save(databasePath);
            database = null;
        }
        public void Save()
        {
            if (!Connected)
            {
                return;
            }

            CheckFile();

            database.Save(databasePath);
        }
    }
}
