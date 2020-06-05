using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace YDS6000.Models
{
    [Obsolete("放弃使用",true)]
    public class CheckAttribute : Attribute
    {
        // 三个bool变量用于确定是有要验证这些信息        
        private string fieldName = "";//字段中文描述
        private bool checkEmpty = false; //是否为空
        private bool checkMaxLength = false; //最大长度
        private bool checkRegex = false; //用正则表达式验证参数(是邮箱，是否刷数字)
        private int maxLength = 0;
        private string regexStr = string.Empty;

        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }
        public bool CheckEmpty
        {
            get { return this.checkEmpty; }
            set { this.checkEmpty = value; }
        }
        public bool CheckMaxLength
        {
            get { return this.checkMaxLength; }
            set { this.checkMaxLength = value; }
        }
        public bool CheckRegex
        {
            get { return this.checkRegex; }
            set { this.checkRegex = value; }
        }
        public int MaxLength
        {
            get { return this.maxLength; }
            set { this.maxLength = value; }
        }
        public string RegexStr
        {
            get { return this.regexStr; }
            set { this.regexStr = value; }
        }

        public static string CheckField(object obj)
        {
            string errorMsg = "";
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(); // 获取一个类的所有属性
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                // 因为对于此示例，每个Properties(属性)只有一个Attribute(标签),所以用了first()来获取，
                // 不过有一点，就是必须在属性里面添加[CheckAttribute]标签，但是可以不设置表情里面的字段.因为没有的.GetCustomAttributes()返回为null.指向first会报错.
                var attrs = propertyInfo.GetCustomAttributes(typeof(CheckAttribute), false);
                if (attrs == null || attrs.Count() == 0) continue;
                CheckAttribute attribute = attrs.First() as CheckAttribute;
                if (attribute == null) continue;
                if (attribute.CheckEmpty)
                {
                    if (propertyInfo.PropertyType.FullName.Equals(typeof(System.String).FullName))
                    {
                        string val = propertyInfo.GetValue(obj) as string;
                        if (string.IsNullOrEmpty(val))
                        {
                            errorMsg = string.Format("{0} 不能为空", string.IsNullOrEmpty(attribute.FieldName) ? propertyInfo.Name : attribute.FieldName);
                            break;
                        }
                    }
                    else
                    {
                        if (CommFunc.ConvertDBNullToDecimal(propertyInfo.GetValue(obj)) == 0)
                        {
                            errorMsg = string.Format("{0} 不能为空", string.IsNullOrEmpty(attribute.FieldName) ? propertyInfo.Name : attribute.FieldName);
                            break;
                        }
                    }
                }
                if (attribute.CheckMaxLength)
                {
                    string val = propertyInfo.GetValue(obj) as string;
                    if (val != null && val.Length > attribute.MaxLength)
                    {
                        errorMsg = string.Format("{0} 最大长度为{1}", string.IsNullOrEmpty(attribute.FieldName) ? propertyInfo.Name : attribute.FieldName, attribute.MaxLength);
                        break;
                    }
                }
                if (attribute.CheckRegex)
                {
                    string val = propertyInfo.GetValue(obj) as string;
                    Regex regex = new Regex(attribute.RegexStr);
                    if (val != null && !regex.IsMatch(val))
                    {
                        errorMsg = string.Format("{0} 格式不对", string.IsNullOrEmpty(attribute.FieldName) ? propertyInfo.Name : attribute.FieldName);
                        break;
                    }
                }
            }
            return errorMsg;
        }
    }

    /// <summary>
    /// 属性定义
    /// </summary>
    public class CoAttribute : Attribute
    {
        private int _attrib = 0; //属性值
        private string _name = "";// 属性值说明

        public CoAttribute(int attrib,string name = "")
        {
            _attrib = attrib;
            _name = name;
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        public int Attrib { get { return _attrib; } }
        /// <summary>
        /// 获取属性名称
        /// </summary>
        public string Name { get { return _name; } }
    }

}
