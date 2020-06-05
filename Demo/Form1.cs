using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YDS6000.Models;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Collections.Concurrent;

namespace Demo
{
    public partial class Form1 : Form
    {

        public static Form1 mainForm = null;
        public Form1()
        {           
            InitializeComponent();
            mainForm = this;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = mainForm.txtIp.Text;
            //sys_user user = new sys_user();
            //user.UName = "";
            //string err = CheckAttribute.CheckField(user);
            ProcessSevice.Start(new DataProcess.ICEFactory(), 5823);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string aaa = "6.R1B1.2.4.ⅡE.E";
                string[] arr = aaa.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string ns = string.Join(".", arr,0, arr.Length - 1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string key = this.textBox1.Text.Trim();
            RstVar ss = new RstVar();
            ss.lpszdateTime = DateTime.Now;
            ss.lpszVal = this.textBox2.Text.Trim();
            //string ss = this.textBox2.Text.Trim(); 
            //bool last = MemcachedMgr.SetVal(key, this.textBox2.Text.Trim());
            bool last = MemcachedMgr.SetVal(key, ss);
            if (last == true)
            {
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show("失败");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string key = this.textBox1.Text.Trim();
            //object ss1 = MemcachedMgr.GetVal(key);
            //this.textBox2.Text = ss1 == null ? "无值": ss1.ToString();
            //return;
            RstVar ss = MemcachedMgr.GetVal<RstVar>(key);
            if (ss != null)
            {
                this.textBox2.Text = "时间:" + ss.lpszdateTime.ToString();
                this.textBox2.Text = this.textBox2.Text + Environment.NewLine + "值:" + ss.lpszVal;
                //this.textBox2.Text = ss.lpszVal;
            }
            else
            {
                //object obj = MemcachedMgr.GetVal(key);
                //if (obj != null)
                //    this.textBox2.Text = obj.ToString();
                //else
                    MessageBox.Show("获取值失败");

            }
        }

        private static long log_id = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            v2_alarm_logVModel cc = new v2_alarm_logVModel() { Log_id = ++log_id, Module_id = (int)log_id, ModuleAddr = log_id.ToString() };
            object dy = new { cc = cc, time = DateTime.Now, count = 0 };
            m2mqtt.TryAdd(log_id, dy);
            return;
            decimal value = (decimal)9.82;
            //string bbbbb = System.Convert.ToString(aaaaa, 16);
            string str = ((ulong)(value * 100)).ToString("x").Trim();
            byte[] ele = DirectToByte(str, 2);
            byte[] tes = new byte[] { 0x03, 0xd6 };

            byte[] aa = new byte[] { 0x03, 0xD6 };
            return;
            Sms.sendSms();
            return;
            byte a = 0x01;
            byte b = 0x18;
            int k = a;
            k = k << 8;
            k = k + b;
            MessageBox.Show(k.ToString());
            return;
            List<dynamic> dd = new List<dynamic>();
            dd.Add(new { mp = "123456", name = "dfadf" });
            dd.Add(new { mp = "79564", name = "test" });
            foreach (var s in dd)
            {
                string mp = s.mp;
                string name = s.name;

            }
            //MessageBox.Show((Math.Round(17.0 / 15.0,2,MidpointRounding.AwayFromZero)).ToString());
        }
        /// <summary>
        /// 然后直接按两位一组截取字符串转成byte数组
        /// </summary>
        /// <param name="meterID">如电表地址,电表密码,操作者</param>
        /// <returns>byte数组码</returns>
        internal static byte[] DirectToByte(string value, int len)
        {
            StringBuilder str = new StringBuilder(len * 2);
            str.Append(value.ToString().Trim().PadLeft(len * 2, '0'));
            string[] arr = System.Text.RegularExpressions.Regex.Split(str.ToString().Trim(), "(?<=\\G.{2})(?!$)");/*平均两位截取字符串*/
            return arr.Zip(arr, (x, y) => (byte)Convert.ToInt16(x, 16)).ToArray();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte a = 0x03, b = 0x33;
            byte c = (byte)(a - b);

            byte[] data = new byte[] { 0x03, 0x33 };
            string str = ConvertDataToStringBy16(data);
            int x = Convert.ToInt32(str, 16);
            string value = Convert.ToString(c, 2);
            int ssr4 = 0, cmd6 = 0;
            int len = value.Length;
            ssr4 = Convert.ToInt32(value.Substring(len - 5).Substring(0, 1));
            cmd6 = Convert.ToInt32(value.Substring(len - 7).Substring(0, 1));

            string str11 = ConvertDataToString(data);
            int x11 = Convert.ToInt32(str11, 16);
            string value11 = Convert.ToString(x11, 2);
            int ssr4_11 = 0, cmd6_11 = 0;
            int len_11 = value.Length;
            ssr4_11 = Convert.ToInt32(value11.Substring(len_11 - 5).Substring(0, 1));
            cmd6_11 = Convert.ToInt32(value11.Substring(len_11 - 7).Substring(0, 1));
        }

        internal static string ConvertDataToStringBy16(byte[] data)
        {
            data = data.ToArray().Zip(data, (x, y) => (byte)(x - 0x33)).Reverse().ToArray();
            StringBuilder strRtn = new StringBuilder();
            data.ToList().ForEach(x => strRtn.Append(x.ToString().Trim().PadLeft(2, '0')));
            return strRtn.ToString().Trim();
        }

        /// <summary>
        /// 把一个数据域转换BCD码的字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string ConvertDataToString(byte[] data)
        {
            data = data.ToArray().Zip(data, (x, y) => BCD2hex((byte)(x - 0x33))).Reverse().ToArray();
            StringBuilder strRtn = new StringBuilder();
            data.ToList().ForEach(x => strRtn.Append(x.ToString().Trim().PadLeft(2, '0')));
            return strRtn.ToString().Trim();
        }
        internal static byte BCD2hex(byte bcd)
        {
            byte i = (byte)(bcd & 0x0f); //按位与，i得到低四位数。
            bcd >>= 4; //右移四位，将高四位移到低四位的位置，得到高四位码值。
            bcd &= 0x0f; //防止移位时高位补进1，只保留高四位码值
            bcd *= 10; //高位码值乘以10
            i += bcd; //然后与第四位码值相加。
            return i; //将得到的十进制数返回
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("请输入变量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> list = new List<string>();
            list.Add(this.textBox1.Text);
            DataProcess.IOService.Collection.CollectionHelper.Instance(list);
        }

        public class aa
        {
            public int error { get; set; }
            public string message { get; set; }
            public dynamic data { get; set; }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new m2mqtt().Subscribe();
            return;
            aa bb = new aa();
            bb.error = 0;
            bb.message = "login";
            bb.data = new
            {
                accessToken = "asdf2f2545asfegycaetvA",
                optResult = 0
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            string sss = js.Serialize(bb);
            aa a = js.Deserialize<aa>(sss);
            return;

            //decimal mElectric = (decimal)1.2;
            decimal mElectric = (decimal)120.26;
            DateTime mTime = DateTime.Now;
            DateTime.TryParse("2019-10-10 14:25:36", out mTime);
            Random random = new Random();
            uint electric = (uint)(mElectric * 100);
            int second = callCFun.u32CalTotalSecond(mTime.Year, mTime.Month, mTime.Day, mTime.Hour, mTime.Minute, mTime.Second);
            long lsed = 1000;// random.Next(100, 10000);
            byte[] ele = BitConverter.GetBytes(electric);/*电能*/
            byte[] sed = BitConverter.GetBytes(second);/*时间*/
            byte[] pi_buf = ele.Concat(sed).ToArray();/*电能加时间*/
            byte[] ks_buf = BitConverter.GetBytes(lsed);/*密钥*/
            byte[] co_buf = new byte[8];
            callCFun.f_des_encrypt(pi_buf, ks_buf, co_buf);
            byte[] data = co_buf.Concat(ks_buf).ToArray();

            StringBuilder  fcode = new StringBuilder(500);
            //string fcode = "";
            bool isNum = dllHepler.GetSerialNumber(fcode, fcode.Capacity);
            StringBuilder ff = new StringBuilder();
            ff.Append("028C19397341BB21F57967FA1E4E4C5D");
            bool iscc = dllHepler.RegisterDll(ff);
            byte[] data1 = new byte[16];
            bool rst = dllHepler.f_des_encryptByTimeEle(mTime.Year, mTime.Month, mTime.Day, mTime.Hour, mTime.Minute, mTime.Second, (float)mElectric, data1);
        }

        private ConcurrentDictionary<long, object> m2mqtt = new ConcurrentDictionary<long, object>();
        private void button9_Click(object sender, EventArgs e)
        {
            bool bIsNext = true;
            var enumer = m2mqtt.GetEnumerator();
            while (bIsNext == true)
            {
                bIsNext = enumer.MoveNext();
                if (bIsNext == false) break;
                var s0 = enumer.Current;
                long key = s0.Key;
                object obj = s0.Value;

                v2_alarm_logVModel am = CommFunc.GetPropertyValue("cc", obj) as v2_alarm_logVModel;
                DateTime dd = CommFunc.ConvertDBNullToDateTime(CommFunc.GetPropertyValue("time", obj));
                int count = CommFunc.ConvertDBNullToInt32(CommFunc.GetPropertyValue("count", obj));
                bool isUp = false;
                if (count == 0 || dd.AddMinutes(30) >= DateTime.Now)
                {
                    count = count + 1;
                    isUp = true;
                }
                if (count == 6)
                {
                    m2mqtt.TryRemove(key, out obj);
                }
                else
                {
                    if (isUp == true)
                    {
                        object newObj = new { cc = am, time = DateTime.Now, count = count };
                        bool tt = m2mqtt.TryUpdate(key, newObj, obj);
                    }
                }
            }
            return;
            DateTime mTime = DateTime.Now;
            DateTime.TryParse("2019-10-10 14:25:36",out mTime);
            byte[] data = new byte[16];
            dllHepler.f_des_encryptByTimeEle(mTime.Year, mTime.Month, mTime.Day, mTime.Hour, mTime.Minute, mTime.Second, (float)1.2, data);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //m2mqtt.Publish(this.textBox2.Text);
            return;
            dynamic obj = new System.Dynamic.ExpandoObject();
            obj.equ = "t1";
            obj.loop = "t2";
            obj.reation = "t3";
            obj.content = "t4";
            obj.time = DateTime.Now.ToString("yyyy-MM-dd");
            //string sms = JsonHelper.Serialize(obj);
            dynamic postData = new System.Dynamic.ExpandoObject();
            postData.TemplateId = "SMS_164265186";
            postData.TemplateParam = JsonHelper.Serialize(obj);
            postData.PhoneNumbers = "13710218209";
            string sms = JsonHelper.Serialize(postData);
            string res = HttpPost("http://47.106.232.22:6009" + "/api/Sms/Send", sms);
            //string res = HttpPost("http://localhost:2545" + "/api/Sms/Send", sms);
            string sss = "";
            if (res.Contains("BizId"))
            {
                sss = "y";
            }
        }

        private string HttpPost(string Url, string postDataStr)
        {
            CookieContainer cookie = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = System.Text.Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            System.IO.Stream myRequestStream = request.GetRequestStream();
            System.IO.StreamWriter myStreamWriter = new System.IO.StreamWriter(myRequestStream, System.Text.Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            System.IO.Stream myResponseStream = response.GetResponseStream();
            System.IO.StreamReader myStreamReader = new System.IO.StreamReader(myResponseStream, System.Text.Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.IO.Stream myResponseStream = response.GetResponseStream();
            System.IO.StreamReader myStreamReader = new System.IO.StreamReader(myResponseStream, System.Text.Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
