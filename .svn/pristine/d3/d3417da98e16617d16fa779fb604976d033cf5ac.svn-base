using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    /// <summary>
    /// XML类
    /// </summary>
    public class XMLCofig
    {
        private static string strXmlFile = HttpContext.Current.Request.PhysicalApplicationPath + "/Resource/Data/Module.xml";
        private static XDocument objXmlDoc = null;

        private static XMLCofig config = null;
        static XMLCofig()
        {
            if (config == null)
            {                
                objXmlDoc = XDocument.Load(strXmlFile);
            }
        }

        private static DataTable GetXmlToTable()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Type", typeof(System.String));
            dtSource.Columns.Add("Protocol", typeof(System.String));
            dtSource.Columns.Add("FunType", typeof(System.String));
            dtSource.Columns.Add("FunName", typeof(System.String));
            dtSource.Columns.Add("Action", typeof(System.String));
            dtSource.Columns.Add("DataType", typeof(System.String));
            dtSource.Columns.Add("Scale", typeof(System.String));
            dtSource.Columns.Add("OrdNo", typeof(System.String));
            //
            dtSource.Columns.Add("OrdGrp", typeof(System.String));
            dtSource.Columns.Add("Unit", typeof(System.String));
            dtSource.Columns.Add("PointType", typeof(System.String));
            dtSource.Columns.Add("AlarmModel", typeof(System.String));
            dtSource.Columns.Add("SubTab", typeof(System.String));

            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Type"], dtSource.Columns["FunType"] };
            //
            var xs = objXmlDoc.Root.Elements("data");
            foreach (var xData in xs)
            {
                XElement xType = xData.Element("type");
                XElement xPro = xData.Element("protocol");                
                XElement xRows = xData.Element("rows");
                var xFs = xRows.Elements("funtion");
                string strType = xType.Value;
                string strProtocol = xPro == null ? "" : xPro.Value;
                foreach (var xf in xFs)
                {
                    XAttribute xOrdGrp = xf.Attribute("OrdGrp");
                    XAttribute xUnit = xf.Attribute("Unit");
                    XAttribute xPointType = xf.Attribute("PointType");
                    XAttribute xAlarmModel = xf.Attribute("AlarmModel");
                    XAttribute xSubTab = xf.Attribute("SubTab");
                    //
                    string strFunType = xf.Attribute("FunType").Value.TrimEnd();
                    string strFunName = xf.Attribute("FunName").Value.TrimEnd();
                    string strAction = xf.Attribute("Action").Value.TrimEnd();
                    string strDataType = xf.Attribute("DataType").Value.TrimEnd();
                    string strScale = xf.Attribute("Scale").Value.TrimEnd();
                    string strOrdNo = xf.Attribute("OrdNo").Value.TrimEnd();
                    //
                    string strOrdGrp = xOrdGrp == null ? "0" : xOrdGrp.Value.TrimEnd();
                    string strUnit = xUnit == null ? "" : xUnit.Value.TrimEnd();
                    string strPointType = xPointType == null ? "0" : xPointType.Value.TrimEnd();
                    string strAlarmModel = xAlarmModel == null ? "0" : xAlarmModel.Value.TrimEnd();
                    string strSubTab = xSubTab == null ? "" : xSubTab.Value.TrimEnd();
                    //
                    DataRow addDr = dtSource.Rows.Find(new object[] { strType, strFunType });
                    if (addDr == null)
                    {
                        addDr = dtSource.NewRow();
                        addDr["Type"] = strType;
                        addDr["Protocol"] = strProtocol;
                        addDr["FunType"] = strFunType;
                        addDr["FunName"] = strFunName;
                        addDr["Action"] = strAction;
                        addDr["DataType"] = strDataType;
                        addDr["Scale"] = strScale;
                        addDr["OrdNo"] = strOrdNo;
                        addDr["OrdGrp"] = strOrdGrp;
                        addDr["Unit"] = strUnit;
                        addDr["PointType"] = strPointType;
                        addDr["AlarmModel"] = strAlarmModel;
                        addDr["SubTab"] = strSubTab;

                        dtSource.Rows.Add(addDr);
                    }
                }
            }
            return dtSource;
        }

        public static DataTable GetDrive()
        {
            DataTable dtSource = GetXmlToTable();
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("type", typeof(System.String));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["type"] };
            foreach (DataRow dr in dtSource.Rows)
            {
                DataRow addDr = dtRst.Rows.Find(dr["type"]);
                if (addDr == null)
                    dtRst.Rows.Add(dr["type"]);
            }
            return dtRst;
        }

        public static DataTable GetDrive(string moduleType)
        {
            DataTable dtSource = GetXmlToTable();
            int rec = dtSource.Rows.Count;
            for (int i = rec - 1; i >= 0; i--)
            {
                if (!CommFunc.ConvertDBNullToString(dtSource.Rows[i]["type"]).Equals(moduleType))
                    dtSource.Rows[i].Delete();
            }
            dtSource.AcceptChanges();
            return dtSource;
        }

    }
}