using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ParameterBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ParameterDAL dal = null;
        public ParameterBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ParameterDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取充值参数信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetFrPay()
        {
            return dal.GetFrPay();
        }

        /// <summary>
        /// 充值参数
        /// </summary>
        /// <param name="frPay">电量单价</param>
        /// <param name="fReort_head">售电票据单位名称</param>
        /// <param name="vRdChrgType">付费类型</param>
        /// <returns></returns>
        public int FSetFrPay(float frPay, string fReort_head, int vRdChrgType)
        {
            return dal.FSetFrPay(frPay, fReort_head, vRdChrgType);
        }

        /// <summary>
        /// 获取采集频率
        /// </summary>
        /// <returns></returns>
        public DataTable GetTheCmTask()
        {
            return dal.GetTheCmTask();
        }

        /// <summary>
        /// 设置采集频率
        /// </summary>
        /// <param name="frmd">周期</param>
        /// <returns></returns>
        public int SetCmTaskFrMd(string frmd)
        {
            return dal.SetCmTaskFrMd(frmd);
        }

        /// <summary>
        /// 获取登录页文字
        /// </summary>
        /// <returns></returns>
        public DataTable GetTheWords()
        {
            return dal.GetTheWords();
        }

        /// <summary>
        /// 设置登录页文字
        /// </summary>
        /// <param name="frName">首页标题展示文字</param>
        /// <param name="FCopyright">首页版权展示文字</param>
        /// <returns></returns>
        public int SetWords(string frName, string FCopyright)
        {
            return dal.SetWords(frName, FCopyright);
        }

        /// <summary>
        /// 获取首页的LOGO,背景图和标题
        /// </summary>
        /// <returns></returns>
        public DataTable GetPicANDWord()
        {
            return dal.GetPicANDWord();
        }

        /// <summary>
        /// 设置首页LOGO图片
        /// </summary>       
        /// <returns></returns>
        public int SetPic(string Attached)
        {
            return dal.SetPic(Attached);
        }
        
        /// <summary>
        /// 设置首页背景图
        /// </summary>       
        /// <returns></returns>
        public int SetBgImg(string BgImg)
        {
            return dal.SetBgImg(BgImg);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public List<Treeview> GetMenuList(out int total)
        {
            DataTable dtSource = dal.GetMenuList();
            total = dtSource.Rows.Count;
            return this.GetMenuList(dtSource);
        }

        private List<Treeview> GetMenuList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("(parent_no='0' or parent_no='')", "ordno,menu_no");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["prog_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["disabled"]);
                pTr.nodes = new List<Treeview>();
                this.GetMenuList(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["menu_no"]));
                rst.Add(pTr);
            }
            return rst;
        }

        private void GetMenuList(ref DataTable dtSource, ref Treeview pTr, string menu_no)
        {
            DataRow[] pArr = dtSource.Select("parent_no='" + menu_no + "'", "ordno,menu_no");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["prog_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                cTr.attributes = CommFunc.ConvertDBNullToInt32(dr["disabled"]);
                pTr.nodes.Add(cTr);
                this.GetMenuList(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["menu_no"]));
            }
        }

        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public int SetMenuList(string ids)
        {
            return dal.SetMenuList(ids);
        }

        /// <summary>
        /// 获取白名单设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetWhitelist()
        {
            return dal.GetWhitelist();
        }

        /// <summary>
        /// 设置白名单
        /// </summary>
        /// <param name="exCfg"></param>
        /// <returns></returns>
        public int SetWhitelist(string exCfg)
        {
            return dal.SetWhitelist(exCfg);
        }

        /// <summary>
        /// 获取赠电参数
        /// </summary>
        /// <returns></returns>
        public DataTable GetZdCfg()
        {
            return dal.GetZdCfg();
        }

        /// <summary>
        /// 设置赠电参数
        /// </summary>
        /// <param name="MthBaseRoom">每月赠送</param>
        /// <param name="MthBaseCrm">每月按用户个数赠送</param>
        /// <param name="MthBaseIsAcp">未使用完的赠电量是否累计到下月</param>
        /// <param name="BaseType">赠电类型</param>
        /// <returns></returns>
        public int SetZdCfg(decimal MthBaseRoom, decimal MthBaseCrm, int MthBaseIsAcp, int BaseType)
        {
            return dal.SetZdCfg(MthBaseRoom, MthBaseCrm, MthBaseIsAcp, BaseType);
        }

        /// <summary>
        /// 获取告警参数--告警设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmVal()
        {
            return dal.GetAlarmVal();
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
        public int SetAlarmVal(decimal AlarmVal1, decimal Rule1, decimal AlarmVal2, decimal Rule2, decimal AlarmVal3, decimal Rule3)
        {
            return dal.SetAlarmVal(AlarmVal1, Rule1, AlarmVal2, Rule2, AlarmVal3, Rule3);
        }

        /// <summary>
        /// 获取告警参数--透支电量
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmCfg()
        {
            return dal.GetAlarmCfg();
        }

        /// <summary>
        /// 设置告警参数--透支电量
        /// </summary>
        /// <param name="OdValue">最低拉闸电量</param>
        /// <param name="IsClosed">最低电量是否拉闸</param>
        /// <returns></returns>
        public int SetAlarmCfg(decimal OdValue, int IsClosed)
        {
            return dal.SetAlarmCfg(OdValue, IsClosed);
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
        public int SetWxCofig(string appid, string mchid, string key, string appsecret, string redirect_uri)
        {
            return dal.SetWxCofig(appid, mchid, key, appsecret, redirect_uri);
        }

        /// <summary>
        /// 设置短信接口信息
        /// </summary>
        /// <param name="AliUrl">APPID</param>
        /// <param name="AliAppKey">商户号</param>
        /// <param name="AliAppSecret">商户支付密钥</param>
        /// <param name="AliSignName">公众号Secert(仅JSAPI支付时需配置)</param>
        /// <param name="AliTemplateCode">公众号回调页面(仅JSAPI支付时需配置)</param>
        /// <returns></returns>
        public int SetAliConfig(AliSmsConfig Alisms)
        {
            return dal.SetAliConfig(Alisms);
        }

        public int SetComConfig(ComSmsConfig ComConfig)
        {
            return dal.SetComConfig(ComConfig);
        }

        /// <summary>
        /// 设置邮件接口信息
        /// </summary>
        /// <param name="MailFrom">邮件发送人</param>
        /// <param name="MailSmtpHost">邮件服务器地址</param>
        /// <param name="MailSmtpPassword">邮件登陆密码</param>
        /// <param name="MailSmtpUser">邮件登陆名</param>
        /// <returns></returns>
        public int SetMailConfig(string MailFrom, string MailSmtpHost, string MailSmtpPassword, string MailSmtpUser)
        {
            return dal.SetMailConfig(MailFrom, MailSmtpHost, MailSmtpPassword, MailSmtpUser);
        }
        public string GetSmsBy()
        {
            return dal.GetSmsBy();
        }
        public int UpdateSmsBy(int uid)
        {
            return dal.UpdateSmsBy(uid);
        }

    }
}
