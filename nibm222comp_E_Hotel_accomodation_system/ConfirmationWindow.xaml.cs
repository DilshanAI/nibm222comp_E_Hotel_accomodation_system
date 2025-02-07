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

namespace nibm222comp_E_Hotel_accomodation_system
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow(string bookingId, string customerName, string mobile, string roomType, int persons, DateTime reservedDate, DateTime checkInDate, DateTime checkoutDate, string price)
        {
            InitializeComponent();
            
            txt_BookingID.Text = "Booking ID: " + bookingId;
            txt_BookingDate.Text = "Date / Time " + reservedDate;
            txt_CustomerName.Text = "Customer Name: " + customerName;
            txt_CustomerMobile.Text = "Mobile: " + mobile;
            txt_RoomType.Text = "Room Type: " + roomType;
            txt_NoOfPersons.Text = "No. of Persons: " + persons.ToString();
            txt_ReservedDate.Text = "Checkin Date: " + checkInDate.ToShortDateString();
            txt_CheckoutDate.Text = "Checkout Date: " + checkoutDate.ToShortDateString();
            txt_Price.Text = "Price: Rs. " + price;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the confirmation window after clicking "Confirm"
        }
    }
}
