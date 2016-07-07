using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class AES256
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
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                String Output = Convert.ToBase64String(xBuff);
                return Output;
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
                try
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(Input);
                    foreach (var rep in Salt.ReplaceList())
                    {
                        builder.Replace(rep.Value, rep.Key);
                    }
                    return Decrypt(builder.ToString(), key);
                }
                catch
                {
                    return String.Empty;
                }
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
                try
                {
                    RijndaelManaged aes = new RijndaelManaged();
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    var decrypt = aes.CreateDecryptor();
                    byte[] xBuff = null;
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                        {
                            byte[] xXml = Convert.FromBase64String(Input);
                            cs.Write(xXml, 0, xXml.Length);
                        }

                        xBuff = ms.ToArray();
                    }

                    String Output = Encoding.UTF8.GetString(xBuff);
                    return Output;
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
