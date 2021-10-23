namespace AlcatelRouterStatsUnitTests
{
    using Xunit;

    public class KeyParserTests
    {
        [Theory]
        [InlineData("_TclRequestVerificationKey=\"KeyVal\"", "_TclRequestVerificationKey", "KeyVal")]
        [InlineData("varname=\"KeyVal\"", "varname", "KeyVal")]
        [InlineData(" varname = \"KeyVal\" ", "varname", "KeyVal")]
        [InlineData("e.http.headers.common._TclRequestVerificationKey=\"KSDHSDFOGQ5WERYTUIQWERTYUISDFG1HJZXCVCXBN2GDSMNDHKVKFsVBNf\"", "_TclRequestVerificationKey", "KSDHSDFOGQ5WERYTUIQWERTYUISDFG1HJZXCVCXBN2GDSMNDHKVKFsVBNf")]
        public void Keys_can_be_extracted(string libText, string varName, string expectedValue)
        {
            var key = AlcatelRouterStats.SecretsDownloader.ParseQuotedVariable(libText, varName);

            Assert.Equal(expectedValue, key);
        }
    }
}
