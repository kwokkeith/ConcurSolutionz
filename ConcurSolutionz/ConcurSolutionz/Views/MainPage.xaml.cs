using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
<<<<<<< HEAD
using System.IO;
=======
>>>>>>> AddRecord
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConcurSolutionz.Views;

public class FileItem
{
    public string FileName { get; set; }
    public string Icon { get; set; }
    public bool IsFolder { get; set; }
    public DateTime CreationDateTime { get; set; }
<<<<<<< HEAD
    public ObservableCollection<FileItem> Children { get; set; }
=======
>>>>>>> AddRecord

    public FileItem(string fileName, bool isFolder)
    {
        FileName = fileName;
        IsFolder = isFolder;
<<<<<<< HEAD
        if (isFolder)
        {
            Icon = "file_icon.png";
            Children = new ObservableCollection<FileItem>();
=======
        if (isFolder == true)
        {
            Icon = "file_icon.png";
>>>>>>> AddRecord
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

<<<<<<< HEAD
    private string rootDirectoryPath = "/Users/";
    private string currentDirectoryPath;

    public MainPage()
    {
        InitializeComponent();
        Files = new ObservableCollection<FileItem>();
        CurrentSortOption = SortOption.Alphabetical; // Set the default sorting option
        BindingContext = this;

        LoadFiles(rootDirectoryPath);
    }

    private void LoadFiles(string directoryPath)
    {
        currentDirectoryPath = directoryPath;

        if (Directory.Exists(currentDirectoryPath))
        {
            Files.Clear();

            // Load files and folders from the current directory
            string[] fileEntries = Directory.GetFileSystemEntries(currentDirectoryPath);

            foreach (string entryPath in fileEntries)
            {
                string fileName = Path.GetFileName(entryPath);
                bool isFolder = Directory.Exists(entryPath);
                Files.Add(new FileItem(fileName, isFolder));
            }
        }
    }

    private void OnFileTapped(object sender, EventArgs e)
    {
        var tappedFile = (sender as View)?.BindingContext as FileItem;
        if (tappedFile != null)
        {
            if (tappedFile.IsFolder)
            {
                // Open the tapped folder and display its contents
                string folderPath = Path.Combine(currentDirectoryPath, tappedFile.FileName);
                LoadFiles(folderPath);
                SelectedFile = null;
            }
            else
            {
                // Handle file selection logic
                // You can implement your custom logic here
                DisplayAlert("File Selected", $"You selected the file: {tappedFile.FileName}", "OK");
            }
        }
    }

    private async void OnFileDoubleTapped(object sender, EventArgs e)
    {
        // Handle file/folder double tap event
        var tappedFile = (sender as View)?.BindingContext as FileItem;
        if (tappedFile != null && tappedFile.IsFolder)
        {
            // Open the tapped folder and display its contents
            string folderPath = Path.Combine(currentDirectoryPath, tappedFile.FileName);
            LoadFiles(folderPath);
            SelectedFile = null;

            // Delay the selection to avoid immediate reselection due to double-tap gesture
            await Task.Delay(200);
        }
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(currentDirectoryPath))
        {
            string parentDirectoryPath = Directory.GetParent(currentDirectoryPath)?.FullName;
            if (parentDirectoryPath != null)
            {
                LoadFiles(parentDirectoryPath);
                SelectedFile = null;
            }
        }
=======
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

    private async void OnEntry_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EntryPage));
>>>>>>> AddRecord
    }

    private async void OnNewFolderClicked(object sender, EventArgs e)
    {
        // Prompt the user for the new folder name
        string newName = await DisplayPromptAsync("New Folder", "Enter a new folder name");

        if (!string.IsNullOrWhiteSpace(newName))
        {
<<<<<<< HEAD
            try
            {
                // Create a new folder in the target directory
                string newFolderPath = Path.Combine(currentDirectoryPath, newName);
                Directory.CreateDirectory(newFolderPath);

                // Create a new folder item with the entered name
                FileItem newFolder = new FileItem(newName, true);

                // Add the new folder to the collection
                Files.Add(newFolder);

                // Select the new folder
                SelectedFile = newFolder;
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                Console.WriteLine($"Failed to create folder: {ex.Message}");
            }
=======
            // Create a new folder item with the entered name
            FileItem newFolder = new FileItem(newName, true);

            // Add the new folder to the collection
            Files.Add(newFolder);

            // Select the new folder
            SelectedFile = newFolder;
>>>>>>> AddRecord
        }
    }

    private async void OnNewEntryClicked(object sender, EventArgs e)
    {
        // Prompt the user for the new entry name
        string newName = await DisplayPromptAsync("New Entry", "Enter a new entry name");

        if (!string.IsNullOrWhiteSpace(newName))
        {
<<<<<<< HEAD
            try
            {
                // Create a new file in the target directory
                string newFilePath = Path.Combine(currentDirectoryPath, newName);
                File.Create(newFilePath).Dispose();

                // Create a new entry item with the entered name
                FileItem newEntry = new FileItem(newName, false);

                // Add the new entry to the collection
                Files.Add(newEntry);

                // Select the new entry
                SelectedFile = newEntry;
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                Console.WriteLine($"Failed to create file: {ex.Message}");
            }
=======
            // Create a new entry item with the entered name
            FileItem newEntry = new FileItem(newName, false);

            // Add the new entry to the collection
            Files.Add(newEntry);

            // Select the new entry
            SelectedFile = newEntry;
>>>>>>> AddRecord
        }
    }

    private async void OnRenameClicked(object sender, EventArgs e)
    {
        // Handle Rename button click
        if (SelectedFile != null)
        {
<<<<<<< HEAD
            // Prompt for renaming the selected file/folder
=======
            // Prompt for renaming the selected file
>>>>>>> AddRecord
            await RenameSelectedFile(SelectedFile.FileName, "Enter a new name");
        }
    }

    private async Task RenameSelectedFile(string initialValue, string promptMessage)
    {
        // Prompt the user for the new name
        string newName = await DisplayPromptAsync("Rename", promptMessage, initialValue: initialValue);

        if (!string.IsNullOrWhiteSpace(newName))
        {
<<<<<<< HEAD
            try
            {
                string filePath = Path.Combine(currentDirectoryPath, SelectedFile.FileName);
                string newFilePath = Path.Combine(currentDirectoryPath, newName);

                // Rename the selected file/folder in the target directory
                if (SelectedFile.IsFolder)
                {
                    Directory.Move(filePath, newFilePath);
                }
                else
                {
                    File.Move(filePath, newFilePath);
                }

                // Create a new FileItem with the updated file name and other properties
                FileItem renamedFile = new FileItem(newName, SelectedFile.IsFolder);
                renamedFile.CreationDateTime = DateTime.Now;

                // Replace the selected file with the renamed file in the collection
                int selectedIndex = Files.IndexOf(SelectedFile);
                Files[selectedIndex] = renamedFile;

                // Update the SelectedFile property with the renamed file
                SelectedFile = renamedFile;
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                Console.WriteLine($"Failed to rename file/folder: {ex.Message}");
            }
=======
            // Create a new FileItem with the updated file name and other properties
            FileItem renamedFile = new FileItem(newName, SelectedFile.IsFolder);
            renamedFile.CreationDateTime = DateTime.Now;

            // Replace the selected file with the renamed file in the collection
            int selectedIndex = Files.IndexOf(SelectedFile);
            Files[selectedIndex] = renamedFile;

            // Update the SelectedFile property with the renamed file
            SelectedFile = renamedFile;
>>>>>>> AddRecord
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        // Handle Delete button click
        if (SelectedFile != null)
        {
<<<<<<< HEAD
            try
            {
                string filePath = Path.Combine(currentDirectoryPath, SelectedFile.FileName);

                // Delete the selected file/folder from the target directory
                if (SelectedFile.IsFolder)
                {
                    Directory.Delete(filePath, true);
                }
                else
                {
                    File.Delete(filePath);
                }

                // Get the index of the selected file
                int selectedIndex = Files.IndexOf(SelectedFile);

                // Remove the selected file from the collection
                Files.Remove(SelectedFile);

                // Select the previous file if it exists
                if (selectedIndex > 0 && selectedIndex < Files.Count)
                {
                    SelectedFile = Files[selectedIndex];
                    ScrollToSelectedItem();
                }
                else if (selectedIndex > 0 && selectedIndex == Files.Count)
                {
                    SelectedFile = Files[selectedIndex - 1];
                    ScrollToSelectedItem();
                }
                else if (selectedIndex == 0 && Files.Count > 0)
                {
                    SelectedFile = Files[0];
                    ScrollToSelectedItem();
                }
                else
                {
                    SelectedFile = null;
                }
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                Console.WriteLine($"Failed to deletefile/folder: {ex.Message}");
=======
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
>>>>>>> AddRecord
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
<<<<<<< HEAD
                Files = new ObservableCollection<FileItem>(Files.OrderBy(file => file.FileName));
                break;
            case SortOption.Date:
                Files = new ObservableCollection<FileItem>(Files.OrderByDescending(file => file.CreationDateTime));
=======
                Files = new ObservableCollection<FileItem>(Files.OrderBy(f => f.FileName));
                break;
            case SortOption.Date:
                Files = new ObservableCollection<FileItem>(Files.OrderBy(f => f.CreationDateTime));
                break;
            default:
>>>>>>> AddRecord
                break;
        }
    }

    private void ScrollToSelectedItem()
    {
<<<<<<< HEAD
        if (fileListView.SelectedItem != null)
        {
            // Scroll to the selected item in the ListView
            fileListView.ScrollTo(fileListView.SelectedItem, ScrollToPosition.MakeVisible, animated: true);
=======
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
>>>>>>> AddRecord
        }
    }
}

public enum SortOption
{
    Alphabetical,
    Date
}