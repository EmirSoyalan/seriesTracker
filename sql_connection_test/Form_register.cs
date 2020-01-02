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

namespace sql_connection_test
{
    public partial class Form_register : Form
    {
        public Form_register()
        {
            InitializeComponent();
        }
        // TODO: move picturebox's click event to mouse hover event
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        ErrorProvider error = new ErrorProvider();
        bool err;
        private void Form_register_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[1];
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = '\0';
                pictureBox1.Image = imageList1.Images[0];

            }
            else
            {
                textBox2.PasswordChar = '*';
                pictureBox1.Image = imageList1.Images[1];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            error.Clear();
            err = false;
            foreach (Control x in groupBox1.Controls)
            {
                if (x is TextBox)
                {
                    if (String.IsNullOrWhiteSpace(x.Text.Trim()))
                    {
                        error.SetError(x, "Please enter a correct value");
                        err = true;
                        continue;
                    }
                    if (char.IsDigit(x.Text[0]) && x.Name == "textBox1")
                    {
                        error.SetError(x, "First letter can only be letter");
                        err = true;
                        continue;
                    }
                    if (textBox2.Text != textBox3.Text)
                    {
                        error.SetError(textBox3, "Passwords should match");
                        err = true;
                    }
                    if ((x.Name == "textBox2" || x.Name == "textBox3") && x.Text.Length < 4)
                    {
                        error.SetError(x, "Password should be at least 4 characters");
                        err = true;
                        continue;
                    }
                    for (int i = 0; i < x.Text.Length; i++)
                    {
                        if (!char.IsLetterOrDigit(x.Text[i]))
                        {
                            error.SetError(x, "You can only use letters and numbers");
                            err = true;
                            break;
                        }
                    }
                }
            }
            con.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM users", con);
            SqlDataReader reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                if (textBox1.Text == reader["user_name"].ToString().Trim())
                {
                    error.SetError(textBox1, "That user name has taken");
                    err = true;
                    break;
                }
            }
            con.Close();
            if (!err)
            {
                myClass cls = new myClass();
                SqlCommand cmd = new SqlCommand("INSERT INTO users(id,user_name,password,remember) VALUES (@id,@user_name,@password,@remember)", con);
                cmd.Parameters.AddWithValue("@id", cls.getCountOfTable(con, "users"));
                cmd.Parameters.AddWithValue("@user_name", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", textBox2.Text);
                cmd.Parameters.AddWithValue("@remember", 0);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("You've succesfully signed in!", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
