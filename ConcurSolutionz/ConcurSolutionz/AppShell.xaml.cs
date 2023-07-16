namespace ConcurSolutionz;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(Views.ChooseRootPage), typeof(Views.ChooseRootPage));
        Routing.RegisterRoute(nameof(Views.RecordPage), typeof(Views.RecordPage));
		Routing.RegisterRoute(nameof(Views.EntryPage), typeof(Views.EntryPage));
		Routing.RegisterRoute(nameof(Views.MainPage), typeof(Views.MainPage));

	}
}
