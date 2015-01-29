using System;
using System.Collections.Generic;
using System.Net;

namespace SkypeDotnet.Model
{
    public class HttpResponseInfo
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ResponseData { get; set; }

        public Uri ResponseUrl { get; set; }

        public IDictionary<string, string> ResponseHeaders { get; set; }
    }
}