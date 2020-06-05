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
    public partial class FormAKey : Form
    {
        public FormAKey()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string AppCode = txtAppCode.Text;/*机器码*/
            string sn = txtSn.Text;/*注册码*/
            string dsn = txtDsn.Text;/*有限期*/
            if (string.IsNullOrEmpty(sn.Trim()))
            {
                MessageBox.Show("注册码不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtSn.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dsn.Trim()))
            {
                MessageBox.Show("有效期不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtSn.Focus();
                return;
            }

            string EncCode = ConfigHelper.Encrypt(AppCode);
            string dd = ConfigHelper.Decrypt(dsn);
            DateTime dt;
            bool isPass = DateTime.TryParse(dd, out dt);
            if (!EncCode.Trim().Equals(sn.Trim()))
            {
                MessageBox.Show("注册码认证失败", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isPass == true)
            {
                if (dt < DateTime.Now)
                {
                    MessageBox.Show("有效期过期", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("有效期认证失败", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                ConfigHelper.SetAppSettings("sn", sn);
                ConfigHelper.SetAppSettings("dsn", dsn);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置错误:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private void FormAKey_Load(object sender, EventArgs e)
        {
            txtAppCode.Text = ConfigHelper.GetComputerSn();
            txtSn.Text = ConfigHelper.GetAppSettings("sn");
            txtDsn.Text = ConfigHelper.GetAppSettings("dsn");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
