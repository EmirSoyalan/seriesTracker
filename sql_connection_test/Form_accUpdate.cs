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
    public partial class Form_accUpdate : Form
    {
        public Form_accUpdate()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        ErrorProvider error = new ErrorProvider();
        bool err;
        public int user_id;
        private void button1_Click(object sender, EventArgs e)
        {
            error.Clear();
            err = false;
            foreach (Control x in Controls)
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
            if (!err)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE users SET user_name=@user_name, password=@password WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", user_id);
                cmd.Parameters.AddWithValue("@password", textBox2.Text);
                cmd.Parameters.AddWithValue("@user_name", textBox1.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("changed");
                Close();
            }
        }

        private void Form_accUpdate_Load(object sender, EventArgs e)
        {
            // Color adjustment
            con.Open();
            SqlCommand cmd_set = new SqlCommand("SELECT * FROM settings WHERE user_id = '" + user_id + "'", con);
            SqlDataReader settingReader = cmd_set.ExecuteReader();
            while (settingReader.Read())
            {
                if (settingReader["using_pre"].ToString().Trim() == "0")
                {
                    BackColor = Color.FromArgb(Convert.ToInt32(settingReader["bg_color"]));
                }
                else
                {
                    Color bg_color;
                    switch (settingReader["bg_theme"].ToString().Trim())
                    {
                        case "Dark":
                            bg_color = SystemColors.WindowFrame;
                            break;
                        case "Light":
                            bg_color = Color.LightGray;
                            break;
                        default:
                            bg_color = SystemColors.Control;
                            break;
                    }
                    BackColor = bg_color;
                }
            }
            con.Close();
            ////////////
        }
    }
}
