using System;
using ConcurSolutionz.Database;

namespace Unit_Test
{
    public class SettingsTests
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        [Fact]
        public void A_SetRootDirectory_WritesJsonToFile()
        {
            string settingsfilePath = Path.Combine(userProfile, "Documents", "ConcurSolutionz", "settings.json");

            // Arrange
            if (Directory.Exists("D:/ConcurTests/SettingsTest.fdr"))
            {
                Directory.Delete("D:/ConcurTests/SettingsTest.fdr", true);
            }
            Directory.CreateDirectory("D:/ConcurTests/SettingsTest.fdr");

            if (File.Exists(settingsfilePath))
            { 
                File.Delete(settingsfilePath);
            }

            string path = "D:/ConcurTests/SettingsTest.fdr";
            Settings settings = new();

            // Act
            settings.SetRootDirectory(path);

            // Assert  
            string json = File.ReadAllText(settingsfilePath);
            Assert.Contains(path, json);
        }

        [Fact]
        public void B_GetRootDirectory_ReturnsSavedRootDirectory()
        {
            string settingsfilePath = Path.Combine(userProfile, "Documents", "ConcurSolutionz", "settings.json");

            if (File.Exists(settingsfilePath))
            {
                File.Delete(settingsfilePath);
            }

            // Arrange
            string expectedPath = "D:/ConcurTests/SettingsTest.fdr";
            Settings settings = new Settings();
            settings.SetRootDirectory(expectedPath);

            // Act
            string actualPath = settings.GetRootDirectory();

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Fact]
        public void GetRootDirectory_ReturnsNull_WhenSettingsFileMissing()
        {
            string settingsfilePath = Path.Combine(userProfile, "Documents", "ConcurSolutionz", "settings.json");

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
