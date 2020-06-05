using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YDS6000.WebApi
{
    /// <summary>  
    /// 作用：用来说明方法的用途
    /// AttributeUsage:说明特性的目标元素是什么  
    /// AttributeTargets.Method：代表目标元素为Method
    /// </summary>  
    [AttributeUsage(AttributeTargets.Method)]
    public class SecurityCtrl : Attribute
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        private string _prog_id = "";
        /// <summary>
        /// 页面描述
        /// </summary>
        private string _describe = "";
        private bool _authorize = true;

        private bool _chkSession = true;
        /// <summary>  
        /// 页面描述  
        /// </summary>  
        public string describe { get { return _describe; } }
        /// <summary>
        /// 权限ID
        /// </summary>
        public string prog_id { get { return _prog_id; } }

        /// <summary>
        /// 是否权限检查(默认检查)
        /// </summary>
        public bool authorize { get { return _authorize; } }
        /// <summary>
        /// 是否检查session
        /// </summary>
        public bool chkSession { get { return _chkSession; } }

        #region 构造方法，可选的
        /// <summary>
        /// 安全控制
        /// </summary>
        public SecurityCtrl()
        {

        }
        /// <summary>
        /// 检测设置
        /// </summary>
        /// <param name="chkSession">不检测session</param>
        /// <param name="authorize">不检测权限</param>
        public SecurityCtrl(bool chkSession=false,bool authorize = false)
        {
            this._chkSession = chkSession;
            this._authorize = authorize;
        }

        /// <summary>
        /// 权限，页面描述
        /// </summary>
        /// <param name="prog_id">权限ID，为空不需检查权限</param>
        /// <param name="describe">页面描述</param>
        public SecurityCtrl(string describe, string prog_id)
        {
            this._describe = describe;
            this._prog_id = prog_id;
        }
        public SecurityCtrl(string describe)
        {
            this._describe = describe;
            this._authorize = false;
        }
        public SecurityCtrl(string describe, string prog_id, bool authorize)
        {
            this._describe = describe;
            this._prog_id = prog_id;
            this._authorize = authorize;
        }
        #endregion
    }
}