
using ConcurSolutionz.Database;

namespace ConcurSolutionz.Views;
public partial class EmptyPage : ContentPage
{
	public EmptyPage()
	{
		InitializeComponent();
		TogglePage();
	}


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
			db.Setwd(rootDir);
			await Shell.Current.GoToAsync(nameof(MainPage));
		}
	}
}
