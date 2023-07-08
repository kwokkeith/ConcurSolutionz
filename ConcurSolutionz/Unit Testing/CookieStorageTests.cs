using System;
using System.Globalization;
using ConcurSolutionz.Database;
using System.Diagnostics;

namespace Unit_Testing
{
    public class CookieStorageTests
    {
        Cookie.CookieBuilder cookieBuilder = new();
        Cookie cookie;


        [Fact]
        public void StoreCookie_StoresValidCookie_AndCookieFileExists()
        {
            // Arrange
            if (Directory.Exists(@"D:\ConcurTests\CookieStorageTest.fdr\File 1.entry"))
            {
                Directory.Delete(@"D:\ConcurTests\CookieStorageTest.fdr\File 1.entry", true);
            }
            Directory.CreateDirectory(@"D:\ConcurTests\CookieStorageTest.fdr\File 1.entry");


            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetBm_sz("abc")
                    .SetTAsessionID("123")
                    .SetAk_bmsc("def")
                    .Set_abck("456")
                    .SetOTSESSIONAABQRD("ghi")
                    .SetJWT("789")
                    .SetBm_sv("jkl")
                    .Build();

            string path = @"D:\ConcurTests\CookieStorageTest.fdr\File 1.entry";
            CookieStorage storage = new CookieStorage { CookieStoragePath = path };

            // Act
            storage.StoreCookie(cookie);

            // Assert
            Assert.True(File.Exists(Path.Combine(path, "cookie.json")));
        }

        [Fact]
        public void RetrieveCookie_StoresAndRetrievesCookie()
        {
            // Arrange
            if (Directory.Exists(@"D:\ConcurTests\CookieStorageTest.fdr\File 2.entry"))
            {
                Directory.Delete(@"D:\ConcurTests\CookieStorageTest.fdr\File 2.entry", true);
            }
            Directory.CreateDirectory(@"D:\ConcurTests\CookieStorageTest.fdr\File 2.entry");


            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetBm_sz("abc")
                    .SetTAsessionID("123")
                    .SetAk_bmsc("def")
                    .Set_abck("456")
                    .SetOTSESSIONAABQRD("ghi")
                    .SetJWT("789")
                    .SetBm_sv("jkl")
                    .Build();

            string path = @"D:\ConcurTests\CookieStorageTest.fdr\File 2.entry";
            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            storage.StoreCookie(cookie);

            // Act 
            Cookie retrievedCookie = storage.RetrieveCookie();

            // Assert
            Assert.True(retrievedCookie != null);
            Assert.Equal(cookie.ExpiryDate, retrievedCookie.ExpiryDate);
            Assert.Equal(cookie.bm_sz, retrievedCookie.bm_sz);
            Assert.Equal(cookie.TAsessionID, retrievedCookie.TAsessionID);
            Assert.Equal(cookie.ak_bmsc, retrievedCookie.ak_bmsc);
            Assert.Equal(cookie._abck, retrievedCookie._abck);
            Assert.Equal(cookie.OTSESSIONAABQRD, retrievedCookie.OTSESSIONAABQRD);
            Assert.Equal(cookie.JWT, retrievedCookie.JWT);
            Assert.Equal(cookie.bm_sv, retrievedCookie.bm_sv);

        }

        [Fact]
        public void RetrieveCookie_ReturnsNull_IfCookieFileDoesNotExist()
        {
            // Arrange
            if (Directory.Exists(@"D:\ConcurTests\CookieStorageTest.fdr\File 3.entry"))
            {
                Directory.Delete(@"D:\ConcurTests\CookieStorageTest.fdr\File 3.entry", true);
            }
            Directory.CreateDirectory(@"D:\ConcurTests\CookieStorageTest.fdr\File 3.entry");

            string path = @"D:\ConcurTests\CookieStorageTest.fdr\File 3.entry";
            CookieStorage storage = new CookieStorage { CookieStoragePath = path };

            // Act
            Cookie retrievedCookie = storage.RetrieveCookie();

            // Assert
            Assert.True(retrievedCookie == null);
        }

        [Fact]
        public void RetrieveCookie_ReturnsNull_IfCookieExpired()
        {
            // Arrange
            if (Directory.Exists(@"D:\ConcurTests\CookieStorageTest.fdr\File 4.entry"))
            {
                Directory.Delete(@"D:\ConcurTests\CookieStorageTest.fdr\File 4.entry", true);
            }
            Directory.CreateDirectory(@"D:\ConcurTests\CookieStorageTest.fdr\File 4.entry");

            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/1994", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetBm_sz("abc")
                .SetTAsessionID("123")
                .SetAk_bmsc("def")
                .Set_abck("456")
                .SetOTSESSIONAABQRD("ghi")
                .SetJWT("789")
                .SetBm_sv("jkl")
                .Build();

            string path = @"D:\ConcurTests\CookieStorageTest.fdr\File 4.entry";
            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            storage.StoreCookie(cookie);

            // Act 
            Cookie retrievedCookie = storage.RetrieveCookie();

            // Assert
            Assert.True(retrievedCookie == null);

        }

        [Fact]
        public void Z_ClearCookies_DeletesCookieFile()
        {
            // Arrange
            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetBm_sz("abc")
                    .SetTAsessionID("123")
                    .SetAk_bmsc("def")
                    .Set_abck("456")
                    .SetOTSESSIONAABQRD("ghi")
                    .SetJWT("789")
                    .SetBm_sv("jkl")
                    .Build();

            string path = @"D:\ConcurTests\CookieStorageTest.fdr\File 4.entry";
            CookieStorage storage = new CookieStorage { CookieStoragePath = path };

            // Act
            storage.ClearCookies();

            // Assert
            Assert.False(File.Exists(Path.Combine(path, "cookie.json")));
        }
    }
}
