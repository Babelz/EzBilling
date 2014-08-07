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
using EzBilling.Database;
using EzBilling.Database.Serialization;
using EzBilling.Database.Objects;

namespace EzBilling
{
    /// <summary>
    /// Interaction logic for CompanyInformationWindow.xaml
    /// </summary>
    public partial class CompanyInformationWindow : Window
    {
        #region Vars
        private readonly XmlDatabaseConnection database;
        private readonly DatabaseObjectSerializer serializer;
        private readonly InformationWindowHandler informationHandler;

        private List<CompanyInformation> companyInformations;
        #endregion

        public CompanyInformationWindow(XmlDatabaseConnection database)
        {
            InitializeComponent();

            this.database = database;

            serializer = new DatabaseObjectSerializer();
            companyInformations = new List<CompanyInformation>();
            informationHandler = new InformationWindowHandler(this, database, serializer, companies_ComboBox);
            
        }

        private void ResetFields()
        {
            companyName_TextBox.Clear();
            companyStreet_TextBox.Clear();
            companyCity_TextBox.Clear();
            companyPostalCode_TextBox.Clear();
            companyID_TextBox.Clear();
            companyAccountNumber_TextBox.Clear();
            companyBankName_TextBox.Clear();
            companyBankBIC_TextBox.Clear();
            companyBillerName_TextBox.Clear();
            companyPhoneNumber_TextBox.Clear();
            companyEmailAddress_TextBox.Clear();

            companies_ComboBox.SelectedIndex = -1;
        }
        private void AssingToObjectFields(CompanyInformation companyInformation)
        {
            companyInformation.Name = companyName_TextBox.Text;
            companyInformation.Street = companyStreet_TextBox.Text;
            companyInformation.City = companyCity_TextBox.Text;
            companyInformation.PostalCode = companyPostalCode_TextBox.Text;
            companyInformation.CompanyID = companyID_TextBox.Text;
            companyInformation.BankAccountNumber = companyAccountNumber_TextBox.Text;
            companyInformation.BankName = companyBankName_TextBox.Text;
            companyInformation.BankBIC = companyBankBIC_TextBox.Text;
            companyInformation.BillerName = companyBillerName_TextBox.Text;
            companyInformation.PhoneNumber = companyPhoneNumber_TextBox.Text;
            companyInformation.EmailAddress = companyEmailAddress_TextBox.Text;
        }
        private void AssingToFields(CompanyInformation companyInformation)
        {
            companyName_TextBox.Text = companyInformation.Name;
            companyStreet_TextBox.Text = companyInformation.Street;
            companyCity_TextBox.Text = companyInformation.City;
            companyPostalCode_TextBox.Text = companyInformation.PostalCode;
            companyID_TextBox.Text = companyInformation.CompanyID;
            companyAccountNumber_TextBox.Text = companyInformation.BankAccountNumber;
            companyBankName_TextBox.Text = companyInformation.BankName;
            companyBankBIC_TextBox.Text = companyInformation.BankBIC;
            companyBillerName_TextBox.Text = companyInformation.BillerName;
            companyPhoneNumber_TextBox.Text = companyInformation.PhoneNumber;
            companyEmailAddress_TextBox.Text = companyInformation.EmailAddress;
        }
        private void AddNewCompanies()
        {
            ResetFields();

            companyInformations = serializer.Deserialize<CompanyInformation>(database.FindItemRoot("Companies").Elements().ToList());

            for (int i = 0; i < companyInformations.Count; i++)
            {
                if (companies_ComboBox.Items.Contains(companyInformations[i]))
                {
                    continue;
                }

                companies_ComboBox.Items.Add(companyInformations[i].Name);
            }
        }

        #region Event handlers
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                AddNewCompanies();
            }
        }
        private void companies_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CompanyInformation companyInformation = companyInformations.FirstOrDefault(i => i.Name == (string)companies_ComboBox.SelectedItem);

            informationHandler.OnSelectedItemChanged(
                deleteCompanyInformation_Button,
                companyInformation,
                AssingToFields);
        }
        private void saveCompanyInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            CompanyInformation companyInformation = companyInformations.FirstOrDefault(i => i.Name == (string)companies_ComboBox.SelectedItem);

            bool added = informationHandler.OnSave<CompanyInformation>(
                companyInformation,
                companyName_TextBox,
                companyInformations,
                AssingToObjectFields,
                string.Format("Nimellä '{0}' löydettiin yrityksen tiedot. Haluatko ylikirjoittaa ne?", companyName_TextBox.Text),
                string.Format("Yrityksen '{0}' tiedot tallennettu.", companyName_TextBox.Text),
                "Companies");

            if (added)
            {
                AddNewCompanies();
            }
        }
        private void deleteCompanyInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            informationHandler.OnDelete("yrityksen", "Companies", ResetFields);
        }
        private void clearFields_Button_Click(object sender, RoutedEventArgs e)
        {
            informationHandler.OnClear(ResetFields);
        }
        private void companyName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveCompanyInformation_Button.IsEnabled = companyName_TextBox.Text.Length > 0;
        }
        #endregion
    }
}
