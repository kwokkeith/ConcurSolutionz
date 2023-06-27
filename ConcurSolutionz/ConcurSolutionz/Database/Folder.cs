
namespace ConcurSolutionz.Database
{
    public class Folder: FileDB
    {
        public string LinkedPath { get; set; }

        // Set mandatory boolean of File Instance
        protected override void SetFolder(){
            this.Folder = true;
        }


        public override void SelectedAction(){
            StepIntoFolder(this.LinkedPath);
            return;
        }

        public void StepIntoFolder(String LinkedPath){
            // TODO: Step into folder
        }

        public void SetLinkedPath(String path){
            this.LinkedPath = path;
            ;
        }

        public String GetLinkedPath(){
            return this.LinkedPath;
        }


        
        
    }
}