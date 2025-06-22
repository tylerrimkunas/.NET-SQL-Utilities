using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    internal class SQLDictionary : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            Dictionary<string, object> result = [];
            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                for (int i = 0; i < reader.FieldCount; i++)
                { result.Add(reader.GetName(i), reader.GetValue(i)); }
            }
            return result;
        }
    }
}
