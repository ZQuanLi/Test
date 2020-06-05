using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Demo2
{
    /// <summary>
    /// 读取ini帮助类
    /// </summary>
    public static class dllHepler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">产生序列号</param>
        /// <param name="len">初始长度</param>
        /// <returns>尔类型 true成功，false产生序列号错误</returns>
        [DllImport("EncryptDES.dll", EntryPoint = "GetSerialNumber")]
        public static extern bool GetSerialNumber(StringBuilder code, int len);

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="code">注册码</param>
        /// <returns>布尔类型 true成功，false注册码错误</returns>
        [DllImport("EncryptDES.dll", EntryPoint = "RegisterDll")]        
        public static extern bool RegisterDll(StringBuilder code);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        /// <param name="electric">电量</param>
        /// <param name="data">加密数组byte的数组</param>
        /// <returns>尔类型 true成功加密数组有数据，false错误未注册等原因</returns>
        [DllImport("EncryptDES.dll", EntryPoint = "f_des_encryptByTimeEle")]
        public static extern bool f_des_encryptByTimeEle(int year, int month, int day, int hour, int minute, int second, float electric, byte[] data);
    }
}
