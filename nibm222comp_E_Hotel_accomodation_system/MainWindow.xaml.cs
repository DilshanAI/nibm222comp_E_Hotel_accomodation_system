using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace nibm222comp_E_Hotel_accomodation_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlcon = new SqlConnection(Connection.ConnectionString);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            //if (IsValidPassword(password))
            //{
            //    try
            //    {
            //        sqlcon.Open();
            //        string query = "SELECT COUNT(1) FROM Login WHERE Username=@Username AND Password=@Password";
            //        SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
            //        sqlCmd.Parameters.AddWithValue("@Username", username);
            //        sqlCmd.Parameters.AddWithValue("@Password", password);
            //        int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

            //        if (count == 1)
            //        {

                    

                        Dashboard dashboard = new Dashboard();
                        dashboard.Show();
                        this.Close();


            //        }
            //        else
            //        {
            //            MessageBox.Show("Username or password is incorrect.");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    finally
            //    {
            //        sqlcon.Close();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Invalid email format or password length.");
            //}
        }
  

        private bool IsValidPassword(string password)
        {
            return password.Length > 7;
        }
    }
}