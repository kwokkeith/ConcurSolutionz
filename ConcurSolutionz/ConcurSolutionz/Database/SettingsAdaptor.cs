namespace ConcurSolutionz.Database
{
    public static class SettingsAdaptor
    {
        public static dynamic ConvertSettings(Settings settings)
        {
            if (settings.SubType == typeof(Concur).FullName)
            {
                return (Concur)settings;
            }
            else
            {
                throw new ArgumentException(
                    "Invalid Settings subtype detected, " +
                    "could not convert using SettingsSocket!");
            }
        }
    }
}