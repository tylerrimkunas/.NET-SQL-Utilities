using Microsoft.Data.SqlClient;
using System.Data;

namespace CSharp.Types
{
    public interface ISQLActor
    {
        public object? Act(SqlCommand cmd, out Exception? err);
    }
    abstract class SQLType : ISQLActor
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
        protected ICommon? common;
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
        protected virtual object? ReadRow(SqlDataReader rdr) => common?.Run(rdr);
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
        public SQLRow() => common = new CommonRow();
    }
    class SQLMultiRow : SQLReaderMulti
    {
        public SQLMultiRow() => common = new CommonRow();
    }
    class SQLDictionary : SQLReaderSingle
    {
        public SQLDictionary() => common = new CommonDictionary();
    }
    class SQLMultiDictionary : SQLReaderMulti
    {
        public SQLMultiDictionary() => common = new CommonDictionary();
    }
    class SQLDataTable : SQLType
    {
        protected override object? Foo(SqlCommand cmd)
        {
            SqlDataAdapter sda = new(cmd);
            DataTable dt = new();
            sda.Fill(dt);
            return dt;
        }
    }
    interface ICommon
    {
        object? Run(SqlDataReader rdr);
    }
    class CommonDictionary : ICommon
    {
        public object? Run(SqlDataReader rdr)
        {
            Dictionary<string, object> dict = [];
            for (int i = 0; i <= rdr.FieldCount; i++)
            {
                dict[rdr.GetName(i)] = (object)rdr.GetValue(i);
            }
            return dict;
        }
    }
    class CommonRow : ICommon
    {
        public object? Run(SqlDataReader rdr)
        {
            List<object> row = [.. rdr];
            return row;
        }
    }
}
