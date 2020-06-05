using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.ExpAdminApp.Home
{
    public partial class ExpAdminHomeBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.ExpAdminApp.Home.ExpAdminHomeDAL dal = null;
        public ExpAdminHomeBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.ExpAdminApp.Home.ExpAdminHomeDAL(_project, _ledger, _uid);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSys_user(string uSign)
        {
            return dal.GetSys_user(uSign);
        }
        public int UpdateLoginErr(string uSign)
        {
            return dal.UpdateLogin(uSign, 1);
        }

        public int UpdateLoginSue(string uSign)
        {
            return dal.UpdateLogin(uSign, 0);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUser()
        {
            DataTable dtSource = dal.GetUser();
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            return dtSource;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="sys_user"></param>
        public int EditUser(sys_user sys_user)
        {
            return dal.EditUser(sys_user);
        }


        /// <summary>
        /// 运行历史报表
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <param name="DateType"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorOnList(string CoStrcName, string CoName, string DateType, DateTime Start, DateTime End, int Co_id)
        {
            DataTable dtRst = this.GetYdMotionTableScheam();
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"], dtRst.Columns["Module_id"] };
            DataTable dtSource = dal.GetYdMontionOnList(CoStrcName, CoName, Co_id);
            StringBuilder strSplit = new StringBuilder();
            StringBuilder strKey1 = new StringBuilder();

            int cnt = 0, RowId = 0;
            if (DateType.Equals("hour")) cnt = 24;
            if (DateType.Equals("day"))
            {
                DateTime td1 = new DateTime(Start.Year, Start.Month, 1);
                cnt = td1.AddMonths(1).AddDays(-1).Day;
            }
            if (DateType.Equals("month")) cnt = 12;
            if (DateType.Equals("other"))
            {
                cnt = 1;
            }
            //
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(strSplit.ToString()))
                    strSplit.Append(",");
                strSplit.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                //计算时间
                DateTime start = new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, 0, 0), end = new DateTime(End.Year, End.Month, End.Day, End.Hour, 0, 0);
                #region 填充主数据
                while (start <= end)
                {
                    strKey1.Clear();
                    if (DateType.Equals("hour"))
                        strKey1.Append(start.ToString("yyyy-MM-dd"));
                    if (DateType.Equals("day"))
                        strKey1.Append(start.ToString("yyyy-MM"));
                    if (DateType.Equals("month"))
                        strKey1.Append(start.ToString("yyyy"));
                    if (DateType.Equals("year"))
                        strKey1.Append(start.ToString("yyyy"));
                    if (DateType.Equals("other"))
                        strKey1.Append(start.ToString("yyyy-MM-dd"));
                    DataRow addDr = dtRst.Rows.Find(new object[] { start, dr["Module_id"] });
                    if (addDr == null)
                    { //增加需显示的表
                        addDr = dtRst.NewRow();
                        addDr["RowId"] = ++RowId;
                        addDr["TagTime"] = start;
                        addDr["Module_id"] = dr["Module_id"];
                        addDr["ModuleAddr"] = dr["ModuleAddr"];
                        addDr["ModuleName"] = dr["ModuleName"];
                        addDr["Multiply"] = dr["Multiply"];
                        addDr["Co_id"] = dr["Co_id"];
                        addDr["CoStrcName"] = dr["CoStrcName"];
                        addDr["CoName"] = dr["CoName"];
                        addDr["TagTimeS"] = strKey1.ToString();
                        addDr["Fun_id"] = dr["Fun_id"];
                        addDr["Scale"] = dr["Scale"];
                        addDr["Cnt"] = cnt;
                        dtRst.Rows.Add(addDr);
                        //
                    }
                    if (DateType.Equals("hour"))
                        start = start.AddDays(1);
                    if (DateType.Equals("day"))
                        start = start.AddMonths(1);
                    if (DateType.Equals("month"))
                        start = start.AddYears(1);
                    if (DateType.Equals("other"))
                    {
                        start = start.AddDays(1);
                    }
                }
                #endregion
            }
            if (DateType.Equals("other"))
            {
                DateType = "day";
            }
            #region 获取历史数据
            int others = 0;
            DataTable dtTag = WholeBLL.GetCoreQueryData(this.Ledger, strSplit.ToString(), Start, End, DateType);
            foreach (DataRow dr in dtTag.Rows)
            {
                DateTime tagtime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                DateTime key1 = tagtime;
                if (DateType.Equals("hour"))
                    key1 = new DateTime(tagtime.Year, tagtime.Month, tagtime.Day);
                if (DateType.Equals("day"))
                    key1 = new DateTime(tagtime.Year, tagtime.Month, 1);
                if (DateType.Equals("month"))
                    key1 = new DateTime(tagtime.Year, 1, 1);
                if (DateType.Equals("year"))
                    key1 = new DateTime(tagtime.Year, 1, 1);
                if (DateType.Equals("other"))
                    key1 = new DateTime(tagtime.Year, 1, 1);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);

                DataRow rst = dtRst.Rows.Find(new object[] { key1, dr["Module_id"] });
                if (rst == null) continue;
                if (CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) != CommFunc.ConvertDBNullToInt32(rst["Fun_id"])) continue;
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) != CommFunc.ConvertDBNullToInt32(rst["Co_id"])) continue;

                int scale = CommFunc.ConvertDBNullToInt32(rst["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(rst["Multiply"]);
                decimal useVal = (lastVal - firstVal) * multiply;
                if (scale != 0)
                    useVal = Math.Round(useVal, scale, MidpointRounding.AwayFromZero);

                int tagRow = 0;
                if (DateType.Equals("hour"))
                    tagRow = tagtime.Hour;
                if (DateType.Equals("day"))
                    tagRow = tagtime.Day - 1;
                if (DateType.Equals("month"))
                    tagRow = tagtime.Month - 1;
                if (DateType.Equals("other"))
                    tagRow = others;
                rst["h" + tagRow.ToString().PadLeft(2, '0')] = useVal; // dr["LastVal"];
                others = others + 1;
            }
            #endregion
            //////
            return dtRst;
        }


        private DataTable GetYdMotionTableScheam()
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("RowId", typeof(System.Int32));
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("Co_id", typeof(System.Int32));
            dtRst.Columns.Add("CoStrcName", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.Columns.Add("TagTimeS", typeof(System.String));
            dtRst.Columns.Add("TagName", typeof(System.String));
            dtRst.Columns.Add("Multiply", typeof(System.Decimal));/*倍率*/
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));
            dtRst.Columns.Add("Scale", typeof(System.Int32));
            dtRst.Columns.Add("Cnt", typeof(System.Int32));
            dtRst.Columns.Add("h00", typeof(System.Decimal));
            dtRst.Columns.Add("h01", typeof(System.Decimal));
            dtRst.Columns.Add("h02", typeof(System.Decimal));
            dtRst.Columns.Add("h03", typeof(System.Decimal));
            dtRst.Columns.Add("h04", typeof(System.Decimal));
            dtRst.Columns.Add("h05", typeof(System.Decimal));
            dtRst.Columns.Add("h06", typeof(System.Decimal));
            dtRst.Columns.Add("h07", typeof(System.Decimal));
            dtRst.Columns.Add("h08", typeof(System.Decimal));
            dtRst.Columns.Add("h09", typeof(System.Decimal));
            dtRst.Columns.Add("h10", typeof(System.Decimal));
            dtRst.Columns.Add("h11", typeof(System.Decimal));
            dtRst.Columns.Add("h12", typeof(System.Decimal));
            dtRst.Columns.Add("h13", typeof(System.Decimal));
            dtRst.Columns.Add("h14", typeof(System.Decimal));
            dtRst.Columns.Add("h15", typeof(System.Decimal));
            dtRst.Columns.Add("h16", typeof(System.Decimal));
            dtRst.Columns.Add("h17", typeof(System.Decimal));
            dtRst.Columns.Add("h18", typeof(System.Decimal));
            dtRst.Columns.Add("h19", typeof(System.Decimal));
            dtRst.Columns.Add("h20", typeof(System.Decimal));
            dtRst.Columns.Add("h21", typeof(System.Decimal));
            dtRst.Columns.Add("h22", typeof(System.Decimal));
            dtRst.Columns.Add("h23", typeof(System.Decimal));
            dtRst.Columns.Add("h24", typeof(System.Decimal));
            dtRst.Columns.Add("h25", typeof(System.Decimal));
            dtRst.Columns.Add("h26", typeof(System.Decimal));
            dtRst.Columns.Add("h27", typeof(System.Decimal));
            dtRst.Columns.Add("h28", typeof(System.Decimal));
            dtRst.Columns.Add("h29", typeof(System.Decimal));
            dtRst.Columns.Add("h30", typeof(System.Decimal));
            return dtRst;
        }

        /// <summary>
        /// 获取首页缴费
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetHomePay(string StartTime, string EndTime)
        {
            DataTable dtSource = dal.GetHomePay(StartTime, EndTime);
            return dtSource;
        }

        /// <summary>
        /// 获取首页建筑数和房间(总用户数量取数)
        /// </summary>
        /// <param name="Attrib">// 0=空,100=建筑,9000=房间</param>
        /// <returns></returns>
        public DataTable GetHomeBuilding(int Attrib)
        {
            return dal.GetHomeBuilding(Attrib);
        }

        /// <summary>
        /// 获取首页设备数(仪表数)
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeModule()
        {
            return dal.GetHomeModule();
        }



    }
}
