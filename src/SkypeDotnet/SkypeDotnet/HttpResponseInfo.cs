using System;
using System.Net;

namespace SkypeDotnet
{
    public class HttpResponseInfo
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ResponseData { get; set; }

        public Uri ResponseUrl { get; set; }
    }
}