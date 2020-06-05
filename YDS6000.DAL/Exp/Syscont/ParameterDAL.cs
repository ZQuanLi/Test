using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Syscont
{
    public partial class ParameterDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ParameterDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取充值参数信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetFrPay()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger ,a.FrMd,a.Price,a.CoName,a.ChrgType from syscont as a where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 充值参数
        /// </summary>
        /// <param name="Price">电量单价</param>
        /// <param name="CoName">售电票据单位名称</param>
        /// <param name="ChrgType">付费类型</param>
        /// <returns></returns>
        public int FSetFrPay(float Price, string CoName, int ChrgType)
        {
            int cc = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Price from syscont where Ledger=@Ledger");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
            float pp = CommFunc.ConvertDBNullToFloat(obj);
            if (Price != pp) {
                strSql.Clear();
                strSql.Append("START TRANSACTION;");
                strSql.Append("insert into v2_info(`Ledger`, `Co_id`, `Module_id`, `ModuleAddr`, `Fun_id`, `Hist_id`, `YT_id`,");
                strSql.Append("`BeginVal`, `FirstVal`, `LastVal`, `MinVal`, `MaxVal`, `FirstTime`, `LastTime`, `MinTime`, `MaxTime`, `DataCfg`,");
                strSql.Append("`FirstVal1st`,`FirstVal2nd`, `FirstVal3rd`, `FirstVal4th`, `Val1st`, `Val2nd`, `Val3rd`, `Val4th`, `ChargVal`, `RdVal`, `DebtTime`,");
                strSql.Append("`Create_by`, `Create_dt`, `Update_by`, `Update_dt`, `BaseVal`, `RaVal`, `InVal`, `Remark`)");
                strSql.Append(" select a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,ifnull(max.Hist_id,0)+1,a.YT_id,");
                strSql.Append(" a.BeginVal,a.FirstVal,a.LastVal,a.MinVal,a.MaxVal,a.FirstTime,a.LastTime,a.MinTime,a.MaxTime,a.DataCfg,");
                strSql.Append(" a.FirstVal1st,a.FirstVal2nd,a.FirstVal3rd,a.FirstVal4th,a.Val1st,a.Val2nd,a.Val3rd,a.Val4th,a.ChargVal,a.RdVal,DebtTime,");
                strSql.Append(" @SysUid, now(), @SysUid, now(), a.BaseVal,a.RaVal,a.InVal,CONCAT('改单价:',b.Price,'->',@Price)");
                strSql.Append(" from v2_info as a inner join syscont as b on a.Ledger=b.Ledger");
                strSql.Append(" inner join vp_funinfo as fun on a.Ledger=fun.Ledger and a.Co_id = fun.Co_id and a.Module_id = fun.Module_id and a.ModuleAddr = fun.ModuleAddr and a.Fun_id = fun.Fun_id");
                strSql.Append(" left join (select Ledger,Co_id,Module_id,ModuleAddr,Fun_id,max(Hist_id)as Hist_id from v2_info group by Ledger,Co_id,Module_id,ModuleAddr,Fun_id) as max");
                strSql.Append(" on a.Ledger=max.Ledger and a.Co_id = max.Co_id and a.Module_id = max.Module_id and a.ModuleAddr = max.ModuleAddr and a.Fun_id = max.Fun_id");
                strSql.Append(" where a.Ledger=@Ledger and a.Hist_id=0 and fun.FunType='E' and a.RdVal!=0;");
                /*补差额电度*/
                strSql.Append("update vp_v2info as a inner join syscont as b on a.Ledger=b.Ledger");
                strSql.Append(" inner join vp_funinfo as fun on a.Ledger=fun.Ledger and a.Co_id = fun.Co_id and a.Module_id = fun.Module_id and a.ModuleAddr = fun.ModuleAddr and a.Fun_id = fun.Fun_id");
                strSql.Append(" set a.ChargVal = (a.ChargVal - a.RdVal) + round(round(a.RdVal*b.Price,2)/@Price,2)");
                strSql.Append(" where a.Ledger=@Ledger and a.Hist_id=0 and fun.FunType='E' and a.RdVal!=0;");
                strSql.Append("update syscont set Price = @Price where Ledger=@Ledger;");
                //
                strSql.Append("COMMIT;");
                cc = cc + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Price = Price, SysUid = this.SysUid });
            }
            //
            strSql.Clear();
            strSql.Append("update syscont set CoName = @CoName ,ChrgType=@ChrgType where Ledger=@Ledger");
            cc = cc + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Price = Price, CoName = CoName, ChrgType = ChrgType });
            return cc;
        }

        /// <summary>
        /// 获取采集频率
        /// </summary>
        /// <returns></returns>
        public DataTable GetTheCmTask()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger ,a.FrMd,a.Price,a.CoName from syscont as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置采集频率
        /// </summary>
        /// <param name="frmd">周期</param>
        /// <returns></returns>
        public int SetCmTaskFrMd(string FrMd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update syscont set FrMd =@FrMd where Ledger=@Ledger");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, FrMd = FrMd });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取登录页文字
        /// </summary>
        /// <returns></returns>
        public DataTable GetTheWords()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger ,a.Fname,a.FCopyright,a.CoName,a.BgImg from syscont as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置登录页文字
        /// </summary>
        /// <param name="Fname">首页标题展示文字</param>
        /// <param name="FCopyright">首页版权展示文字</param>
        /// <returns></returns>
        public int SetWords(string Fname, string FCopyright)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update syscont set Fname =@Fname,FCopyright = @FCopyright where Ledger=@Ledger");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Fname = Fname, FCopyright = FCopyright });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取首页的LOGO,背景图和标题
        /// </summary>
        /// <returns></returns>
        public DataTable GetPicANDWord()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Fname,a.FCopyright,a.Attached,a.CoName,a.BgImg from syscont as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置首页LOGO图片
        /// </summary>  
        /// <param name="Attached"></param>
        /// <returns></returns>
        public int SetPic(string Attached)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update syscont set Attached=@Attached where Ledger=@Ledger");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Attached = Attached });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 设置首页背景图
        /// </summary>  
        /// <param name="BgImg"></param>
        /// <returns></returns>
        public int SetBgImg(string BgImg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update syscont set BgImg=@BgImg where Ledger=@Ledger");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, BgImg = BgImg });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT * FROM sys_menu WHERE ledger=@Ledger and attrib=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public int SetMenuList(string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update sys_menu set disabled=1 where ledger=@Ledger and attrib=0");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger });
            strSql.Clear();
            foreach (string prog_id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                strSql.Append("update sys_menu set disabled=0 where ledger=@Ledger and menu_no='" + prog_id + "' ;");
            }
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取白名单设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetWhitelist()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger ,a.ExCfg,a.MthBaseRoom,a.MthBaseCrm,a.MthBaseIsAcp,a.BaseType from syscont as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置白名单
        /// </summary>
        /// <param name="exCfg">{m01:0,m02:1,m03:0,m04:0,m05:1,m06:0,m07:0,m08:0,m09:0,m10:0,m11:0,m12:0}</param>
        /// <returns></returns>
        public int SetWhitelist(string exCfg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(1) from syscont");
            strSql.Append(" where ledger=@ledger ");
            object obj2 = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger });
            if (CommFunc.ConvertDBNullToInt32(obj2) == 0)
            {
                strSql.Clear();
                strSql.Append("insert into syscont(");
                strSql.Append("Ledger,CoName,FrMd,Price,exCfg,FCopyright,Attached,ChrgType,ExCfg,MthBaseRoom,MthBaseCrm,MthBaseIsAcp,BaseType,Pri1st,Pri2nd,Pri3rd,Pri4th)");
                strSql.Append(" values (");
                strSql.Append("@ledger,'',0,0.0,'','','',0,'',0,0,0,0,0.0,0.0,0.0,0.0)");
                SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger });
            }
            strSql.Clear();
            strSql.Append("update syscont set ");
            strSql.Append("ExCfg=@exCfg");
            strSql.Append(" where Ledger=@ledger ");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, exCfg = exCfg });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取赠电参数
        /// </summary>
        /// <returns></returns>
        public DataTable GetZdCfg()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger ,a.ExCfg,a.MthBaseRoom,a.MthBaseCrm,a.MthBaseIsAcp,a.BaseType from syscont as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
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
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update syscont set MthBaseRoom =@MthBaseRoom,MthBaseCrm = @MthBaseCrm ,MthBaseIsAcp=@MthBaseIsAcp,BaseType=@BaseType where Ledger=@Ledger");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, MthBaseRoom = MthBaseRoom, MthBaseCrm = MthBaseCrm, MthBaseIsAcp = MthBaseIsAcp, BaseType = BaseType });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取告警参数--告警设置
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmVal()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select tb1.CfType,tb1.cfvalue as AlarmVal1, tb1.rule as Rule1,  tb2.cfvalue as AlarmVal2, tb2.rule as Rule2,");
            strSql.Append(" tb3.cfvalue as AlarmVal3, tb3.rule as Rule3");
            strSql.Append(" from (select cfvalue,rule,Ledger,CfType from sys_config where cfkey='AlarmVal1') as tb1,");
            strSql.Append(" (select cfvalue,rule,Ledger,CfType from sys_config where cfkey='AlarmVal2') as tb2,");
            strSql.Append(" (select cfvalue,rule,Ledger,CfType from sys_config where cfkey='AlarmVal3') as tb3");
            strSql.Append(" WHERE tb1.Ledger=@Ledger;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
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
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)value(@Ledger,'AlarmVal1','Alarm',@AlarmVal1,@Rule1)");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue = @AlarmVal1 ,Rule=@Rule1;");

            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)value(@Ledger,'AlarmVal2','Alarm',@AlarmVal2,@Rule2)");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue = @AlarmVal2 ,Rule=@Rule2;");

            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)value(@Ledger,'AlarmVal3','Alarm',@AlarmVal3,@Rule3)");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue = @AlarmVal3 ,Rule=@Rule3;");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, AlarmVal1 = AlarmVal1, Rule1 = Rule1, AlarmVal2 = AlarmVal2, Rule2 = Rule2, AlarmVal3 = AlarmVal3, Rule3 = Rule3 });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取告警参数--透支电量
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmCfg()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger ,a.CfType,a.CfValue,a.Rule from sys_config as a");
            strSql.Append(" where a.Ledger=@Ledger and CfKey='AlarmValOd'");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置告警参数--透支电量
        /// </summary>
        /// <param name="OdValue">最低拉闸电量</param>
        /// <param name="IsClosed">最低电量是否拉闸</param>
        /// <returns></returns>
        public int SetAlarmCfg(decimal OdValue, int IsClosed)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)value(@Ledger,'AlarmValOd','Alarm',@OdValue,@IsClosed)");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue = @OdValue ,Rule=@IsClosed;");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, OdValue = OdValue, IsClosed = IsClosed });
            return CommFunc.ConvertDBNullToInt32(obj);
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
        public int SetWxCofig(string WxAPPID, string WxMCHID, string WxKEY, string WxAPPSECRET, string WxRedirect_uri)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'WxAPPID','Wx',@WxAPPID,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@WxAPPID;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'WxMCHID','Wx',@WxMCHID,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@WxMCHID;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'WxKEY','Wx',@WxKEY,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@WxKEY;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'WxAPPSECRET','Wx',@WxAPPSECRET,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@WxAPPSECRET;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'WxRedirect_uri','Wx',@WxRedirect_uri,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@WxRedirect_uri;");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, WxAPPID = WxAPPID, WxMCHID = WxMCHID, WxKEY = WxKEY, WxAPPSECRET = WxAPPSECRET, WxRedirect_uri = WxRedirect_uri });
            return CommFunc.ConvertDBNullToInt32(obj);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'AliUrl','AliSms',@AliUrl,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@AliUrl;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'AliAppKey','AliSms',@AliAppKey,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@AliAppKey;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'AliAppSecret','AliSms',@AliAppSecret,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@AliAppSecret;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'AliSignName','AliSms',@AliSignName,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@AliSignName;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'AliTemplateCode','AliSms',@AliTemplateCode,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@AliTemplateCode;");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, AliUrl = Alisms.AliUrl, AliAppKey = Alisms.AliAppKey, AliAppSecret = Alisms.AliAppSecret, AliSignName = Alisms.AliSignName, AliTemplateCode = Alisms.AliTemplateCode });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        public int SetComConfig(ComSmsConfig ComConfig)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'ComSms','Sms',@ComSms,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@ComSms;");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, ComSms = ComConfig.ComSms });
            return CommFunc.ConvertDBNullToInt32(obj);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'MailFrom','Email',@MailFrom,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@MailFrom;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'MailSmtpHost','Email',@MailSmtpHost,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@MailSmtpHost;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'MailSmtpPassword','Email',@MailSmtpPassword,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@MailSmtpPassword;");
            //
            strSql.Append("insert into sys_config(Ledger,CfKey,CfType,CfValue,Rule)values(@Ledger,'MailSmtpUser','Email',@MailSmtpUser,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE CfValue=@MailSmtpUser;");
            object obj = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, MailFrom = MailFrom, MailSmtpHost = MailSmtpHost, MailSmtpPassword = MailSmtpPassword, MailSmtpUser = MailSmtpUser });
            return CommFunc.ConvertDBNullToInt32(obj);
        }
        public string GetSmsBy()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SmsBy from syscont where Ledger=@Ledger");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
            return CommFunc.ConvertDBNullToString(obj);
        }
        public int UpdateSmsBy(int uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update syscont set SmsBy=@Uid where Ledger=@Ledger");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Uid = uid });
        }

    }
}
