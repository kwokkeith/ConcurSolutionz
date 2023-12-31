using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public static class FileCreator
    {
        /// <summary>Creates the file (Entry or Folder) in the database, including the creation of the files in the OS Directory</summary>
        /// <param name="file">FileDB instance to be created</param>
        public static void CreateFile(FileDB file)
        {
            // Is File == Folder?
            if (Directory.Exists(file.FilePath))
            {
                throw new IOException("File already exists");
            }
            else
            {

                if (file.Folder)
                {
                    CreateFolder(file);
                }
                else
                {
                    CreateEntry(file as Entry);
                }
            }
        }


        /// <summary>Creates a folder in the specified file directory.</summary>
        /// <param name="folder">The FileDB object representing the folder to be created.</param>
        /// <exception cref="Exception">Thrown when the folder could not be created.</exception>
        private static void CreateFolder(FileDB folder)
        {
            if (!Directory.Exists(folder.FilePath))
            {
                Directory.CreateDirectory(folder.FilePath);
                Console.WriteLine(folder.FileName + " Folder Created");
            }
            else
            {
                throw new Exception("Could not create folder " + folder.FileName + " at " + folder.FilePath);
            }
        }


        /// <summary>Creates a directory entry for a file.</summary>
        /// <param name="entry">The file database entry.</param>
        /// <remarks>
        /// If the directory specified in the file database entry does not exist, it will be created.
        /// </remarks>
        private static void CreateEntry(Entry entry)
        {
            if (!Directory.Exists(entry.FilePath))
            {
                // Create base entry folder
                Directory.CreateDirectory(entry.FilePath);
                try
                {
                    // FolderPath for Receipt inside entry
                    string receiptFolderPath = Utilities.ConstRecordsFdrPath(entry.FilePath);
                
                    // FolderPath for Receipt Json folder (Storing all other receipt jsons) inside entry
                    string receiptJSONFolder = Utilities.ConstRecordsMetaDataPath(entry.FilePath);

                    // Not necessary as Creating Receipt MetaData path creates Receipts folder as well
                    //Directory.CreateDirectory(receiptFolderPath);
                    Directory.CreateDirectory(receiptJSONFolder);

                    // Create Entry MetaData
                    string json;

                    string entryMetaDataPath = Utilities.ConstEntryMetaDataPath(entry.FilePath);
                    json = JsonSerializer.Serialize(MDAdaptor.ConvertMetaData(entry.MetaData));
                    
                    File.WriteAllText(entryMetaDataPath, json);
                    PopulateReceiptFolder(entry, receiptFolderPath, receiptJSONFolder);

                    Console.WriteLine(entry.FileName + " Folder Created");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                }
            }
        }


        /// <summary>Populates the receipt folder with the image and metadata of each receipt in an entry.</summary>
        /// <param name="entry">The entry containing the receipts.</param>
        /// <param name="receiptFolderPath">The path to the folder where the receipt images will be copied.</param>
        /// <param name="receiptJSONFolder">The path to the folder where the receipt metadata JSON files will be saved.</param>
        /// <remarks>
        /// This method iterates through each receipt in the entry and performs the following steps:
        /// 1. Copies the receipt image to the specified receipt folder path.
        /// 2. Serializes the receipt object to JSON and saves it as a file in the specified receipt JSON folder path.</remarks>
        public static void PopulateReceiptFolder(Entry entry, string recordFolderPath, string receiptJSONFolder)
        {
            List<string> writtenFiles = new List<string>();

            foreach (Record record in entry.GetRecords())
            {
                // Convert record into receipt (throw an error if any of the records is not of the Receipt SubType)
                Receipt receipt = RecordAdaptor.ConvertRecord(record);

                // Store pictures
                string imgPath = receipt.ImgPath;
                string receiptPath = Path.Combine(recordFolderPath, receipt.RecordID.ToString() + Path.GetExtension(imgPath));   

                // Add receipt images into receipt folder directory
                if (imgPath != receiptPath)
                {
                    CopyFile(imgPath, receiptPath);
                }

                // Update receipt image path to the new location
                receipt.ImgPath = receiptPath;
                writtenFiles.Add(receiptPath); 

                // Store Receipt Metadata
                try
                {
                    // Generate unique metaData filepath name
                    string receiptMetaDataPath = Path.Combine(receiptJSONFolder, receipt.RecordID + ".json");
                    writtenFiles.Add(receiptMetaDataPath);

                    // Serialise record object and write it to the unique metadata location above
                    string json = JsonSerializer.Serialize(receipt);
                    File.WriteAllText(receiptMetaDataPath, json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw e;
                }
            }

            // Delete any files that were not written to
            foreach (string file in Directory.GetFiles(recordFolderPath))
            {
                if (!writtenFiles.Contains(file))
                {
                    File.Delete(file);
                }
            }

            foreach (string file in Directory.GetFiles(receiptJSONFolder))
            {
                if (!writtenFiles.Contains(file))
                {
                    File.Delete(file);
                }
            }
        }


        /// <summary>Copies a file from the source path to the destination path.</summary>
        /// <param name="sourcePath">The path of the file to be copied.</param>
        /// <param name="destinationPath">The path where the file will be copied to.</param>
        /// <remarks>If a file with the same name already exists at the destination path, it will be overwritten.</remarks>
        public static void CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                if (!File.Exists(destinationPath))
                {
                    File.Create(destinationPath).Close();
                }
                File.Copy(sourcePath, destinationPath, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }
    }
}