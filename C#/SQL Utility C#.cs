using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    public class SQLUtility
    {
        private (string, object)[] _parameters;
        public string Query { get; set; }
        public string Connection { get; set; }
        public (string name, object pValue)[] Parameters
        {
            get => _parameters;
            set => _parameters = value != null ? [.. value.Select((x) => (x.name[0] != '@' ? $"@{x.name}" : x.name, x.pValue))] : [];
        }
        public SQLUtility(string query, string connection, (string name, object value)[]? parameters = null)
        {
            this.Query = query;
            this.Connection = connection;
            this.Parameters = parameters;
        }
        public object? Execute(ISQLActor x, out Exception? err)
        {
            object? result = null;
            using (SqlConnection con = new(Connection))
            {
                using SqlCommand cmd = new(Query, con);
                foreach (var (name, pValue) in Parameters)
                { cmd.Parameters.AddWithValue(name, pValue); }
                con.Open();
                result = x.Act(cmd, out err);
                con.Close();
            }
            return result;
        }
        public object? Execute(Func<SqlCommand, object?> func)
        {
            object? result = null;
            using (SqlConnection con = new(Connection))
            {
                using SqlCommand cmd = new(Query, con);
                foreach (var (name, pValue) in Parameters)
                { cmd.Parameters.AddWithValue(name, pValue); }
                con.Open();
                result = func(cmd);
                con.Close();
            }
            return result;
        }
    }
}
