using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    internal class SQLMultiRow : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            List<object[]> result = [];
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetSqlValues(row);
                    result.Add(row);
                }
            }
            return result;
        }
    }
}
