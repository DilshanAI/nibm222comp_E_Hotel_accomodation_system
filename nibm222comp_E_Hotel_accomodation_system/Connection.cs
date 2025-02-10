﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace nibm222comp_E_Hotel_accomodation_system
{
    class Connection
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter da;


        public static readonly string ConnectionString =
    @"Data Source=PAVITHRA432;Initial Catalog=HotelAccomodationSystem;Integrated Security=True;TrustServerCertificate=True";


        public Connection() // Default Constructor
        {
            con = new SqlConnection(ConnectionString);
        }
        public void openConnection()
        {
            con.Open();
        }
        public void closeConnection()
        {
            con.Close();
        }

        public DataTable getData(string a)
        {
            openConnection();
            da = new SqlDataAdapter(a, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            closeConnection();
            return dt;
        }
        public void executeQuery(string query)
        {
            try
            {
                openConnection();
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                closeConnection();
            }
        }

    }
}
