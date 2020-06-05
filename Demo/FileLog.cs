using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataProcess
{
    public class FileLog
    {
		private static readonly object lockObj = new object();
        private static string strFileName = "error.log";
        private static int iFileLen = 1000 * 1024; // K

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="errorText"></param>
        public static void Error(string errorText)
        {
            //if (Config.Log_levenl > 0)
            Write_Log("ERROR", errorText);
        }
        /// <summary>
        ///  向日志文件写入调试信息
        /// </summary>
        /// <param name="content"></param>
        public static void Debug(string content)
        {
          
                Write_Log("DEBUG", content);
        }
        /// <summary>
        /// 向日志文件写入运行时信息
        /// </summary>
        /// <param name="errorText"></param>
        public static void Info(string content)
        {
                Write_Log("INFO", content);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLog(string content)
        {
            Write_Log("LOG", content);
        }

        private static void Write_Log(string type, string content)
        {
            try
            {
                string filePath  = AppDomain.CurrentDomain.BaseDirectory + @"/logs";
                if (System.IO.Directory.Exists(filePath) == false)
                    System.IO.Directory.CreateDirectory(filePath);
                string logPath = filePath + "/" + strFileName;
                lock (lockObj)
                {
                    FileInfo fi = new FileInfo(logPath);
                    if (fi.Exists && fi.Length > iFileLen)
                    {
                        FileCopy(logPath);
                        fi.Delete();
                    }
                    using (StreamWriter sw = new StreamWriter(logPath, true, Encoding.Unicode))
                    {
                        sw.WriteLine(string.Format("{0}-->{1}-->{2}", DateTime.Now, type, content));
                    }
                }

            }
            catch
            {
            }
        }

        static private void FileCopy(string strSrcPath)
        {
            string strPathBase = Path.GetDirectoryName(strSrcPath);
            string strFile = Path.GetFileNameWithoutExtension(strSrcPath);
            int i=1;
            //strFile += i.ToString();
            string strNew = strPathBase + "\\" + strFile + ".log";            
            while (File.Exists(strNew))            
            {
                strFile = "_" + i;
                strNew = strPathBase + "\\" + strFile + ".log";
                i++;
            }
            File.Copy(strSrcPath, strNew);
        }
    }
}
