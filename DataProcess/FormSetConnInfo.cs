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
    public partial class FormSetConnInfo : Form
    {
        public FormSetConnInfo()
        {
            InitializeComponent();
            this.button1.Enabled = false;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string HOST = this.txtSource.Text.Trim();
            string DBUID = this.txtUid.Text.Trim();
            string DBPWD = this.txtPasswd.Text.Trim();
            string DBNAME = this.txtDbName.Text.Trim();
            if (string.IsNullOrEmpty(HOST))
            {
                MessageBox.Show("请输入数据源", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtSource.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBUID))
            {
                MessageBox.Show("请输入用户名", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtUid.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBPWD))
            {
                MessageBox.Show("请输入密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtPasswd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBNAME))
            {
                MessageBox.Show("请输入数据库实例", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtDbName.Focus();
                return;
            }
            //
            try
            {
                string connect = ConfigHelper.GetConnectionStrings("DefaultConnection");
                IniHepler.SetConfig("Db", "DbHost", HOST, Config.lpFileName);
                IniHepler.SetConfig("Db", "DbName", DBNAME, Config.lpFileName);
                IniHepler.SetConfig("Db", "DbUid", DBUID, Config.lpFileName);
                IniHepler.SetConfig("Db", "DbPwd", ConfigHelper.Encrypt(DBPWD), Config.lpFileName);
                connect = string.Format(connect, HOST, DBNAME, DBUID, DBPWD);
                YDS6000.BLL.WholeBLL.ConnectionString(connect);
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新错误" + ex.Message);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonTestConn_Click(object sender, EventArgs e)
        {
            string HOST = this.txtSource.Text.Trim();
            string DBUID = this.txtUid.Text.Trim();
            string DBPWD = this.txtPasswd.Text.Trim();
            string DBNAME = this.txtDbName.Text.Trim();
            if (string.IsNullOrEmpty(HOST))
            {
                MessageBox.Show("请输入数据源", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtSource.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBUID))
            {
                MessageBox.Show("请输入用户名", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtUid.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBPWD))
            {
                MessageBox.Show("请输入密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtPasswd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBNAME))
            {
                MessageBox.Show("请输入数据库实例", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.txtDbName.Focus();
                return;
            }
            this.button1.Enabled = false;
            bool connService = false, connDb = false;
            try
            {
                connService = YDS6000.BLL.WholeBLL.ConneectingServices(HOST, "information_schema", DBUID, DBPWD);
                if (connService == true)
                {
                    connDb = YDS6000.BLL.WholeBLL.ConneectingServices(HOST, DBNAME, DBUID, DBPWD);
                }
                MessageBox.Show("连接服务器成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (connService == false)
                {
                    MessageBox.Show("连接服务器错误"+ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (connDb == false)
                {
                    MessageBox.Show("连接服务器成功" + System.Environment.NewLine + "但连接服务器下的实例" + DBNAME + "错误:" + ex.Message,
                        "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.button1.Enabled = true;
                    return;
                }
            }
        }

        private void FormSetConnInfo_Load(object sender, EventArgs e)
        {
            this.txtSource.Text = IniHepler.GetConfig(Config.lpFileName, "Db", "DbHost");
            this.txtUid.Text = IniHepler.GetConfig(Config.lpFileName, "Db", "DbUid");
            this.txtPasswd.Text = ConfigHelper.Decrypt(IniHepler.GetConfig(Config.lpFileName, "Db", "DBPwd"));
            this.txtDbName.Text = IniHepler.GetConfig(Config.lpFileName, "Db", "DbName");
        }

        private int iRtyTime = 10;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                timer1.Enabled = false;
                checkBox1.Text = string.Format("手动点击[退出]以运行系统");
                return;
            }

            if (iRtyTime == 0)
            {
                timer1.Enabled = false;
                this.buttonOK_Click(sender, e);
                DialogResult = DialogResult.OK;
            }
            iRtyTime--;
            checkBox1.Text = string.Format("{0}秒后自动运行", 
                iRtyTime);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定初始化数据库实例吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != System.Windows.Forms.DialogResult.Yes)
                return;
            string HOST = this.txtSource.Text.Trim();
            string DBUID = this.txtUid.Text.Trim();
            string DBPWD = this.txtPasswd.Text.Trim();
            string DBNAME = this.txtDbName.Text.Trim();
            if (string.IsNullOrEmpty(HOST))
            {
                MessageBox.Show("请输入数据源", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtSource.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBUID))
            {
                MessageBox.Show("请输入用户名", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtUid.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBPWD))
            {
                MessageBox.Show("请输入密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtPasswd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DBNAME))
            {
                MessageBox.Show("请输入数据库实例", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtDbName.Focus();
                return;
            }
            //
            try
            {
                //if (WholeBLL.Db_Exis(HOST, DBNAME, DBUID, DBPWD) == true)
                //{
                //    if (MessageBox.Show("数据库实例:" + DBNAME + "已存在,是否覆盖旧实例创建新实例？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != System.Windows.Forms.DialogResult.Yes)
                //        return ;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息:"+ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                //WholeBLL.DbCreate(HOST, DBNAME, DBUID, DBPWD);
                //MessageBox.Show("初始化成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void txtDbName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
