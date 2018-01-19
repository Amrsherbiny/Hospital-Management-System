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
    public partial class Add_Room : Form
    {
        string connectionString;
        SqlConnection connection;
        public Add_Room()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["project_hospital.Properties.Settings.hospitalConnectionString"].ConnectionString;
            display();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Rooms Values(@RoomNumber, @Vacant) ";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@RoomNumber", textBox1.Text);
                command.Parameters.AddWithValue("@Vacant", textBox2.Text);
                //command.Parameters.AddWithValue("@Speciality", textBox3.Text);
                command.ExecuteNonQuery();
            }
            textBox1.Text = textBox2.Text = "";
            display();
        }
        private void display()
        {
            using (connection = new SqlConnection(connectionString))
            {
                DataTable table = new DataTable();
                SqlDataAdapter adapt = new SqlDataAdapter("select * from Rooms", connection);
                adapt.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Rooms WHERE RoomNumber like'" + int.Parse(textBox1.Text) + "%'";
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
                    MessageBox.Show("Can't delete Room \nRoom May not exist in Database");
                }
            } 
        }
        private void Add_Room_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}