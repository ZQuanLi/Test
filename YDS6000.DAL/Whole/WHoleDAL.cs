using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL
{
    public class WHoleDAL
    {  
        internal static string EncryptPwd = "DiLidili";

        public static DataTable GetCoreQueryData(int ledger, string idlist, DateTime start, DateTime end, string dataType, string funType)
        {
            object parmas = new { _mLedger = ledger, _mQuery = idlist, _mDtStart = start, _mDtEnd = end, _mDateType = dataType, _mFunType = funType };
            DataTable dtSource = SQLHelper.ExecuteStoredProcedure("sp_YdCoreQuery", parmas);
            return dtSource;
        }


        internal static string splitException = "YDH20D";
        internal static string MdItems = "";

        public static void AddLog(int ledger, int uid, string prog_id, string userHostAddress, string controllerName, string actionName, string content)
        {
            object para = new
            {
                Ledger = ledger,
                Uid = uid,
                Prog_id = prog_id,
                Control = controllerName,
                Action = actionName,
                Addr = userHostAddress,
                Content = content,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sys_log(");
            strSql.Append("Ledger,Cdate,Uid,Prog_id,Control,Action,Content,Addr,CTime)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,now(),@Uid,@Prog_id,@Control,@Action,@Content,@Addr,now())");
            SQLHelper.Execute(strSql.ToString(), para);
        }


        //internal static string MdItems = "";
        ///// <summary>
        ///// 解密
        ///// </summary>
        ///// <param name="Key"></param>
        ///// <returns></returns>
        //public static string Decrypt(string Key)
        //{
        //    return ConfigHelper.Decrypt(Key);
        //}

        //public static bool Db_Exis(string HOST, string DBNAME, string DBUID, string DBPWD)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("DBNAME",DBNAME),
        //    };
        //    string connString = DBUtility.ConfigHelper.GetConnectionStrings("Connection");
        //    string dbString = string.Format(connString, HOST, "information_schema", DBUID, DBPWD);
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Clear();
        //    strSql.Append("select COUNT(*)AS CNT from information_schema.schemata where schema_name=@DBNAME;");
        //    object obj = SQLUtilities.ExecuteScalar(dbString, strSql.ToString(), params01);
        //    return (obj != null && int.Parse(obj.ToString()) > 0) ? true : false;
        //}

        //public static void DbCreate(string HOST, string DBNAME, string DBUID, string DBPWD)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("DBNAME",DBNAME),
        //    };
        //    string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\dbScript";
        //    string connString = DBUtility.ConfigHelper.GetConnectionStrings("Connection");
        //    string emptyString = string.Format(connString, HOST, "", DBUID, DBPWD);
        //    string dbString = string.Format(connString, HOST, DBNAME, DBUID, DBPWD);
        //    string fileName1 = path + @"\YDS9500.sql";
        //    string fileName2 = path + @"\YDS9500_2.sql";

        //    if (!System.IO.File.Exists(fileName1))
        //    {
        //        throw new Exception("路径:" + path + " 缺少脚本文件:YDS9500.sql");
        //    }
        //    if (!System.IO.File.Exists(fileName2))
        //    {
        //        throw new Exception("路径:" + path + " 缺少脚本文件:YDS9500_2.sql");
        //    }
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Clear();//创建数据
        //    strSql.Append("create database if not exists `" + DBNAME + "` DEFAULT charset utf8 COLLATE utf8_general_ci");
        //    SQLUtilities.ExecuteNonScript(emptyString, strSql.ToString());
        //    //
        //    //ExecuteSqlFile(dbString, fileName);
        //    strSql.Clear();
        //    System.IO.FileInfo file = new System.IO.FileInfo(fileName1);  //filename是sql脚本文件路径。
        //    strSql.Append("USE " + DBNAME + ";" + Environment.NewLine);
        //    strSql.Append(file.OpenText().ReadToEnd());
        //    SQLUtilities.ExecuteNonScript(dbString, strSql.ToString());
        //    strSql.Clear();
        //    System.IO.FileInfo file2 = new System.IO.FileInfo(fileName2);  //filename是sql脚本文件路径。
        //    strSql.Append("USE " + DBNAME + ";" + Environment.NewLine);
        //    strSql.Append(file2.OpenText().ReadToEnd());
        //    SQLUtilities.ExecuteNonScript(dbString, strSql.ToString());
        //}

        //private static bool ExecuteSqlFile(string dbString, string varFileName)
        //{
        //    using (System.IO.StreamReader reader = new System.IO.StreamReader(varFileName, System.Text.Encoding.GetEncoding("utf-8")))
        //    {
        //        string line = "";
        //        string l;
        //        try
        //        {

        //            while (true)
        //            {
        //                // 如果line被使用，则设为空  
        //                if (line.EndsWith(";"))
        //                    line = "";

        //                l = reader.ReadLine();

        //                // 如果到了最后一行，则退出循环  
        //                if (l == null) break;
        //                // 去除空格  
        //                l = l.TrimEnd();
        //                // 如果是空行，则跳出循环  
        //                if (l == "") continue;
        //                // 如果是注释，则跳出循环  
        //                if (l.StartsWith("--")) continue;

        //                // 行数加1   
        //                line += l;
        //                // 如果不是完整的一条语句，则继续读取  
        //                if (!line.EndsWith(";")) continue;
        //                if (line.StartsWith("/*!"))
        //                {
        //                    continue;
        //                }
        //                if (line.Equals("DELIMITER ;;")) continue;
        //                //执行当前行  
        //                SQLUtilities.ExecuteNonQuery(dbString, line, null);
        //            }
        //        }
        //        catch (Exception ex)
        //        { 
        //        }
        //        finally
        //        {

        //        }
        //    }

        //    return true;
        //}  

        ///// <summary>
        ///// 初始化数据设备
        ///// </summary>
        ///// <param name="Project"></param>
        ///// <param name="Role_id"></param>
        //public static void DbInit(int Ledger, string Project, int Role_id)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("Ledger",Ledger),
        //        new SQLParameter("Project",Project),
        //        new SQLParameter("Role_id",Role_id),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Clear();
        //    strSql.Append("select UserType from sys_role where Ledger=@Ledger and Role_Id=@Role_id");
        //    object obj = SQLUtilities.ExecuteScalar(strSql.ToString(), params01);
        //    int userType = CommFunc.ConvertDBNullToInt32(obj);
        //    strSql.Clear();
        //    strSql.Append("SET group_concat_max_len = 20000;");/*最大字符串*/
        //    strSql.Append("SET GLOBAL log_bin_trust_function_creators = 1;");/*安全性设置*/
        //    if (userType == 1)
        //    {
        //        strSql.Append("insert into sys_user_prog(Ledger,Role_Id,prog_id,_read,_write,_delete,_app,_area)");
        //        strSql.Append("select @Ledger,@Role_id,a.prog_id,1,1,1,1,''");
        //        strSql.Append(" from sys_menu as a");
        //        strSql.Append(" where a.ledger=@Ledger and a.disabled=0 and not EXISTS(select * from sys_user_prog where Ledger=@Ledger and Role_id=@Role_id and prog_id=a.prog_id);");
        //    }
        //    SQLUtilities.ExecuteNonQuery(strSql.ToString(), params01);
        //}
        ///// 设置程序集
        //internal static void SetSys_menu(int ledger,string sysMenu, string prog_id, string parent_no, string descr, string absolutePath)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("ledger",ledger),
        //        new SQLParameter("project",sysMenu),
        //        new SQLParameter("prog_id",prog_id),
        //        new SQLParameter("descr",descr),
        //        new SQLParameter("parent_no",parent_no),
        //        new SQLParameter("path",absolutePath,64),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("insert into sys_menu(ledger,menu_no,descr,project,parent_no,prog_id,path)values(@ledger,@prog_id,@descr,@project,@parent_no,@prog_id,@path)");
        //    strSql.Append("ON DUPLICATE KEY UPDATE parent_no=@parent_no,path=@path");//descr=@descr,
        //    SQLUtilities.ExecuteNonQuery(strSql.ToString(), params01);
        //}
        ///// <summary>
        ///// 记录Log
        ///// </summary>
        ///// <param name="log"></param>
        //internal static void AddLog(sys_logVModel log)
        //{
        //    SQLParameter[] parameters = {
        //            new SQLParameter("@Ledger", log.Ledger),
        //            new SQLParameter("@Cdate", log.Cdate),
        //            new SQLParameter("@Uid", log.Uid),
        //            new SQLParameter("@Prog_id", log.Prog_id),
        //            new SQLParameter("@Control", log.Control),
        //            new SQLParameter("@Action", log.Action),
        //            new SQLParameter("@Content", log.Content),
        //            new SQLParameter("@Addr", log.Addr),
        //            new SQLParameter("@CTime", log.CTime)
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("insert into sys_log(");
        //    strSql.Append("Ledger,Cdate,Uid,Prog_id,Control,Action,Content,Addr,CTime)");
        //    strSql.Append(" values (");
        //    strSql.Append("@Ledger,@Cdate,@Uid,@Prog_id,@Control,@Action,@Content,@Addr,@CTime)");
        //    SQLUtilities.ExecuteNonQuery(strSql.ToString(), parameters);
        //}
        ///// 获取权限
        //internal static bool GetPower(int Ledger, int Role_id, string Prog_id)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("Ledger",Ledger),
        //        new SQLParameter("Role_id",Role_id),
        //        new SQLParameter("Prog_id",Prog_id),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select count(a.Ledger) as cnt");
        //    strSql.Append(" from sys_user_prog as a");
        //    strSql.Append(" where a.Ledger=@Ledger and a.Role_id=@Role_id and a.Prog_id=@Prog_id");
        //    strSql.Append(" and a._read + a._write + a._delete + a._app > 0 ");
        //    object obj = SQLUtilities.ExecuteScalar(strSql.ToString(), params01);
        //    return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        //}

        ///// <summary>
        ///// 获取菜单列表
        ///// </summary>
        ///// <param name="Ledger"></param>
        ///// <param name="Uid"></param>
        ///// <param name="Parent_no"></param>
        ///// <returns></returns>
        //internal static DataTable GetMenuList(int Ledger, int Uid, string Parent_no)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("Ledger",Ledger),
        //        new SQLParameter("Uid",Uid),
        //        new SQLParameter("Parent_no",Parent_no),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select m1.menu_no,m1.descr,m1.parent_no,m1.prog_id");
        //    strSql.Append(" from sys_user as a inner join sys_role as b on a.Ledger=b.Ledger and a.Role_id=b.Role_id");
        //    strSql.Append(" inner join sys_user_prog as c on b.Ledger=c.Ledger and b.Role_id=c.Role_Id");
        //    strSql.Append(" inner join sys_menu as m1 on m1.prog_id=c.prog_id");
        //    strSql.Append(" where a.Ledger=@Ledger and a.Uid=@Uid and m1.parent_no=@Parent_no and ifnull(m1.disabled,0)=0");
        //    strSql.Append(" and c._read + c._write + c._delete + c._app > 0 ");
        //    strSql.Append(" order by m1.ordno,m1.menu_no");
        //    DataSet ds = SQLUtilities.Query(strSql.ToString(), params01);
        //    return ds.Tables[0];
        //}

        /// <summary>
        /// 获取区域权限字符串
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public static bool GetAreaPower(string Project,int Ledger, int Uid, out string Area)
        {
            Area = "";
            object params01 = new
            {
                Ledger = Ledger,
                Uid = Uid,
                Project = Project
            };
            bool isPower = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select s1.role_id,s1.UserType,s2._area");
            strSql.Append(" from sys_user as s1 left join sys_user_prog as s2 on s1.Ledger=s2.Ledger and s1.role_id=s2.Role_Id and s2.prog_id=@Project");
            strSql.Append(" where s1.Ledger=@Ledger and s1.Uid=@Uid");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            foreach (DataRow dr in dt.Rows)
            {                
                Area = CommFunc.ConvertDBNullToString(dr["_area"]);
                if (string.IsNullOrEmpty(Area))/*如果没有任何区域权限，且是超级管理员不检测此权限*/
                    isPower = CommFunc.ConvertDBNullToInt32(dr["role_id"]) != 1;
            }
            if (string.IsNullOrEmpty(Area))
            {
                isPower = false; //不需要区域权限这功能了，因此返回false
            }
            return isPower;
        }

        /// <summary>
        /// 获取区域权限字符串
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Uid"></param>
        /// <returns></returns>
        public static bool GetAreaPower(int Ledger, int Uid, out string Area)
        {
            Area = "";
            object params01 = new
            {
                Ledger = Ledger,
                Uid = Uid,
            };
            bool isPower = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select s1.role_id,s1.UserType,s2._area");
            strSql.Append(" from sys_user as s1 inner join syscont as m on s1.Ledger=m.Ledger");
            strSql.Append(" left join sys_user_prog as s2 on s1.Ledger=s2.Ledger and s1.role_id=s2.Role_Id and s2.prog_id=m.Project");
            strSql.Append(" where s1.Ledger=@Ledger and s1.Uid=@Uid");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            foreach (DataRow dr in dt.Rows)
            {
                Area = CommFunc.ConvertDBNullToString(dr["_area"]);
                if (string.IsNullOrEmpty(Area))/*如果没有任何区域权限，且是超级管理员不检测此权限/*内置角色不检测区域*/
                    isPower = CommFunc.ConvertDBNullToInt32(dr["UserType"]) == 1 ? false : true;
            }
            if (string.IsNullOrEmpty(Area))
                isPower = false; //不需要区域权限这功能了，因此返回false
            return isPower;
        }

        /// 获取采集数据信息(Hisdata)
        internal static DataTable GetCoreQueryDataOnHisdata(int Ledger, string _mQuery, DateTime _mDtStart, DateTime _mDtEnd, string _mDateType)
        {
            object params01 = new {
                _mLedger=Ledger,
                _mQuery=_mQuery,
                _mDtStart=_mDtStart,
                _mDtEnd=_mDtEnd,
                _mDateType=_mDateType,
                _mDisBack=0,
            };
            DataTable dtSource = SQLHelper.ExecuteStoredProcedure("hisdata.sp_YdCoreQueryOnDetail", params01);
            return dtSource;
        }
        /// 获取采集数据信息
        //internal static DataTable GetCoreQueryData(int Ledger, string _mQuery, DateTime _mDtStart, DateTime _mDtEnd, string _mDateType)
        //{
        //    object params01 = new
        //    {
        //        _mLedger = Ledger,
        //        _mQuery = _mQuery,
        //        _mDtStart = _mDtStart,
        //        _mDtEnd = _mDtEnd,
        //        _mDateType = _mDateType,
        //        _mDisBack = 0
        //    };
        //    DataTable dtSource = SQLHelper.ExecuteStoredProcedure("sp_YdCoreQueryOnDetail", params01);
        //    return dtSource;
        //}

        ///// 获取采集数据信息
        //internal static DataTable GetCoreQueryData(int Ledger, string _mQuery, DateTime _mDtStart, DateTime _mDtEnd, string _mDateType, string _mFunType)
        //{
        //    object params01 = new
        //    {
        //        _mLedger = Ledger,
        //        _mQuery = _mQuery,
        //        _mDtStart = _mDtStart,
        //        _mDtEnd = _mDtEnd,
        //        _mDateType = _mDateType,
        //        _mFunType = _mFunType
        //    };
        //    DataTable dtSource = SQLHelper.ExecuteStoredProcedure("sp_YdCoreQuery", params01);
        //    return dtSource;
        //}
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="CfType"></param>
        /// <returns></returns>
        public static DataTable GetSysConfig(int Ledger, string CfType)
        {
            object params01 = new
            {
                Ledger = Ledger,
                CfType = CfType
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CfKey,CfType,CfValue,Rule from sys_config where Ledger=@Ledger");
            if (!string.IsNullOrEmpty(CfType))
                strSql.Append(" and CfType=@Cftype");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}
