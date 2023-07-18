
namespace ConcurSolutionz.Views;

using ConcurSolutionz.Database;

public partial class EmptyPage : ContentPage
{
	public EmptyPage()
	{
		InitializeComponent();

		TogglePage();

	}

	private async void TogglePage()
	{
        Concur concur = new Concur();
        Database db = Database.Instance;
        db.SetSetting(concur);
        string rootDir = db.GetSettings().GetRootDirectory();

		if(rootDir is null)
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
