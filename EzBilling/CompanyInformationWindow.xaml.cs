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
    public partial class companyWindow : Window
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
        private readonly CompanyRepository companyRepository;
        #endregion

        #region Properties
        public InformationWindowViewModel<Company> CompanyWindowViewModel
        {
            get;
            private set;
        }
        #endregion

        public companyWindow(CompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
            
            CompanyWindowViewModel = new InformationWindowViewModel<Company>();
            CompanyWindowViewModel.Items = new ObservableCollection<Company>(companyRepository.All.ToList());

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
        private Company BuildCompany()
        {
            Dictionary<string, string> valuePairs = GetFieldInformations();

            Company company = new Company()
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

            company.Address.Street = valuePairs[STREET];
            company.Address.City = valuePairs[CITY];
            company.Address.PostalCode = valuePairs[POSTALCODE];

            return company;
        }
        private void RemoveFromDatabase(Company company)
        {
            CompanyWindowViewModel.Items.Remove(company);
            companyRepository.Delete(company);

            if (ReferenceEquals(company, CompanyWindowViewModel.SelectedItem))
            {
                CompanyWindowViewModel.SelectedItem = null;
            }

            companyRepository.Save();
        }
        private void AddToDatabase(Company company)
        {
            CompanyWindowViewModel.Items.Add(company);
            companyRepository.InsertOrUpdate(company);

            companyRepository.Save();
        }

        #region Event handlers
        private void savecompany_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyWindowViewModel.Items.Contains(CompanyWindowViewModel.SelectedItem))
            {
                companyRepository.InsertOrUpdate(CompanyWindowViewModel.SelectedItem);
                companyRepository.Save();

                MessageBox.Show("Muutokset tallennettu.", "EzBilling", MessageBoxButton.OK);

                return;
            }

            Company info = BuildCompany();

            controller.AddInformation(string.Format("Yrityksen {0} tiedot lisätty.", info.Name), AddToDatabase, info);
        }
        private void deletecompany_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.DeleteInformation(string.Format("Haluatko varmasti poistaa yrityksen {0} tiedot?", CompanyWindowViewModel.SelectedItem.Name), RemoveFromDatabase);
        }
        private void resetFields_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.ResetFields();
        }
        private void companyName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            savecompany_Button.IsEnabled = companyName_TextBox.Text.Length > 0;
        }
        private void companies_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deletecompany_Button.IsEnabled = companies_ComboBox.SelectedIndex != -1;
        }
        #endregion
    }
}