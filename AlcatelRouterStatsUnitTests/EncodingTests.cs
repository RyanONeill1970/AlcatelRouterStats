namespace AlcatelRouterStatsUnitTests
{
    using Xunit;

    public class EncodingTests
    {
        [Theory]
        [InlineData("admin", "dc13ibej?7")]
        [InlineData("1234567890ABCxyz!", "df76ggho4241_[QZ_Ugdfckmklo`n`kfgd")]
        public void Encoded_user_and_password_values_are_correct(string clearText, string expectedEncoding)
        {
            var cipherText = AlcatelRouterStats.Encoder.Encode(clearText);

            Assert.Equal(expectedEncoding, cipherText);
        }

        [Theory]
        [InlineData(84122577, "mf16egno3271_[^Z")]
        [InlineData(64941726, "cf16mgho0251Z[_Z")]
        public void Encoded_token_values_are_correct(int token, string expectedEncoding)
        {
            var cipherText = AlcatelRouterStats.Encoder.Encode(token.ToString());

            Assert.Equal(expectedEncoding, cipherText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Null_and_void_values_return_an_empty_string(string voidValue)
        {
            var cipherText = AlcatelRouterStats.Encoder.Encode(voidValue);

            Assert.Empty(cipherText);
        }
    }
}
