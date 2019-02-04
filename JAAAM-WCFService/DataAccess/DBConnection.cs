using System.Configuration;
using System.Data.SqlClient;

namespace DataAccess {
    class DBConnection {
        public static string ConnectionString {
            get {
                string connStr = ConfigurationManager.ConnectionStrings["JaaamDbString"].ToString();

                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connStr);
                sb.ApplicationName = ApplicationName ?? sb.ApplicationName;
                sb.ConnectTimeout = (ConnectionTimeout > 0) ? ConnectionTimeout : sb.ConnectTimeout;
                return sb.ToString();
            }
        }

        public static SqlConnection GetSqlConnection() {
            SqlConnection conn = new SqlConnection(ConnectionString);
            return conn;
        }

        public static int ConnectionTimeout { get; set; }

        public static string ApplicationName {
            get;
            set;
        }
    }
}
