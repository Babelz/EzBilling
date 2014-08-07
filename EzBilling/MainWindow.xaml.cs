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
using EzBilling.Database;
using EzBilling.Database.Objects;

namespace EzBilling
{
    public partial class MainWindow : Window
    {
        #region Vars
        private readonly ClientInformationWindow clientInfoWindow;
        private readonly CompanyInformationWindow companyInfoWindow;

        private readonly XmlDatabaseConnection database;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            database = new XmlDatabaseConnection(string.Format(@"{0}\db.xml", AppDomain.CurrentDomain.BaseDirectory));
            database.OpenConnection();

            clientInfoWindow = new ClientInformationWindow(database);
            companyInfoWindow = new CompanyInformationWindow(database);

            clientInfoWindow.Closing += new CancelEventHandler(clientInfoWindow_Closing);
            companyInfoWindow.Closing += new CancelEventHandler(companyInfoWindow_Closing);
        }

        #region Child window event handlers
        private void companyInfoWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!IsVisible)
            {
                return;
            }

            e.Cancel = true;
            companyInfoWindow.Hide();
        }
        private void clientInfoWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!IsVisible)
            {
                return;
            }

            e.Cancel = true;
            clientInfoWindow.Hide();
        }
        #endregion

        #region Window event handlers
        private void Window_Closed(object sender, EventArgs e)
        {
            clientInfoWindow.Close();
            companyInfoWindow.Close();
        }
        #endregion

        #region Manage menu event handlers
        private void editCompanyInfos_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            companyInfoWindow.Show();
            companyInfoWindow.Topmost = true;
            companyInfoWindow.Focus();
            companyInfoWindow.Activate();
        }
        private void editClientInfos_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            clientInfoWindow.Show();
            clientInfoWindow.Topmost = true;
            clientInfoWindow.Focus();
            clientInfoWindow.Activate();
        }
        #endregion
    }
}
