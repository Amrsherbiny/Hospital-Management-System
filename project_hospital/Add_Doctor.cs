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
    public partial class Add_Doctor : Form
    {
        string connectionString;
        SqlConnection connection;
        public Add_Doctor()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["project_hospital.Properties.Settings.hospitalConnectionString"].ConnectionString;
            display();
        }
        private void add_doctor(object x, EventArgs y)
        {
            string query = "INSERT INTO Doctors Values(@Name, @Id, @speciality) ";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", textBox1.Text);
                command.Parameters.AddWithValue("@Id", int.Parse(textBox2.Text));
                command.Parameters.AddWithValue("@speciality", textBox3.Text);
                command.ExecuteNonQuery();
            }
            display();
        }
        private void display()
        {
            using (connection = new SqlConnection(connectionString))
            {
                DataTable table = new DataTable();
                SqlDataAdapter adapt = new SqlDataAdapter("select * from Doctors", connection);
                adapt.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        /*private void Search_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Speciality")
            {
                //string spec = textBox4.Text; //entered speciality
                string query = "SELECT * FROM Doctors WHERE Speciality like'"+textBox4.Text+"%'";
                using (connection = new SqlConnection(connectionString))
                {
                    DataTable table = new DataTable();
                    SqlDataAdapter adapt = new SqlDataAdapter(query, connection);
                    adapt.Fill(table);
                    dataGridView2.DataSource = table;
                }
            }
            else
                if (comboBox1.Text == "Name")
                {
                    string query = "SELECT * FROM Doctors WHERE Name like'" + textBox4.Text + "%'";
                    using (connection = new SqlConnection(connectionString))
                    {
                        DataTable table = new DataTable();
                        SqlDataAdapter adapt = new SqlDataAdapter(query, connection);
                        adapt.Fill(table);

                        dataGridView2.DataSource = table;
                    }
                }
        }*/
    }
}
