namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
	public EntryPage()
	{
		InitializeComponent();

		recordCollection.ItemsSource = GetRecords();
	}

	private async void AddRecord_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(RecordPage));
	}

	private List<Models.Record> GetRecords()
	{
		return new List<Models.Record>
		{
			new Models.Record {RecordName = "Macs", CreationDate="15 June", Amount=100.00},
			new Models.Record {RecordName="Hardware", CreationDate="16 June", Amount=2000 }
		};
	}

	
}
