using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Demo
{
    /// <summary>
    /// 调用C函数模块
    /// </summary>
    internal static class callCFun_Back
    {
        /// <summary>
        /// 把一个日期转换为秒
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        [DllImport("EncryptDES.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern UInt32 u32CalTotalSecond(int year, int month, int day, int hour, int minute, int second);

        /// <summary>
        /// 按密钥加密数组
        /// </summary>
        /// <param name="pi_buf">8位长度的数组</param>
        /// <param name="ks_buf">8位长度的密钥</param>
        /// <param name="co_buf">8位长度的加密后数据</param>
        [DllImport("EncryptDES.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void f_des_encrypt(byte[] pi_buf, byte[] ks_buf, byte[] co_buf);

    }

    internal class callCFun
    {
        private static byte[] DAYS_PER_MONTH = { 0, 0x31, 0x28, 0x31, 0x30, 0x31, 0x30, 0x31, 0x31, 0x30, 0x31, 0x30, 0x31 };

        internal static int u32CalTotalSecond(int Year, int Month, int Day, int Hour, int Minute, int Second)
        {
            byte year = (byte)Year, month = (byte)Month, day = (byte)Day, hour = (byte)Hour, minute = (byte)Minute, second = (byte)Second;
            int rtn = u32CalTotalMinute(Year, Month, Day, Hour, Minute);
            rtn *= 60;								// 计算秒
            byte ss = u8BcdToHex(second);
            rtn += ss;
            return rtn;
        }

        internal static int u32CalTotalMinute(int Year, int Month, int Day, int Hour, int Minute)
        {
            byte year = (byte)Year, month = (byte)Month, day = (byte)Day, hour = (byte)Hour, minute = (byte)Minute;
            int rtn = u32CalTotalHour(Year, Month, Day, Hour);	// 计算小时
            rtn *= 60;										// 计算分钟
            byte mm = u8BcdToHex(minute);
            rtn += mm;
            return rtn;
        }

        internal static int u32CalTotalHour(int Year, int Month, int Day, int Hour)
        {
            byte year = (byte)Year, month = (byte)Month, day = (byte)Day, hour = (byte)Hour;
            int rtn = u32CalTotalDay(Year, Month, Day);	// 计算天数
            rtn *= 24;								// 计算小时
            byte hh = u8BcdToHex(hour);
            rtn += hh;
            return rtn;
        }

        internal static int u32CalTotalDay(int Year, int Month, int Day)
        {
            byte year = (byte)Year, month = (byte)Month, day = (byte)Day;
            byte yy = u8BcdToHex(year);
            byte mm = u8BcdToHex(month);
            byte dd = u8BcdToHex(day);

            int rtn = 365 * yy;
            rtn += (yy != 0 ? (yy - 1) / 4 + 1 : 0);
            for (byte i = 1; i < mm; i++)
                rtn += u8BcdToHex(DAYS_PER_MONTH[i]);
            rtn = (yy % 4 == 0) && mm > 2 ? rtn + 1 : rtn; // 闰年
            rtn += dd;
            rtn--;     	//sub current day
            rtn += 6;	//map to week, 2000年1月1日星期六。这个days出来后是%7算星期 
            return rtn;
        }

        // 8位bcd码转换成8位十六进制码
        internal static byte u8BcdToHex(byte bcd)
        {
            int rst = ((bcd >> 4) * 10 + (bcd & 0x0f));
            return (byte)rst;
        }

        internal static void f_des_encrypt(byte[] pi_buf, byte[] ks_buf, byte[] co_buf)
        {
            byte[] ki_buf = new byte[96];
            f_build_ki(ks_buf, ki_buf);
            f_des(pi_buf, ki_buf, co_buf, 1);
        }

        private static void f_build_ki(byte[] ks_buf, byte[] ki_buf)
        {
            for (int i = 0; i < 96; i++)
            {
                ki_buf[i] = 0;
            }
            for (int i = 0; i < 16; i++)
            {
                byte byte_n = 0;
                byte bit_n = 0;
                byte shift_len = encryptData.m_key_shift_tbl[i];
                for (int j = 0; j < 56; j++)
                {
                    if ((ks_buf[byte_n] & encryptData.m_bit8_tbl[bit_n]) != 0)
                    {
                        byte k = encryptData.m_pc1_tbl[j];
                        if (k >= 28)
                        {
                            k -= shift_len;
                            if (k < 28)
                            {
                                k += 28;
                            }
                        }
                        else
                        {
                            k -= shift_len;
                            if (k > 28)
                            {
                                k += 28;
                            }
                        }
                        k = encryptData.m_pc2_tbl[k];
                        if (k < 48)
                        {
                            int byte_n1 = k >> 3;
                            int bit_n1 = k & 0x07;
                            ki_buf[byte_n1 + (i * 6)] |= encryptData.m_bit8_tbl[bit_n1];
                        }
                    }
                    if (++bit_n >= 7)
                    {
                        bit_n = 0;
                        byte_n++;
                    }
                }
            }
        }

        private static void f_des(byte[] pi_buf, byte[] ki_buf, byte[] co_buf, byte encry_decry)
        {
            byte i, j, k;
            int byte_n, bit_n, byte_n1, bit_n1;
            byte[] tmp_buf = new byte[8], s_out_buf = new byte[4];

            for (i = 0; i < 8; i++)
            {
                co_buf[i] = 0;
            }
            byte_n = 0; bit_n = 0;
            for (i = 0; i < 64; i++)
            {
                j = encryptData.m_ip_tbl[i];
                byte_n1 = j >> 3;
                bit_n1 = j & 0x07;
                if ((pi_buf[byte_n1] & encryptData.m_bit8_tbl[bit_n1]) != 0)
                {
                    co_buf[byte_n] |= encryptData.m_bit8_tbl[bit_n];
                }
                if (++bit_n >= 8)
                {
                    bit_n = 0;
                    byte_n++;
                }
            }
            for (i = 0; i < 16; i++)
            {
                byte_n = 0; bit_n = 0;
                for (j = 0; j < 6; j++)
                {
                    tmp_buf[j] = 0;
                }
                for (j = 0; j < 48; j++)
                {
                    k = (byte)encryptData.m_e_tbl[j];
                    byte_n1 = 4 + (k >> 3);
                    bit_n1 = k & 0x07;
                    if ((co_buf[byte_n1] & encryptData.m_bit8_tbl[bit_n1]) != 0)
                    {
                        tmp_buf[byte_n] |= encryptData.m_bit8_tbl[bit_n];
                    }
                    if (++bit_n >= 8)
                    {
                        bit_n = 0;
                        byte_n++;
                    }
                }
                if (encry_decry != 0)
                {
                    j = (byte)(i * 6);
                }
                else
                {
                    j = (byte)((15 - i) * 6);
                }
                tmp_buf[0] ^= ki_buf[j];
                tmp_buf[1] ^= ki_buf[j + 1];
                tmp_buf[2] ^= ki_buf[j + 2];
                tmp_buf[3] ^= ki_buf[j + 3];
                tmp_buf[4] ^= ki_buf[j + 4];
                tmp_buf[5] ^= ki_buf[j + 5];
                byte_n = (tmp_buf[0] & 0x80) >> 6 | (tmp_buf[0] & 0x04) >> 2;
                bit_n = (tmp_buf[0] & 0x78) >> 3;
                s_out_buf[0] = (byte)(encryptData.m_s1_tbl[byte_n, bit_n] << 4);
                byte_n = (tmp_buf[0] & 0x02) | (tmp_buf[1] & 0x10) >> 4;
                bit_n = (tmp_buf[0] & 0x01) << 3 | tmp_buf[1] >> 5;
                s_out_buf[0] |= (byte)(encryptData.m_s2_tbl[byte_n, bit_n]);
                byte_n = (tmp_buf[1] & 0x08) >> 2 | (tmp_buf[2] & 0x40) >> 6;
                bit_n = (tmp_buf[1] & 0x07) << 1 | tmp_buf[2] >> 7;
                s_out_buf[1] = (byte)(encryptData.m_s3_tbl[byte_n, bit_n] << 4);
                byte_n = (tmp_buf[2] & 0x20) >> 4 | (tmp_buf[2] & 0x01);
                bit_n = (tmp_buf[2] & 0x1e) >> 1;
                s_out_buf[1] |= (byte)(encryptData.m_s4_tbl[byte_n, bit_n]);
                byte_n = (tmp_buf[3] & 0x80) >> 6 | (tmp_buf[3] & 0x04) >> 2;
                bit_n = (tmp_buf[3] & 0x78) >> 3;
                s_out_buf[2] = (byte)(encryptData.m_s5_tbl[byte_n, bit_n] << 4);
                byte_n = (tmp_buf[3] & 0x02) | (tmp_buf[4] & 0x10) >> 4;
                bit_n = (tmp_buf[3] & 0x01) << 3 | tmp_buf[4] >> 5;
                s_out_buf[2] |= (byte)(encryptData.m_s6_tbl[byte_n, bit_n]);
                byte_n = (tmp_buf[4] & 0x08) >> 2 | (tmp_buf[5] & 0x40) >> 6;
                bit_n = (tmp_buf[4] & 0x07) << 1 | tmp_buf[5] >> 7;
                s_out_buf[3] = (byte)(encryptData.m_s7_tbl[byte_n, bit_n] << 4);
                byte_n = (tmp_buf[5] & 0x20) >> 4 | (tmp_buf[5] & 0x01);
                bit_n = (tmp_buf[5] & 0x1e) >> 1;
                s_out_buf[3] |= (byte)(encryptData.m_s8_tbl[byte_n, bit_n]);
                byte_n = 0;
                bit_n = 0;
                tmp_buf[0] = 0;
                tmp_buf[1] = 0;
                tmp_buf[2] = 0;
                tmp_buf[3] = 0;
                for (j = 0; j < 32; j++)
                {
                    k = encryptData.m_p_tbl[j];
                    byte_n1 = k >> 3;
                    bit_n1 = k & 0x07;
                    if ((s_out_buf[byte_n1] & encryptData.m_bit8_tbl[bit_n1]) != 0)
                    {
                        tmp_buf[byte_n] |= encryptData.m_bit8_tbl[bit_n];
                    }
                    if (++bit_n >= 8)
                    {
                        bit_n = 0;
                        byte_n++;
                    }
                }
                tmp_buf[0] ^= co_buf[0];
                tmp_buf[1] ^= co_buf[1];
                tmp_buf[2] ^= co_buf[2];
                tmp_buf[3] ^= co_buf[3];
                co_buf[0] = co_buf[4];
                co_buf[1] = co_buf[5];
                co_buf[2] = co_buf[6];
                co_buf[3] = co_buf[7];
                co_buf[4] = tmp_buf[0];
                co_buf[5] = tmp_buf[1];
                co_buf[6] = tmp_buf[2];
                co_buf[7] = tmp_buf[3];
            }
            tmp_buf[0] = co_buf[4];
            tmp_buf[1] = co_buf[5];
            tmp_buf[2] = co_buf[6];
            tmp_buf[3] = co_buf[7];
            tmp_buf[4] = co_buf[0];
            tmp_buf[5] = co_buf[1];
            tmp_buf[6] = co_buf[2];
            tmp_buf[7] = co_buf[3];
            byte_n = 0;
            bit_n = 0;
            co_buf[0] = 0;
            co_buf[1] = 0;
            co_buf[2] = 0;
            co_buf[3] = 0;
            co_buf[4] = 0;
            co_buf[5] = 0;
            co_buf[6] = 0;
            co_buf[7] = 0;
            for (i = 0; i < 64; i++)
            {
                j = encryptData.m_ip1_tbl[i];
                byte_n1 = j >> 3;
                bit_n1 = j & 0x07;
                if ((tmp_buf[byte_n1] & encryptData.m_bit8_tbl[bit_n1]) != 0)
                {
                    co_buf[byte_n] |= encryptData.m_bit8_tbl[bit_n];
                }
                if (++bit_n >= 8)
                {
                    bit_n = 0;
                    byte_n++;
                }
            }
        }
    }

    internal class encryptData
    {
        /*build sub-ki_buf shift num*/
        internal static byte[] m_key_shift_tbl = new byte[16] { 1, 2, 4, 6, 8, 10, 12, 14, 15, 17, 19, 21, 23, 25, 27, 28 };
        internal static byte[] m_bit8_tbl = new byte[8] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
        internal static byte[] m_pc1_tbl = new byte[56]{
           7,  15,  23,  55,  51,  43,  35,
           6,  14,  22,  54,  50,  42,  34,
           5,  13,  21,  53,  49,  41,  33,
           4,  12,  20,  52,  48,  40,  32,
           3,  11,  19,  27,  47,  39,  31,
           2,  10,  18,  26,  46,  38,  30,
           1,   9,  17,  25,  45,  37,  29,
           0,   8,  16,  24,  44,  36,  28};
        /*PC-2 tbl*/
        internal static byte[] m_pc2_tbl = new byte[56]{
           4,  23,   6,  15,   5,   9,  19,  17,
          48,  11,   2,  14,  22,   0,   8,  18,
           1,  48,  13,  21,  10,  48,  12,   3,
          48,  16,  20,   7,  46,  30,  26,  47,
          34,  40,  48,  45,  27,  48,  38,  31,
          24,  43,  48,  36,  33,  42,  28,  35,
          37,  44,  32,  25,  41,  48,  29,  39};
        /*IP tbl*/
        internal static byte[] m_ip_tbl = new byte[64]{
          57,  49,  41,  33,  25,  17,  9,   1,
          59,  51,  43,  35,  27,  19,  11,  3,
          61,  53,  45,  37,  29,  21,  13,  5,
          63,  55,  47,  39,  31,  23,  15,  7,
          56,  48,  40,  32,  24,  16,   8,  0,
          58,  50,  42,  34,  26,  18,  10,  2,
          60,  52,  44,  36,  28,  20,  12,  4,
          62,  54,  46,  38,  30,  22,  14,  6 };
        /*E tbl*/
        internal static byte[] m_e_tbl = new byte[48]{
          31,   0,   1,    2,    3,   4,
           3,   4,   5,    6,    7,   8,
           7,   8,   9,   10,   11,  12,
          11,  12,  13,   14,   15,  16,
          15,  16,  17,   18,   19,  20,
          19,  20,  21,   22,   23,  24,
          23,  24,  25,   26,   27,  28,
          27,  28,  29,   30,   31,   0 };
        /*S box*/
        internal static byte[,] m_s1_tbl = new byte[4, 16] {
          {14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7},
          { 0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8},
          { 4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0},
          {15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13}};

        internal static byte[,] m_s2_tbl = new byte[4, 16]{
          {15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10},
          { 3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5},
          { 0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15},
          {13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14, 9}};
        internal static byte[,] m_s3_tbl = new byte[4, 16]{
          {10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8},
          {13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1},
          {13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7},
          {1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12}};
        internal static byte[,] m_s4_tbl = new byte[4, 16]{
          { 7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15},
          {13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9},
          {10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4},
          { 3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14}};
        internal static byte[,] m_s5_tbl = new byte[4, 16]{
          { 2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9},
          {14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6},
          { 4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14},
          {11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10,  4,  5, 3}};
        internal static byte[,] m_s6_tbl = new byte[4, 16]{
          {12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11},
          {10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8},
          { 9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6},
          { 4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13}};
        internal static byte[,] m_s7_tbl = new byte[4, 16]{
          { 4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1},
          {13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6},
          { 1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2},
          { 6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12}};
        internal static byte[,] m_s8_tbl = new byte[4, 16]{
          {13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7},
          { 1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2},
          { 7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8},
          { 2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11}};
        /*P tbl*/
        internal static byte[] m_p_tbl = new byte[32]{
          15,   6,  19,  20,  28,  11,  27,  16,
           0,  14,  22,  25,   4,  17,  30,   9,
           1,   7,  23,  13,  31,  26,   2,   8,
          18,  12,  29,   5,  21,  10,   3,  24 };
        /*IP_1 tbl*/
        internal static byte[] m_ip1_tbl = new byte[64]{
          39,  7,  47,  15,  55,  23,  63,  31,
          38,  6,  46,  14,  54,  22,  62,  30,
          37,  5,  45,  13,  53,  21,  61,  29,
          36,  4,  44,  12,  52,  20,  60,  28,
          35,  3,  43,  11,  51,  19,  59,  27,
          34,  2,  42,  10,  50,  18,  58,  26,
          33,  1,  41,   9,  49,  17,  57,  25,
          32,  0,  40,   8,  48,  16,  56,  24 };
    }

    /// <summary>
    /// BCD码转换模块
    /// </summary>
    internal class hex
    {
        /// <summary>
        /// 此函数用于将8421BCD码转换为十进制数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static byte BCD2hex(byte bcd)
        {
            byte i = (byte)(bcd & 0x0f); //按位与，i得到低四位数。
            bcd >>= 4; //右移四位，将高四位移到低四位的位置，得到高四位码值。
            bcd &= 0x0f; //防止移位时高位补进1，只保留高四位码值
            bcd *= 10; //高位码值乘以10
            i += bcd; //然后与第四位码值相加。
            return i; //将得到的十进制数返回
        }
        /*
        //假设16进制数3FH，其十进制数值为3*16+15=63，所以先进行如下过程：
        //63÷10=6……3
        //即商是6，余数是
        //如果将商乘以16再加上余数，就是：
        //6×16+3=99，其16进制数就是63H，即是十进制数63（16进制为3FH）的BCD码为63H。
        //如果在单片机中，程序这么写：
        //char HEX,BCD；
        //BCD=(HEX/10*16)+(HEX%10);
        //其中HEX存储十六进制数，BCD中存储的就是其BCD码。        
         */
        /// <summary>
        /// 十六进制数转BCD码。
        /// </summary>
        /// <param name="hex">十六进制数</param>
        /// <returns>BCD码</returns>
        internal static byte hexBcd2(byte hex)
        {
            return (byte)((hex / 10 * 16) + (hex % 10));
        }
    }


}
