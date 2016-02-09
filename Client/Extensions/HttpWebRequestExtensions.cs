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
        /// <summary>
        /// Sets an OAuth Authorization Header
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="auth">The OAuth token object authorize</param>
        /// <returns>The Request</returns>
        public static HttpWebRequest AuthorizeAs(this HttpWebRequest request, OAuthAccessToken auth)
        {
            if (request.Headers.AllKeys.Any(x => x.Equals("Authorization")))
            {
                request.Headers.Remove(HttpRequestHeader.Authorization);
            }
            request.Headers.Add(auth.AuthorizationHeader);
            return request;
        }

        /// <summary>
        /// Sets a basic authorization header
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="basicCredentials">The basic credentials to use</param>
        /// <returns>The Request</returns>
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

        /// <summary>
        /// Sets the Credentials on the Request
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="auth">an ICredentials object with the credentials to use</param>
        /// <returns>The Request</returns>
        public static HttpWebRequest AuthorizeAs(this HttpWebRequest request, ICredentials auth)
        {
            request.Credentials = auth;
            return request;
        }

        /// <summary>
        /// Sets the ContinueTimeout, ReadWriteTimeout and general Timeout value on the Request
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="timeout">The Timeout value</param>
        /// <returns>The Request</returns>
        public static HttpWebRequest SetTimeout(this HttpWebRequest request, TimeSpan timeout)
        {
            request.ContinueTimeout = request.ReadWriteTimeout = request.Timeout = (int)timeout.TotalMilliseconds;
            return request;
        }

        /// <summary>
        /// Sets the ContinueTimeout, ReadWriteTimeout and general Timeout value on the Request
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="seconds">The timeout value in seconds</param>
        /// <returns>The Request</returns>
        public static HttpWebRequest SetTimeout(this HttpWebRequest request, int seconds)
        {
            request.ContinueTimeout = request.ReadWriteTimeout = request.Timeout = (seconds * 1000);
            return request;
        }

        /// <summary>
        /// Sets the Accept header on the request
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="contentTypes">A list of accepted content types (e.g. text/html, text/plain, etc.)</param>
        /// <returns></returns>
        public static HttpWebRequest Accepts(this HttpWebRequest request, params string[] contentTypes)
        {
            request.Accept = string.Join(";", contentTypes);
            return request;
        }

        /// <summary>
        /// Sets the content type of the request (e.g. application/json, etc)
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="requestContent">A predefined content type</param>
        /// <returns>The request</returns>
        public static HttpWebRequest WithContent(this HttpWebRequest request, HttpClient.RequestContentTypes requestContent)
        {
            request.ContentType = HttpClient.GetContentType(requestContent);
            return request;
        }

        /// <summary>
        /// Sets the content type and body content on the request. The body content is serialized into the body request stream
        /// </summary>
        /// <param name="request">The Request</param>
        /// <param name="requestContent">A predefined content type</param>
        /// <param name="content">the content as an object</param>
        /// <returns>The Request</returns>
        public static HttpWebRequest WithContent(this HttpWebRequest request, HttpClient.RequestContentTypes requestContent, object content)
        {
            return WithContent<object>(request, requestContent, content);
        }


        /// <summary>
        /// Sets the content type and body content on the request. The body content is serialized into the body request stream
        /// </summary>
        /// <typeparam name="T">The content object type</typeparam>
        /// <param name="request">The Request</param>
        /// <param name="requestContent">A predefined content type</param>
        /// <param name="content">The content object of the type T</param>
        /// <returns>The Request</returns>
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

        
        /// <summary>
        /// Executes the request asynchroniously
        /// </summary>
        /// <typeparam name="TResponse">The type of response data</typeparam>
        /// <param name="request">The Request</param>
        /// <returns>A HttpResponse with the response data set as a TResponse object</returns>
        public static async Task<HttpResponse<TResponse>> ExecuteAsync<TResponse>(this HttpWebRequest request)
        {
            return await HttpClient.ExecuteAsync<TResponse>(request);
        }

        /// <summary>
        /// Executes the request asynchroniously
        /// </summary>
        /// <typeparam name="TResponse">The type of response data</typeparam>
        /// <param name="request">The Request</param>
        /// <returns>A HttpResponse with the response data set as a TResponse object</returns>
        public static HttpResponse<TResponse> Execute<TResponse>(this HttpWebRequest request)
        {
            return HttpClient.Execute<TResponse>(request);
        }

        












    }
}
