using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Newtonsoft.Json.Linq;

using SkypeDotnet.Abstract;
using SkypeDotnet.Model;

namespace SkypeDotnet
{
    public class HttpClient : IHttpClient
    {
        private readonly CookieContainer sessionCookieContainer;

        public HttpClient()
        {
            sessionCookieContainer = new CookieContainer();
        }

        public HttpResponseInfo SendGet(Uri url, Dictionary<string, string> customHeaders = null)
        {
            var request = InitRequest(url, customHeaders);
            return InitResponse((HttpWebResponse)request.GetResponse());
        }

        public HttpResponseInfo SendPost(Uri url, Dictionary<string, string> postParameters, Dictionary<string, string> customHeaders = null )
        {
            return SendPost(url, InitPostParameters(postParameters), "application/x-www-form-urlencoded", customHeaders);
        }

        public HttpResponseInfo SendPost(Uri url, JObject json, Dictionary<string, string> customHeaders = null )
        {
            return SendPost(url, json.ToString(), "application/json", customHeaders);
        }

        public HttpResponseInfo SendPost(Uri url, string postData, string contentTypeHeader, Dictionary<string, string> customHeaders = null )
        {
            var request = InitRequest(url, customHeaders);
            request.Method = "POST";
            request.ContentType = contentTypeHeader;
            var postBytes = Encoding.Default.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();
            return InitResponse((HttpWebResponse)request.GetResponse());

        }

        private string InitPostParameters(Dictionary<string, string> postParameters)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(postParameters[key]) + "&";
            }


            return postData;
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

        private HttpWebRequest InitRequest(Uri url, Dictionary<string, string> customHeaders = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            
            request.CookieContainer = sessionCookieContainer;
            request.AllowAutoRedirect = false;
            if(customHeaders == null) 
                return request;
            foreach (var customHeader in customHeaders)
            {
                request.Headers.Add(customHeader.Key, customHeader.Value);
            }
            return request;
        }
    }
}