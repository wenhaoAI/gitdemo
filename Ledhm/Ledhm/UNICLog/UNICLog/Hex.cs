using System;
using System.Collections.Generic;
using System.Text;

namespace abtkModbus
{
    public class Hex
    {


        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes, string split = "")
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + split;
                }
            }
            return returnStr;
        }

        public static string byte_to_string2_wei(int bytetotr)
        {
            Byte tb = (Byte)bytetotr;
            return tb.ToString("X2");

        }
        /** 
    * byte数组中取int数值，本方法适用于(低位在前，高位在后)的顺序，和和intToBytes（）配套使用
    *  
    * @param src 
    *            byte数组 
    * @param offset 
    *            从数组的第offset位开始 
    * @return int数值 
    */
        private static int bytesToInt(byte[] src, int offset)
        {
            int value;
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }
        public static string hecheng(string[] st)
        {
            string tor = "";
            for (int i = 0; i < st.Length; i++)
            {
                tor += st[i];
                if (i != st.Length - 1)
                {
                    tor += " ";
                }

            }
            return tor;
        }
        public static string strimString_to_2(string totrim)
        {
            string tor = totrim.Trim();
            if (tor.Length == 1)
            {
                tor = "0" + tor;
            }
            else if (tor.Length > 2)
            {
                tor.Remove(2);
            }
            return tor;
        }
        /** 
        * byte数组中取int数值，本方法适用于(低位在后，高位在前)的顺序。和int2byte2（）配套使用
        */
        public static int bytesToInt2(byte[] src, int offset)
        {
            int value;
            value = (int)(((src[offset] & 0xFF) << 8)
                    | ((src[offset + 1] & 0xFF) << 0));
             
            return value;
        }

        /// <summary> 
        /// 从汉字转换到16进制 
        /// </summary> 
        /// <param name="s"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <param name="fenge">是否每字符用逗号分隔</param> 
        /// <returns></returns> 
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格 
                //throw new ArgumentException("s is not valid chinese string!"); 
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        private static void int2byte(int n, ref byte[] buf, int offset)
        {
            buf[offset] = (byte)(n >> 24);
            buf[offset + 1] = (byte)(n >> 16);
            buf[offset + 2] = (byte)(n >> 8);
            buf[offset + 3] = (byte)n;
        }

        /// <summary>
        /// / //int转两个byte
        /// </summary>
        /// <param name="n"></param>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        public static void int2byte2(int n, ref byte[] buf, int offset)
        {

            buf[offset] = (byte)(n >> 8);
            buf[offset + 1] = (byte)n;
        }

        ///<summary> 
        /// 从16进制转换成汉字 
        /// </summary> 
        /// <param name="hex"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <returns></returns> 
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格 
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }
    }
}
