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
    public partial class Add_Nurse : Form
    {
        string connectionString;
        SqlConnection connection;
        public Add_Nurse()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["project_hospital.Properties.Settings.hospitalConnectionString"].ConnectionString;
            display();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Nurses Values(@Name, @Id) ";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", textBox1.Text);
                command.Parameters.AddWithValue("@Id", int.Parse(textBox2.Text));
                //command.Parameters.AddWithValue("@Speciality", textBox3.Text);
                command.ExecuteNonQuery();
            }
            display();
        }
        private void display()
        {
            using (connection = new SqlConnection(connectionString))
            {
                DataTable table = new DataTable();
                SqlDataAdapter adapt = new SqlDataAdapter("select * from Nurses", connection);
                adapt.Fill(table);
                dataGridView1.DataSource = table;
            } 
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Nurses WHERE Name like'" + textBox1.Text + "%'";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    display();
                }
                catch
                {
                    MessageBox.Show("Can't delete Nurse \nNurse may be assigned to a patient");
                }
            }
        }
    }
}
