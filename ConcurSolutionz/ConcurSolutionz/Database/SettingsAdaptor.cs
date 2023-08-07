namespace ConcurSolutionz.Database
{
    public static class SettingsAdaptor
    {
        /// <summary>Converts a Settings instance to its corresponding subtype.</summary>
        /// <param name="settings">Settings instance to be converted.</param> 
        /// <return>Instance of the Subtype of the Settings instance.</return>
        /// <exception cref="ArgumentException">Thrown when the Settings subtype is undetected/incorrect.</exception>
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