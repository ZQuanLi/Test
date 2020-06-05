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
    public partial class CheckPwd : Form
    {
        public CheckPwd()
        {
            InitializeComponent();
        }
        private static YDS6000.BLL.DataProcess.MainFormBLL bll = new YDS6000.BLL.DataProcess.MainFormBLL(Config.Systems);
        public void Login()
        {
            string uid = this.txtName.Text.Trim();
            string passwd1 = this.txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(uid))
            {

                MessageBox.Show("请输入用户名");
                this.txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(passwd1))
            {
                MessageBox.Show("请输入密码");
                this.txtPassword.Focus();
                return;
            }
            //
            try
            {
                string name = "", passwd2 = "";
                foreach (var s1 in NCSys.Pro)
                {
                    DataTable dt = bll.GetSys_user(s1.Key,uid);
                    if (dt.Rows.Count > 0)
                    {
                        name = CommFunc.ConvertDBNullToString(dt.Rows[0]["USign"]).Trim();
                        passwd2 = CommFunc.ConvertDBNullToString(dt.Rows[0]["UPasswd"]).Trim();
                        if (passwd1.Trim().Equals(passwd2.Trim()))
                            break;
                    }
                }                                
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("用户名不存在");
                    this.txtName.SelectAll();
                    this.txtPassword.Focus();
                    return;
                }
                if (!passwd1.Trim().Equals(passwd2.Trim()))
                {
                    MessageBox.Show("密码错误");
                    this.txtPassword.SelectAll();
                    this.txtPassword.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息:" + ex.Message);
                return;
            }

            if (txtName.Text == uid && txtPassword.Text == passwd1)
            {
                MessageBox.Show("管理员密码验证成功！", "提示");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("请输入管理员用户名和密码进行验证", "提示");
                return;
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }
        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtPassword.Focus();
        }
        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.Focus();
        }
        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Login();
            }
        }
    }
}
