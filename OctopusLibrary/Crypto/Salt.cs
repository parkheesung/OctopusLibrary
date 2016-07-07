using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Crypto
{
    public class Salt
    {
        public const string PrivateKey = "AE74E08140UHX7W5";

        internal static List<KeyValuePair<string, string>> ReplaceList()
        {
            List<KeyValuePair<string, string>> ReplaceList = new List<KeyValuePair<string, string>>();
            ReplaceList.Add(new KeyValuePair<string, string>("=", "EvSxrTzQ"));
            ReplaceList.Add(new KeyValuePair<string, string>("+", "PDkcVjeDL"));
            ReplaceList.Add(new KeyValuePair<string, string>("/", "SkenFkkd"));
            return ReplaceList;
        }

        public static string RandomString(int length)
        {
            StringBuilder result = new StringBuilder();

            Random random = new Random();
            int rand = 0;
            for(int i = 0; i < length; i++)
            {
                do
                {
                    rand = random.Next(48, 123);
                }
                while (!(rand >= 48 && rand <= 122 && (rand <= 57 || rand >= 65) && (rand <= 90 || rand >= 97)));
                result.Append(Convert.ToChar(rand));
            }


            return result.ToString();
        }
    }
}
