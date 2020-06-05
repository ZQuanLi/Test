using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.DataProcess;

namespace YDS6000.BLL.DataProcess
{
    public class MainFormBLL
    {
        private string Ledgers = "";
        private readonly MainFormDAL dal = null;

        public MainFormBLL(string _ledgers)
        {
            Ledgers = _ledgers;
            dal = new MainFormDAL(_ledgers);
        }

        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetV1_gateway()
        {
            return dal.GetV1_gateway();
        }

        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetV1_gateway_esp()
        {
            return dal.GetV1_gateway_esp();
        }
        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetV1_gateway_esp_module()
        {
            return dal.GetV1_gateway_esp_module();
        }

        public DataTable GetArmData()
        {
            return dal.GetArmData();
        }

        public DataTable GetCurData(int ledger,int module_id,string funType,string dataValue)
        {
            return dal.GetCurData(ledger, module_id, funType, dataValue);
        }

        public DataTable GetWrData()
        {
            return dal.GetWrData();
        }

        public DataTable GetSys_user(int ledger,string uSign)
        {
            return dal.GetSys_user(ledger, uSign);
        }
    }
}
