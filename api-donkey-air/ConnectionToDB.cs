
using MySql.Data.MySqlClient;

namespace ConnectionToDB
{
    public class Connection
    {

        public static void Main()
        {
            string dbdatas = "server=localhost;uid=root;pwd=manasse22;database=donkeyair";
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = dbdatas;
            con.Open(); // pour ouvir la connection
            string sql = "SELECT * FROM departure";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Console.WriteLine(reader["country"]);

        }

        public static void GetDbParams()
        {

            Console.WriteLine("hello");
        }



    }

}

