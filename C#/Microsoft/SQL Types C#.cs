using Microsoft.Data.SqlClient;

namespace CSharp.Types
{
    public interface SQLActor
    {
        public object? Act(SqlCommand cmd, out Exception? err);
    }
    abstract class SQLType : SQLActor
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
    abstract class SQLReader : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            object? result;
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                result = ReaderFoo(rdr);
            }
            return result;
        }
        protected abstract object? ReaderFoo(SqlDataReader rdr);
    }
    abstract class SQLReaderSingle : SQLReader
    {
        protected override object? ReaderFoo(SqlDataReader rdr)
        {
            List<object?> result = [];
            rdr.Read();
            result.Add(ReadValue(rdr));
            return result;
        }
        protected abstract object? ReadValue(SqlDataReader rdr);
    }
    abstract class SQLReaderMulti : SQLReader
    {
        protected override object? ReaderFoo(SqlDataReader rdr)
        {
            List<object?> result = [];
            while (rdr.Read())
            {
                result.Add(ReadRow(rdr));
            }
            return result;
        }
        protected abstract object? ReadRow(SqlDataReader rdr);
    }
    class SQLNone : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            cmd.ExecuteNonQuery();
            return null;
        }
    }
    class SQLScalar : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            return cmd.ExecuteScalar();
        }
    }
    class SQLRow : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            List<object> list = [];
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                rdr.Read();
                foreach (object cell in rdr) { list.Add(cell); }
            }
            return list;
        }
    }
    class SQLMultiRow : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            List<List<object>> list = [];
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    List<object> row = new List<object>();
                    foreach (object cell in rdr) { row.Add(cell); }
                    list.Add(row);
                }
            }
            return list;
        }
    }
    class SQLDictionary : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            Dictionary<string, object> dict = [];
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    { dict[rdr.GetName(i)] = rdr.GetValue(i); }
                }
            }
            return dict;

        }
    }
    class SQLDictRows : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            throw new NotImplementedException();
        }
    }
    class SQLDataTable : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            throw new NotImplementedException();
        }
    }
}
