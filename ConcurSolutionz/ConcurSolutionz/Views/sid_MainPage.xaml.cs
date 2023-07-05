using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FirstApp
{
    public class FileItem
    {
        public string FileName { get; set; }
        public string Icon { get; set; }
        public bool IsFolder { get; set; }
        public DateTime CreationDateTime { get; set; }

        public FileItem(string fileName, bool isFolder)
        {
            FileName = fileName;
            IsFolder = isFolder;
            if (isFolder == true)
            {
                Icon = "file_icon.png";
            }
            else
            {
                Icon = "doc_icon.png";
            }
            CreationDateTime = DateTime.Now;
        }
    }

    public partial class MainPage : ContentPage
    {
        public ObservableCollection<FileItem> Files { get; set; }
        public FileItem SelectedFile { get; set; }
        public SortOption CurrentSortOption { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Files = new ObservableCollection<FileItem>
            {
                new FileItem("Document1.txt", false), // Set icon for non-folder item
                new FileItem("Document2.txt", false), // Set icon for non-folder item
                new FileItem("Folder1", true) // Set icon for folder item
            };
            CurrentSortOption = SortOption.Alphabetical; // Set the default sorting option
            BindingContext = this;
        }

        private async void OnNewFolderClicked(object sender, EventArgs e)
        {
            // Prompt the user for the new folder name
            string newName = await DisplayPromptAsync("New Folder", "Enter a new folder name");

            if (!string.IsNullOrWhiteSpace(newName))
            {
                // Create a new folder item with the entered name
                FileItem newFolder = new FileItem(newName, true);

                // Add the new folder to the collection
                Files.Add(newFolder);

                // Select the new folder
                SelectedFile = newFolder;
            }
        }

        private async void OnNewEntryClicked(object sender, EventArgs e)
        {
            // Prompt the user for the new entry name
            string newName = await DisplayPromptAsync("New Entry", "Enter a new entry name");

            if (!string.IsNullOrWhiteSpace(newName))
            {
                // Create a new entry item with the entered name
                FileItem newEntry = new FileItem(newName, false);

                // Add the new entry to the collection
                Files.Add(newEntry);

                // Select the new entry
                SelectedFile = newEntry;
            }
        }

        private async void OnRenameClicked(object sender, EventArgs e)
        {
            // Handle Rename button click
            if (SelectedFile != null)
            {
                // Prompt for renaming the selected file
                await RenameSelectedFile(SelectedFile.FileName, "Enter a new name");
            }
        }

        private async Task RenameSelectedFile(string initialValue, string promptMessage)
        {
            // Prompt the user for the new name
            string newName = await DisplayPromptAsync("Rename", promptMessage, initialValue: initialValue);

            if (!string.IsNullOrWhiteSpace(newName))
            {
                // Create a new FileItem with the updated file name and other properties
                FileItem renamedFile = new FileItem(newName, SelectedFile.IsFolder);
                renamedFile.CreationDateTime = DateTime.Now;

                // Replace the selected file with the renamed file in the collection
                int selectedIndex = Files.IndexOf(SelectedFile);
                Files[selectedIndex] = renamedFile;

                // Update the SelectedFile property with the renamed file
                SelectedFile = renamedFile;
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            // Handle Delete button click
            if (SelectedFile != null)
            {
                // Get the index of the selected file
                int selectedIndex = Files.IndexOf(SelectedFile);

                // Delete the selected file
                Files.Remove(SelectedFile);

                // Select the previous file if it exists
                if (selectedIndex > 0 && selectedIndex < Files.Count)
                {
                    SelectedFile = Files[selectedIndex];
                    ScrollToSelectedItem();
                }
                else if (selectedIndex > 0 && selectedIndex == Files.Count)
                {
                    // If the last file was deleted, select the previous file
                    SelectedFile = Files[selectedIndex - 1];
                    ScrollToSelectedItem();
                }
                else if (selectedIndex == 0 && Files.Count > 0)
                {
                    // If the first file was deleted, select the first file
                    SelectedFile = Files[0];
                    ScrollToSelectedItem();
                }
                else
                {
                    // If there are no files left, clear the selection
                    SelectedFile = null;
                }
            }
        }

        private async void OnSortClicked(object sender, EventArgs e)
        {
            // Display a dialog box to choose the sorting option
            string action = await DisplayActionSheet("Sort by", "Cancel", null, "By Alphabetical Order", "By Creation Date");

            // Determine the selected sorting option
            if (action == "By Alphabetical Order")
            {
                CurrentSortOption = SortOption.Alphabetical;
            }
            else if (action == "By Creation Date")
            {
                CurrentSortOption = SortOption.Date;
            }
            else
            {
                // Cancelled or no option selected
                return;
            }

            // Sort the files based on the selected option
            SortFiles();

            // Notify the UI that the Files collection has changed
            OnPropertyChanged(nameof(Files));
        }

        private void SortFiles()
        {
            switch (CurrentSortOption)
            {
                case SortOption.Alphabetical:
                    Files = new ObservableCollection<FileItem>(Files.OrderBy(f => f.FileName));
                    break;
                case SortOption.Date:
                    Files = new ObservableCollection<FileItem>(Files.OrderBy(f => f.CreationDateTime));
                    break;
                default:
                    break;
            }
        }

        private void ScrollToSelectedItem()
        {
            if (SelectedFile != null)
            {
                // Scroll the ListView to the selected item
                fileListView.ScrollTo(SelectedFile, ScrollToPosition.MakeVisible, true);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedFile))
            {
                // Update the selected item visual state after selection changes
                fileListView.SelectedItem = SelectedFile;
            }
        }
    }

    public enum SortOption
    {
        Alphabetical,
        Date
    }
}