using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YDS6000.Models
{
    /// <summary>
    /// V0_Cust 表的Attrib属性值
    /// </summary>
    public enum CoAttrib
    {
        /// <summary>
        /// 项目
        /// </summary>
        [Display(Name = "项目")]
        Project = 0,
        /// <summary>
        /// 建筑
        /// </summary>
        [Display(Name = "建筑")]
        Build = 100,
        /// <summary>
        /// 区域
        /// </summary>
        [Display(Name = "区域")]
        Area = 1000,
        /// <summary>
        /// 机房
        /// </summary>
        [Display(Name = "机房")]
        Station = 2000,
        /// <summary>
        /// 站点
        /// </summary>
        [Display(Name = "站点")]
        Room = 9000,
    }

    /// <summary>
    /// V0_Cust 表的Attrib属性值
    /// </summary>
    public enum CoAttribV2_1
    {
        /// <summary>
        /// 区域
        /// </summary>
        [Display(Name = "区域")]
        Area = 1010,
        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "单位")]
        Unit = 1020,
        /// <summary>
        /// 项目
        /// </summary>
        [Display(Name = "项目")]
        Project = 1030,
        /// <summary>
        /// 用电单元
        /// </summary>
        [Display(Name = "用电单元")]
        Cell = 1040,
    }

    /// <summary>
    /// V0_Cic表的Attrib属性值
    /// </summary>
    public enum CicAttrib
    {
        /// <summary>
        /// 公司
        /// </summary>
        [Display(Name = "公司")]
        Corp = 100,
        /// <summary>
        /// 场景
        /// </summary>
        [Display(Name = "场景")]
        Scene = 200,
        /// <summary>
        /// 公司
        /// </summary>
        [Display(Name = "机房类型")]
        StationType = 300,
        /// <summary>
        /// 物业收费项
        /// </summary>
        [Display(Name = "物业收费项")]
        ChrgType = 500,
    }

    //暂时没有使用
    public enum V0Fun
    {
        /// <summary>
        /// 通讯状态
        /// </summary>
        [Display(Name = "通讯状态")]
        Comm = 1000,

        /// <summary>
        /// 当前正向有功总电能示值
        /// </summary>
        [Display(Name = "正向有功总电能")]
        E = 9010,
        /// <summary>
        /// 当前电表剩余电量
        /// </summary>
        [Display(Name = "剩余电量")]
        RdVal = 9011,

        #region 2017-8-5 增加电流电压等采集项目        
        /// <summary>
        /// 正向无功总电能
        /// </summary>
        [Display(Name = "正向无功总电能")]
        EAnt = 9101,

        /// <summary>
        /// A相电流
        /// </summary>
        [Display(Name = "A相电流")]
        Ia = 9105,
        /// <summary>
        /// B相电流
        /// </summary>
        [Display(Name = "B相电流")]
        Ib = 9106,
        /// <summary>
        /// C相电流
        /// </summary>
        [Display(Name = "C相电流")]
        Ic = 9107,
        /// <summary>
        /// A相电压
        /// </summary>
        [Display(Name = "A相电压")]
        Ua = 9108,
        /// <summary>
        /// B相电压
        /// </summary>
        [Display(Name = "B相电压")]
        Ub = 9109,
        /// <summary>
        /// C相电压
        /// </summary>
        [Display(Name = "C相电压")]
        Uc = 9110,
        /// <summary>
        /// 三相有功功率
        /// </summary>        
        [Display(Name = "三相有功功率")]
        Psum = 9111,
        /// <summary>
        /// A相有功功率
        /// </summary>        
        [Display(Name = "A相有功功率")]
        Pa = 9112,
        /// <summary>
        /// B相有功功率
        /// </summary>
        [Display(Name = "B相有功功率")]
        Pb = 9114,
        /// <summary>
        /// C相有功功率
        /// </summary>
        [Display(Name = "C相有功功率")]
        Pc = 9115,
        /// <summary>
        /// 三相无功功率
        /// </summary>        
        [Display(Name = "A三相无功功率")]
        Qsum = 9116,
        /// <summary>
        /// A相有功功率
        /// </summary>        
        [Display(Name = "A相有功功率")]
        Qa = 9117,
        /// <summary>
        /// B相有功功率
        /// </summary>
        [Display(Name = "B相有功功率")]
        Qb = 9118,
        /// <summary>
        /// C相有功功率
        /// </summary>
        [Display(Name = "C相有功功率")]
        Qc = 9119,
        /// <summary>
        /// A相功率因数
        /// </summary>
        [Display(Name = "三相功率因数")]
        Pfav = 9120,
        /// <summary>
        /// A相功率因数
        /// </summary>
        [Display(Name = "A相功率因数")]
        PFa = 9121,
        /// <summary>
        /// B相功率因数
        /// </summary>
        [Display(Name = "B相功率因数")]
        PFb = 9122,
        /// <summary>
        /// C相功率因数
        /// </summary>
        [Display(Name = "C相功率因数")]
        PFc = 9123,
        #endregion

        /// <summary>
        /// 拉合闸状态
        /// </summary>
        [Display(Name = "拉合闸状态")]
        Ssr = 9999,
        /// <summary>
        /// 合闸
        /// </summary>
        //[Display(Name = "合闸")]
        //Ssr0 = 8999,
        ///// <summary>
        ///// 拉闸
        ///// </summary>
        //[Display(Name = "拉闸")]
        //Ssr1 = 8998,
        /// <summary>
        /// 充值
        /// </summary>
        [Display(Name = "充值")]
        Pay = 2500,
        /// <summary>
        /// 退费
        /// </summary>
        [Display(Name = "退费")]
        Refund = 2501,

        /// <summary>
        /// 保电(=1启用)
        /// </summary>
        [Display(Name = "保电(=1启用)")]
        IsPaul = 2502,

        /// <summary>
        /// 有功功率负荷限值(kW)
        /// </summary>
        [Display(Name = "有功功率负荷限值(kW)")]
        Nrp = 2503,
        /// <summary>
        /// 恶性负载判定有功功率增量(kW)(瞬时额定功率)
        /// </summary>
        [Display(Name = "瞬时额定功率(kW)")]
        McNrp = 2504,
        /// <summary>
        /// 恶性负载判定无功功率(kW)
        /// </summary>
        [Display(Name = "恶性负载判定无功功率(kW)")]
        Wpf = 2505,
        /// <summary>
        /// 违规用电判定功能的功率下限(kW)
        /// </summary>
        [Display(Name = "违规用电判定功能的功率下限(kW)")]
        Uep = 2506,
        /// <summary>
        /// 一天内违规断电次数限值(0~255)
        /// </summary>
        [Display(Name = "一天内违规断电次数限值(0~255)")]
        Miss = 2507,
        /// <summary>
        /// 违规断电后重启时间(S)
        /// </summary>
        [Display(Name = "违规断电后重启时间(S)")]
        Rest = 2508,
        /// <summary>
        /// 单次功率增量限值(kW)
        /// </summary>
        [Display(Name = "单次功率增量限值(kW)")]
        Saf = 2509,
        /// <summary>
        /// 过流门限值(A)
        /// </summary>
        [Display(Name = "过流门限值(A)")]
        Avlm = 2510,
        /// <summary>
        /// 启用移相器监测功率下限(kW)
        /// </summary>
        [Display(Name = "移相器监测功率下限(kW)")]
        Phase = 2511,
        /// <summary>
        /// 设置阻行负载(kW)
        /// </summary>
        [Display(Name = "设置阻行负载(kW)")]
        Resist = 2512,
        /// <summary>
        /// 移相器(=1启用)
        /// </summary>
        [Display(Name = "移相器(=1启用)")]
        IsPhase = 2513,

        /// <summary>
        /// 继电器参数(=0电平式A路继电器设置;=1电平式B路继电器设置;=93脉冲式设置)
        /// </summary>
        [Display(Name = "继电器参数")]
        IsRelay = 2514,
        #region 2017.09.06
        /// <summary>
        /// 获取定时断送电表
        /// </summary>
        [Display(Name = "获取定时断送电表")]
        TiVal = 2515,
        #region 2018.07.02河北工业大学特殊定制（硬件硬塞的东西一群SB）
        /// <summary>
        /// 增电
        /// </summary>
        [Display(Name = "充电")]
        PaySb1 = 2016,
        /// <summary>
        /// 增电
        /// </summary>
        [Display(Name = "增电")]
        PaySb2 = 2017,
        #endregion
        #endregion

        /// <summary>
        /// 待机检测(=1检测)
        /// </summary>
        [Display(Name = "待机检测")]
        IsStdby = 2018,

        #region 2019.03.19 可读写
        /// <summary>
        /// 报警电量 1 限值
        /// </summary>
        [Display(Name = "报警电量 1 限值")]
        Alarm1 = 2019,
        /// <summary>
        /// 报警电量 2 限值
        /// </summary>
        [Display(Name = "报警电量 2 限值")]
        Alarm2 = 2020,
        /// <summary>
        /// 囤积电量限
        /// </summary>
        [Display(Name = "囤积电量限")]
        Collect = 2021,
        /// <summary>
        /// 透支电量限值
        /// </summary>
        [Display(Name = "透支电量限值")]
        Overdraft = 2022,
        #endregion

        /// <summary>
        /// 记录恶性负载事件
        /// </summary>
        [Display(Name = "恶性负载")]
        EventWpf = 3001,


        #region 2017.07.06
        [Display(Name = "漏电流一")]
        Leak1 = 4001,
        [Display(Name = "漏电流二")]
        Leak2 = 4002,
        [Display(Name = "漏电流三")]
        Leak3 = 4003,
        [Display(Name = "漏电流四")]
        Leak4 = 4004,
        [Display(Name = "漏电流五")]
        Leak5 = 4005,
        [Display(Name = "漏电流六")]
        Leak6 = 4006,
        [Display(Name = "漏电流七")]
        Leak7 = 4007,
        [Display(Name = "漏电流八")]
        Leak8 = 4008,
        [Display(Name = "温度一")]
        Temp1 = 4009,
        [Display(Name = "温度二")]
        Temp2 = 4010,
        [Display(Name = "漏电设备告警")]
        LeakAlarm = 4011,
        #endregion
        #region 2017.08.22
        [Display(Name = "电流阀值一")]
        MaxLeak1 = 4012,
        [Display(Name = "电流阀值二")]
        MaxLeak2 = 4013,
        [Display(Name = "电流阀值三")]
        MaxLeak3 = 4014,
        [Display(Name = "电流阀值四")]
        MaxLeak4 = 4015,
        [Display(Name = "电流阀值五")]
        MaxLeak5 = 4016,
        [Display(Name = "电流阀值六")]
        MaxLeak6 = 4017,
        [Display(Name = "电流阀值七")]
        MaxLeak7 = 4018,
        [Display(Name = "电流阀值八")]
        MaxLeak8 = 4019,
        [Display(Name = "温度阀值一")]
        MaxTemp1 = 4020,
        [Display(Name = "温度阀值二")]
        MaxTemp2 = 4021,
        [Display(Name = "温度阀值三")]
        MaxTemp3 = 4022,
        [Display(Name = "温度阀值四")]
        MaxTemp4 = 4023,
        [Display(Name = "电流使能一")]
        StatusLeak1 = 4024,
        [Display(Name = "电流使能二")]
        StatusLeak2 = 4025,
        [Display(Name = "电流使能三")]
        StatusLeak3 = 4026,
        [Display(Name = "电流使能四")]
        StatusLeak4 = 4027,
        [Display(Name = "电流使能五")]
        StatusLeak5 = 4028,
        [Display(Name = "电流使能六")]
        StatusLeak6 = 4029,
        [Display(Name = "电流使能七")]
        StatusLeak7 = 4030,
        [Display(Name = "电流使能八")]
        StatusLeak8 = 4031,
        [Display(Name = "温度使能一")]
        StatusTemp1 = 4032,
        [Display(Name = "温度使能二")]
        StatusTemp2 = 4033,
        [Display(Name = "温度使能三")]
        StatusTemp3 = 4034,
        [Display(Name = "温度使能四")]
        StatusTemp4 = 4035,
        #endregion

    }

    /// <summary>
    /// 发送结果
    /// </summary>
    public enum HdType
    {
        /// <summary>
        /// 发送QF
        /// </summary>
        [Display(Name = "发送QF")]
        AL_QF = 100,
        /// <summary>
        /// 发送短信告警
        /// </summary>
        [Display(Name = "短信告警")]
        AL_Sms = 200,
        /// <summary>
        /// 发送短信告警
        /// </summary>
        [Display(Name = "邮件告警")]
        AL_Email = 201,
        /// <summary>
        /// 发送微信告警
        /// </summary>
        [Display(Name = "微信告警")]
        AL_Wx = 202
    }

    /// <summary>
    /// 告警类型
    /// </summary>
    public enum AlarmType
    {

        /// <summary>
        /// 发送短信告警
        /// </summary>
        [Display(Name = "短信告警")]
        Alarm = 200,

        ///// <summary>
        ///// 发送QF
        ///// </summary>
        //[Display(Name = "发送QF")]
        //AL_QF = 100,
        ///// <summary>
        ///// 发送短信告警
        ///// </summary>
        //[Display(Name = "短信告警")]
        //AL_Sms = 200,
        ///// <summary>
        ///// 发送短信告警
        ///// </summary>
        //[Display(Name = "邮件告警")]
        //AL_Email = 201,
        ///// <summary>
        ///// 发送微信告警
        ///// </summary>
        //[Display(Name = "微信告警")]
        //AL_Wx = 202,

        /// <summary>
        /// 发送短信告警
        /// </summary>
        [Display(Name = "恶性负载事件")]
        EV_Malig = 1000,

        /// <summary>
        /// 电气火灾告警
        /// </summary>
        [Display(Name = "电气火灾告警")]
        AL_Leak = 1001,

        //
        // 摘要:
        // 用电告警
        [Display(Name = "用电告警")]
        Al_Use = 2000,

        // 摘要:
        // 告警成功
        [Display(Name = "运行成功")]
        Sue = 1,
        //
        // 摘要:
        //     通讯错误 -1000
        [Display(Name = "通讯错误")]
        Err_Tx = -1000,
        //
        // 摘要:
        //     电表类型配置错误 -2000
        [Display(Name = "电表类型配置错误")]
        Err_Pz = -2000,
        /// <summary>
        /// 设备类告警
        /// </summary>
        [Display(Name = "设备告警")]
        Err_Sb = -3000,
        //
        // 摘要:
        // 数据库类错误 -4000
        [Display(Name = "入库错误")]
        Err_Db = -4000,
        //
        // 摘要:
        // 线程类错误 -5000
        [Display(Name = "线程类错误")]
        Err_Th = -5000,
        //
        // 摘要:
        // 电量反转 -6000
        [Display(Name = "电量反转")]
        Ez_1 = -6000,
        //
        // 摘要:
        // 时间反转 -6001
        [Display(Name = "时间反转")]
        Ez_2 = -6001,
        //
        // 摘要:
        // 越限 -7001
        [Display(Name = "越限")]
        Err_OFlow = -7001,
        //
        // 摘要:
        // 命令错误 -9998
        [Display(Name = "没有定义指令")]
        Err_Nd = -9998,
        //
        // 摘要:
        // 命令错误 -9999
        [Display(Name = "其他错误")]
        Err_Qt = -9999,
    }

    /// <summary>
    /// 类型
    /// </summary>
    public enum CoType
    {
        /// <summary>
        /// 站点类型 宏站
        /// </summary>
        [Display(Name = "宏站")]
        RmBg = 10,
        /// <summary>
        /// 站点类型 宏站
        /// </summary>
        [Display(Name = "室分")]
        RmSa = 20,
    }

    /// <summary>
    /// 供电类型
    /// </summary>
    public enum Switch
    {
        /// <summary>
        /// 转供
        /// </summary>
        [Display(Name = "转供电")]
        RmAc = 10,
        /// <summary>
        /// 直供
        /// </summary>
        [Display(Name = "直供电")]
        RmDc = 20,
        /// <summary>
        /// 交流电
        /// </summary>
        [Display(Name = "交流电")]
        MdAc = 30,
        /// <summary>
        /// 直流电
        /// </summary>
        [Display(Name = "直流电")]
        MdDc = 40,

    }

    /// <summary>
    /// 缴费方式
    /// </summary>
    public enum ChargAttrib
    {
        /// <summary>
        /// 房间预付费
        /// </summary>
        [Display(Name = "房间预付费")]
        RmCharg = 1,
        /// <summary>
        /// 房间后付费
        /// </summary>
        [Display(Name = "房间后付费")]
        RmPost = 2,
        /// <summary>
        /// 设备预付费
        /// </summary>
        [Display(Name = "设备预付费")]
        MdCharg = 3,
        /// <summary>
        /// 设备后付费
        /// </summary>
        [Display(Name = "设备后付费")]
        MdPost = 4,
    }

    /// <summary>
    /// 通信方式 通信类型=0 com口，=1 TCP/Client，=2 UDP/Client , =3 TCP/Server
    /// </summary>
    public enum TfAttrib
    {
        /// <summary>
        /// COM口
        /// </summary>
        [Display(Name = "COM口")]
        Com = 0,
        /// <summary>
        /// TCP/Client
        /// </summary>
        [Display(Name = "TCP/Client")]
        T_Client = 1,
        /// <summary>
        /// UDP/Client
        /// </summary>
        [Display(Name = "UDP/Client")]
        U_Client = 2,
        /// <summary>
        /// TCP/Server
        /// </summary>
        [Display(Name = "TCP/Server")]
        T_Server = 3,
    }


    /// <summary>
    /// 监听功能列表
    /// </summary>
    public enum ListenCFun
    {
        /// <summary>
        /// 增加rdc变量
        /// </summary>
        [Display(Name = "增加rdc变量")]
        addvar = 1,
        /// <summary>
        /// 更新配置
        /// </summary>
        [Display(Name = "更新配置")]
        config = 2,
        /// <summary>
        /// 远程控制
        /// </summary>
        [Display(Name = "远程控制")]
        cmd = 3,
        /// <summary>
        /// 远程控制
        /// </summary>
        [Display(Name = "获取采集数据")]
        collect = 4,
    }

    /// <summary>
    /// 单位
    /// </summary>
    public enum RateUnit
    {
        [Display(Name = "平方米")]
        Area = 0,
        [Display(Name = "户数")]
        Bank = 1,
    }
    /// <summary>
    /// 计算规则
    /// </summary>
    public enum RateRule
    {
        [Display(Name = "正常")]
        Normal = 0,
        [Display(Name = "时间范围")]
        Time = 1,
        [Display(Name = "数量范围")]
        Number = 2,
    }

    /// <summary>
    /// 设备类型定义;300 到400之间使用是能源使用
    /// </summary>
    public enum MmDefine
    {
        [Display(Name = "电能")]
        MM = 0,
        [Display(Name = "用水")]
        WTM = 300,
        [Display(Name = "天然气")]
        GAS = 310,
        [Display(Name = "冷热")]
        KH = 320,
        [Display(Name = "压缩空气")]
        YSKS = 330,
        [Display(Name = "He+Ar+O2瓶装气体")]
        HAO = 340,
        [Display(Name = "Ar+CO2保护气体")]
        ACO = 350,
        [Display(Name = "N2氮气")]
        N2 = 360,
    }

    /// <summary>
    /// 充值类型
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 充值
        /// </summary>
        [Display(Name = "充值")]
        pay = 1,
        /// <summary>
        /// 退费
        /// </summary>
        [Display(Name = "退费")]
        refund = 2,
        /// <summary>
        /// 赠送
        /// </summary>
        [Display(Name = "赠送")]
        present = 3,
        /// <summary>
        /// 退赠送电
        /// </summary>
        [Display(Name = "退赠送电")]
        reback = 4,
        /// <summary>
        /// 补电
        /// </summary>
        [Display(Name = "补电")]
        fill = 5,
    }

    /// <summary>
    /// 设备属性
    /// </summary>
    public enum MdAttrib
    {
        /// <summary>
        /// 馈线
        /// </summary>
        [Display(Name = "馈线")]
        outline = 0,
        /// <summary>
        /// 进线
        /// </summary>
        [Display(Name = "进线")]
        incoming = 1,
        /// <summary>
        /// 赠送
        /// </summary>
        [Display(Name = "其他")]
        other = 2,
    }

    [AttributeUsage(AttributeTargets.All)]
    public class Describe : Attribute
    {
        /// <summary>
        /// 页面描述
        /// </summary>
        private string _describe = "";
        /// <summary>  
        /// 描述  
        /// </summary>  
        public string describe { get { return _describe; } }
        /// <summary>
        /// 描述
        /// </summary>
        /// <param name="prog_id">权限ID，为空不需检查权限</param>
        /// <param name="describe">页面描述</param>
        public Describe(string describe)
        {
            this._describe = describe;
        }
    }
}
