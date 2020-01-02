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
    public partial class Form_user : Form
    {
        public Form_user()
        {
            InitializeComponent();
        }
        void reArrangeDatabaseID(int removedID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE series SET id = id - 1 WHERE id > '" + removedID + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        public int user_id;
        private void Form_user_Load(object sender, EventArgs e)
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
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE id = @id",con);
            cmd.Parameters.AddWithValue("@id",user_id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                label2.Text = reader["user_name"].ToString();
                label4.Text = reader["password"].ToString();
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form_accUpdate fmr = new Form_accUpdate { user_id = this.user_id };
            fmr.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete your account?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (MessageBox.Show("Really?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand cmd_deleteShows = new SqlCommand("DELETE FROM series WHERE user_id='" + user_id + "' ", con);
                    cmd_deleteShows.ExecuteNonQuery();
                    SqlCommand cmd_deleteSet = new SqlCommand("DELETE FROM settings WHERE user_id='" + user_id + "' ", con);
                    cmd_deleteSet.ExecuteNonQuery();
                    SqlCommand cmd_deleteAcc = new SqlCommand("DELETE FROM users WHERE id='" + user_id + "' ", con);
                    cmd_deleteAcc.ExecuteNonQuery();
                    SqlCommand cmd = new SqlCommand("UPDATE series SET id = id - 1 WHERE id > '" + user_id + "'", con);
                    cmd.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("UPDATE users SET id = id - 1  WHERE id > '" + user_id + "'", con);
                    cmd2.ExecuteNonQuery();
                    SqlCommand cmd3 = new SqlCommand("UPDATE settings SET user_id = user_id - 1 WHERE user_id > '" + user_id + "'", con);
                    cmd3.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Your account has been deleted","Success?",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
                    Application.Exit();
                }
            } 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all of the series?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                con.Open();
                SqlCommand cmd_resetShows = new SqlCommand("Delete from series where user_id='" + user_id + "' ", con);
                cmd_resetShows.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("All shows have been deleted");
                reArrangeDatabaseID(-1);
            }
        }
    }
}
