using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace DataProcess.YdProcess
{
    public static class ZpRaVal
    {
        /// <summary>
        /// 赠送电量
        /// </summary>
        public static void BaseVal(int ledger)
        {
            YDS6000.BLL.DataProcess.ZpRaValBLL bll = new YDS6000.BLL.DataProcess.ZpRaValBLL(ledger, Config.Uid);
            bll.BaseVal();
        }
    }
}
