
namespace ConcurSolutionz.Database
{
    public static class Utilities
    {
        /// <summary>Checks if an argument is null.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="arg">The argument to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public static void CheckNull<T>(T arg){
            if (arg == null){
                throw new ArgumentNullException(nameof(arg));
            }
        }

        public static string ConstEntryMetaDataPath(string entryPath){
            return entryPath + "\\" + "EntryMetaData.json";
        }

        public static string ConstReceiptsFdrPath(string entryPath){
            return entryPath + "\\Receipts";
        }

        public static string ConstReceiptMetaDataPath(string entryPath){
            return entryPath + "\\Receipts\\ReceiptJSON";
        }
    }
}