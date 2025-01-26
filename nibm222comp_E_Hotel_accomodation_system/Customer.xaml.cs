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
using System.Diagnostics;

namespace nibm222comp_E_Hotel_accomodation_system
{

    public partial class Customer : UserControl
    {
        SqlConnection sqlcon = new SqlConnection(Connection.ConnectionString);
        string selectedRoomType = "0";
        string selectedBedType = "0";
        public Customer()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string CustomerID = txtUpdateCusId.Text.Trim();

            if (string.IsNullOrEmpty(CustomerID))
            {
                MessageBox.Show("Please enter a Customer ID.", "Search Customer", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                sqlcon.Open();
                string query = "SELECT CusName,C_Address, Cus_Tele,CusNic,Cemail,BirthOfDate FROM Customer WHERE CustomeRID = @CustomeRID";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                cmd.Parameters.AddWithValue("@CustomeRID", CustomerID);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtUpdateName.Text = reader["CusName"].ToString();
                    txtUpdateAddress.Text = reader["C_Address"].ToString();
                    txtUpdatemobile.Text = reader["Cus_Tele"].ToString();
                    txtUpdatenic.Text = reader["CusNic"].ToString();
                    txtUpdateemail.Text = reader["Cemail"].ToString();
                    datePickerDOB.Text = reader["BirthOfDate"].ToString();
                }
                else
                {
                    MessageBox.Show("Customer ID does not exist.", "Search Customer", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void clearFiled()
        {
            txtAddress.Text = "";
            txtCusId.Text = "";
            txtName.Text = "";
            txtmobile.Text = "";
            txtnic.Text = "";
            txtemail.Text = "";
            datePickerDOB.Text = "";
        }
   
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string updateCustomerID = txtUpdateCusId.Text.Trim();
            string updateName = txtUpdateName.Text.Trim();
            string updateAddress = txtUpdateAddress.Text.Trim();
            string updateMobile = txtUpdatemobile.Text.Trim();
            string updateNIC = txtUpdatenic.Text.Trim();
            string updateEmail = txtUpdateemail.Text.Trim();
            string updateDOB = datePickerDOB.Text.Trim();


            try
            {
                sqlcon.Open(); 
                string customerquery = "SELECT COUNT(1) FROM Customer WHERE CustomeRID = @CustomeRID";
                SqlCommand checkCmd = new SqlCommand(customerquery, sqlcon);
                checkCmd.Parameters.AddWithValue("@CustomeRID", updateCustomerID);

                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists == 0)
                {
                    MessageBox.Show("Customer ID does not exist.", "Update Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string updateQuery = "UPDATE Customer SET CusName = @CusName, C_Address = @C_Address, Cus_Tele = @Cus_Tele,  CusNic = @CusNic, Cemail = @Cemail, BirthOfDate = @BirthOfDate, UpdateDate = @UpdateDate WHERE CustomeRID = @CustomeRID";
                SqlCommand updateCmd = new SqlCommand(updateQuery, sqlcon);
                updateCmd.Parameters.AddWithValue("@CusName", updateName);
                updateCmd.Parameters.AddWithValue("@C_Address", updateAddress);
                updateCmd.Parameters.AddWithValue("@Cus_Tele", updateMobile);
                updateCmd.Parameters.AddWithValue("@CusNic", updateNIC);
                updateCmd.Parameters.AddWithValue("@Cemail", updateEmail);
                updateCmd.Parameters.AddWithValue("@BirthOfDate", UpdatedatePickerDOB);
                updateCmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Customer details updated successfully.", "Update Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearUpdateFields();
                }
                else
                {
                    MessageBox.Show("Failed to update customer details.", "Update customer", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void ClearUpdateFields()
        {
            txtUpdateName.Text = "";
            txtUpdateAddress.Text = "";
            txtUpdatemobile.Text = "";
            txtUpdatenic.Text = "";
            txtUpdateemail.Text = "";
            datePickerDOB.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlcon.Open();

                string customerId = txtCusId.Text.Trim();
                string name = txtName.Text.Trim();
                string mobile = txtmobile.Text.Trim();
                string nic = txtnic.Text.Trim();
                string address = txtAddress.Text.Trim();
                string email = txtemail.Text.Trim();
                string dob = datePickerDOB.Text.Trim();

                if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(nic) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(dob))
                {
                    MessageBox.Show("Please fill all fields", "Customer Details", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
             

                else
                {
                    string room = "SELECT COUNT(1) FROM Customer WHERE CustomeRID = @CustomeRID";
                    SqlCommand Cmd = new SqlCommand(room, sqlcon);
                    Cmd.Parameters.AddWithValue("@CustomeRID", customerId);

                    int exists = Convert.ToInt32(Cmd.ExecuteScalar());
                    if (exists > 0)
                    {
                        MessageBox.Show("Customer ID is already available. Please use a different Customer ID.", "Customer Details", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string query = "INSERT INTO Customer (CustomeRID, CusName, Cus_Tele, CusNic, C_Address, Cemail,BirthOfDate,CreateDate,UpdateDate) VALUES (@CustomeRID, @CusName, @Cus_Tele, @CusNic, @C_Address,@Cemail, @BirthOfDate,@CreateDate,@UpdateDate)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlcon);

                    sqlCmd.Parameters.AddWithValue("@CustomeRID", customerId);
                    sqlCmd.Parameters.AddWithValue("@CusName", name);
                    sqlCmd.Parameters.AddWithValue("@Cus_Tele", mobile);
                    sqlCmd.Parameters.AddWithValue("@CusNic", nic);
                    sqlCmd.Parameters.AddWithValue("@C_Address", address);
                    sqlCmd.Parameters.AddWithValue("@Cemail", email);
                    sqlCmd.Parameters.AddWithValue("@BirthOfDate", dob);
                    sqlCmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);

                    int rowsAffected = sqlCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer details added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        clearFiled();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer details", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Search2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Search2_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
