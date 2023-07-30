using System;
using System.Text.Json;
using ConcurSolutionz.Database;

namespace Unit_Test
{
    public class ConcurSetup : IDisposable
    {
        public ConcurSetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "ConcurTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "ConcurTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "ConcurTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class ConcurTests: IClassFixture<ConcurSetup>
    {
        string concurtestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "ConcurTest.fdr");
        string settingsfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurSolutionz", "settings.json");

        [Fact, TestPriority(0)]
        public void SetRootDirectory_WritesJsonToFile()
        {
            // Arrange
            if (File.Exists(settingsfilePath))
            { 
                File.Delete(settingsfilePath);
            }

            Concur concursettings = new();

            // Act
            concursettings.SetRootDirectory(concurtestpath);

            // Assert  
            string json = File.ReadAllText(settingsfilePath);
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement rootDirectory = doc.RootElement.GetProperty("RootDirectory");
            Assert.Equal(concurtestpath, rootDirectory.GetString());
        }

        [Fact, TestPriority(1)]
        public void GetRootDirectory_ReturnsSavedRootDirectory()
        {
            if (File.Exists(settingsfilePath))
            {
                File.Delete(settingsfilePath);
            }

            // Arrange
            Concur concursettings = new();
            concursettings.SetRootDirectory(concurtestpath);

            // Act
            string actualPath = concursettings.GetRootDirectory();

            // Assert
            Assert.Equal(concurtestpath, actualPath);
        }

        [Fact, TestPriority(2)]
        public void GetRootDirectory_ReturnsNull_WhenSettingsFileMissing()
        {
            if (File.Exists(settingsfilePath))
            {
                File.Delete(settingsfilePath);
            }

            Concur concursettings = new();

            // Act                                   
            string path = concursettings.GetRootDirectory();

            // Assert
            Assert.Null(path);
        }

        [Fact, TestPriority(3)]
        public void SetCookieStoragePath_SetsCookieStoragePath()
        {
            // Arrange
            Concur concursettings = new();
            CookieStorage cookieStorage = new CookieStorage { CookieStoragePath = concurtestpath };

            // Act
            concursettings.SetRootDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurSolutionz"));
            concursettings.CookieStorage = cookieStorage;

            // Assert
            Assert.Equal(cookieStorage, concursettings.CookieStorage);
            Assert.Equal(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurSolutionz", "CookieStorage"), cookieStorage.CookieStoragePath);
        }

    }
}
