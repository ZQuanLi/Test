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
                   
                }
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public override string GetCollectVal(string strKey, Ice.Current current)
        {
            return "";
        }

        public override string send(string content, Ice.Current current)
        {
            System.Windows.Forms.MessageBox.Show(content);
            return "";
        }
        public override string receive(string command, Ice.Current current)
        {
            System.Windows.Forms.MessageBox.Show(command);
            return "";
        }
    }

}