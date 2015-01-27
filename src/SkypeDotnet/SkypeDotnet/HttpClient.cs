using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SkypeDotnet
{
    public class HttpClient : IHttpClient
    {
        private readonly CookieContainer sessionCookieContainer;

        public HttpClient()
        {
            sessionCookieContainer = new CookieContainer();
        }

        public HttpResponseInfo SendGet(Uri url)
        {
            var request = InitRequest(url);
            return InitResponse((HttpWebResponse)request.GetResponse());
        }

        public HttpResponseInfo SendPost(Uri url, Dictionary<string, string> postParameters)
        {

            var request = InitRequest(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var responseData = InitPostParameters(postParameters);
            request.ContentLength = responseData.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(responseData, 0, responseData.Length);
            requestStream.Close();

            return InitResponse((HttpWebResponse) request.GetResponse());
        }

        private byte[] InitPostParameters(Dictionary<string, string> postParameters)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }


            return Encoding.ASCII.GetBytes(postData);
        }

        private HttpResponseInfo InitResponse(HttpWebResponse response)
        {
            HttpWebRequest request;
            while (response.StatusCode == HttpStatusCode.Found)
            {
                response.Close();
                request = InitRequest(new Uri(response.Headers["Location"]));
                response = (HttpWebResponse)request.GetResponse();
            }

            var reader = new StreamReader(response.GetResponseStream());
            var result = reader.ReadToEnd();
            reader.Dispose();
            return new HttpResponseInfo()
            {
                ResponseData = result,
                StatusCode = response.StatusCode,
                ResponseUrl = response.ResponseUri
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