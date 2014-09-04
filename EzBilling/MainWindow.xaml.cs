using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using EzBilling.Database;
using System.Collections.ObjectModel;
using EzBilling.Excel;
using System.IO;
using System.Text.RegularExpressions;
using EzBilling.Components;
using EzBilling.Models;
using System.Windows.Documents;
using System.Diagnostics;
using System.Threading;

namespace EzBilling
{
    // TODO: total and vat amount labels wont get new values when changing, removing or adding product informations.
    // TODO: selected client is null some times?
    public partial class MainWindow : Window
    {
        #region Vars
        private readonly TextBox[] productSectionInputFields;
        private readonly BillManager billManager;
        private readonly ExcelConnection excelConnection;
        private readonly CompanyRepository companyRepository;
        private readonly ClientRepository clientRepository;
        private readonly clientWindow clientWindow;
        private readonly companyWindow companyWindow;
        private readonly BillRepository billRepository;

        private EzBillingModel model;
        #endregion

        #region Properties
        public InformationWindowViewModel<Client> ClientViewModel
        {
            get;
            private set;
        }
        public InformationWindowViewModel<Company> CompanyViewModel
        {
            get;
            private set;
        }
        public InformationWindowViewModel<Bill> BillViewModel
        {
            get;
            private set;
        }
        public InformationWindowViewModel<Product> ProductViewModel
        {
            get;
            set;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            model = new EzBillingModel();
            clientRepository = new ClientRepository(model);
            companyRepository = new CompanyRepository(model);
            billRepository = new BillRepository(model);

            excelConnection = new ExcelConnection();
            billManager = new BillManager();

            clientWindow = new clientWindow(clientRepository, billRepository, billManager);
            companyWindow = new companyWindow(companyRepository, billRepository, billManager);

            clientWindow.Closing += new CancelEventHandler(window_Closing);
            companyWindow.Closing += new CancelEventHandler(window_Closing);

            ClientViewModel = new InformationWindowViewModel<Client>();
            ClientViewModel.Items = new ObservableCollection<Client>(clientRepository.All.ToList());

            CompanyViewModel = new InformationWindowViewModel<Company>();
            CompanyViewModel.Items = new ObservableCollection<Company>(companyRepository.All.ToList());
            CompanyViewModel.PropertyChanged += CompanyViewModel_PropertyChanged;

            ProductViewModel = new InformationWindowViewModel<Product>();
            ProductViewModel.Items = new ObservableCollection<Product>();
            ProductViewModel.PropertyChanged += ProductViewModel_PropertyChanged;

            BillViewModel = new InformationWindowViewModel<Bill>();
            BillViewModel.Items = new ObservableCollection<Bill>();
            BillViewModel.PropertyChanged += BillViewModel_PropertyChanged;

            this.Closing += MainWindow_Closing;

            productSectionInputFields = new TextBox[] 
            {
                productName_TextBox,
                productQuantity_TextBox,
                productUnit_TextBox,
                productUnitPrice_TextBox,
                productVATPercent_TextBox
            };

            DataContext = this;

            excelConnection.Open();
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
            else if (ClientViewModel.SelectedItem == null)
            {
                MessageBox.Show("Laskua ei voida luoda koska asiakasta ei ole valittu.", "EzBilling", MessageBoxButton.OK);
            }
            else
            {
                result = true;
            }

            return result;
        }
        private Bill NewBill()
        {
            return new Bill()
            {
                Name = billManager.CreateBillName(ClientViewModel.SelectedItem.Name)
            };
        }
        private void UpdateClientInformations()
        {
            ClientViewModel.Items = new ObservableCollection<Client>(clientRepository.All);

            if (ClientViewModel.SelectedItem != null && !ClientViewModel.Items.Contains(ClientViewModel.SelectedItem))
            {
                ClientViewModel.SelectedItem = null;
            }

            ResetModels();
        }
        private void UpdateCompanyInformations()
        {
            CompanyViewModel.Items = new ObservableCollection<Company>(companyRepository.All);

            if (CompanyViewModel.SelectedItem != null && !CompanyViewModel.Items.Contains(CompanyViewModel.SelectedItem))
            {
                CompanyViewModel.SelectedItem = null;
            }

            ResetModels();
        }
        private void ResetModels()
        {
            if (CompanyViewModel.SelectedItem == null || ClientViewModel.SelectedItem == null)
            {
                ClientViewModel.SelectedItem = null;
                ClientViewModel.Items = new ObservableCollection<Client>(clientRepository.All);

                BillViewModel.SelectedItem = null;
                BillViewModel.Items.Clear();

                ProductViewModel.SelectedItem = null;
                ProductViewModel.Items.Clear();

                bills_ListView.Items.Refresh();
                products_ListView.Items.Refresh();
            }
        }
        private bool OnExiting()
        {
            MessageBoxResult result = MessageBox.Show("Kaikki tallentamattomat tiedot poistuvat, haluatko varmasti sulkea ohjelman?", "EzBilling", MessageBoxButton.YesNo);

            return result == MessageBoxResult.Yes;
        }

        #region Main window event handlers
        /// <summary>
        /// Close all child windows.
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            excelConnection.Close();

            companyWindow.Close();
            clientWindow.Close();
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!OnExiting())
            {
                e.Cancel = true;
            }
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

            if (ReferenceEquals(sender, clientWindow))
            {
                UpdateClientInformations();
            }
            else
            {
                UpdateCompanyInformations();
            }
        }
        #endregion

        #region Menu event handlers
        private void closeProgram_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void about_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simple billing application.\nLicense: WTFPL - www.wtfpl.net", "EzBilling", MessageBoxButton.OK);
        }
        private void editCompanyInfos_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            companyWindow.Show();
            companyWindow.Topmost = true;
            companyWindow.Focus();
            companyWindow.Activate();
        }
        private void editClientInfos_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            clientWindow.Show();
            clientWindow.Topmost = true;
            clientWindow.Focus();
            clientWindow.Activate();
        }
        #endregion

        #region Client section event handlers
        private void clientName_ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            newBill_Button.IsEnabled = clientName_ComboBox.SelectedIndex != -1;

            // Loop until view model selected item gets a value.
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    Thread.Sleep(150);

                    if (ClientViewModel.SelectedItem != null)
                    {
                        BillViewModel.Items = new ObservableCollection<Bill>(ClientViewModel.SelectedItem.Bills);
                        BillViewModel.SelectedItem = null;
                    }

                    bills_ListView.Items.Refresh();
                }));
        }
        #endregion

        #region Product section event handlers
        private void clearFields_Button_Click(object sender, RoutedEventArgs e)
        {
            ProductViewModel.SelectedItem = null;
            ProductViewModel.SelectedItem = new Product();
        }
        private void removeSelectedProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            BillViewModel.SelectedItem.Products.Remove(products_ListView.SelectedItem as Product);

            products_ListView.Items.Refresh();
        }
        private void addProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            if (BillViewModel.SelectedItem.Products.Contains(ProductViewModel.SelectedItem))
            {
                Product selected = ProductViewModel.SelectedItem;

                Product product = new Product()
                {
                    Name = selected.Name,
                    Quantity = selected.Quantity,
                    Unit = selected.Unit,
                    UnitPrice = selected.UnitPrice,
                    VATPercent = selected.VATPercent,
                    ProductID = 0
                };

                BillViewModel.SelectedItem.Products.Add(product);
            }
            else
            {
                BillViewModel.SelectedItem.Products.Add(ProductViewModel.SelectedItem);
            }

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
            Bill bill = BillViewModel.SelectedItem;

            FileInfo fileInfo = billManager.ResolveBill(ClientViewModel.SelectedItem.Name, bill.Name);

            if (!fileInfo.Exists)
            {
                MessageBox.Show("Lasku pitää tallentaa ennen tulostamista.");
                return;
            }
            else
            {
                // Write changes.
                billManager.SaveBillAsPDF(excelConnection.GetWorksheet(), CompanyViewModel.SelectedItem, ClientViewModel.SelectedItem, bill);
                excelConnection.ResetWorksheet();

                clientRepository.InsertOrUpdate(ClientViewModel.SelectedItem);
                clientRepository.Save();
            }

            ProcessStartInfo info = new ProcessStartInfo(fileInfo.FullName);
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process.Start(info);
        }
        private void deleteBill_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa laskun tiedot? Tietoja ei voi palauttaa.", "EzBilling", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                string path = string.Format("Bills\\{0}\\{1}.pdf", ClientViewModel.SelectedItem.Name, BillViewModel.SelectedItem.Name);

                Client curClient = ClientViewModel.SelectedItem;
                Bill curBill = BillViewModel.SelectedItem;

                for (int i = 0; i < curBill.Products.Count; i++)
                {
                    curBill.Products.Remove(curBill.Products[i]);
                    clientRepository.InsertOrUpdate(curClient);
                    clientRepository.Save();
                }

                billManager.RemoveKnownBill(curBill.Name);

                curClient.Bills.Remove(curBill);
                billRepository.Delete(curBill);
                billRepository.Save();

                clientRepository.InsertOrUpdate(curClient);
                clientRepository.Save();

                BillViewModel.Items.Remove(BillViewModel.SelectedItem);
                BillViewModel.SelectedItem = null;

                MessageBox.Show("Lasku poistettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        private void newBill_Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BillViewModel.SelectedItem = NewBill();

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
                billManager.SaveBillAsPDF(excelConnection.GetWorksheet(), CompanyViewModel.SelectedItem, ClientViewModel.SelectedItem, BillViewModel.SelectedItem);
                excelConnection.ResetWorksheet();

                BillViewModel.SelectedItem.CompanyID = CompanyViewModel.SelectedItem.CompanyID;
                BillViewModel.SelectedItem.Client = ClientViewModel.SelectedItem;
                BillViewModel.SelectedItem.ClientID = ClientViewModel.SelectedItem.ClientId;

                ClientViewModel.SelectedItem.Bills.Add(BillViewModel.SelectedItem);
                clientRepository.InsertOrUpdate(ClientViewModel.SelectedItem);
                clientRepository.Save();

                ClientViewModel.SelectedItem.Bills.Remove(BillViewModel.SelectedItem);
                BillViewModel.SelectedItem = billRepository.First(b => b.Name == BillViewModel.SelectedItem.Name);
                ClientViewModel.SelectedItem.Bills.Add(BillViewModel.SelectedItem);

                MessageBox.Show("Lasku tallennettu.", "EzBilling", MessageBoxButton.OK);
            }
        }
        private void bills_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1;
            deleteBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1 && !string.IsNullOrEmpty(BillViewModel.SelectedItem.Name);
            printBill_Button.IsEnabled = bills_ListView.SelectedIndex != -1;
        }
        private void product_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addProduct_Button.IsEnabled = productName_TextBox.Text.Length > 0 &&
                                          productQuantity_TextBox.Text.Length > 0 &&
                                          productUnitPrice_TextBox.Text.Length > 0 &&
                                          productVATPercent_TextBox.Text.Length > 0;

            TextBox textBox = sender as TextBox;

            int dotcount = textBox.Text.Count(c => c == '.');

            if (dotcount > 1)
            {
                AsyncUndo(textBox);
                return;
            }

            for (int i = 0; i < textBox.Text.Length; i++)
            {
                if (!(char.IsDigit(textBox.Text[i]) || textBox.Text[i] == '.'))
                {
                    AsyncUndo(textBox);
                    return;
                }
            }
        }
        #endregion
    }
}
