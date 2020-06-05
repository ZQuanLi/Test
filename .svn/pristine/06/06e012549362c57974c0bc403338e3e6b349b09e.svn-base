using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.ExpAdminApp.Monitor
{
    public partial class ExpAdminMonitorBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.ExpAdminApp.Monitor.ExpAdminMonitorDAL dal = null;
        public ExpAdminMonitorBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.ExpAdminApp.Monitor.ExpAdminMonitorDAL(_project, _ledger, _uid);
        }


        /// <summary>
        /// 运行状况管理列表
        /// </summary>
        /// <param name="coName"></param>
        /// <param name="moduleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorOnList(string CoStrcName, string CoName, string Ssr, int co_id)
        {
            DataTable dtSource = dal.GetYdMonitorOnList(0, CoStrcName, CoName, Ssr, co_id);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            dtSource.Columns["Ssr"].MaxLength = 10;
            return dtSource;
        }

        public DataTable GetCity(int Attrib, int Co_id)
        {
            return dal.GetCity(Attrib, Co_id);
        }

        public DataTable GetRoom(int Attrib, int Co_id)
        {
            return dal.GetRoom(Attrib, Co_id);
        }

    }
}
