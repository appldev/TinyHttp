using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace TinyHttp
{
    public static class NameValueCollectionExtensions
    {
        public static string AppendAsQueryString(this NameValueCollection col, string Url)
        {
            if (col.Count > 0)
            {
                Url += Url.Contains("?") ? "" : (Url.EndsWith("&") ? "" : "?") + string.Join("&", GetPairs(col));
            }
            return Url;
        }

        public static string[] GetPairs(this NameValueCollection nvc)
        {
            List<string> keyValuePair = new List<string>();

            foreach (string key in nvc.AllKeys)
            {
                string encodedKey = HttpUtility.UrlEncode(key) + "=";
                foreach (string value in nvc.GetValues(key))
                {
                    keyValuePair.Add(encodedKey + HttpUtility.UrlEncode(value));
                }
            }
            return keyValuePair.ToArray();
        }
    }
}
