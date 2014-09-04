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
    /// Interaction logic for clientWindow.xaml
    /// </summary>
    public partial class clientWindow : Window
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
        private readonly ClientRepository clientRepository;
        private readonly BillRepository billRepository;
        private readonly BillManager billManager;
        #endregion

        #region Properties
        public InformationWindowViewModel<Client> ClientWindowViewModel
        {
            get;
            private set;
        }
        #endregion

        public clientWindow(ClientRepository clientRepository, BillRepository billRepository, BillManager billManager)
        {
            this.clientRepository = clientRepository;
            this.billRepository = billRepository;
            this.billManager = billManager;

            ClientWindowViewModel = new InformationWindowViewModel<Client>();
            ClientWindowViewModel.Items = new ObservableCollection<Client>(clientRepository.All.ToList());

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
        private Client BuildClient()
        {
            Dictionary<string, string> valuePairs = GetFieldInformations();

            Client client = new Client()
            {
                Name = valuePairs[NAME]
            };

            client.Address.Street = valuePairs[STREET];
            client.Address.City = valuePairs[CITY];
            client.Address.PostalCode = valuePairs[POSTALCODE];

            return client;
        }
        private void RemoveFromDatabase(Client client)
        {
            for (int i = 0; i < client.Bills.Count; i++)
            {
                for (int j = 0; j < client.Bills[i].Products.Count; j++)
                {
                    client.Bills[i].Products.Remove(client.Bills[i].Products[j]);
                    clientRepository.InsertOrUpdate(client);
                    clientRepository.Save();
                }

                Bill bill = client.Bills[i];
                client.Bills.Remove(bill);
                billManager.RemoveKnownBill(bill.Name);

                billRepository.Delete(bill);
                billRepository.Save();

                clientRepository.InsertOrUpdate(client);
                clientRepository.Save();
            }

            ClientWindowViewModel.Items.Remove(client);
            clientRepository.Delete(client);

            if (ReferenceEquals(client, ClientWindowViewModel.SelectedItem))
            {
                ClientWindowViewModel.SelectedItem = null;
            }

            clientRepository.Save();
        }
        private void AddToDatabase(Client client)
        {
            ClientWindowViewModel.Items.Add(client);
            clientRepository.InsertOrUpdate(client);

            clientRepository.Save();
        }

        #region Event handlers
        private void saveclient_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ClientWindowViewModel.Items.Contains(ClientWindowViewModel.SelectedItem))
            {
                clientRepository.InsertOrUpdate(ClientWindowViewModel.SelectedItem);
                clientRepository.Save();

                MessageBox.Show("Muutokset tallennettu.", "EzBilling", MessageBoxButton.OK);

                return;
            }

            Client info = BuildClient();

            controller.AddInformation(string.Format("Asiakkaan {0} tiedot lisätyy.", info.Name), AddToDatabase, info);
        }
        private void deleteclient_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.DeleteInformation(string.Format("Halutko varmasti poistaa asiakkaan {0} tiedot? Asiakkaan laskut poistetaan myös.", ClientWindowViewModel.SelectedItem.Name),                                              RemoveFromDatabase);
        }
        private void resetFields_Button_Click(object sender, RoutedEventArgs e)
        {
            controller.ResetFields();
        }
        private void clientName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveclient_Button.IsEnabled = clientName_TextBox.Text.Length > 0;
        }
        private void clients_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deleteclient_Button.IsEnabled = clients_ComboBox.SelectedIndex != -1;
        }
        #endregion
    }
}
