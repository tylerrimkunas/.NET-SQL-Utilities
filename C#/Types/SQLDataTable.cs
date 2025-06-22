using Microsoft.Data.SqlClient;
using System.Data;

namespace SQLUtility.CSharp
{
    public class SQLDataTable : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            DataTable dt = new();
            using (SqlDataAdapter sda = new(cmd)) { sda.Fill(dt); }
            return dt;
        }
    }
}
