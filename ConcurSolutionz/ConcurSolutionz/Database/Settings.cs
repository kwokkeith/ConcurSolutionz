
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public class Settings
    {
        private static readonly string WindowSettingsPath = "%userprofile%\\documents\\ConcurSolutionz\\Settings";
        private static readonly string MacSettingsPath = "$HOME\\Library\\ConcurSolutionz\\Settings";
        private string settingsPath;

        public Settings(){
            SetSettingsPath();
        }

        /// <summary>Fetches the JSON file from settings path containing the root directory
        /// Returns the root directory of the application.</summary>
        /// <returns>The root directory of the application.</returns>
        public string GetRootDirectory(){
            // Get all text from Path
            if (File.Exists(settingsPath))
            {
                try{
                    string json = File.ReadAllText(settingsPath); 
                
                    // Extract JSON properties
                    JsonDocument jsonDocument = JsonDocument.Parse(json);
                    RootDirectoryData rootDirectory = JsonSerializer.Deserialize<RootDirectoryData>(json);

                    return rootDirectory.RootDirectory;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return null;
                }
            }

            // If the file with root directory does not exist, prompt user to create root directory

            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // TODO: CREATE AN INTERFACE TO PROMPT USER FOR ROOT DIRECTORY
            // Possibly a method to call a simple UI to key in root directory
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            return "-1";
        }


        public void SetRootDirectory(string path){
            // Create RootDirectoryObject to be converted to Json
            RootDirectoryData rootDirectory = new()
            {
                RootDirectory = path
            };
            string json = JsonSerializer.Serialize(rootDirectory);
            File.WriteAllText(settingsPath, json);
        }

        // Wrapper class for JSON
        class RootDirectoryData{
            public string RootDirectory { get; set; }

        }

        private void SetSettingsPath(){
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
            // Windows OS
            settingsPath = WindowSettingsPath;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
            // MAC OS
            settingsPath = MacSettingsPath;
            }
            else {
                throw new Exception("OS Unsupported");
            }
        }
    }
}