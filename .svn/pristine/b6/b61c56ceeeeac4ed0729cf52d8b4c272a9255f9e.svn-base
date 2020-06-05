using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统配置-参数管理
    /// </summary>
    [RoutePrefix("api/Exp/ExpSyscont")]
    public class ExpSyscontController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpParameterHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpParameterHelper();

        /// <summary>
        /// 增加操作记录
        /// </summary>
        /// <param name="content">操作内容</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("AddLog")]
        public APIRst AddLog(string content)
        {
            return infoHelper.AddLog(content);
        }

        /// <summary>
        /// 获取充值参数信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetFrPay")]
        public APIRst GetFrPay()
        {
            return infoHelper.GetFrPay();
        }

        /// <summary>
        /// 充值参数
        /// </summary>
        /// <param name="frPay">电量单价</param>
        /// <param name="fReort_head">售电票据单位名称</param>
        /// <param name="vRdChrgType">付费类型</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetFrPay")]
        public APIRst SetFrPay(string frPay, string fReort_head, int vRdChrgType)
        {
            return infoHelper.FSetFrPay(frPay, fReort_head, vRdChrgType);
        }

        /// <summary>
        /// 获取采集频率
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetTheCmTask")]
        public APIRst GetTheCmTask()
        {
            return infoHelper.GetTheCmTask();
        }

        /// <summary>
        /// 设置采集频率
        /// </summary>
        /// <param name="frmd">周期</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetCmTaskFrMd")]
        public APIRst SetCmTaskFrMd(string frmd)
        {
            return infoHelper.SetCmTaskFrMd(frmd);
        }

        /// <summary>
        /// 获取登录页文字
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [SecurityCtrl(false, false)]//打上这个标签，不登陆获取接口
        [Route("GetTheWords")]
        public APIRst GetTheWords()
        {
            return infoHelper.GetTheWords();
        }

        /// <summary>
        /// 设置登录页文字
        /// </summary>
        /// <param name="frName">首页标题展示文字</param>
        /// <param name="FCopyright">首页版权展示文字</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]        
        [Route("SetWords")]
        public APIRst SetWords(string frName, string FCopyright)
        {
            return infoHelper.SetWords(frName, FCopyright);
        }

        /// <summary>
        /// 获取首页的LOGO,背景图和标题
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPicANDWord")]
        public APIRst GetPicANDWord()
        {
            return infoHelper.GetPicANDWord();
        }

        /// <summary>
        /// 设置首页logo
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetPic")]
        public APIRst SetPic()
        {
            return infoHelper.SetPic();
        }

        /// <summary>
        /// 设置首页背景图
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetBgImg")]
        public APIRst SetBgImg()
        {
            return infoHelper.SetBgImg();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMenuList")]
        public APIRst GetMenuList()
        {
            return infoHelper.GetMenuList();
        }

        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetMenuList")]
        public APIRst SetMenuList(string ids)
        {
            return infoHelper.SetMenuList(ids);
        }

        /// <summary>
        /// 获取白名单设置
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetWhitelist")]
        public APIRst GetWhitelist()
        {
            return infoHelper.GetWhitelist();
        }

        /// <summary>
        /// 设置白名单
        /// </summary>
        /// <param name="exCfg">月（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetWhitelist")]
        public APIRst SetWhitelist(string exCfg)
        {
            return infoHelper.SetWhitelist(exCfg);
        }

        /// <summary>
        /// 获取赠电参数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetZdCfg")]
        public APIRst GetZdCfg()
        {
            return infoHelper.GetZdCfg();
        }

        /// <summary>
        /// 设置赠电参数
        /// </summary>
        /// <param name="MthBaseRoom">每月赠送</param>
        /// <param name="MthBaseCrm">每月按用户个数赠送</param>
        /// <param name="MthBaseIsAcp">未使用完的赠电量是否累计到下月</param>
        /// <param name="BaseType">赠电类型</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetZdCfg")]
        public APIRst SetZdCfg(decimal MthBaseRoom, decimal MthBaseCrm, int MthBaseIsAcp, int BaseType)
        {
            return infoHelper.SetZdCfg(MthBaseRoom, MthBaseCrm, MthBaseIsAcp, BaseType);
        }

        /// <summary>
        /// 获取告警参数--告警设置
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmVal")]
        public APIRst GetAlarmVal()
        {
            return infoHelper.GetAlarmVal();
        }

        /// <summary>
        /// 设置告警参数--告警设置
        /// </summary>
        /// <param name="AlarmVal1">方案①： 低于告警电量</param>
        /// <param name="Rule1">天/次的频率告警</param>
        /// <param name="AlarmVal2">方案②： 低于告警电量</param>
        /// <param name="Rule2">天/次的频率告警</param>
        /// <param name="AlarmVal3">方案③： 低于告警电量</param>
        /// <param name="Rule3">天/次的频率告警</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetAlarmVal")]
        public APIRst SetAlarmVal(decimal AlarmVal1, decimal Rule1, decimal AlarmVal2, decimal Rule2, decimal AlarmVal3, decimal Rule3)
        {
            return infoHelper.SetAlarmVal(AlarmVal1, Rule1, AlarmVal2, Rule2, AlarmVal3, Rule3);
        }

        /// <summary>
        /// 获取告警参数--透支电量
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmCfg")]
        public APIRst GetAlarmCfg()
        {
            return infoHelper.GetAlarmCfg();
        }

        /// <summary>
        /// 设置告警参数--透支电量
        /// </summary>
        /// <param name="OdValue">最低拉闸电量</param>
        /// <param name="IsClosed">最低电量是否拉闸</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetAlarmCfg")]
        public APIRst SetAlarmCfg(decimal OdValue, int IsClosed)
        {
            return infoHelper.SetAlarmCfg(OdValue, IsClosed);
        }

        /// <summary>
        /// 获取微信公众号信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetWxCofig")]
        public APIRst GetWxCofig()
        {
            return infoHelper.GetWxCofig();
        }

        /// <summary>
        /// 设置微信公众号信息
        /// </summary>
        /// <param name="appid">APPID</param>
        /// <param name="mchid">商户号</param>
        /// <param name="key">商户支付密钥</param>
        /// <param name="appsecret">公众号Secert(仅JSAPI支付时需配置)</param>
        /// <param name="redirect_uri">公众号回调页面(仅JSAPI支付时需配置)</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetWxCofig")]
        public APIRst SetWxCofig(string appid, string mchid, string key, string appsecret, string redirect_uri)
        {
            return infoHelper.SetWxCofig(appid, mchid, key, appsecret, redirect_uri);
        }

        /// <summary>
        /// 获取短信接口信息-模块一
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAliConfig")]
        public APIRst GetAliConfig()
        {
            return infoHelper.GetAliConfig();
        }

        /// <summary>
        /// 获取短信接口信息-模块二
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetComConfig")]
        public APIRst GetComConfig()
        {
            return infoHelper.GetComConfig();
        }

        /// <summary>
        /// 设置短信接口信息
        /// </summary>
        /// <param name="AliUrl">APPID</param>
        /// <param name="AliAppKey">商户号</param>
        /// <param name="AliAppSecret">商户支付密钥</param>
        /// <param name="AliSignName">公众号Secert(仅JSAPI支付时需配置)</param>
        /// <param name="AliTemplateCode">公众号回调页面(仅JSAPI支付时需配置)</param>
        /// <param name="ComSms">商户支付密钥</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetAliConfig")]
        public APIRst SetAliConfig(string AliUrl, string AliAppKey, string AliAppSecret, string AliSignName, string AliTemplateCode, string ComSms)
        {
            return infoHelper.SetAliConfig(AliUrl, AliAppKey, AliAppSecret, AliSignName, AliTemplateCode, ComSms);
        }

        /// <summary>
        /// 获取邮件接口信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMailConfig")]
        public APIRst GetMailConfig()
        {
            return infoHelper.GetMailConfig();
        }

        /// <summary>
        /// 设置邮件接口信息
        /// </summary>
        /// <param name="MailFrom">邮件发送人</param>
        /// <param name="MailSmtpHost">邮件服务器地址</param>
        /// <param name="MailSmtpPassword">邮件登陆密码</param>
        /// <param name="MailSmtpUser">邮件登陆名</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetMailConfig")]
        public APIRst SetMailConfig(string MailFrom, string MailSmtpHost, string MailSmtpPassword, string MailSmtpUser)
        {
            return infoHelper.SetMailConfig(MailFrom, MailSmtpHost, MailSmtpPassword, MailSmtpUser);
        }

        /// <summary>
        /// 获取短信接收人
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetSmsBy")]
        public APIRst GetSmsBy()
        {
            return infoHelper.GetSmsBy();
        }
        /// <summary>
        /// 设置短信接收人
        /// </summary>
        /// <param name="uid">接收人ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateSmsBy")]
        public APIRst UpdateSmsBy(int uid)
        {
            return infoHelper.UpdateSmsBy(uid);
        }
    }
}