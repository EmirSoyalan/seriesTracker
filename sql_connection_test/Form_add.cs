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
    public partial class Form_add : Form
    {
        private readonly Form_main frm_m;
        public Form_add(Form_main frm)
        {
            InitializeComponent();
            frm_m = frm;
        }
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=alist_db;Integrated Security=True");
        ErrorProvider error = new ErrorProvider();
        bool err;
        
        private void button1_Click(object sender, EventArgs e)
        {
            error.Clear();
            err = false;
            foreach (Control x in groupBox1.Controls)
            {
                if (x is TextBox)
                {
                    if ((String.IsNullOrWhiteSpace(x.Text.Trim()) || x.Text == ""  ) && !(x is RichTextBox) && (x.Name != "textBox2"))
                    {
                        error.SetError(x, "Please enter a correct value");
                        err = true;
                    }
                    for (int i = 0; i < x.Text.Length; i++)
                    {
                        if (x.Text[i] == ' ')
                        {
                            continue;
                        }
                        if (!char.IsLetterOrDigit(x.Text[i]))
                        {
                            error.SetError(x, "You can only type letters and numbers");
                            err = true;
                        }
                    }
                }              
            }

            con.Open();
            SqlCommand cmd_suggest = new SqlCommand("SELECT * FROM series WHERE user_id = '" + frm_m.user_id + "' ", con);
            SqlDataReader reader = cmd_suggest.ExecuteReader();
            while (reader.Read())
            {
                if (textBox1.Text == reader["name"].ToString())
                {
                    error.SetError(textBox1, "There is already a show with this title");
                    err = true;
                    break;
                }
            }
            con.Close();
            if (comboBox1.Items.Count == 0)
            {
                error.SetError(comboBox1, "You should add at least a genre");
                err = true;
            }
            if (!err)
            {
                myClass cls = new myClass();
                StringBuilder genres = new StringBuilder();
                Form_main frm = new Form_main();
                SqlCommand cmd = new SqlCommand("INSERT INTO series (id,name,genre,episodes,current_episode,rating,description,situation,user_id) VALUES (@id,@name,@genre,@episodes,@current_episode,@rating,@description,@situation,@user_id)", con);

                foreach (string name in comboBox1.Items)
                {
                    genres.Append(name);
                    genres.Append(",");
                }

                genres = genres.Remove(genres.Length -1, 1);
                cmd.Parameters.AddWithValue("@id", cls.getCountOfTable(con, "series"));
                cmd.Parameters.AddWithValue("@name", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", genres.ToString());
                cmd.Parameters.AddWithValue("@episodes", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@current_episode", numericUpDown2.Value);
                cmd.Parameters.AddWithValue("@rating", comboBox2.Items[comboBox2.SelectedIndex]);
                cmd.Parameters.AddWithValue("@description", "" + richTextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@situation", comboBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@user_id", frm_m.user_id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                frm_m.updateTable();
                MessageBox.Show("The Series have been succesfully added.", "ADDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach(Control x in groupBox1.Controls)
                {
                    if(x is TextBox)
                    {
                        x.Text = "";
                    }
                    if(x is NumericUpDown)
                    {

                    }
                }
                numericUpDown1.Value = 1;
                numericUpDown2.Value = 0;
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                comboBox1.Items.Clear();
                richTextBox1.Text = "";
            } 
        }
        private void Form1_Load(object sender, EventArgs e)
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
            ///////////////
            numericUpDown2.Maximum = 0;
            numericUpDown1.Minimum = 1;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            AutoCompleteStringCollection suggest_list = new AutoCompleteStringCollection();
            con.Open();
            SqlCommand cmd_suggest = new SqlCommand("SELECT * FROM series WHERE user_id = '" + frm_m.user_id + "' ", con);
            SqlDataReader reader = cmd_suggest.ExecuteReader();
            while (reader.Read())
            {
                string[] words = reader["genre"].ToString().Split(',');
                suggest_list.AddRange(words);
            }
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox2.AutoCompleteCustomSource = suggest_list;
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_add_Click_1(object sender, EventArgs e)
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

        private void button_remove_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Maximum = numericUpDown1.Value;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == numericUpDown2.Value)
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
