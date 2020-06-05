using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;


namespace YDS6000.BLL.Exp.RunReport
{
    public partial class ExpYdRepHisBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.RunReport.ExpYdRepHisDAL dal = null;
        public ExpYdRepHisBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.RunReport.ExpYdRepHisDAL(_project, _ledger, _uid);
        }

        public DataTable GetModuleListByOrg(string CoStrcName, string CoName, params string[] funTypeParams)
        {
            return dal.GetModuleListByOrg(CoStrcName, CoName, funTypeParams);
        }

        public List<YdRepHisVModel> GetYdRepHisList(DateTime dtFirst, DateTime dtLast, string CoStrcName, string CoName, int IsMultiply, params string[] funTypeParams)
        {
            List<YdRepHisVModel> list = new List<YdRepHisVModel>();
            var dtModules = dal.GetModuleListByOrg(CoStrcName, CoName, funTypeParams);
            var modulesEnum = from s1 in dtModules.AsEnumerable()
                              select new
                              {
                                  Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                                  ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                                  ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                                  Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                                  CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                                  CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                                  Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                                  FunType = CommFunc.ConvertDBNullToString(s1["FunType"])
                              };
            if (modulesEnum == null || modulesEnum.Count() == 0) return list;
            var modulesList = modulesEnum.ToList();
            HashSet<int> ds = new HashSet<int>();
            for (int i = 0; i < modulesList.Count; i++)
            {
                ds.Add(modulesList[i].Module_id);
            }
            var dtSourceFirst = WholeBLL.GetCoreQueryData(this.Ledger, string.Join(",", ds), dtFirst, dtFirst, "day");
            var dtSourceLast = WholeBLL.GetCoreQueryData(this.Ledger, string.Join(",", ds), dtLast, dtLast, "day");
            var sourceFirst = from a in dtSourceFirst.AsEnumerable()
                              select new
                              {
                                  Tagtime = CommFunc.ConvertDBNullToDateTime(a["Tagtime"]),
                                  Co_id = CommFunc.ConvertDBNullToInt32(a["Co_id"]),
                                  Fun_id = CommFunc.ConvertDBNullToInt32(a["Fun_id"]),
                                  Module_id = CommFunc.ConvertDBNullToInt32(a["Module_id"]),
                                  ModuleAddr = CommFunc.ConvertDBNullToString(a["ModuleAddr"]),
                                  FirstVal = CommFunc.ConvertDBNullToDecimal(a["FirstVal"]),
                                  LastVal = CommFunc.ConvertDBNullToDecimal(a["LastVal"])
                              };
            var sourceLast = from a in dtSourceLast.AsEnumerable()
                             select new
                             {
                                 Tagtime = CommFunc.ConvertDBNullToDateTime(a["Tagtime"]),
                                 Co_id = CommFunc.ConvertDBNullToInt32(a["Co_id"]),
                                 Fun_id = CommFunc.ConvertDBNullToInt32(a["Fun_id"]),
                                 Module_id = CommFunc.ConvertDBNullToInt32(a["Module_id"]),
                                 ModuleAddr = CommFunc.ConvertDBNullToString(a["ModuleAddr"]),
                                 FirstVal = CommFunc.ConvertDBNullToDecimal(a["FirstVal"]),
                                 LastVal = CommFunc.ConvertDBNullToDecimal(a["LastVal"])
                             };
            var sourceFirstList = sourceFirst.ToList();
            var sourceLastList = sourceLast.ToList();
            for (int i = 0; i < modulesList.Count; i++)
            {
                var model = new YdRepHisVModel();
                model.ID = i + 1;
                model.Co_id = modulesList[i].Co_id;
                model.CoName = modulesList[i].CoName;
                model.CoStrcName = modulesList[i].CoStrcName;
                model.Date = dtFirst.ToString("yyyy-MM-dd") + "~" + dtLast.ToString("yyyy-MM-dd");
                model.Multiply = modulesList[i].Multiply.ToString("f2");
                model.FirstVal = "NULL";
                model.LastVal = "NULL";
                model.UseVal = "NULL";
                model.ModuleName = modulesList[i].ModuleName;
                model.ModuleAddr = modulesList[i].ModuleAddr;
                model.Module_id = modulesList[i].Module_id;
                var firstObj = sourceFirstList.Where(c => c.Co_id == model.Co_id && c.Module_id == model.Module_id && c.ModuleAddr == model.ModuleAddr).FirstOrDefault();

                if (firstObj != null) model.FirstVal = IsMultiply == 1 ? (firstObj.FirstVal * modulesList[i].Multiply).ToString("f2") : firstObj.FirstVal.ToString("f2");
                var lastObj = sourceLastList.Where(c => c.Co_id == model.Co_id && c.Module_id == model.Module_id && c.ModuleAddr == model.ModuleAddr).FirstOrDefault();

                if (lastObj != null) model.LastVal = IsMultiply == 1 ? (lastObj.LastVal * modulesList[i].Multiply).ToString("f2") : lastObj.LastVal.ToString("f2");

                if (firstObj != null && lastObj != null)
                    model.UseVal = IsMultiply == 1 ? ((lastObj.LastVal - firstObj.FirstVal) * modulesList[i].Multiply).ToString("f2") : (lastObj.LastVal - firstObj.FirstVal).ToString("f2");
                list.Add(model);
            }
            return list;
        }
    }
}
