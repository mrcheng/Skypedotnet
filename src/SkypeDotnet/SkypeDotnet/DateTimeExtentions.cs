using System;

namespace SkypeDotnet
{
    public static class DateTimeExtentions
    {
        public static double ToUnixTimestapm(this DateTime dateTime)
        {
            var epoch = new DateTime(1970,1,1);
            var currentTime = DateTime.Now.ToUniversalTime();

            return (currentTime - epoch).TotalSeconds;
        }
    }
}