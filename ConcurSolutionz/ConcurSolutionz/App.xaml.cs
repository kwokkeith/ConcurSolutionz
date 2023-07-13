﻿namespace ConcurSolutionz;

using ConcurSolutionz.Database;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		Application.Current.UserAppTheme = AppTheme.Light;

		Concur concur = new Concur();
		Database.Database db = Database.Database.Instance;
		db.SetSetting(concur);
		var result = db.GetSettings().GetRootDirectory();
		

	}
}
