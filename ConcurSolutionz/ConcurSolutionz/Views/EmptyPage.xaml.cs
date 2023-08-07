using ConcurSolutionz.Database;

namespace ConcurSolutionz.Views;
public partial class EmptyPage : ContentPage
{
	public EmptyPage()
	{
		InitializeComponent();
		TogglePage();
	}

	/// <summary>
	/// If root directory for our system is not yet initialised, allow the user to choose a root directory
	/// else, show the user the file management page immediately
	/// </summary>
	private async void TogglePage()
	{
        Concur concur = new();
        Database.Database db = Database.Database.Instance;
        db.SetSetting(concur);
        string rootDir = db.GetSettings().GetRootDirectory();

		if (rootDir is null)
		{
			await Shell.Current.GoToAsync(nameof(ChooseRootPage));
		}
		else
		{
            Database.Database.Instance.Setwd(rootDir);
			await Shell.Current.GoToAsync(nameof(MainPage));
		}
	}
}
