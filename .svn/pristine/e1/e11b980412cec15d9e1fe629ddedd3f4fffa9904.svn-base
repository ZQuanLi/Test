using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBUtility
{
    public class MySqlConnectionAdapter : ISqlConnectionAdapter
    {
        public IDbConnection GetDbConnection(string connString)
        {
            return new MySqlConnection(connString);
        }
    }
}
