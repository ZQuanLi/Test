using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YDS6000.Models
{

    public class Redis11111
    {
        public int Ledger { get; set; }
        public int Module_id { get; set; }
        public string ModuleAddr { get; set; }
        public int Fun_id { get; set; }
        public int Action { get; set; }
        public bool IsDb { get; set; }
        /// <summary>
        /// 更新入库时间
        /// </summary>
        public DateTime UpTime = new DateTime(1900, 1, 1);
        public RstVar RstVar { get; set; }

    }

    /// <summary>
    /// memcached结果集
    /// </summary>
    [Serializable]
    public class RstVar
    {
        /// <summary>
        /// 值
        /// </summary>
        public string lpszVal { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime lpszdateTime { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public UInt32 dwUserData { get; set; }
    }

    /// <summary>
    /// 监听结果
    /// </summary>
    [Serializable]
    public class ListenVModel
    {
        /// <summary>
        /// 项目
        /// </summary>
        public string project = "";
        /// <summary>
        /// 功能
        /// </summary>
        public string cfun = "";
        /// <summary>
        /// 执行内容
        /// </summary>
        public string content = "";
    }

    /// <summary>
    /// 采集命令
    /// </summary>
    [Serializable]
    public class CommandVModel
    {
        public int Ledger { get; set; }

        private bool _isui = false;
        public int Esp_id { get; set; }
        public DateTime? CollectTime { get; set; }
        /// <summary>
        /// 所属结构
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 是否前台命令（前台命令优先）
        /// </summary>
        public bool IsUI { get { return _isui; } set { _isui = value; } }
        public string EspAddr { get; set; }
        public string ComPort { get; set; }
        public int Baud { get; set; }
        public int DataBit { get; set; }
        public int Parity { get; set; }
        public int StopBit { get; set; }
        //
        // 摘要:
        //     网关IP
        public string Ip { get; set; }
        //
        // 摘要:
        //     TCP端口
        public int TcpPort { get; set; }
        //
        // 摘要:
        //     超时
        public int TimeOut { get; set; }
        //
        // 摘要:
        //     被谁处理的
        public string HandledBY { get; set; }
        //
        // 摘要:
        //     型号如DSS3366
        public string ModuleType { get; set; }
        //
        // 摘要:
        //     电表密码
        public string ModulePwd { get; set; }
        //
        // 摘要:
        //     电表操作者
        public string ModuleUid { get; set; }

        //
        // 摘要:
        //     通信类型=0 com口，=1 Tcp服务端，=2 UDP服务端 =99 模拟器
        public int TransferType { get; set; }
        //
        public int Module_id { get; set; }
        public string ModuleAddr { get; set; }
        public int Fun_id { get; set; }
        public string FunName { get; set; }
        public string FunType { get; set; }
        //
        // 摘要:
        //     协议类型 modbus 及 2007
        public string Protocol { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string DataValue { get; set; }
        public int Co_id { get; set; }
        /// <summary>
        /// 0读，1写
        /// </summary>
        public int Action { get; set; }
        /// <summary>
        /// 变量TagName
        /// </summary>
        public string LpszDbVarName { get; set; }
        /// <summary>
        /// 最新采集值
        /// </summary>
        public decimal LastVal { get; set; }
        /// <summary>
        /// 最新采集时间
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 命令ID号
        /// </summary>
        public long Log_id { get; set; }
        /// <summary>
        /// 是否先运行再记录到数据库(true 先运行)
        /// </summary>
        public bool IsNDb { get; set; }
        /// <summary>
        /// 动作描述
        /// </summary>
        public string Descr { get; set; }
        /// <summary>
        /// 错误次数
        /// </summary>
        public int ErrCnt { get; set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public string MeterAddr { get; set; }

        private DateTime _create_dt = DateTime.Now;
        private int _create_by = 1;
        /// <summary>
        /// 建立人
        /// </summary>
        public int Create_by { get { return _create_by; } set { _create_by = value; } }

        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime Create_dt { get { return _create_dt; } set { _create_dt = value; } }

        private int _isrn = 0;
        /// <summary>
        /// 命令标示
        /// </summary>
        public int IsRn { get { return _isrn; } set { _isrn = value; } }
        /// <summary>
        /// 利用序列化实现深度复制
        /// </summary>
        /// <returns></returns>
        public CommandVModel Clone()
        {
            return JsonHelper.Deserialize<CommandVModel>(JsonHelper.Serialize(this));
        }

    }

    /// <summary>
    /// 恶性负载Json
    /// </summary>
    [Serializable]
    public class MalignantVModel
    {
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 远程状态字
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 断电时电压
        /// </summary>
        public decimal Ua { get; set; }
        /// <summary>
        /// 断电时电流
        /// </summary>
        public decimal Ia { get; set; }
        /// <summary>
        /// 有功功率
        /// </summary>
        public decimal Psum1 { get; set; }
        /// <summary>
        /// 无功功率
        /// </summary>
        public decimal Psum2 { get; set; }
        /// <summary>
        /// 视在功率
        /// </summary>
        public decimal Psum3 { get; set; }
        /// <summary>
        /// 恶性功率
        /// </summary>
        public decimal Psum4 { get; set; }

    }

    /// <summary>
    /// API实时采集类
    /// </summary>
    public class ApiVar
    {
        /// <summary>
        /// 变量名
        /// </summary>
        [Required(ErrorMessage = "请输入变量名")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "变量名")]
        public string tagName { get; set; }
        /// <summary>
        /// 采集值
        /// </summary>
        [Required(ErrorMessage = "请输入采集值")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "采集值")]
        public string value { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        [Required(ErrorMessage = "请输入采集采集时间")]
        [Display(Name = "采集采集时间")]
        public DateTime time { get; set; }
    }
}
