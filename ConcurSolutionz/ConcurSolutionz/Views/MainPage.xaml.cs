using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using ConcurSolutionz.Database;
using ConcurSolutionz.Models.CustomException;

namespace ConcurSolutionz.Views;
public class FileItem
{
    public string FileName { get; set; }
    public string Icon { get; set; }
    public bool IsFolder { get; set; }
    public DateTime CreationDateTime { get; set; }
    public ObservableCollection<FileItem> Children { get; set; }

    public FileItem(string fileName, bool isFolder)
    {
        IsFolder = isFolder;
        if (isFolder)
        {
            Icon = "folder.png";
            Children = new ObservableCollection<FileItem>();
        }
        else
        {
            Icon = "entry.png";
        }
        FileName = fileName;
        string filePath = Path.Combine(Database.Database.Instance.Getwd(), fileName);
        CreationDateTime = Directory.GetCreationTime(filePath);
    }
}

public partial class MainPage : ContentPage
{
    public ObservableCollection<FileItem> Files { get; set; }
    public FileItem SelectedFile { get; set; }
    public SortOption CurrentSortOption { get; set; }
    public Grid SelectedItem { get; set; }
    public ICommand RefreshCommand { get; set; }
    public ICommand SetRootCommand { get; set; }

    public string SearchText;
    public bool IsRefreshing;

    private string rootDirectoryPath = Database.Database.Instance.GetSettings().GetRootDirectory();
    public string workingDirectoryPath;

    // On Page load
    public MainPage()
    {
        InitializeComponent();
        Files = new ObservableCollection<FileItem>();
        CurrentSortOption = SortOption.Alphabetical; // Set the default sorting option
        BindingContext = this;
        RefreshWorkingDirectoryLabel();

        // Get current working directory from database
        LoadFilesFromDB();

        fileListView.RefreshCommand = new Command(() =>
        {
            fileListView.IsRefreshing = true;
            RefreshPage();
            fileListView.IsRefreshing = false;
        });

    }


    // Run code when the Main Page is navigated to
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Populate File Management View
        RefreshPage();
    }

    private async void BackToChooseRoot(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChooseRootPage));
    }


    // Loads the files using the database's current working directory
    private void LoadFilesFromDB()
    {
        List<string> fileNames = Database.Database.Instance.GetFileNamesFromWD();

        foreach (string fileName in fileNames)
        {
            // Check if file is boolean
            bool isFolder;

            if (fileName.EndsWith(".fdr"))
            {
                isFolder = true;
                // Create FileItem instance
                Files.Add(new FileItem(fileName, isFolder));

            }
            else
            {
                isFolder = false;
                // Create FileItem instance
                Files.Add(new FileItem(fileName, isFolder));

            }
        }
    }


    // update color of the selected grids
    private void UpdateColor(Grid Previous, Grid Current)
    {
        Application.Current.RequestedThemeChanged += (s, a) =>
        {
            Current.Background = Colors.Transparent;
        };
        if (!(Previous is null))
        {
            Previous.Background = Colors.Transparent;
        }

        if (Application.Current.RequestedTheme == AppTheme.Light)
        {
            Current.Background = Colors.LightSkyBlue;
        }
        else
        {
            Current.Background = Colors.SlateBlue;
        }
    }


    private void OnFileTapped(object sender, EventArgs e)
    {
        var tappedFile = (sender as View)?.BindingContext as FileItem;

        // set the color of selected grid
        var previousItem = SelectedItem;
        SelectedItem = (Grid)sender;
        UpdateColor(previousItem, SelectedItem);

        if (tappedFile != null)
        {
            // if file tapped is a folder
            if (tappedFile.IsFolder)
            {
                // Modify selected file
                SelectedFile = tappedFile;
            }
            else
            {
                try
                {
                    // Modify selected file
                    SelectedFile = tappedFile;
                }
                catch
                {
                    DisplayAlert("Failure!", "File failed to open!", "OK");
                }
            }
        }
    }


    private async void OnFileDoubleTapped(object sender, EventArgs e)
    {
        // Handle file/folder double tap event
        var tappedFile = (sender as View)?.BindingContext as FileItem;
        string filePath = Path.Combine(Database.Database.Instance.Getwd(), tappedFile.FileName);
        if (tappedFile != null && tappedFile.IsFolder && tappedFile.Equals(SelectedFile))
        {
            // SELECT file
            Database.Database.Instance.FileSelectByFileName(tappedFile.FileName);

            // Populate File Management View
            RefreshPage();
        }
        else
        {
            // Call entry UI
            try
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(EntryPage)}?" +
                    $"fileName={tappedFile.FileName}&" +
                    $"existingFile={true}&" +
                    $"filePath={filePath}");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SynchronisationException)
                {
                    await DisplayAlert(
                        "Database Synchronising",
                        $"Don't worry :)\nDatabase is syncing...\nPlease try again~\n\n{ex.Message}",
                        "OK"
                        );
                }
                else if (ex.InnerException is MetaDataConversionException)
                {
                    await DisplayAlert(
                        "Failure!",
                        $"Failed to convert MetaData when loading from existing file! Error is: {ex.Message}",
                        "OK"
                        );
                }
                else if (ex.InnerException is RecordConversionException)
                {
                    await DisplayAlert(
                        "Failure!",
                        "Failed to convert record instance to receipt when loading existing file!",
                        "OK"
                        );
                }
                else if (ex.InnerException is MissingEntryFileException)
                {
                    await DisplayAlert(
                        "Database Synchronising!",
                        $"Don't worry :)\nDatabase will syncing...\nPlease try again~\n\n{ex.Message}",
                        "OK"
                        );
                }
                else
                {
                    await DisplayAlert("Warning", $"{ex.Message}", "OK");
                }
                // Refresh page to show valid changes
                RefreshPage();
            }
            // Delay the selection to avoid immediate reselection due to double-tap gesture
            await Task.Delay(200);
        }
    }


    private void OnBackClicked(object sender, EventArgs e)
    {
        Database.Database.Instance.FileGoBack();
        SelectedFile = null;
        RefreshWorkingDirectoryLabel();
        RefreshPage();
    }


    private async void OnNewFolderClicked(object sender, EventArgs e)
    {
        // Prompt the user for the new folder name
        string newName = await DisplayPromptAsync(
            "New Folder",
            "Enter a new folder name"
            );

        if (!string.IsNullOrWhiteSpace(newName))
        {
            try
            {
                // Create a folder using the Database
                Folder.FolderBuilder builder = new Folder.FolderBuilder()
                    .SetCreationDate(DateTime.Now)
                    .SetFileName(newName)
                    .SetFilePath(Database.Database.Instance.Getwd())
                    .SetLastModifiedDate(DateTime.Now);
                Folder folder = builder.Build();
                Database.Database.CreateFile(folder);

                // Repopulate Files
                RefreshPage();
            }
            catch
            {
                await DisplayAlert("Failure!", "File failed to open!", "OK");
            }
        }
    }


    private async void OnNewEntryClicked(object sender, EventArgs e)
    {
        // Prompt the user for the new entry name
        string newName = await DisplayPromptAsync("New Entry", "Enter a new entry name");
        string filePath = Path.Combine(Database.Database.Instance.Getwd(), newName);

        if (!string.IsNullOrWhiteSpace(newName))
        {
            try
            {
                // Call Entry system (UI)
                await Shell.Current.GoToAsync($"{nameof(EntryPage)}?fileName={newName}&existingFile={false}&filePath={filePath}");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure!", "Failed to create new Entry!", "OK");
            }
        }
    }


    private async void OnRenameClicked(object sender, EventArgs e)
    {
        // Handle Rename button click
        if (SelectedFile != null)
        {
            // Prompt for renaming the selected file/folder
            await RenameSelectedFile(Path.GetFileNameWithoutExtension(SelectedFile.FileName), "Enter a new name");
        }
    }

    private async Task RenameSelectedFile(string initialValue, string promptMessage)
    {
        // Prompt the user for the new name
        string newName = await DisplayPromptAsync("Rename", promptMessage, initialValue: initialValue);

        if (!string.IsNullOrWhiteSpace(newName))
        {
            try
            {
                if (newName.Substring(Math.Max(0,newName.Length - 4)) != ".fdr" || newName.Substring(Math.Max(0, newName.Length - 6)) != ".entry")
                {
                    if (SelectedFile.IsFolder)
                    {
                        newName = newName + ".fdr";
                    }
                    else
                    {
                        newName = newName + ".entry";
                    }
                }
                string filePath = Path.Combine(Database.Database.Instance.Getwd(), SelectedFile.FileName);
                string newFilePath = Path.Combine(Database.Database.Instance.Getwd(), newName);


                // Rename the selected file/folder in the target directory
                Directory.Move(filePath, newFilePath);

                // Create a new FileItem with the updated file name and other properties
                FileItem renamedFile = new(newName, SelectedFile.IsFolder)
                {
                    CreationDateTime = DateTime.Now
                };

                // Replace the selected file with the renamed file in the collection
                int selectedIndex = Files.IndexOf(SelectedFile);
                Files[selectedIndex] = renamedFile;

                // Update the SelectedFile property with the renamed file
                SelectedFile = renamedFile;

                // Handle updating of Metadata
                Database.Database.RenameEntry(newFilePath);
            }
            catch (Exception ex)
            {
                // Handle any potential errors
                Console.WriteLine($"Failed to rename file/folder: {ex.Message}");
            }
        }
    }


    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        // Handle Delete button click
        if (SelectedFile != null)
        {
            bool answer = await DisplayAlert("Confirm Deletion", $"Are you sure you want to delete the following {(SelectedFile.IsFolder ? "Folder": "Entry")}: {Path.GetFileNameWithoutExtension(SelectedFile.FileName)}?", "Yes", "No");

            if (answer)
            {
                try
                {
                    Database.Database.DeleteDirectoryByFilePath(Path.Combine(Database.Database.Instance.Getwd(), SelectedFile.FileName));
                    SelectedFile = null;
                    RefreshPage();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failure!", $"Failed to delete file! Error: {ex}", "OK");
                }
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


    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        SearchText = e.NewTextValue;
        if (SearchText == null || SearchText == "" || SearchText == " ")
        {
            RefreshPage();
        }
    }


    private void OnSearchButtonClicked(object sender, EventArgs e)
    {
        // Filter the files based on the search text
        Files = new ObservableCollection<FileItem>(
            Files.Where(f => f.FileName.Contains(SearchText))
            );

        // Refresh list view
        OnPropertyChanged(nameof(Files));
    }


    // <Summary> Functionality to sort the files in the file management page </Summary>
    private void SortFiles()
    {
        switch (CurrentSortOption)
        {
            case SortOption.Alphabetical:
                Files = new ObservableCollection<FileItem>(Files.OrderBy(file => file.FileName));
                break;
            case SortOption.Date:
                Files = new ObservableCollection<FileItem>(Files.OrderByDescending(file => file.CreationDateTime));
                break;
        }
    }

    // <Summary>To refresh the file management page</Summary>
    public void RefreshPage()
    {
        Files.Clear(); // Remove all files in the Files list
        LoadFilesFromDB(); // Reload from current working directory
        RefreshWorkingDirectoryLabel(); // Reload the current working path label
        SelectedFile = null; // deselect selected files
    }


    // <Summary>Updates the working directory label with the current directory</Summary>
    public void RefreshWorkingDirectoryLabel()
    {
        string workingDirectory = Database.Database.Instance.Getwd();
        string rootDirectory = Database.Database.Instance.Settings.GetRootDirectory();

        // to replace the specific text with blank
        string printablePath = workingDirectory.Replace(rootDirectory, "");

        // Drop the '\' or '/' at the front of the string
        if (printablePath.Length > 0)
        {
            printablePath = printablePath.Substring(1);
        }

        printablePath = Path.Combine(".", printablePath);

        WorkingDirectory.Text = printablePath;
    }
}


// UTILITIES
public class RemoveExtensionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string fileName = (string)value;
        return Path.GetFileNameWithoutExtension(fileName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public enum SortOption
{
    Alphabetical,
    Date
}