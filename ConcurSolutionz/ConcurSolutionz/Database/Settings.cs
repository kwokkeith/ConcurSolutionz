
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public class Settings
    {
        private string settingsfilePath;
        private string settingsdirectoryPath;
        public string SubType {get; set;}
        
        public Settings(){
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
        public string GetRootDirectory(){
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
                    if (!File.Exists(rootDirectory.RootDirectory))
                    {
                        Directory.CreateDirectory(rootDirectory.RootDirectory);
                        string rtdir = "D:";
                        Directory.CreateDirectory(Path.Combine(rtdir, "ConcurOCRsystem"));

                    }

                    return rootDirectory.RootDirectory;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return null;
                }
            }

            else
            {
                using (StreamWriter writer = new StreamWriter(settingsfilePath))
                {
                    writer.Write("");
                }
                return null;
            }
            // If the file with root directory does not exist, prompt user to create root directory

            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // TODO: CREATE AN INTERFACE TO PROMPT USER FOR ROOT DIRECTORY
            // Possibly a method to call a simple UI to key in root directory
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        }


        public void SetRootDirectory(string path){
            // Create RootDirectoryObject to be converted to Json
            RootDirectoryData rootDirectory = new()
            {
                RootDirectory = path
            };
            string json = JsonSerializer.Serialize(rootDirectory);
            File.WriteAllText(settingsfilePath, json);
        }

        // Wrapper class for JSON
        class RootDirectoryData{
            public string RootDirectory { get; set; }

        }

        private void SetSettingsPath(){
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
            // Windows OS
            settingsdirectoryPath = Path.Combine(userProfile, "Documents", "ConcurSolutionz");
            settingsfilePath = Path.Combine(userProfile, "Documents", "ConcurSolutionz", "settings.json");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
            // MAC OS
            settingsdirectoryPath = Path.Combine(userProfile, "Library", "ConcurSolutionz");
            settingsfilePath = Path.Combine(userProfile, "Library", "ConcurSolutionz", "settings.json");
            }

            else {
                throw new Exception("OS Unsupported");
            }
        }
    }
}