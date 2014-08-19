using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EzBilling.Components;
using System.Windows.Controls;
using System.Windows;

namespace EzBilling
{
    public sealed class InformationWindowController<T> where T : class
    {
        #region Contants
        private const string MSG_BOX_CAPT = "EzBilling";
        #endregion

        #region Vars
        private readonly InformationWindowViewModel<T> viewModel;
        private readonly ComboBox items_ComboBox;
        private readonly TextBox[] inputFields;
        #endregion

        public InformationWindowController(InformationWindowViewModel<T> viewModel, ComboBox items_ComboBox, TextBox[] inputFields)
        {
            this.viewModel = viewModel;
            this.items_ComboBox = items_ComboBox;
            this.inputFields = inputFields;
        }

        public void ResetFields()
        {
            for (int i = 0; i < inputFields.Length; i++)
            {
                inputFields[i].Clear();
            }

            items_ComboBox.SelectedIndex = -1;
        }
        public void DeleteInformation(string displayString, Action<T> removeFromDatabase)
        {
            MessageBoxResult result = MessageBox.Show(displayString, MSG_BOX_CAPT, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.Items.Remove(viewModel.SelectedItem);

                removeFromDatabase(viewModel.SelectedItem);

                viewModel.SelectedItem = null;

                ResetFields();

                MessageBox.Show("Tiedot poistettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        public void AddInformation(string displayString, Action<T> addToDatabase, T item)
        {
            MessageBox.Show(displayString, MSG_BOX_CAPT, MessageBoxButton.OK);

            viewModel.Items.Add(item);
            viewModel.SelectedItem = item;

            addToDatabase(item);

            items_ComboBox.SelectedIndex = items_ComboBox.Items.IndexOf(item);
        }
    }
}
