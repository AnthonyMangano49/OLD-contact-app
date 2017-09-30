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
using System.Windows.Shapes;
using WpfContacts.Classes;

namespace WpfContacts
{
    /// <summary>
    /// Interaction logic for EditContactWindow.xaml
    /// </summary>
    public partial class EditContactWindow : Window
    {

        public EditContactWindow()
        {
            InitializeComponent();
        }

        // Initialize contact entry: set contact Id to null value
        public void InitContractEntry()
        {
            label_Id.Content = ContactService.NullId;
        }
        
        // Display input contact entry
        public void DisplayContactEntry (ContactEntry contact)
        {
            textBox_firstName.Text = contact.FirstName;
            textBox_lastName.Text = contact.LastName;
            textBox_emailAddress.Text = contact.EmailAddress;
            textBox_telephoneNumber.Text = contact.TelephoneNumber;
            textBox_Address1.Text = contact.AddressLine1;
            textBox_Address2.Text = contact.AddressLine2;
            textBox_city.Text = contact.City;
            textBox_state.Text = contact.StateOrInfo1;
            textBox_zipCode.Text = contact.ZipOrInfo2;
            label_Id.Content = contact.Id;

            // Set display according to 
            //  indicator of non US address
            setDisplayNonUSAddress(contact.nonUSAddress);
       }

        // Save button clicked: save contact and close window
        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            // If contact ID is null, add new contact  
            //  otherwise update contact

            int contactID = (int)label_Id.Content;
            if (contactID == ContactService.NullId)
            {
                addNewContact();
            } 
            else
            {
                updateContact(contactID);
            }

            // Close the window
            this.Close();
        }

        // Quit button clicked: close window
        private void button_quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Checkbox for non US address selected: set labels
        private void checkBox_nonUSAddress_Checked(object sender, RoutedEventArgs e)
        {
            setLabelsNonUSAddress();
        }

        // Checkbox for non US address cleared: set labels
        private void checkBox_nonUSAddress_Unchecked(object sender, RoutedEventArgs e)
        {
            setLabelsUSAddress();
        }

        // Set checkbox & labels according to 
        //   input nonUSAddress, which is true if non-US address,
        //    otherwise false
        private void setDisplayNonUSAddress(bool nonUSAddress)
        {
            if (nonUSAddress)
            {
                checkBox_nonUSAddress.IsChecked = true;
                setLabelsNonUSAddress();
            }
            else
            {
                checkBox_nonUSAddress.IsChecked = false;
                setLabelsUSAddress();
            }
        }
         
        // Set labels for non-US address  
        private void setLabelsNonUSAddress()
        {
            label_state.Content = "Additional Info (1)";
            label_zipCode.Content = "Additional Info (2)";
        }

        // Set labels for US address  
        private void setLabelsUSAddress()
        {
            label_state.Content = "State";
            label_zipCode.Content = "Zip Code";
        }

        // Add new contact from window data
        private void addNewContact()
        {
            // Create a new contact with all the data from the window
            ContactEntry newContact = new ContactEntry();
            copyContactData(newContact);
            
            // Get a new ID for the contact
            newContact.Id = ContactService.getNewId();

            // Add the new contact
            ContactService.AddContact(newContact);
        }

        // Update existing contact
        private void updateContact(int contactId)
        {
            try
            {
                // Get the contact corresponding to the contact ID
                ContactEntry EditedContact =
                    ContactService.GetContactById(contactId);

                // Copy window data to contact
                copyContactData(EditedContact);

                // Refresh the data grid in the main window
                var mainWindow = (MainWindow)Owner;
                mainWindow.dataGrid_contacts.Items.Refresh();
            }
            catch (Exception except)
            {
                MessageBox.Show ("An error occurred while updating contact ID " 
                    + contactId + ": " + except.Message);
            }
        }

        // Copy contact data from window to contact object
        private void copyContactData (ContactEntry contact)
        {
            // Copy textbox fields to contact
            contact.FirstName = textBox_firstName.Text;
            contact.LastName = textBox_lastName.Text;
            contact.EmailAddress = textBox_emailAddress.Text;
            contact.TelephoneNumber = textBox_telephoneNumber.Text;
            contact.AddressLine1 = textBox_Address1.Text;
            contact.AddressLine2 = textBox_Address2.Text;
            contact.City = textBox_city.Text;
            contact.StateOrInfo1 = textBox_state.Text;
            contact.ZipOrInfo2 = textBox_zipCode.Text;

            // Copy contact ID
            contact.Id = (int)label_Id.Content;

            // Set indication of non-US address according to checkbox
            if (checkBox_nonUSAddress.IsChecked == true)
            {
                contact.nonUSAddress = true;
            }
            else
            {
                contact.nonUSAddress = false;
            }
            
        }


    }
}
