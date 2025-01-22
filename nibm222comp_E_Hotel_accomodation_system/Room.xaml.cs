using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace nibm222comp_E_Hotel_accomodation_system
{
    /// <summary>
    /// Interaction logic for Room.xaml
    /// </summary>
    public partial class Room : UserControl
    {

        SqlConnection sqlcon = new SqlConnection(Connection.ConnectionString);
        string selectedRoomType = "0";
        string selectedBedType = "0";

        public Room()
        {
            InitializeComponent();
            LoadData();

        }

        private void LoadData()
        {
            try
            {
                sqlcon.Open();
                SqlCommand roomQuery = new SqlCommand("SELECT Distinct RoomType FROM room", sqlcon);
                using (SqlDataReader reader = roomQuery.ExecuteReader())
                {
                    cmbRoomType.Items.Clear();
                    while (reader.Read())
                    {
                        cmbRoomType.Items.Add(reader["RoomType"].ToString());
                    }
                }

                SqlCommand cusQuery = new SqlCommand("SELECT Distinct BedType FROM room", sqlcon);
                using (SqlDataReader reader = cusQuery.ExecuteReader())
                {
                    cmbBedType.Items.Clear();
                    while (reader.Read())
                    {
                        cmbBedType.Items.Add(reader["BedType"].ToString());
                    }
                }

               

            }
            catch (SqlException)
            {
                MessageBox.Show("Database error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlcon.Open();

                // Retrieve user input
                string roomID = txtRoomId.Text.Trim();
                string selectedRoomType = cmbRoomType.SelectedItem?.ToString();
                string selectedBedType = cmbBedType.SelectedItem?.ToString();
                string totalPrice = txtPrice.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(roomID) || string.IsNullOrEmpty(selectedRoomType) || string.IsNullOrEmpty(selectedBedType) || string.IsNullOrEmpty(totalPrice))
                {
                    MessageBox.Show("Please fill all fields", "Room Details", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (!IsValidRoomId(roomID))
                {
                    MessageBox.Show("Room ID format is incorrect. Please enter in the format R001, R002, etc.", "Room Details", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                else if (!decimal.TryParse(totalPrice, out decimal price))
                {
                    MessageBox.Show("Invalid price. Please enter a valid numeric value.", "Room Details", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                else {

                    // Check if RoomID already exists
                    string checkQuery = "SELECT COUNT(1) FROM Room WHERE RoomID = @RoomID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, sqlcon);
                    checkCmd.Parameters.AddWithValue("@RoomID", roomID);

                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (exists > 0)
                    {
                        MessageBox.Show("Room ID is already available. Please use a different Room ID.", "Room Details", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // SQL query to insert data
                    string query = "INSERT INTO Room (RoomID, RoomType, BedType, Price, CreateDate) VALUES (@RoomID, @RoomType, @BedType, @Price, @CreateDate)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);

                // Add parameters
                sqlCmd.Parameters.AddWithValue("@RoomID", roomID);
                sqlCmd.Parameters.AddWithValue("@RoomType", selectedRoomType);
                sqlCmd.Parameters.AddWithValue("@BedType", selectedBedType);
                sqlCmd.Parameters.AddWithValue("@Price", price);
                sqlCmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                // Execute the query
                int rowsAffected = sqlCmd.ExecuteNonQuery();

                // Confirm success
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Room details added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    clear();
                }
                else
                {
                    MessageBox.Show("Failed to add room details", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        private bool IsValidRoomId(string roomId)
        {
            Regex regex = new Regex(@"^R\d{3}$");
            return regex.IsMatch(roomId);
        }

        private void clear()
        {
            cmbRoomType.Items.Clear();
            cmbBedType.Items.Clear();

            // Add default placeholder option
            cmbRoomType.Items.Add("-- Select --");
            cmbBedType.Items.Add("-- Select --");

            // Reset the selected index to show the default placeholder
            cmbRoomType.SelectedIndex = 0;
            cmbBedType.SelectedIndex = 0;

            txtRoomId.Text = "";
            txtPrice.Text = "";
        }

        private void selectRoomType(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRoomType.SelectedItem != null)
            {
                this.selectedRoomType = cmbRoomType.SelectedItem.ToString();
               
            }
        }

        private void selectBedType(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBedType.SelectedItem != null)
            {
                this.selectedBedType = cmbBedType.SelectedItem.ToString();

            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
