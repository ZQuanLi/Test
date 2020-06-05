using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.IFSMgr.Opertion.Collect
{
    public class CollectHelper
    {
        private CacheUser user = null;
        public CollectHelper()
        {
            user = WebConfig.GetSession();
        }

        public APIRst CollectData(string dataValue)
        {
            APIRst rst = new APIRst() { rst = true };
            try
            {
                Pack pack = JsonHelper.Deserialize<Pack>(dataValue);
                if (pack == null || pack.Data == null || pack.Data.Count == 0)
                {
                    rst = new APIRst() { rst = false, data = "", err = new APIErr() { code = -2, msg = "没有值" } };
                    FileLog.WriteLog("接收到数据格式不正确:" + dataValue);
                    return rst;
                }
                string key = pack.DevAddr + "." + pack.DisNo + "." + pack.EleNo + ".";
                SortedList<string, Data> e = new SortedList<string, Data>();
                SortedList<string, Data> e4 = new SortedList<string, Data>();
                List<Data> ld = new List<Data>();
                foreach (Data dd in pack.Data)
                {
                    if (dd.FunType.Equals("E") || dd.FunType.Equals("E1") || dd.FunType.Equals("E2") || dd.FunType.Equals("E3") || dd.FunType.Equals("E4"))
                    {
                        string funType = dd.FunType;
                        if (dd.FunType.Equals("E"))
                            funType = "E0";
                        if (dd.Type == 0)
                            e.Add(funType, dd);
                        else
                            e4.Add(funType, dd);

                    }
                    else
                    {
                        ld.Add(dd);
                    }
                }
                if (this.AddType(ref e, ref ld, 0) == false)
                {
                    FileLog.WriteLog("接收到非冻结数据总电能不等于尖峰平谷:" + dataValue);
                }
                if (this.AddType(ref e4, ref ld, 1) == false)
                {
                    FileLog.WriteLog("接收到冻结数据总电能不等于尖峰平谷:" + dataValue);
                }
                //if (e.Count > 0)
                //{
                //    FileLog.WriteLog("接收到实时尖峰平谷数据:" + dataValue);
                //    FileLog.WriteLog("实时压栈数据:" + JsonHelper.Serialize(ld));
                //}
                //if (e4.Count > 0)
                //{
                //    FileLog.WriteLog("接收到冻结尖峰平谷数据:" + dataValue);
                //    FileLog.WriteLog("冻结压栈数据:" + JsonHelper.Serialize(ld));
                //}
                foreach (Data dd in ld)
                {
                    if (dd.FunType.Equals("E1") || dd.FunType.Equals("E2") || dd.FunType.Equals("E3") ||
                        dd.FunType.Equals("E5") || dd.FunType.Equals("E6") || dd.FunType.Equals("E6") || dd.FunType.Equals("E8") ||
                        dd.FunType.Equals("E9") || dd.FunType.Equals("E10") || dd.FunType.Equals("E11") || dd.FunType.Equals("E12"))
                        continue;
                    CommandVModel cmd = new CommandVModel();
                    cmd.Ledger = WebConfig.Ledger;
                    cmd.TransferType = 4;
                    cmd.FunType = dd.FunType;

                    cmd.LpszDbVarName = key + dd.FunType;
                    cmd.DataValue = dd.DataValue;
                    cmd.CollectTime = dd.Time;

                    ListenVModel vm = new ListenVModel() { cfun = ListenCFun.collect.ToString(), content = JsonHelper.Serialize(cmd) };
                    string msg = "";
                    bool rr = CacheMgr.SendCollectVal(vm, out msg);
                    if ((dd.FunType.Equals("E") || dd.FunType.Equals("E4")) && rr == true)
                    {
                        FileLog.WriteLog("成功发送数据:" + vm.content);
                    }
                    //if (rr==true)
                    //    FileLog.WriteLog("成功发送数据:" + vm.content);
                }
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取采集信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private bool AddType(ref SortedList<string, Data> source, ref List<Data> ld, int type)
        {
            if (source.Count == 0) return true;
            Data de = new Data();
            decimal useValE0 = 0, useValEa = 0;
            foreach (var dd in source)
            {
                de.FunType = type == 0 ? "E" : "E4";
                de.Type = 0;
                de.Time = dd.Value.Time;
                if (!string.IsNullOrEmpty(de.DataValue))
                    de.DataValue = de.DataValue + ",";
                if (dd.Key.Equals("E0"))
                {
                    de.DataValue = "E0:{0}";
                    useValE0 = CommFunc.ConvertDBNullToDecimal(dd.Value.DataValue);
                }
                else
                {
                    useValEa = useValEa + CommFunc.ConvertDBNullToDecimal(dd.Value.DataValue);
                    de.DataValue = de.DataValue + dd.Key + ":" + dd.Value.DataValue;
                }
            }
            bool rst = true;
            if (Math.Abs(useValE0 - useValEa) > (decimal)0.01)
            {
                FileLog.WriteLog((type == 0 ? "非" : "") + "冻结数据E0不等于4个费率之和:" + de.DataValue);
                rst = false;
            }
            else
            {
                de.DataValue = string.Format(de.DataValue, useValEa.ToString());
                ld.Add(de);
            }
            return rst;
        }

        public class Pack
        {
            /// <summary>
            /// 集中器的行政区划
            /// </summary>
            public string DisNo { get; set; }
            /// <summary>
            /// 集中器的设备地址
            /// </summary>
            public string DevAddr { get; set; }
            /// <summary>
            /// 挂载在集中器下的电表编号
            /// </summary>
            public string EleNo { get; set; }
            /// <summary>
            /// 数据数组
            /// </summary>
            public List<Data> Data { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 测点编码
            /// </summary>
            public string FunType { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public string DataValue { get; set; }
            /// <summary>
            /// 采集时间
            /// </summary>
            public DateTime Time { get; set; }
            /// <summary>
            /// 测点类型
            /// </summary>
            public int Type { get; set; }
        }
    }
}