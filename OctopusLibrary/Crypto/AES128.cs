using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class AES128
    {
        public static string Encrypt(string Input, string key, bool overridestring)
        {
            if (!String.IsNullOrEmpty(Input))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(Encrypt(Input, key));
                foreach (var rep in Salt.ReplaceList())
                {
                    builder.Replace(rep.Key, rep.Value);
                }
                return builder.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        public static string Encrypt(string Input, string key)
        {
            if (!String.IsNullOrEmpty(Input))
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(Input);
                byte[] Salt = Encoding.ASCII.GetBytes(key.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(PlainText, 0, PlainText.Length);
                cryptoStream.FlushFinalBlock();

                byte[] CipherBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();

                string EncryptedData = Convert.ToBase64String(CipherBytes);

                return EncryptedData;
            }
            else
            {
                return String.Empty;
            }
        }

        public static string Decrypt(string Input, string key, bool overridestring)
        {
            if (!String.IsNullOrEmpty(Input))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(Input);
                foreach (var rep in Salt.ReplaceList())
                {
                    builder.Replace(rep.Value, rep.Key);
                }
                return Decrypt(builder.ToString(), key);
            }
            else
            {
                return String.Empty;
            }
        }

        public static string Decrypt(string Input, string key)
        {
            if (!String.IsNullOrEmpty(Input))
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();

                byte[] EncryptedData = Convert.FromBase64String(Input);
                byte[] Salt = Encoding.ASCII.GetBytes(key.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                byte[] PlainText = new byte[EncryptedData.Length];

                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                memoryStream.Close();
                cryptoStream.Close();

                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

                return DecryptedData;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
