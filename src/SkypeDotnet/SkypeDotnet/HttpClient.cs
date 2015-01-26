using System;
using System.IO;
using System.Net;

namespace SkypeDotnet
{
    public class HttpClient
    {
        private CookieContainer sessionCookieContainer;

        public HttpClient()
        {
            sessionCookieContainer = new CookieContainer();
        }

        public HttpResponseInfo SendGet(Uri url)
        {
            var request = InitRequest(url);
            var response = (HttpWebResponse)request.GetResponse();
            while (response.StatusCode == HttpStatusCode.Found)
            {
                response.Close();
                request = InitRequest(url);
                response = (HttpWebResponse) request.GetResponse();
            }
            
            var reader = new StreamReader(response.GetResponseStream());
            var result = reader.ReadToEnd();
            reader.Dispose();
            return new HttpResponseInfo()
            {
                ResponseData = result,
                StatusCode = response.StatusCode
            };
        }

        private HttpWebRequest InitRequest(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = sessionCookieContainer;
            request.AllowAutoRedirect = false;
            return request;
        }
    }
}