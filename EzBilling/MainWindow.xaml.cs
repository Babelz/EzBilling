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
using System.Collections.ObjectModel;
using EzBilling.Excel;
using System.IO;
using System.Text.RegularExpressions;
using EzBilling.Components;

namespace EzBilling
{
    /// <summary>
    /// TODO: could split sections to custom controls?
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Vars
        private readonly BillManager billManager;
        private readonly ExcelConnection excelConnection;
        private readonly EzBillingDatabase database;
        private readonly ClientInformationWindow clientInformationWindow;
        private readonly CompanyInformationWindow companyInformationWindow;
        #endregion

        #region Properties
        public ObservableCollection<ClientInformation> ClientInformations
        {
            get;
            set;
        }
        public ClientInformation SelectedClientInformation
        {
            get;
            set;
        }
        
        public ObservableCollection<CompanyInformation> CompanyInformations
        {
            get;
            set;
        }
        public CompanyInformation SelectedCompanyInformation
        {
            get;
            set;
        }

        public ObservableCollection<BillInformation> BillInformations
        {
            get;
            set;
        }
        public BillInformation SelectedBillInformation
        {
            get;
            set;
        }

        public InformationWindowViewModel<ProductInformation> ProductViewModel
        {
            get;
            set;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            database = new EzBillingDatabase();

            ClientInformations = new ObservableCollection<ClientInformation>();
            CompanyInformations = new ObservableCollection<CompanyInformation>();
            BillInformations = new ObservableCollection<BillInformation>();

            clientInformationWindow = new ClientInformationWindow();
            companyInformationWindow = new CompanyInformationWindow();

            clientInformationWindow.Closing += new CancelEventHandler(window_Closing);
            companyInformationWindow.Closing += new CancelEventHandler(window_Closing);

            excelConnection = new ExcelConnection();
            DataContext = this;

            LoadItemsFromDatabase();
            excelConnection.Open();

            SelectedBillInformation = new BillInformation();

            ProductViewModel = new InformationWindowViewModel<ProductInformation>();
            ProductViewModel.SelectedItem = new ProductInformation();

            billManager = new BillManager();
        }

        private void LoadItemsFromDatabase()
        {
            ClientInformations.Clear();
            CompanyInformations.Clear();

            List<CompanyInformation> companyInfos = database.GetCompanyInformations();
            List<ClientInformation> clientInfos = database.GetClientInformations();

            for (int i = 0; i < companyInfos.Count; i++)
            {
                CompanyInformations.Add(companyInfos[i]);
            }
            for (int i = 0; i < clientInfos.Count; i++)
            {
                ClientInformations.Add(clientInfos[i]);
            }
        }
        private void AsyncUndo(TextBox textBox)
        {
            Dispatcher.BeginInvoke(new Action(() => { textBox.Undo(); }));
        }
        private bool ValidateBill()
        {
            bool result = false;

            if (SelectedCompanyInformation == null)
            {
                MessageBox.Show("Laskua ei voida luoda koska laskuttavaa yritystä ei ole valittu.", "EzBilling", MessageBoxButton.OK);
            }
            else if (SelectedClientInformation == null)
            {
                MessageBox.Show("Laskua ei voida luoda koska asiakasta ei ole valittu.", "EzBilling", MessageBoxButton.OK);
            }
            else
            {
                result = true;
            }

            return result;
        }
        private BillInformation MakeNew()
        {
            return new BillInformation()
            {
                Name = billManager.CreateBillName(SelectedClientInformation.Name)
            };
        }

        #region Main window event handlers
        /// <summary>
        /// Close all child windows.
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            excelConnection.Close();

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
        private void closeProgram_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO: wtf?
        }
        private void about_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // TODO: display about menu.
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

        #region Client section event handlers
        private void clientName_ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // TODO: get bills for selected client.
            newBill_Button.IsEnabled = clientName_ComboBox.SelectedIndex != -1;
        }
        #endregion

        #region Product section event handlers
        private void clearFields_Button_Click(object sender, RoutedEventArgs e)
        {
            ProductViewModel.SelectedItem = null;
            ProductViewModel.SelectedItem = new ProductInformation();
        }
        private void removeSelectedProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedBillInformation.Products.Remove(products_ListView.SelectedItem as ProductInformation);

            products_ListView.Items.Refresh();
        }
        private void addProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedBillInformation.Products.Add(ProductViewModel.SelectedItem);

            products_ListView.Items.Refresh();
        }
        private void products_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeSelectedProduct_Button.IsEnabled = products_ListView.SelectedIndex > 0;
        }
        #endregion

        #region Bill section event handlers
        private void printBill_Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: print current bill.
        }
        private void deleteBill_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa laskun tiedot? Tietoja ei voi palauttaa.", "EzBilling", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                string path = string.Format("Bills\\{0}\\{1}.pdf", SelectedClientInformation.Name, SelectedBillInformation.Name);

                File.Delete(path);

                MessageBox.Show("Lasku poistettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        private void newBill_Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!SelectedBillInformation.IsEmpty())
                {
                    MessageBoxResult result = MessageBox.Show("Kaikki tallentamattomat tiedot häviävät, haluatko jatkaa?", "EzBilling", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        SelectedBillInformation = MakeNew();
                    }
                }
                else
                {
                    SelectedBillInformation = MakeNew();
                }

                if (!BillInformations.Contains(SelectedBillInformation))
                {
                    BillInformations.Add(SelectedBillInformation);

                    bills_ListView.Items.Refresh();
                }
            }));
        }
        private void saveBill_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateBill())
            {
                BillInformations.Add(SelectedBillInformation);

                billManager.SaveBillAsPDF(excelConnection.GetWorksheet(), SelectedCompanyInformation, SelectedClientInformation, SelectedBillInformation);
                excelConnection.ResetWorksheet();

                // TODO: write to database.

                MessageBox.Show("Lasku tallennettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        private void bills_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1;
            deleteBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1 && !string.IsNullOrEmpty(SelectedBillInformation.Name);
            printBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1;
        }
        private void productInformation_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addProduct_Button.IsEnabled = productName_TextBox.Text.Length > 0 &&
                                          productQuantity_TextBox.Text.Length > 0 &&
                                          productUnitPrice_TextBox.Text.Length > 0 &&
                                          productVATPercent_TextBox.Text.Length > 0;

            TextBox textBox = sender as TextBox;

            int dotcount = textBox.Text.Count(c => c == ',');

            if (dotcount > 1)
            {
                AsyncUndo(textBox);
                return;
            }

            for (int i = 0; i < textBox.Text.Length; i++)
            {
                if (!(char.IsDigit(textBox.Text[i]) || textBox.Text[i] == ','))
                {
                    AsyncUndo(textBox);
                    return;
                }
            }
        }
        #endregion
    }
}
