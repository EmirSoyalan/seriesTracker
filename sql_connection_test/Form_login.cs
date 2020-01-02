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
using System.Text.RegularExpressions;

namespace sql_connection_test
{
    public partial class Form_login : Form
    {
        public Form_login()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        ErrorProvider error = new ErrorProvider();
        //string invalidChars = @"";
        //Regex.Matches(string,invalidChars).Count>0
        bool err = false;
        private void Form_login_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[1];
            con.Open();
            SqlCommand cmd_check = new SqlCommand("SELECT * FROM users",con);
            SqlDataReader reader = cmd_check.ExecuteReader();
            while (reader.Read())
            {
                if (reader["remember"].ToString() == "1")
                {
                    Form_main frm = new Form_main { user_id = int.Parse(reader["id"].ToString()) };
                    con.Close();
                    frm.Show();
                    Visible = false;
                    ShowInTaskbar = false;
                    break;
                }
            }
            con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(textBox2.PasswordChar == '*')
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
                    //x.Enter += { error.Clear(); };
                    if (String.IsNullOrWhiteSpace(x.Text.Trim()))
                    {
                        error.SetError(x, "Please enter a correct value");
                        err = true;
                        continue;
                    }
                    /*
                    if (char.IsDigit(x.Text[0]) && x.Name != "textBox2")
                    {
                        error.SetError(x, "First letter can only be letter");
                        err = true;
                    }
                    */
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
            int id=0;
            if (!err)
            {
                con.Open();
                SqlCommand cmd_check = new SqlCommand("SELECT * FROM users", con);
                SqlDataReader reader = cmd_check.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["user_name"].ToString().Trim() == textBox1.Text.Trim() && reader["password"].ToString().Trim() == textBox2.Text.Trim())
                    {
                        MessageBox.Show("You've succesfully logged in!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        id = int.Parse(reader["id"].ToString());
                        Form_main frm = new Form_main { user_id = id };
                        frm.Show();
                        Hide();
                        break;
                    }
                }
                con.Close();
                error.SetError(button1, "User name or password is wrong");
                if (checkBox1.Checked)
                {
                    con.Open();
                    SqlCommand cmd_rem = new SqlCommand("UPDATE users SET remember=1 WHERE id = '" + id + "'", con);
                    cmd_rem.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_register frmrgt = new Form_register();
            frmrgt.Show();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            error.Clear();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            error.Clear();
        }
    }
}
