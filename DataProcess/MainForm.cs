using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using YDS6000.Models;

namespace DataProcess
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        #region 右下角图票
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!IsHandleCreated)
            {
                this.Close();
            }
        }
        #endregion

        private DataTable dtModule = new DataTable();
        private static YDS6000.BLL.DataProcess.MainFormBLL bll = new YDS6000.BLL.DataProcess.MainFormBLL(Config.Systems);

        /// <summary>
        /// 是否显示第三个框框
        /// </summary>
        private bool isShowList = true;/*是否显示第三个框框*/

        //private delegate void DelegateARM(DataTable dtSource);//用于自动抄表等等
        private delegate void DelegateARM();//用于自动抄表等等

        private delegate void ShowResultToList();//创建一个委托类型,用于显示Log
        private delegate void ShowUI(CmdResult command); /*显示结果集*/


        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("真的要退出吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                CheckPwd form1 = new CheckPwd();
                if (form1.ShowDialog() == DialogResult.OK)
                {
                    this.btnEnd_Click(sender, e);
                    /*清理缓存*/
                    try
                    {
                        //bool bIsNext = true;
                        //var enumer = NCSys.Result.GetEnumerator();
                        //while (bIsNext == true)
                        //{
                        //    bIsNext = enumer.MoveNext();
                        //    if (bIsNext == false) break;
                        //    var s0 = enumer.Current;
                        //    CollectVModel s1 = s0.Value;
                        //    if (s1 == null) continue;

                        //    SysPro pro;
                        //    NCSys.Pro.TryGetValue(s1.Ledger, out pro);
                        //    if (pro == null) continue;
                        //    int cc = 0;
                        //    while (++cc <= 2)
                        //    {
                        //        if (MemcachedMgr.RemoveKey(pro.ProjectKey + s1.CachedKey) == true)
                        //        {
                        //            //FileLog.WriteLog("key:" + pro.ProjectKey + s1.CachedKey + "移除成功");
                        //            break;
                        //        }
                        //        Thread.Sleep(5);
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        FileLog.Error("退出清理缓存错误" + ex.Message + ex.StackTrace);
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//将窗体最小化

            Control.CheckForIllegalCrossThreadCalls = false;
            //绑定数据
            this.dgv1.AutoGenerateColumns = false;
            this.dgv1.ReadOnly = true;
            this.dgv1.AllowUserToAddRows = false;
            this.update_dt1.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            dtModule.Columns.Add("Ledger", typeof(System.Int32)); /**/
            dtModule.Columns.Add("Co_id", typeof(System.Int32)); /*房间ID*/
            dtModule.Columns.Add("Module_id", typeof(System.Int32)); /*设备ID*/
            dtModule.Columns.Add("ModuleAddr", typeof(System.String)); /*设备地址*/
            dtModule.Columns.Add("PortName", typeof(System.String));/*PC端口*/
            dtModule.Columns.Add("ModuleName", typeof(System.String));/*设备名称*/
            dtModule.Columns.Add("CoFullName", typeof(System.String));/*房间全名*/
            dtModule.Columns.Add("TransferType", typeof(System.Int32));/*通信方式*/
            dtModule.Columns.Add("Update_dt", typeof(System.DateTime));/*更新时间*/
            dtModule.Columns.Add("Status", typeof(System.String));/*设备状态*/
            dtModule.Columns.Add("Value", typeof(System.String));/*值*/
            dtModule.Columns.Add("ViewTimes", typeof(System.Int32));/*通信次数*/
            dtModule.Columns.Add("SueTimes", typeof(System.Int32));/*成功次数*/
            dtModule.Columns.Add("SuePer", typeof(System.String));/*成功率*/
            dtModule.Columns.Add("ErrCode", typeof(System.Int32));/*异常code*/
            dtModule.Columns.Add("TransferTypeS", typeof(System.String));/*通信方式说明*/
            dtModule.PrimaryKey = new DataColumn[] { dtModule.Columns["Ledger"], dtModule.Columns["Module_id"] };
            this.dgv1.DataSource = dtModule;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {            
            try
            {
                this.txtBeginTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.txtArmTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.txtStatus.Text = "";
                //SysPro pro;
                //if (NCSys.Pro.TryGetValue(8, out pro) == true)
                //{
                this.dgv1.ContextMenuStrip = this.contextMenuStrip1;
                //}
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ShowResultToList AsyncShowResultToList = new ShowResultToList(ShowResult);/*显示返回的结果集*/
            AsyncShowResultToList.BeginInvoke(null, null);/*启动显示记录线程*/

        }

        private void LoadData()
        {
            #region 加载数据前，禁用一些按钮功能
            this.btnStart.Enabled = false;
            this.btnEnd.Enabled = false;
            this.btnReload.Enabled = false;
            #endregion
            #region 加载数据

            #region 抄表频率
            //bool isVrc = new ThreadBLL().GetVrc();
            //if (isVrc == false)
            //{/*过期*/
            //    MessageBox.Show("有效期已过", "告警信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Application.Exit();
            //    return;
            //}
            #endregion

            #region 设备List
            this.treeView1.Nodes.Clear();
            this.dtModule.Clear();
            this.listViewLog.Items.Clear();
            //
            TreeNode node = new TreeNode();
            node.Name = "module";
            node.Text = "设备信息";
            node.Tag = null;
            node.ExpandAll();
            node.Expand();
            this.treeView1.Nodes.Add(node);
            //

            StringBuilder strFilter = new StringBuilder();
            DataTable dtPe_GateWay = bll.GetV1_gateway();
            DataTable dtPe_GateWay_Esp = bll.GetV1_gateway_esp();
            DataTable dtPe_GateWay_Esp_Module = bll.GetV1_gateway_esp_module();
            //
            #region 加载数据
            foreach (DataRow drGw in dtPe_GateWay.Rows)
            {
                TreeNode GwNode = new TreeNode();
                GwNode.Name = "GW:" + CommFunc.ConvertDBNullToString(drGw["Gw_id"]).Trim();
                GwNode.Text = CommFunc.ConvertDBNullToString(drGw["GwName"]).Trim();
                GwNode.ExpandAll();
                GwNode.Expand();
                node.Nodes.Add(GwNode);
                strFilter.Clear();
                strFilter.Append("Ledger=" + CommFunc.ConvertDBNullToInt32(drGw["Ledger"]) + " and Gw_id=" + CommFunc.ConvertDBNullToInt32(drGw["Gw_id"]));
                foreach (DataRow drEsp in dtPe_GateWay_Esp.Select(strFilter.ToString()))
                {
                    TreeNode EspNode = new TreeNode();
                    EspNode.Name = "ESP:" + CommFunc.ConvertDBNullToString(drEsp["Esp_id"]).Trim();
                    EspNode.Text = CommFunc.ConvertDBNullToString(drEsp["EspName"]).Trim() + "(" + CommFunc.ConvertDBNullToString(drEsp["EspIp"]) + ":" + CommFunc.ConvertDBNullToString(drEsp["EspPort"]) + ")";
                    EspNode.ExpandAll();
                    EspNode.Expand();
                    GwNode.Nodes.Add(EspNode);
                    //
                    strFilter.Clear();
                    strFilter.Append("Ledger=" + CommFunc.ConvertDBNullToInt32(drEsp["Ledger"]) + " and Esp_id=" + CommFunc.ConvertDBNullToInt32(drEsp["Esp_id"]));
                    foreach (DataRow drModule in dtPe_GateWay_Esp_Module.Select(strFilter.ToString()))
                    {
                        DataRow addDr = dtModule.NewRow();
                        int TransferType = CommFunc.ConvertDBNullToInt32(drEsp["TransferType"]);
                        addDr["Ledger"] = drModule["Ledger"];
                        addDr["Co_id"] = CommFunc.ConvertDBNullToInt32(drModule["Co_id"]);/*房间ID*/
                        addDr["Module_id"] = drModule["Module_id"];/*设备ID*/
                        addDr["ModuleAddr"] = drModule["ModuleAddr"];/*设备地址*/
                        addDr["PortName"] = CommFunc.ConvertDBNullToString(drGw["GwIp"]) + ":" + CommFunc.ConvertDBNullToString(drEsp["EspPort"]) + (TransferType == 3 ? ":" + CommFunc.ConvertDBNullToString(drEsp["EspAddr"]) : "");/*PC端口*/ CommFunc.ConvertDBNullToString(drEsp["ComPort"]);/*PC端口*/
                        addDr["moduleName"] = drModule["ModuleAddr"];/*设备名称*/
                        addDr["CoFullName"] = drModule["CoFullName"]; ;/*房间全名*/
                        addDr["TransferType"] = TransferType;/*通信方式*/
                        addDr["Status"] = "刚运行未更新";/*设备状态*/
                        addDr["Update_dt"] = DateTime.Now;/*更新时间*/
                        addDr["Value"] = "";/*值*/
                        addDr["ViewTimes"] = 0;/*通信次数*/
                        addDr["SueTimes"] = 0;/*成功次数*/
                        addDr["SuePer"] = "";/*成功率*/
                        addDr["ErrCode"] = DBNull.Value;/*异常信息*/
                        if (TransferType == 0)
                            addDr["TransferTypeS"] = "COM";/*通信方式说明*/
                        else if (TransferType == 1)
                            addDr["TransferTypeS"] = "TCP/Client";/*通信方式说明*/
                        else if (TransferType == 2)
                            addDr["TransferTypeS"] = "UDP/Client";/*通信方式说明*/
                        else if (TransferType == 3)
                            addDr["TransferTypeS"] = "TCP/Servce";/*通信方式说明*/
                        else if (TransferType == 4)
                            addDr["TransferTypeS"] = "IOService";/*通信方式说明*/
                        dtModule.Rows.Add(addDr);
                        //通信类型=0 com口，=1 Tcp服务端，=2 Tcp客户端 =99 模拟器
                    }
                }
            }
            #endregion

            //
            this.treeView1.ExpandAll();
            this.treeView1.SelectedNode = node;
            //
            #endregion

            #endregion
            if (dtModule.Rows.Count > 500)
                this.isStop.Checked = true;
            this.checkBox1.Checked = false;          
            #region 加载完毕后，开启功能
            this.btnReload.Enabled = true;
            this.btnStart_Click(null, null);

            #endregion
        }

        private void Start()
        {
            NCSys.IsRun = true;
            this.btnStart.Enabled = false;
            this.btnEnd.Enabled = true;
            //DataTable dtSource = bll.GetArmData();
            DelegateARM AsyncOnArm = new DelegateARM(AutoARM); /*用于自动抄表*/
            //IAsyncResult AsyncResultOnArm = AsyncOnArm.BeginInvoke(dtSource, null, null);/*启动抄表线程*/
            IAsyncResult AsyncResultOnArm = AsyncOnArm.BeginInvoke(null, null);/*启动抄表线程*/
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            NCSys.IsRun = false;
            this.btnStart.Enabled = true;
            this.btnEnd.Enabled = false;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                NCSys.IsRun = false;
                NCSys.Reset = true;
                this.dtUpARM = DateTime.Now.AddDays(-100);
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.isShowList = !this.checkBox1.Checked;
        }

        private void btnLogHide_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
            this.isShowList = !this.splitContainer1.Panel2Collapsed;
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                this.listViewLog.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        DateTime dtUpARM = DateTime.Now.AddDays(-100);
        private void AutoARM()//DataTable dtSource
        {
            DateTime? dtStart = null;
            DateTime? dtIsWrDate = null;
            DataTable dtSource = null;
            while (true)
            {
                Thread.Sleep(1000);
                ////////////
                if (NCSys.IsRun == false) break;/*网关未启动*/
                bool running = (dtStart == null || DateTime.Now > dtStart.Value.AddMinutes(60)) ? true : false; /*15分钟没有运行过立刻再运行一次*/

                if (NCSys.BackgroundCount > 0 && running == false) continue; /*没有后台命令时才循环再发 /*防止过多的命令，导致内存不足*/

                if (NCSys.UIResult.Count >= 15000)
                {/*没有后台命令时才循环再发 /*防止过多的命令，导致内存不足*/
                    Thread.Sleep(1000 * 60 * 10);/*等10分钟*/
                }
                if (NCSys.UIResult.Count >= 5000)
                {/*没有后台命令时才循环再发 /*防止过多的命令，导致内存不足*/
                    continue;
                }
                try
                {
                    int frMd = 0;
                    if (dtSource != null)
                    {
                        foreach (DataRow dr in dtSource.Rows)
                        {
                            frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                            CommandVModel cmd = ModelHandler<CommandVModel>.FillModel(dr);
                            this.AddCmd(cmd);
                        }
                    }
                    frMd = frMd == 0 ? 15 : frMd;

                    #region 获取读写的数据
                    int r1 = dtIsWrDate == null ? 0 : CommFunc.ConvertDBNullToInt32(dtIsWrDate.Value.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                    int r2 = CommFunc.ConvertDBNullToInt32(DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                    bool isRun = (dtIsWrDate == null) ? true : false;
                    if (isRun == false)
                        if (DateTime.Now.Hour <= 4)
                            isRun = (r1 != r2) ? true : false;
                    //if (isRun == true && DateTime.Now.Hour <= 4)
                    if (isRun == true && 1 == 2)
                    {
                        DataTable dtWr = bll.GetWrData();
                        foreach (DataRow dr in dtWr.Rows)
                        {
                            CommandVModel cmd = ModelHandler<CommandVModel>.FillModel(dr);
                            this.AddCmd(cmd);
                        }
                        dtIsWrDate = DateTime.Now;
                    }
                    #endregion
                    #region 更新抄表数据
                    if (DateTime.Now >= dtUpARM.AddMinutes(frMd))
                    {
                        dtSource = bll.GetArmData();
                        dtUpARM = DateTime.Now;
                    }
                    #endregion
                    dtStart = DateTime.Now;
                }
                catch (Exception ex)
                {
                    FileLog.Error("自动抄表命令错误" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(1000 * 15);
            }
        }

        private void AddCmd(CommandVModel cmd)
        {
            if (string.IsNullOrEmpty(cmd.HandledBY))
            {
                FileLog.Error("队列键值为空" + JsonHelper.Serialize(cmd));
                return;
            }
            DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
            //if (cmd.Protocol.Trim().ToLower().Equals("modbus".ToLower()))
            //    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
            //else if (Config.Project.Trim().ToLower().Equals("YdDrive".ToLower()))
            //    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
            //else
            //    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                //FileLog.Info("抄表类型错误");
        }
        /// <summary>
        /// 显示结果
        /// </summary>
        private void ShowResult()
        {
            DateTime errTime = DateTime.Now;
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    this.UpdateAllStatus();
                    int uiCnt = NCSys.UIResult.Count;
                    for (int i = 0; i < uiCnt; i++)
                    {
                        if (i % 200 == 0)
                            Thread.Sleep(200);
                        else
                            Thread.Sleep(80);
                        CmdResult command = null;
                        if (NCSys.UIResult.TryDequeue(out command) == false) continue;
                        if (command == null) continue;
                        if (NCSys.IsRun == true)
                            this.Invoke(new ShowUI(UpdateList), command);
                    }
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog("错误信息:" + ex.Message + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// 更新内存的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="command"></param>
        /// <param name="msg"></param>
        private void UpdateList(CmdResult command)
        {
            if (command == null) return;
            DataRow curDr = null;
            #region 表头
            try
            {
                this.UpdateAllStatus();
                curDr = this.dtModule.Rows.Find(new object[] { command.Ledger, command.Module_id });
                if (curDr != null)
                {
                    string status = command.FunName; //"抄表";
                    int viewTimes = (CommFunc.ConvertDBNullToInt32(curDr["ViewTimes"]) + 1);
                    int sueTimes = (CommFunc.ConvertDBNullToInt32(curDr["SueTimes"]) + (command.ErrCode == 1 ? 1 : 0));

                    if (command.FunType.Equals(V0Fun.EventWpf.ToString()))
                    {
                        status = "恶性负载";
                        curDr["Update_dt"] = DateTime.Now;
                        curDr["Value"] = "";
                    }
                    else
                    {
                        status = command.FunType.Equals(V0Fun.E.ToString()) ? "抄表" : command.FunType.Equals(V0Fun.Ssr.ToString()) ? "拉合闸状态" : status;
                        status = string.IsNullOrEmpty(status) ? "抄表" : status;
                        curDr["Update_dt"] = command.Update;
                        curDr["Value"] = command.Value;
                    }
                    curDr["Status"] = status;
                    curDr["ViewTimes"] = viewTimes;
                    curDr["SueTimes"] = sueTimes;
                    curDr["ErrCode"] = command.ErrCode;
                    if (viewTimes == 0)
                        curDr["SuePer"] = "--";
                    else
                        curDr["SuePer"] = (((decimal)sueTimes / (decimal)viewTimes) * 100).ToString("f2");
                }
            }
            catch
            { /*重载时的错误，不处理*/
            }
            #endregion
            #region 明细
            if (this.isShowList == true && command != null)
            {
                try
                {/*清除时错误,不处理*/
                    if (this.listViewLog.Items.Count > 3000)
                    {
                        int rmCC = 2000;
                        while (rmCC > 0)
                        {
                            this.listViewLog.Items.RemoveAt(0);
                            rmCC = --rmCC;
                        }
                    }
                    //
                    if (!string.IsNullOrEmpty(this.txtModuleAddr.Text.Trim()))
                    {
                        if (!command.ModuleAddr.Trim().Equals(this.txtModuleAddr.Text.Trim()))
                            return;
                    }
                    else
                    {
                        #region 防止生产过快，消费过慢
                        if (NCSys.UIResult.Count > 5000)
                        {/*防止生产过快，消费过慢*/
                            this.checkBox1.Checked = true;
                        }
                        #endregion
                    }

                    if (command.Result.Count == 0)
                        this.listViewLog.Items.Add(new ListViewItem(new string[] { "电表", command.ModuleAddr, "", command.BiludBY, command.Update.ToString("yyyy-MM-dd HH:mm:ss fff"), command.ErrTxt }));
                    foreach (string s in command.Result)
                    {
                        this.listViewLog.Items.Add(new ListViewItem(s.Split(';').ToArray()));
                        Thread.Sleep(5);
                    }
                    //
                    int allCC = this.listViewLog.Items.Count;
                    if (allCC - 1 > 1)
                        this.listViewLog.EnsureVisible(allCC - 1);
                }
                catch
                { }
            }
            #endregion
        }

        private int sueMin = 0;
        private void UpdateAllStatus()
        {
            long total = NCSys.TotalCount;
            long err = NCSys.ErrCount;
            int bcc = NCSys.BackgroundCount;
            int uiTotal = NCSys.UIResult.Count;
            if (NCSys.BackgroundCount == 0)
            {
                DateTime la1 = string.IsNullOrEmpty(this.txtArmTime.Text) ? CommFunc.ConvertDBNullToDateTime(this.txtBeginTime.Text) : CommFunc.ConvertDBNullToDateTime(this.txtArmTime.Text);
                int aa = (int)(DateTime.Now - la1).TotalMinutes;
                if (aa >= 2 && aa <10000)
                {
                    sueMin = aa;
                    this.txtArmTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            if (uiTotal >= 5000)
                this.isStop.Checked = true;

            this.txtStatus.Text = "总采集数:" + NCSys.TotalCount + " 采集错误个数:" + NCSys.ErrCount;
            this.txtStatus.Text = this.txtStatus.Text + (sueMin == 0 ? "" : " 上次采集周期:" + sueMin + "分钟完成");
            this.txtStatus.Text = this.txtStatus.Text + (bcc == 0 ? "" : " 本次采集数:" + bcc);
            this.txtStatus.Text = this.txtStatus.Text + (uiTotal == 0 ? "" : " 结果缓存数:" + uiTotal);
        }

        private void ArmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedIndices(V0Fun.E.ToString());
            }
            catch(Exception ex)
            {
                FileLog.Error("下发指令错误:" + ex.Message + ex.StackTrace);
            }
        }
        private void Ssr0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedIndices(V0Fun.Ssr.ToString(), "0");
            }
            catch (Exception ex)
            {
                FileLog.Error("下发指令错误:" + ex.Message + ex.StackTrace);
            }
        }

        private void Ssr1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedIndices(V0Fun.Ssr.ToString(), "1");
            }
            catch (Exception ex)
            {
                FileLog.Error("下发指令错误:" + ex.Message + ex.StackTrace);
            }
        }

        private void SetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedIndices("SginSet");
            }
            catch (Exception ex)
            {
                FileLog.Error("下发指令错误:" + ex.Message + ex.StackTrace);
            }
        }

        private void SelectedIndices(string func,string dataValue = "")
        {
            if (this.btnStart.Enabled == true)
            {
                MessageBox.Show("请先启动数据网关", "告警", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Control cc = contextMenuStrip1.SourceControl;
            if ((cc as DataGridView) != null)
            {
                this.SetSelectedIndicesByModule_id(func, dataValue);
            }
            else if ((cc as TreeView) != null)
            {
                this.SetSelectedIndicesByZongXian(func);
            }
        }

        private void SetSelectedIndicesByModule_id(string func, string dataValue)
        {
            foreach (DataGridViewRow dgvRow in this.dgv1.SelectedRows)
            {
                int index = dgv1.Rows.IndexOf(dgvRow);
                DataRow currDr = (dgv1.Rows[index].DataBoundItem as DataRowView).Row;
                int ledger = CommFunc.ConvertDBNullToInt32(currDr["Ledger"]);
                int module_id = CommFunc.ConvertDBNullToInt32(currDr["Module_id"]);
                DataTable dtSource = bll.GetCurData(ledger,module_id, func.ToString(), dataValue);
                if (dtSource.Rows.Count == 0)
                {
                    MessageBox.Show("没有命令记录", "告警", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }
                this.SetSelectedIndices(dtSource);
            }
        }

        private void SetSelectedIndicesByZongXian(string Func)
        {
            TreeNode node = this.treeView1.SelectedNode;
            string tt = node.Name;
            string[] a = tt.Split(':');
            //int gw_id = 0, esp_id = 0;
            //DataTable dtSource = null;
            //if (a.Length == 2)
            //{
            //    if (a[0].Trim() == "GW")
            //    {
            //        gw_id = CommFunc.ConvertDBNullToInt32(a[1]);
            //        dtSource = bll.GetModuleInfoByGw_id(gw_id);
            //    }
            //    else if (a[0].Trim() == "ESP")
            //    {
            //        esp_id = CommFunc.ConvertDBNullToInt32(a[1]);
            //        dtSource = bll.GetModuleInfoByEsp_id(esp_id);
            //    }
            //}
            //else
            //{
            //    dtSource = bll.GetModuleInfo();
            //}
            //this.SetSelectedIndices(dtSource, Func);
        }

        private void SetSelectedIndices(DataTable dtSource)
        {
            if (dtSource == null) return;
            //
            foreach (DataRow dr in dtSource.Rows)
            {
                string funType = CommFunc.ConvertDBNullToString(dr["FunType"]);
                int action = CommFunc.ConvertDBNullToInt32(dr["Action"]);
                CommandVModel cmd = ModelHandler<CommandVModel>.FillModel(dr);
                cmd.IsUI = true;
                cmd.IsNDb = funType.Equals(V0Fun.E.ToString()) ? false : true;
                cmd.IsNDb = action == 1 ? true : cmd.IsNDb;
                DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
            }
        }

        private void btnCfg_Click(object sender, EventArgs e)
        {
            CheckPwd form1 = new CheckPwd();
            if (form1.ShowDialog() == DialogResult.OK)
            {
                FormCfg form = new FormCfg();
                form.ShowDialog();
            }
            else
            {
                return;
            }     
        }


        private void dgv1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                DataRow currDr = (dgv1.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                if (currDr != null)
                {
                    if (currDr["errCode"] != DBNull.Value && CommFunc.ConvertDBNullToInt32(currDr["ErrCode"]) != 1)
                        this.dgv1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    else
                        this.dgv1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            catch
            { }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void isStop_CheckedChanged(object sender, EventArgs e)
        {
            NCSys.IsUIResult = !this.isStop.Checked;
        }
    }
}
