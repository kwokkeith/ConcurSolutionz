using CommunityToolkit.Maui.Storage;
using ConcurSolutionz.Database;

namespace ConcurSolutionz.Views;
public partial class ChooseRootPage : ContentPage
{
	public ChooseRootPage()
	{
		InitializeComponent();
	}

    /// <summary>
    /// Opens up a file picker when the button for choosing root directory is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ChooseRoot_Clicked(object sender, EventArgs e)
    {
        //Display folder picker
        var folder = await FolderPicker.PickAsync(default);
        if (folder.IsSuccessful)
        {
            filePath.Text = folder.Folder.Path;

            // update root directory in database
            Settings settings = new Settings();
            settings.SetRootDirectory(folder.Folder.Path);
            Database.Database.Instance.Setwd(folder.Folder.Path);


            await Shell.Current.GoToAsync(nameof(MainPage));
        }
        else
        {
            return;
        }
        
    }
}
