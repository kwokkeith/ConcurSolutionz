
namespace ConcurSolutionz.Database
{
    public sealed class Database
    {   
        private string workingDirectory = Environment.CurrentDirectory;
        private List<FileDB> files = new List<FileDB>();
        private Settings settings;
        private static readonly Database instance = new Database();
        static Database(){}
        private Database(){}


        public static Database getInstance(){
            return instance;
        }

        public Settings getSettings(){
            return settings;
        }

        public void setSetting(Settings settings){
            this.settings = settings;
        }

        public String getwd(){
            return workingDirectory;
        }

        public void setwd(string wd){
            workingDirectory = wd;
        }

        public class FileDB
        {
            internal string filePath;
        }

        public void createFile(){
            FileDB file = new FileDB();
            files.Add(file);
            File.Create(file.filePath);
        }

        public void deleteFile(FileDB file){
            files.Remove(file);
            File.Delete(file.filePath);
        }

        public static Database Instance
        {
            get
            {
                return instance;
            }
        }
    }
}