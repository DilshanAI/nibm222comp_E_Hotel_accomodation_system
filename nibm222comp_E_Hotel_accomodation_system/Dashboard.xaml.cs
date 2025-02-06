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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
       
        public Dashboard()
        {
            InitializeComponent();
            ContentArea.Content = new Dashboard_Content();

        }

        private void rooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Click_Room(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new Room();
        }

        private void customers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new Customer();
        }

        private void employees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new Employee();
        }

        private void inventories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadInventoryPage();
        }

        private void LoadInventoryPage()
        {
            Inventory inventoryPage = new Inventory();

            // Subscribe to the event for navigating to InventoryDetails.xaml
            inventoryPage.NavigateToInventoryDetails += LoadInventoryDetailsPage;

            ContentArea.Content = inventoryPage;
        }

        private void LoadInventoryDetailsPage()
        {
            InventoryDetails detailsPage = new InventoryDetails();

            // Subscribe to event to go back to Inventory.xaml
            detailsPage.NavigateBackToInventory += LoadInventoryPage;

            ContentArea.Content = detailsPage;
        }

        private void reports_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new Reports();

        }

        private void RoomReservation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new ReserveRoom();
        }

        private void profile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new Profile();
        }

        private void dashboard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Reset the dashboard content
            ContentArea.Content = new Dashboard_Content();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Close the current Dashboard window
            this.Close();
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

       
    }
}
