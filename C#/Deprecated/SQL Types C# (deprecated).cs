using System.Data.SqlClient;

namespace CSharp.Types.Deprecated
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
            object? result;
            rdr.Read();
            result = ReadRow(rdr);
            return result;
        }
        protected abstract object? ReadRow(SqlDataReader rdr);
    }
    abstract class SQLReaderMulti : SQLReaderSingle
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
    class SQLRow : SQLReaderSingle
    {
        protected override object? Foo(SqlCommand cmd)
        {
            List<object> list = [];
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                rdr.Read();
                foreach (object cell in rdr) { list.Add(cell);}
            }
            return list;
        }

        protected override object? ReadRow(SqlDataReader rdr)
        {
            throw new NotImplementedException();
        }
    }
    class SQLMultiRow : SQLReaderMulti
    {
        protected override object? ReadRow(SqlDataReader rdr)
        {
            List<object> row = new List<object>();
            foreach (object cell in rdr) { row.Add(cell); }
            return row;
        }
    }
    class SQLDictionary : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            Dictionary<string, object> dict = [];
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while(rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++) 
                    { dict[rdr.GetName(i)] = rdr.GetValue(i);}
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
