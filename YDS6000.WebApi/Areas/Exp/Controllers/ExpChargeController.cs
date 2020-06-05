using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 收费管理Api
    /// </summary>
    [RoutePrefix("api/Exp/Charge")]
    public class ExpChargeController : ApiController
    {
        private ChargeHelper infoHelper = new ChargeHelper();

        /// <summary>
        /// 获取收费管理列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdPrePayInMdOnList")]
        public APIRst GetYdPrePayInMdOnList(string strcName="", string coName="",int co_id=0)
        {
            return infoHelper.GetYdPrePayInMdOnList(strcName, coName, co_id);
        }
        /// <summary>
        /// 获取充值/退费/赠电/退赠电明细信息
        /// </summary>
        /// <param name="co_id">房间id号</param>
        /// <param name="module_id">电表id号</param>
        /// <param name="moduleAddr">电表</param>
        /// <param name="price">电价</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdPrepPayInMd_01OnInfo")]
        public APIRst GetYdPrepPayInMd_01OnInfo(int co_id, int module_id, string moduleAddr, decimal price)
        {
            return infoHelper.GetYdPrepPayInMd_01OnInfo(co_id, module_id, moduleAddr, price);
        }

        /// <summary>
        /// 充值/退费/赠电/退赠电
        /// </summary>
        /// <param name="price">电价</param>
        /// <param name="payType">1:充值，2:退费，3:赠电，4:退赠电</param>
        /// <param name="co_id">房间id号</param>
        /// <param name="module_id">电表id号</param>
        /// <param name="moduleAddr">电表</param>
        /// <param name="fun_id">功能id号</param>
        /// <param name="payAmt">缴费金额</param>
        /// <param name="isPay">支付方式(0:现金，10:微信)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdPrePayInMd_01OnChrg")]
        public APIRst YdPrePayInMd_01OnChrg(decimal price, int payType, int co_id, int module_id, string moduleAddr, int fun_id, decimal payAmt, int isPay)
        {
            return infoHelper.YdPrePayInMd_01OnChrg(price, payType, co_id, module_id, moduleAddr, fun_id, payAmt, isPay);
        }

        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="pId">支付记录id号</param>
        /// <param name="pMid">电表id号</param>
        /// <param name="pTrade_no">支付商品号(可选)</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("YdPrepPayInMdRecallPay")]
        public APIRst YdPrepPayInMdRecallPay(int pId, int pMid, string pTrade_no="")
        {
            return infoHelper.YdPrepPayInMdRecallPay(pId, pMid, pTrade_no);
        }

        /// <summary>
        /// 清零
        /// </summary>
        /// <param name="module_id">电表id号</param>
        /// <param name="fun_id">功能id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdPrepPayInMd_Reset")]
        public APIRst YdPrepPayInMd_Reset(int module_id, int fun_id)
        {
            return infoHelper.YdPrepPayInMd_Reset(module_id, fun_id);
        }
        /// <summary>
        /// 获取小票信息
        /// </summary>
        /// <param name="Log_id">支付记录id号</param>
        /// <returns></returns> 
        [HttpGet, HttpOptions]
        [Route("GetYdPayBill")]
        public APIRst GetYdPayBill(long Log_id)
        {      
            return infoHelper.GetYdPayBill(Log_id);
        }
        ///// <summary>
        ///// 汇出Excel（充值缴费--打印小票）
        ///// </summary>
        ///// <param name="Co_id">房间id</param>
        ///// <param name="PayAmt">缴费金额</param>
        ///// <param name="IsPay">缴费方式：=0现金充值，=10微信充值</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetYdPrePayInMdExcel")]
        //public APIRst GetYdPrePayInMdExcel(int Co_id,string PayAmt,int IsPay)
        //{
        //    return infoHelper.GetYdPrePayInMdExcel(Co_id, PayAmt, IsPay);
        //}
        /// <summary>
        /// 获取二维码图片
        /// </summary>
        /// <param name="pQrUrl">二维码地址</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdPayOnQrCode")]
        public HttpResponseMessage GetYdPayOnQrCode(string pQrUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(pQrUrl))
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent);
                }
                System.Drawing.Bitmap image = new WxPayAPI.WxPay().GetQrCode(pQrUrl);
                //保存为PNG到内存流  
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //输出二维码图片
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StreamContent(new System.IO.MemoryStream(ms.GetBuffer()));
                resp.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return resp;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog("产生二维码错误(YdPostPayInMd_01OnQrCode)", ex.Message + ex.StackTrace);
                throw new Exception("产生二维码错误(YdPostPayInMd_01OnQrCode)" + ex.Message);
            }
        }
    }
}
