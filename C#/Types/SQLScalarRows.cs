using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    internal class SQLScalarRows : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            List<object> result = [];
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    result.Add(reader[0]);
            return result;
        }
    }
}
