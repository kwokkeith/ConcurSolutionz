namespace ConcurSolutionz.Database
{
    public class FileAdaptor
    {
        /// <summary>Converts a FileDB to its corresponding file subtype.</summary>
        /// <param name="file">FileDB instance to be converted.</param> 
        /// <return>Instance of the Subtype of the FileDB instance.</return>
        /// <exception cref="ArgumentException">Thrown when the file subtype is undetected/incorrect.</exception>
        public static dynamic ConvertFileType( FileDB file )
        {
            if (file.FileType == typeof(Entry).FullName)
            { 
                    return (Entry) file;
            }
            else
            {
                throw new ArgumentException("Invalid File subtype detected, could not convert using FileSocket!");
            }
        }
    }
}