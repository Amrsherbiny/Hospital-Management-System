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
    public partial class Add_Patient : Form
    {
        string connectionString;
        SqlConnection connection;
        public Add_Patient()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["project_hospital.Properties.Settings.hospitalConnectionString"].ConnectionString;
            display();
            fill_listbox();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Patients Values(@Name, @Number, @Diagnosis, @DoctorID, @NurseID, @RoomNumber) ";
            int rows_affected = 0;
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", textBox1.Text);
                    command.Parameters.AddWithValue("@Number", int.Parse(textBox2.Text));
                    command.Parameters.AddWithValue("@Diagnosis", textBox3.Text);

                    command.Parameters.AddWithValue("@DoctorID", int.Parse(textBox4.Text));
                    SqlCommand Check_drs = new SqlCommand("SELECT * FROM Doctors WHERE ([ID] = @ID)", connection); //check if a doctor with such ID exists
                    Check_drs.Parameters.AddWithValue("@ID", int.Parse(textBox4.Text));
                    SqlDataReader reader = Check_drs.ExecuteReader();//returns number of rows 
                    if (!reader.HasRows)
                        MessageBox.Show("Doctor Id Doesn't Exist! ");
                    reader.Close();

                    command.Parameters.AddWithValue("@NurseID", int.Parse(textBox5.Text)); //check if a nurse with such ID exists
                    SqlCommand check_nurse = new SqlCommand("SELECT * FROM Nurses WHERE ([ID] = @ID)", connection);
                    check_nurse.Parameters.AddWithValue("@ID", int.Parse(textBox5.Text));
                    SqlDataReader reader2 = check_nurse.ExecuteReader();
                    if (!reader2.HasRows)
                        MessageBox.Show("Nurse ID doesn't exist");
                    reader2.Close();

                    command.Parameters.AddWithValue("@RoomNumber", int.Parse(textBox6.Text));
                    SqlCommand check_rooms = new SqlCommand("SELECT * FROM Rooms WHERE ([RoomNumber] = @RoomNumber)", connection); //checks if this room exists/available
                    check_rooms.Parameters.AddWithValue("@RoomNumber",int.Parse(textBox6.Text));
                    SqlDataReader reader3 = check_rooms.ExecuteReader();
                    if(!reader3.HasRows)
                        MessageBox.Show("Room is not available");
                    reader3.Close();
                    try
                    {
                        rows_affected = command.ExecuteNonQuery(); //will return 1 if the statements are executed
                    }
                    catch
                    {
                        MessageBox.Show("try again");
                    }
                }
                if (rows_affected != 0)
                {
                    query = "UPDATE Rooms Set Vacant = @Vacant Where RoomNumber = @RoomNumber"; //changed the rooms from vacant to taken
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomNumber", int.Parse(textBox6.Text));
                        command.Parameters.AddWithValue("@Vacant", "False");
                        command.ExecuteNonQuery();
                        fill_listbox();
                        rows_affected = 0;
                        clear_textboxes();
                    }
                }
            }
            display();
        }
        private void display()  //displays all patients in database
        {
            using (connection = new SqlConnection(connectionString)) 
            {
                DataTable table = new DataTable();
                SqlDataAdapter adapt = new SqlDataAdapter("select * from Patients", connection);
                adapt.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        public void fill_listbox() //displays all vacant rooms
        {
            using (connection = new SqlConnection(connectionString))
            {
                DataTable table = new DataTable();
                SqlDataAdapter adapt = new SqlDataAdapter("select * from Rooms Where Vacant = 'True'", connection);
                adapt.Fill(table);

                listBox1.DisplayMember = "RoomNumber";
                listBox1.DataSource= table;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Patients WHERE Name like'" + textBox1.Text + "%'" ;
            int rows_affected = 0;
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    rows_affected = command.ExecuteNonQuery();
                    display();
                }
                catch
                {
                    MessageBox.Show("Can't delete Patient \nPatient May not exist in Database");
                }
            }
            if (rows_affected != 0)
            {
                query = "UPDATE Rooms Set Vacant = @Vacant Where RoomNumber = @RoomNumber"; //changed the rooms from taken to Vacant
                using (connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    try
                    {
                        command.Parameters.AddWithValue("@RoomNumber", int.Parse(textBox6.Text));
                        command.Parameters.AddWithValue("@Vacant", "True");
                        command.ExecuteNonQuery();
                        fill_listbox();
                        rows_affected = 0;
                    }
                    catch
                    {
                        MessageBox.Show("Try again");
                    }
                }
            }
        }
        public void clear_textboxes()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = "";
        }
        private void Add_Patient_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
