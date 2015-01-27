using System;
using System.Collections.Generic;

namespace SkypeDotnet.Abstract
{
    public interface IHttpClient
    {
        HttpResponseInfo SendGet(Uri url);

        HttpResponseInfo SendPost(Uri url, Dictionary<string, string> postParameters);
    }
}