using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLUtility.CSharp
{
    [TestClass]
    public class Tests
    {
        private readonly string connection = "Add connection string here"; //TODO: Add Connection String and test queries
        private readonly string read = "SELECT * FROM Employees";
        private readonly string write = "INSERT INTO Employees (Name, Salary, StartDate) VALUES (@name, @salary, @date)";
        private readonly string update = "UPDATE Employees SET Salary=@salary WHERE Id=@id";
        private readonly string delete = "DELETE Employees WHERE Id=@id";
        private readonly string truncate = "TRUNCATE TABLE Employees";
        [TestMethod]
        public void ReadToMultiDictionary()
        { ReadHelper(new SQLMultiDictionary()); }
        [TestMethod]
        public void ReadToScalar()
        { ReadHelper(new SQLScalar()); }
        [TestMethod]
        public void ReadToRow()
        { ReadHelper(new SQLRow()); }
        [TestMethod]
        public void ReadToMultiRow()
        { ReadHelper(new SQLMultiRow()); }
        [TestMethod]
        public void ReadToScalarRows()
        { ReadHelper(new SQLScalarRows()); }
        [TestMethod]
        public void ReadToDictionary()
        { ReadHelper(new SQLDictionary()); }
        [TestMethod]
        public void ReadToDataTable()
        { ReadHelper(new SQLDataTable()); }
        private void ReadHelper(ISQLActor actor)
        {
            SQLUtility sql = new(read, connection);
            Assert.IsNotNull(sql.Execute(actor, out Exception? err));
            Assert.IsNull(err, err == null ? "Unkown Error" : err.Message);
        }
        [TestMethod]
        public void WriteTest()
        {
            SQLUtility sql = new(write, connection, [("@name", "Tyler Rimkunas"), ("@salary", 30000), ("date", DateTime.Now)]);
            sql.Execute(new SQLNone(), out Exception? err);
            Assert.IsNull(err, err == null ? "Unknown Error" : err.Message);
        }
        [TestMethod]
        public void DeleteTest()
        {
            SQLUtility sql = new(delete, connection, [("@id", 2)]);
            sql.Execute(new SQLNone(), out Exception? err);
            Assert.IsNull(err, err == null ? "Unknown Error" : err.Message);
        }
        [TestMethod]
        public void UpdateTest()
        {
            SQLUtility sql = new(update, connection, [("@salary", 60000), ("@id", 1)]);
            sql.Execute(new SQLNone(), out Exception? err);
            Assert.IsNull(err, err == null ? "Unknown Error" : err.Message);
        }
        [TestMethod]
        public void TruncateTest()
        {
            SQLUtility sql = new(truncate, connection);
            sql.Execute(new SQLNone(), out Exception? err);
            Assert.IsNull(err, err == null ? "Unknown Error" : err.Message);
        }
        [TestMethod]
        public void LambdaTest()
        {
            SQLUtility sql = new(read, connection);
            bool result = false;
            var output = sql.Execute((x) =>
            {
                result = true;
                try
                { return x.ExecuteScalar(); }
                catch
                { return null; }
            });
            Assert.IsTrue(result);
            Assert.IsNotNull(output);
        }
    }
}
