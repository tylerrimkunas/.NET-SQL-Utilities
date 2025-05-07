using Microsoft.Data.SqlClient;
using CSharp.Types;

namespace CSharp.Utility
{
    public class SQLUtility
    {
        private string? query;
        private string constr;
        private (string, object)[]? parameters;
        public SQLUtility(string? q = null, string c = "DEFAULT", (string, Object)[]? param_arr = null)
        {
            query = q;
            constr = c;
            parameters = param_arr == null ? null : FormatParams(param_arr);
        }
        private static (string, object)[]? FormatParams((string, object)[] arr)
        {
            return [.. arr.Select(x => x = x.Item1[0] != '@' ? ($"@{x.Item1}", x.Item2) : (x.Item1, x.Item2))];
        }
        public object? Execute(SQLActor x, out Exception? err)
        {
            object? result = null;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.AddWithValue(item.Item1, item.Item2);
                        }
                    }
                    con.Open();
                    result = x.Act(cmd, out err);
                    con.Close();
                }
            }
            return result;
        }
    }
}
