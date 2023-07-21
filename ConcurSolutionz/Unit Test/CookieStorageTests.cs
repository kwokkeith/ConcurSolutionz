using System;
using System.Globalization;
using ConcurSolutionz.Database;
using System.Diagnostics;

namespace Unit_Test
{
    public class CookieStorageSetup : IDisposable
    {
        public CookieStorageSetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "CookieStorageTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "CookieStorageTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "CookieStorageTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class CookieStorageTests: IClassFixture<CookieStorageSetup>
    {
        Cookie.CookieBuilder cookieBuilder = new();
        Cookie cookie;
        string cookiestoragetestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "CookieStorageTest.fdr");


        [Fact]
        public void StoreCookie_StoresValidCookie_AndCookieFileExists()
        {
            string path = Path.Combine(cookiestoragetestpath, "File 1.entry");

            // Arrange
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);


            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetBm_sz("abc")
                    .SetTAsessionID("123")
                    .SetAk_bmsc("def")
                    .Set_abck("456")
                    .SetOTSESSIONAABQRD("ghi")
                    .SetJWT("789")
                    .SetBm_sv("jkl")
                    .Build();

            CookieStorage storage = new CookieStorage { CookieStoragePath = path };

            // Act
            storage.StoreCookie(cookie);

            // Assert
            Assert.True(File.Exists(Path.Combine(path, "cookie.json")));
        }

        [Fact]
        public void RetrieveCookie_RetrievesValidCookie()
        {
            string path = Path.Combine(cookiestoragetestpath, "File 2.entry");

            // Arrange
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);


            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetBm_sz("abc")
                    .SetTAsessionID("123")
                    .SetAk_bmsc("def")
                    .Set_abck("456")
                    .SetOTSESSIONAABQRD("ghi")
                    .SetJWT("789")
                    .SetBm_sv("jkl")
                    .Build();

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
        public void RetrieveCookie_ReturnsNull_IfCookieFolderDoesNotExist()
        {
            string path = Path.Combine(cookiestoragetestpath, "File 3.entry");

            // Arrange
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            Directory.Delete(path, true);

            // Act
            Cookie retrievedCookie = storage.RetrieveCookie();

            // Assert
            Assert.True(retrievedCookie == null);
        }

        [Fact]
        public void RetrieveCookie_ReturnsNull_IfCookieExpired()
        {
            string path = Path.Combine(cookiestoragetestpath, "File 4.entry");

            // Arrange
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/1994", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetBm_sz("abc")
                .SetTAsessionID("123")
                .SetAk_bmsc("def")
                .Set_abck("456")
                .SetOTSESSIONAABQRD("ghi")
                .SetJWT("789")
                .SetBm_sv("jkl")
                .Build();

            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            storage.StoreCookie(cookie);

            // Act 
            Cookie retrievedCookie = storage.RetrieveCookie();

            // Assert
            Assert.True(retrievedCookie == null);

        }

        [Fact]
        public void RetrieveCookie_ReturnsNull_IfCookieFileDoesNotExist()
        {
            string path = Path.Combine(cookiestoragetestpath, "File 3.entry");

            // Arrange
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            File.Delete(Path.Combine(path, "cookie.json"));

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

            string path = Path.Combine(cookiestoragetestpath, "File 4.entry");
            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            storage.StoreCookie(cookie);

            // Act
            storage.ClearCookies();

            // Assert
            Assert.False(File.Exists(Path.Combine(path, "cookie.json")));
        }
    }
}
