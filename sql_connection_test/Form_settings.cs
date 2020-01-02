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
    public partial class Form_settings : Form
    {
        public Form_settings()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        public int user_id=0;
        private void Form_settings_Load(object sender, EventArgs e)
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
            button1.Hide();
            button2.Hide();
            button3.Hide();
            button4.Hide();
            button5.Hide();
            button8.Hide();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            con.Open();
            SqlCommand cmd_c = new SqlCommand("UPDATE settings SET using_pre=0, bg_color = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
            cmd_c.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Background Color has changed. Reopen the application to see the effects","Success",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            con.Open();
            SqlCommand cmd_c = new SqlCommand("UPDATE settings SET using_pre=0, color_1 = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
            cmd_c.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Color has changed. Reopen the application to see the effects", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            con.Open();
            SqlCommand cmd_c = new SqlCommand("UPDATE settings SET using_pre=0, color_2 = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
            cmd_c.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Color has changed. Reopen the application to see the effects", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            con.Open();
            SqlCommand cmd_c = new SqlCommand("UPDATE settings SET using_pre=0, color_3 = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
            cmd_c.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Color has changed. Reopen the application to see the effects", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            con.Open();
            SqlCommand cmd_c = new SqlCommand("UPDATE settings SET using_pre=0, color_4 = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
            cmd_c.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Color has changed. Reopen the application to see the effects", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool valid = false;
            con.Open();
            SqlCommand cmd_check = new SqlCommand("SELECT * FROM settings", con);
            SqlDataReader read = cmd_check.ExecuteReader();
            while (read.Read())
            {
                if(read["user_id"].ToString().Trim() == user_id.ToString())
                {
                    valid = true;
                }
            }
            con.Close();
            if (valid)
            {
                con.Open();
                SqlCommand cmd_c = new SqlCommand("UPDATE settings SET using_pre=1, bg_theme = '" + comboBox1.SelectedItem + "' WHERE user_id = '" + user_id + "'", con);
                cmd_c.ExecuteNonQuery();
                MessageBox.Show("Color has changed. Refresh table or reopen the application to see the effects", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                con.Close();
            }
            else
            {
                con.Open();
                SqlCommand cmd_add = new SqlCommand("INSERT INTO settings(user_id,color_1,color_2,color_3,color_4,bg_color,bg_theme,using_pre) VALUES(@user_id,@color_1,@color_2,@color_3,@color_4,@bg_color,@bg_theme,@using_pre) ", con);
                cmd_add.Parameters.AddWithValue("@user_id",user_id);
                cmd_add.Parameters.AddWithValue("@color_1", 0);
                cmd_add.Parameters.AddWithValue("@color_2", 0);
                cmd_add.Parameters.AddWithValue("@color_3", 0);
                cmd_add.Parameters.AddWithValue("@color_4", 0);
                cmd_add.Parameters.AddWithValue("@bg_color", 0);
                cmd_add.Parameters.AddWithValue("@bg_theme", 0);
                cmd_add.Parameters.AddWithValue("@using_pre", 0);
                cmd_add.ExecuteNonQuery();
                con.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Choose a background color","Custom Theme",MessageBoxButtons.OK,MessageBoxIcon.Asterisk,MessageBoxDefaultButton.Button1);
            colorDialog1.ShowDialog();
            con.Open();
            SqlCommand cmd_c1 = new SqlCommand("UPDATE settings SET using_pre=0, bg_color = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
            cmd_c1.ExecuteNonQuery();
            con.Close();
            for (int i = 1; i < 5; i++)
            {
                string which="";
                switch (i)
                {
                    case 1: which = "Watching"; break;
                    case 2: which = "Watched"; break;
                    case 3: which = "Gonna Watch"; break;
                    case 4: which = "Dropped"; break;
                }
                if(MessageBox.Show("Choose color of \"" + which + "\" series", "Custom Theme", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    colorDialog1.ShowDialog();
                }
                con.Open();
                SqlCommand cmd_c = new SqlCommand("UPDATE settings SET " + "color_"+i.ToString() + " = '" + colorDialog1.Color.ToArgb().ToString() + "' WHERE user_id = '" + user_id + "'", con);
                cmd_c.ExecuteNonQuery();
                con.Close();
            }
            MessageBox.Show("Custom theme has been made. Refresh table or ropen the application to see the effects", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            /*
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "SQL Server Dosyası |*.dbo";
            file.ShowDialog();
            */
        }
    }
}
