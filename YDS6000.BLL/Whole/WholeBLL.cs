using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL
{
    public class WholeBLL
    {        
        public static void ConnectionString(string connectionString)
        {
            new DBUtility.SQLHelper(connectionString);
        }
        /// <summary>
        /// 获取采集的历史数据
        /// </summary>
        /// <param name="ledger">账目</param>
        /// <param name="idlist">电表ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="dataType">查询类型 hour 小时 day 天 month 月</param>
        /// <param name="funType">查询数据类型</param>
        /// <returns></returns>
        internal static DataTable GetCoreQueryData(int ledger, string idlist, DateTime start, DateTime end, string dataType, string funType = "E")
        {
            return YDS6000.DAL.WHoleDAL.GetCoreQueryData(ledger, idlist, start, end, dataType, funType);
        }


        public static bool ConneectingServices(string HOST, string DBNAME, string DBUID, string DBPWD)
        {
            string connectionStrings = ConfigHelper.GetConnectionStrings("DefaultConnection");
            string connectionString = string.Format(connectionStrings, HOST, DBNAME, DBUID, DBPWD);
            try
            {
                DBUtility.SQLHelper.ConneectingServices(connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static void AddLog(int ledger,int uid,string prog_id,string userHostAddress,string controllerName,string actionName,string content)
        {
            YDS6000.DAL.WHoleDAL.AddLog(ledger, uid, prog_id, userHostAddress, controllerName, actionName, content);
        }
        //internal static string splitException = "YDH20D";
        //internal static string MdItems = "";

        //public static void MdItemsInit(string mdItems)
        //{
        //    MdItems = mdItems;
        //    DAL.WholeDAL.MdItems = mdItems;
        //}
        ///// <summary>
        ///// 解密
        ///// </summary>
        ///// <param name="Key"></param>
        ///// <returns></returns>
        //public static string Decrypt(string Key)
        //{
        //    return DAL.WholeDAL.Decrypt(Key);
        //}
        ///// <summary>
        ///// 加密
        ///// </summary>
        ///// <param name="Key"></param>
        ///// <returns></returns>
        //public static string Encrypt(string Key)
        //{
        //    return DESEncrypt.Encrypt(Key); //加密;
        //}
        ///// <summary>
        ///// 完整的连接字符串
        ///// </summary>
        ///// <param name="Key"></param>
        ///// <returns></returns>
        //public static string GetConnectionStrings()
        //{
        //    return DBUtility.ConfigHelper.GetFullConnectionStrings("Connection");
        //}

        ///// <summary>
        ///// 连接字符串
        ///// </summary>
        ///// <param name="Key"></param>
        ///// <returns></returns>
        //public static string GetConnectionStrings(string Key)
        //{
        //    return DBUtility.ConfigHelper.GetConnectionStrings(Key);
        //}


        ///// <summary>
        ///// 获取配置节点数据
        ///// </summary>
        ///// <param name="setValue"></param>
        ///// <returns></returns>
        //public static string GetAppSettings(string Key)
        //{
        //    return DBUtility.ConfigHelper.GetAppSettings(Key);
        //}
        ///// <summary>
        ///// 设置配置节点数据
        ///// </summary>
        ///// <param name="Key"></param>
        ///// <param name="Value"></param>
        ///// <returns></returns>
        //public static bool SetAppSettings(string Key, string Value)
        //{
        //    return DBUtility.ConfigHelper.SetAppSettings(Key, Value);
        //}

        //public static void SetConnectingString()
        //{
        //    new DBUtility.SQLUtilities();
        //}

        //public static void SetConnectingString(string connectStr)
        //{
        //    new DBUtility.SQLUtilities(connectStr);
        //}

        //public static bool ConneectingServices(string HOST, string DBNAME, string DBUID, string DBPWD)
        //{
        //    string connectionStrings = DBUtility.ConfigHelper.GetConnectionStrings("Connection");
        //    string connectionString = string.Format(connectionStrings, HOST, DBNAME, DBUID, DBPWD);
        //    try
        //    {
        //        DBUtility.SQLUtilities.Connection(connectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// 数据库是否存在
        ///// </summary>
        ///// <param name="HOST"></param>
        ///// <param name="DBNAME"></param>
        ///// <param name="DBUID"></param>
        ///// <param name="DBPWD"></param>
        ///// <returns></returns>
        //public static bool Db_Exis(string HOST, string DBNAME, string DBUID, string DBPWD)
        //{
        //    return DAL.WholeDAL.Db_Exis(HOST, DBNAME, DBUID, DBPWD);
        //}

        ///// <summary>
        ///// 获取电脑序列号
        ///// </summary>
        ///// <returns></returns>
        //public static string GetComputerSn()
        //{
        //    return License.GetComputerInfo();
        //}


        ///// <summary>
        ///// 初始化数据库，初始化最高管理员权限
        ///// </summary>
        ///// <param name="Project"></param>
        ///// <param name="Role_id"></param>
        //public static void DbInit(int Ledger, string Project, int Role_id)
        //{
        //    DAL.WholeDAL.DbInit(Ledger, Project, Role_id);
        //}
        ///// <summary>
        ///// 初始化数据库
        ///// </summary>
        ///// <param name="HOST"></param>
        ///// <param name="DBNAME"></param>
        ///// <param name="DBUID"></param>
        ///// <param name="DBPWD"></param>
        //public static void DbCreate(string HOST, string DBNAME, string DBUID, string DBPWD)
        //{
        //    DAL.WholeDAL.DbCreate(HOST, DBNAME, DBUID, DBPWD);
        //}

        ///// 设置程序集
        //public static void SetSys_menu(int ledger, string sysMenu, string prog_id, string descr, string absolutePath)
        //{
        //    string parent_no = "";
        //    string[] array = prog_id.Split('_');
        //    if (array.Length - 1 != 0)
        //    {
        //        for (int i = 0; i < array.Length - 1; i++)
        //        {
        //            if (i == 0)
        //            {
        //                parent_no = array[i];
        //            }
        //            else
        //                parent_no += "_" + array[i];
        //        }
        //    }
        //    DAL.WholeDAL.SetSys_menu(ledger, sysMenu, prog_id, parent_no, descr, absolutePath);
        //}
        //// /// 记录Log
        //public static void AddLog(sys_logVModel log)
        //{
        //    DAL.WholeDAL.AddLog(log);
        //}
        ///// <summary>
        ///// 获取权限
        ///// </summary>
        ///// <param name="Ledger"></param>
        ///// <param name="Role_id"></param>
        ///// <param name="Prog_id"></param>
        ///// <returns></returns>
        //public static bool GetPower(int Ledger, int Role_id, string Prog_id)
        //{
        //    return DAL.WholeDAL.GetPower(Ledger, Role_id, Prog_id);
        //}
        ///// <summary>
        ///// 获取菜单列表
        ///// </summary>
        ///// <param name="Ledger"></param>
        ///// <param name="Uid"></param>
        ///// <param name="Parent_no"></param>
        ///// <returns></returns>
        //public static DataTable GetMenuList(int Ledger, int Uid, string Parent_no)
        //{
        //    return DAL.WholeDAL.GetMenuList(Ledger, Uid, Parent_no);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ledger">账目</param>
        /// <param name="Uid">登陆人</param>
        /// <param name="Area">返回区域ID字符串</param>
        /// <returns>true 检测权限，fale 不需要检测权限</returns>
        public static bool GetAreaPower(string Project, int Ledger, int Uid, out string Area)
        {
            return DAL.WHoleDAL.GetAreaPower(Project, Ledger, Uid, out Area);
        }
        ///// <summary>
        ///// 获取采集数据信息(Hisdata)
        ///// </summary>
        ///// <param name="Ledger">账目</param>
        ///// <param name="_mQuery">查询tanName字符串</param>
        ///// <param name="_mDtStart">开始时间</param>
        ///// <param name="_mDtEnd">结束时间</param>
        ///// <param name="_mDateType">类型，hour,day,week,month,year</param>
        ///// <param name="_mDisBack">是否返回结果，默认返回</param>
        ///// <returns></returns>
        //public static DataTable GetCoreQueryDataOnHisdata(int Ledger, string _mQuery, DateTime _mDtStart, DateTime _mDtEnd, string _mDateType)
        //{
        //    return DAL.WholeDAL.GetCoreQueryDataOnHisdata(Ledger, _mQuery, _mDtStart, _mDtEnd, _mDateType);
        //}
        ///// <summary>
        ///// 获取采集数据信息
        ///// </summary>
        ///// <param name="Ledger">账目</param>
        ///// <param name="_mQuery">查询tanName字符串</param>
        ///// <param name="_mDtStart">开始时间</param>
        ///// <param name="_mDtEnd">结束时间</param>
        ///// <param name="_mDateType">类型，hour,day,week,month,year</param>
        ///// <param name="_mDisBack">是否返回结果，默认返回</param>
        ///// <returns></returns>
        //public static DataTable GetCoreQueryData_Old(int Ledger, string _mQuery, DateTime _mDtStart, DateTime _mDtEnd, string _mDateType)
        //{
        //    return DAL.WholeDAL.GetCoreQueryData(Ledger, _mQuery, _mDtStart, _mDtEnd, _mDateType);
        //}
        ///// <summary>
        ///// 获取采集数据信息
        ///// </summary>
        ///// <param name="Ledger">账目</param>
        ///// <param name="_mQuery">查询tanName字符串</param>
        ///// <param name="_mDtStart">开始时间</param>
        ///// <param name="_mDtEnd">结束时间</param>
        ///// <param name="_mDateType">类型，hour,day,week,month,year</param>
        ///// <param name="_mDisBack">是否返回结果，默认返回</param>
        ///// <returns></returns>
        //public static DataTable GetCoreQueryData(int Ledger, string _mQuery, DateTime _mDtStart, DateTime _mDtEnd, string _mDateType, string _mFunType = "E")
        //{
        //    return DAL.WholeDAL.GetCoreQueryData(Ledger, _mQuery, _mDtStart, _mDtEnd, _mDateType, _mFunType);
        //}
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="CfType"></param>
        /// <returns></returns>
        public static DataTable GetSysConfig(int Ledger, string CfType)
        {
            return DAL.WHoleDAL.GetSysConfig(Ledger, CfType);
        }

    }

}
