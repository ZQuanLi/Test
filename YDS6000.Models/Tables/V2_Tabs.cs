using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.Models
{
    /// <summary>
    /// v2_alarm_log:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v2_alarm_logVModel
    {
        public v2_alarm_logVModel()
        { }
        #region Model
        private long _log_id;
        private int _ledger;
        private DateTime _cdate = DateTime.Now;
        private int _co_id;
        private int _module_id;
        private string _moduleaddr;
        private int _fun_id = 0;
        private string _funtype = "";
        private string _funname = "";
        private string _content;
        private string _collectvalue = "";
        private DateTime _collecttime; 
        private int _errcode = 0;
        private string _errtxt = "";
        private bool ismqtt = false;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long Log_id
        {
            set { _log_id = value; }
            get { return _log_id; }
        }
        /// <summary>
        /// 账目
        /// </summary>
        public int Ledger
        {
            set { _ledger = value; }
            get { return _ledger; }
        }
        /// <summary>
        /// 告警日期
        /// </summary>
        public DateTime CDate
        {
            set { _cdate = value; }
            get { return _cdate; }
        }
        ///// <summary>
        ///// 公司ID号
        ///// </summary>
        //public int Co_id
        //{
        //    set { _co_id = value; }
        //    get { return _co_id; }
        //}
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
        /// 告警类型ID号
        /// </summary>
        public int Fun_id
        {
            set { _fun_id = value; }
            get { return _fun_id; }
        }
        /// <summary>
        /// 告警类型
        /// </summary>
        public string FunType
        {
            set { _funtype = value; }
            get { return _funtype; }
        }
        /// <summary>
        /// 告警名称
        /// </summary>
        public string FunName
        {
            set { _funname = value; }
            get { return _funname; }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectTime
        {
            set { _collecttime = value; }
            get { return _collecttime; }
        }
        /// <summary>
        /// 采集内容
        /// </summary>
        public string CollectValue
        {
            set { _collectvalue = value; }
            get { return _collectvalue; }
        }
        /// <summary>
        /// 错误码 =1 成功，其他失败
        /// </summary>
        public int ErrCode
        {
            set { _errcode = value; }
            get { return _errcode; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrTxt
        {
            set { _errtxt = value; }
            get { return _errtxt; }
        }
        public bool IsMqtt
        {
            set { ismqtt = value; }
            get { return ismqtt; }
        }
        #endregion Model

    }

    /// <summary>
    /// v2_alarm_log:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class v2_alarm_logHandVModel
    {
        public v2_alarm_logHandVModel()
        { }
        #region Model
        private long _log_id;
        private int _aid;
        private string _hdtype;
        private string _content = "";
        private int _errcode = 0;
        private string _errtxt = "";
        /// <summary>
        /// auto_increment
        /// </summary>
        public long Log_id
        {
            set { _log_id = value; }
            get { return _log_id; }
        }
        /// <summary>
        /// ID号
        /// </summary>
        public int Aid
        {
            set { _aid = value; }
            get { return _aid; }
        }

        /// <summary>
        /// 告警类型
        /// </summary>
        public string HdType
        {
            set { _hdtype = value; }
            get { return _hdtype; }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }

        /// <summary>
        /// 错误码 =1 成功，其他失败
        /// </summary>
        public int ErrCode
        {
            set { _errcode = value; }
            get { return _errcode; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrTxt
        {
            set { _errtxt = value; }
            get { return _errtxt; }
        }


        public DateTime HdTime { get; set; }
        #endregion Model

    }

}
