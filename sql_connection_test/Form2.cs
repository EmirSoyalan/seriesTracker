using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
 * 
 * TODO:
 * new form to update the value on double cell click
 * 
 * 
 * 
 * 
 */
namespace sql_connection_test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'alist_dbDataSet.series' table. You can move, or remove it, as needed.
            this.seriesTableAdapter.Fill(this.alist_dbDataSet.series);
            DataGridViewButtonColumn updateButtonColumn = new DataGridViewButtonColumn();
            DataGridViewButtonColumn watchedButtonColumn = new DataGridViewButtonColumn();
            updateButtonColumn.Width = 60;
            watchedButtonColumn.Width = 20;
            updateButtonColumn.Name = "Adjust";
            updateButtonColumn.Text = "Update";
            watchedButtonColumn.Name = "Watched";
            watchedButtonColumn.Text = "+";
            updateButtonColumn.UseColumnTextForButtonValue = true;
            watchedButtonColumn.UseColumnTextForButtonValue = true;
            if (dataGridView1.Columns["Adjust"] == null && dataGridView1.Columns["Watched"] == null)
            {
                dataGridView1.Columns.Insert(4, watchedButtonColumn);
                dataGridView1.Columns.Insert(7, updateButtonColumn);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Adjust"].Index && e.RowIndex != -1)
            {
                // updateForm.show
            }
            if (e.ColumnIndex == dataGridView1.Columns["Watched"].Index && e.RowIndex != -1)
            {
                // current_episode++
            }
        }
    }
}
