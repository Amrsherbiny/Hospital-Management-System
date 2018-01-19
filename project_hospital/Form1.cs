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
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        Add_Doctor add_doctor;
        Add_Nurse add_nurse;
        Add_Room add_room;
        Add_Patient add_patient;
        Search_Doctors search_doctors;
        public Form1()
        {
            InitializeComponent();
            add_doctor = new Add_Doctor();
            add_nurse = new Add_Nurse();
            add_room = new Add_Room();
            add_patient = new Add_Patient();
            search_doctors = new Search_Doctors();
            connectionString = ConfigurationManager.ConnectionStrings["project_hospital.Properties.Settings.hospitalConnectionString"].ConnectionString;
        }
        private void Search_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Doctors")
                search_doctors.Show();
        }
        private void Insert_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Doctor")
                add_doctor.Show();
            else
                if (comboBox2.Text == "Nurse")
                    add_nurse.Show();
                else
                    if (comboBox2.Text == "Patient")
                        add_patient.Show();
                    else
                        if (comboBox2.Text == "Room")
                            add_room.Show();
                        else
                            MessageBox.Show("Please select a type to insert");
        }
       /* private void Form1_Load(object sender, EventArgs e)
        {
            fill();
        }
        private void fill()
        {
            using(connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("select * from Doctors", connection))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);

                listBox1.DisplayMember = "Name";
                listBox1.ValueMember = "Id";
                listBox1.DataSource = table; 
            }
        }*/
    }
}
