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
        private readonly TextBox[] productSectionInputFields;
        private readonly BillManager billManager;
        private readonly ExcelConnection excelConnection;
        private readonly EzBillingDatabase database;
        private readonly ClientInformationWindow clientInformationWindow;
        private readonly CompanyInformationWindow companyInformationWindow;
        #endregion

        #region Properties
        public InformationWindowViewModel<ClientInformation> ClientViewModel
        {
            get;
            private set;
        }

        public InformationWindowViewModel<CompanyInformation> CompanyViewModel
        {
            get;
            private set;
        }

        public InformationWindowViewModel<BillInformation> BillViewModel
        {
            get;
            private set;
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

            clientInformationWindow = new ClientInformationWindow();
            companyInformationWindow = new CompanyInformationWindow();

            clientInformationWindow.Closing += new CancelEventHandler(window_Closing);
            companyInformationWindow.Closing += new CancelEventHandler(window_Closing);

            excelConnection = new ExcelConnection();

            ClientViewModel = new InformationWindowViewModel<ClientInformation>();
            ClientViewModel.Items = new ObservableCollection<ClientInformation>();

            CompanyViewModel = new InformationWindowViewModel<CompanyInformation>();
            CompanyViewModel.Items = new ObservableCollection<CompanyInformation>();
            CompanyViewModel.PropertyChanged += CompanyViewModel_PropertyChanged;

            ProductViewModel = new InformationWindowViewModel<ProductInformation>();
            ProductViewModel.Items = new ObservableCollection<ProductInformation>();
            ProductViewModel.PropertyChanged += ProductViewModel_PropertyChanged;

            BillViewModel = new InformationWindowViewModel<BillInformation>();
            BillViewModel.Items = new ObservableCollection<BillInformation>();
            BillViewModel.PropertyChanged += BillViewModel_PropertyChanged;

            billManager = new BillManager();

            productSectionInputFields = new TextBox[] 
            {
                productName_TextBox,
                productQuantity_TextBox,
                productUnit_TextBox,
                productUnitPrice_TextBox,
                productVATPercent_TextBox
            };

            DataContext = this;

            LoadItemsFromDatabase();
            excelConnection.Open();

            clientInformationWindow.ClientWindowViewModel.Items = ClientViewModel.Items;
            companyInformationWindow.CompanyWindowViewModel.Items = CompanyViewModel.Items;
        }

        private void CompanyViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                clientAndBillWrapper_Grid.IsEnabled = CompanyViewModel.SelectedItem != null;
            }
        }

        private void ProductViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                bool enableFields = ProductViewModel.SelectedItem != null;

                for (int i = 0; i < productSectionInputFields.Length; i++)
                {
                    productSectionInputFields[i].IsEnabled = enableFields;
                }
            }
        }

        private void BillViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                productSection_Grid.IsEnabled = BillViewModel.SelectedItem != null;
            }
        }

        private void LoadItemsFromDatabase()
        {
            ClientInformations.Clear();
            CompanyViewModel.Items.Clear();

            List<CompanyInformation> companyInfos = database.GetCompanyInformations();
            List<ClientInformation> clientInfos = database.GetClientInformations();

            for (int i = 0; i < companyInfos.Count; i++)
            {
                CompanyViewModel.Items.Add(companyInfos[i]);
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

            if (CompanyViewModel.SelectedItem == null)
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
            BillViewModel.SelectedItem.Products.Remove(products_ListView.SelectedItem as ProductInformation);

            products_ListView.Items.Refresh();
        }
        private void addProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            BillViewModel.SelectedItem.Products.Add(ProductViewModel.SelectedItem);

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
                string path = string.Format("Bills\\{0}\\{1}.pdf", SelectedClientInformation.Name, BillViewModel.SelectedItem.Name);

                if (File.Exists(path))
                {
                    File.Delete(path);

                    // TODO: remove from db.
                }

                BillViewModel.Items.Remove(BillViewModel.SelectedItem);
                BillViewModel.SelectedItem = null;

                MessageBox.Show("Lasku poistettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        private void newBill_Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BillViewModel.SelectedItem = MakeNew();

                if (!BillViewModel.Items.Contains(BillViewModel.SelectedItem))
                {
                    BillViewModel.Items.Add(BillViewModel.SelectedItem);

                    bills_ListView.Items.Refresh();
                }

                products_ListView.Items.Refresh();
            }));
        }
        private void saveBill_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateBill())
            {
                BillViewModel.Items.Add(BillViewModel.SelectedItem);

                billManager.SaveBillAsPDF(excelConnection.GetWorksheet(), CompanyViewModel.SelectedItem, SelectedClientInformation, BillViewModel.SelectedItem);
                excelConnection.ResetWorksheet();

                // TODO: write to database.

                MessageBox.Show("Lasku tallennettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        private void bills_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1;
            deleteBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1 && !string.IsNullOrEmpty(BillViewModel.SelectedItem.Name);
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
