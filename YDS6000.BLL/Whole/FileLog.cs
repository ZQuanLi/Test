using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace YDS6000
{
    public class FileLog
    {
        private static readonly object lockObj = new object();
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "logs/application";
        /**
        * 实际的写日志操作
        * @param content 写入内容
        */
        public static void WriteLog(string content)
        {
            Write("", content);
        }
        /**
        * 实际的写日志操作
        * @param content 写入内容
        */
        public static void WriteLog(string className, string content)
        {
            Write(className, content);
        }
        /**
        * 实际的写日志操作
        * @param className 类名
        * @param content 写入内容
        */
        private static void Write(string className, string content)
        {
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            //string filename = path + "/FileLog.log";//用日期对日志文件命名
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名
            lock (lockObj)
            {
                //创建或打开日志文件，向日志文件末尾追加记录
                StreamWriter mySw = File.AppendText(filename);
                //向日志文件写入内容
                string write_content = "";
                if (!string.IsNullOrEmpty(className))
                    write_content = time + "-->" + className + ": " + content;
                else
                    write_content = time + "-->" + content;
                mySw.WriteLine(write_content);
                //关闭日志文件
                mySw.Close();
            }
        }
    }
}