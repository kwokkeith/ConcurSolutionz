using System.Security.AccessControl;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public sealed class Database
    {   
        private string WorkingDirectory { get; set;}
        public List<string> Files { get; private set; }
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

        public List<string> GetFilesFromWD(){
            // Make use of working directory to retrieve files
            string[] folderPaths = Directory.GetFiles(@WorkingDirectory, "*.fdr");
            string[] entryPaths = Directory.GetFiles(@WorkingDirectory, "*.entry");
            
            Array.Resize(ref folderPaths, folderPaths.Length + entryPaths.Length);
            Array.Copy(entryPaths, 0, folderPaths, folderPaths.Length - entryPaths.Length, entryPaths.Length);
            
            List<string> files = folderPaths.ToList();
            return files; 
        }


        public void FileSelectByFileName(string fileName){
            // If fileName exist in Files
            if (Files.Contains(fileName)){
                string newPath = WorkingDirectory + "\\" + fileName;

                FileSelectByFilePath(newPath);
            }
            else{
                throw new Exception(fileName + " not found in Files<List> of Database! "
                + "Perhaps need to update Files<List> of Database?" + "\n Files: " + Files);
            }
        }

        
        public void FileSelectByFilePath(string filePath){
            // Check if File is Folder:
            if(filePath.EndsWith(".fdr")){
                // If Folder then change workingdirectory path
                WorkingDirectory = filePath;
            }

            else if (filePath.EndsWith(".entry")){
                // If Entry then:

                // Construct entry filepath for Entry Subsystem
                // Construct metadata of entry for Entry Subsystem
                // Construct list of receipt metadata
                
                string EntryMetaDataPath = Utilities.ConstEntryMetaDataPath(filePath);

                // Extract Entry MetaData from JSON
                MetaData EntryMetaData = ExtractEntryMetaData(EntryMetaDataPath);
                

                // Extract out receipt from receipt metadata and return a list
                string ReceiptMetaDataPath = Utilities.ConstReceiptMetaDataPath(filePath);
                List<Record> records = ExtractRecords(ReceiptMetaDataPath);

                // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                // TODO: CALL ENTRY SUBSYSTEM
                // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            }

            else{
            throw new Exception(filePath + " found in Files<List> but of invalid extension!");
            }
            
        }

        public static void CreateFile(FileDB file){
            // Call FileCreator class method to createFile
            FileCreator.CreateFile(file);
        }

        public static void DeleteFile(string filePath){
            // Delete a file using FilePath (Physical file management system)
            File.Delete(filePath);
        }


        // @@@@@@@@@@@@@@@@@@@@@@@@@
        // DATABASE UTILITY METHODS
        // @@@@@@@@@@@@@@@@@@@@@@@@@
        private static MetaData ExtractEntryMetaData(string MetaDataPath){
            if (File.Exists(MetaDataPath))
            {
                try
                {
                    string json = File.ReadAllText(MetaDataPath);
                    MetaData metaData = JsonSerializer.Deserialize<MetaData>(json);
                    return metaData;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Entry MetaData Json does not exist.");
                return null;
            }
        }

        private static List<Record> ExtractRecords(string RecordsMetaDataPath){
            if (File.Exists(RecordsMetaDataPath))
            {
                try
                {
                    List<Record> Records = new();
                    string[] ReceiptMetaDatas = Directory.GetFiles(RecordsMetaDataPath + "\\", "*.json");

                    foreach(string fileName in ReceiptMetaDatas){
                        string path = RecordsMetaDataPath + fileName;

                        string json = File.ReadAllText(path);
                        Receipt metaData = JsonSerializer.Deserialize<Receipt>(json);
                        Records.Add(metaData);
                    }

                    return Records;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Entry MetaData Json does not exist.");
                return null;
            }
        }
    }
}