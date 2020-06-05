using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text; 
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using YDS6000.Models;

namespace DBUtility
{
    public enum DbTypeDefines
    {
        Mysql,
        SqlServer,
    }

    public class SQLHelper
    {
        protected static string _connectionString { get; private set; }
        private static readonly Dictionary<DbTypeDefines, ISqlConnectionAdapter> AdapterDictionary = new Dictionary<DbTypeDefines, ISqlConnectionAdapter>
        {
            [DbTypeDefines.Mysql] = new MySqlConnectionAdapter(),
        };

        /// <summary>
        /// 数据库连接
        /// </summary>
        public static IDbConnection GetDBConnection(DbTypeDefines dbType = DbTypeDefines.Mysql)
        {
            var connection = AdapterDictionary[dbType];
            if (connection == null)
            {
                throw new Exception(string.Format("dbType:{0} is not defines", dbType.ToString()));
            }          
            return connection.GetDbConnection(_connectionString);
        }

        #region 增加测试数据库连接是否成功
        /// <summary>
        /// 数据库连接
        /// </summary>
        private static IDbConnection GetDBConnection(string connectionStr, DbTypeDefines dbType = DbTypeDefines.Mysql)
        {
            var connection = AdapterDictionary[dbType];
            if (connection == null)
            {
                throw new Exception(string.Format("dbType:{0} is not defines", dbType.ToString()));
            }
            return connection.GetDbConnection(connectionStr);
        }

        public static void ConneectingServices(string connectionString)
        {
            using (var dbConnection = GetDBConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Close();
            }
        }
        #endregion 

        public SQLHelper(string connectionString)
        {
            _connectionString = connectionString;
        }




        /// <summary>
        /// 是否已存在
        /// </summary>
        public bool Contains<T>(T model) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Contains<T>(model);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        public static long Insert<T>(T model) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                try
                {
                    return dbConnection.Insert<T>(model);
                }
                catch (Exception ex)
                {

                    return 0;
                }

            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public bool Update<T>(T model) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Update<T>(model);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete<T>(int id) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.DeleteByKey<T>(id);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete<T>(object entity) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Delete<T>(entity);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool Delete<T>(T model) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Delete<T>(model);
            }
        }


        /// <summary>
        /// 删除所有
        /// </summary>
        public bool DeleteAll<T>() where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.DeleteAll<T>();
            }
        }

        /// <summary>
        /// 根据主键获取
        /// </summary>
        public T Get<T>(object id) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Get<T>(id);
            }
        }

        public static T GetSigle<T>(object parameters = null) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.GetSigle<T>(parameters);
            }
        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        public int GetRowCount<T>(object parameters = null) where T : class
        {
            using (var dbConnection = GetDBConnection())
            {
                int count = dbConnection.GetRowCount<T>(parameters);
                return count;
            }
        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        public List<T> GetPage<T>(int pageSize, int pageIndex, bool asc = true) where T : class
        {
            return GetPage<T>(pageSize, pageIndex, null, true);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        public List<T> GetPage<T>(int pageSize, int pageIndex, object parameters = null, bool asc = true) where T : class
        {
            if (pageSize > 0)
            {
                using (var dbConnection = GetDBConnection())
                {
                    var enm = dbConnection.GetPage<T>(pageIndex, pageSize, parameters, asc);
                    if (enm == null)
                    {
                        return null;
                    }
                    return enm.ToList();
                }
            }
            else
            {
                return GetAll<T>(asc);
            }

        }



        /// <summary>
        /// 获取所有
        /// </summary>
        public List<T> GetAll<T>(bool asc = true) where T : class
        {
            List<T> list = null;
            using (var dbConnection = GetDBConnection())
            {
                list = dbConnection.GetAll<T>(asc).ToList();
            }
            return list;
        }


        /// <summary>
        /// 获取指定数据
        /// </summary>
        public List<T> GetList<T>(object parameters = null, bool asc = true) where T : class
        {
            List<T> list = null;
            using (var dbConnection = GetDBConnection())
            {
                list = dbConnection.GetList<T>(parameters, asc).ToList();

            }
            return list;
        }

        /// <summary>
        /// 执行指令
        /// </summary>
        public static int Execute(string sql, object parameters = null)
        {
            StringBuilder strCommand = new StringBuilder();
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Execute(sql, parameters);
            }
        }


        /// <summary>
        /// 执行指令
        /// </summary>
        public static object ExecuteScalar(string sql, object parameters = null)
        {
            StringBuilder strCommand = new StringBuilder();
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.ExecuteScalar(sql, parameters);
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        public static List<T> Query<T>(string sql, object parameters = null)
        {
            StringBuilder strCommand = new StringBuilder();
            using (var dbConnection = GetDBConnection())
            {
                return dbConnection.Query<T>(sql, parameters).ToList();
            }
        }

        /// <summary>
        /// 执行查询返回table
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable Query(string sql, object parameters = null)
        {
            DataTable dtRst = new DataTable();
            using (var dbConnection = GetDBConnection())
            {
                IDataReader reader = dbConnection.ExecuteReader(sql, parameters);
                dtRst.Load(reader);
                dbConnection.Close();
            }
            return dtRst;
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool TransactionExecute(Dictionary<string, System.Dynamic.ExpandoObject> keyValue, bool ignoreFirst = false)
        {
            StringBuilder strCommand = new StringBuilder();
            bool isSuccess = true;
            using (var dbConnection = GetDBConnection())
            {
                dbConnection.Open();
                //开户事务
                var trans = dbConnection.BeginTransaction();
                int i = 0;
                foreach (var item in keyValue)
                {

                    var ddd = item.Value;
                    var d2 = item.Key.Substring(4, item.Key.Length - 5);
                    var rows = dbConnection.Execute(item.Key.Substring(4, item.Key.Length - 5), item.Value);
                    //是否忽略第一条的执行结果，如果忽略，则不回滚
                    if (i == 0 && ignoreFirst)
                    {
                        rows = 1;
                    }
                    if (rows == 0)
                    {
                        isSuccess = false;
                        trans.Rollback();
                        break;
                    }
                    i++;
                }
                if (isSuccess)
                {
                    trans.Commit();
                }
                dbConnection.Close();
            }
            return isSuccess;
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetCount(string sql, object parameters)
        {
            sql = sql.ToLower();
            if (!sql.Contains(" from "))
            {
                return 0;
            }
            var fromIndex = sql.IndexOf(" from ");
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append(" select count(*) ");
            sbsql.Append(sql.Substring(fromIndex)); 
            return CommFunc.ConvertDBNullToInt32(ExecuteScalar(sbsql.ToString(), parameters));
        }

        /// <summary>
        /// Execute StoredProcedure and map result to POCO
        /// </summary>
        /// <param name="sqlConnnectionString"></param>
        public List<T> ExecuteStoredProcedure<T>(string SQLProcedureName, object parameters)
        {
            using (var dbConnection = GetDBConnection())
            {
                var list = dbConnection.Query<T>(SQLProcedureName,
                                       parameters,
                                        null,
                                        true,
                                        null,
                                        CommandType.StoredProcedure).ToList();
                return list;
            }
        }

        /// <summary>
        /// Execute StoredProcedure and map result to POCO
        /// </summary>
        /// <param name="sqlConnnectionString"></param>
        public static DataTable ExecuteStoredProcedure(string SQLProcedureName, object parameters,int? timeOut =null)
        {
            //
            DataTable dtRst = new DataTable();
            using (var dbConnection = GetDBConnection())
            {
                IDataReader reader = dbConnection.ExecuteReader(SQLProcedureName, parameters, null, timeOut, CommandType.StoredProcedure);
                dtRst.Load(reader);
            }
            return dtRst;
        }

        /// <summary>
        /// 执行存储过程, 带信息字符串输出(返回参数名称,在存储过程中要设置为_mMsg)
        /// </summary>
        /// <param name="SQLProcedureName">存储过程名称</param>
        /// <param name="parameters">参数, 不包括输出参数_mMsg</param>
        public void ExecuteStoredProcedureWithOutMsg(string SQLProcedureName, object parameters)
        {
            DynamicParameters p = new DynamicParameters();
            Type type = parameters.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                p.Add("@" + i.Name, i.GetValue(parameters));
            }
            //p.Add("@_mMsg", null, DbType.String, ParameterDirection.Output);
            using (var dbConnection = GetDBConnection())
            {
                dbConnection.Execute(SQLProcedureName, p, null, null, CommandType.StoredProcedure);
                //string result = p.Get<string>("@_mMsg"); 
            }
        }


    }
}
