using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    public class SQLNone : SQLType
    {
        protected override object? Foo(SqlCommand cmd) => cmd.ExecuteNonQuery();
    }
}
