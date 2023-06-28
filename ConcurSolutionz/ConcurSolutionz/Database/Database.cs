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


        /// <summary>Method handles selection of a file by its file name.</summary>
        /// <param name="fileName">The name of the file to select.</param>
        /// <exception cref="Exception">Thrown when the specified file name is not found in the Files list of the Database.</exception>
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

        
        /// <summary>Method handles selection of a file based on its file path.</summary>
        /// <param name="filePath">The file path of the file to be selected.</param>
        /// <exception cref="Exception">Thrown when the file has an invalid extension.</exception>
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


        /// <summary>Creates a file in the file management system (Either Entry, Folder Type).</summary>
        /// <param name="file">The file information.</param>
        /// <remarks>
        /// This method delegates the file creation process to the <see cref="FileCreator.CreateFile"/> method.
        /// </remarks>
        public static void CreateFile(FileDB file){
            // Call FileCreator class method to createFile
            FileCreator.CreateFile(file);
        }


        /// <summary>Deletes a file from the specified file path.</summary>
        /// <param name="filePath">The path of the file to be deleted.</param>
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