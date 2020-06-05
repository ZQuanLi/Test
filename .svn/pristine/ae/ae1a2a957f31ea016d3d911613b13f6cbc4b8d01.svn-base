using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;


namespace YDS6000.BLL.Exp.RunReport
{
    public partial class ExpYdMotionBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.RunReport.ExpYdMotionDAL dal = null;
        public ExpYdMotionBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.RunReport.ExpYdMotionDAL(_project, _ledger, _uid);
        }

        public DataTable GetYdMonitorOnList(string CoStrcName, string CoName, string DateType, DateTime Start, DateTime End)
        {
            DataTable dtRst = this.GetYdMotionTableScheam();
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"], dtRst.Columns["Module_id"], dtRst.Columns["Fun_id"] };
            DataTable dtSource = dal.GetYdMontionOnList(CoStrcName, CoName);
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
                    DataRow addDr = dtRst.Rows.Find(new object[] { start, dr["Module_id"], dr["Fun_id"] });
                    if (addDr == null)
                    { //增加需显示的表
                        addDr = dtRst.NewRow();
                        addDr["RowId"] = ++RowId;
                        addDr["TagTime"] = start;
                        addDr["Module_id"] = dr["Module_id"];
                        addDr["Fun_id"] = dr["Fun_id"];
                        addDr["FunType"] = dr["FunType"];
                        addDr["FunName"] = dr["FunName"];
                        addDr["ModuleAddr"] = dr["ModuleAddr"];
                        addDr["ModuleName"] = dr["ModuleName"];
                        addDr["Multiply"] = dr["Multiply"];
                        addDr["Co_id"] = dr["Co_id"];
                        addDr["CoStrcName"] = dr["CoStrcName"];
                        addDr["CoName"] = dr["CoName"];
                        addDr["TagTimeS"] = strKey1.ToString();
                        //addDr["Fun_id"] = dr["Fun_id"];
                        addDr["Scale"] = dr["Scale"];
                        addDr["Cnt"] = cnt;
                        dtRst.Rows.Add(addDr);
                        //
                    }
                    if (DateType.Equals("hour"))
                        start = start.AddDays(1);
                    else if (DateType.Equals("day"))
                        start = start.AddMonths(1);
                    else if (DateType.Equals("month"))
                        start = start.AddYears(1);
                    break;
                }
                #endregion
            }
            #region 获取历史数据
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
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);

                DataRow rst = dtRst.Rows.Find(new object[] { key1, dr["Module_id"] , dr["Fun_id"] });
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
                rst["h" + tagRow.ToString().PadLeft(2, '0')] = useVal; // dr["LastVal"];
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
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));
            dtRst.Columns.Add("FunType", typeof(System.String));
            dtRst.Columns.Add("FunName", typeof(System.String));
            dtRst.Columns.Add("RowId", typeof(System.Int32));
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("Co_id", typeof(System.Int32));
            dtRst.Columns.Add("CoStrcName", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.Columns.Add("TagTimeS", typeof(System.String));
            dtRst.Columns.Add("TagName", typeof(System.String));
            dtRst.Columns.Add("Multiply", typeof(System.Decimal));/*倍率*/
            //dtRst.Columns.Add("Fun_id", typeof(System.Int32));
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

        private DataTable GetYdHisTableScheam()
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));
            dtRst.Columns.Add("FunType", typeof(System.String));
            dtRst.Columns.Add("FunName", typeof(System.String));
            dtRst.Columns.Add("RowId", typeof(System.Int32));
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("Co_id", typeof(System.Int32));
            dtRst.Columns.Add("CoStrcName", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.Columns.Add("TagTimeS", typeof(System.String));
            dtRst.Columns.Add("TagName", typeof(System.String));
            dtRst.Columns.Add("Multiply", typeof(System.Decimal));/*倍率*/
            dtRst.Columns.Add("Scale", typeof(System.Int32));
            dtRst.Columns.Add("Cnt", typeof(System.Int32));
            dtRst.Columns.Add("Value", typeof(System.Decimal));         
            return dtRst;
        }
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="attrib"></param>
        /// <returns></returns>
        public DataTable GetVp_coinfo(CoAttribV2_1 attrib)
        {
            return dal.GetVp_coinfo(attrib);
        }

        public DataTable GetModuleCombox(int co_id)
        {
            return dal.GetModuleCombox(co_id);
        }
        public DataTable GetFunCombox(int module_id)
        {
            return dal.GetFunCombox(module_id);
        }

        /// <summary>
        /// 历史数据
        /// </summary>
        ///// <param name="co_id">用电单元ID号</param>
        /// <param name="module_id">回路ID号</param>
        /// <param name="funType">采集项目</param>
        /// <param name="dataType">查询类型</param>
        /// <param name="startTime">开始日期</param>
        /// <returns></returns>
        public DataTable GetHisData(int module_id, string funType, string dataType, DateTime start,DateTime end)
        {
            dataType = dataType.ToLower(); 
            DataTable dtRst = this.GetYdHisTableScheam();
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"], dtRst.Columns["Module_id"], dtRst.Columns["Fun_id"] };
            DataTable dtSource = dal.GetYdMontionOnList(module_id, funType);
            StringBuilder strSplit = new StringBuilder();
            StringBuilder strKey1 = new StringBuilder();

            int cnt = 0, RowId = 0;
            if (dataType.Equals("hour")) cnt = 24;
            if (dataType.Equals("day"))
            {
                DateTime td1 = new DateTime(start.Year, end.Month, 1);
                cnt = td1.AddMonths(1).AddDays(-1).Day;
            }
            if (dataType.Equals("month")) cnt = 12;
            //
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(strSplit.ToString()))
                    strSplit.Append(",");
                strSplit.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                //计算时间
                DateTime begin = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0), over = new DateTime(end.Year, end.Month, end.Day, end.Hour, 0, 0);
                #region 填充主数据
                while (begin <= over)
                {
                    strKey1.Clear();
                    if (dataType.Equals("hour"))
                        strKey1.Append(begin.ToString("yyyy-MM-dd"));
                    if (dataType.Equals("day"))
                        strKey1.Append(begin.ToString("yyyy-MM"));
                    if (dataType.Equals("month"))
                        strKey1.Append(begin.ToString("yyyy"));
                    if (dataType.Equals("year"))
                        strKey1.Append(begin.ToString("yyyy"));
                    DataRow addDr = dtRst.Rows.Find(new object[] { begin, dr["Module_id"], dr["Fun_id"] });
                    if (addDr == null)
                    { //增加需显示的表
                        addDr = dtRst.NewRow();
                        addDr["RowId"] = ++RowId;
                        addDr["TagTime"] = begin;
                        addDr["Module_id"] = dr["Module_id"];
                        addDr["Fun_id"] = dr["Fun_id"];
                        addDr["FunType"] = dr["FunType"];
                        addDr["FunName"] = dr["FunName"];
                        addDr["ModuleAddr"] = dr["ModuleAddr"];
                        addDr["ModuleName"] = dr["ModuleName"];
                        addDr["Multiply"] = dr["Multiply"];
                        addDr["Co_id"] = dr["Co_id"];
                        addDr["CoStrcName"] = dr["CoStrcName"];
                        addDr["CoName"] = dr["CoName"];
                        addDr["TagTimeS"] = strKey1.ToString();
                        //addDr["Fun_id"] = dr["Fun_id"];
                        addDr["Scale"] = dr["Scale"];
                        addDr["Cnt"] = cnt;
                        addDr["Value"] = 0;
                        dtRst.Rows.Add(addDr);
                    }
                    if (dataType.Equals("hour"))
                        begin = begin.AddHours(1);
                    else if (dataType.Equals("day"))
                        begin = begin.AddDays(1);
                    else if(dataType.Equals("month"))
                        begin = begin.AddMonths(1);
                    else break;
                }
                #endregion
            }
            #region 获取历史数据
            DataTable dtTag = WholeBLL.GetCoreQueryData(this.Ledger, strSplit.ToString(), start, end, dataType, funType);
            foreach (DataRow dr in dtTag.Rows)
            {
                DateTime tagtime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                DateTime key1 = tagtime;
                if (dataType.Equals("hour"))
                    key1 = new DateTime(tagtime.Year, tagtime.Month, tagtime.Day);
                if (dataType.Equals("day"))
                    key1 = new DateTime(tagtime.Year, tagtime.Month, 1);
                if (dataType.Equals("month"))
                    key1 = new DateTime(tagtime.Year, 1, 1);
                if (dataType.Equals("year"))
                    key1 = new DateTime(tagtime.Year, 1, 1);

                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);

                DataRow rst = dtRst.Rows.Find(new object[] { tagtime, dr["Module_id"], dr["Fun_id"] });
                if (rst == null) continue;
                if (CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) != CommFunc.ConvertDBNullToInt32(rst["Fun_id"])) continue;
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) != CommFunc.ConvertDBNullToInt32(rst["Co_id"])) continue;

                int scale = CommFunc.ConvertDBNullToInt32(rst["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(rst["Multiply"]);
                decimal useVal = lastVal;

                if (CommFunc.ConvertDBNullToString(rst["FunType"]).Equals("E"))
                {
                    useVal = (lastVal - firstVal) * multiply;
                    if (scale != 0)
                        useVal = Math.Round(useVal, scale, MidpointRounding.AwayFromZero);
                }
                rst["Value"] = useVal;
                //int tagRow = 0;
                //if (dataType.Equals("hour"))
                //    tagRow = tagtime.Hour;
                //if (dataType.Equals("day"))
                //    tagRow = tagtime.Day - 1;
                //if (dataType.Equals("month"))
                //    tagRow = tagtime.Month - 1;
                //rst["h" + tagRow.ToString().PadLeft(2, '0')] = useVal; // dr["LastVal"];
            }
            #endregion
            return dtRst;
        }

        /// <summary>
        /// 数据对比
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="unitId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetCompare(int areaId, int unitId, int projectId)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Month", typeof(System.Int32));
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("CurTims", typeof(System.Int32));
            dtRst.Columns.Add("PreTims", typeof(System.Int32));
            dtRst.Columns.Add("Per", typeof(System.Int32));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Month"] };
            int cc = 0;
            DateTime dd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime minTime = dd;
            while (cc >= -11)
            {
                minTime = dd.AddMonths(cc--);
                DataRow addDr = dtRst.Rows.Find(minTime.Month);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Month"] = minTime.Month;
                    addDr["TagTime"] = minTime;
                    addDr["CurTims"] = 0;
                    addDr["PreTims"] = 0;
                    addDr["Per"] = 0;
                    dtRst.Rows.Add(addDr);
                }
            }

            DataTable dtSource = dal.GetCompare(areaId, unitId, projectId);
            foreach (DataRow dr in dtSource.Rows)
            {
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["CDate"]);
                tagTime = new DateTime(tagTime.Year, tagTime.Month, 1);
                DataRow curDr = dtRst.Rows.Find(tagTime.Month);
                if (curDr == null) continue;
                if (tagTime >= minTime)
                    curDr["CurTims"] = CommFunc.ConvertDBNullToInt32(curDr["CurTims"]) + 1;
                else
                    curDr["PreTims"] = CommFunc.ConvertDBNullToInt32(curDr["PreTims"]) + 1;
                int curTime = CommFunc.ConvertDBNullToInt32(curDr["CurTims"]);
                int preTime = CommFunc.ConvertDBNullToInt32(curDr["PreTims"]);
                curDr["Per"] = preTime == 0 ? 0 : (int)((curTime - preTime) / (decimal)preTime) * 100;
            }
            return dtRst;
        }
    }
}
