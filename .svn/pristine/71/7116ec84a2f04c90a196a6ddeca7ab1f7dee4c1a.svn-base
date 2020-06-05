using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using YDS6000.Models;
using Excel;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    partial class ChargeHelper
    {
        //缴费总列表
        public APIRst GetYdPrePayInMdOnList(string strcName,string coName, int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdPrePayInMdOnList(strcName, coName, co_id);
                int total = dtSource.Rows.Count;
                int RowId = 0;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = ++RowId,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),                       
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ChrgVal = CommFunc.ConvertDBNullToDecimal(s1["ChrgVal"]).ToString("f2"),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString("f2"),
                               RdVal = CommFunc.ConvertDBNullToDecimal(s1["RdVal"]).ToString("f2"),
                               RdAmt = (CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) * CommFunc.ConvertDBNullToDecimal(s1["Price"])).ToString("f2"),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]).ToString("f3"),
                               ZsVal = CommFunc.ConvertDBNullToDecimal(s1["ZsVal"]).ToString("f2"),
                               SyZsVal = CommFunc.ConvertDBNullToDecimal(s1["SyZsVal"]).ToString("f2"),
                               SyVal = (CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) - CommFunc.ConvertDBNullToDecimal(s1["SyZsVal"])).ToString("f2")
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取预付费页面错误：(GetYdPrePayInMdOnList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        // 充值明细信息
        public APIRst GetYdPrepPayInMd_01OnInfo(int co_id,int module_id,string moduleAddr,decimal price)
        {
            APIRst rst = new APIRst();
            try
            {
                decimal rdVal = 0;
                decimal payVal = 0, payAmt = 0, syVal = 0, syAmt = 0, chrgVal = 0, syZsVal = 0, syZsAmt = 0;
                DataTable dtSource = bll.GetYdPrepPayInMd_01OnInfo(co_id, module_id, moduleAddr);
                foreach (DataRow dr in dtSource.Rows)
                {
                    rdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
                    chrgVal = CommFunc.ConvertDBNullToDecimal(dr["ChrgVal"]);
                    syZsVal = CommFunc.ConvertDBNullToDecimal(dr["SyZsVal"]);
                }
                rdVal = rdVal - syZsVal;
                payVal = rdVal > 0 ? 0 : Math.Abs(rdVal);
                syVal = rdVal;
                payAmt = Math.Round(payVal * price, 2, MidpointRounding.AwayFromZero); /*需缴电费*/
                syAmt = Math.Round(syVal * price, 2, MidpointRounding.AwayFromZero); /*剩余电费*/
                //chrgVal = Math.Round(chrgVal * price, 2, MidpointRounding.AwayFromZero); /*总充值电量*/
                syZsAmt = Math.Round(syZsVal * price, 2, MidpointRounding.AwayFromZero); /*剩余赠电费*/

                object obj = new { PayVal = rdVal, PayAmt = payAmt, SyAmt = syAmt, ChrgVal = chrgVal, Price = price, SyZsVal = syZsVal, SyZsAmt = syZsAmt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取电表信息错误(GetYdPrepPayInMd_01OnInfo)：", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        // 充值
        public APIRst YdPrePayInMd_01OnChrg(decimal price,int payType,int co_id,int module_id,string moduleAddr,int fun_id,decimal payAmt,int isPay)
        {
            APIRst rst = new APIRst();
            if (price == 0)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "单价不能为零";
                return rst;
            }
            v4_pay_logVModel pay = new v4_pay_logVModel();
            pay.Co_id = co_id;
            pay.Module_id = module_id;           
            pay.ModuleAddr = moduleAddr;
            pay.Fun_id = fun_id;
            pay.PayAmt = payAmt;
            //pay.PayStartTime = CommFunc.ConvertDBNullToDateTime(Request["StartTime"]);
            //pay.PayEndTime = CommFunc.ConvertDBNullToDateTime(Request["EndTime"]);
            //pay.PayVal = (decimal)(((int)((pay.PayAmt / price) * 100)) / 100.00); /*不四舍五入*/ // Math.Round(pay.PayAmt / price, 2, MidpointRounding.AwayFromZero);
            pay.PayVal = Math.Round(pay.PayAmt / price, 2, MidpointRounding.AwayFromZero);
            pay.PayType = payType;
            pay.IsWrong = 0;
            pay.IsPay = isPay;
            if (pay.Co_id == 0 || pay.Module_id == 0 || pay.Fun_id ==0 || string.IsNullOrEmpty(pay.ModuleAddr))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "信息缺失";
                return rst;
            }
            if (pay.PayAmt == 0 || pay.PayVal == 0)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "金额不能为零";
                return rst;
            }
            if (pay.PayStartTime.Year == 1900 || pay.PayEndTime.Year == 1900 || pay.PayEndTime < pay.PayStartTime)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "日期错误";
                return rst;
            }
            if (pay.PayType != 1 && pay.PayType != 2 && pay.PayType != 3 && pay.PayType != 4)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "缴费类型错误";
                return rst;
            }

            try
            {
                string urlPay = "";
                if (pay.IsPay != 0)
                    urlPay = this.GetPayUrl(pay); //获取支付二维码                
                bll.AddV4_pay_log(pay);/*先记录数据库*/
                object obj = new { Log_id = pay.Log_id, Trade_no = pay.Trade_no, UrlPay = urlPay };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("支付错误信息(YdPostPayInMd)：", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        // 回调       
        public APIRst YdPrepPayInMdRecallPay(int pId,int pMid,string pTrade_no)
        {
            long log_id = CommFunc.ConvertDBNullToLong(pId);
            int module_id = CommFunc.ConvertDBNullToInt32(pMid);
            string out_trade_no = CommFunc.ConvertDBNullToString(pTrade_no);
            APIRst rst = new APIRst();
            string msg = "";
            string status = "";
            bool result = false;
            bool pass = true;
            bool upSb = true;
            bool upDb = false;
            DateTime dtNow = DateTime.Now;
            long cmd_log_id = 0;
            #region 检查支付结果并更新数据库
            System.Threading.Thread.Sleep(1000);/*一秒检查一次*/
            object row = "";
            object osObj = new { upSb = false, upDb = false, msg = "" };
            try
            {
                if (string.IsNullOrEmpty(out_trade_no))
                {
                    result = bll.YdPrePayInMdOnRecallPay(log_id, 1, "", out msg, out cmd_log_id);
                }
                else
                {
                    lock (ObjLock)
                    {/*每次只能单独运行，目的是防止重复更新数据库的充值信息（重要事情）*/
                        pass = new WxPayAPI.WxPay().GetResult(out_trade_no, out status);
                        if (pass == true)
                            result = bll.YdPrePayInMdOnRecallPay(log_id, 1, "", out msg, out cmd_log_id);
                    }
                }
                //
                if (result == true)
                {
                    string msg2 = "";
                    if (cmd_log_id != 0)//立刻发送命令到网关
                    {/*充值命令*/
                        upSb = false;
                        upSb = new YdToGw(user.Ledger, user.Uid).YdToGwAgain(cmd_log_id, out upDb, out msg2);
                    }
                    else
                    {
                        upDb = true;/*是否下发命令网关*/
                    }
                    //#region 发送命令到网关（这款用于河北工业大学的SB设备，特殊定制化）
                    //DataTable cmdPay = bll.GetGwPayData(module_id);
                    //foreach (DataRow dr in cmdPay.Rows)
                    //{
                    //    decimal sbRdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
                    //    decimal sbSyZsVal = CommFunc.ConvertDBNullToDecimal(dr["SyZsVal"]);

                    //    CommandVModel cmd = ModelHandler<CommandVModel>.FillModel(dr);
                    //    cmd.Fun_id = 0;
                    //    cmd.FunType = "PaySb1";
                    //    cmd.FunName = "充电";
                    //    cmd.DataValue = (sbRdVal - sbSyZsVal).ToString("f2");
                    //    cmd.Descr = "充电";
                    //    cmd.IsUI = true;
                    //    cmd.IsNDb = true;
                    //    new YdToGw(user.Ledger, user.Uid).SendCmd(cmd);
                    //    //
                    //    cmd = ModelHandler<CommandVModel>.FillModel(dr);
                    //    cmd.Fun_id = 0;
                    //    cmd.FunType = "PaySb2";
                    //    cmd.FunName = "增电";
                    //    cmd.DataValue = sbSyZsVal.ToString("f2");
                    //    cmd.Descr = "增电";
                    //    cmd.IsUI = true;
                    //    cmd.IsNDb = true;
                    //    new YdToGw(user.Ledger, user.Uid).SendCmd(cmd);
                    //}
                    //#endregion


                    //#region 发送短信
                    //bll.SendSms(log_id);
                    //#endregion
                    v4_pay_logVModel vpay = bll.GetV4_pay_log(log_id);

                    osObj = new { upSb = upSb, upDb = upDb, msg = msg2 };
                    decimal chrgVal = 0, useVal = 0, rdVal = 0, syZsVal = 0;
                    DataTable dtSource = bll.GetYdPostPayInMdOnList(module_id);
                    if (dtSource.Rows.Count > 0)
                    {
                        chrgVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["ChrgVal"]);
                        useVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["UseVal"]);
                        rdVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["RdVal"]);
                        syZsVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["SyZsVal"]);
                    }
                    row = new { ChrgVal = chrgVal.ToString("f2"), UseVal = useVal.ToString("f2"), RdVal = rdVal.ToString("f2"), SyVal = (rdVal - syZsVal).ToString("f2"), SyZsVal = syZsVal.ToString("f2") };

                    //判断是否拉闸
                    YDS6000.BLL.Exp.Syscont.ParameterBLL sysBll = new YDS6000.BLL.Exp.Syscont.ParameterBLL(user.Ledger, user.Uid);
                    var dt = sysBll.GetAlarmCfg();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        decimal odValue = CommFunc.ConvertDBNullToDecimal(dt.Rows[0]["CfValue"]);
                        int isClosed = CommFunc.ConvertDBNullToInt32(dt.Rows[0]["Rule"]);
                        if (isClosed == 1)
                        {
                            if (rdVal <= odValue)
                            {
                                if (!YdBatchControlOfSsr("Ssr", "1", module_id.ToString()))
                                {
                                    FileLog.WriteLog("拉闸失败(YdPrepPayInMdRecallPay)", "拉闸失败");
                                }
                            }
                            else
                            {
                                if (vpay.PayType == (int)PayType.pay)
                                {/*充值动作才能合闸*/
                                    if (!YdBatchControlOfSsr("Ssr", "0", module_id.ToString()))
                                    {
                                        FileLog.WriteLog("合闸失败(YdPrepPayInMdRecallPay)", "合闸失败");
                                    }
                                }
                            }
                        }
                    }
                }
                object obj = new { pass = pass, row = row, osObj = osObj };
                rst.rst = result;
                rst.data = obj;
                rst.err.msg = msg;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("预付费错误(YdPrepPayInMdRecallPay)", ex.Message + ex.StackTrace);
            }
            #endregion
            return rst;
        }
        // 批量下发命令
        private bool YdBatchControlOfSsr(string v0Fun,string dataValue,string pStr)
        {
            string msg = "";
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            V0Fun fun = V0Fun.E;
            if (Enum.TryParse<V0Fun>(v0Fun, out fun) == false)
                return false;
            //V0Fun fun = V0Fun.Ssr;
            try
            {
                foreach (string s in pStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    gw.YdToGwCmd(CommFunc.ConvertDBNullToInt32(s), fun, dataValue);
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FileLog.WriteLog("获取状态列表错误 (GetYdBatchControlOfList)", ex.Message + ex.StackTrace);
            }
            return false;
        }

        public APIRst YdPrepPayInMd_Reset(int module_id,int fun_id)
        {
            APIRst rst = new APIRst();
            try
            {
                int cnt = bll.YdPrepPayInMd_Reset(module_id, fun_id);
                decimal chrgVal = 0, useVal = 0, rdVal = 0, syZsVal = 0;
                DataTable dtSource = bll.GetYdPostPayInMdOnList(module_id);
                if (dtSource.Rows.Count > 0)
                {
                    chrgVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["ChrgVal"]);
                    useVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["UseVal"]);
                    rdVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["RdVal"]);
                    syZsVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["SyZsVal"]);
                }
                object obj = new { ChrgVal = chrgVal.ToString("f2"), UseVal = useVal.ToString("f2"), RdVal = rdVal.ToString("f2"), SyVal = (rdVal - syZsVal).ToString("f2"), SyZsVal = syZsVal.ToString("f2") };

                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取电表信息错误(GetYdPrepPayInMd_01OnInfo)：", ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 汇出Excel（充值缴费--打印小票）
        /// </summary>
        /// <returns></returns> 
        public APIRst GetYdPrePayInMdExcel(int Co_id,string PayAmt,int IsPay)
        {
            long Room_id = CommFunc.ConvertDBNullToLong(Co_id);//房间id
            PayAmt = CommFunc.ConvertDBNullToDecimal(PayAmt).ToString("f2");//缴费金额
            //int IsPay = CommFunc.ConvertDBNullToInt32(Request["IsPay"]);//缴费方式：=0现金充值，=10微信充值
            APIRst rst = new APIRst();            
            try
            {
                DataTable dtSource = bll.GetPayList(Room_id, PayAmt, IsPay);
                //string path = GetYdPayLogOnExport(dtSource);
                //return Json(new { rst = true, msg = "", data = path }, JsonRequestBehavior.DenyGet);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                               IsPayS = CommFunc.ConvertDBNullToString(s1["IsPayS"]),
                               payTypeS = CommFunc.ConvertDBNullToString(s1["payTypeS"]),
                               PayVal = CommFunc.ConvertDBNullToDecimal(s1["PayVal"]).ToString("f2"),
                               PayAmt = CommFunc.ConvertDBNullToDecimal(s1["PayAmt"]).ToString("f2"),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]).ToString("f3"),
                               strAmt = CommFunc.ConvertDBNullToString(s1["strAmt"]),
                               Create_by = CommFunc.ConvertDBNullToString(s1["Create_by"]),
                               Create_dt = CommFunc.ConvertDBNullToString(s1["Create_dt"]),
                           };
                rst.data = res1.ToList();
                //return Json(new { rst = true, msg = "", data = new { total = total, rows = res1.ToList() } }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdPrePayInMdExcel:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        private string GetYdPayLogOnExport(DataTable dtSource)
        {
            string fn = "/XTemp/缴费明细单.xls";
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/XTemp");
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = System.Web.Hosting.HostingEnvironment.MapPath(fn);
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            if (dtSource == null || dtSource.Rows.Count == 0)
                throw new Exception("没有数据");
            Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
            columnCellStyle0 = new Excel.ExcelCellStyle()
            {
                DataFormart = "0.00",
                HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
            };
            Excel.ExcelCellStyle columnCellStyle1 = new Excel.ExcelCellStyle();
            columnCellStyle1 = new Excel.ExcelCellStyle()
            {
                DataFormart = "yyyy-MM-dd HH:mm:ss",
            };
            Excel.ExcelOparete excel = new Excel.ExcelOparete("缴费明细单");
            var cellStyle = new ExcelCellStyle();
            cellStyle.HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            cellStyle.FontSize = 12;
            excel.SetObjectValue("缴费明细单", 0, 0, 1, cellStyle);

            excel.SetObjectValue2("建筑名称", 1, 1, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["CoStrcName"], 1, 1, 1, 1);
            excel.SetObjectValue2("房间名称", 2, 2, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["CoName"], 2, 2, 1, 1);
            excel.SetObjectValue2("倍率", 3, 3, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["Multiply"], 3, 3, 1, 1);
            excel.SetObjectValue2("单价(元/kWh)", 4, 4, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["Price"], 4, 4, 1, 1);
            excel.SetObjectValue2("充值类型", 5, 5, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["IsPayS"], 5, 5, 1, 1);
            excel.SetObjectValue2("充值方式", 6, 6, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["payTypeS"], 6, 6, 1, 1);
            excel.SetObjectValue2("充值电量(kWh)", 7, 7, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["PayVal"], 7, 7, 1, 1);
            excel.SetObjectValue2("充值金额(元)", 8, 8, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["PayAmt"], 8, 8, 1, 1);
            excel.SetObjectValue2("金额(大写)", 9, 9, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["strAmt"], 9, 9, 1, 1);
            excel.SetObjectValue2("操作者", 10, 10, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["Create_by"], 10, 10, 1, 1);
            excel.SetObjectValue2("操作时间", 11, 11, 0, 0);
            excel.SetObjectValue2(dtSource.Rows[0]["Create_dt"], 11, 11, 1, 1);

            excel.SaveExcelByFullFileName(filename);
            return fn;
        }
        /// <summary>
        /// 充值缴费--打印小票
        /// </summary>
        /// <param name="Log_id">支付记录id号</param>
        /// <returns></returns> 
        public APIRst GetYdPayBill(long Log_id)
        {      
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdPayBill(Log_id);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                               IsPayS = CommFunc.ConvertDBNullToString(s1["IsPayS"]),
                               payTypeS = CommFunc.ConvertDBNullToString(s1["payTypeS"]),
                               PayVal = CommFunc.ConvertDBNullToDecimal(s1["PayVal"]).ToString("f2"),
                               PayAmt = CommFunc.ConvertDBNullToDecimal(s1["PayAmt"]).ToString("f2"),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]).ToString("f3"),
                               strAmt = CommFunc.ConvertDBNullToString(s1["strAmt"]),
                               Create_by = CommFunc.ConvertDBNullToString(s1["Create_by"]),
                               Create_dt = CommFunc.ConvertDBNullToString(s1["Create_dt"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdPayBill:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}
