using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    public partial class ChargeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Charge.ChargeBLL bll = null;
        private static object ObjLock = new object();
        public ChargeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Charge.ChargeBLL(WebConfig.SysProject, user.Ledger, user.Uid);
        }
        //生成二维码地址
        private string GetPayUrl(v4_pay_logVModel pay)
        {
            //pay.IsPay = 1;
            string body = string.Format("【{0}{1}】电费充值", pay.ModuleAddr, "");//商品描述;
            int total_fee = (int)(pay.PayAmt * 100);
            string out_trade_no = "";
            string productId = pay.Co_id.ToString() + "-" + pay.Module_id.ToString() + "-" + pay.ModuleAddr;
            string urlPay = "";
            urlPay = new WxPayAPI.WxPay().GetPayUrl(productId, body, total_fee, out out_trade_no);
            pay.Trade_no = out_trade_no;
            return urlPay;
        }
    }
}