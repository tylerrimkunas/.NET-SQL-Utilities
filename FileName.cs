using System.Data.SqlClient;

namespace SQLUtility
{
    internal class FileName
    {
        public void main()
        {
            var query = "SELECT * FROM Users";
            var con = new SqlConnection(query);
            using ( SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
