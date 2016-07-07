using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class BitCrypto
    {
        /// <summary>
        /// Bit 암호화
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="num">움직일 Bit수</param>
        /// <returns></returns>
        public static string Obfuscation(string str, int num = 3)
        {
            StringBuilder result = new StringBuilder();

            if (!String.IsNullOrEmpty(str))
            {
                int value = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    value = Convert.ToInt32(Encoding.ASCII.GetBytes(str.Substring(i, 1))[0]) + num;
                    result.Append(value.ToString().PadLeft((num + 1), '0'));
                }
            }

            return result.ToString();
        }
    }
}
