using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder fcode = new StringBuilder(500);
            //string fcode = "";
            bool rst = dllHepler.GetSerialNumber(fcode, fcode.Capacity);
            if (rst == true)
            {
                this.textBox1.Text = fcode.ToString();
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show("错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder fcode = new StringBuilder();
            fcode.Append(this.textBox1.Text);
            if (string.IsNullOrEmpty(fcode.ToString()))
                MessageBox.Show("请先输入注册码");
            bool rst = dllHepler.RegisterDll(fcode);
            if (rst == true)
                MessageBox.Show("成功");
            else
                MessageBox.Show("错误");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime mTime = DateTime.Now;
            byte[] data = new byte[16];
            bool rst = dllHepler.f_des_encryptByTimeEle(mTime.Year, mTime.Month, mTime.Day, mTime.Hour, mTime.Minute, mTime.Second, (float)1.2, data);
            if (rst == true)
            {
                this.textBox1.Text = BitConverter.ToString(data);
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show("错误");
            }
        }
    }
}
