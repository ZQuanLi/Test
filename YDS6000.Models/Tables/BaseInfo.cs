using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YDS6000.Models
{
    [CoAttribute(0,"项目信息")]
    public class ProVModel
    {
        /// <summary>
        /// 区域ID号
        /// </summary>
        [Required]
        [Display(Name = "项目ID号")]
        public int Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        [Display(Name = "项目名称")]
        [StringLength(50, ErrorMessage = "{0}超出长度50")]
        public string ProName { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [Display(Name = "项目状态")]
        [Range(0, 1, ErrorMessage = "{0}请输入0或1的值")]
        public int Disabled { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        [Display(Name = "建筑面积")]
        public decimal Area { get; set; }
        /// <summary>
        /// 项目地址
        /// </summary>
        [Display(Name = "项目地址")]
        [StringLength(200, ErrorMessage = "{0}超出长度200")]
        public string ProAddr { get; set; }
        /// <summary>
        /// 安全负责人
        /// </summary>
        [Display(Name = "安全负责人")]
        [StringLength(50, ErrorMessage = "{0}超出长度50")]
        public string Person { get; set; }
        /// <summary>
        /// 安全负责人
        /// </summary>
        [Display(Name = "联系电话")]
        [StringLength(32, ErrorMessage = "{0}超出长度32")]
        public string TelNo { get; set; }

        [Display(Name = "项目简介")]
        [StringLength(1000, ErrorMessage = "{0}超出长度1000")]
        public string Remark { get; set; }


    }

    public class BuildVModel
    {
        /// <summary>
        /// 建筑ID号
        /// </summary>
        [Required]
        [Display(Name = "建筑ID号")]
        public int Id { get; set; }
        /// <summary>
        /// 建筑名称
        /// </summary>
        [Required]
        [Display(Name = "建筑名称")]
        [StringLength(50, ErrorMessage = "{0}超出长度50")]
        public string BuildName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [Display(Name = "地址")]
        [StringLength(200, ErrorMessage = "{0}超出长度200")]
        public string BuildAddr { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [StringLength(32, ErrorMessage = "{0}超出长度32")]
        public string TelNo { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        //[Required]
        [Display(Name = "电子邮件")]
        [StringLength(32, ErrorMessage = "{0}超出长度32")]
        public string Email { get; set; }


        [Display(Name = "父ID号")]
        public int Parent_id { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        //[Required]
        [Display(Name = "电子邮件")]
        [StringLength(500, ErrorMessage = "{0}超出长度500")]
        public string Remark { get; set; }
    }

    public class AreaVModel
    {
        /// <summary>
        /// 区域ID号
        /// </summary>
        [Required]
        [Display(Name = "区域ID号")]
        public int AreaId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Required(ErrorMessage ="请输入区域名称")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "区域名称")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域父ID号
        /// </summary>
        [Display(Name = "区域父ID号")]
        public int Parent_id { get; set; }
    }
    public class StationVModel
    {
        /// <summary>
        /// 机房ID号
        /// </summary>
        [Required]
        [Display(Name = "机房ID号")]
        public int StationId { get; set; }
        /// <summary>
        /// 机房编码
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "机房编码")]
        public string StationNo { get; set; }
        /// <summary>
        /// 机房名称
        /// </summary>
        [Required(ErrorMessage = "请输入机房名称")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "机房名称")]
        public string StationName { get; set; }
        /// <summary>
        /// 所属区域ID号
        /// </summary>
        [Required]
        [Display(Name = "所属区域ID号")]
        public int AreaId { get; set; }
        /// <summary>
        /// 机房类型
        /// </summary>
        [Display(Name = "机房类型")]
        public int StationType { get; set; }
        /// <summary>
        /// 机房地址
        /// </summary>
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "机房地址")]
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled { get; set; }

    }
    public class RoomVModel
    {
        /// <summary>
        /// 站点ID号
        /// </summary>
        [Required]
        [Display(Name = "站点ID号")]
        public int RoomId { get; set; }
        /// <summary>
        /// 站点编码
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "站点编码")]
        public string RoomNo { get; set; }
        /// <summary>
        /// 站点名称
        /// </summary>
        [Required(ErrorMessage = "请输入站点名称")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "站点名称")]
        public string RoomName { get; set; }
        /// <summary>
        /// 所属机房ID号
        /// </summary>
        [Required]
        [Display(Name = "所属机房ID号")]
        public int StationId { get; set; }
        /// <summary>
        /// 站点地址
        /// </summary>
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "站点地址")]
        public string Address { get; set; }

        /// <summary>
        /// 所属场景ID号
        /// </summary>
        [Display(Name = "所属场景ID号")]
        public int RoomSight { get; set; }

        /// <summary>
        /// 供电类型
        /// </summary>
        [Display(Name = "供电类型")]
        [EnumDataType(enumType: typeof(Switch), ErrorMessage = "供电类型错误")]
        public string Switch { get; set; }
        /// <summary>
        /// 供电类型
        /// </summary>
        [Display(Name = "站点类型")]
        [EnumDataType(enumType: typeof(CoType), ErrorMessage = "站点类型错误")]
        public string RoomType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否弃用
        /// </summary>        
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled { get; set; }
    }

    public class EspVModel
    {
        /// <summary>
        /// 集中器ID号
        /// </summary>
        [Required]
        [Display(Name = "集中器ID号")]
        public int Esp_id { get; set; }
        /// <summary>
        /// 集中器名称
        /// </summary>
        [Required(ErrorMessage ="请输入集中器名称")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "集中器名称")]
        public string EspName { get; set; }
        /// <summary>
        /// 采集器地址
        /// </summary>
        [Required(ErrorMessage = "集中器编码")]
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "集中器编码")]
        public string EspAddr { get; set; }
        /// <summary>
        /// 通信类型=0 com口，=1 Tcp，=2 UDP
        /// </summary>
        //[Required(ErrorMessage = "请输入通信方式")]
        //[Display(Name = "通信方式")]
        //public int TransferType { get; set; }
        /// <summary>
        /// 通信IP
        /// </summary>
        //[Display(Name = "通信IP")]
        //public string EspIp { get; set; }
        /// <summary>
        /// 通信端口
        /// </summary>
        //[Display(Name = "通信端口")]
        //public int EspPort { get; set; }
        /// <summary>
        /// 型号描述
        /// </summary>
        [StringLength(32, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "型号")]
        public string EspType { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "安装位置")]
        public string Inst_loc { get; set; }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled { get; set; }
        /// <summary>
        /// 供应商描述
        /// </summary>
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "供应商")]
        public string Supplier { get; set; }
    }

    /// <summary>
    /// 设备型号
    /// </summary>
    public class ModuleTypeVModel
    {
        /// <summary>
        /// 设备类型ID号
        /// </summary>
        [Required(ErrorMessage = "请输入设备类型ID号")]
        [Display(Name = "设备类型ID号")]
        public int ModuleTypeId { get; set; }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Required(ErrorMessage = "请输入设备类型名称")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "设备类型名称")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 设备类型型号
        /// </summary>
        //[Required(ErrorMessage = "请输入设备类型型号")]
        //[StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        //[Display(Name = "设备类型型号")]
        //public string ModuleType { get; set; }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled { get; set; }

        [Display(Name = "功能码类型")]
        [EnumDataType(enumType: typeof(MmDefine), ErrorMessage = "功能码类型错误")]
        public int IsDefine { get; set; }
    }

    public class ModuleFunVModel
    {
        /// <summary>
        /// 设备类型采集点ID号
        /// </summary>
        [Required(ErrorMessage = "请输入采集点ID号")]
        [Display(Name = "采集点ID号")]
        public int Fun_id { get; set; }
        /// <summary>
        /// 设备类型ID号
        /// </summary>
        [Required(ErrorMessage = "请输入设备类型ID号")]
        [Display(Name = "设备类型ID号")]
        public int ModuleTypeId { get; set; }
        /// <summary>
        /// 采集点类型
        /// </summary>
        [Required(ErrorMessage = "请输入采集点类型")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "采集点类型")]
        public string FunType { get; set; }

        /// <summary>
        /// 采集点类型名称
        /// </summary>
        [Required(ErrorMessage = "请输入采集点类型名称")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "采集点类型名称")]
        public string FunName { get; set; }

    }
    /// <summary>
    /// 设备型号
    /// </summary>
    public class MeterTypeVModel
    {
        /// <summary>
        /// 设备类型ID号
        /// </summary>
        [Required(ErrorMessage = "请输入设备类型ID号")]
        [Display(Name = "设备类型ID号")]
        public int ModuleTypeId { get; set; }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Required(ErrorMessage = "请输入设备类型名称")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "设备类型名称")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 设备类型型号
        /// </summary>
        [Required(ErrorMessage = "请输入设备类型型号")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "设备类型型号")]
        public string ModuleType { get; set; }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled { get; set; }
        ///// <summary>
        ///// 设备类型型号
        ///// </summary>
        //[StringLength(6, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        //[Display(Name = "设备类型操作密码")]
        //public string ModulePwd { get; set; }
        ///// <summary>
        ///// 设备类型操作用户
        ///// </summary>
        //[StringLength(6, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        //[Display(Name = "设备类型操作用户")]
        //public string ModuleUid { get; set; }
        ///// <summary>
        ///// 设备类型操作密码级别
        ///// </summary>
        //[StringLength(2, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        //[Display(Name = "设备类型操作密码级别")]
        //public string Level { get; set; }
        /// <summary>
        /// 接线方式
        /// </summary>
        [StringLength(1000, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "接线方式")]
        public string Spec { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "供应商")]
        public string Fty_prod { get; set; }
        ///// <summary>
        ///// 计费方式
        ///// </summary>
        //[Range(0, 1, ErrorMessage = "请输入0或1的值")]
        //[Display(Name = "计费方式")]
        //public int IsCharg { get; set; }
    }

    public class MdVModel
    {
        public MdVModel()
        { }
        #region Model
        private int _meter_id = 0;
        private string _metername = "";
        private string _meterNo = "";
        private int _meterTypeId = 0;//T '类型ID号',
        private int _disabled = 0;
        private int _attrib = 0;
        /// <summary>
        /// 集中器ID号
        /// </summary>
        [Required]
        [Display(Name = "设备ID号")]
        public int Meter_id
        {
            set { _meter_id = value; }
            get { return _meter_id; }
        }
        /// <summary>
        /// 电能表名称
        /// </summary>
        [Required(ErrorMessage = "请输入电能表名称")]
        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "电能表名称")]
        public string MeterName
        {
            set { _metername = value; }
            get { return _metername; }
        }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(20, ErrorMessage = "最大长度20位")]
        [Display(Name = "编码")]
        public string MeterNo
        {
            set { _meterNo = value; }
            get { return _meterNo; }
        }

        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 所属型号ID号
        /// </summary>
        [Required(ErrorMessage = "请输入所属型号")]
        [Display(Name = "所属型号ID号")]
        public int MeterTypeId
        {
            set { _meterTypeId = value; }
            get { return _meterTypeId; }
        }
        /// <summary>
        /// 设备属性设置 =0 馈线 ; =1 进线 = 2其他
        /// </summary>
        [Display(Name = "设备属性")]
        [EnumDataType(enumType: typeof(MdAttrib), ErrorMessage = "设备属性错误")]
        public int Attrib
        {
            set { _attrib = value; }
            get { return _attrib; }
        }
        #endregion Model
    }

    public class MdVModel_PDU
    {
        public MdVModel_PDU()
        { }
        #region Model
        private int _meter_id = 0;
        private string _metername = "";
        private string _meterNo = "";
        private int _meterTypeId = 0;//T '类型ID号',
        private int _disabled = 0;
        private int _parent_id = 0;
        private int _co_id;
        /// <summary>
        /// 设备ID号
        /// </summary>
        [Required]
        [Display(Name = "设备ID号")]
        public int Meter_id
        {
            set { _meter_id = value; }
            get { return _meter_id; }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "请输入设备名称")]
        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "设备名称")]
        public string MeterName
        {
            set { _metername = value; }
            get { return _metername; }
        }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(20, ErrorMessage = "最大长度20位")]
        [Display(Name = "编码")]
        public string MeterNo
        {
            set { _meterNo = value; }
            get { return _meterNo; }
        }

        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 所属型号ID号
        /// </summary>
        [Required(ErrorMessage = "请输入所属型号")]
        [Display(Name = "所属型号ID号")]
        public int MeterTypeId
        {
            set { _meterTypeId = value; }
            get { return _meterTypeId; }
        }
        /// <summary>
        /// 关联PDU
        /// </summary>
        [Required(ErrorMessage = "请输入关联PDU")]
        [Display(Name = "关联PDU")]
        public int Co_id
        {
            set { _co_id = value; }
            get { return _co_id; }
        }
        /// <summary>
        /// 父ID号
        /// </summary>
        [Required(ErrorMessage = "请输入父ID号")]
        [Display(Name = "所属父ID号")]
        public int Parent_id
        {
            set { _parent_id = value; }
            get { return _parent_id; }
        }
        #endregion Model
    }

    //设备类
    public class MeterVModel
    {
        public MeterVModel()
        { }
        #region Model
        private int _meter_id = 0;
        private int _esp_id = 0;
        private string _metername = "";
        private string _meteraddr = "";
        private string _inst_loc = "";
        //private string _switch = "";
        private int _disabled = 0;
        private string _remark = "";
        private decimal _multiply = 1M;
        private int _mm_id = 0;//T '类型ID号',
        private int _roomid = 0; // 所属站点
        /// <summary>
        /// 集中器ID号
        /// </summary>
        [Required]
        [Display(Name = "设备ID号")]
        public int Meter_id
        {
            set { _meter_id = value; }
            get { return _meter_id; }
        }
        /// <summary>
        /// 所属采集器
        /// </summary>
        [Required(ErrorMessage ="请输入集中器")]
        [Display(Name = "所属集中器")]
        public int Esp_id
        {
            set { _esp_id = value; }
            get { return _esp_id; }
        }
        /// <summary>
        /// 电能表编码
        /// </summary>
        [Required(ErrorMessage = "请输入电能表编码")]
        [StringLength(32, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "电能表编码")]
        public string MeterAddr
        {
            set { _meteraddr = value; }
            get { return _meteraddr; }
        }
        /// <summary>
        /// 电能表名称
        /// </summary>
        [Required(ErrorMessage = "请输入电能表名称")]
        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "电能表名称")]
        public string MeterName
        {
            set { _metername = value; }
            get { return _metername; }
        }

        /// <summary>
        /// 安装位置（计量点）
        /// </summary>
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "安装位置")]
        public string Inst_loc
        {
            set { _inst_loc = value; }
            get { return _inst_loc; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "备注")]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 倍率 变比
        /// </summary>
        [Required(ErrorMessage = "请输入倍率")]
        [Display(Name = "倍率")]
        [Range(1, 9999999999, ErrorMessage = "请输入大于1的值")]
        public decimal Multiply
        {
            set { _multiply = value; }
            get { return _multiply; }
        }
        /// <summary>
        /// 所属型号ID号
        /// </summary>
        [Required(ErrorMessage = "请输入所属型号")]
        [Display(Name = "所属型号ID号")]
        public int MeterTypeId
        {
            set { _mm_id = value; }
            get { return _mm_id; }
        }
        /// <summary>
        /// 所属型号ID号
        /// </summary>
        [Required(ErrorMessage = "请输入所属站点")]
        [Display(Name = "所属站点")]
        public int RoomId
        {
            set { _roomid = value; }
            get { return _roomid; }
        }
        #endregion Model
    }
    // 回路
    public class ModuleVModel
    {
        public ModuleVModel()
        { }
        #region Model
        private int _module_id = 0;
        private string _moduleaddr = "";
        private string _modulename = "";
        private int _disabled = 0;
        private string _remark = "";
        private int _meter_id = 0;

        /// <summary>
        /// 回路ID号
        /// </summary>
        [Required(ErrorMessage = "请输入回路")]
        [Display(Name = "回路ID号")]
        public int Module_id
        {
            set { _module_id = value; }
            get { return _module_id; }
        }
        /// <summary>
        /// 电能表编码
        /// </summary>
        [Required(ErrorMessage = "请输入回路编码")]
        [StringLength(32, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "回路编码")]
        public string ModuleAddr
        {
            set { _moduleaddr = value; }
            get { return _moduleaddr; }
        }
        /// <summary>
        /// 电能表名称
        /// </summary>
        [Required(ErrorMessage = "请输入回路名称")]
        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "回路名称")]
        public string ModuleName
        {
            set { _modulename = value; }
            get { return _modulename; }
        }

        /// <summary>
        /// 供电方式
        /// </summary>
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]

        [Display(Name = "供电方式")]
        [EnumDataType(enumType: typeof(Switch), ErrorMessage = "供电方式错误")]
        public string PSWay { get; set; }
        /// <summary>
        /// 缴费方式
        /// </summary>
        [Display(Name = "缴费方式")]
        [EnumDataType(enumType: typeof(ChargAttrib), ErrorMessage = "缴费方式错误")]
        public int ChrgType { get; set; }

        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled
        {
            set { _disabled = value; }
            get { return _disabled; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "备注")]
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 所属设备ID号
        /// </summary>
        [Required(ErrorMessage = "请输入设备")]
        [Display(Name = "所属设备")]
        public int Meter_id
        {
            set { _meter_id = value; }
            get { return _meter_id; }
        }
        #endregion Model
    }

    /// <summary>
    /// 采集码映射变量
    /// </summary>
    public class MapVModel
    {
        /// <summary>
        /// 集中器ID号
        /// </summary>
        [Required(ErrorMessage = "请输入设备ID号")]
        [Range(1, 999999999999, ErrorMessage = "请输入设备ID号")]
        [Display(Name = "设备ID号")]
        public int Module_id { get; set; }
        /// <summary>
        /// 采集码ID号
        /// </summary>
        [Required(ErrorMessage = "请输入采集码ID号")]
        [Range(1, 999999999999, ErrorMessage = "请输入采集码ID号")]
        [Display(Name = "采集码ID号")]
        public int Fun_id { get; set; }
        /// <summary>
        /// 采集码映射变量
        /// </summary>
        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "采集码映射变量")]
        public string TagName { get; set; }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "请输入0或1的值")]
        public int Disabled { get; set; }
    }

    public class CrmVModel
    {
        /// <summary>
        /// 集中器ID号
        /// </summary>
        [Required(ErrorMessage = "请输入业主ID号")]
        [Display(Name = "业主ID号")]
        public int Crm_id { get; set; }
        [Display(Name = "业主证件号")]
        public string CrmNo { get; set; }

        [StringLength(64, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "业主名称")]
        public string CrmName { get; set; }

        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "邮件地址")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "移动电话号码")]
        public string MPhone { get; set; }

        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }

}
