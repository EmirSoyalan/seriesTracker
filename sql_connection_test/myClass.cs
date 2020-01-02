using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace sql_connection_test
{
    class myClass
    {
        public int getCountOfTable(SqlConnection con, string table )
        {
            con.Open();
            int count = 0;
            SqlCommand cmd_count = new SqlCommand();
            cmd_count.CommandText = "SELECT * FROM " + table;
            cmd_count.Connection = con;
            //cmd_count.CommandType = CommandType.Text;
            SqlDataReader reader = cmd_count.ExecuteReader();
            while (reader.Read())
            {
                count++;
            }
            con.Close();
            return count;
        }
    }
}
