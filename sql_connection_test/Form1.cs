using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
/*
 * 
 *  TODO: 
 *  clear boxes when new series has been added
 *  clear the box when new genre added
 *  suggest existing genres when typing
 *  stylize
 *  check for invalid inputs
 *  adjust inputs (trim, lowercase etc.)
 * 
 */
namespace sql_connection_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        ErrorProvider error = new ErrorProvider();

        private int getCountOfTable(string table)
        {
            con.Open();
            int count = 0;
            SqlCommand cmd_count = new SqlCommand();
            cmd_count.CommandText = "SELECT * FROM " + table;
            cmd_count.Connection = con;
            cmd_count.CommandType = CommandType.Text;
            SqlDataReader reader = cmd_count.ExecuteReader();
            while (reader.Read())
            {
                count++;
            }
            con.Close();
            return count;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder genres = new StringBuilder();
            foreach (string name in comboBox1.Items)
            {
                genres.Append(name);
                genres.Append(", ");
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO series (id,name,genre,episodes,current_episode,rating,description) VALUES (@id,@name,@genre,@episodes,@current_episode,@rating,@description)",con);
            cmd.Parameters.AddWithValue("@id", getCountOfTable("series"));
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@genre", genres.ToString());
            cmd.Parameters.AddWithValue("@episodes", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@current_episode", numericUpDown2.Value);
            cmd.Parameters.AddWithValue("@rating", comboBox2.Items[comboBox2.SelectedIndex]);
            cmd.Parameters.AddWithValue("@description", ""+richTextBox1.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            /* SELECT*/
            /*
            con.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM series",con);
            SqlDataReader reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                textBox3.Text = reader["id"].ToString();
                textBox2.Text = reader["genre"].ToString();
                textBox1.Text = reader["episodes"].ToString();
                textBox6.Text = reader["current_episode"].ToString();
                textBox5.Text = reader["rating"].ToString();
                textBox4.Text = reader["description"].ToString();
            }
            con.Close();
            */
            MessageBox.Show("The Series have been succesfully added.", "ADDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex >= 0)
            {
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
            }
            else
            {
                //error.SetError();
            }
            
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Add(textBox2.Text);
            comboBox1.SelectedIndex = comboBox1.Items.Count-1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Hide();
        }
    }
}
