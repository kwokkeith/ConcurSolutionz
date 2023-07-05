namespace ConcurSolutionz;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Views.RecordPage), typeof(Views.RecordPage));
		Routing.RegisterRoute(nameof(Views.EntryPage), typeof(Views.EntryPage));
	}
}
