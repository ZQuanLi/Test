using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YDS6000.Models
{
    /// <summary>
    /// v1_gateway:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_gatewayVModel
    {
        public v1_gatewayVModel()
        { }
        #region Model
        private int _gw_id;
        private string _gwname;
        private string _gwaddr;
        private string _gwip;
        private int _gwport;
        private int _timeout = 1000;
        private string _gwtype = "";
        private string _gprsnum = "";
        private string _inst_loc;
        private string _remark;
        private int _disabled = 0;
        /// <summary>
        /// ID号(自动生成)
        /// </summary>
        public int Gw_id
        {
            set { _gw_id = value; }
            get { return _gw_id; }
        }
        /// <summary>
        /// 集中器名称
        /// </summary>
        public string GwName
        {
            set { _gwname = value; }
            get { return _gwname; }
        }
        /// <summary>
        /// 集中器地址
        /// </summary>
        public string GwAddr
        {
            set { _gwaddr = value; }
            get { return _gwaddr; }
        }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string GwIp
        {
            set { _gwip = value; }
            get { return _gwip; }
        }
        /// <summary>
        /// 端口
        /// </summary>
        public int GwPort
        {
            set { _gwport = value; }
            get { return _gwport; }
        }
        /// <summary>
        /// 超时毫秒
        /// </summary>
        public int Timeout
        {
            set { _timeout = value; }
            get { return _timeout; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string GwType
        {
            set { _gwtype = value; }
            get { return _gwtype; }
        }
        /// <summary>
        /// GPRS号码
        /// </summary>
        public string GPRSNum
        {
            set { _gprsnum = value; }
            get { return _gprsnum; }
        }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Inst_loc
        {
            set { _inst_loc = value; }
            get { return _inst_loc; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        #endregion Model

    }

    /// <summary>
    /// v1_gateway_esp:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_gateway_espVModel
    {
        public v1_gateway_espVModel()
        { }
        #region Model
        private int _esp_id = 0;
        private int _gw_id = 0;
        private string _espname = "";
        private string _espaddr = "";
        private int _transferType = 0;
        private string _espip = "";
        private int _espport = 0;
        private int _timeout = 1000;
        private string _esptype = "";
        private string _inst_loc = "";
        private string _comPort = "";
        private int _baud = 0;
        private int _dataBit = 0;
        private int _parity = 0;
        private int _stopBit = 0;
        private string _remark = "";
        private int _disabled = 0;
        /// <summary>
        /// 采集器ID号，自动生成
        /// </summary>
        public int Esp_id
        {
            set { _esp_id = value; }
            get { return _esp_id; }
        }
        /// <summary>
        /// 所属集中器
        /// </summary>
        public int Gw_id
        {
            set { _gw_id = value; }
            get { return _gw_id; }
        }
        /// <summary>
        /// 采集器名称
        /// </summary>
        public string EspName
        {
            set { _espname = value; }
            get { return _espname; }
        }
        /// <summary>
        /// 采集器地址
        /// </summary>
        public string EspAddr
        {
            set { _espaddr = value; }
            get { return _espaddr; }
        }
        /// <summary>
        /// 通信类型=0 com口，=1 Tcp，=2 UDP
        /// </summary>
        public int TransferType
        {
            set { _transferType = value; }
            get { return _transferType; }
        }
        /// <summary>
        /// 采集器IP
        /// </summary>
        public string EspIp
        {
            set { _espip = value; }
            get { return _espip; }
        }
        /// <summary>
        /// 采集器端口
        /// </summary>
        public int EspPort
        {
            set { _espport = value; }
            get { return _espport; }
        }
        /// <summary>
        /// 延时毫秒
        /// </summary>
        public int Timeout
        {
            set { _timeout = value; }
            get { return _timeout; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string EspType
        {
            set { _esptype = value; }
            get { return _esptype; }
        }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Inst_loc
        {
            set { _inst_loc = value; }
            get { return _inst_loc; }
        }
        /// <summary>
        /// 串口号
        /// </summary>
        public string ComPort
        {
            set { _comPort = value; }
            get { return _comPort; }
        }
        /// <summary>
        /// 串口波特率
        /// </summary>
        public int Baud
        {
            set { _baud = value; }
            get { return _baud; }
        }
        /// <summary>
        /// 串口数据位
        /// </summary>
        public int DataBit
        {
            set { _dataBit = value; }
            get { return _dataBit; }
        }
        /// <summary>
        /// 串口校验位
        /// </summary>
        public int Parity
        {
            set { _parity = value; }
            get { return _parity; }
        }
        /// <summary>
        /// 串口停止位
        /// </summary>
        public int StopBit
        {
            set { _stopBit = value; }
            get { return _stopBit; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        #endregion Model

    }

    /// <summary>
    /// v1_gateway_esp_module:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_gateway_esp_meterVModel
    {
        public v1_gateway_esp_meterVModel()
        { }
        #region Model
        private int _meter_id = 0;
        private int _esp_id = 0;
        private string _metername = "";
        private string _meteraddr = "";
        private string _inst_loc = "";
        private int _disabled = 0;
        private string _remark = "";
        private decimal _multiply = 1M;
        private int _mm_id = 0;//T '类型ID号',
        private string _meterNo = "";

        /// <summary>
        /// 电能表序号（自动生成）
        /// </summary>
        public int Meter_id
        {
            set { _meter_id = value; }
            get { return _meter_id; }
        }
        /// <summary>
        /// 所属采集器
        /// </summary>
        public int Esp_id
        {
            set { _esp_id = value; }
            get { return _esp_id; }
        }
        /// <summary>
        /// 电能表名称
        /// </summary>
        public string MeterName
        {
            set { _metername = value; }
            get { return _metername; }
        }
        /// <summary>
        /// 电能表地址
        /// </summary>
        public string MeterAddr
        {
            set { _meteraddr = value; }
            get { return _meteraddr; }
        }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string Inst_loc
        {
            set { _inst_loc = value; }
            get { return _inst_loc; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 倍率 变比
        /// </summary>
        public decimal Multiply
        {
            set { _multiply = value; }
            get { return _multiply; }
        }
        /// <summary>
        /// 所属类型
        /// </summary>
        public int Mm_id
        {
            set { _mm_id = value; }
            get { return _mm_id; }
        }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string MeterNo
        {
            set { _meterNo = value; }
            get { return _meterNo; }
        }
        #endregion Model

    }

    /// <summary>
    /// v1_gateway_esp_module:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_gateway_esp_moduleVModel
    {
        public v1_gateway_esp_moduleVModel()
        { }
        #region Model
        private int _module_id = 0;
        private int _meter_id = 0;
        private string _modulename = "";
        private string _moduleaddr = "";
        private string _moduletype = "";
        private string _inst_loc = "";
        private int _disabled = 0;
        private string _remark = "";
        private decimal _multiply = 1M;
        private string _energyitemcode = "01000";
        private int _mm_id = 0;//T '类型ID号',
        private int _co_id = 0;
        private int _parent_id = 0; // 加总组父ID
        //private int _isorg = 0; // 是否真的所属组织 1是
        private int _layer = 0; // 层级数
        private int _isAlarm = 0;// 是否发送告警
        private decimal _minpay = 0;
        private decimal _price = 0;
        private int _rate_id = 0;

        /// <summary>
        /// 电能表序号（自动生成）
        /// </summary>
        public int Module_id
        {
            set { _module_id = value; }
            get { return _module_id; }
        }
        /// <summary>
        /// 所属采集器
        /// </summary>
        public int Meter_id
        {
            set { _meter_id = value; }
            get { return _meter_id; }
        }
        /// <summary>
        /// 电能表名称
        /// </summary>
        public string ModuleName
        {
            set { _modulename = value; }
            get { return _modulename; }
        }
        /// <summary>
        /// 电能表地址
        /// </summary>
        public string ModuleAddr
        {
            set { _moduleaddr = value; }
            get { return _moduleaddr; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string ModuleType
        {
            set { _moduletype = value; }
            get { return _moduletype; }
        }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Inst_loc
        {
            set { _inst_loc = value; }
            get { return _inst_loc; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 倍率 变比
        /// </summary>
        public decimal Multiply
        {
            set { _multiply = value; }
            get { return _multiply; }
        }
        /// <summary>
        /// 电能项
        /// </summary>
        public string EnergyItemCode
        {
            set { _energyitemcode = value; }
            get { return _energyitemcode; }
        }
        /// <summary>
        /// 所属类型
        /// </summary>
        public int Mm_id
        {
            set { _mm_id = value; }
            get { return _mm_id; }
        }
        /// <summary>
        /// 所属组织
        /// </summary>
        public int Co_id
        {
            set { _co_id = value; }
            get { return _co_id; }
        }
        /// <summary>
        /// 所属父ID 
        /// </summary>
        public int Parent_id
        {
            set { _parent_id = value; }
            get { return _parent_id; }
        }
        /// <summary>
        /// 是否真的所属组织 1是
        /// </summary>
        //public int IsOrg
        //{
        //    set { _isorg = value; }
        //    get { return _isorg; }
        //}
        /// <summary>
        /// 层级数
        /// </summary>
        public int Layer
        {
            set { _layer = value; }
            get { return _layer; }
        }
        /// <summary>
        /// 是否发送告警
        /// </summary>
        public int IsAlarm
        {
            set { _isAlarm = value; }
            get { return _isAlarm; }
        }

        public decimal MinPay
        {
            set { _minpay = value; }
            get { return _minpay; }
        }

        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 费率ID号
        /// </summary>
        public int Rate_id
        {
            set { _rate_id = value; }
            get { return _rate_id; }
        }
        #endregion Model

    }

    /// <summary>
    /// v4_pay:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v4_pay_logVModel
    {
        public v4_pay_logVModel()
        {
            _payStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _payEndTime = _payStartTime.AddMonths(1).AddDays(-1);
        }
        #region Model
        private long _log_id ;
        private int _ledger = 0;
        private DateTime _cdate = new DateTime();
        private int _co_id;
        private int _module_id = 0;
        private int _fun_id = 0;
        private string _moduleaddr;
        private decimal _payval;
        private decimal _payamt;
        private int _paytype;
        private int _iswrong = 0;
        private int? _errcode = 0;
        private string _errtxt = "";
        private int? _ispay;
        private string _paytxt = "";
        private string _trade_no = "";
        private DateTime _payStartTime ;
        private DateTime _payEndTime ;
        private int _isCharg = 0;
        private int _chrgtype = 0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long Log_id
        {
            set { _log_id = value; }
            get { return _log_id; }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CDate
        {
            set { _cdate = value; }
            get { return _cdate; }
        }
        /// <summary>
        /// 组织ID号
        /// </summary>
        public int Co_id
        {
            set { _co_id = value; }
            get { return _co_id; }
        }
        /// <summary>
        /// 设备ID号
        /// </summary>
        public int Module_id
        {
            set { _module_id = value; }
            get { return _module_id; }
        }
        /// <summary>
        /// 设备地址
        /// </summary>
        public string ModuleAddr
        {
            set { _moduleaddr = value; }
            get { return _moduleaddr; }
        }
        /// <summary>
        /// 功能ID号
        /// </summary>
        public int Fun_id
        {
            set { _fun_id = value; }
            get { return _fun_id; }
        }
        /// <summary>
        /// 支付电度
        /// </summary>
        public decimal PayVal
        {
            set { _payval = value; }
            get { return _payval; }
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmt
        {
            set { _payamt = value; }
            get { return _payamt; }
        }
        /// <summary>
        /// 支付类型 充值类型=1充值=2退电=3赠送电
        /// </summary>
        public int PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// 是否错账处理 =0非错帐处理=1错账处理
        /// </summary>
        public int IsWrong
        {
            set { _iswrong = value; }
            get { return _iswrong; }
        }
        /// <summary>
        /// 是否充值成功=0初始状态;<0充值失败;>0充值成功
        /// </summary>
        public int? ErrCode
        {
            set { _errcode = value; }
            get { return _errcode; }
        }
        /// <summary>
        /// 系统错误信息
        /// </summary>
        public string ErrTxt
        {
            set { _errtxt = value; }
            get { return _errtxt; }
        }        
        /// <summary>
        /// 支付类型=0系统在线支付，=1微信支付
        /// </summary>
        public int? IsPay
        {
            set { _ispay = value; }
            get { return _ispay; }
        }       
        /// <summary>
        /// 支付商品号
        /// </summary>
        public string Trade_no
        {
            set { _trade_no = value; }
            get { return _trade_no; }
        }
        /// <summary>
        /// 支付说明
        /// </summary>
        public string PayTxt
        {
            set { _paytxt = value; }
            get { return _paytxt; }
        }
        /// <summary>
        /// 支付起启时间
        /// </summary>
        public DateTime PayStartTime
        {
            set { _payStartTime = value; }
            get { return _payStartTime; }
        }
        /// <summary>
        /// 支付结束时间
        /// </summary>
        public DateTime PayEndTime
        {
            set { _payEndTime = value; }
            get { return _payEndTime; }
        }
        /// <summary>
        /// =0电费型;=1金额型
        /// </summary>
        public int IsCharg
        {
            set { _isCharg = value; }
            get { return _isCharg; }
        }
        /// <summary>
        /// 付费(充值)类型=0预付费;=1后付费
        /// </summary>
        public int ChrgType
        {
            set { _chrgtype = value; }
            get { return _chrgtype; }
        }

        /// <summary>
        /// 支付单价
        /// </summary>
        public decimal Price
        {
            set; get;
        }
        #endregion Model

    }

    /// <summary>
    /// v0_module:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v0_moduleVModel
    {
        public v0_moduleVModel()
        { }
        #region Model
        private int _mm_id;
        private string _modulename;
        private string _moduletype;
        private int _disabled = 0;
        private string _modulepwd;
        private string _moduleuid;
        private string _level;
        private string _spec = "";
        private string _fty_prod = "";
        private int _ischarg = 0;
        private string _protocol = "";
        /// <summary>
        /// 自增长ID号
        /// </summary>
        public int Mm_id
        {
            set { _mm_id = value; }
            get { return _mm_id; }
        }
        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModuleName
        {
            set { _modulename = value; }
            get { return _modulename; }
        }
        /// <summary>
        /// 型号
        /// </summary>
        public string ModuleType
        {
            set { _moduletype = value; }
            get { return _moduletype; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 编程密码
        /// </summary>
        public string ModulePwd
        {
            set { _modulepwd = value; }
            get { return _modulepwd; }
        }
        /// <summary>
        /// 编程操作人
        /// </summary>
        public string ModuleUid
        {
            set { _moduleuid = value; }
            get { return _moduleuid; }
        }
        /// <summary>
        /// 密码级别
        /// </summary>
        public string Level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 接线方式
        /// </summary>
        public string Spec
        {
            set { _spec = value; }
            get { return _spec; }
        }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Fty_prod
        {
            set { _fty_prod = value; }
            get { return _fty_prod; }
        }
        /// <summary>
        /// 计费方式
        /// </summary>
        public int IsCharg
        {
            set { _ischarg = value; }
            get { return _ischarg; }
        }
        /// <summary>
        /// 协议类型
        /// </summary>
        public string Protocol
        {
            set { _protocol = value; }
            get { return _protocol; }
        }
        
        #endregion Model

    }


	/// <summary>
	/// v2_command:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class v2_commandVModel
	{
        public v2_commandVModel()
		{}
		#region Model
		private long _log_id;
		private DateTime _cdate;
		private int _co_id;
		private int _module_id;
		private string _moduleaddr;
		private int _fun_id;
		private string _funtype;
		private string _datavalue;
		private int _errcode=0;
		private string _errtxt;
		private DateTime? _endtime;
		/// <summary>
		/// auto_increment
		/// </summary>
		public long Log_id
		{
			set{ _log_id=value;}
			get{return _log_id;}
		}
		/// <summary>
		/// 建立日期
		/// </summary>
		public DateTime CDate
		{
			set{ _cdate=value;}
			get{return _cdate;}
		}

		/// <summary>
		/// 组织ID
		/// </summary>
		public int Co_id
		{
			set{ _co_id=value;}
			get{return _co_id;}
		}
		/// <summary>
		/// 设备ID
		/// </summary>
		public int Module_id
		{
			set{ _module_id=value;}
			get{return _module_id;}
		}
		/// <summary>
		/// 设备地址
		/// </summary>
		public string ModuleAddr
		{
			set{ _moduleaddr=value;}
			get{return _moduleaddr;}
		}
		/// <summary>
		/// 功能ID号
		/// </summary>
		public int Fun_id
		{
			set{ _fun_id=value;}
			get{return _fun_id;}
		}
		/// <summary>
		/// 功能码
		/// </summary>
		public string FunType
		{
			set{ _funtype=value;}
			get{return _funtype;}
		}
		/// <summary>
		/// 值
		/// </summary>
		public string DataValue
		{
			set{ _datavalue=value;}
			get{return _datavalue;}
		}
		/// <summary>
		/// 错误代号
		/// </summary>
		public int ErrCode
		{
			set{ _errcode=value;}
			get{return _errcode;}
		}
		#endregion Model

	}

    

    /// <summary>
    /// ydm_si_ssr:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_si_ssrVModel
    {
        public v1_si_ssrVModel()
        { }
        #region Model
        private string _ledger;
        private int _si_id;
        private string _descr;
        private string _sissr;
        private string _md;
        private string _wk;
        private string _ts;
        private int _disabled;
        private string _create_by;
        private DateTime _create_dt;
        private string _update_by;
        private DateTime? _update_dt;
        /// <summary>
        /// 账目
        /// </summary>
        public string ledger
        {
            set { _ledger = value; }
            get { return _ledger; }
        }
        /// <summary>
        /// 策略ID
        /// </summary>
        public int si_id
        {
            set { _si_id = value; }
            get { return _si_id; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string descr
        {
            set { _descr = value; }
            get { return _descr; }
        }
        /// <summary>
        /// 定义策略
        /// </summary>
        public string siSSR
        {
            set { _sissr = value; }
            get { return _sissr; }
        }
        /// <summary>
        /// 月+日期组合策略
        /// </summary>
        public string md
        {
            set { _md = value; }
            get { return _md; }
        }
        /// <summary>
        /// 星期组合策略
        /// </summary>
        public string wk
        {
            set { _wk = value; }
            get { return _wk; }
        }
        /// <summary>
        /// 特殊日期组合策略
        /// </summary>
        public string ts
        {
            set { _ts = value; }
            get { return _ts; }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 建立人
        /// </summary>
        public string create_by
        {
            set { _create_by = value; }
            get { return _create_by; }
        }
        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime create_dt
        {
            set { _create_dt = value; }
            get { return _create_dt; }
        }
        /// <summary>
        /// 更新人
        /// </summary>
        public string update_by
        {
            set { _update_by = value; }
            get { return _update_by; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_dt
        {
            set { _update_dt = value; }
            get { return _update_dt; }
        }
        #endregion Model

    }



    /// <summary>
    /// v1_cust:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_custInfoVModel
    {
        public v1_custInfoVModel()
        { }
        #region Model
        private int _co_id = 0;
        private int _cono = 0;
        private string _coname;
        private int _disabled = 0;
        private int _parent_id = 0;
        private int _attrib = 0;/*属性，9000为房间，基站等信息*/
        private int _layer = 0;/*层级数*/
        private string _custaddr = "";
        private string _office_tel = "";
        private string _mobile = "";
        private string _email = "";
        private int _isdefine = 0; /*附加属性*/
        private string _strucname = "";/*全名*/

        //public int Ledger { get; set; }
        /// <summary>
        /// 客户ID号
        /// </summary>
        public int Co_id
        {
            set { _co_id = value; }
            get { return _co_id; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        public int CoNo
        {
            set { _cono = value; }
            get { return _cono; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string CoName
        {
            set { _coname = value; }
            get { return _coname; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 父ID号
        /// </summary>
        public int Parent_id
        {
            set { _parent_id = value; }
            get { return _parent_id; }
        }
        /// <summary>
        /// 属性，9000为房间，基站等信息
        /// </summary>
        public int Attrib
        {
            set { _attrib = value; }
            get { return _attrib; }
        }
        /// <summary>
        /// 层级数
        /// </summary>
        public int Layer
        {
            set { _layer = value; }
            get { return _layer; }
        }
        /// <summary>
        /// 客户地址
        /// </summary>
        public string CustAddr
        {
            set { _custaddr = value; }
            get { return _custaddr; }
        }
        /// <summary>
        /// 办公电话
        /// </summary>
        public string Office_tel
        {
            set { _office_tel = value; }
            get { return _office_tel; }
        }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 附加属性
        /// </summary>
        public int IsDefine
        {
            set { _isdefine = value; }
            get { return _isdefine; }
        }
        /// <summary>
        /// 全名
        /// </summary>
        public string StrucName
        {
            set { _strucname = value; }
            get { return _strucname; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion Model

    }


    /// <summary>
    /// v3_user:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v3_userVModel
    {
        public v3_userVModel()
        { }
        #region Model
        private int _ledger;
        private int _crm_id;
        private int _co_id;
        private int _isAdmin;
        private int _ishold;
        private string _crmno;
        private string _crmname;
        private string _mphone;
        private string _email;
        private string _phone;
        private string _remark;
        private string _passwd;
        private int _create_by;
        private DateTime _create_dt;
        private int _update_by;
        private DateTime _update_dt;
        private string _contract;
        private string _moduleName;
        private string _period = "";
        public int IsHold
        {
            set { _ishold = value; }
            get { return _ishold; }
        }
        public int IsAdmin
        {
            set { _isAdmin = value; }
            get { return _isAdmin; }
        }

        public int Co_id
        {
            set { _co_id = value; }
            get { return _co_id; }
        }



        /// <summary>
        /// 
        /// </summary>
        public int Ledger
        {
            set { _ledger = value; }
            get { return _ledger; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Crm_id
        {
            set { _crm_id = value; }
            get { return _crm_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CrmNo
        {
            set { _crmno = value; }
            get { return _crmno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CrmName
        {
            set { _crmname = value; }
            get { return _crmname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MPhone
        {
            set { _mphone = value; }
            get { return _mphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Passwd
        {
            set { _passwd = value; }
            get { return _passwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Create_by
        {
            set { _create_by = value; }
            get { return _create_by; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Create_dt
        {
            set { _create_dt = value; }
            get { return _create_dt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update_by
        {
            set { _update_by = value; }
            get { return _update_by; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Update_dt
        {
            set { _update_dt = value; }
            get { return _update_dt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Contract
        {
            set { _contract = value; }
            get { return _contract; }
        }
        /// <summary>
        /// 届信息
        /// </summary>
        public string Period
        {
            set { _period = value; }
            get { return _period; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ModuleName
        {
            set { _moduleName = value; }
            get { return _moduleName; }
        }


        #endregion Model

    }



    /// <summary>
    /// v1_rate:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v1_rateVModel
    {
        public v1_rateVModel()
        { }
        #region Model
        private int _ledger;
        private int _rate_id;
        private string _descr;
        private decimal _pri1st = 0.0000M;
        private decimal _pri2nd = 0.0000M;
        private decimal _pri3rd = 0.0000M;
        private decimal _pri4th = 0.0000M;
        private string _datacfg;
        private int _disabled = 0;
        private int _attrib = 0;
        private int _rule = 0;
        private string _unit = "";
        private decimal _unitBase = 1;
        /// <summary>
        /// 账目
        /// </summary>
        public int Ledger
        {
            set { _ledger = value; }
            get { return _ledger; }
        }
        /// <summary>
        /// 费率ID
        /// </summary>
        public int Rate_id
        {
            set { _rate_id = value; }
            get { return _rate_id; }
        }
        /// <summary>
        /// 费率描述
        /// </summary>
        public string Descr
        {
            set { _descr = value; }
            get { return _descr; }
        }
        /// <summary>
        /// 尖 单价
        /// </summary>
        public decimal Pri1st
        {
            set { _pri1st = value; }
            get { return _pri1st; }
        }
        /// <summary>
        /// 峰 单价
        /// </summary>
        public decimal Pri2nd
        {
            set { _pri2nd = value; }
            get { return _pri2nd; }
        }
        /// <summary>
        /// 平 单价
        /// </summary>
        public decimal Pri3rd
        {
            set { _pri3rd = value; }
            get { return _pri3rd; }
        }
        /// <summary>
        /// 谷 单价
        /// </summary>
        public decimal Pri4th
        {
            set { _pri4th = value; }
            get { return _pri4th; }
        }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public string DataCfg
        {
            set { _datacfg = value; }
            get { return _datacfg; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }

        /// <summary>
        /// =0费率；=1物业收费标准
        /// </summary>
        public int Attrib
        {
            set { _attrib = value; }
            get { return _attrib; }
        }
        /// <summary>
        /// 物业收费计算规则=0，=1时间范围；=2数量范围
        /// </summary>
        public int Rule
        {
            set { _rule = value; }
            get { return _rule; }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        
        /// <summary>
        /// 单位基数
        /// </summary>
        public decimal UnitBase
        {
            set { _unitBase = value; }
            get { return _unitBase; }
        }        
        #endregion Model

    }
    [Serializable]
    public class v1_rateCfg
    {
        public string T1st { get; set; }
        public string T2nd { get; set; }
        public string T3rd { get; set; }
        public string T4th { get; set; }
    }

    /// <summary>
    /// 运行历史报表返回对象
    /// </summary>
    //[Serializable]
    public partial class YdRepHisVModel
    {
        public int ID { get; set; }
        public int Co_id { get; set; }
        public int Module_id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleAddr { get; set; }
        public string Date { get; set; }
        public string CoStrcName { get; set; }
        public string CoName { get; set; }
        public string Multiply { get; set; }
        public string FirstVal { get; set; }
        public string LastVal { get; set; }
        public string UseVal { get; set; }


    }







}
