using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public sealed class Database
    {   
        public List<string> Files { get; private set; }
        public Settings Settings { get; set; }

        private static Database _instance;
        
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        private Database(){
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
            return Directory.GetCurrentDirectory();
        }

        public void Setwd(string wd){
            try
            {
                Directory.SetCurrentDirectory(wd);
            }
                       catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }

        public List<string> GetFilesFromWD(){
            // Make use of working directory to retrieve files
            string[] folderPaths = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*.fdr");
            string[] entryPaths = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*.entry");
            List<string> files;
            if (folderPaths == null || folderPaths.Length == 0)
            {
                // Working directory has no folders
                if (entryPaths == null || entryPaths.Length == 0)
                {
                    // Working directory has no files
                    files = new List<string>();
                }
                else
                {
                    files = entryPaths.ToList();
                }
            }
            else if (entryPaths == null || entryPaths.Length == 0)
            {
                // Working Directory has no Entry files
                if (folderPaths == null || folderPaths.Length == 0)
                {
                    // No folder as well
                    files = new List<string>();
                }
                else
                {
                    files = folderPaths.ToList();
                }
            }
            else
            {
                // Have both folders and files
                Array.Resize(ref folderPaths, folderPaths.Length + entryPaths.Length);
                Array.Copy(entryPaths, 0, folderPaths, folderPaths.Length - entryPaths.Length, entryPaths.Length);
                files = folderPaths.ToList();
            }

            return files; 
        }


        /// <summary>Method handles selection of a file by its file name.</summary>
        /// <param name="fileName">The name of the file to select.</param>
        /// <exception cref="Exception">Thrown when the specified file name is not found in the Files list of the Database.</exception>
        public void FileSelectByFileName(string fileName){
            Files = GetFilesFromWD();
            string newPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            // If fileName exist in Files
            if (Files.Any(newPath.Contains)){
                FileSelectByFilePath(newPath);
            }
            else{
                throw new Exception(fileName + " not found in Files<List> of Database! "
                + "Ensure that extension is correct" + "\n Files: " + Files);
            }
        }

        
        /// <summary>Method handles selection of a file based on its file path.</summary>
        /// <param name="filePath">The file path of the file to be selected.</param>
        /// <exception cref="Exception">Thrown when the file has an invalid extension.</exception>
        public void FileSelectByFilePath(string filePath){

            if (!Directory.Exists(filePath) && (filePath.EndsWith(".entry") || filePath.EndsWith(".fdr")))
            {
                throw new Exception(filePath + " not found! "
                               + "Check the filename again");
            }


            // Check if File is Folder:
           else if (filePath.EndsWith(".fdr")){
                // If Folder then change workingdirectory path
                Setwd(filePath);
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
                throw new Exception(filePath + "is of invalid extension!");
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
            // Delete a file using Directory (Physical file management system)
            Directory.Delete(filePath);
        }
        
        /// <summary>Navigates back to the previous directory.</summary>
        /// <remarks>
        /// This method updates the working directory by removing the last directory from the path.
        /// If the working directory is already at the root directory, no action is taken.
        /// </remarks>
        public void FileGoBack(){
            // Set working directory one path back
            string rootDirectory = Settings.GetRootDirectory();

            // Check if RootDirectory == WorkingDirectory
            if (Getwd().Equals(rootDirectory))
            {
                return;
            }
            else
            {
                Setwd(Directory.GetParent(Getwd()).FullName);
            }
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
                    string[] ReceiptMetaDatas = Directory.GetFiles(Path.Combine(RecordsMetaDataPath, ""), "*.json");
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