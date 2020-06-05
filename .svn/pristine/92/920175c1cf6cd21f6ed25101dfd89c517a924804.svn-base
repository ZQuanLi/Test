using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net.Mail;
using MemcachedProviders.Cache;

namespace YDS6000.Models
{
    public static class CommFunc
    {
        /// <summary>
        /// 数字转字母
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NunberToChar(int number)
        {
            //if (1 <= number && 36 >= number)
            //{
            //    int num = number + 64;
            //    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            //    byte[] btNumber = new byte[] { (byte)num };
            //    return Convert.ToString(asciiEncoding.GetString(btNumber));
            //}
            //return "";
            int num = number;
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            byte[] btNumber = new byte[] { (byte)num };
            return Convert.ToString(asciiEncoding.GetString(btNumber));
        }

        /// <summary>
        /// 字母转数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CharToNuner(string str)
        {
            byte[] array = new byte[1];   //定义一组数组array
            array = System.Text.Encoding.ASCII.GetBytes(str); //string转换的字母
            int asciicode = (short)(array[0]); /*  */
            return asciicode;
        }

        /// <summary>
        /// 轉換DBNull to string
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static string ConvertDBNullToString(Object obj1)
        {
            if (Convert.IsDBNull(obj1) || obj1 == null)
            {
                return "";
            }
            else
            {
                return obj1.ToString().Trim();
            }
        }

        /// <summary>
        /// 轉換DBNull to Int
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static Int32 ConvertDBNullToInt32(Object obj1)
        {
            Int32 rtn1 = 0;
            if (Convert.IsDBNull(obj1) || obj1 == null)
            {
                rtn1 = 0;
            }
            else
            {
                if (obj1.ToString().Trim().Length > 0)
                {
                    Int32.TryParse(obj1.ToString(), out rtn1);
                }
            }
            return rtn1;
        }

        /// <summary>
        /// 轉換DBNull to Int
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static long ConvertDBNullToLong(Object obj1)
        {
            long rtn1 = 0;
            if (Convert.IsDBNull(obj1) || obj1 == null)
            {
                rtn1 = 0;
            }
            else
            {
                if (obj1.ToString().Trim().Length > 0)
                {
                    long.TryParse(obj1.ToString(), out rtn1);
                }
            }
            return rtn1;
        }
        /// <summary>
        /// 轉換DBNull to Int
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static ulong ConvertDBNullToULong(Object obj1)
        {
            ulong rtn1 = 0;
            if (Convert.IsDBNull(obj1) || obj1 == null)
            {
                rtn1 = 0;
            }
            else
            {
                if (obj1.ToString().Trim().Length > 0)
                {
                    ulong.TryParse(obj1.ToString(), out rtn1);
                }
            }
            return rtn1;
        }
        /// <summary>
        /// 轉換DBNull to Decimal
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static Decimal ConvertDBNullToDecimal(Object obj1)
        {
            Decimal rtn1 = 0;
            if (Convert.IsDBNull(obj1) || obj1 == null)
            {
                rtn1 = 0;
            }
            else
            {
                if (obj1.ToString().Trim().Length > 0)
                {
                    Decimal.TryParse(obj1.ToString(), out rtn1);
                }
            }
            return rtn1;
        }

        /// <summary>
        /// 轉換DBNull to Decimal
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static float ConvertDBNullToFloat(Object obj1)
        {
            float rtn1 = 0;
            if (Convert.IsDBNull(obj1) || obj1 == null)
            {
                rtn1 = 0;
            }
            else
            {
                if (obj1.ToString().Trim().Length > 0)
                {
                    float.TryParse(obj1.ToString(), out rtn1);
                }
            }
            return rtn1;
        }

        /// <summary>
        /// 轉換DBNull to Datetime
        /// </summary>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public static DateTime ConvertDBNullToDateTime(Object obj1)
        {
            DateTime rtn1 = new DateTime();
            if (Convert.IsDBNull(obj1) || obj1 == null || ConvertDBNullToString(obj1) == "")
            {
                rtn1 = new DateTime(1900, 1, 1);
            }
            else
            {
                if (obj1.ToString().Trim().Length > 0)
                {
                    DateTime.TryParse(obj1.ToString(), out rtn1);
                }
            }
            return rtn1;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static object GetPropertyValue(string name, object app)
        {
            if (app != null)
            {
                System.Reflection.PropertyInfo p = app.GetType().GetProperty(name);
                if (p != null)
                    return p.GetValue(app, null);
            }
            return "";
        }
        /// <summary>
        /// 转换为人民币大写
        /// </summary>
        /// <param name="decMoney"></param>
        /// <returns></returns>
        public static string convertMoneytoRMB(decimal decMoney)
        {
            string qz = "";
            if (decMoney < 0)
            {
                qz = "负";
                decMoney = Math.Abs(decMoney);
            }
            decMoney = CommFunc.ConvertDBNullToDecimal(decMoney.ToString("f2"));
            string strMoney, strOneNum, strTemp, strConverted;
            int i, iLen;

            //设初值
            strConverted = "";
            strMoney = decMoney.ToString();
            iLen = strMoney.Length;

            //先取小数位
            if (strMoney.IndexOf(".") > 0)
            {
                strTemp = strMoney.Substring(strMoney.IndexOf(".") + 1, strMoney.Length - strMoney.IndexOf(".") - 1);
                if (strTemp.Length > 2)
                {
                    //Console.WriteLine("错误：无法计算超过2位的小数");
                    return strConverted;
                }
                else if (strTemp == "0" || strTemp == "00" || strTemp == "")
                    strTemp = "";
                else
                {
                    if (strTemp.Length == 1 && strTemp != "0")
                    {
                        strConverted = converNumtoCapital(strTemp) + "角" + strConverted;
                    }
                    else
                    {
                        strOneNum = strTemp.Substring(0, 1);
                        strConverted = converNumtoCapital(strOneNum) + (strOneNum != "0" ? "角" : "") + strConverted;
                        strOneNum = strTemp.Substring(1, 1);
                        strConverted = strConverted + (strOneNum != "0" ? converNumtoCapital(strOneNum) + "分" : "");
                    }
                }
            }

            //取整数部分
            if (strMoney.IndexOf(".") < 0)
                strTemp = strMoney;
            else
                strTemp = strMoney.Substring(0, strMoney.IndexOf("."));

            iLen = strTemp.Length;
            if (iLen > 0 && decimal.Parse(strTemp) != 0)
            {
                strConverted = "元" + strConverted;

                for (i = 0; i < iLen; ++i)
                {
                    strOneNum = strTemp.Substring(iLen - 1 - i, 1);
                    //if (strOneNum == "0")
                    //{
                    //    //Console.WriteLine(strConverted.Substring(0, 1));
                    //    if ((strConverted.Substring(0, 1) == "零" || strConverted.Substring(0, 1) == "元" || strConverted.Substring(0, 1) == "万" || strConverted.Substring(0, 1) == "亿") && !((i + 1) % 12 == 0 || (i + 1) == 5 || (i + 1) % 9 == 0))
                    //        continue;
                    //    else
                    //        strConverted = converNumtoCapital(strOneNum) + strConverted;
                    //}

                    //Console.WriteLine((i + 1) % 4);

                    if ((i + 1) == 1)
                    {
                        strConverted = (strOneNum == "0" ? "" : converNumtoCapital(strOneNum)) + strConverted;
                    }
                    else if (((i + 1) % 4 == 2 || (i + 1) == 2) && i % 4 != 0 && i % 8 != 0)
                    {
                        if (strOneNum == "0")
                        {
                            if (strConverted.Substring(0, 1) == "零" || strConverted.Substring(0, 1) == "元" || strConverted.Substring(0, 1) == "万" || strConverted.Substring(0, 1) == "亿")
                                continue;
                            else
                                strConverted = "零" + strConverted;
                        }
                        else
                            strConverted = converNumtoCapital(strOneNum) + "拾" + strConverted;
                    }
                    else if (((i + 1) % 4 == 3 || (i + 1) == 3) && i % 4 != 0)
                    {
                        if (strOneNum == "0")
                        {
                            if (strConverted.Substring(0, 1) == "零" || strConverted.Substring(0, 1) == "元" || strConverted.Substring(0, 1) == "万" || strConverted.Substring(0, 1) == "亿")
                                continue;
                            else
                                strConverted = "零" + strConverted;
                        }
                        else
                            strConverted = converNumtoCapital(strOneNum) + "佰" + strConverted;
                    }
                    else if ((i + 1) % 4 == 0 && i % 4 != 0)
                    {
                        if (strOneNum == "0")
                        {
                            if (strConverted.Substring(0, 1) == "零" || strConverted.Substring(0, 1) == "元" || strConverted.Substring(0, 1) == "万" || strConverted.Substring(0, 1) == "亿")
                                continue;
                            else
                                strConverted = "零" + strConverted;
                        }
                        else
                            strConverted = converNumtoCapital(strOneNum) + "千" + strConverted;
                    }
                    else if (i % 4 == 0 && i % 8 != 0)
                    {
                        //Console.WriteLine("万位{0}", i);
                        strConverted = (strOneNum == "0" ? "" : converNumtoCapital(strOneNum)) + "万" + strConverted;
                    }
                    else if (i % 8 == 0)
                    {
                        //Console.WriteLine("亿位{0}", i);
                        if (strConverted.Substring(0, 1) == "万") strConverted = strConverted.Substring(1, strConverted.Length - 1);
                        strConverted = (strOneNum == "0" ? "" : converNumtoCapital(strOneNum)) + "亿" + strConverted;
                    }
                    else
                    {
                        //Console.WriteLine(i);
                        strConverted = converNumtoCapital(strOneNum) + strConverted;
                    }
                }
            }

            return qz + strConverted;
        }
        private static string converNumtoCapital(string strNum)
        {
            string strCapital = "";
            switch (strNum)
            {
                case "0":
                    strCapital = "零";
                    break;
                case "1":
                    strCapital = "壹";
                    break;
                case "2":
                    strCapital = "贰";
                    break;
                case "3":
                    strCapital = "叁";
                    break;
                case "4":
                    strCapital = "肆";
                    break;
                case "5":
                    strCapital = "伍";
                    break;
                case "6":
                    strCapital = "陆";
                    break;
                case "7":
                    strCapital = "柒";
                    break;
                case "8":
                    strCapital = "捌";
                    break;
                case "9":
                    strCapital = "玖";
                    break;
                default:
                    strCapital = "";
                    break;
            }
            return strCapital;
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDisplay(Type enumType, int value)
        {
            try
            {
                object enumObj = Enum.ToObject(enumType, value);
                if (enumObj == null) return "";
                FieldInfo field = enumType.GetField(enumObj.ToString());
                if (field == null) return enumObj.ToString();
                var obj = field.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    System.ComponentModel.DataAnnotations.DisplayAttribute md = obj[0] as System.ComponentModel.DataAnnotations.DisplayAttribute;
                    return md.Name;
                }
                return enumObj.ToString();
            }
            catch { }
            return "";
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDisplay(Type enumType, string value)
        {
            try
            {
                object enumObj = Enum.Parse(enumType, value, true);
                if (enumObj == null) return "";
                FieldInfo field = enumType.GetField(enumObj.ToString());
                if (field == null) return enumObj.ToString();
                var obj = field.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    System.ComponentModel.DataAnnotations.DisplayAttribute md = obj[0] as System.ComponentModel.DataAnnotations.DisplayAttribute;
                    return md.Name;
                }
                return enumObj.ToString();
            }
            catch { }
            return "";
        }
    }

    public static class ModelHandler<T> where T : class, new()
    {
        #region 开放方法
        /// <summary>
        /// 跟据数据行转化为 model类
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }
            T model = new T();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                Type type = propertyInfo.PropertyType;
                if (type.IsSealed)
                {
                    if (dr.Table.Columns.Contains(propertyInfo.Name.Trim()))
                    {
                        if (propertyInfo.CanWrite && dr[propertyInfo.Name.Trim()] != DBNull.Value)
                        {
                            propertyInfo.SetValue(model, dr[propertyInfo.Name.Trim()], null);
                        }
                    }
                }
                else
                {
                    //object obj = Activator.CreateInstance(type);
                    if (type.IsGenericType == false)
                        propertyInfo.SetValue(model, FillObjectSetValue(propertyInfo, dr), null);
                }
            }
            return model;
        }

        public static void FillDataRow<T>(T model, DataRow dr)
        {
            foreach (System.Reflection.PropertyInfo propertyInfo in model.GetType().GetProperties())
            {
                Type type = propertyInfo.PropertyType;
                if (type.IsSealed)
                {
                    if (dr.Table.Columns.Contains(propertyInfo.Name.Trim()))
                    {
                        object obj = propertyInfo.GetValue(model, null);
                        if (obj == null) dr[propertyInfo.Name.Trim()] = DBNull.Value;
                        else dr[propertyInfo.Name.Trim()] = obj;
                    }
                }
                else
                {
                    object obj = propertyInfo.GetValue(model, null);
                    if (obj != null)
                        ModelHandler<Object>.FillDataRow(obj, dr);
                }
            }
            dr.AcceptChanges();
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public static DataTable CreateDataTable<T>(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                CreateDataTable(propertyInfo, ref dataTable);
            }
            return dataTable;
        }

        /// <summary>
        /// 把model类转json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string modelToJsonText(T model)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(model.GetType());
            string jsonText;
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, model);
                jsonText = Encoding.UTF8.GetString(stream.ToArray());
            }
            return jsonText;
        }

        /// <summary>
        /// 把json转model
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T jsonTextToModel(string jsonText)
        {
            T model = new T();
            if (string.IsNullOrEmpty(jsonText.Trim()))
                return model;
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
                {
                    DataContractJsonSerializer serializer1 = new DataContractJsonSerializer(typeof(T));
                    model = (T)serializer1.ReadObject(ms);
                }
            }
            catch
            {
            }
            return model;
        }

        #endregion
  
        #region 私有方法
        private static object FillObjectSetValue(PropertyInfo propertyInfo, DataRow dr)
        {
            object obj = Activator.CreateInstance(propertyInfo.PropertyType);
            //IsRead read = Attribute.GetCustomAttribute(obj.GetType(), typeof(IsRead)) as IsRead;
            //if (read != null && read.Read == true) return obj;
            foreach (PropertyInfo propertyInfo1 in obj.GetType().GetProperties())
            {
                Type type = propertyInfo1.PropertyType;
                if (type.IsSealed)
                {
                    if (dr.Table.Columns.Contains(propertyInfo1.Name.Trim()))
                    {
                        if (propertyInfo1.CanWrite && dr[propertyInfo1.Name.Trim()] != DBNull.Value)
                            propertyInfo1.SetValue(obj, dr[propertyInfo1.Name.Trim()], null);
                    }
                }
                else
                {
                    propertyInfo1.SetValue(obj, FillObjectSetValue(propertyInfo1, dr), null);
                }
            }
            return obj;
        }

        private static void CreateDataTable(PropertyInfo propertyInfo, ref DataTable dataTable)
        {
            Type type = propertyInfo.PropertyType;
            if (type.IsSealed)
            {
                if (propertyInfo.Name != "CTimestamp")//一些字段为oracle中的Timesstarmp类型
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        dataTable.Columns.Add(new DataColumn(propertyInfo.Name, type.GetGenericArguments()[0]));
                    }
                    else
                    {
                        dataTable.Columns.Add(new DataColumn(propertyInfo.Name, type));
                    }
                }
                else
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, typeof(DateTime)));
                }
            }
            else
            {
                foreach (PropertyInfo propertyInfo1 in Activator.CreateInstance(type).GetType().GetProperties())
                {
                    CreateDataTable(propertyInfo1, ref dataTable);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 读取ini帮助类
    /// </summary>
    public static class IniHepler 
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        public static string GetConfig(string lpFileName, string lpAppName, string lpKeyName)
        {
            string encodingName = "utf-8";
            byte[] buffer = new byte[1024];
            if (System.IO.File.Exists(lpFileName))
            {
                int count = GetPrivateProfileString(getBytes(lpAppName, encodingName), getBytes(lpKeyName, encodingName), getBytes("", encodingName), buffer, 1024, lpFileName);
                return Encoding.GetEncoding(encodingName).GetString(buffer, 0, count).Trim();
            }
            return "";
        }

        public static void SetConfig(string section, string key, string val, string filePath)
        {
            long size = IniHepler.WritePrivateProfileString(section, key, val, filePath);
        }

        private static byte[] getBytes(string s, string encodingName)
        {
            return null == s ? null : Encoding.GetEncoding(encodingName).GetBytes(s);
        }
    }


    /// <summary>
    /// 时间操作类
    /// </summary>
    public class DateTimeOpt 
    {
        /// <summary>
        /// 获取指定年月日期的开始日期与结束日期
        /// </summary>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetFirstAndLastByYearAndMonth(DateTime datetime)
        {
            //本月第一天时间      
            DateTime dt_First = datetime.AddDays(1 - (datetime.Day));
            //获得某年某月的天数    
            int year = datetime.Year;
            int month = datetime.Month;
            int dayCount = DateTime.DaysInMonth(year, month);
            //本月最后一天时间    
            DateTime dt_Last = dt_First.AddDays(dayCount - 1);
            return Tuple.Create<DateTime, DateTime>(dt_First, dt_Last);
        }        
    }

    /// <summary>
    /// 读取设置memcached类
    /// </summary>
    public class MemcachedMgrOld
    {
        public static long lNumofMilliSeconds = (1000 * 60 * 60 * 2);/*缓存时间2个小时*/
        //public static long lNumofMilliSeconds = 0;/*缓存时间3个小时*/

        /// <summary>
        /// 获取一个key数据
        /// </summary>
        /// <param name="strKey">key值</param>
        /// <returns></returns>
        public static object GetVal(string strKey)
        {
            return DistCache.Get(strKey);
        }
        /// <summary>
        /// 获取一个key数据
        /// </summary>
        /// <typeparam name="T">泛型数据</typeparam>
        /// <param name="strKey">key</param>
        /// <returns>泛型数据</returns>
        public static T GetVal<T>(string strKey)
        {
            T t = DistCache.Get<T>(strKey);
            if (t == null)
            {
                object obj = DistCache.Get(strKey);
                if (obj == null) return t;
                T rst = JsonHelper.Deserialize<T>(obj.ToString());
                if (rst == null) return t;
                t = rst;
            }
            return t;
            //return DistCache.Get<T>(strKey);
        }

        public static bool SetVal(string strKey, object objValue)
        {
            if (lNumofMilliSeconds == 0)
                return DistCache.Add(strKey, objValue);
            else
                return DistCache.Add(strKey, objValue, lNumofMilliSeconds);
        }

        public static bool SetVal(string strKey, RstVar objValue)
        {
            if (lNumofMilliSeconds == 0)
                return DistCache.Add(strKey, JsonHelper.Serialize(objValue));
            else
                return DistCache.Add(strKey, JsonHelper.Serialize(objValue), lNumofMilliSeconds);
            //return DistCache.Add(strKey, JsonHelper.Serialize(objValue));
        }



        public static bool RemoveKey(string strKey)
        {
            object obj = DistCache.Remove(strKey);
            return Convert.ToBoolean(obj);
        }

        public static void RemoveAll()
        {
            DistCache.RemoveAll();
        }


    }


    public class EmailUtilities
    {
        private static EmailConfig _EmConfig = new EmailConfig();
        public static EmailConfig EmConfig { get { return _EmConfig; } set { _EmConfig = value; } }
        /// <summary>
        /// 發郵件函數
        /// </summary>
        /// <param name="subj1">主題</param>
        /// <param name="mailcont1">郵件正文</param>
        /// <param name="fromEmail">发件人Email(為空就是配置文件指定的發件人)</param>
        /// <param name="listReceipts">收件人列表</param>
        /// <param name="listCCs">副本列表</param>
        /// <param name="listAttachments">附件列表,如: c:\\temp\test.xls </param>
        public void SendMail(string subj1, string mailcont1, string fromEmail, List<string> listReceipts, List<string> listCCs, List<string> listAttachments)
        {
            string mailFrom = EmConfig.MailFrom;
            if (string.IsNullOrEmpty(fromEmail.Trim()))
                fromEmail = mailFrom;

            if (string.IsNullOrEmpty(mailFrom))
                throw new Exception("没有邮件发件人");


            MailAddress from = new MailAddress(fromEmail); //邮件的发件人
            MailMessage mail = new MailMessage();
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            SmtpClient client = new SmtpClient();
            try
            {
                string[] smtp = EmConfig.MailSmtpHost.Split(':');// ConfigurationManager.AppSettings["SmtpHost"].Split(':');
                int smtpPort = 25;
                if (smtp.Count() > 1)
                {
                    if (int.TryParse(smtp[1], out smtpPort) == false)
                    {
                        smtpPort = 25;
                    }
                }
                string smtpHost = smtp[0];
                string smtpUser = EmConfig.MailSmtpUser; //ConfigurationManager.AppSettings["SmtpUser"];
                string smtpPassword = EmConfig.MailSmtpPassword;

                if (string.IsNullOrEmpty(smtpHost))
                    throw new Exception("没有发送邮件主机地址");
                if (string.IsNullOrEmpty(smtpUser))
                    throw new Exception("没有邮件主机用户名");
                if (string.IsNullOrEmpty(smtpPassword))
                    throw new Exception("没有邮件主机用户名密码");

                //邮件标题
                mail.Subject = subj1;
                //邮件发件人
                mail.From = from;
                //邮件内容
                mail.Body = mailcont1;
                //邮件格式
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                //邮件发送级别
                mail.Priority = MailPriority.Normal;
                //邮件收件人
                foreach (string name in listReceipts)
                {
                    if (name.Trim() != string.Empty)
                    {
                        mail.To.Add(new MailAddress(name));
                    }
                }
                //邮件抄送收件人
                if (listCCs != null)
                {
                    foreach (string name in listCCs)
                    {
                        if (name.Trim() != string.Empty)
                        {
                            mail.CC.Add(new MailAddress(name));
                        }
                    }
                }
                //邮件附件
                if (listAttachments != null)
                {
                    foreach (string name in listAttachments)
                    {
                        if (name.Trim() != string.Empty)
                        {
                            mail.Attachments.Add(new Attachment(name));
                        }
                    }
                }
                ///

                //SMTP主机
                client.Host = smtpHost;
                //SMTP 端口，默认的是 25
                //client.Timeout = 3000;
                client.Port = smtpPort;
                //
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                //邮箱登陆名和密码
                client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //正式发送
                client.Send(mail);
            }
            catch (SmtpFailedRecipientsException sfre)
            {
                throw new Exception(sfre.Message);
            }
            catch (SmtpFailedRecipientException fre)
            {
                throw new Exception(fre.Message);
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message);
                //throw new Exception(string.Format("郵件發送失敗:{0}\r\n{1}", ex2.Message, ex2.StackTrace));
            }
            finally
            {
                mail.Dispose();
                client.Dispose();
            }
        }
    }
}
