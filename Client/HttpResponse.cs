using System;
using System.Net;

namespace TinyHttp
{
    /// <summary>
    /// Represents the response from a call to the Execute() or ExecuteAsync() method in the HttpClient
    /// </summary>
    /// <typeparam name="TResponseData">The type of data contained in the response</typeparam>
    public class HttpResponse<TResponseData>
    {
        /// <summary>
        /// Used by the Execute() methods to initialize the HttpResponse
        /// </summary>
        /// <param name="responseData">The parsed response data from the Web Response</param>
        /// <param name="exception">The exception object, if the Execute method threw an exception</param>
        /// <param name="responseStatus">the HTTP Response status code from the remote host</param>
        /// <param name="exceptionStatus">The HTTP exception status from the HTTP Client</param>
        internal HttpResponse(TResponseData responseData, Exception exception = null, HttpStatusCode responseStatus = HttpStatusCode.OK, WebExceptionStatus exceptionStatus = WebExceptionStatus.Success)
        {
            this.ResponseData = responseData;
            this.ResponseStatus = responseStatus;
            this.ExceptionStatus = exceptionStatus;
            this.Exception = exception;
        }

        /// <summary>
        /// The response status code from the remote service (e.g. 200 OK, 400 BAD REQUEST, etc.)
        /// </summary>
        public readonly HttpStatusCode ResponseStatus;
        /// <summary>
        /// If the ResponseStatus is an error status, the ExceptionStatus will contain the Exception status code
        /// </summary>
        public readonly WebExceptionStatus ExceptionStatus;
        /// <summary>
        /// The data returned from the server
        /// </summary>
        public readonly TResponseData ResponseData;
        /// <summary>
        /// If an error occurs in the HttpClient, Exception will contain the Exception object thrown
        /// </summary>
        public readonly Exception Exception;
    }
}
