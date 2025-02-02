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
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : UserControl
    {
        SqlConnection sqlcon = new SqlConnection(Connection.ConnectionString);
        string selectedRoomType = "0";
        string selectedBedType = "0";
        public Employee()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlcon.Open();

                string employeeId = txtempId.Text.Trim();
                string name = txtName.Text.Trim();
                string mobile = txtmobile.Text.Trim();
                string nic = txtNIC.Text.Trim();
                string address = txtAddress.Text.Trim();
                string email = txtemail.Text.Trim();
                string designation = (designationComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();



                if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(nic) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(designation))
                {
                    MessageBox.Show("Please fill all fields", "Employee Details", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                else
                {
                    string room = "SELECT COUNT(1) FROM Employee WHERE EmployeeID = @EmployeeID";
                    SqlCommand Cmd = new SqlCommand(room, sqlcon);
                    Cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

                    int exists = Convert.ToInt32(Cmd.ExecuteScalar());
                    if (exists > 0)
                    {
                        MessageBox.Show("Employee ID is already available. Please use a different Employee ID.", "Employee Details", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string query = "INSERT INTO Employee (EmployeeID, Name, Mobile, NIC, Address, Email,CreateDate,UpdateDate,Designation) VALUES (@EmployeeID, @Name, @Mobile, @NIC, @Address,@Email,@CreateDate,@UpdateDate ,@Designation )";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlcon);

                    sqlCmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                    sqlCmd.Parameters.AddWithValue("@Name", name);
                    sqlCmd.Parameters.AddWithValue("@Mobile", mobile);
                    sqlCmd.Parameters.AddWithValue("@NIC", nic);
                    sqlCmd.Parameters.AddWithValue("@Address", address);
                    sqlCmd.Parameters.AddWithValue("@Email", email);
                    sqlCmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                    sqlCmd.Parameters.AddWithValue("@Designation", designation);

                    int rowsAffected = sqlCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employee details added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
        
        private void clearFiled()
        {
            txtempId.Text = "";
            txtName.Text = "";
            txtmobile.Text = "";
            txtNIC.Text = "";
            txtAddress.Text = "";
            txtemail.Text = "";
            designationComboBox.Text = "";
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string updateEmpID = txtUpdateEmpId.Text.Trim();
            string updateName = txtUpdateName.Text.Trim();
            string updateAddress = txtUpdateAddress.Text.Trim();
            string updateMobile = txtUpdatemobile.Text.Trim();
            string updateNIC = txtUpdatenic.Text.Trim();
            string updateEmail = txtUpdateemail.Text.Trim();
            //string updateDesignation = designationComboBox.Text.Trim();
            string updateDesignation = (designationComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(updateEmpID))
            {
                MessageBox.Show("Please enter an Employee ID.", "Update Employee", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                sqlcon.Open();

                // Check if EmployeeID exists
                string empquery = "SELECT COUNT(1) FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand checkCmd = new SqlCommand(empquery, sqlcon);
                checkCmd.Parameters.AddWithValue("@EmployeeID", updateEmpID);

                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists == 0)
                {
                    MessageBox.Show("Employee ID does not exist.", "Update Employee", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Corrected UPDATE query with properly mapped parameters
                string updateQuery = @"UPDATE Employee 
                          SET Name = @Name, 
                              Mobile = @Mobile, 
                              NIC = @NIC,  
                              Address = @Address, 
                              Email = @Email, 
                              UpdateDate = @UpdateDate, 
                              Designation = @Designation 
                          WHERE EmployeeID = @EmployeeID";

                SqlCommand updateCmd = new SqlCommand(updateQuery, sqlcon);
                updateCmd.Parameters.AddWithValue("@Name", updateName);
                updateCmd.Parameters.AddWithValue("@Mobile", updateMobile);
                updateCmd.Parameters.AddWithValue("@NIC", updateNIC);
                updateCmd.Parameters.AddWithValue("@Address", updateAddress);
                updateCmd.Parameters.AddWithValue("@Email", updateEmail);
                updateCmd.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@Designation", updateDesignation);
                updateCmd.Parameters.AddWithValue("@EmployeeID", updateEmpID); // Missing in original

                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Employee details updated successfully.", "Update Employee", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearUpdateFields();
                }
                else
                {
                    MessageBox.Show("Failed to update employee details.", "Update Employee", MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (sqlcon.State == ConnectionState.Open)
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
            designationComboBox.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string EmployeeID = txtUpdateEmpId.Text.Trim();

            if (string.IsNullOrEmpty(EmployeeID))
            {
                MessageBox.Show("Please enter a Employee ID.", "Search Employee", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                sqlcon.Open();
                string query = "SELECT Name,Mobile, NIC,Address,Email,Designation FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtUpdateName.Text = reader["Name"].ToString();
                    txtUpdateAddress.Text = reader["Mobile"].ToString();
                    txtUpdatemobile.Text = reader["NIC"].ToString();
                    txtUpdatenic.Text = reader["Address"].ToString();
                    txtUpdateemail.Text = reader["Email"].ToString();
                    designationComboBox.Text = reader["Designation"].ToString();
                }
                else
                {
                    MessageBox.Show("Employee ID does not exist.", "Search Employee", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
