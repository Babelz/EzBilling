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
using EzBilling.Database.Objects;
using System.Xml.Linq;
using EzBilling.Database.Serialization;

namespace EzBilling
{
    /// <summary>
    /// Interaction logic for ClientInformationWindow.xaml
    /// </summary>
    public partial class ClientInformationWindow : Window
    {
        #region Vars
        private readonly XmlDatabaseConnection database;
        private readonly DatabaseObjectSerializer serializer;
        private readonly InformationWindowHandler informationHandler;

        private List<ClientInformation> clientInformations;
        #endregion

        public ClientInformationWindow(XmlDatabaseConnection database)
        {
            InitializeComponent();

            this.database = database;

            serializer = new DatabaseObjectSerializer();
            clientInformations = new List<ClientInformation>();
            informationHandler = new InformationWindowHandler(this, database, serializer, clients_ComboBox);
        }

        private void ResetFields()
        {
            clientName_TextBox.Clear();
            clientStreet_TextBox.Clear();
            clientCity_TextBox.Clear();
            clientPostalCode_TextBox.Clear();

            clients_ComboBox.SelectedIndex = -1;
        }
        private void AssingToObjectFields(ClientInformation clientInformation)
        {
            clientInformation.Name = clientName_TextBox.Text;
            clientInformation.Street = clientStreet_TextBox.Text;
            clientInformation.City = clientCity_TextBox.Text;
            clientInformation.PostalCode = clientPostalCode_TextBox.Text;
        }
        private void AssingToFields(ClientInformation clientInformation)
        {
            clientName_TextBox.Text = clientInformation.Name;
            clientStreet_TextBox.Text = clientInformation.Street;
            clientCity_TextBox.Text = clientInformation.City;
            clientPostalCode_TextBox.Text = clientInformation.PostalCode;
        }
        private void AddNewClients()
        {
            ResetFields();

            clientInformations = serializer.Deserialize<ClientInformation>(database.FindItemRoot("Clients").Elements().ToList());

            for (int i = 0; i < clientInformations.Count; i++)
            {
                if (clients_ComboBox.Items.Contains(clientInformations[i].Name))
                {
                    continue;
                }

                clients_ComboBox.Items.Add(clientInformations[i].Name);
            }
        }


        #region Event handlers
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                AddNewClients();
            }
        }
        private void clearFields_Button_Click(object sender, RoutedEventArgs e)
        {
            informationHandler.OnClear(ResetFields);
        }
        private void deleteClientInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            informationHandler.OnDelete("asiakkaan", "Clients", ResetFields);
        }
        private void saveClientInformation_Button_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation clientInformation = clientInformations.FirstOrDefault(i => i.Name == (string)clients_ComboBox.SelectedItem);
            
            bool added = informationHandler.OnSave<ClientInformation>(
                clientInformation, 
                clientName_TextBox, 
                clientInformations, 
                AssingToObjectFields,
                string.Format("Nimellä '{0}' löydettiin asiakkaan tiedot. Haluatko ylikirjoittaa ne?", clientName_TextBox.Text),
                string.Format("Asiakkaan '{0}' tiedot tallennettu.", clientName_TextBox.Text),
                "Clients");

            if (added)
            {
                AddNewClients();
            }
        }
        private void clients_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientInformation clientInformation = clientInformations.FirstOrDefault(i => i.Name == (string)clients_ComboBox.SelectedItem);

            informationHandler.OnSelectedItemChanged<ClientInformation>(
                deleteClientInformation_Button,
                clientInformation,
                AssingToFields);
        }
        private void clientName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveClientInformation_Button.IsEnabled = clientName_TextBox.Text.Length > 0;
        }
        #endregion
    }
}
