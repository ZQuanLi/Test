using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DataProcess.Rdc.Package
{
    internal class RdcFunc
    {
        /* 
         * 变量数据类型:
         * #define ECP_DATA_INT			    0		///> 4字节有符号数
         * #define ECP_DATA_DWORD			1		///> 4字节无符号数
         * #define ECP_DATA_FLOAT			2		///> 4字节IEEE754浮点数
         * ///////////////////////////////////////////////
         *  变量说明:
         *  #define RDC_OK					0		///> 成功
         *  #define RDC_ERR					1		///> 错误
         *  #define RDC_ISRUN				2		///> 正在运行
         *  #define RDC_ERRHANDLE			3		///> 错误的句柄
         *  #define RDC_ERR_NO_VAR			131		///> 无此变量
         */

        /// <summary>
        /// 实时数据变化回调函数 RDC_VarOk
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <param name="lpszDbVarName">数据名</param>
        /// <param name="lpszVal">数据值</param>
        /// <param name="lpszdateTime">日期时间,2009-02-14 13:58:08.123</param>
        /// <param name="dwUserData">用户数据，可以是一个用户自己的对象指针</param>
        /// <returns> @return BOOL</returns>
        internal delegate int RDC_VarOk(IntPtr handle, string lpszDbVarName, string lpszVal, string lpszdateTime, UInt32 dwUserData);

        /// <summary>
        /// 打开客户端
        /// </summary>
        /// <param name="ip">服务器IP</param>
        /// <param name="port">端口:8109</param>
        /// <param name="fnvarok">数据变化回调函数</param>
        /// <param name="dwUserData">暂时为 0</param>
        /// <returns>返回值：	客户端句柄	NULL	失败</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_Open", CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr RDC_Open(string ip, int port, RDC_VarOk fnvarok, UInt32 dwUserData);

        /// <summary>
        /// 添加需要获取的变量
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <param name="lpszDbVarName">服务方的变量名</param>
        /// <param name="nDataType">数据类型ECP_DATA_INT = 0，ECP_DATA_DWORD = 1，ECP_DATA_FLOAT = 2</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_AddVar", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_AddVar(IntPtr handle, string lpszDbVarName, int nDataType);


        /// <summary>
        /// 写变量
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <param name="lpszDbVarName">服务方的变量名</param>
        /// <param name="pData">数据类型ECP_DATA_INT = 0，ECP_DATA_DWORD = 1，ECP_DATA_FLOAT = 2</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_WriteVarByName", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_WriteVarByName(IntPtr handle, string lpszDbVarName, System.IntPtr pData);
        /// <summary>
        /// 启动客户端
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_StartRun", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_StartRun(IntPtr handle);

        /// <summary>
        /// 停止客户端
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_StopRun", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_StopRun(IntPtr handle);


        /// <summary>
        /// 关闭客户端
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3 </returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_Close", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_Close(IntPtr handle);

        /// <summary>
        /// 读事件处理信息
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <param name="lpszDbVar">存放事件处理空间</param>
        /// <param name="pData">存放事件处理空间长度</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_ReadEventDo", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_ReadEventDo(IntPtr handle, StringBuilder lpszDbVar, int pData);

        /// <summary>
        /// 读事件处理信息
        /// </summary>
        /// <param name="handle">客户端句柄</param>
        /// <param name="lpszDbVar">存放事件处理空间</param>
        /// <param name="pData">存放事件处理空间长度</param>
        /// <returns>返回值：RDC_OK = 0,RDC_ERR = 1,RDC_ISRUN = 2,RDC_ERRHANDLE = 3</returns>
        [DllImport("rdc.dll", CharSet = CharSet.Ansi, EntryPoint = "RDC_ReadEvent", CallingConvention = CallingConvention.StdCall)]
        internal static extern int RDC_ReadEvent(IntPtr handle, IntPtr lpszStr, int pData);

    }
}
