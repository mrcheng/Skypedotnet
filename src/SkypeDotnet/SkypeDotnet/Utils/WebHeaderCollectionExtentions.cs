using System.Collections.Generic;
using System.Net;

namespace SkypeDotnet.Utils
{
    public static class WebHeaderCollectionExtentions
    {
        public static IDictionary<string, string> GetHeaders(this WebHeaderCollection webHeaderCollection)
        {
            var keys = webHeaderCollection.AllKeys;
            var keyVals = new Dictionary<string, string>(keys.Length);
            foreach (string s in keys)
                keyVals.Add(s, webHeaderCollection[s]);
            return keyVals;
        }
    }
}