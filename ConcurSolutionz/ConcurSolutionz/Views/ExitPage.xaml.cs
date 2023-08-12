namespace ConcurSolutionz.Views;

public partial class ExitPage : ContentPage
{
    // On Page load
    public ExitPage()
    {
        InitializeComponent();
    }
    private void CloseApp(object sender, EventArgs e)
    {
        Environment.Exit(0);
    }
}
