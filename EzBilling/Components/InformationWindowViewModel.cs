using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EzBilling.Components
{
    public sealed class InformationWindowViewModel<T> : INotifyPropertyChanged where T : class
    {
        #region Vars
        private T selectetItem;
        private ObservableCollection<T> items;
        #endregion

        #region Properties
        public T SelectedItem
        {
            get
            {
                return selectetItem;
            }
            set
            {
                if (selectetItem != value)
                {
                    selectetItem = value;

                    OnPropertyChanged("SelectedItem");
                }
            }
        }
        public ObservableCollection<T> Items
        {
            get
            {
                return items;
            }
            set
            {
                if (items != value)
                {
                    items = value;

                    OnPropertyChanged("Items");
                }
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public InformationWindowViewModel()
        {
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
