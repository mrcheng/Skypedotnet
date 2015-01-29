using System.Net;

namespace SkypeDotnet.Utils
{
    public static class WebRequestExtentions
    {
        public static WebResponse GetResponseNoThrow(this WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    return wex.Response;
                }
                throw;
            }
        }
    }
}