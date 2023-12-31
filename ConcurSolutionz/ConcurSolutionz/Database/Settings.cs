using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public class Settings
    {
        private string settingsfilePath;
        private string settingsdirectoryPath;
        public string SubType { get; set; }

        public Settings()
        {
            SetSettingsPath();

            if (!Directory.Exists(settingsdirectoryPath))
            {
                Directory.CreateDirectory(settingsdirectoryPath);
            }

            if (!File.Exists(settingsfilePath))
            {
                File.Create(settingsfilePath).Close();
            }
        }


        /// <summary>Fetches the JSON file from settings path containing the root directory
        /// Returns the root directory of the application.</summary>
        /// <returns>The root directory of the application.</returns>
        public string GetRootDirectory()
        {
            // Get all text from Path
            if (File.Exists(settingsfilePath))
            {
                try
                {
                    string json = File.ReadAllText(settingsfilePath);

                    // Extract JSON properties
                    JsonDocument jsonDocument = JsonDocument.Parse(json);
                    RootDirectoryData rootDirectory = JsonSerializer.Deserialize<RootDirectoryData>(json);

                    // Create root folder if it does not exist
                    if (!Directory.Exists(rootDirectory.RootDirectory))
                    {
                        Directory.CreateDirectory(rootDirectory.RootDirectory);

                    }

                    return rootDirectory.RootDirectory;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return null;
                }
            }

            // Return null if root directory does not exist
            else
            {
                using (StreamWriter writer = new StreamWriter(settingsfilePath))
                {
                    writer.Write("");
                }
                return null;
            }
        }

        /// <summary>Sets the Root Directory of the application.</summary>
        /// <param name="path">Path to be new Root Directory.</param> 
        public void SetRootDirectory(string path)
        {
            // Create RootDirectoryObject to be converted to Json
            RootDirectoryData rootDirectory = new()
            {
                RootDirectory = path
            };

            string json = JsonSerializer.Serialize(rootDirectory);
            File.WriteAllText(settingsfilePath, json);
        }


        // Wrapper class for JSON
        class RootDirectoryData
        {
            public string RootDirectory { get; set; }

        }


        /// <summary>
        /// Sets the current Settings Path of the application.
        /// Creates the json as well as the folder required to store the Settings.
        /// </summary>
        private void SetSettingsPath()
        {
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            settingsdirectoryPath = Path.Combine(userProfile, "Documents", "ConcurSolutionz");
            settingsfilePath = Path.Combine(userProfile, "Documents", "ConcurSolutionz", "settings.json");
        }
    }
}