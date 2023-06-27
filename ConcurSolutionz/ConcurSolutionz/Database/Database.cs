
namespace ConcurSolutionz.Database
{
    public sealed class Database
    {   
        private string WorkingDirectory { get; set;}
        public List<FileDB> Files { get; private set; }
        public Settings Settings { get; set; }

        private static Database _instance;
        
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        private Database(){
            WorkingDirectory = Environment.CurrentDirectory; // Root directory
        }
        
        public static Database Instance{
            get {
                if (_instance == null){
                    _instance = new Database();
                }
                return _instance;
            }
        }

        public Settings GetSettings(){
            return Settings;
        }

        public void SetSetting(Settings settings){
            this.Settings = settings;
        }

        public string Getwd(){
            return WorkingDirectory;
        }

        public void Setwd(string wd){
            WorkingDirectory = wd;
        }

        public void CreateFile(FileDB file){
            // Append to file local storage
            Files.Add(file);
            
            // Create a file using FilePath (Physical file management system)
            File.Create(file.FilePath);
        }

        public void DeleteFile(FileDB file){
            Files.Remove(file);

            // Delete a file using FilePath (Physical file management system)
            File.Delete(file.FilePath);
        }
    }
}