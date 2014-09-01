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
using System.Windows.Shapes;
using EzBilling.Components;
using System.Collections.ObjectModel;
using EzBilling.Database;
using EzBilling.Models;

namespace EzBilling
{
    public partial class CompanyInformationWindow : Window
    {
        #region Constants
        // Dict keys.
        private const string NAME = "Name";
        private const string STREET = "Street";
        private const string CITY = "City";
        private const string POSTALCODE = "PostalCode";
        private const string ID = "ID";
        private const string ACCOUNTNUMBER = "AccountNumber";
        private const string BANKNAME = "BankName";
        private const string BANKBIC = "BankBIC";
        private const string BILLERNAME = "BillerName";
        private const string PHONENUMBER = "Phone";
        private const string EMAILADDRESS = "EmailAddress";
        #endregion

        #region Vars
        private readonly InformationWindowController<Company> controller;

        private readonly CompanyRepository database;
        #endregion

        #region Properties
        public InformationWindowViewModel<Company> CompanyWindowViewModel
        {
            get;
            private set;
        }
        #endregion

        public CompanyInformationWindow()
        {
            CompanyWindowViewModel = new InformationWindowViewModel<Company>();
            CompanyWindowViewModel.Items = new ObservableCollection<Company>();

            InitializeComponent();

            DataContext = this;

            controller = new InformationWindowController<Company>(CompanyWindowViewModel, companies_ComboBox,
                new TextBox[]
            {
                companyName_TextBox,
                companyStreet_TextBox,
                companyCity_TextBox,
                companyPostalCode_TextBox,
                companyID_TextBox,
                companyAccountNumber_TextBox,
                companyBankName_TextBox,
                companyBankBIC_TextBox,
                companyBillerName_TextBox,
                companyPhoneNumber_TextBox,
                companyEmailAddress_TextBox
            });

            database = new CompanyRepository(new EzBillingModel());

            LoadInformationsFromDatabase();
        }

        private Dictionary<string, string> GetFieldInformations()
        {
            return new Dictionary<string, string>()
            {
                { NAME, companyName_TextBox.Text },
                { STREET, companyStreet_TextBox.Text },
                { CITY, companyCity_TextBox.Text },
                { POSTALCODE, companyPostalCode_TextBox.Text },
                { ID, companyID_TextBox.Text },
                { ACCOUNTNUMBER, companyAccountNumber_TextBox.Text },
                { BANKNAME, companyBankName_TextBox.Text },
                { BANKBIC, companyBankBIC_TextBox.Text },
                { BILLERNAME, companyBillerName_TextBox.Text },
                { PHONENUMBER, companyPhoneNumber_TextBox.Text },
                { EMAILADDRESS, companyEmailAddress_TextBox.Text },
            };
        }
        private Company BuildCompanyInformation()
        {
            Dictionary<string, string> valuePairs = GetFieldInformations();

            Company companyInformation = new Company()
            {
                Name = valuePairs[NAME],
                CompanyID = valuePairs[ID],
                AccountNumber = valuePairs[ACCOUNTNUMBER],
                BankName = valuePairs[BANKNAME],
                BankBIC = valuePairs[BANKBIC],
                BillerName = valuePairs[BILLERNAME],
                Phone = valuePairs[PHONENUMBER],
                Email = valuePairs[EMAILADDRESS]
            };
            companyInformation.Address.Street = valuePairs[STREET];
            companyInformation.Address.City = valuePairs[CITY];
            companyInformation.Address.PostalCode = valuePairs[POSTALCODE];

            return companyInformation;
        }
        private void RemoveFromDatabase(Company companyInformation)
        {
        }
        private void AddToDatabase(Company companyInformation)
        {
        }
        private void LoadInformationsFromDatabase()
        {
            List<Company> list = database.All.ToList();
            CompanyWindowViewModel.Items.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                CompanyWindowViewModel.Items.Add(list[i]);
            }
        }

        #region Event handlers
        private void saveCompanyInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            Company info = BuildCompanyInformation();

            controller.AddInformation(string.Format("Yrityksen {0} tiedot lisätty.", info.Name), AddToDatabase, info);
        }
        private void deleteCompanyInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.DeleteInformation(string.Format("Haluatko varmasti poistaa yrityksen {0} tiedot?", CompanyWindowViewModel.SelectedItem.Name), RemoveFromDatabase);
        }
        private void resetFields_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.ResetFields();
        }
        private void companyName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveCompanyInformation_Button.IsEnabled = companyName_TextBox.Text.Length > 0;
        }
        private void companies_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deleteCompanyInformation_Button.IsEnabled = companies_ComboBox.SelectedIndex != -1;
        }
        #endregion
    }
}