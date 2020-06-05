using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Menu
{
    public partial class MenuDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public MenuDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="project">项目名称</param>
        /// <returns></returns>
        public DataTable GetMenuList(string project)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Role_id from sys_user where Ledger=@Ledger and Uid=@Uid");
            int role_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Uid = this.SysUid }));
            strSql.Clear();
            strSql.Append("select a.menu_no,a.descr,trim(ifnull(a.parent_no,''))as parent_no,a.prog_id,a.path,a.ordno,a.attrib,");
            strSql.Append("CONCAT('d:',ifnull(b._delete,0),',w:',ifnull(b._write,0),',r:',case when ifnull(b._delete,0)+ifnull(b._write,0)+ifnull(b._app,0)>0 then 1 else ifnull(b._read,0) end) as power");
            strSql.Append(" from sys_menu as a");
            strSql.Append(" left join sys_user_prog as b on b.Ledger=@Ledger and b.Role_id=@Role_id and a.prog_id=b.prog_id");
            strSql.Append(" where a.project=@project and ifnull(a.disabled,0)=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, project = project,Role_id = role_id });
        }
    }
}
