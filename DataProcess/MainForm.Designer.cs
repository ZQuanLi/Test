namespace DataProcess
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.isStop = new System.Windows.Forms.CheckBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnCfg = new System.Windows.Forms.Button();
            this.btnLogHide = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.portName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleNode1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferType1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update_dt1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viewTimes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sueTimes1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.suePer1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtModuleAddr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.module2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeA2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeB2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeC2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.runTime2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rmk2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ArmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Ssr1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Ssr0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBeginTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArmTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "数据处理程序";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.isStop);
            this.panel1.Controls.Add(this.btnClearLog);
            this.panel1.Controls.Add(this.btnCfg);
            this.panel1.Controls.Add(this.btnLogHide);
            this.panel1.Controls.Add(this.btnReload);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 43);
            this.panel1.TabIndex = 14;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // isStop
            // 
            this.isStop.AutoSize = true;
            this.isStop.Location = new System.Drawing.Point(288, 17);
            this.isStop.Name = "isStop";
            this.isStop.Size = new System.Drawing.Size(96, 16);
            this.isStop.TabIndex = 10;
            this.isStop.Text = "停止缓存数据";
            this.isStop.UseVisualStyleBackColor = true;
            this.isStop.CheckedChanged += new System.EventHandler(this.isStop_CheckedChanged);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(799, 14);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 9;
            this.btnClearLog.Text = "清除Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnCfg
            // 
            this.btnCfg.Location = new System.Drawing.Point(891, 13);
            this.btnCfg.Name = "btnCfg";
            this.btnCfg.Size = new System.Drawing.Size(75, 23);
            this.btnCfg.TabIndex = 8;
            this.btnCfg.Text = "参数配置";
            this.btnCfg.UseVisualStyleBackColor = true;
            this.btnCfg.Click += new System.EventHandler(this.btnCfg_Click);
            // 
            // btnLogHide
            // 
            this.btnLogHide.Location = new System.Drawing.Point(678, 14);
            this.btnLogHide.Name = "btnLogHide";
            this.btnLogHide.Size = new System.Drawing.Size(104, 23);
            this.btnLogHide.TabIndex = 7;
            this.btnLogHide.Text = "隐藏显示Log";
            this.btnLogHide.UseVisualStyleBackColor = true;
            this.btnLogHide.Click += new System.EventHandler(this.btnLogHide_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(183, 14);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "重新加载";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(414, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "停止刷新";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(4, 14);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(85, 14);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 23);
            this.btnEnd.TabIndex = 0;
            this.btnEnd.Text = "停止";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 45);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtModuleAddr);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.listViewLog);
            this.splitContainer1.Size = new System.Drawing.Size(1127, 616);
            this.splitContainer1.SplitterDistance = 341;
            this.splitContainer1.TabIndex = 15;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgv1);
            this.splitContainer2.Size = new System.Drawing.Size(1127, 341);
            this.splitContainer2.SplitterDistance = 194;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(194, 341);
            this.treeView1.TabIndex = 0;
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToResizeRows = false;
            this.dgv1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.portName1,
            this.moduleName1,
            this.moduleNode1,
            this.TransferType1,
            this.status1,
            this.update_dt1,
            this.value1,
            this.viewTimes1,
            this.sueTimes1,
            this.suePer1});
            this.dgv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv1.GridColor = System.Drawing.SystemColors.Window;
            this.dgv1.Location = new System.Drawing.Point(0, 0);
            this.dgv1.Name = "dgv1";
            this.dgv1.RowHeadersVisible = false;
            this.dgv1.RowHeadersWidth = 25;
            this.dgv1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgv1.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv1.RowTemplate.Height = 18;
            this.dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv1.Size = new System.Drawing.Size(929, 341);
            this.dgv1.TabIndex = 0;
            this.dgv1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgv1_RowPrePaint);
            // 
            // portName1
            // 
            this.portName1.DataPropertyName = "PortName";
            this.portName1.HeaderText = "通信参数";
            this.portName1.Name = "portName1";
            this.portName1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.portName1.Width = 130;
            // 
            // moduleName1
            // 
            this.moduleName1.DataPropertyName = "ModuleName";
            this.moduleName1.HeaderText = "设备地址";
            this.moduleName1.Name = "moduleName1";
            this.moduleName1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.moduleName1.Width = 110;
            // 
            // moduleNode1
            // 
            this.moduleNode1.DataPropertyName = "CoFullName";
            this.moduleNode1.HeaderText = "房间名";
            this.moduleNode1.Name = "moduleNode1";
            this.moduleNode1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.moduleNode1.Width = 180;
            // 
            // TransferType1
            // 
            this.TransferType1.DataPropertyName = "TransferTypeS";
            this.TransferType1.HeaderText = "通信方式";
            this.TransferType1.Name = "TransferType1";
            this.TransferType1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TransferType1.Width = 70;
            // 
            // status1
            // 
            this.status1.DataPropertyName = "Status";
            this.status1.HeaderText = "设备状态";
            this.status1.Name = "status1";
            this.status1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.status1.Width = 90;
            // 
            // update_dt1
            // 
            this.update_dt1.DataPropertyName = "Update_dt";
            this.update_dt1.HeaderText = "状态更新时间";
            this.update_dt1.Name = "update_dt1";
            this.update_dt1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.update_dt1.Width = 130;
            // 
            // value1
            // 
            this.value1.DataPropertyName = "Value";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.value1.DefaultCellStyle = dataGridViewCellStyle2;
            this.value1.HeaderText = "值";
            this.value1.Name = "value1";
            this.value1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.value1.Width = 80;
            // 
            // viewTimes1
            // 
            this.viewTimes1.DataPropertyName = "ViewTimes";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.viewTimes1.DefaultCellStyle = dataGridViewCellStyle3;
            this.viewTimes1.HeaderText = "通信次数";
            this.viewTimes1.Name = "viewTimes1";
            this.viewTimes1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.viewTimes1.Width = 90;
            // 
            // sueTimes1
            // 
            this.sueTimes1.DataPropertyName = "SueTimes";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sueTimes1.DefaultCellStyle = dataGridViewCellStyle4;
            this.sueTimes1.HeaderText = "成功次数";
            this.sueTimes1.Name = "sueTimes1";
            this.sueTimes1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sueTimes1.Width = 90;
            // 
            // suePer1
            // 
            this.suePer1.DataPropertyName = "SuePer";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.suePer1.DefaultCellStyle = dataGridViewCellStyle5;
            this.suePer1.HeaderText = "成功率";
            this.suePer1.Name = "suePer1";
            this.suePer1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.suePer1.Width = 70;
            // 
            // txtModuleAddr
            // 
            this.txtModuleAddr.Location = new System.Drawing.Point(79, 9);
            this.txtModuleAddr.Name = "txtModuleAddr";
            this.txtModuleAddr.Size = new System.Drawing.Size(123, 21);
            this.txtModuleAddr.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(208, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "查询日志（输入后请按回车键）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "按设备地址";
            // 
            // listViewLog
            // 
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.module2,
            this.typeA2,
            this.typeB2,
            this.typeC2,
            this.runTime2,
            this.rmk2});
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.Location = new System.Drawing.Point(0, 34);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(1124, 232);
            this.listViewLog.TabIndex = 2;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // module2
            // 
            this.module2.Tag = "module2";
            this.module2.Text = "模块";
            this.module2.Width = 115;
            // 
            // typeA2
            // 
            this.typeA2.Tag = "typeA2";
            this.typeA2.Text = "类型1";
            this.typeA2.Width = 115;
            // 
            // typeB2
            // 
            this.typeB2.Tag = "typeB2";
            this.typeB2.Text = "类型2";
            this.typeB2.Width = 115;
            // 
            // typeC2
            // 
            this.typeC2.Tag = "typeC2";
            this.typeC2.Text = "类型3";
            this.typeC2.Width = 200;
            // 
            // runTime2
            // 
            this.runTime2.Tag = "runTime2";
            this.runTime2.Text = "时间";
            this.runTime2.Width = 160;
            // 
            // rmk2
            // 
            this.rmk2.Tag = "rmk2";
            this.rmk2.Text = "内容";
            this.rmk2.Width = 800;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ArmToolStripMenuItem,
            this.Ssr1ToolStripMenuItem,
            this.Ssr0ToolStripMenuItem,
            this.SetInfoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 92);
            // 
            // ArmToolStripMenuItem
            // 
            this.ArmToolStripMenuItem.Name = "ArmToolStripMenuItem";
            this.ArmToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ArmToolStripMenuItem.Text = "测试电表通信";
            this.ArmToolStripMenuItem.Click += new System.EventHandler(this.ArmToolStripMenuItem_Click);
            // 
            // Ssr1ToolStripMenuItem
            // 
            this.Ssr1ToolStripMenuItem.Name = "Ssr1ToolStripMenuItem";
            this.Ssr1ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.Ssr1ToolStripMenuItem.Text = "拉闸";
            this.Ssr1ToolStripMenuItem.Click += new System.EventHandler(this.Ssr1ToolStripMenuItem_Click);
            // 
            // Ssr0ToolStripMenuItem
            // 
            this.Ssr0ToolStripMenuItem.Name = "Ssr0ToolStripMenuItem";
            this.Ssr0ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.Ssr0ToolStripMenuItem.Text = "合闸";
            this.Ssr0ToolStripMenuItem.Click += new System.EventHandler(this.Ssr0ToolStripMenuItem_Click);
            // 
            // SetInfoToolStripMenuItem
            // 
            this.SetInfoToolStripMenuItem.Name = "SetInfoToolStripMenuItem";
            this.SetInfoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.SetInfoToolStripMenuItem.Text = "设置电表参数";
            this.SetInfoToolStripMenuItem.Click += new System.EventHandler(this.SetToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 675);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "开始运行时间";
            // 
            // txtBeginTime
            // 
            this.txtBeginTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBeginTime.Location = new System.Drawing.Point(81, 671);
            this.txtBeginTime.Name = "txtBeginTime";
            this.txtBeginTime.ReadOnly = true;
            this.txtBeginTime.Size = new System.Drawing.Size(122, 21);
            this.txtBeginTime.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(212, 675);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "抄表完成时间";
            // 
            // txtArmTime
            // 
            this.txtArmTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtArmTime.Location = new System.Drawing.Point(288, 671);
            this.txtArmTime.Name = "txtArmTime";
            this.txtArmTime.ReadOnly = true;
            this.txtArmTime.Size = new System.Drawing.Size(127, 21);
            this.txtArmTime.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(431, 675);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "运行状况:";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtStatus.AutoSize = true;
            this.txtStatus.Location = new System.Drawing.Point(490, 676);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(41, 12);
            this.txtStatus.TabIndex = 21;
            this.txtStatus.Text = "label6";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 696);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtArmTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBeginTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据网关V3.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnCfg;
        private System.Windows.Forms.Button btnLogHide;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.TextBox txtModuleAddr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.ColumnHeader module2;
        private System.Windows.Forms.ColumnHeader typeA2;
        private System.Windows.Forms.ColumnHeader typeB2;
        private System.Windows.Forms.ColumnHeader typeC2;
        private System.Windows.Forms.ColumnHeader runTime2;
        private System.Windows.Forms.ColumnHeader rmk2;
        private System.Windows.Forms.DataGridViewTextBoxColumn portName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduleNode1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferType1;
        private System.Windows.Forms.DataGridViewTextBoxColumn status1;
        private System.Windows.Forms.DataGridViewTextBoxColumn update_dt1;
        private System.Windows.Forms.DataGridViewTextBoxColumn value1;
        private System.Windows.Forms.DataGridViewTextBoxColumn viewTimes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sueTimes1;
        private System.Windows.Forms.DataGridViewTextBoxColumn suePer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ArmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Ssr1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Ssr0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetInfoToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBeginTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtArmTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtStatus;
        private System.Windows.Forms.CheckBox isStop;
    }
}

