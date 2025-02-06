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
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace nibm222comp_E_Hotel_accomodation_system
{
    /// <summary>
    /// Interaction logic for ReserveRoom.xaml
    /// </summary>
    public partial class ReserveRoom : UserControl
    {

        SqlConnection sqlcon = new SqlConnection(Connection.ConnectionString);
        public ReserveRoom()
        {
            InitializeComponent();
            GenerateBookingId();
            LoadRoomtypeData();
        }

        private void GenerateBookingId()
        {
            try
            {
                sqlcon.Open();
                string query = "SELECT TOP 1 ReID FROM Reservation ORDER BY ReID DESC";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                object result = cmd.ExecuteScalar();

                string newId = "B001"; // Default ID if no records exist

                if (result != null)
                {
                    string lastId = result.ToString();
                    int numPart = int.Parse(lastId.Substring(1)); // Extract number part
                    newId = "B" + (numPart + 1).ToString("D3"); // Increment & format
                }

                txt_bookingid.Text = newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Booking ID: " + ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }
        }

        private void LoadRoomtypeData()
        {
            try
            {
                sqlcon.Open();
                SqlCommand roomQuery = new SqlCommand("SELECT Distinct RoomType FROM room", sqlcon);
                using (SqlDataReader reader = roomQuery.ExecuteReader())
                {
                    cmbRoomtype.Items.Clear();
                    while (reader.Read())
                    {
                        cmbRoomtype.Items.Add(reader["RoomType"].ToString());
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


        private void btn_sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_cusid.Text) || string.IsNullOrEmpty(cmbroomno.Text) ||
    string.IsNullOrEmpty(cmbQuantity.Text) || dp_reservedate.SelectedDate == null || dp_checkdate.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Get the values from the UI controls
            string customerId = txt_cusid.Text; // Customer ID from txt_cusid
            string roomId = cmbroomno.SelectedItem.ToString(); // Room ID from cmbroomno
            DateTime reservationDate = dp_reservedate.SelectedDate.Value; // Reservation date from dp_reservedate
            DateTime checkInDate = dp_reservedate.SelectedDate.Value; // Check-in date from dp_reservedate
            DateTime checkOutDate = dp_checkdate.SelectedDate.Value; // Check-out date from dp_checkdate
            string remarks = txt_remarks.Text; // Remarks from txt_remarks (assuming this exists)
            int numberOfPersons = Convert.ToInt32(((ComboBoxItem)cmbQuantity.SelectedItem).Content.ToString());

            string bookingId = txt_bookingid.Text; // Booking ID from txt_bookingid

            try
            {
                sqlcon.Open();

                // Insert data into the Reservation table
                SqlCommand cmd = new SqlCommand("INSERT INTO Reservation (ReID, Re_date, check_in, check_out, Remarks, No_Person, CusID, RoID) " +
                                                "VALUES (@ReID, @ReDate, @CheckInDate, @CheckOutDate, @Remarks, @NoPersons, @CusID, @RoID)", sqlcon);

                // Add parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@ReID", bookingId);
                cmd.Parameters.AddWithValue("@ReDate", reservationDate);
                cmd.Parameters.AddWithValue("@CheckInDate", checkInDate);
                cmd.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrEmpty(remarks) ? (object)DBNull.Value : remarks); // Handle null remarks
                cmd.Parameters.AddWithValue("@NoPersons", numberOfPersons);
                cmd.Parameters.AddWithValue("@CusID", customerId);
                cmd.Parameters.AddWithValue("@RoID", roomId);

                // Execute the query
                cmd.ExecuteNonQuery();

                MessageBox.Show("Reservation saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Optionally, clear the form or reset fields after saving
                ClearForm();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error while saving reservation: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlcon.Close();
                clear();
            }
        }

        private void clear()
        {
            // Clear text fields
            txt_mobile.Text = "";

            // Clear and reload Room Numbers
            cmbRoomtype.Items.Clear();
            LoadRoomtypeData();  // Reload room numbers from the database
            GenerateBookingId();

            cmbBedtype.Items.Clear();
            cmbroomno.Items.Clear();

            txt_remarks.Text = "";
        }

            private void ClearForm()
        {
            // Clear or reset the form fields if needed
            txt_cusid.Clear();
            cmbroomno.SelectedIndex = -1;
            dp_reservedate.SelectedDate = null;
            dp_checkdate.SelectedDate = null;
            txt_remarks.Text = "";
            cmbQuantity.SelectedIndex = -1;
            txt_price.Text = ""; // Clear the price text
        }

        private void cmbRoomtype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRoomtype.SelectedItem != null)
            {
                string selectedRoomType = cmbRoomtype.SelectedItem.ToString();
                LoadBedTypes(selectedRoomType);

                // Clear price text when room type is changed
                txt_price.Text = ""; // Clear the price text
            }
        }

        private void LoadBedTypes(string roomType)
        {

            try
            {
                sqlcon.Open();
                SqlCommand bedtypeQuery = new SqlCommand("SELECT DISTINCT BedType FROM Room WHERE RoomType = @RoomType", sqlcon);
                bedtypeQuery.Parameters.AddWithValue("@RoomType", roomType);
                SqlDataReader reader = bedtypeQuery.ExecuteReader();

                cmbBedtype.Items.Clear();
                while (reader.Read())
                {
                    cmbBedtype.Items.Add(reader["BedType"].ToString());
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

        private void cmbBedtype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbBedtype.SelectedItem != null && cmbRoomtype.SelectedItem != null)
            {
                string selectedBedType = cmbBedtype.SelectedItem.ToString();
                string selectedRoomType = cmbRoomtype.SelectedItem.ToString();
                LoadRoomid(selectedRoomType, selectedBedType);

                // Clear price text when bed type is changed
                txt_price.Text = ""; // Clear the price text
            }
        }

        private void LoadRoomid(string roomType, string bedType)
        {

            try
            {

                // Ensure both dates are selected
                if (!dp_reservedate.SelectedDate.HasValue || !dp_checkdate.SelectedDate.HasValue)
                {
                    cmbroomno.Items.Clear();
                    return;
                }
                DateTime checkInDate = dp_reservedate.SelectedDate.Value; // Check-in date from dp_reservedate
                DateTime checkOutDate = dp_checkdate.SelectedDate.Value; // Check-out date from dp_checkdate

                sqlcon.Open();
                SqlCommand roomidQuery = new SqlCommand(@"
        SELECT DISTINCT Room.RoomID 
        FROM Room 
        LEFT JOIN Reservation ON Room.RoomID = Reservation.RoID 
        AND (
            (Reservation.check_in <= @CheckOutDate AND Reservation.check_out >= @CheckInDate)
        )
        WHERE Room.RoomType = @RoomType 
        AND Room.BedType = @BedType 
        AND Reservation.RoID IS NULL", sqlcon);

                roomidQuery.Parameters.AddWithValue("@RoomType", roomType);
                roomidQuery.Parameters.AddWithValue("@BedType", bedType);
                roomidQuery.Parameters.AddWithValue("@CheckInDate", checkInDate);
                roomidQuery.Parameters.AddWithValue("@CheckOutDate", checkOutDate);

                SqlDataReader reader = roomidQuery.ExecuteReader();

                cmbroomno.Items.Clear(); // Clear existing Room IDs

                while (reader.Read())
                {
                    cmbroomno.Items.Add(reader["RoomID"].ToString()); // Add available Room IDs to combo box
                }
                reader.Close();
                CalculatePrice();
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

        private void cmbroomno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbroomno.SelectedItem != null)
            {
                string selectedRoomID = cmbroomno.SelectedItem.ToString();
                LoadRoomPrice(selectedRoomID);

                // Recalculate price when room number is changed
                CalculatePrice();
            }
        }

        private void LoadRoomPrice(string roomID)
        {
            try
            {
                sqlcon.Open();
                SqlCommand priceQuery = new SqlCommand("SELECT Price FROM Room WHERE RoomID = @RoomID", sqlcon);
                priceQuery.Parameters.AddWithValue("@RoomID", roomID);

                object result = priceQuery.ExecuteScalar(); // Get the price as a single value

                if (result != null)
                {
                    txt_price.Text = result.ToString(); // Update txt_price with the price
                }
                else
                {
                    txt_price.Text = "N/A"; // If no price found, set default text
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Database error while fetching price", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dp_reservedate.DisplayDateStart = DateTime.Today;
            dp_checkdate.DisplayDateStart = DateTime.Today;
        }

        private void dp_reservedate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadRooms();
           
        }

        private void dp_checkdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadRooms();

        }

        private void ReloadRooms()
        {
            txt_price.Text = "";
            if (cmbRoomtype.SelectedItem != null && cmbBedtype.SelectedItem != null
                && dp_reservedate.SelectedDate.HasValue && dp_checkdate.SelectedDate.HasValue)
            {
                string selectedRoomType = cmbRoomtype.SelectedItem.ToString();
                string selectedBedType = cmbBedtype.SelectedItem.ToString();
                LoadRoomid(selectedRoomType, selectedBedType);
                CalculatePrice();
            }
        }
        private void CalculatePrice()
        {
            // Check if both dates are selected
            if (dp_reservedate.SelectedDate.HasValue && dp_checkdate.SelectedDate.HasValue)
            {
                DateTime checkInDate = dp_reservedate.SelectedDate.Value;
                DateTime checkOutDate = dp_checkdate.SelectedDate.Value;

                // Ensure the check-out date is after the check-in date
                if (checkOutDate >= checkInDate)
                {
                    // Calculate the number of nights
                    int numberOfNights = (checkOutDate - checkInDate).Days;

                    // Get the room price
                    if (cmbroomno.SelectedItem != null)
                    {
                        string selectedRoomID = cmbroomno.SelectedItem.ToString();
                        decimal roomPrice = GetRoomPrice(selectedRoomID);

                        // Calculate the total price
                        decimal totalPrice = numberOfNights * roomPrice;

                        // Format the total price in LKR (Sri Lankan Rupees)

                        txt_price.Text = totalPrice.ToString(); // Format as LKR (e.g., Rs. 100.00)
                    }
                }
                else
                {
                    MessageBox.Show("Check-out date must be after the check-in date.", "Invalid Dates", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private decimal GetRoomPrice(string roomID)
        {
            decimal price = 0;
            try
            {
                sqlcon.Open();
                SqlCommand priceQuery = new SqlCommand("SELECT Price FROM Room WHERE RoomID = @RoomID", sqlcon);
                priceQuery.Parameters.AddWithValue("@RoomID", roomID);

                object result = priceQuery.ExecuteScalar(); // Get the price as a single value

                if (result != null)
                {
                    price = Convert.ToDecimal(result); // Convert to decimal
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Database error while fetching price", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlcon.Close();
            }

            return price;
        }

        private void txt_mobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            string mobileNumber = txt_mobile.Text;

            if (!string.IsNullOrEmpty(mobileNumber) && mobileNumber.Length == 10) // Validate phone number length
            {
                LoadCustomerId(mobileNumber);
            }
            else
            {
                txt_cusid.Text = ""; // Clear the customer ID if the mobile number is invalid
            }
        }

        private void LoadCustomerId(string mobileNumber)
        {
            try
            {
                sqlcon.Open();

                // Query to fetch CustomerID based on the mobile number
                SqlCommand cmd = new SqlCommand("SELECT CustomerID FROM Customer WHERE Cus_Tele = @MobileNumber", sqlcon);
                cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);

                object result = cmd.ExecuteScalar(); // Execute the query and fetch the result

                if (result != null)
                {
                    txt_cusid.Text = result.ToString(); // Set the customer ID in txt_cusid
                }
                else
                {
                   // txt_cusid.Text = "Not Found"; // If no customer found, show a message or leave it empty
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }

    }
}

