using System;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using ConcurSolutionz.Database;

namespace Unit_Test
{
    public class SettingsSetup : IDisposable
    {
        public SettingsSetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "SettingsTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "SettingsTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "SettingsTest.fdr"));
            System.Threading.Thread.Sleep(100);
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class SettingsTests: IClassFixture<SettingsSetup>
    {
        string settingsfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurSolutionz", "settings.json");
        string settingstestPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "SettingsTest.fdr");

        [Fact(DisplayName = "10.x"), TestPriority(0)]
        public void SetRootDirectory_WritesJsonToFile()
        {
            // Arrange
            if (Directory.Exists(settingstestPath))
            {
                Directory.Delete(settingstestPath, true);
            }
            Directory.CreateDirectory(settingstestPath);

            if (File.Exists(settingsfilePath))
            { 
                File.Delete(settingsfilePath);
            }

            string path = settingstestPath;
            Settings settings = new();

            // Act
            settings.SetRootDirectory(path);

            // Assert  
            string json = File.ReadAllText(settingsfilePath);
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement rootDirectory = doc.RootElement.GetProperty("RootDirectory");
            Assert.Equal(path, rootDirectory.GetString());
        }

        [Fact(DisplayName = "10.x"), TestPriority(1)]
        public void GetRootDirectory_ReturnsSavedRootDirectory()
        {
            if (File.Exists(settingsfilePath))
            {
                File.Delete(settingsfilePath);
            }

            // Arrange
            string expectedPath = settingstestPath;
            Settings settings = new Settings();
            settings.SetRootDirectory(expectedPath);

            // Act
            string actualPath = settings.GetRootDirectory();

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Fact(DisplayName = "10.x"), TestPriority(2)]
        public void GetRootDirectory_ReturnsNull_WhenSettingsFileMissing()
        {
            if (File.Exists(settingsfilePath))
            {
                File.Delete(settingsfilePath);
            }

            Settings settings = new();

            // Act                                   
            string path = settings.GetRootDirectory();

            // Assert
            Assert.Null(path);
        }

    }
}
