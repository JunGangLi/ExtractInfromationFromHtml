using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace connectPostgreSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            string connection = "Server=localhost;Port=5432;User Id=postgres;Password=1;Database=postgres;";
            NpgsqlConnection conn = new NpgsqlConnection(connection);
            conn.Open();
            if (conn.State==System.Data.ConnectionState.Open)
            {
                Console.WriteLine("open!!");
            }
            Console.ReadKey();
            //using (NpgsqlDataAdapter cmd = new NpgsqlDataAdapter("select id,name from test_table;", conn))
            //{
            //    DataSet ds = new DataSet();
            //    cmd.Fill(ds);
            //}

            string cmdtxt = "insert into test_table(id,name) values (11,\'李磊\'),(12,\'韩梅梅\');";
            using (NpgsqlCommand cmd=new NpgsqlCommand(cmdtxt,conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
