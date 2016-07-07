using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class AES
    {
        public static string Encrypt(string publicKey, string privateKey, string input)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(input))
            {
                try
                {
                    RijndaelManaged Aes = new RijndaelManaged();

                    Aes.Mode = CipherMode.CBC;
                    Aes.KeySize = 128;
                    Aes.BlockSize = 128;
                    Aes.Key = Encoding.ASCII.GetBytes(publicKey);
                    Aes.IV = Encoding.ASCII.GetBytes(privateKey);

                    ICryptoTransform transform = Aes.CreateEncryptor();
                    byte[] plainText = Encoding.UTF8.GetBytes(input);
                    byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                    result = Convert.ToBase64String(cipherBytes);
                }
                catch
                {

                }
            }

            return result;
        }

        public static string Decrypt(string publicKey, string privateKey, string input)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(input))
            {
                try
                {
                    RijndaelManaged Aes = new RijndaelManaged();

                    Aes.Mode = CipherMode.CBC;
                    Aes.KeySize = 128;
                    Aes.BlockSize = 128;

                    byte[] encryptedData = Convert.FromBase64String(input);
                    Aes.Key = Encoding.ASCII.GetBytes(publicKey);
                    Aes.IV = Encoding.ASCII.GetBytes(privateKey);

                    ICryptoTransform transform = Aes.CreateDecryptor();
                    byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                    result = Encoding.UTF8.GetString(plainText);
                }
                catch
                {

                }
            }

            return result;
        }
    }
}
