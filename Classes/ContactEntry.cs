using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfContacts.Classes
{
    // Class for contact entries
    public class ContactEntry
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        // Indicates if address is not in US
        public bool nonUSAddress { get; set; }

        // If nonUSaddress is false, holds state,
        // otherwise holds extra information
        public string StateOrInfo1 { get; set; }

        // If nonUSaddress is false, holds zip code,
        // otherwise holds extra information
        public string ZipOrInfo2 { get; set; }  
        public int Id { get; set; }  
        
        // Method to return column index of Id        
        public static int IdColumnIndex()
        {
            return 10;
        }    
    }
}
