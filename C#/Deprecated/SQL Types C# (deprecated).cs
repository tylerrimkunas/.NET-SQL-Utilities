using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLUtility.CSharp.Deprecated
{
    interface SQLActor
    {
        object? Act(SqlCommand cmd, out Exception? err);
    }
    abstract class SQLType : SQLActor
    {
        public object? Act(SqlCommand cmd, out Exception? err)
        {
            object? result = null;
            try
            {
                result = Foo(cmd, out err);
                err = null;
            }
            catch (Exception e) 
            {
                HandleError(e, out err);
            }
            return result;
        }
        private void HandleError(Exception e, out Exception err)
        {
            err = e;
        }
        public abstract object Foo(SqlCommand cmd, out Exception? err);
    }
    class SQLNone : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
    class SQLScalar : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
    class SQLRow : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
    class SQLMultiRow : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
    class SQLDictionary : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
    class SQLDictRows : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
    class SQLDataTable : SQLType
    {
        public override object Foo()
        {
            throw new NotImplementedException();
        }
    }
}
