using System.Text.Json;
using ConcurSolutionz.Models.CustomException;

namespace ConcurSolutionz.Database
{
    public sealed class Database
    {
        private string WorkingDirectory { get; set; }
        public List<string> Files { get; private set; }
        public Settings Settings { get; set; }

        private static Database _instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        private Database() { }

        public static Database Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }
                return _instance;
            }
        }


        public Settings GetSettings()
        {
            return Settings;
        }


        public void SetSetting(Settings settings)
        {
            Settings = settings;
        }


        public string Getwd()
        {
            return WorkingDirectory;
        }


        public void Setwd(string wd)
        {
            WorkingDirectory = wd;
        }


        public List<string> GetFilePathsFromWD()
        {
            // Make use of working directory to retrieve files
            string[] folderPaths = Directory.GetDirectories(WorkingDirectory, "*.fdr");
            string[] entryPaths = Directory.GetDirectories(WorkingDirectory, "*.entry");
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
                    files = new List<String>();
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


        public List<string> GetFileNamesFromWD()
        {
            List<string> files = GetFilePathsFromWD();
            List<string> fileNames = new List<string>();
            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }
            return fileNames;
        }


        /// <summary>Method handles selection of a file by its file name.</summary>
        /// <param name="fileName">The name of the file to select.</param>
        /// <exception cref="Exception">Thrown when the specified file name is not found in the Files list of the Database.</exception>
        public void FileSelectByFileName(string fileName)
        {
            string newPath = Path.Combine(WorkingDirectory, fileName);

            // If fileName exist in Files
            if (Directory.Exists(newPath))
            {
                FileSelectByFilePath(newPath);
            }
            else
            {
                throw new Exception(fileName + " not found in Files<List> of Database! "
                + "Perhaps need to update Files<List> of Database?" + "\n Files: " + Files);
            }
        }


        /// <summary>Method handles selection of a file based on its file path.</summary>
        /// <param name="filePath">The file path of the file to be selected.</param>
        /// <exception cref="Exception">Thrown when the file has an invalid extension.</exception>
        public void FileSelectByFilePath(string filePath)
        {
            // Check if File is Folder:
            if (filePath.EndsWith(".fdr"))
            {
                // If Folder then change workingdirectory path
                WorkingDirectory = filePath;
            }

            else if (filePath.EndsWith(".entry"))
            {
                // If Entry then, Do Nothing as it is implemented
                return;
            }

            else
            {
                throw new Exception(filePath + " found in Files<List> but of invalid extension!");
            }
        }


        // Uses a fileName to find a file and return a tuple of (Metadata, List<Record>) 
        public Tuple<MetaData, List<Record>> getFileDetailFromFileName(string fileName)
        {
            string filePath = Path.Combine(WorkingDirectory, fileName); // root of file (Entry)
            if (Path.Exists(filePath))
            {
                // Synchronise the Entry to check if user changed any files in OS native directory
                SynchroniseEntry(filePath);

                // Entry metadata path
                string EntryMetaDataPath = Utilities.ConstEntryMetaDataPath(filePath);

                // Extract Entry MetaData 
                MetaData EntryMetaData = ExtractEntryMetaData(EntryMetaDataPath);

                // Extract out receipt from receipt metadata and return a list
                string ReceiptMetaDataPath = Utilities.ConstRecordsMetaDataPath(filePath);

                List<Record> records = ExtractRecords(ReceiptMetaDataPath);

                return new Tuple<MetaData, List<Record>>(EntryMetaData, records);
            }
            else
            {
                throw new ArgumentException("Failed to extract information of file with file name " + fileName + "!" +
                    "\n" + "Deleting all files associated to " + fileName + " to ensure synchronisation!");
            }
        }


        /// <summary>Creates a file in the file management system (Either Entry, Folder Type).</summary>
        /// <param name="file">The file information.</param>
        /// <remarks>
        /// This method delegates the file creation process to the <see cref="FileCreator.CreateFile"/> method.
        /// </remarks>
        public static void CreateFile(FileDB file)
        {
            // Call FileCreator class method to createFile
            FileCreator.CreateFile(file);
        }


        /// <summary>Deletes a file from the specified file path.</summary>
        /// <param name="filePath">The path of the file to be deleted.</param>
        public static void DeleteFileByFilePath(string filePath)
        {
            // Delete a file using Directory (Physical file management system)
            File.Delete(filePath);
        }


        /// <summary>Deletes a file from the specified file path.</summary>
        /// <param name="filePath">The path of the file to be deleted.</param>
        public static void DeleteDirectoryByFilePath(string filePath)
        {
            // Delete a file using Directory (Physical file management system)
            Directory.Delete(filePath, true);
        }


        /// <summary>Renames EntryMetadata when Entry is renamed.</summary>
        /// <remarks>
        /// This method updates the EntryName parameter of the EntryMetaData.json file if the Entry is renamed.
        /// </remarks>
        public static void RenameEntry(string filePath)
        {
            // Get EntryMetaDataPath
            string EntryMetaDataPath = Utilities.ConstEntryMetaDataPath(filePath);

            // Extract Entry MetaData from JSON
            MetaData EntryMetaData = ExtractEntryMetaData(EntryMetaDataPath);

            // Update EntryName parameter of EntryMetaData.json file
            EntryMetaData.EntryName = Path.GetFileNameWithoutExtension(filePath);

            // Write EntryMetaData back to JSON
            File.WriteAllText(EntryMetaDataPath, JsonSerializer.Serialize(MDAdaptor.ConvertMetaData(EntryMetaData)));
        }


        /// <summary>Navigates back to the previous directory.</summary>
        /// <remarks>
        /// This method updates the working directory by removing the last directory from the path.
        /// If the working directory is already at the root directory, no action is taken.
        /// </remarks>
        public void FileGoBack()
        {
            // Set working directory one path back
            string rootDirectory = Settings.GetRootDirectory();

            // Check if RootDirectory == WorkingDirectory
            if (WorkingDirectory.Equals(rootDirectory))
            {
                return;
            }
            else
            {
                WorkingDirectory = System.IO.Directory.GetParent(WorkingDirectory).FullName;
            }
        }


        // **********************************************
        // @@@@@@@@@@ DATABASE UTILITY METHODS @@@@@@@@@@
        // **********************************************
        private static MetaData ExtractEntryMetaData(string MetaDataPath)
        {
            if (Path.Exists(MetaDataPath))
            {
                try
                {
                    string json = File.ReadAllText(MetaDataPath);
                    JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement subtypeElement = doc.RootElement.GetProperty("SubType");
                    string subType = subtypeElement.GetString();

                    MetaData metaData = JSONAdaptor.GetEntryMetaDataFromJSON(json, subType);
                  
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


        private static List<Record> ExtractRecords(string RecordsMetaDataPath)
        {
            if (Directory.Exists(RecordsMetaDataPath))
            {
                try
                {
                    List<Record> Records = new();
                    string[] ReceiptMetaDatas = Directory.GetFiles(Path.Combine(RecordsMetaDataPath, ""), "*.json");
                    foreach (string filePath in ReceiptMetaDatas)
                    {
                        string json = File.ReadAllText(filePath);
                        JsonDocument doc = JsonDocument.Parse(json);
                        JsonElement subtypeElement = doc.RootElement.GetProperty("SubType");
                        string subType = subtypeElement.GetString();


                        Record record_MD = JSONAdaptor.GetRecordFromJSON(json, subType);

                        Records.Add(record_MD);
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


        // <summary> Synchronises Entry if modifications were made to receipt images or metadata in native file directory </summary>
        // <remarks>
        // This method handles the synchronisation of entries if user makes modifciations to the receipt folder or receipt metadata
        // through the native file directory.
        // </remarks>
        private static void SynchroniseEntry(string entryFilePath)
        {
            // Get EntryMetaDataPath
            string EntryMetaDataPath = Utilities.ConstEntryMetaDataPath(entryFilePath);

            // If Entry MetaData missing in native OS File Directory
            if (!Path.Exists(EntryMetaDataPath))
            {
                Directory.Delete(entryFilePath, true);
                throw new MissingEntryFileException("Failed to find metadata file of file with name" + entryFilePath + "!"
                    + "\n" + "Deleting Entry File to ensure Database synchronisation...");
            }

            // Extract out Record from Record metadata and return a list
            string RecordsMetaDataPath = Utilities.ConstRecordsMetaDataPath(entryFilePath);

            // If the Record folder is missing in native OS File Directory
            if (!Path.Exists(RecordsMetaDataPath))
            {
                Directory.Delete(entryFilePath, true);
                throw new MissingEntryFileException("Failed to find record folder of file with name" + entryFilePath + "!"
                    + "\n" + "Deleting Entry File to ensure Database synchronisation...");
            }

            // Check between record metadata and record images
            string[] RecordMetaDatas = Directory.GetFiles(Path.Combine(RecordsMetaDataPath, ""), "*.json");
            for (int i = 0; i < RecordMetaDatas.Length; i++)
            {
                string filePath = RecordMetaDatas[i];
                filePath = Path.GetFileNameWithoutExtension(filePath);
                RecordMetaDatas[i] = filePath;
            }

            // Get Record Folder path
            string RecordFolderPath = Utilities.ConstRecordsFdrPath(entryFilePath);

            // Get all the file in Record Folder
            string[] RecordImages = Directory.GetFiles(RecordFolderPath);
            string[] RecordImagesWithoutFileExt = new string[RecordImages.Length];

            // Remove extension of images
            for (int i = 0; i < RecordImages.Length; i++)
            {
                string filePath = RecordImages[i];
                RecordImagesWithoutFileExt[i] = Path.GetFileNameWithoutExtension(filePath);
            }

            // Find any conflicting files (Missing)
            // Flag to check if there were missing files
            bool missingRecordImage = false;
            bool missingRecordMetaData = false;

            // 1. Finding missing record metadata files
            var diff = RecordImagesWithoutFileExt.Except(RecordMetaDatas);

            // Construct backup path
            string backupPath = Utilities.ConstImageBackupPath();

            // Clear the backup folder
            if (Directory.Exists(backupPath))
            {
                Directory.Delete(backupPath, true);
            }

            // Create backup folder
            Directory.CreateDirectory(backupPath);

            foreach (string fileName in diff)
            {
                // Delete record images missing a metadata
                foreach (string fileNameWithExt in RecordImages)
                {
                    string fileNameWithoutext = Path.GetFileNameWithoutExtension(fileNameWithExt);
                    if (fileNameWithoutext == fileName)
                    {
                        string imageBackupPath = Path.Combine(backupPath, Path.GetFileName(fileNameWithExt));
                        File.Copy(fileNameWithExt, imageBackupPath);
                        File.Delete(fileNameWithExt);
                    }
                }
                missingRecordMetaData = true;
            }

            // 2. Finding missing record image 
            diff = RecordMetaDatas.Except(RecordImagesWithoutFileExt);
            foreach (string fileName in diff)
            {
                // Delete record metadatas that is missing record image
                File.Delete(Path.Combine(RecordsMetaDataPath, fileName + ".json"));
                missingRecordImage = true;
            }

            // Throw error if missing files
            if (missingRecordImage && missingRecordMetaData)
            {
                throw new SynchronisationException("Record Image and Record MetaData desync, " +
                    "database would do the required actions to synchronise...\n" +
                    "Images removed can be found in <root_directory>/Image_Backup.");
            }
            else if (missingRecordMetaData)
            {
                throw new SynchronisationException("Record Metadata desync, " +
                    "database would do the required actions to synchronise...");
            }
            else if (missingRecordImage)
            {
                throw new SynchronisationException("Record Image desync, " +
                    "database would do the required actions to synchronise...\n" +
                    "Images removed can be found in <root_directoy>/Image_Backup.");
            }
        }
    }
}