namespace AlcatelRouterStats
{
    using System.Net;

    public class ExtractedSecrets
    {
        /// <summary>
        /// Extracted from the js lib at load and used for generation of API secrets.
        /// Looks something like the following when in the file;
        ///     i.e.cookie("t", "1D4347205720C3A64AD974ABCDE10498B" + i.g.encrypt(String(t.result.token))),
        /// Extracted, looks like;
        ///     "1D4347205720C3A64AD974ABCDE10498B"
        /// </summary>
        public string CookiePrefix { get; set; }

        /// <summary>
        /// Extracted from the js lib at load and used when calling the API.
        /// 
        /// Looks something like the following (I've obfuscated a little) when in the file;
        ///     e.http.headers.common._TclRequestVerificationKey = "KSDHSEYBCQ5WERYTUIQWERTYUISDFG1HJBWAZVCXBRUBQPMNDHKVKFsVBNf",
        /// Extracted, looks like;
        ///     "KSDHSEYBCQ5WERYTUIQWERTYUISDFG1HJBWAZVCXBRUBQPMNDHKVKFsVBNf"
        /// 
        /// This seems dodgy because the string on my router has both QWERTY and ZXCV in them (two runs of keys on a standard QWERTY keyboard).
        /// Looks like a lazy coder didn't finish the job and wrote their own homebrew 'encryption'. Or perhaps they were unpaid and Alcatel
        /// got what they paid for.
        /// </summary>
        public string RequestVerificationKey { get; set; }
    }

    public static class SecretsDownloader
    {
        private const string CookiePrefixText = "i.e.cookie(\"t\",";
        private const string RequestVerificationKeyVariableName = "_TclRequestVerificationKey=";

        internal static ExtractedSecrets DownloadAndParseKey(string ipAddress)
        {
            var jsContent = DownloadJavascript(ipAddress);

            return new ExtractedSecrets
            {
                CookiePrefix = ParseQuotedVariable(jsContent, CookiePrefixText),
                RequestVerificationKey = ParseQuotedVariable(jsContent, RequestVerificationKeyVariableName),
            };

        }

        /// <summary>
        /// Extracts values from the javascript library contents.
        /// Used for extracting the request verification key and seed cookie values for API requests.
        /// 
        /// The string is expected to be of the format;
        ///  e.http.headers.common._TclRequestVerificationKey = "KEY GOES HERE",
        ///     
        /// </summary>
        /// <param name="jsContent">The javascript library downloaded from the router which has the values embedded in it.</param>
        /// <returns>The value, or an empty string if not found.</returns>
        /// <remarks>
        /// I loathe regex as it is a security risk, so we don't use it here.
        /// Just plain string parsing and unit tests.
        /// </remarks>
        internal static string ParseQuotedVariable(string jsContent, string prefixTextToFind)
        {
            if (string.IsNullOrEmpty(jsContent))
            {
                return "";
            }

            // Find the variable name, as in _TclRequestVerificationKey
            var startOfVarName = jsContent.IndexOf(prefixTextToFind);
            if (startOfVarName == -1)
            {
                return "";
            }

            // Move past the size of the variable string we just found.
            var endOfVarName = startOfVarName + prefixTextToFind.Length - 1;

            // Find the first quote character which delimits the start of the value.
            var firstQuote = jsContent.IndexOf("\"", endOfVarName + 1);
            if (firstQuote == -1)
            {
                return "";
            }

            // Finally the last quote character which delimits the end of the value.
            var lastQuote = jsContent.IndexOf("\"", firstQuote + 1);
            if (lastQuote == -1)
            {
                return "";
            }

            // We now have the token in between the quote positions.
            return jsContent.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
        }

        /// <summary>
        /// Reads the javascript file which runs most of the router front end.
        /// We need this file because it has the request verification key and cookie embedded in it which is required for some API calls.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal static string DownloadJavascript(string ipAddress)
        {
            var client = new WebClient();
            var url = $"http://{ipAddress}/dist/build.js";

            return client.DownloadString(url);
        }
    }
}
