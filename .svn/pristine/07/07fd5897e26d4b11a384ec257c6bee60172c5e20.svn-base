using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;

namespace YDS6000.Models
{
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为json字符串
        /// </summary>
        /// <param name="value">需要序列化的对象</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Serialize(object value)
        {
            IsoDateTimeConverter time = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(value, time);
        }
        /// <summary>
        /// 将json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象的返回类型</typeparam>
        /// <param name="json">需要序列化的字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, JsonHelper.ToStr(dataRow[dataColumn.ColumnName]));
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串
        }
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                if (dictionary[current] == null)
                                    dataTable.Columns.Add(current, typeof(System.String));
                                else
                                    dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            if (dictionary[current] == null)
                            {
                                dataRow[current] = DBNull.Value;
                            }
                            else
                            {
                                if (dictionary[current].ToString().ToLower().Equals("null"))
                                    dataRow[current] = DBNull.Value;
                                else
                                    dataRow[current] = dictionary[current];
                            }
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            result = dataTable;
            return result;
        }

        /// <summary>
        ///  转换为string字符串类型
        /// </summary>
        /// <param name="s">获取需要转换的值</param>
        /// <param name="format">需要格式化的位数</param>
        /// <returns>返回一个新的字符串</returns>
        private static string ToStr(object s, string format = "")
        {
            string result = "";
            try
            {
                if (format == "")
                {
                    result = s.ToString();
                }
                else
                {
                    result = string.Format("{0:" + format + "}", s);
                }
            }
            catch
            {
            }
            return result;
        }
    }
}
