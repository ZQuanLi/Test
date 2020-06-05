using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Configuration;
using YdSecurity;

namespace YDS6000.Models
{
    public class ConfigHelper
    {
        private static Configuration config = null;

        /// <summary>
        /// 打开默认的配置文件
        /// </summary>
        static ConfigHelper()
        {
            if (config == null)
            {
                try
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                catch
                {

                }
            }

            if (config == null)
            {
                try
                {
                    config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
                }
                catch
                {
                }
            }
        }

        public static void OpenConfig(string path)
        {
            config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(path);
        }

        public static string GetConnectionStrings(string Key)
        {
            return config.ConnectionStrings.ConnectionStrings[Key.Trim()].ToString().Trim();
        }

        /// <summary>
        /// 获取配置节点数据
        /// </summary>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public static string GetAppSettings(string Key)
        {
            return config.AppSettings.Settings[Key].Value;
        }

        /// <summary>
        /// 设置配置节点数据
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool SetAppSettings(string Key, string Value)
        {
            bool rtn = false;
            if (config.AppSettings.Settings[Key] != null)
            {
                config.AppSettings.Settings[Key].Value = Value;
                rtn = true;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            return rtn;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return DESEncrypt.Decrypt(Text); //解密;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return DESEncrypt.Encrypt(Text); //加密;
        }

        /// <summary>
        /// 获取电脑序列号
        /// </summary>
        /// <returns></returns>
        public static string GetComputerSn()
        {
            return License.GetComputerInfo();
        }
    }
}
