using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TinyHttp;

namespace System.Net
{
    public static class HttpWebRequestExtensions
    {
        public static HttpWebRequest AuthorizeAs(this HttpWebRequest request, OAuthAccessToken auth)
        {
            if (request.Headers.AllKeys.Any(x => x.Equals("Authorization")))
            {
                request.Headers.Remove(HttpRequestHeader.Authorization);
            }
            request.Headers.Add(auth.AuthorizationHeader);
            return request;
        }

        public static HttpWebRequest AuthorizeAs(this HttpWebRequest request, ICredentials auth)
        {
            request.Credentials = auth;
            return request;
        }

        public static HttpWebRequest SetTimeout(this HttpWebRequest request, TimeSpan timeout)
        {
            request.ContinueTimeout = request.ReadWriteTimeout = request.Timeout = (int)timeout.TotalMilliseconds;
            return request;
        }

        



    }
}
