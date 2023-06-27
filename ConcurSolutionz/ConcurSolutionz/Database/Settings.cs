
namespace ConcurSolutionz.Database
{
    public sealed class Settings
    {
        private string rootDirectory = Environment.CurrentDirectory;
        private static readonly Settings instance = new Settings();
        static Settings(){}
        private Settings(){}

        public static Settings getInstance(){
            return instance;
        }
        public String getrootDirectory(){
            return rootDirectory;
        }

        public void setrootDirectory(String path){
            this.rootDirectory = path;
        }


        public static Settings Instance
        {
            get
            {
                return instance;
            }
        }
    }
}