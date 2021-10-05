using System;
using System.Net;
using System.Net.Http;

namespace Easy.Transfers.CrossCutting.Configuration.Exceptions
{
    public class ApiHttpCustomException : HttpRequestException
    {
        public HttpStatusCode ResponseCode { get; }
      

        public ApiHttpCustomException(string message, HttpStatusCode responseCode, Exception innerException) : base(message, innerException, responseCode)
        {
            ResponseCode = responseCode;
       
        }
    }
}