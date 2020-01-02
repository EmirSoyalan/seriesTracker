using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace sql_connection_test
{
    public partial class Form_main : Form
    {
        public Form_main()
        {
            InitializeComponent();
        }
        public int user_id;
        int columnCount;
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        DataGridViewButtonColumn updateButtonColumn = new DataGridViewButtonColumn();
        DataGridViewButtonColumn watchedButtonColumn = new DataGridViewButtonColumn();
        public void updateTable()
        {
            con.Open();
            SqlCommand cmd_list = new SqlCommand("SELECT id,name,genre,episodes,current_episode,rating,description,situation FROM series WHERE user_id = '" + user_id + "'", con);
            SqlDataAdapter adap = new SqlDataAdapter(cmd_list);
            DataTable dt = new DataTable();
            adap.Fill(dt);
            this.dataGridView2.DataSource = dt;
            con.Close();
            columnCount = dt.Columns.Count;
            // Color adjustment
            con.Open();
            SqlCommand cmd_set = new SqlCommand("SELECT * FROM settings WHERE user_id = '" + user_id + "'", con);
            SqlDataReader settingReader = cmd_set.ExecuteReader();
            while (settingReader.Read())
            {
                if (settingReader["using_pre"].ToString().Trim() == "0")
                {
                    BackColor = Color.FromArgb(Convert.ToInt32(settingReader["bg_color"]));
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        string RowType = row.Cells["situation"].Value.ToString();
                        if (RowType == "Watching")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(Convert.ToInt32(settingReader["color_1"]));
                        }
                        else if (RowType == "Watched")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(Convert.ToInt32(settingReader["color_2"]));
                        }
                        else if (RowType == "Gonna Watch")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(Convert.ToInt32(settingReader["color_3"]));
                        }
                        else if (RowType == "Dropped")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(Convert.ToInt32(settingReader["color_4"]));
                        }
                    }
                }
                else
                {
                    Color color1, color2, color3, color4, bg_color;
                    switch (settingReader["bg_theme"].ToString().Trim())
                    {
                        case "Dark":
                            color1 = Color.DarkViolet;
                            color2 = SystemColors.MenuHighlight;
                            color3 = SystemColors.WindowFrame;
                            color4 = SystemColors.ControlLightLight;
                            bg_color = SystemColors.WindowFrame;
                            break;
                        case "Light":
                            color1 = Color.LightGreen;
                            color2 = Color.LightBlue;
                            color3 = Color.LightYellow;
                            color4 = Color.LightPink;
                            bg_color = SystemColors.Control;
                            break;
                        default:
                            color1 = Color.LightGreen;
                            color2 = Color.Orange;
                            color3 = Color.LightPink;
                            color4 = Color.BlanchedAlmond;
                            bg_color = SystemColors.Control;
                            break;
                    }
                    BackColor = bg_color;
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        string RowType = row.Cells["situation"].Value.ToString();
                        if (RowType == "Watching")
                        {
                            row.DefaultCellStyle.BackColor = color1;
                        }
                        else if (RowType == "Watched")
                        {
                            row.DefaultCellStyle.BackColor = color2;
                        }
                        else if (RowType == "Gonna Watch")
                        {
                            row.DefaultCellStyle.BackColor = color3;
                        }
                        else if (RowType == "Dropped")
                        {
                            row.DefaultCellStyle.BackColor = color4;
                        }
                    }
                }
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // VALIDATION?
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            updateTable();
            comboBox1.SelectedIndex = 0;
            if (dataGridView2.Columns["Adjust"] == null && dataGridView2.Columns["Increase"] == null)
            {
                updateButtonColumn.Width = 60;
                watchedButtonColumn.Width = 20;
                updateButtonColumn.Name = "Adjust";
                updateButtonColumn.Text = "Update";
                watchedButtonColumn.Name = "Increase";
                watchedButtonColumn.Text = "+";
                updateButtonColumn.UseColumnTextForButtonValue = true;
                watchedButtonColumn.UseColumnTextForButtonValue = true;
                dataGridView2.Columns.Insert(5, watchedButtonColumn);
                dataGridView2.Columns.Insert(9, updateButtonColumn);
            }
            updateTable();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.Columns["id"].Visible = false;
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Adjust"].Index && e.RowIndex != -1)
            {
                Form_update frm_up = new Form_update(this)
                {
                    id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString()),
                    name = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString(),
                    genre = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString(),
                    episodes = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString(),
                    current_episode = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString(),
                    rating = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString(),
                    description = dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString(),
                    situation = dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString()
                };
                frm_up.Show();
            }
            if (e.ColumnIndex == dataGridView2.Columns["Increase"].Index && e.RowIndex != -1)
            {
                int cur_ep = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[6].Value);
                int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
                int episodes = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString());
                if(cur_ep < episodes)
                {
                    con.Open();
                    SqlCommand cmd_increment = new SqlCommand("UPDATE series SET current_episode=@current_episode WHERE id=@id ", con);
                    cmd_increment.Parameters.AddWithValue("@current_episode", cur_ep + 1);
                    cmd_increment.Parameters.AddWithValue("@id", id);
                    cmd_increment.ExecuteNonQuery();
                    con.Close();
                    updateTable();
                }
                else
                {
                    MessageBox.Show("You've already watched all of it!");
                }
                
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_user frm_usr = new Form_user();
            frm_usr.user_id = this.user_id;
            frm_usr.Show();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE users SET remember=0 WHERE id = @id", con);
            cmd.Parameters.AddWithValue("@id", user_id);
            cmd.ExecuteNonQuery();
            con.Close();
            Form_login frm_log = new Form_login();
            frm_log.Show();
            this.Hide();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView2.DataSource;
            bs.Filter = comboBox1.SelectedItem.ToString() + " LIKE '%" + textBox1.Text + "%'";
            dataGridView2.DataSource = bs;
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                Update();
            }
        }

        private void controlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_settings frm_s = new Form_settings();
            frm_s.user_id = user_id;
            frm_s.Show();
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_help frm = new Form_help();
            frm.Show();
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_contact frm = new Form_contact();
            frm.Show();
        }

        private void updateShowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_update frm = new Form_update(this);
            frm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            updateTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_add frm = new Form_add(this);
            frm.Show();
        }
    }
}
