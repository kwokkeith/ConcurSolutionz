using System;
using System.Globalization;
using ConcurSolutionz.Database;

namespace Unit_Testing
{
    public class CookieTests
    {
        [Fact]
        public void Build_CookieShouldBuildUsingBuilder()
        {
            // Arrange
            Cookie.CookieBuilder cookieBuilder = new();
            Cookie cookie;

            // Act
            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetBm_sz("abc")
                .SetTAsessionID("123")
                .SetAk_bmsc("def")
                .Set_abck("456")
                .SetOTSESSIONAABQRD("ghi")
                .SetJWT("789")
                .SetBm_sv("jkl")
                .Build();

            // Assert
            DateTime Expected1 = DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected1, cookie.ExpiryDate);

            string Expected2 = "abc";
            Assert.Equal(Expected2, cookie.bm_sz);

            string Expected3 = "123";
            Assert.Equal(Expected3, cookie.TAsessionID);

            string Expected4 = "def";
            Assert.Equal(Expected4, cookie.ak_bmsc);

            string Expected5 = "456";
            Assert.Equal(Expected5, cookie._abck);

            string Expected6 = "ghi";
            Assert.Equal(Expected6, cookie.OTSESSIONAABQRD);

            string Expected7 = "789";
            Assert.Equal(Expected7, cookie.JWT);

            string Expected8 = "jkl";
            Assert.Equal(Expected8, cookie.bm_sv);

        }

        [Fact]
        public void Build_CookieWithNullValues_ThrowsArgumentNullException()
        {
            // Arrange
            Cookie.CookieBuilder cookieBuilder = new();
            Cookie cookie;

            // Assert
            Assert.Throws<ArgumentNullException>(() => cookieBuilder.Build());

        }
    }
}
