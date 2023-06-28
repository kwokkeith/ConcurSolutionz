using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CloudKit;

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
                string receiptJSONFolder = Utilities.ConstEntryMetaDataPath(entry.FilePath);

                Directory.CreateDirectory(receiptFolderPath);
                Directory.CreateDirectory(receiptJSONFolder);

                // Create Entry MetaData
                try{
                    string entryMetaDataPath = Utilities.ConstEntryMetaDataPath(entry.FilePath);
                    string json = JsonSerializer.Serialize(entry.MetaData);
                    File.WriteAllText(entryMetaDataPath, json);
                }
                catch (Exception e){
                    Console.WriteLine("Error: " + e);
                }
                

                // Populate Receipt Folder
                foreach( Receipt record in entry.GetRecords().Cast<Receipt>())
                {
                    // Store pictures
                    string imgPath = record.ImgPath;
                    File.Copy(imgPath, receiptFolderPath);

                    // Store Receipt Metadata
                    try{
                        // Generate unique metaData filepath name
                        string receiptMetaDataPath = receiptJSONFolder + "\\"
                                                    + record.GetRecordID() + ".json"; 

                        // Serialise record object and write it to the unique metadata location above
                        string json = JsonSerializer.Serialize(record);
                        File.WriteAllText(receiptMetaDataPath, json);
                    }
                    catch (Exception e){
                        Console.WriteLine("Error: " + e);
                    }
                }
                Console.WriteLine(entry.FileName + " Folder Created");
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

            foreach(string file in files)
            {
                File.Copy(sourcePath,destinationPath);  
            }
        }

        /// <summary>Copies a file from the source path to the destination path.</summary>
        /// <param name="sourcePath">The path of the file to be copied.</param>
        /// <param name="destinationPath">The path where the file will be copied to.</param>
        /// <remarks>If a file with the same name already exists at the destination path, it will be overwritten.</remarks>
        private static void CopyFile(string sourcePath, string destinationPath){
            File.Copy(sourcePath, destinationPath, true);
        }
    }
}