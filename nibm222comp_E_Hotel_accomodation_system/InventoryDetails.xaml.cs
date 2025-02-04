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

namespace nibm222comp_E_Hotel_accomodation_system
{
    /// <summary>
    /// Interaction logic for InventoryDetails.xaml
    /// </summary>
    public partial class InventoryDetails : UserControl
    {
        public event Action NavigateBackToInventory;
        public InventoryDetails()
        {
            InitializeComponent();
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackToInventory?.Invoke();
        }
    }
}
