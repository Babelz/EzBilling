using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EzBilling.Database;
using System.Windows.Controls;
using EzBilling.Database.Serialization;

namespace EzBilling
{
    /// <summary>
    /// Class that handles things that are common with information windows.
    /// </summary>
    public sealed class InformationWindowHandler
    {
        #region Vars
        private readonly XmlDatabaseConnection database;
        private readonly Window owner;
        private readonly DatabaseObjectSerializer serializer;
        private readonly ComboBox itemsComboBox;
        #endregion
        
        public InformationWindowHandler(Window owner, XmlDatabaseConnection database, DatabaseObjectSerializer serializer, ComboBox itemsComboBox)
        {
            this.owner = owner;
            this.database = database;
            this.serializer = serializer;
            this.itemsComboBox = itemsComboBox;
        }

        public void OnClear(Action resetFieldsMethod)
        {
            MessageBoxResult results = MessageBox.Show("Haluatko varmasti tyhjentää kaikki kentät?", owner.Title, MessageBoxButton.YesNo);

            if (results == MessageBoxResult.Yes)
            {
                resetFieldsMethod();
            }
        }
        public void OnDelete(string itemName, string rootKey, Action resetFieldsMethod)
        {
            MessageBoxResult result = MessageBox.Show(string.Format("Haluatko varmasti poistaa {0} tiedot?", itemName), owner.Title, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                database.Remove(rootKey, database.FindItem(rootKey, i => i.Attribute("Name").Value == (string)itemsComboBox.SelectedItem));

                itemsComboBox.Items.Remove(itemsComboBox.SelectedItem);

                resetFieldsMethod();

                database.Save();
            }
        }
        public bool OnSave<T>(T information, TextBox nameTextBox, List<T> informations, Action<T> assingMethod, string itemFoundMessage, string itemSavedMessage, string rootKey) where T : class, new()
        {
            MessageBoxResult? result = null;
            bool overwrite = false;

            if (itemsComboBox.SelectedIndex != -1)
            {
                overwrite = nameTextBox.Text == (string)itemsComboBox.SelectedItem;
            }

            if (overwrite)
            {
                result = MessageBox.Show(itemFoundMessage, owner.Title, MessageBoxButton.YesNo);
            }

            if (result == MessageBoxResult.Yes || !overwrite)
            {
                information = information ?? new T();

                assingMethod(information);

                if (!overwrite)
                {
                    informations.Add(information);
                    database.Add(rootKey, serializer.Serialize<T>(information));
                    database.Save();
                }

                MessageBox.Show(itemSavedMessage, owner.Title, MessageBoxButton.OK);

                return true;
            }

            return false;
        }
        public void OnSelectedItemChanged<T>(Button deleteButton, T information, Action<T> assingMethod) where T : class
        {
            deleteButton.IsEnabled = itemsComboBox.SelectedIndex != -1;

            if (information == null)
            {
                return;
            }

            assingMethod(information);
        }
    }
}
