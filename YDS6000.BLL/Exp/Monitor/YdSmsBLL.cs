using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Monitor
{
    partial class MonitorBLL
    {
        public DataTable GetYdSmsContent(int co_id ,int module_id, string moduleAddr)
        {
            return dal.GetYdSmsContent(co_id, module_id, moduleAddr);
        }
        public DataTable GetMailInfo()
        {
            return dal.GetMailInfo();
        }
    }
}
