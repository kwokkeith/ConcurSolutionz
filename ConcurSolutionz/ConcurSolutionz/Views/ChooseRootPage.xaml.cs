using CommunityToolkit.Maui.Storage;

namespace ConcurSolutionz.Views;

public partial class ChooseRootPage : ContentPage
{
	public ChooseRootPage()
	{
		InitializeComponent();
	}

    //User choose their root directory
    private async void ChooseRoot_Clicked(object sender, EventArgs e)
    {
        //Display folder picker
        var folder = await FolderPicker.PickAsync(default);
        //filePath.Text = folder.Folder.Path;

        // update root directory in database
        Database.Settings settings = new();
        settings.SetRootDirectory("hi");

        filePath.Text = settings.GetRootDirectory();

        //await Shell.Current.GoToAsync(nameof(MainPage));
    }
}
