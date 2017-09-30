using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfContacts.Classes;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace WpfContacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string contactsFilePath = "C:\\temp";
        public string contactsFileName = "contacts.json";        
        public MainWindow()
        {
            InitializeComponent();

            // Specify contact list as source for data grid
            dataGrid_contacts.ItemsSource = ContactService.ContactList;
            
        }

        // Add clicked: Open EditContactWindow, and set its title to indicate add
        private void button_addContact_Click(object sender, RoutedEventArgs e)
        {
            EditContactWindow editContactWindow = new EditContactWindow();
            editContactWindow.Title = "Add Contact";                        
            editContactWindow.Owner = this;
            editContactWindow.Show();

            // Initialize the entry
            editContactWindow.InitContractEntry();
        }

        // Edit clicked: Open EditContactWindow, 
        //  and display the selected contact
        private void button_editContact_Click(object sender, RoutedEventArgs e)
        {
            // Check that non-empty row has been selected 
            if (dataGrid_contacts.SelectedItem != null &&
                  dataGrid_contacts.SelectedItem is ContactEntry)
            {
                // Create edit contact window
                EditContactWindow editContactWindow = new EditContactWindow();
                editContactWindow.Title = "Edit Contact";
                editContactWindow.Owner = this;
                editContactWindow.Show();

                // Get row data and display it in edit window
                ContactEntry contact = new ContactEntry();
                contact = (ContactEntry)dataGrid_contacts.SelectedItem;
                editContactWindow.DisplayContactEntry(contact);
            }            
        }

        // Delete clicked: Delete selected contact
        private void button_deleteContact_Click(object sender, RoutedEventArgs e)
        {
            // Check that non-empty row has been selected 
            if (dataGrid_contacts.SelectedItem != null &&
                  dataGrid_contacts.SelectedItem is ContactEntry)
            {
                // Prompt for confirmation before deleting
                MessageBoxResult deleteConfirm =
                    MessageBox.Show
                     ("Are you sure you want to delete the contact?",                       
                       "Confirm deletion", MessageBoxButton.YesNo);
                if (deleteConfirm == MessageBoxResult.Yes)
                {
                    // Get the contact ID from the selected row data
                    //  and delete the contact
                    ContactEntry contact = new ContactEntry();
                    contact = (ContactEntry)dataGrid_contacts.SelectedItem;
                    ContactService.DeleteContact(contact.Id);
                }
            }
        }

        // Window loaded: 
        // Read contacts from file 
        // Hide the ID column
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load from contacts list file if it exists
            LoadContacts();

            // Hide the ID column
            int idColumn = ContactEntry.IdColumnIndex();
            dataGrid_contacts.Columns[idColumn].Visibility 
                                      = Visibility.Hidden;
        }

        // Exiting application: Save contacts list
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                // Create directory if it doesn't exist,
                // and save file
                if (!Directory.Exists(contactsFilePath))
                {
                    Directory.CreateDirectory(contactsFilePath);
                }
                    ContactService.SaveToFile(contactsFilePath + "\\" + contactsFileName);
            }
            catch (Exception except)
            {
                MessageBox.Show("Error saving contacts file: " + except.Message);
            }
                       
        }

        // Load contacts from file if it exists,
        // and specify contact list as source for data grid
        private void LoadContacts ()
        {
            // Read contacts list file if it exists
            if (File.Exists(contactsFilePath + "\\" + contactsFileName))
            {
                // Read contacts from file
                ContactService.ReadFromFile(contactsFilePath + "\\" + contactsFileName);

                // Specify contact list as source for data grid
                dataGrid_contacts.ItemsSource = ContactService.ContactList;

                // MessageBox.Show("ContactList count: " + ContactService.ContactList.Count);
            }
        }
    }
}
