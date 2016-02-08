using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TinyHttp;

namespace TinyHttp
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

        public static HttpWebRequest AuthorizeAs(this HttpWebRequest request, string basicCredentials)
        {
            if (request.Headers.AllKeys.Any(x => x.Equals("Authorization")))
            {
                request.Headers.Remove(HttpRequestHeader.Authorization);
            }
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("Authorization", "Basic " + basicCredentials);
            request.Headers.Add(nvc);
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

        public static HttpWebRequest SetTimeout(this HttpWebRequest request, int seconds)
        {
            request.ContinueTimeout = request.ReadWriteTimeout = request.Timeout = (seconds * 1000);
            return request;
        }

        public static HttpWebRequest Accepts(this HttpWebRequest request, params string[] contentTypes)
        {
            request.Accept = string.Join(";", contentTypes);
            return request;
        }

        public static HttpWebRequest WithContent(this HttpWebRequest request, HttpClient.RequestContentTypes requestContent)
        {
            request.ContentType = HttpClient.GetContentType(requestContent);
            return request;
        }

        public static HttpWebRequest WithContent(this HttpWebRequest request, HttpClient.RequestContentTypes requestContent, object content)
        {
            return WithContent<object>(request, requestContent, content);
        }


        public static async Task<HttpResponse<TResponse>> ExecuteAsync<TResponse>(this HttpWebRequest request)
        {
            return await HttpClient.ExecuteAsync<TResponse>(request);
        }

        public static HttpWebRequest WithContent<T>(this HttpWebRequest request, HttpClient.RequestContentTypes requestContent, T content)
        {
            request.ContentType = HttpClient.GetContentType(requestContent);
            StringBuilder sb = new StringBuilder();
            if (requestContent == HttpClient.RequestContentTypes.Json)
            {
                sb.Append(JsonConvert.SerializeObject(content, Formatting.None, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            }
            else if (requestContent == HttpClient.RequestContentTypes.Xml)
            {
                if (typeof(T) == typeof(string))
                {
                    sb.Append(Convert.ToString(content));
                }
                else if (typeof(T) == typeof(System.Xml.XmlDocument))
                {
                    sb.Append((content as System.Xml.XmlDocument).OuterXml);
                }
                else
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
                    {
                        serializer.Serialize(sw, content);
                        sw.Close();
                    }
                }
            }
            else if (typeof(T) == typeof(string) || typeof(T) == typeof(object))
            {
                sb.Append(Convert.ToString(content));
            }
            else
            {
                throw new ArgumentException(string.Format("The type {0] cannot convert to content for the content type {1}", typeof(T).GetType().FullName, requestContent), "content");
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(request.GetRequestStream()))
            {
                sw.Write(sb.ToString());
                sw.Close();
            }
            return request;
        }












    }
}
