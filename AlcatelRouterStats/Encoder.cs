namespace AlcatelRouterStats
{
    using System;

    public static class Encoder
    {
        private const string cipher = "e5dl12XYVggihggafXWf0f2YSf2Xngd1";

        /// <summary>
        /// Don't use this function in your code, it was translated from browser based javascript to C# and
        /// probably has issues with the safety of the crypotography.
        /// 
        /// The variable names are probably wrong and you should not write your own 'encryption'.
        /// 
        /// This is just used to encode the login values for the Alcatel backend to receive.
        /// </summary>
        /// <param name="clearText">The username or password to encode.</param>
        /// <returns>The encoded user / pass / token to send to the device API.</returns>
        /// <remarks>
        /// 
        /// The original JS code;
        /// 
        ///     a.encrypt = function(e) {
        ///         if ("" == e || void 0 == e)
        ///             return "";
        ///         for (var t = "e5dl12XYVggihggafXWf0f2YSf2Xngd1", n = [], i = "", a = 0; a < e.length; a++) {
        ///             var s = e.charAt(a)
        ///               , r = s.charCodeAt();
        ///             n[2 * a] = 240 & t[a % t.length].charCodeAt() | 15 & r ^ 15 & t[a % t.length].charCodeAt(),
        ///             n[2 * a + 1] = 240 & t[a % t.length].charCodeAt() | r >> 4 ^ 15 & t[a % t.length].charCodeAt()
        ///         }
        ///         for (var a = 0; a < n.length; a++)
        ///             i += String.fromCharCode(n[a]);
        ///         return i
        ///     }
        ///     
        /// </remarks>
        public static string Encode(string clearText)
        {
            if (string.IsNullOrWhiteSpace(clearText))
            {
                return "";
            }

            var n = new int[clearText.Length * 2];
            for (var i = 0; i < clearText.Length; i++)
            {
                var charToEncode = clearText[i];
                var idx = i % cipher.Length;
                var cipherChar = cipher[idx];

                n[2 * i] = 240 & cipherChar | 15 & charToEncode ^ 15 & cipherChar;
                n[2 * i + 1] = 240 & cipherChar | charToEncode >> 4 ^ 15 & cipherChar;
            }

            var encoded = "";

            for (var i = 0; i < n.Length; i++)
            {
                encoded += Convert.ToChar(n[i]);
            }

            return encoded;
        }
    }
}
