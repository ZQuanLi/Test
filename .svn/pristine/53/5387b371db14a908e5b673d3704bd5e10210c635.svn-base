using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YDS6000.Models;

namespace DataProcess
{
    public partial class FormCfg : Form
    {
        public FormCfg()
        {
            InitializeComponent();
        }

        private void FormCfg_Load(object sender, EventArgs e)
        {
            this.bind();
            int isAlarm = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "isAlarm"));
            this.IsAlarm.Checked = isAlarm == 1 ? true : false;
            this.txtHour.Value = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "hour"));
            this.txtPortName.Value = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "portName"));
            this.cbBaudRate.Text = IniHepler.GetConfig(Config.lpFileName, "Alarm", "baudRate");
            this.cbDataBits.Text = IniHepler.GetConfig(Config.lpFileName, "Alarm", "databits");
            this.cbStopBits.Text = IniHepler.GetConfig(Config.lpFileName, "Alarm", "stopBits");
            this.cbParity.SelectedValue = CommFunc.ConvertDBNullToInt32(IniHepler.GetConfig(Config.lpFileName, "Alarm", "parity"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定更改保存吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                return;
            int isAlarm = this.IsAlarm.Checked == true ? 1 : 0;
            int hour = CommFunc.ConvertDBNullToInt32(this.txtHour.Value);
            int portName = CommFunc.ConvertDBNullToInt32(this.txtPortName.Value);
            int baudRate = CommFunc.ConvertDBNullToInt32(this.cbBaudRate.Text);
            int databits = CommFunc.ConvertDBNullToInt32(this.cbDataBits.Text);
            int stopBits = CommFunc.ConvertDBNullToInt32(this.cbStopBits.Text);
            int parity = CommFunc.ConvertDBNullToInt32(this.cbParity.SelectedValue);
            IniHepler.SetConfig("Alarm", "isAlarm", isAlarm.ToString(), Config.lpFileName);
            IniHepler.SetConfig("Alarm", "hour", hour.ToString(), Config.lpFileName);
            IniHepler.SetConfig("Alarm", "portName", portName.ToString(), Config.lpFileName);
            IniHepler.SetConfig("Alarm", "baudRate", baudRate.ToString(), Config.lpFileName);
            IniHepler.SetConfig("Alarm", "databits", databits.ToString(), Config.lpFileName);
            IniHepler.SetConfig("Alarm", "stopBits", stopBits.ToString(), Config.lpFileName);
            IniHepler.SetConfig("Alarm", "parity", parity.ToString(), Config.lpFileName);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void bind()
        {
            this.cbBaudRate.Items.Add(600);
            this.cbBaudRate.Items.Add(1200);
            this.cbBaudRate.Items.Add(2400);
            this.cbBaudRate.Items.Add(4800);
            this.cbBaudRate.Items.Add(9600);
            this.cbBaudRate.Items.Add(14400);
            this.cbBaudRate.Items.Add(19200);
            this.cbBaudRate.Items.Add(33600);
            //
            this.cbDataBits.Items.Add(5);
            this.cbDataBits.Items.Add(6);
            this.cbDataBits.Items.Add(7);
            this.cbDataBits.Items.Add(8);
            //
            this.cbStopBits.Items.Add(1);
            this.cbStopBits.Items.Add(2);
            this.cbStopBits.Items.Add(3);
            //   
            Dictionary<int, string> dicParity = new Dictionary<int, string>();
            dicParity.Add(0, "N 无");
            dicParity.Add(1, "O 奇");
            dicParity.Add(2, "E 偶");
            dicParity.Add(3, "M 标志");
            dicParity.Add(4, "S 空格");
            //
            BindingSource bs0 = new BindingSource();
            bs0.DataSource = dicParity;
            this.cbParity.DataSource = bs0;
            cbParity.DisplayMember = "Value";
            cbParity.ValueMember = "Key";


        }

    }
}
