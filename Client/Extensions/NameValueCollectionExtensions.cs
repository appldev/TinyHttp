using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace TinyHttp
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Appends the NameValueCollection as query string parameters to a Url
        /// </summary>
        /// <param name="col">the NameValueCollection to append</param>
        /// <param name="Url">the Url to append to</param>
        /// <returns>the Url with the query string parameters</returns>
        public static string AppendAsQueryString(this NameValueCollection col, string Url)
        {
            if (col.Count > 0)
            {
                Url += Url.Contains("?") ? "" : (Url.EndsWith("&") ? "" : "?") + string.Join("&", GetPairs(col));
            }
            return Url;
        }

        /// <summary>
        /// Gets a NameValueCollection as UrlEncoded pairs for a query string
        /// </summary>
        /// <param name="nvc">The NameValueCollection</param>
        /// <returns>an array of string Url Encoded string pairs</returns>
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
