using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility
{
    public class SqlServerConnectionAdapter : ISqlConnectionAdapter
    {
        public IDbConnection GetDbConnection(string connString)
        {
            return new SqlConnection(connString);
        }
    }
}
