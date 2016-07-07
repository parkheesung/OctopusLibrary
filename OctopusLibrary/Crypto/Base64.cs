using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class Base64
    {
        /// <summary>
        /// Base64 암호화
        /// </summary>
        /// <param name="str">대상 문자열</param>
        /// <param name="overridestring">Salt 적용 여부</param>
        /// <returns></returns>
        public static string Encrypt(string str, bool overridestring = true)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(str))
            {
                try
                {
                    System.Text.Encoding en = System.Text.Encoding.UTF8;
                    byte[] strByte = en.GetBytes(str);
                    result = Convert.ToBase64String(strByte);
                    if (overridestring)
                    {
                        var ReplaceList = Salt.ReplaceList();
                        if (ReplaceList.Count > 0)
                        {
                            foreach (var obj in ReplaceList)
                            {
                                result = result.Replace(obj.Key, obj.Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Base64 복호화
        /// </summary>
        /// <param name="str">대상 문자열</param>
        /// <param name="overridestring">Salt 적용 여부</param>
        /// <returns></returns>
        public static string Decrypt(string str, bool overridestring = true)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(str))
            {
                try
                {
                    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                    System.Text.Decoder utf8Decode = encoder.GetDecoder();

                    if (overridestring)
                    {
                        var ReplaceList = Salt.ReplaceList();
                        if (ReplaceList.Count > 0)
                        {
                            foreach (var obj in ReplaceList)
                            {
                                str = str.Replace(obj.Value, obj.Key);
                            }
                        }
                    }
                    byte[] todecode_byte = Convert.FromBase64String(str);
                    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                    char[] decoded_char = new char[charCount];
                    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                    result = new String(decoded_char);
                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
            }

            return result;
        }
    }
}
