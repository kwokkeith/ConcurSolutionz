
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
		Settings settings = new Settings();
		var result = settings.GetRootDirectory();

		if(result == null)
		{
			await Shell.Current.GoToAsync(nameof(ChooseRootPage));
		}
		else
		{
			await Shell.Current.GoToAsync(nameof(MainPage));
		}
	}
}
