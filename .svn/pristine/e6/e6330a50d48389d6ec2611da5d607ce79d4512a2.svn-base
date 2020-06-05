using System;
using System.Data;
using System.Collections.Generic;
using YDS6000.Models;

namespace DataProcess
{
    public class ICEFactory : DataProcess.communicationDisp_
    {
        public override bool SendCollectVal(string content, Ice.Current current)
        {
            try
            {
                List<ApiVar> apiVal = JsonHelper.Deserialize<List<ApiVar>>(content);
                if (apiVal == null)
                    return false;
                foreach (ApiVar val in apiVal)
                {
                    try
                    {
                        //DataProcess.YdProcess.Helper.SaveCollectCache(val.tagName, val.value, val.time, 0);
                    }
                    catch (Exception ex)
                    {
                        FileLog.Error("保存采集缓存错误(SendCollectVal):" + ex.Message + ex.StackTrace);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                FileLog.Error("获取采集缓存错误(SendCollectVal):" + ex.Message + ex.StackTrace);
            }
            return false;
        }
        public override string GetCollectVal(string strKey, Ice.Current current)
        {
            try
            {
                CollectVModel collect = null;
                if (NCSys.Result.TryGetValue(strKey, out collect) == false)
                    return "";
                if (collect == null) return "";
                return JsonHelper.Serialize(collect.RstVar);
            }
            catch (Exception ex)
            {
                FileLog.Error("获取采集缓存错误(GetCollectVal):" + ex.Message + ex.StackTrace);
            }
            return "";
        }

        public override string send(string content, Ice.Current current)
        {
            //System.Windows.Forms.MessageBox.Show(content);
            return "";
        }
        public override string receive(string command, Ice.Current current)
        {
            //System.Windows.Forms.MessageBox.Show(command);
            return "";
        }
    }

}