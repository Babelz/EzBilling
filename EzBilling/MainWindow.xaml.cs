using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using EzBilling.DatabaseObjects;
using System.Collections.ObjectModel;

namespace EzBilling
{
    public partial class MainWindow : Window
    {
        #region Vars
        private readonly ClientInformationWindow clientInformationWindow;
        private readonly CompanyInformationWindow companyInformationWindow;
        #endregion

        #region Properties
        public ObservableCollection<ClientInformation> ClientInformations
        {
            get;
            private set;
        }
        public ClientInformation SelectedClientInformation
        {
            get;
            private set;
        }
        public ObservableCollection<CompanyInformation> CompanyInformations
        {
            get;
            private set;
        }
        public CompanyInformation SelectedCompanyInformation
        {
            get;
            private set;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            ClientInformations = new ObservableCollection<ClientInformation>();
            CompanyInformations = new ObservableCollection<CompanyInformation>();

            clientInformationWindow = new ClientInformationWindow();
            companyInformationWindow = new CompanyInformationWindow();

            clientInformationWindow.Closing += new CancelEventHandler(window_Closing);
            companyInformationWindow.Closing += new CancelEventHandler(window_Closing);

            DataContext = this;
        }

        #region Event handlers
        /// <summary>
        /// Close all child windows.
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            companyInformationWindow.Close();
            clientInformationWindow.Close();
        }
        #endregion

        #region Child window event handlers
        /// <summary>
        /// Prevent child window from closing if main window has not exited.
        /// </summary>
        private void window_Closing(object sender, CancelEventArgs e)
        {
            Window window = sender as Window;

            if (IsActive)
            {
                return;
            }

            e.Cancel = true;
            window.Hide();
        }
        #endregion

        #region Menu event handlers
        private void loadBill_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void newBill_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void saveBill_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void printBill_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void closeProgram_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void about_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void editCompanyInfos_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            companyInformationWindow.Show();
            companyInformationWindow.Topmost = true;
            companyInformationWindow.Focus();
            companyInformationWindow.Activate();
        }
        private void editClientInfos_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            clientInformationWindow.Show();
            clientInformationWindow.Topmost = true;
            clientInformationWindow.Focus();
            clientInformationWindow.Activate();
        }
        #endregion
    }
}
