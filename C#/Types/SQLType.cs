using Microsoft.Data.SqlClient;
namespace SQLUtility.CSharp
{
    public abstract class SQLType : ISQLActor
    {
        public object? Act(SqlCommand cmd, out Exception? err)
        {
            object? result = null;
            try
            {
                result = Foo(cmd);
                err = null;
            }
            catch (Exception e)
            {
                HandleError(e, out err);
            }
            return result;
        }
        protected virtual void HandleError(Exception e, out Exception err)
        {
            err = e;
        }
        protected abstract object? Foo(SqlCommand cmd);
    }

}