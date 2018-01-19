using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace project_hospital
{
    public partial class Search_Doctors : Form
    {
        string connectionString;
        SqlConnection connection;
        public Search_Doctors()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["project_hospital.Properties.Settings.hospitalConnectionString"].ConnectionString;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Speciality")
            {
                //string spec = textBox4.Text; //entered speciality
                string query = "SELECT * FROM Doctors WHERE Speciality like'" + textBox1.Text + "%'";
                using (connection = new SqlConnection(connectionString))
                {
                    DataTable table = new DataTable();
                    SqlDataAdapter adapt = new SqlDataAdapter(query, connection);
                    adapt.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            else
                if (comboBox1.Text == "Name")
                {
                    string query = "SELECT * FROM Doctors WHERE Name like'" + textBox1.Text + "%'";
                    using (connection = new SqlConnection(connectionString))
                    {
                        DataTable table = new DataTable();
                        SqlDataAdapter adapt = new SqlDataAdapter(query, connection);
                        adapt.Fill(table);

                        dataGridView1.DataSource = table;
                    }
                    query = "SELECT ID FROM DOCTORS WHERE Name Like '" + textBox1.Text +"%'";
                    using (connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand();
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "SELECT ID FROM DOCTORS WHERE Name Like '" + textBox1.Text + "%'";
                        int id = (int)(command.ExecuteScalar());
                        fill_listbox(id);
                    }
                }
                else
                    MessageBox.Show("Please choose type to search with");
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Doctors WHERE Name like'"+textBox1.Text+"%'";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Can't delete Doctor\nDoctor May be associated with a patient");
                }
            }
        }
        private void fill_listbox(int ID)
        {
            string query = "Select * From Patients Where DoctorID like'" + ID +"%'";
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query,connectionString))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                listBox1.DisplayMember = "Name";
                listBox1.DataSource = table;
                
            }
        }
    }
}
