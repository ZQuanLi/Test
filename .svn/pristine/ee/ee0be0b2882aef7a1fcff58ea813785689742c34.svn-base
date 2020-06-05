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
    public class SyscontBLL
    {
        private string Ledgers = "";

        private readonly SyscontDAL dal = null;
        public SyscontBLL(string _ledgers)
        {
            this.Ledgers = _ledgers;
            dal = new SyscontDAL(_ledgers);
        }

        /// <summary>
        /// 获取基本配置信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSyscont()
        {
            return dal.GetSyscont();
        }

        /// <summary>
        /// 获取回路采集点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetMapInfo()
        {
            return dal.GetMapInfo();
        }
    }
}
