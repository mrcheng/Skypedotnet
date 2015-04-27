using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using SkypeDotnet.Model;

namespace SkypeDotnet.Abstract
{
    public interface IHttpClient
    {
        HttpResponseInfo SendGet(Uri url, Dictionary<string, string> customHeaders = null);

        HttpResponseInfo SendPost(Uri url, Dictionary<string, string> postParameters, Dictionary<string, string> customHeaders = null );

        HttpResponseInfo SendPost(Uri url, JObject json, Dictionary<string, string> customHeaders = null );

        HttpResponseInfo SendPost(Uri url, string postData, string contentTypeHeader, Dictionary<string, string> customHeaders = null);

        HttpResponseInfo SendPut(Uri url, Dictionary<string, string> customHeaders = null);
        
        void UpdateSharedCustomHeader(string xSkypetoken, string token);
    }
}