using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    internal class SQLMultiDictionary : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            List<Dictionary<string, object>> result = [];
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Dictionary<string, object> row = [];
                    for (int i = 0; i < reader.FieldCount; i++)
                    { row.Add(reader.GetName(i), reader.GetValue(i)); }
                    result.Add(row);
                }
            }
            return result;
        }
    }
}
