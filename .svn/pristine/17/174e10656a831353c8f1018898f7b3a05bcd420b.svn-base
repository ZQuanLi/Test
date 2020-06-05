using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpYdBuildingBatchBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpYdBuildingBatchDAL dal = null;
        public ExpYdBuildingBatchBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpYdBuildingBatchDAL(_ledger, _uid);
        }

        /// <summary>
        /// 导出采集器模板(导出Excel模板)
        /// </summary>
        /// <returns></returns>
        public bool ExportBuildingBatch(string filename, DataTable dtSource)
        {
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            if (dtSource == null || dtSource.Rows.Count == 0)
                throw new Exception("没有数据");
            Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
            columnCellStyle0 = new Excel.ExcelCellStyle()
            {
                DataFormart = "0.00",
                HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
            };
            Excel.ExcelCellStyle columnCellStyle1 = new Excel.ExcelCellStyle();
            columnCellStyle1 = new Excel.ExcelCellStyle()
            {
                DataFormart = "yyyy-MM-dd HH:mm:ss",
            };
            Excel.ExcelColumnCollection columns = new Excel.ExcelColumnCollection();

            columns.Add(new Excel.ExcelColumn("集中器名称", "GwName", 20) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("集中器IP地址", "GwIp", 20) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("采集器名称", "EspName", 20) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("采集器地址", "EspAddr", 20) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("通讯方式（0:COM;1:TCP/Client;3:TCP/Server;4:IOServer）", "TransferType", 40) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("TCP端口", "EspPort", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("COM口", "ComPort", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("波特率", "Baud", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("数据位", "DataBit", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("停止位", "StopBit", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("校验方式（0 无/1 奇/2 偶/3标志/4 空格）", "Parity", 35) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("设备地址", "MeterAddr", 17) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("倍率", "Multiply", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电表型号(如DDS3366L)", "ModuleType", 25) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("回路地址", "ModuleAddr", 17) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间(约定定义)（如男生宿舍->南苑1栋->南苑1层->101房间）", "CoFullName", 30) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("Excel模板");
            excel.SetColumnName(columns, 0, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 1, 0);
            excel.SaveExcelByFullFileName(filename);
            return true;
        }

        //批量上载采集器Excel数据
        public DataTable GetExcelDataOfBuilding(string filename)
        {
            string filename1 = filename;
            //
            #region 1.0获取XLS数据
            Excel.XlsOparete excel = new Excel.XlsOparete();
            DataTable dtSource = excel.XlsToDataTable(filename1);
            DataRow dr = null;
            StringBuilder strDel = new StringBuilder();
            if (dtSource.Rows.Count > 0)
            {
                dr = dtSource.Rows[0];
            }
            else
            {
                foreach (DataColumn dc in dtSource.Columns)
                {
                    if (!string.IsNullOrEmpty(strDel.ToString()))
                        strDel.Append(",");
                    strDel.Append(dc.ColumnName);
                }
            }
            #endregion

            #region 2.0修改列
            if (dr != null)
            {
                foreach (DataColumn dc in dtSource.Columns)
                {
                    if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("集中器名称"))
                    {
                        dc.ColumnName = "GwName";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("集中器IP地址"))
                    {
                        dc.ColumnName = "GwIp";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("采集器名称"))
                    {
                        dc.ColumnName = "EspName";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("采集器地址"))
                    {
                        dc.ColumnName = "EspAddr";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("通讯方式（0:COM;1:TCP/Client;3:TCP/Server;4:IOServer）"))
                    {
                        dc.ColumnName = "TransferType";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("TCP端口"))
                    {
                        dc.ColumnName = "EspPort";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("COM口"))
                    {
                        dc.ColumnName = "ComPort";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("波特率"))
                    {
                        dc.ColumnName = "Baud";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("数据位"))
                    {
                        dc.ColumnName = "DataBit";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("停止位"))
                    {
                        dc.ColumnName = "StopBit";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("校验方式（0 无/1 奇/2 偶/3标志/4 空格）"))
                    {
                        dc.ColumnName = "Parity";
                    }
                    //else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("电表地址"))
                    //{
                    //    dc.ColumnName = "ModuleAddr";
                    //}
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("设备地址"))
                    {
                        dc.ColumnName = "MeterAddr";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("倍率"))
                    {
                        dc.ColumnName = "Multiply";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("电表型号(如DDS3366L)"))
                    {
                        dc.ColumnName = "ModuleType";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("回路地址"))
                    {
                        dc.ColumnName = "ModuleAddr";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("房间(约定定义)（如男生宿舍->南苑1栋->南苑1层->101房间）"))
                    {
                        dc.ColumnName = "CoFullName";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strDel.ToString()))
                            strDel.Append(",");
                        strDel.Append(dc.ColumnName);
                    }
                }
                dtSource.Rows.RemoveAt(0);
            }
            #endregion

            #region 3.0删除列
            if (!string.IsNullOrEmpty(strDel.ToString()))
            {
                foreach (string s in strDel.ToString().Split(','))
                    dtSource.Columns.Remove(s);
            }
            #endregion

            #region 4.0增加列
            string strCs = "ErrTxt,ErrCode";
            foreach (string s in strCs.Split(','))
            {
                if (dtSource.Columns.Contains(s) == true) continue;
                dtSource.Columns.Add(s, typeof(System.String));
            }

            //string strCs = "ErrTxt,Co_idS,GwAddr,GwIp,ModuleName,MeterAddr,ModuleAddr";
            //foreach (string s in strCs.Split(','))
            //{
            //    if (dtSource.Columns.Contains(s) == true) continue;
            //    dtSource.Columns.Add(s, typeof(System.String));
            //}
            //string intCs = "Gw_id,Esp_id,Mm_id,Module_id,Meter_id,Parent_id,Attrib,Layer,IsDefine,ErrCode,Co_id";
            //foreach (string s in intCs.Split(','))
            //{
            //    if (dtSource.Columns.Contains(s) == true) continue;
            //    dtSource.Columns.Add(s, typeof(System.Int32));
            //}
            #endregion

            #region 5.0删除空白行 (必须倒叙删除，因为每删除一行，索引就会发生改变)
            for (int i = dtSource.Rows.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["EspName"])) && string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["EspName"])) && string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["ModuleAddr"])))
                {
                    dtSource.Rows[i].Delete();
                }
            }
            #endregion

            dtSource.AcceptChanges();
            return dtSource;
        }

        public bool UpdateBuildingMode(DataTable dtSource, ref string fileName, out string msg)
        {
            msg = "";
            this.UpdateCoList(dtSource);
            this.UpdateMdList(dtSource);
            msg = this.ExportInfo(dtSource, ref fileName);
            return true;
        }

        /// <summary>
        /// 上载建筑信息
        /// </summary>
        /// <param name="dtSource"></param>
        private void UpdateCoList(DataTable dtSource)
        {
            StringBuilder coSplit = new StringBuilder();
            var res1 = (from s1 in dtSource.AsEnumerable()
                        select new
                        {
                            CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"])
                        }).Distinct();
            foreach (var obj in res1)
            {
                string[] str = obj.CoFullName.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                if (str.Count() == 0) continue;
                DataTable dtCoInfo = dal.GetCoInfo(str[0]);//获取全部建筑信息
                coSplit.Clear();
                int pid = 0, layer = 0;
                foreach (string name in str)
                {
                    if (!string.IsNullOrEmpty(coSplit.ToString()))
                        coSplit.Append("->");
                    coSplit.Append(CommFunc.ConvertDBNullToString(name));
                    DataRow[] arr = dtCoInfo.Select("CoFullName='" + coSplit.ToString() + "'");
                    int co_id = 0, dbLayer = 0;
                    string coName = "";
                    if (arr.Count() > 0)
                    {
                        co_id = CommFunc.ConvertDBNullToInt32(arr[0]["Co_id"]);
                        dbLayer = CommFunc.ConvertDBNullToInt32(arr[0]["Layer"]);
                        coName = CommFunc.ConvertDBNullToString(arr[0]["CoName"]);
                    }
                    pid = dal.UpdateCoList(co_id, name.Trim(), pid, layer);/*更新建筑*/
                    layer = layer + 1;
                }
            }
        }

        private void UpdateMdList(DataTable dtSource)
        {
            DataTable dtCoInfo = dal.GetCoInfo("");
            DataTable dtMdInfo = dal.GetMdInfo();
            //
            foreach (DataRow dr in dtSource.Rows)
            {
                string gwName = CommFunc.ConvertDBNullToString(dr["GwName"]);
                string espName = CommFunc.ConvertDBNullToString(dr["EspName"]);
                string meterAddr = CommFunc.ConvertDBNullToString(dr["MeterAddr"]);
                string moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                string coFullName = CommFunc.ConvertDBNullToString(dr["CoFullName"]);
                //
                int gw_id = this.GwInfo(dr);
                if (gw_id == 0) continue;
                int esp_id = this.EspInfo(dr, gw_id);
                if (esp_id == 0) continue;
                int meter_id = this.MeterInfo(dr, esp_id);
                if (meter_id == 0) continue;
                int module_id = this.ModuleInfo(dr, meter_id);
                if (meter_id == 0) continue;
            }
        }


        private int GwInfo(DataRow dr)
        {
            string gwName = CommFunc.ConvertDBNullToString(dr["GwName"]);
            string gwIp = CommFunc.ConvertDBNullToString(dr["GwIp"]);
            string msg = "";
            //
            bool isUp = true;
            int gw_id = 0;
            DataTable dtMdInfo = dal.GetMdInfo(gwName);
            //更新集中器
            DataRow[] arr = dtMdInfo.Select();
            if (arr.Count() > 0)
            {
                gw_id = CommFunc.ConvertDBNullToInt32(arr[0]["Gw_id"]);
                if (gwIp.Equals(CommFunc.ConvertDBNullToString(arr[0]["GwIp"])))
                    isUp = false;
            }
            if (isUp == true)
            {
                gw_id = dal.UpdateGwInfo(gw_id, gwName, gwIp, out msg);
                if (gw_id == 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = msg;
                }
            }
            return gw_id;
        }

        private int EspInfo(DataRow dr, int gw_id)
        {
            v1_gateway_espVModel vm = new v1_gateway_espVModel
            {
                EspAddr = CommFunc.ConvertDBNullToString(dr["EspAddr"]),
                EspName = CommFunc.ConvertDBNullToString(dr["EspName"]),
                TransferType = CommFunc.ConvertDBNullToInt32(dr["TransferType"]),
                EspPort = CommFunc.ConvertDBNullToInt32(dr["EspPort"]),
                ComPort = CommFunc.ConvertDBNullToString(dr["ComPort"]),
                Baud = CommFunc.ConvertDBNullToInt32(dr["Baud"]),
                DataBit = CommFunc.ConvertDBNullToInt32(dr["DataBit"]),
                Parity = CommFunc.ConvertDBNullToInt32(dr["Parity"]),
                StopBit = CommFunc.ConvertDBNullToInt32(dr["StopBit"]),
            };
            string msg = "";
            bool isUp = false;
            int esp_id = 0;
            //更新
            DataTable dtMdInfo = dal.GetEspInfo(gw_id, vm.EspName);
            DataRow[] arr = dtMdInfo.Select();
            if (arr.Count() > 0)
            {
                esp_id = CommFunc.ConvertDBNullToInt32(arr[0]["Esp_id"]);
                if (!vm.EspName.Equals(CommFunc.ConvertDBNullToString(arr[0]["EspName"])))
                    isUp = true;
                else if (vm.TransferType != CommFunc.ConvertDBNullToInt32(arr[0]["TransferType"]))
                    isUp = true;
                else if (vm.EspPort != CommFunc.ConvertDBNullToInt32(arr[0]["EspPort"]))
                    isUp = true;
                else if (!vm.ComPort.Equals(CommFunc.ConvertDBNullToString(arr[0]["ComPort"])))
                    isUp = true;
                else if (vm.Baud != CommFunc.ConvertDBNullToInt32(arr[0]["Baud"]))
                    isUp = true;
                else if (vm.DataBit != CommFunc.ConvertDBNullToInt32(arr[0]["DataBit"]))
                    isUp = true;
                else if (vm.Parity != CommFunc.ConvertDBNullToInt32(arr[0]["Parity"]))
                    isUp = true;
                else if (vm.StopBit != CommFunc.ConvertDBNullToInt32(arr[0]["StopBit"]))
                    isUp = true;
            }
            else
            {
                isUp = true;
            }
            vm.Gw_id = gw_id;
            vm.Esp_id = esp_id;
            if (isUp == true)
            {
                esp_id = dal.UpdateEspInfo(vm, out msg);
                if (esp_id == 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = msg;
                }
            }
            return vm.Esp_id;
        }

        private int MeterInfo(DataRow dr, int esp_id)
        {
            v1_gateway_esp_meterVModel vm = new v1_gateway_esp_meterVModel()
            {
                Esp_id = esp_id,
                MeterAddr = CommFunc.ConvertDBNullToString(dr["MeterAddr"]),
                Multiply = CommFunc.ConvertDBNullToDecimal(dr["Multiply"])
            };
            string moduleType = CommFunc.ConvertDBNullToString(dr["ModuleType"]);
            string msg = "";
            bool isUp = false;
            //更新
            DataTable dtMdInfo = dal.GetMeterInfo(esp_id, vm.MeterAddr);
            DataRow[] arr = dtMdInfo.Select();
            if (arr.Count() > 0)
            {
                vm.Meter_id = CommFunc.ConvertDBNullToInt32(arr[0]["Meter_id"]);
                if (!moduleType.Equals(CommFunc.ConvertDBNullToString(arr[0]["ModuleType"])))
                    isUp = true;
                else if (vm.Multiply != CommFunc.ConvertDBNullToDecimal(arr[0]["Multiply"]))
                    isUp = true;
            }
            else
            {
                isUp = true;
            }
            if (isUp == true)
            {
                int meter_id = dal.UpdateMeterInfo(vm, moduleType, out msg);
                if (meter_id == 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = msg;
                }
            }
            return vm.Meter_id;
        }

        private int ModuleInfo(DataRow dr, int meter_id)
        {
            v1_gateway_esp_moduleVModel vm = new v1_gateway_esp_moduleVModel()
            {
                Module_id = 0,
                Meter_id = meter_id,
                ModuleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]),
            };
            string coFullName = CommFunc.ConvertDBNullToString(dr["CoFullName"]);
            string msg = "";
            //
            bool isUp = false;
            //更新
            DataTable dtMdInfo = dal.GetMdInfo(meter_id, vm.ModuleAddr);
            DataRow[] arr = dtMdInfo.Select();
            if (arr.Count() > 0)
            {
                vm.Module_id = CommFunc.ConvertDBNullToInt32(arr[0]["Module_id"]);
                if (!vm.ModuleAddr.Equals(CommFunc.ConvertDBNullToString(arr[0]["ModuleAddr"])))
                    isUp = true;
                else if (!coFullName.Equals(CommFunc.ConvertDBNullToString(arr[0]["CoFullName"])))
                    isUp = true;
            }
            else
            {
                isUp = true;
            }
            if (isUp == true)
            {
                int module_id = dal.UpdateModuleInfo(vm, coFullName, out msg);
                if (module_id == 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = msg;
                }
            }
            return vm.Module_id;
        }

        private string ExportInfo(DataTable dtSource, ref string fileName)
        {
            string msg = "";
            #region 汇出上载错误信息
            try
            {
                //
                int Total = dtSource.Rows.Count;
                //string sPath = @"c:\temp";
                string sPath = fileName;
                if (!System.IO.Directory.Exists(sPath))
                    System.IO.Directory.CreateDirectory(sPath);
                //
                DataRow[] errArr = dtSource.Select("isnull(ErrCode,0)<0");
                int eNum = errArr.Count();
                msg = "总记录:" + Total + ",成功上载:" + (Total - eNum);
                if (eNum > 0)
                {
                    Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
                    columnCellStyle0 = new Excel.ExcelCellStyle()
                    {
                        DataFormart = "0.00",
                        HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
                    };
                    string fn = "执行错误信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    string filePath = fileName + fn;
                    fileName = fn;
                    Excel.ExcelColumnCollection columns = new Excel.ExcelColumnCollection();

                    columns.Add(new Excel.ExcelColumn("集中器名称", "GwName", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("集中器IP地址", "GwIp", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("采集器名称", "EspName", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("采集器地址", "EspAddr", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("通讯方式（0:COM;1:TCP/Client;3:TCP/Server;4:IOServer）", "TransferType", 40) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("TCP端口", "EspPort", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("COM口", "ComPort", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("波特率", "Baud", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("数据位", "DataBit", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("停止位", "StopBit", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("校验方式（0 无/1 奇/2 偶/3标志/4 空格）", "Parity", 35) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("设备地址", "MeterAddr", 17) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("倍率", "Multiply", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("电表型号(如DDS3366L)", "ModuleType", 25) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("回路地址", "ModuleAddr", 17) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("房间(约定定义)（如男生宿舍->南苑1栋->南苑1层->101房间）", "CoFullName", 30) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("错误提示", "ErrTxt", 30) { IsSetWith = true });
                    Excel.ExcelOparete excel = new Excel.ExcelOparete("执行结果");
                    excel.SetColumnName(columns, 0, 0);
                    excel.SetColumnValue(columns, errArr, 1, 0);
                    excel.SaveExcelByFullFileName(filePath);
                    msg = msg + ",失败上载:" + eNum;
                }
                else
                {
                    fileName = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            #endregion
            return msg;
        }




    }
}
