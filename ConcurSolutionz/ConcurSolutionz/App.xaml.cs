namespace ConcurSolutionz;

using ConcurSolutionz.Database;

public partial class App : Application
{
    private static string appGuid = "6da3476b-d5ad-4b27-bdfc-37c30ab440f5";
    private static Mutex mutex = new Mutex(true, appGuid);

    public App()
    {

        if (mutex.WaitOne(TimeSpan.Zero, true))
        {
            InitializeComponent();
            MainPage = new AppShell();
            Concur concur = new Concur();
            Database.Database db = Database.Database.Instance;
            db.SetSetting(concur);
            var result = db.GetSettings().GetRootDirectory();

        } else {

            InitializeComponent();
            MainPage = new Views.ExitPage();

        }


    }
}
