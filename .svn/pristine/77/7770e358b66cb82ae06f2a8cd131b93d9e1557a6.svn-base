using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace DataProcess.YdProcess
{
    internal class YdPayEst
    {
        public static void UpdatePayEst(int ledger)
        {
            YDS6000.BLL.DataProcess.PayEstBLL bll = new YDS6000.BLL.DataProcess.PayEstBLL(ledger, Config.Uid);
            //
            DateTime dtTime = DateTime.Now;
            DataTable dtRateInfo = bll.GetRateInfo();
            DataTable dtSource = bll.GetPayEstInfo();
            dtSource.Columns.Add("UseVal", typeof(System.Decimal));/**/
            dtSource.Columns.Add("UseAmt", typeof(System.Decimal));/*物业费金额*/
            StringBuilder strFilter = new StringBuilder();
            int point = 2;
            #region 计算物业费
            foreach (DataRow dr in dtSource.Rows)
            {
                int attrib = CommFunc.ConvertDBNullToInt32(dr["Attrib"]);
                int isDefine = CommFunc.ConvertDBNullToInt32(dr["IsDefine"]);
                int bank = CommFunc.ConvertDBNullToInt32(dr["Bank"]);
                int isCtl = CommFunc.ConvertDBNullToInt32(dr["IsCtl"]);
                decimal area = CommFunc.ConvertDBNullToDecimal(dr["Area"]);
                int rate_id = CommFunc.ConvertDBNullToInt32(dr["Rate_id"]);
                int rule = CommFunc.ConvertDBNullToInt32(dr["Rule"]);
                string unit = CommFunc.ConvertDBNullToString(dr["Unit"]);
                decimal unitBase = CommFunc.ConvertDBNullToDecimal(dr["UnitBase"]);
                unitBase = unitBase == 0 ? 1 : unitBase;
                if (rate_id == 0) continue;
                ////////
                ////////
                decimal useAmt = 0;
                #region 计算金额
                if (isCtl == 0)
                {/*使用系统计算*/
                    foreach (DataRow drRate in dtRateInfo.Select("Rate_id=" + rate_id))
                    {
                        string pStart = CommFunc.ConvertDBNullToString(drRate["PStart"]);
                        string pEnd = CommFunc.ConvertDBNullToString(drRate["PEnd"]);
                        decimal price = CommFunc.ConvertDBNullToDecimal(drRate["Price"]);
                        /////////////////////////////////////////////////////////////////////
                        bool isBindGo = false;

                        if (RateUnit.Area.ToString().Equals(unit))
                        {
                            #region 按平方计算
                            if (rule == 0)
                            {
                                isBindGo = true;
                            }
                            else if (rule == 1)
                            { /*按平方的日期计算*/
                                DateTime dtFm = CommFunc.ConvertDBNullToDateTime(pStart);
                                DateTime dtTo = CommFunc.ConvertDBNullToDateTime(pEnd);
                                if (dtTime > dtFm && dtTime <= dtTo)
                                    isBindGo = true;
                            }
                            else if (rule == 2)
                            {
                                decimal nStart = CommFunc.ConvertDBNullToDecimal(pStart);
                                decimal nEnd = CommFunc.ConvertDBNullToDecimal(pEnd);
                                if (area > nStart && area <= nEnd)
                                    isBindGo = true;
                            }
                            if (isBindGo == true)
                                useAmt = Math.Round((area / unitBase) * price, point, MidpointRounding.AwayFromZero);
                            #endregion
                        }
                        else if (RateUnit.Bank.ToString().Equals(unit))
                        { /*按户数计算*/
                            #region 按户数计算
                            if (rule == 0)
                            {
                                isBindGo = true;
                            }
                            else if (rule == 1)
                            { /*按平方的日期计算*/
                                DateTime dtFm = CommFunc.ConvertDBNullToDateTime(pStart);
                                DateTime dtTo = CommFunc.ConvertDBNullToDateTime(pEnd);
                                if (dtTime > dtFm && dtTime <= dtTo)
                                    isBindGo = true;
                            }
                            else if (rule == 2)
                            {
                                decimal nStart = CommFunc.ConvertDBNullToDecimal(pStart);
                                decimal nEnd = CommFunc.ConvertDBNullToDecimal(pEnd);
                                if (bank > nStart && bank <= nEnd)
                                    isBindGo = true;
                            }
                            if (isBindGo == true)
                                useAmt = Math.Round((bank / unitBase) * price, point, MidpointRounding.AwayFromZero);
                            #endregion
                        }
                        if (isBindGo == true) break;
                    }
                }
                else
                {

                }
                #endregion
                dr["UseAmt"] = useAmt;                
                ///////
            }
            #endregion

            bll.UpdatePayEst(dtSource);/*更新*/
        }

        public static void UpV4_pay_charg(int ledger)
        {
            YDS6000.BLL.DataProcess.PayEstBLL bll = new YDS6000.BLL.DataProcess.PayEstBLL(ledger, Config.Uid);
            DataTable dtSource = bll.GetV4_pay_charg();
            foreach (DataRow dr in dtSource.Rows)
            {
                long log_id = CommFunc.ConvertDBNullToLong(dr["Log_id"]);
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal price = CommFunc.ConvertDBNullToDecimal(dr["Price"]);
                decimal chargVal = CommFunc.ConvertDBNullToDecimal(dr["ChargVal"]);
                decimal useAmt = Math.Round((lastVal - firstVal) * price, 2, MidpointRounding.AwayFromZero) + chargVal;
                DateTime firstTime = CommFunc.ConvertDBNullToDateTime(dr["FirstTime"]);
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                if (DateTime.Now < lastTime) continue;
                bll.UpV4_pay_charg(log_id, co_id, useAmt);
            }
        }
    }
}
