using System;
using System.Security.Cryptography;
using System.Text;

namespace OctopusLibrary.Crypto
{
    public class Sha256
    {
        /// <summary>
        /// Hash로 암호화된 문자열 비교
        /// </summary>
        /// <param name="password">입력한 암호</param>
        /// <param name="correctHash">저장된 Hash암호</param>
        /// <returns>bool</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            if (!String.IsNullOrEmpty(password))
            {
                if (CreateHash(password) == correctHash)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Hash 암호화
        /// </summary>
        /// <param name="password">암호화할 문자열</param>
        /// <returns>string</returns>
        public static string CreateHash(string password)
        {
            StringBuilder result = new StringBuilder(64);

            if (!String.IsNullOrEmpty(password))
            {
                SHA256 mySHA256 = SHA256Managed.Create();
                byte[] temp = Encoding.Default.GetBytes(password);
                byte[] hashValue = mySHA256.ComputeHash(temp);
                int i = 0;
                for (i = 0; i < hashValue.Length; i++)
                {
                    result.AppendFormat("{0:X2}", hashValue[i]);
                }
            }

            return result.ToString();
        }
    }
}
