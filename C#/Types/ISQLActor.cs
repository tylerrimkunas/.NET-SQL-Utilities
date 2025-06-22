using Microsoft.Data.SqlClient;

namespace SQLUtility.CSharp
{
    public interface ISQLActor
    {
        public object? Act(SqlCommand cmd, out Exception? err);
    }
}
