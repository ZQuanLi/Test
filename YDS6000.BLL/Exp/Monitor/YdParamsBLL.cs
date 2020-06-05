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
        /// <summary>
        /// 获取参数设置列表
        /// </summary>
        /// <param name="coName"></param>
        /// <param name="moduleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdParamsOfList(string CoStrcName, string CoName)
        {
            DataTable dtSource = dal.GetYdParamsOfList(CoStrcName, CoName);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            return dtSource;
        }

        public DataTable GetYdParamsOfFunType()
        {
            return dal.GetYdParamsOfFunType();
        }
    }
}
