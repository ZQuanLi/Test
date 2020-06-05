using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YDS6000.Models
{
    /// <summary>
    /// sys_user 用户
    /// </summary>
    public class sys_user
    {
        #region 1.0用户
        private int uid;//'用户ID号',
        private string uSign;// '登录名',
        private string uPasswd; //'登录密码',
        private string uName;// '名称',
        private int role_id;// 角色',
        private int disabled;// 是否弃用',

        /// <summary>
        /// 用户ID号
        /// </summary>
        [Required]
        [Display(Name = "用户ID号")]
        public int Uid 
        {
            get { return uid; }
            set { uid = value; }
        }
        /// <summary>
        /// 登陆用户
        /// </summary>
        [Required(ErrorMessage = "请输入登陆用户")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "登录用户")]
        public string USign
        {
            get { return uSign; }
            set { uSign = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入登录密码")]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 8)]
        [Display(Name = "登录密码")]
        public string UPasswd
        {
            get { return uPasswd; }
            set { uPasswd = value; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "姓名")]
        public string UName
        {
            get { return uName; }
            set { uName = value; }
        }
        /// <summary>
        /// 角色
        /// </summary>
        [Required(ErrorMessage = "请输入角色")]
        [Display(Name = "角色")]
        public int Role_id
        {
            get { return role_id; }
            set { role_id = value; }
        }
        /// <summary>
        /// 是否弃用
        /// </summary>
        [Display(Name = "是否弃用")]
        [Range(0, 1, ErrorMessage = "{0}请输入0或1的值")]
        public int Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(32, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "联系电话")]
        public string TelNo { get; set; }
        #endregion
    }

    /// <summary>
    /// sys_role 用户角色
    /// </summary>
    public class sys_role
    {

        private int role_id;
        private string descr;
        private string remark;
        /// <summary>
        /// 角色ID
        /// </summary>
        [Required]
        [Display(Name = "角色ID")]
        public int Role_id
        {
            get { return role_id; }
            set { role_id = value; }
        }
        /// <summary>
        /// 角色描述
        /// </summary>
        [Required(ErrorMessage = "请输入角色描述")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [Display(Name = "角色描述")]
        public string Descr
        {
            get { return descr; }
            set { descr = value; }
        }
        /// <summary>
        /// 角色备注
        /// </summary>
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 0)]
        [Display(Name = "备注")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }

    
}
