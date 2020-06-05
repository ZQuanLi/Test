using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.ExpAdminApp.Opertion.Home
{
    public partial class ExpAdminHomeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.ExpAdminApp.Home.ExpAdminHomeBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpAdminHomeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.ExpAdminApp.Home.ExpAdminHomeBLL(WebConfig.SysProject, user.Ledger, user.Uid);
        }
        /// <summary>
        /// 登录用户_平台
        /// </summary>
        /// <param name="ledger"></param>
        /// <param name="uSign"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public APIRst Login(int ledger, string uSign, string pwd)
        {
            //bll = new YDS6000.BLL.ExpAdminApp.Home.ExpAdminHomeBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            APIRst rst = new APIRst();
            if (string.IsNullOrEmpty(uSign))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "用户名不能为空";
                return rst;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "密码不能为空";
                return rst;

            }

            try
            {
                DataTable dtLedger = new YDS6000.BLL.ExpApp.Home.HomeBLL(9999, 0).GetProjectList();
                foreach (DataRow dr in dtLedger.Rows)
                    ledger = CommFunc.ConvertDBNullToInt32(dr["Rule"]);
                bll = new YDS6000.BLL.ExpAdminApp.Home.ExpAdminHomeBLL(WebConfig.SysProject, ledger, user.Uid);

                DataTable dtSource = bll.GetSys_user(uSign);
                int nRows = dtSource.Rows.Count;
                if (nRows == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "没有此用户";
                    return rst;
                }
                else if (nRows != 1)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "此用户有多个";
                    return rst;
                }

                int uid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Uid"]);
                string uName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UName"]);
                string dbPwd = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UPasswd"]);
                string project = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Project"]);
                int role_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Role_id"]);
                if (!pwd.Trim().Equals(dbPwd))
                {
                    //查询最后最后的登录时间，判断如果少于十五分钟且错误次数已经为五次，提示十五分钟后再次登录
                    int num = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["LoginInt"]);
                    DateTime dt = CommFunc.ConvertDBNullToDateTime(dtSource.Rows[0]["LoginDate"]);
                    TimeSpan ts = new TimeSpan();
                    ts = DateTime.Now - dt; //现在时间-数据库时间
                    int Result = Convert.ToInt32(ts.TotalMinutes); //转换时间间隔为 分钟  Double型转化成Int型
                    if (Result < 15 && num > 4)
                    {
                        rst.err.code = (int)ResultCodeDefine.Error;
                        rst.err.msg = "登录的次数超过了规定次数，请十五分钟后再试";
                        return rst;
                    }
                    //登录密码错误，将登录错误次数+1
                    bll.UpdateLoginErr(uSign);
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "密码错误";
                    return rst;
                }
                //////
                //WebConfig.Ledger = ledger;
                WebConfig.SysProject = project;
                //////
                int seed = new Random(Guid.NewGuid().GetHashCode()).Next(65, 91);
                string zm = CommFunc.NunberToChar(seed);
                zm = string.IsNullOrEmpty(zm) ? "A" : zm;
                string ticket = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + zm + (ledger + seed).ToString() + zm + (seed + uid).ToString();
                string ccKey = ledger.ToString() + "AdminApp" + uid.ToString();
                CacheUser cacheUser = new CacheUser();
                cacheUser.Ledger = ledger;
                cacheUser.Uid = uid;
                cacheUser.USign = uSign;
                cacheUser.Role_id = role_id;
                cacheUser.CacheKey = project + "->" + ledger + "->";
                //随机数前10位+ 种子数+ (种子数+用户数之和)
                cacheUser.Ticket = ticket;// Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + (seed + cacheUser.Uid).ToString().PadLeft(7, '0');
                //
                bll.UpdateLoginSue(uSign);
                rst.err.code = 0;
                rst.data = new { Ticket = cacheUser.Ticket, Name = uName };
                //
                int timeSpan = CommFunc.ConvertDBNullToInt32(ConfigHelper.GetAppSettings("Cached:Time"));
                timeSpan = timeSpan == 0 ? 15 : timeSpan;
                HttpRuntime.Cache.Insert(ccKey, cacheUser, null, DateTime.MaxValue, TimeSpan.FromMinutes(timeSpan));
                HttpContext.Current.Session["CacheUser"] = cacheUser;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("登录错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetUser()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetUser();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                                   Rid = CommFunc.ConvertDBNullToInt32(s1["Role_id"]),
                                   Ledger = CommFunc.ConvertDBNullToInt32(s1["Ledger"]),
                                   Uid = CommFunc.ConvertDBNullToInt32(s1["Uid"]),
                                   Name = CommFunc.ConvertDBNullToString(s1["UName"]),
                                   Pwd = CommFunc.ConvertDBNullToString(s1["UPasswd"]),
                                   SName = CommFunc.ConvertDBNullToString(s1["USign"]),
                                   RName = CommFunc.ConvertDBNullToString(s1["Descr"]),
                                   Create_by = CommFunc.ConvertDBNullToInt32(s1["Create_by"]),
                                   Create_byName = CommFunc.ConvertDBNullToString(s1["Create_byName"]),
                                   Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd")
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="Uid">用户ID</param>
        /// <param name="sAct">操作类型：增加=1，修改=2，删除=3</param>
        /// <param name="Rid">用户权限</param>
        /// <param name="Name">用户名称</param>
        /// <param name="SName">登录名</param>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        public APIRst SetUser(string Name, string Pwd)
        {
            APIRst rst = new APIRst();
            try
            {
                if (string.IsNullOrEmpty(Name) || Name == "{Name}" || Name == "null")
                    Name = string.Empty;
                if (string.IsNullOrEmpty(Pwd) || Pwd == "{Pwd}" || Pwd == "null")
                    Pwd = string.Empty;

                if (string.IsNullOrEmpty(Name))
                    throw new Exception(" 请输入用户名称");
                if (string.IsNullOrEmpty(Pwd))
                    throw new Exception(" 请输入用户密码");

                sys_user uu = new sys_user();
                uu.UName = Name;//用户名称
                uu.UPasswd = Pwd;//登录密码

                int total = bll.EditUser(uu);

                rst.data = new { Total = total, Name = Name };

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 运行历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="DateType">报表类型</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">结束开始</param>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public APIRst GetYdMontionOnList(string StrcName, string CoName, string DateType, DateTime StartTime, DateTime EndTime, int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                string time_str = CommFunc.ConvertDBNullToString(StartTime) + (DateType.Equals("month") ? "-01" : DateType.Equals("year") ? "-01-01" : "");
                DateTime start = CommFunc.ConvertDBNullToDateTime(time_str);
                DateTime end = start;
                string msg = "";
                if (DateType.Equals("hour"))
                {
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
                }
                if (DateType.Equals("day"))
                {
                    //start = new DateTime(start.Year, start.Month, start.Day);
                    start = new DateTime(start.Year, start.Month, 1);
                    end = start.AddMonths(1).AddDays(-1);
                }
                if (DateType.Equals("month"))
                {
                    start = new DateTime(start.Year, 1, 1);
                    end = start.AddYears(1).AddDays(-1);
                }
                if (DateType.Equals("year"))
                {
                    start = new DateTime(start.Year, 1, 1);
                    end = start.AddYears(1).AddDays(-1);
                }
                if (DateType.Equals("other"))
                {
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    end = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, 23, 59, 59);
                }
                int total = 0;

                //object rows = GetYdMontionOnList(StrcName, CoName, DateType, start, end, Co_id, out total);

                DataTable dtSource = bll.GetYdMonitorOnList(StrcName, CoName, DateType, start, end, Co_id);
                Decimal kWh = 0;
                foreach (DataRow row in dtSource.Rows)
                {
                    string h00 = row["h00"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h00"]).ToString();
                    string h01 = row["h01"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h01"]).ToString();
                    string h02 = row["h02"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h02"]).ToString();
                    string h03 = row["h03"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h03"]).ToString();
                    string h04 = row["h04"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h04"]).ToString();
                    string h05 = row["h05"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h05"]).ToString();
                    string h06 = row["h06"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h06"]).ToString();
                    string h07 = row["h07"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h07"]).ToString();
                    string h08 = row["h08"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h08"]).ToString();
                    string h09 = row["h09"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h09"]).ToString();
                    string h10 = row["h10"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h10"]).ToString();
                    string h11 = row["h11"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h11"]).ToString();
                    string h12 = row["h12"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h12"]).ToString();
                    string h13 = row["h13"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h13"]).ToString();
                    string h14 = row["h14"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h14"]).ToString();
                    string h15 = row["h15"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h15"]).ToString();
                    string h16 = row["h16"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h16"]).ToString();
                    string h17 = row["h17"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h17"]).ToString();
                    string h18 = row["h18"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h18"]).ToString();
                    string h19 = row["h19"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h19"]).ToString();
                    string h20 = row["h20"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h20"]).ToString();
                    string h21 = row["h21"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h21"]).ToString();
                    string h22 = row["h22"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h22"]).ToString();
                    string h23 = row["h23"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h23"]).ToString();
                    string h24 = row["h24"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h24"]).ToString();
                    string h25 = row["h25"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h25"]).ToString();
                    string h26 = row["h26"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h26"]).ToString();
                    string h27 = row["h27"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h27"]).ToString();
                    string h28 = row["h28"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h28"]).ToString();
                    string h29 = row["h29"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h29"]).ToString();
                    string h30 = row["h30"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(row["h30"]).ToString();
                    kWh += CommFunc.ConvertDBNullToDecimal(h00) + CommFunc.ConvertDBNullToDecimal(h01) + CommFunc.ConvertDBNullToDecimal(h02) + CommFunc.ConvertDBNullToDecimal(h03) + CommFunc.ConvertDBNullToDecimal(h04) + CommFunc.ConvertDBNullToDecimal(h05) + CommFunc.ConvertDBNullToDecimal(h06) + CommFunc.ConvertDBNullToDecimal(h07) + CommFunc.ConvertDBNullToDecimal(h08) + CommFunc.ConvertDBNullToDecimal(h09);
                    kWh += CommFunc.ConvertDBNullToDecimal(h10) + CommFunc.ConvertDBNullToDecimal(h11) + CommFunc.ConvertDBNullToDecimal(h12) + CommFunc.ConvertDBNullToDecimal(h13) + CommFunc.ConvertDBNullToDecimal(h14) + CommFunc.ConvertDBNullToDecimal(h15) + CommFunc.ConvertDBNullToDecimal(h16) + CommFunc.ConvertDBNullToDecimal(h17) + CommFunc.ConvertDBNullToDecimal(h18) + CommFunc.ConvertDBNullToDecimal(h19);
                    kWh += CommFunc.ConvertDBNullToDecimal(h20) + CommFunc.ConvertDBNullToDecimal(h21) + CommFunc.ConvertDBNullToDecimal(h22) + CommFunc.ConvertDBNullToDecimal(h23) + CommFunc.ConvertDBNullToDecimal(h24) + CommFunc.ConvertDBNullToDecimal(h25) + CommFunc.ConvertDBNullToDecimal(h26) + CommFunc.ConvertDBNullToDecimal(h27) + CommFunc.ConvertDBNullToDecimal(h28) + CommFunc.ConvertDBNullToDecimal(h29) + CommFunc.ConvertDBNullToDecimal(h30);
                    //int iii = CommFunc.ConvertDBNullToDateTime(row["TagTime"].ToString()).Month;                    
                }

                total = dtSource.Rows.Count;
                object obj = new { total = total, kWh = kWh };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("运行历史报表(GetYdMontionOnList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        private object GetYdMontionOnList(string CoStrcName, string CoName, string DateType, DateTime Start, DateTime End, int Co_id, out int total)
        {
            DataTable dtSource = bll.GetYdMonitorOnList(CoStrcName, CoName, DateType, Start, End, Co_id);
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                           TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                           ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                           ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                           CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           TagTimeS = CommFunc.ConvertDBNullToString(s1["TagTimeS"]),
                           Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
                           h00 = s1["h00"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h00"]).ToString(),
                           h01 = s1["h01"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h01"]).ToString(),
                           h02 = s1["h02"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h02"]).ToString(),
                           h03 = s1["h03"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h03"]).ToString(),
                           h04 = s1["h04"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h04"]).ToString(),
                           h05 = s1["h05"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h05"]).ToString(),
                           h06 = s1["h06"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h06"]).ToString(),
                           h07 = s1["h07"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h07"]).ToString(),
                           h08 = s1["h08"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h08"]).ToString(),
                           h09 = s1["h09"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h09"]).ToString(),
                           h10 = s1["h10"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h10"]).ToString(),
                           h11 = s1["h11"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h11"]).ToString(),
                           h12 = s1["h12"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h12"]).ToString(),
                           h13 = s1["h13"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h13"]).ToString(),
                           h14 = s1["h14"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h14"]).ToString(),
                           h15 = s1["h15"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h15"]).ToString(),
                           h16 = s1["h16"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h16"]).ToString(),
                           h17 = s1["h17"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h17"]).ToString(),
                           h18 = s1["h18"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h18"]).ToString(),
                           h19 = s1["h19"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h19"]).ToString(),
                           h20 = s1["h20"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h20"]).ToString(),
                           h21 = s1["h21"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h21"]).ToString(),
                           h22 = s1["h22"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h22"]).ToString(),
                           h23 = s1["h23"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h23"]).ToString(),
                           h24 = s1["h24"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h24"]).ToString(),
                           h25 = s1["h25"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h25"]).ToString(),
                           h26 = s1["h26"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h26"]).ToString(),
                           h27 = s1["h27"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h27"]).ToString(),
                           h28 = s1["h28"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h28"]).ToString(),
                           h29 = s1["h29"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h29"]).ToString(),
                           h30 = s1["h30"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h30"]).ToString()
                       };
            total = dtSource.Rows.Count;
            return res1.ToList();
        }


        /// <summary>
        /// 获取首页缴费和笔数
        /// </summary>
        /// <param name="DayOrMonth">1=查询当日,2=查询当月</param>
        /// <returns></returns>
        public APIRst GetHomePay(int DayOrMonth)
        {
            APIRst rst = new APIRst();
            try
            {
                var StartTime = "";
                var EndTime = "";
                if (DayOrMonth == 1) //查询当日
                {
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd");
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DayOrMonth == 2) //查询当月
                {
                    DateTime dt = DateTime.Now; //当前时间
                    StartTime = dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd"); //本月月初
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DataTable dtSource = bll.GetHomePay(StartTime, EndTime);
                decimal PayAmt = 0;
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    PayAmt = PayAmt + Convert.ToDecimal(dtSource.Rows[i]["PayAmt"].ToString());
                }

                object obj = new { Amount = dtSource.Rows.Count, PayAmt = PayAmt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页缴费(GetHomePay):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页建筑数和房间(总用户数量取数)
        /// </summary>
        /// <param name="Attrib">// 0=空,100=建筑,9000=房间</param>
        /// <returns></returns>
        public APIRst GetHomeBuilding(int Attrib)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetHomeBuilding(Attrib);
                //var res1 = from s1 in dtSource.AsEnumerable()
                //           select new
                //           {
                //               Cic_id = CommFunc.ConvertDBNullToInt32(s1["Cic_id"]),
                //               Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                //               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                //               Rule = CommFunc.ConvertDBNullToInt32(s1["Rule"]),
                //               Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                //               UnitBase = CommFunc.ConvertDBNullToDecimal(s1["UnitBase"]),
                //               CicName = CommFunc.ConvertDBNullToString(s1["CicName"]),
                //               RateName = CommFunc.ConvertDBNullToString(s1["RateName"]),
                //               RuleName = CommFunc.GetEnumDisplay(typeof(RateRule), CommFunc.ConvertDBNullToInt32(s1["Rule"])),
                //               UnitName = CommFunc.GetEnumDisplay(typeof(RateUnit), CommFunc.ConvertDBNullToString(s1["Unit"])),
                //           };
                object obj = new { Total = dtSource.Rows.Count };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页建筑数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页设备数(仪表数)
        /// </summary>
        /// <returns></returns>
        public APIRst GetHomeModule()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetHomeModule();
                object obj = new { Total = dtSource.Rows.Count };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页设备数(仪表数)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}