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
using EzBilling.Excel;

namespace EzBilling
{
    /// <summary>
    /// TODO: could split sections to custom controls?
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Vars
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
        public BillInformation SelectedBill
        {
            get;
            set;
        }

        public ProductInformation SelectedProduct
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
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { excelConnection.Close(); };
            AppDomain.CurrentDomain.DomainUnload += (s, e) => { excelConnection.Close(); };

            DataContext = this;

            LoadItemsFromDatabase();

            try
            {
                excelConnection = new ExcelConnection();
                excelConnection.Connect();

                BillWriter writer = new BillWriter(
                    new CompanyInformation()
                    {
                        Name = "Jeesus ja pajarit",
                        Email = "jeesus@nasaretinnakkikiosti.org",
                        City = "Nasaret",
                        BillerName = "Jeesus",
                        BankName = "JEWS INC",
                        BankBIC = "JEWSZZ123",
                        AccountNumber = "FI123 123 123 123",
                        Phone = "044 2758595",
                        PostalCode = "96200",
                        Street = "Sämpylä kuja 12"
                    },
                    new ClientInformation()
                    {
                        City = "Rollo hoodz",
                        Name = "Mynssi sami and da boyz",
                        PostalCode = "9200",
                        Street = "Mynssikuja 12"
                    },
                    new BillInformation()
                    {
                        AdditionalInformation = "Mynsseistä",
                        DueDate = "30.8.2014",
                        Name = "",
                        Products = new List<ProductInformation>()
                            {
                                new ProductInformation()
                                {
                                    Name = "OOGEE Kush",
                                    Quantity = "20",
                                    Unit = "g",
                                    UnitPrice = "25",
                                    VATPercent = "55"
                                },
                                new ProductInformation()
                                {
                                    Name = "Afgan Kush",
                                    Quantity = "20",
                                    Unit = "g",
                                    UnitPrice = "15",
                                    VATPercent = "55"
                                },
                                new ProductInformation()
                                {
                                    Name = "Masta Kush",
                                    Quantity = "20",
                                    Unit = "g",
                                    UnitPrice = "35",
                                    VATPercent = "24"
                                }
                            },
                        Reference = "Mynsseistä"
                    });
                writer.Write(excelConnection.GetWorksheet());
                writer.SaveAsPDF(excelConnection.GetWorksheet(), @"jeesus");
            }
            catch (Exception e)
            {
                excelConnection.Close();

                MessageBox.Show(e.Message);
            }
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

        private void InitializeNewBill()
        {
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

        #region Product section event handlers
        private void clearFields_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void removeSelectedProduct_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void addProduct_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void products_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeSelectedProduct_Button.IsEnabled = products_ListView.SelectedIndex > 0;
        }
        private void productName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            addProduct_Button.IsEnabled = productName_TextBox.Text.Length > 0;
        }
        #endregion

        #region Bill section event handlers
        private void printBill_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void deleteBill_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void newBill_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void saveBill_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void bills_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveBill_Button.IsEnabled = bills_ComboBox.SelectedIndex != -1;
            deleteBill_Button.IsEnabled = bills_ComboBox.SelectedIndex != -1;
            printBill_Button.IsEnabled = bills_ComboBox.SelectedIndex != -1;
        }
        #endregion
    }
}
