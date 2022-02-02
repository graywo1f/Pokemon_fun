using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Pokemon_Api.Common
{
   public static class RegexParser
    {
        public static string ParseOffsetFromURL(string URL)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                var regRes = new Regex(@"^.*(\?)(.*offset=)([^#\&\?]*).*", RegexOptions.Compiled).Match(URL);
                if (regRes.Success)
                {
                    return regRes.Groups[3].Value;
                }
                else return null;
            }
            else return null;
        }

        public static string ParseListTypeFromURL(string URL)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                var regRes = new Regex(@"^.*(api\/v2\/)(.*)[\/](.*)[\/].*", RegexOptions.Compiled).Match(URL);
            if (regRes.Success)
            {
                return regRes.Groups[2].Value;
            }
            else return null;
            }
            else return null;
        }

        public static string ParseListIdFromURL(string URL)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                var regRes = new Regex(@"^.*(api\/v2\/)(.*)[\/](.*)[\/].*", RegexOptions.Compiled).Match(URL);
                if (regRes.Success)
                {
                    return regRes.Groups[3].Value;
                }
                else return null;
            }
            else return null;
        }
    }
    
}
