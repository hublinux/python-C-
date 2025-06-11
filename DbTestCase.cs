//数据库测试扩展 DbTestCase.cs
using System.Data.SqlClient;

namespace AutomatedTestTool
{
    public abstract class DbTestCase : TestCase
    {
        protected SqlConnection Connection { get; private set; }
        protected string ConnectionString { get; set; } = "YourConnectionString";

        public override void Setup()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        public override void Teardown()
        {
            Connection?.Close();
        }

        protected int ExecuteScalar(string query)
        {
            using (var cmd = new SqlCommand(query, Connection))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
    }

    public class SampleDbTest : DbTestCase
    {
        public override void Test()
        {
            var count = ExecuteScalar("SELECT COUNT(*) FROM Users");
            Assert.IsTrue(count > 0, "No users found in database");
        }
    }
}
