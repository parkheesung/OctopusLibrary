using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class Sha512
    {
        private const int PBKDF2_ITERATIONS = 1000;

        /// <summary>
        /// Hash로 암호화된 문자열 비교
        /// </summary>
        /// <param name="password">입력한 암호</param>
        /// <param name="correctHash">저장된 Hash암호</param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            if (!String.IsNullOrEmpty(password))
            {
                char[] delimiter = { ':' };
                string[] split = correctHash.Split(delimiter);
                int iterations = Int32.Parse(split[0]);
                byte[] salt = Convert.FromBase64String(split[1]);
                byte[] hash = Convert.FromBase64String(split[2]);
                byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
                return SlowEquals(hash, testHash);
            }
            else
            {
                return false;
            }
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Hash 암호화
        /// </summary>
        /// <param name="password">암호화할 문자열</param>
        /// <returns></returns>
        public static string CreateHash(string password)
        {
            RNGCryptoServiceProvider CString = new RNGCryptoServiceProvider();
            byte[] salt = new byte[24];
            CString.GetBytes(salt);
            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, 24);
            return String.Format("{0}:{1}:{2}", PBKDF2_ITERATIONS, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Hash 암호화 전용 Salt
        /// </summary>
        /// <param name="password">암호화된 문자열</param>
        /// <param name="salt">Salt</param>
        /// <param name="iterations">IterationCount</param>
        /// <param name="outputBytes">Byte Size</param>
        /// <returns></returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
