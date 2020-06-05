using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YDS6000.Models;
using YADA.Utilities;

namespace DataProcess.YdDrive.Collection
{
    internal class Drive
    {
        /// <summary>
        /// 抄表
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool ARM(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";            
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //电量
                decimal curVal = Operation.IReceive.GetModuleCurrValue(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            //try
            //{
            //    Operation.ISetInfo.SetDateTime(command.ModuleAddr, out send, out result);
            //}
            //catch (Exception ex)
            //{
            //    FileLog.WriteLog("表ID:" + command.Module_id + "表地址:" + command.ModuleAddr + "表类型:" + command.ModuleType + "校时发生错误:" + ex.Message + ex.StackTrace);
            //}
            return rst;
        }

        /// <summary>
        /// Ia
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetIa(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetIa(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Ib
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetIb(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetIb(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Ib
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetIc(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetIc(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Ua
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetUa(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetUa(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Ub
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetUb(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetUb(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Uc
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetUc(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetUc(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }



        /// <summary>
        /// Psum
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPsum(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPsum(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Pa
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPa(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPa(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Pb
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPb(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPb(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Pc
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPc(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPc(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }


        /// <summary>
        /// Qsum
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetQsum(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetQsum(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Qa
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetQa(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetQa(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Qb
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetQb(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetQb(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// Qc
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetQc(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetQc(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }


        /// <summary>
        /// Pfav
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPfav(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPfav(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// PFa
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPFa(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPFa(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// PFb
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPFb(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPFb(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        /// <summary>
        /// PFc
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetPFc(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                //点流
                decimal curVal = Operation.IReceive.GetPFc(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }


        internal static bool SetDateTime(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                //电量
                Operation.ISetInfo.SetDateTime(command.ModuleAddr, out send, out result);
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 当前电表剩余电量
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool RdVal(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            try
            {
                //剩余电量
                decimal rdVal = Operation.IReceive.GetModuleRdValue(command.ModuleAddr, out send, out result);
                value = rdVal.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 拉合闸状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool Ssr(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                //电量
                string moduleValue = string.Empty;
                int mno = (int)(CommFunc.ConvertDBNullToLong(command.ModuleAddr) - CommFunc.ConvertDBNullToLong(command.MeterAddr));
                mno = (mno > 0 && mno <= 2) ? mno : 0;
                int ssr = Operation.IReceive.GetModuleCurrIsSSR(command.ModuleAddr, out send, out result, out moduleValue, mno);
                value = ssr.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 合闸
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool Ssr0(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            command.FunName = "合闸";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                //电量
                Operation.IEvent.SetSwitchIn(command.ModuleAddr, out send, out result);
                FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 合闸成功");
                SysPro pro = null;
                NCSys.Pro.TryGetValue(command.Ledger, out pro);
                if (pro != null && pro.Project.Equals("YdXip"))
                {
                    if (command.ModuleType.Equals("DDS3366L-1"))
                    {/*空调电表，合闸后，再开启“待机检测”功能*/
                        System.Threading.Thread.Sleep(1000);/*等1秒*/
                        Operation.ISetInfo.SetStandby(command.ModuleAddr, true, out send, out result);
                        FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 待机检测开启成功");
                    }
                }
                value = "0";                
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 合闸失败:" + msg);
            }
            return false;
        }

        /// <summary>
        /// 拉闸
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool Ssr1(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            command.FunName = "拉闸";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            try
            {
                SysPro pro = null;
                NCSys.Pro.TryGetValue(command.Ledger, out pro);
                if (pro != null && pro.Project.Equals("YdXip"))
                { /*西安培华 照名插座用电检测电流，NND设备有问题让软件来解决*/
                    if (command.ModuleType.Equals("DDS3366D-2PEB"))
                    {/*照明电表拉闸后检测是否有电流，若有再次拉闸*/
                        Operation.IEvent.SetSwitchOut(command.ModuleAddr, out send, out result);
                        FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 拉闸成功");
                        int again = 0;
                        byte[] iaSend = null, iaRst = null;
                        while (++again < 3)
                        {
                            System.Threading.Thread.Sleep(1000 * 10);/*等10秒*/
                            decimal ia = Operation.IReceive.GetIa(command.ModuleAddr, out iaSend, out iaRst);
                            if (ia != 0) /*再次拉闸*/
                            {
                                Operation.IEvent.SetSwitchOut(command.ModuleAddr, out send, out result);
                                FileLog.WriteSysLog("电表:" + command.ModuleAddr + "第"+ again + "次拉闸成功，原因等待10秒还有电流");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (command.ModuleType.Equals("DDS3366L-1"))
                    {/*空调电表，拉闸前要关闭“待机检测”功能*/
                        Operation.ISetInfo.SetStandby(command.ModuleAddr, false, out send, out result);
                        FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 待机检关闭启成功");
                        System.Threading.Thread.Sleep(1000);/*等1秒*/
                        Operation.IEvent.SetSwitchOut(command.ModuleAddr, out send, out result);
                        FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 拉闸成功");
                    }
                    else
                    {
                        Operation.IEvent.SetSwitchOut(command.ModuleAddr, out send, out result);
                        FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 拉闸成功");
                    }
                }
                else
                {
                    Operation.IEvent.SetSwitchOut(command.ModuleAddr, out send, out result);
                    FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 拉闸成功");
                }
                value = "1";               
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.WriteSysLog("电表:" + command.ModuleAddr + " 拉闸失败:" + msg);
            }
            return false;
        }

        /// <summary>
        /// 电表充值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool Pay(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                //电量
                string moduleValue = string.Empty;
                decimal ele = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                Operation.ISecurity.SetPayAccount(command.ModuleAddr, ele, DateTime.Now, out send, out result);
                value = ele.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 电表退费
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool Refund(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                //电量
                string moduleValue = string.Empty;
                decimal ele = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                Operation.ISecurity.SetBackAccount(command.ModuleAddr, ele, DateTime.Now, out send, out result);
                value = ele.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }
        
        #region 2018.07.02
        /// <summary>
        /// 电表充电
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool PaySb1(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                decimal ele = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                Operation.ISetInfo.SetPayVal(command.ModuleAddr, ele, out send, out result);
                value = ele.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }
        /// <summary>
        /// 电表赠电
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool PaySb2(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                decimal ele = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                Operation.ISetInfo.SetBaseVal(command.ModuleAddr, ele, out send, out result);
                value = ele.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 电表保电
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetPaul(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int paul = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    Operation.IEvent.SetPaul(command.ModuleAddr, paul, out send, out result);
                    value = paul.ToString();
                }
                else
                {
                    int val = Operation.IReceive.GetPaul(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 有功功率负荷限值(kW)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetNrp(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode,out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetNrp(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetNrp(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 恶性负载判定有功功率增量(kW)(瞬时额定功率)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetMcNrp(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetMc_Nrp(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetMc_Nrp(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }


        /// <summary>
        /// 恶性负载判定无功功率(kW) 设置恶性负载判定无功功率因数增量门限
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetWpf(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetWpf(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetWpf(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 违规用电判定功能的功率下限(kW)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetUep(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetUep(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetUep(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 一天内违规断电次数限值(0~255)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetMiss(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    Operation.ISetInfo.SetMiss(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else 
                {
                    int val = Operation.IReceive.GetMiss(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 违规断电后重启时间(S)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetRest(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    Operation.ISetInfo.SetRest(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    int val = Operation.IReceive.GetRest(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 单次功率增量限值(kW)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetSaf(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetSaf(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetSaf(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 过流门限值(A)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetAvlm(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetAvlm(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetAvlm(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 启用移相器监测功率下限(kW)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetPhase(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetPhaseVal(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else 
                {
                    decimal val = Operation.IReceive.GetPhaseVal(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 设置阻行负载(kW)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetResist(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetResist(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetResist(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }
        /// <summary>
        /// 移相器(=1启用)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetIsPhase(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    Operation.ISetInfo.SetPhase(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    int val = Operation.IReceive.GetPhase(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 继电器参数(=0电平式A路继电器设置;=1电平式B路继电器设置;=127脉冲式设置)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetIsRelay(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {

                int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                Operation.ISetInfo.SetRelay(command.ModuleAddr, val, out send, out result);
                value = val.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.WriteLog("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        /// <summary>
        /// 待机检测=0不检测
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetStandby(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                bool val = CommFunc.ConvertDBNullToInt32(command.DataValue) == 0 ? false : true;
                Operation.ISetInfo.SetStandby(command.ModuleAddr, val, out send, out result);
                value = val.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.WriteLog("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        
        /// <summary>
        /// 继电器参数(=0电平式A路继电器设置;=1电平式B路继电器设置;=127脉冲式设置)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetTiVal(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    string val = CommFunc.ConvertDBNullToString(command.DataValue);
                    Operation.ISetInfo.SetTiVal(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    string val = Operation.IReceive.GetTiVal(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.Info("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        
        /// <summary>
        /// 漏电流
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetMeterLeak(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                decimal curVal = 0;
                //漏电流
                if (command.FunType.Equals(V0Fun.Leak1.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak1(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak2.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak2(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak3.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak3(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak4.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak4(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak5.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak5(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak6.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak6(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak7.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak7(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Leak8.ToString()))
                    curVal = Operation.IReceive.GetMeterLeak8(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 温度
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetMeterTemp(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                decimal curVal = 0;
                //漏电流
                if (command.FunType.Equals(V0Fun.Temp1.ToString()))
                    curVal = Operation.IReceive.GetMeterTemperature1(command.ModuleAddr, out send, out result);
                else if (command.FunType.Equals(V0Fun.Temp2.ToString()))
                    curVal = Operation.IReceive.GetMeterTemperature2(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }
        
        /// <summary>
        /// 漏电设备告警
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns>BIT0-7对应漏电流1-8;BIT8-11对应温度1-4;BIT12-15对应开入1-4;1为警告，0为正常</returns>
        internal static bool GetLeakAlarm(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                string curVal = "";
                //漏电流
                curVal = Operation.IReceive.GetLeakAlarm(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 设置电流阀值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetMaxLeak(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    if (command.FunType.ToLower().Equals(V0Fun.MaxLeak1.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak1(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak2.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak2(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak3.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak3(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak4.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak4(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak5.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak5(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak6.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak6(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak7.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak7(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak8.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxLeak8(command.ModuleAddr, val, out send, out result);
                    else
                        throw new Exception("更新电流阀值类型错误");
                    value = val.ToString();
                }
                else
                {
                    decimal val = 0;
                    if (command.FunType.ToLower().Equals(V0Fun.MaxLeak1.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak1(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak2.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak2(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak3.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak3(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak4.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak4(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak5.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak5(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak6.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak6(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak7.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak7(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxLeak8.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxLeak8(command.ModuleAddr, out send, out result);
                    else
                        throw new Exception("获取电流阀值类型错误");
                    value = val.ToString();
                }
                return true;

            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.Info("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 设置电流使能
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetStatusLeak(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    if (command.FunType.ToLower().Equals(V0Fun.StatusLeak1.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak1(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak2.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak2(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak3.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak3(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak4.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak4(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak5.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak5(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak6.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak6(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak7.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak7(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak8.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusLeak8(command.ModuleAddr, val, out send, out result);
                    else
                        throw new Exception("更新电流使能类型错误");
                    value = val.ToString();
                }
                else
                {
                    decimal val = 0;
                    if (command.FunType.ToLower().Equals(V0Fun.StatusLeak1.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak1(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak2.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak2(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak3.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak3(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak4.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak4(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak5.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak5(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak6.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak6(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak7.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak7(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusLeak8.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusLeak8(command.ModuleAddr, out send, out result);
                    else
                        throw new Exception("获取电流使能类型错误");
                    value = val.ToString();
                }
                return true;

            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.Info("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 设置温度阀值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetMaxTemp(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    if (command.FunType.ToLower().Equals(V0Fun.MaxTemp1.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxTemp1(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxTemp2.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxTemp2(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxTemp3.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxTemp3(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxTemp4.ToString().ToLower()))
                        Operation.ISetInfo.SetMaxTemp4(command.ModuleAddr, val, out send, out result);
                    else
                        throw new Exception("更新温度阀值类型错误");
                    value = val.ToString();
                }
                else
                {
                    decimal val = 0;
                    if (command.FunType.ToLower().Equals(V0Fun.MaxTemp1.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxTemp1(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxTemp2.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxTemp2(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxTemp3.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxTemp3(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.MaxTemp4.ToString().ToLower()))
                        val = Operation.IReceive.GetMaxTemp4(command.ModuleAddr, out send, out result);
                    else
                        throw new Exception("获取温度阀值类型错误");
                    value = val.ToString();
                }
                return true;

            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.Info("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 设置温度使能
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool SetStatusTemp(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                    if (command.FunType.ToLower().Equals(V0Fun.StatusTemp1.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusTemp1(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusTemp1.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusTemp2(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusTemp1.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusTemp3(command.ModuleAddr, val, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusTemp1.ToString().ToLower()))
                        Operation.ISetInfo.SetStatusTemp4(command.ModuleAddr, val, out send, out result);
                    else
                        throw new Exception("更新温度使能类型错误");
                    value = val.ToString();
                }
                else
                {
                    int val = 0;
                    if (command.FunType.ToLower().Equals(V0Fun.StatusTemp1.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusTemp1(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusTemp2.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusTemp2(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusTemp3.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusTemp3(command.ModuleAddr, out send, out result);
                    else if (command.FunType.ToLower().Equals(V0Fun.StatusTemp4.ToString().ToLower()))
                        val = Operation.IReceive.GetStatusTemp4(command.ModuleAddr, out send, out result);
                    else
                        throw new Exception("获取电流使能类型错误");
                    value = val.ToString();
                }
                return true;

            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                FileLog.Info("地址:" + command.ModuleAddr + "执行" + command.FunName + "错误:" + ex.Message + ex.StackTrace);
            }
            return false;
        }

        #region 2019.03.19 设置
        /// <summary>
        /// 获取报警电量 1 限值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Network"></param>
        /// <param name="send"></param>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <param name="errCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static bool GetAlarm1(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            bool rst = false;
            try
            {
                decimal curVal = Operation.IReceive.GetAlarm1(command.ModuleAddr, out send, out result);
                value = curVal.ToString();
                rst = true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
                rst = false;
            }
            return rst;
        }
        internal static bool SetAlarm1(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out string value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = "";
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                if (!string.IsNullOrEmpty(command.DataValue))
                {
                    decimal val = CommFunc.ConvertDBNullToDecimal(command.DataValue);
                    Operation.ISetInfo.SetSaf(command.ModuleAddr, val, out send, out result);
                    value = val.ToString();
                }
                else
                {
                    decimal val = Operation.IReceive.GetSaf(command.ModuleAddr, out send, out result);
                    value = val.ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }

        #endregion

        internal static bool GetEventWpf(CommandVModel command, NetworkResource Network, out byte[] send, out byte[] result, out Dictionary<string,string> value, out AlarmType errCode, out string msg)
        {
            send = null; result = null;
            errCode = AlarmType.Sue; msg = "";
            value = null;
            Operation Operation = Drive.GetStatus(command, Network, out errCode, out msg);
            if (Operation == null)
                return false;
            //当前电表读数
            try
            {
                int val = CommFunc.ConvertDBNullToInt32(command.DataValue);
                value = Operation.IReceive.GetMalignant(command.ModuleAddr, out send, out result);
                return true;
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Sb;
                msg = ex.Message;
            }
            return false;
        }
        private static YADA.Utilities.Operation GetStatus(CommandVModel handle, NetworkResource Network, out AlarmType errCode, out string msg)
        {
            errCode = AlarmType.Sue;
            msg = "";
            Operation operation = null;
            if (Network.IsOpen == false)
            {
                errCode = AlarmType.Err_Tx;
                msg = "端口:" + Network.NetworkName + "连接失败";
                return operation;
            }
            try
            {
                operation = Drive.GetOperation(handle, Network);
            }
            catch (Exception ex)
            {
                errCode = AlarmType.Err_Pz;
                msg = ex.Message;
            }
            return operation;
        }

        private static YADA.Utilities.Operation GetOperation(CommandVModel handle, NetworkResource Network)
        {
            //
            YADA.Utilities.IPortsItem portsItem = new YADA.Utilities.IPortsItem();
            //
            portsItem.PortName = "";
            if (handle.TransferType == 0)
            {
                if (string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(handle.ComPort)))
                    throw new Exception("电表通讯COM端口未设置");
                portsItem.PortName = CommFunc.ConvertDBNullToString(handle.ComPort).Trim();
            }          
            else 
            {
                if (string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(handle.Ip)))
                    throw new Exception("电表通讯IP地址未设置");
                if (CommFunc.ConvertDBNullToInt32(handle.TcpPort) == 0)
                    throw new Exception("电表通讯IP端口未设置");
                if (handle.TransferType == 3)
                    portsItem.PortName = handle.HandledBY;
                else
                    portsItem.PortName = CommFunc.ConvertDBNullToString(handle.Ip).Trim() + ":" + CommFunc.ConvertDBNullToInt32(handle.TcpPort).ToString().Trim();
            }
            if (!portsItem.PortName.Trim().Equals(Network.NetworkName.Trim()))
                throw new Exception("电表通讯端口和资源设置不一致");

            portsItem.BaudRate = handle.Baud;
            portsItem.DataBits = handle.DataBit;
            portsItem.Parity = (System.IO.Ports.Parity)Enum.ToObject(typeof(System.IO.Ports.Parity), handle.Parity);
            portsItem.StopBits = (System.IO.Ports.StopBits)Enum.ToObject(typeof(System.IO.Ports.StopBits), handle.StopBit);
            //默认信息
            portsItem.ReadTimeout = handle.TimeOut;
            portsItem.WriteTimeout = handle.TimeOut;
            portsItem.Encoding = Encoding.ASCII;
            portsItem.Handshake = System.IO.Ports.Handshake.None;
            portsItem.RtsEnable = true;
            portsItem.ModuleType = handle.ModuleType;
            portsItem.Protocol = CommFunc.ConvertDBNullToString(handle.Protocol);
            Operation Operation = new YADA.Utilities.Operation();
            Operation.ModuleModel = handle.ModuleType;
            Operation.Network = Network;
            Operation.mPwd = CommFunc.ConvertDBNullToString(handle.ModulePwd).Trim();
            Operation.mUid = CommFunc.ConvertDBNullToString(handle.ModuleUid).Trim();
            Operation.ModifyInstance(portsItem);
            return Operation;
        }
    }
}
