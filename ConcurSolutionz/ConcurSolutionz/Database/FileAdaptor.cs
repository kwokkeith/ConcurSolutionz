namespace ConcurSolutionz.Database
{
    public class FileAdaptor
    {
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