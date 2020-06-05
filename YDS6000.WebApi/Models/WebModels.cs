using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    /// <summary>
    /// 返回结果集
    /// </summary>
    public class APIResult
    {
        private int _code = (int)ResultCodeDefine.Success;

        /// <summary>
        /// 代码，0成功其他错误
        /// </summary>
        public int Code { get { return _code; } set { _code = value; } }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 返回结果集
    /// </summary>
    public class APIRst
    {
        private bool _rst = true;
        private APIErr _err = new APIErr() { msg = "", code = 0 };
        /// <summary>
        /// true成功其他错误
        /// </summary>
        public bool rst { get { return _rst; } set { _rst = value; } }
        /// <summary>
        /// 数据内容
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 错误对象
        /// </summary>
        public APIErr err
        {
            get { return _err; }
            set { _err = value; }
        }
    }

    /// <summary>
    /// 返回结果集
    /// </summary>
    //public class APIRst
    //{
    //    private bool _rst = true;
    //    private int _code = (int)ResultCodeDefine.Success;
    //    private string _msg = "";
    //    /// <summary>
    //    /// true成功其他错误
    //    /// </summary>
    //    public bool rst { get { return _rst; } set { _rst = value; } }

    //    /// <summary>
    //    /// 代码，0成功其他错误
    //    /// </summary>
    //    public int code { get { return _code; } set { if (value < 0) rst = false; else if (value == 0) rst = true; _code = value; } }
    //    /// <summary>
    //    /// 错误描述
    //    /// </summary>
    //    public string msg { get { return _msg; } set { _msg = value; } }
    //    /// <summary>
    //    /// 数据内容
    //    /// </summary>
    //    public object data { get; set; }
    //}

    public class APIErr
    {
        /// <summary>
        /// 错误描述
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int code { get; set; }
    }

    /// <summary>
    /// session 对象
    /// </summary>
    [Serializable]
    public class CacheUser
    {
        private int _ledger = 0;
        private int _uid = 0;
        private string _usign = "游客";
        private int _role_id = 0;
        /// <summary>
        /// 账目ID
        /// </summary>
        public int Ledger { get { return _ledger; } set { _ledger = value; } }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Uid { get { return _uid; } set { _uid = value; } }
        /// <summary>
        /// 登录名
        /// </summary>
        public string USign { get { return _usign; } set { _usign = value; } }
        /// <summary>
        /// 权限ID号
        /// </summary>
        public int Role_id { get { return _role_id; } set { _role_id = value; } }
        /// <summary>
        /// API票据信息
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        /// 缓存Key
        /// </summary>
        public string CacheKey { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public dynamic Attach { get; set; }
    }

    /// <summary>
    /// 返回结果定义
    /// </summary>
    public enum ResultCodeDefine
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Display(Name = "成功")]
        Success = 0,
        /// <summary>
        /// 错误
        /// </summary>
        [Display(Name = "错误")]
        Error = -1,
        /// <summary>
        /// 登录失效
        /// </summary>
        [Display(Name = "登录失效，请重新登录")]
        Error_LoginInvalid = -200,
        /// <summary>
        /// 参数传递错误
        /// </summary>
        [Display(Name = "参数传递错误")]
        Auth_ParamsInvalid = -201,
        /// <summary>
        /// 签名无效
        /// </summary>
        [Display(Name = "Ticket无效")]
        Auth_TicketInvalid = -202,
        /// <summary>
        /// 时间戳超时
        /// </summary>
        [Display(Name = "时间戳超时")]
        Auth_TimestampTimeOut = -203,
        /// <summary>
        /// 时间戳超时
        /// </summary>
        [Display(Name = "访问次数过于频繁")]
        Auth_HitTimeOut = -204,
        /// <summary>
        /// 用户没有权限
        /// </summary>
        [Display(Name = "用户没有权限")]
        Auth_UserNoPermission = -220,
    }

    public class VEasyUiTree
    {
        private string _id = "";
        private string _text = "";
        private string _state = "";
        private bool _checked = false;
        private object _attributes = "";
        private string _iconCls = "";

        public string id { get { return _id; } set { _id = value; } }
        public string text { get { return _text; } set { _text = value; } }
        /// <summary>
        /// 'open' 或 'closed'，默认是 'open'。
        /// 如果为'closed'的时候，将不自动展开该节点。
        /// </summary>
        public string state { get { return _state; } set { _state = value; } }
        public string iconCls { get { return _iconCls; } set { _iconCls = value; } }

        public bool @checked { get { return _checked; } set { _checked = value; } }// replace checked
        public object attributes { get { return _attributes; } set { _attributes = value; } }
        public List<VEasyUiTree> children { get; set; }
    }

}