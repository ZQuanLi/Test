using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.Models
{
    public class WxFun
    {
        public WxConfig WxConfig = new WxConfig();
        private WxPayAPI.WxPay wx = new WxPayAPI.WxPay();

        /// <summary>
        /// 微信推送信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="concent"></param>
        /// <param name="remark"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PushAlarm(string openid, string firstTitle,string concent, string remark,out string msg)
        {
            return wx.PushAlarm(openid, firstTitle, concent, remark, out msg);
        }
    }
}
