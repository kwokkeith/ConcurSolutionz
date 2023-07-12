using System.Linq.Expressions;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public static class FileCreator
    {
        public static void CreateFile(FileDB file){
            // Is File == Folder?
            try{
                if (file.Folder){
                    CreateFolder(file);
                }
                else{
                    CreateEntry(file as Entry);
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex.ToString());
            }

        }


        /// <summary>Creates a folder in the specified file directory.</summary>
        /// <param name="folder">The FileDB object representing the folder to be created.</param>
        /// <exception cref="Exception">Thrown when the folder could not be created.</exception>
        private static void CreateFolder(FileDB folder){
            if (!Directory.Exists(folder.FilePath)){
                Directory.CreateDirectory(folder.FilePath);
                Console.WriteLine(folder.FileName + " Folder Created");
            }
            else{
                throw new Exception("Could not create folder " + folder.FileName + " at " + folder.FilePath);
            }
        }


        /// <summary>Creates a directory entry for a file.</summary>
        /// <param name="entry">The file database entry.</param>
        /// <remarks>
        /// If the directory specified in the file database entry does not exist, it will be created.
        /// </remarks>
        private static void CreateEntry(Entry entry){
            if (!Directory.Exists(entry.FilePath)){
                // Create base entry folder
                Directory.CreateDirectory(entry.FilePath);

                // FolderPath for Receipt inside entry
                string receiptFolderPath = Utilities.ConstReceiptsFdrPath(entry.FilePath);
                
                // FolderPath for Receipt Json folder (Storing all other receipt jsons) inside entry
                string receiptJSONFolder = Utilities.ConstReceiptMetaDataPath(entry.FilePath);

                // Not necessary as Creating Receipt MetaData path creates Receipts folder as well
//                Directory.CreateDirectory(receiptFolderPath);
                Directory.CreateDirectory(receiptJSONFolder);

                // Create Entry MetaData
                string json;
                try{
                    string entryMetaDataPath = Utilities.ConstEntryMetaDataPath(entry.FilePath);
                    switch (entry.MetaData.SubType){
                        case "StudentProjectClaimMetaData":
                            json = JsonSerializer.Serialize((StudentProjectClaimMetaData)entry.MetaData);
                            break;

                        default:
                            json = JsonSerializer.Serialize(entry.MetaData);
                            break;
                    }
                    
                    File.WriteAllText(entryMetaDataPath, json);
                }
                catch (Exception e){
                    Console.WriteLine("Error: " + e);
                }
                PopulateReceiptFolder(entry, receiptFolderPath, receiptJSONFolder);

                Console.WriteLine(entry.FileName + " Folder Created");
            }
        }


        /// <summary>Populates the receipt folder with the image and metadata of each receipt in an entry.</summary>
        /// <param name="entry">The entry containing the receipts.</param>
        /// <param name="receiptFolderPath">The path to the folder where the receipt images will be copied.</param>
        /// <param name="receiptJSONFolder">The path to the folder where the receipt metadata JSON files will be saved.</param>
        /// <remarks>
        /// This method iterates through each receipt in the entry and performs the following steps:
        /// 1. Copies the receipt image to the specified receipt folder path.
        /// 2. Serializes the receipt object to JSON and saves it as a file in the specified receipt JSON folder path.
        public static void PopulateReceiptFolder(Entry entry, string receiptFolderPath, string receiptJSONFolder){
            foreach( Record record in entry.GetRecords())
                {
                    // Convert record into receipt (throw an error if any of the records is not of the Receipt SubType)
                    Receipt receipt = RecordSocket.ConvertRecord(record);

                    // @@@@@@@@@@@@@@@@@@@@@@
                    // FOR RECEIPT PICTURE 
                    // Store pictures
                    string imgPath = receipt.ImgPath;
                    string receiptPath = Path.Combine(receiptFolderPath, "Receipt " + receipt.RecordID.ToString() + Utilities.GetFileExtension(imgPath));   

                    // Clear all receipt images from receipt folder directory
                    string[] filePaths = Directory.GetFiles(receiptFolderPath);
                    foreach(string filePath in filePaths){
                        File.Delete(filePath);
                    }

                    // Add receipt images into receipt folder directory
                    CopyFile(imgPath, receiptPath);

                    // @@@@@@@@@@@@@@@@@@@@@@
                    // FOR RECEIPT METADATA
                    // Clear Receipt Metadta
                    filePaths = Directory.GetFiles(receiptJSONFolder);
                    foreach(string filePath in filePaths){
                        File.Delete(filePath);
                    }

                    // Store Receipt Metadata
                    try{
                        // Generate unique metaData filepath name
                        string receiptMetaDataPath = Path.Combine(receiptJSONFolder, receipt.RecordID + ".json");       

                        // Serialise record object and write it to the unique metadata location above
                        string json = JsonSerializer.Serialize(receipt);
                        File.WriteAllText(receiptMetaDataPath, json);
                    }
                    catch (Exception e){
                        Console.WriteLine("Error: " + e);
                    }
                }
        }

        /// <summary>Copies files from a source directory to a destination directory.</summary>
        /// <param name="sourcePath">The path of the source directory.</param>
        /// <param name="destinationPath">The path of the destination directory.</param>
        /// <remarks>
        /// This method copies all files from the source directory to the destination directory.
        /// If the destination directory already contains a file with the same name, it will be overwritten.
        /// </remarks>
        private static void CopyFiles(string sourcePath, string destinationPath){
            string[] files = Directory.GetFiles(sourcePath);
            try{
                foreach(string file in files)
                {
                    File.Copy(sourcePath,destinationPath);  
                }
            }
            catch(Exception e){
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>Copies a file from the source path to the destination path.</summary>
        /// <param name="sourcePath">The path of the file to be copied.</param>
        /// <param name="destinationPath">The path where the file will be copied to.</param>
        /// <remarks>If a file with the same name already exists at the destination path, it will be overwritten.</remarks>
        private static void CopyFile(string sourcePath, string destinationPath){
            try{
                if (!File.Exists(destinationPath))
                {
                    File.Create(destinationPath);
                }
                File.Copy(sourcePath, destinationPath, true);
            }
            catch(Exception e){
                Console.WriteLine(e.ToString());
            }
        }
    }
}