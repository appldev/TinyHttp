using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TinyHttp
{
    public class HttpResponse<TResponseData>
    {
        internal HttpResponse(TResponseData responseData, Exception exception = null, HttpStatusCode responseStatus = HttpStatusCode.OK, WebExceptionStatus exceptionStatus = WebExceptionStatus.Success)
        {
            this.ResponseData = responseData;
            this.ResponseStatus = responseStatus;
            this.ExceptionStatus = exceptionStatus;
            this.Exception = exception;
        }

        public readonly HttpStatusCode ResponseStatus;
        public readonly WebExceptionStatus ExceptionStatus;
        public readonly TResponseData ResponseData;
        public readonly Exception Exception;
    }
}
