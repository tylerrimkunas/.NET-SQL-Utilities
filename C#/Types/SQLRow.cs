using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    internal class SQLRow : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            object[] result;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                result = new object[reader.FieldCount];
                reader.GetSqlValues(result);
            }
            return result;
        }
    }
}
