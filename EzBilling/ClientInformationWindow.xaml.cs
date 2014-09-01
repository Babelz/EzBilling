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
using System.Xml.Linq;
using EzBilling.Database;
using EzBilling.Components;
using System.Collections.ObjectModel;
using EzBilling.Models;

namespace EzBilling
{
    /// <summary>
    /// Interaction logic for ClientInformationWindow.xaml
    /// </summary>
    public partial class ClientInformationWindow : Window
    {
        #region Contants
        // Dict keys.
        private const string NAME = "Name";
        private const string STREET = "Street";
        private const string CITY = "City";
        private const string POSTALCODE = "PostalCode";
        #endregion

        #region Vars
        private readonly InformationWindowController<Client> controller;

        private readonly ClientRepository database;

        #endregion

        #region Properties
        public InformationWindowViewModel<Client> ClientWindowViewModel
        {
            get;
            private set;
        }
        #endregion

        public ClientInformationWindow()
        {
            ClientWindowViewModel = new InformationWindowViewModel<Client>();
            ClientWindowViewModel.Items = new ObservableCollection<Client>();

            InitializeComponent();
           
            DataContext = this;

            controller = new InformationWindowController<Client>(ClientWindowViewModel, clients_ComboBox, 
                new TextBox[] 
            {
                clientName_TextBox,
                clientStreet_TextBox,
                clientCity_TextBox,
                clientPostalCode_TextBox
            });

            database = new ClientRepository(new EzBillingModel());

            LoadInformationsFromDatabase();
        }

        private Dictionary<string, string> GetFieldInformations()
        {
            return new Dictionary<string, string>()
            {
                { NAME,  clientName_TextBox.Text},
                { STREET,  clientStreet_TextBox.Text },
                { CITY,  clientCity_TextBox.Text },
                { POSTALCODE,  clientPostalCode_TextBox.Text }
            };
        }
        private Client BuildClientInformation()
        {
            Dictionary<string, string> valuePairs = GetFieldInformations();

            Client clientInformation = new Client()
            {
                Name = valuePairs[NAME]
            };
            clientInformation.Address.Street = valuePairs[STREET];
            clientInformation.Address.City = valuePairs[CITY];
            clientInformation.Address.PostalCode = valuePairs[POSTALCODE];

            return clientInformation;
        }
        private void RemoveFromDatabase(Client clientInformation)
        {
        }
        private void AddToDatabase(Client clientInformation)
        {
        }
        private void LoadInformationsFromDatabase()
        {
            List<Client> list = database.All.ToList();
            ClientWindowViewModel.Items.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                ClientWindowViewModel.Items.Add(list[i]);
            }
        }

        #region Event handlers
        private void saveClientInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            Client info = BuildClientInformation();

            controller.AddInformation(string.Format("Asiakkaan {0} tiedot lisätyy.", info.Name), AddToDatabase, info);
        }
        private void deleteClientInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.DeleteInformation(string.Format("Halutko varmasti poistaa asiakkaan {0} tiedot?", ClientWindowViewModel.SelectedItem.Name), RemoveFromDatabase);
        }
        private void resetFields_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.ResetFields();
        }
        private void clientName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveClientInformation_Button.IsEnabled = clientName_TextBox.Text.Length > 0;
        }
        private void clients_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deleteClientInformation_Button.IsEnabled = clients_ComboBox.SelectedIndex != -1;
        }
        #endregion
    }
}
