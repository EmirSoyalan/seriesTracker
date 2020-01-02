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
    public partial class Form_update : Form
    {
        private readonly Form_main frm_m;
        public Form_update(Form_main frm)
        {
            InitializeComponent();
            frm_m = frm;
        }
        // to prevent override on PRIMARY KEY when an ID gets removed. Simply keeps ID's in correct order (0,1,2,3...)
        void reArrangeDatabaseID(int removedID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE series SET id = id - 1 WHERE id > '" + removedID + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public int id;
        public string name;
        public string genre;
        public string episodes;
        public string current_episode;
        public string rating;
        public string description;
        public string situation;
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        StringBuilder sb = new StringBuilder();
        ErrorProvider error = new ErrorProvider();
        bool err;
        private void Form_Update_Load(object sender, EventArgs e)
        {
            // Color adjustment
            con.Open();
            SqlCommand cmd_set = new SqlCommand("SELECT * FROM settings WHERE user_id = '" + frm_m.user_id + "'", con);
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
            numericUpDown2.Maximum = 0;
            numericUpDown1.Minimum = 1;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            if (!String.IsNullOrEmpty(name))
            {
                comboBox4.Items.Add(name);
                comboBox4.SelectedIndex = 0;
                comboBox4.Enabled = false;
                numericUpDown1.Value = int.Parse(episodes);
                numericUpDown2.Value = int.Parse(current_episode);
                comboBox2.SelectedIndex = int.Parse(rating) - 1;
                comboBox3.SelectedItem = situation.Trim();
                richTextBox1.Text = description;
                if(comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }

            // Autocomplete settings section
            AutoCompleteStringCollection suggest_list = new AutoCompleteStringCollection();
            con.Open();
            SqlCommand cmd_suggest = new SqlCommand("SELECT * FROM series WHERE user_id = '" + frm_m.user_id + "' ", con);
            SqlDataReader reader = cmd_suggest.ExecuteReader();
            while (reader.Read())
            {
                suggest_list.Add(reader["name"].ToString());
                comboBox4.Items.Add(reader["name"].ToString());
            }
            comboBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox4.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBox4.AutoCompleteCustomSource = suggest_list;
            con.Close();
            AutoCompleteStringCollection suggest_list2 = new AutoCompleteStringCollection();
            con.Open();
            SqlCommand cmd_suggest2 = new SqlCommand("SELECT * FROM series WHERE user_id = '" + frm_m.user_id + "' ", con);
            SqlDataReader reader2 = cmd_suggest2.ExecuteReader();
            while (reader2.Read())
            {
                string[] words = reader2["genre"].ToString().Split(',');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].ToString().Trim();
                }
                suggest_list2.AddRange(words);
            }
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox2.AutoCompleteCustomSource = suggest_list2;
            con.Close();
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            // error handling
            error.Clear();
            err = false;
            foreach (Control x in groupBox1.Controls)
            {
                if (x is TextBox || x is ComboBox)
                {
                    if ((String.IsNullOrWhiteSpace(x.Text.Trim()) || x.Text == "") && !(x is RichTextBox) && (x.Name != "textBox2") && (x.Name != "textBox_rename"))
                    {
                        error.SetError(x, "Please enter a correct value");
                        err = true;
                    }
                    for (int i = 0; i < x.Text.Length; i++)
                    {
                        if(x.Text[i] == ' ')
                        {
                            continue;
                        }
                        if (!char.IsLetterOrDigit(x.Text[i]) )
                        {
                            error.SetError(x, "You can only type letters and numbers");
                            err = true;
                        }
                    }
                }
            }
            if (comboBox1.Items.Count == 0)
            {
                error.SetError(comboBox1, "You should add at least a genre");
                err = true;
            }
            if (!String.IsNullOrEmpty(textBox_rename.Text.Trim()))
            {
                con.Open();
                SqlCommand cmd_checkID = new SqlCommand("SELECT name FROM series WHERE NOT id = @id AND user_id = @user_id",con);
                cmd_checkID.Parameters.AddWithValue("@id", id);
                cmd_checkID.Parameters.AddWithValue("@user_id", frm_m.user_id);
                SqlDataReader R = cmd_checkID.ExecuteReader();
                while (R.Read())
                {
                    if (textBox_rename.Text == R["name"].ToString())
                    {
                        error.SetError(textBox_rename, "There is another show with that name");
                        err = true;
                    }
                }
                con.Close();
            }
            if (!err)
            {
                bool valid = false;
                con.Open();
                SqlCommand cmd_check = new SqlCommand("SELECT * FROM series WHERE user_id = @user_id", con);
                cmd_check.Parameters.AddWithValue("@user_id", frm_m.user_id);
                SqlDataReader reader = cmd_check.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["name"].ToString() == comboBox4.Text)
                    {
                        valid = true;
                    }
                }
                con.Close();
                if (valid)
                {
                    foreach (string name in comboBox1.Items)
                    {
                        sb.Append(name);
                        sb.Append(", ");
                    }
                    sb = sb.Remove(sb.Length - 2, 2);
                    con.Open();
                    SqlCommand cmd_update = new SqlCommand("UPDATE series SET name=@name, genre=@genre, episodes=@episodes, current_episode=@current_episode, rating=@rating, description=@description, situation=@situation WHERE id=@id", con);
                    if (!String.IsNullOrEmpty(textBox_rename.Text.Trim()))
                    {
                        cmd_update.Parameters.AddWithValue("@name", textBox_rename.Text);
                    }
                    else
                    {
                        cmd_update.Parameters.AddWithValue("@name", comboBox4.Text);
                    }
                    cmd_update.Parameters.AddWithValue("@genre", sb.ToString());
                    cmd_update.Parameters.AddWithValue("@episodes", numericUpDown1.Value);
                    cmd_update.Parameters.AddWithValue("@current_episode", numericUpDown2.Value);
                    cmd_update.Parameters.AddWithValue("@rating", comboBox2.Text);
                    cmd_update.Parameters.AddWithValue("@description", richTextBox1.Text);
                    cmd_update.Parameters.AddWithValue("@situation", comboBox3.Text);
                    cmd_update.Parameters.AddWithValue("@id", id);
                    cmd_update.ExecuteNonQuery();
                    frm_m.updateTable();
                    MessageBox.Show("The Series has been updated");
                    con.Close();
                    if (!String.IsNullOrEmpty(name))
                    {
                        Close();
                    }            
                }
                else
                {
                    MessageBox.Show("Series not found");
                }
                comboBox1.Items.Clear();
                numericUpDown1.Value = 1;
                numericUpDown2.Value = 0;
                numericUpDown2.Maximum = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                richTextBox1.Text = "";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool valid = false;
            con.Open();
            SqlCommand cmd_check = new SqlCommand("SELECT * FROM series WHERE user_id = @user_id", con);
            cmd_check.Parameters.AddWithValue("@user_id", frm_m.user_id);
            SqlDataReader reader = cmd_check.ExecuteReader();
            while (reader.Read())
            {
                if (reader["name"].ToString() == comboBox4.Text)
                {
                    valid = true;
                    id = Convert.ToInt32(reader["id"]);
                }
            }
            con.Close();
            if (valid)
            {
                if (MessageBox.Show("Are you sure you want to delete the series", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand cmd_update = new SqlCommand("DELETE FROM series WHERE id=@id", con);
                    cmd_update.Parameters.AddWithValue("@id", id);
                    cmd_update.ExecuteNonQuery();
                    con.Close();
                    frm_m.updateTable();
                    MessageBox.Show("The Series has been deleted");
                    reArrangeDatabaseID(id);
                } 
            }
            else
            {
                MessageBox.Show("Series not found");
            }
            
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            error.Clear();
            err = false;
            if ((String.IsNullOrWhiteSpace(textBox2.Text.Trim())))
            {
                error.SetError(textBox2, "Please enter a correct value");
                err = true;
            }
            for (int i = 0; i < textBox2.Text.Length; i++)
            {
                if(textBox2.Text[i] == ' ')
                {
                    continue;
                }
                if (!char.IsLetterOrDigit(textBox2.Text[i]))
                {
                    error.SetError(textBox2, "You can only type letters and numbers");
                    err = true;
                }
            }
            if (!err)
            {
                comboBox1.Items.Add(textBox2.Text.Trim());
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                textBox2.Text = "";
            }      
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            con.Open();
            SqlCommand cmd_suggest = new SqlCommand("SELECT * FROM series WHERE user_id = '" + frm_m.user_id + "' ", con);
            SqlDataReader reader = cmd_suggest.ExecuteReader();
            while (reader.Read())
            {
                if(comboBox4.SelectedItem.ToString() == reader["name"].ToString())
                {
                    string[] words = reader["genre"].ToString().Split(',');
                    foreach (string word in words)
                    {
                        if (word.Trim() != "")
                        {
                            comboBox1.Items.Add(word.Trim());
                        }
                    }
                    numericUpDown1.Value = int.Parse(reader["episodes"].ToString());
                    numericUpDown2.Value = int.Parse(reader["current_episode"].ToString());
                    comboBox2.SelectedIndex = int.Parse(reader["rating"].ToString()) - 1;
                    comboBox3.SelectedItem = reader["situation"].ToString().Trim();
                    richTextBox1.Text = reader["description"].ToString();
                    id = Convert.ToInt32(reader["id"]);
                }
            }
            con.Close();
            if(comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Maximum = numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown1.Value == numericUpDown2.Value)
            {
                comboBox3.SelectedIndex = 1;
            }
            else if (numericUpDown1.Value > numericUpDown2.Value)
            {
                if (numericUpDown2.Value != 0)
                {
                    comboBox3.SelectedIndex = 0;
                }
                else
                {
                    comboBox3.SelectedIndex = 2;
                }
            }
            else
            {
                MessageBox.Show("error.");
            }
        }
    }
}
