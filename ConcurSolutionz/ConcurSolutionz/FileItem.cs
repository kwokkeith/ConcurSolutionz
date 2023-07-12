using System;
using System.Collections.ObjectModel;

namespace ConcurSolutionz
{
    public class FileItem
    {
        public string FileName { get; set; }
        public string Icon { get; set; }
        public bool IsFolder { get; set; }
        public DateTime CreationDateTime { get; set; }
        public ObservableCollection<FileItem> Children { get; set; }

        public FileItem(string fileName, bool isFolder)
        {
            FileName = fileName;
            IsFolder = isFolder;
            if (isFolder)
            {
                Icon = "file_icon.png";
                Children = new ObservableCollection<FileItem>();
            }
            else
            {
                Icon = "doc_icon.png";
            }
            CreationDateTime = DateTime.Now;
        }
    }
}

