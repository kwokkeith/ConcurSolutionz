using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConcurSolutionz.Database
{
    public static class Utilities
    {
        /// <summary>Checks if an argument is null.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="arg">The argument to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public static void CheckNull<T>(T arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
        }


        /// <summary>Checks if an argument is negative.</summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is Negative.</exception>
        public static void CheckIfNegative<T>(T value)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) < 0)
            {
                throw new ArgumentException(value + " cannot be negative!");
            }
        }


        /// <summary>Checks if an argument is numeric.</summary>
        /// <param name="o">An object to be checked.</param>
        /// <return>true if object is a valid numeric type, else false.</return> 
        public static bool IsNumericType(this object o)
        {
            if (o == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>Checks if an argument is an empty string.</summary>
        /// <param name="value">A string to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is an empty string.</exception>
        public static void CheckIfEmptyString(string value)
        {
            if (string.IsNullOrEmpty(value)){
                throw new ArgumentException("Empty string passed!");
            }
        }


        /// <summary>Checks if a string is an valid file name.</summary>
        /// <param name="value">A name to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is an invalid filename.</exception>
        public static void CheckIfValidName(string value)
        {
            CheckIfEmptyString(value);
            if (Regex.IsMatch(value, @"[\\/:*?""<>|]"))
            {
                throw new ArgumentException("Name contains illegal characters");
            }
        }


        /// <summary>Checks if a date is an valid file name.</summary>
        /// <param name="date">A DateTime instance to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when the date is ahead of the current date/time.</exception>
        public static void CheckDateTimeAheadOfNow(DateTime date)
        {
            CheckNull(date);
            int res = DateTime.Compare(date, DateTime.Now);
            if (res > 0) // Date passed is more than current date
            {
                throw new ArgumentException("Date passed is ahead of current date!");
            }
        }


        /// <summary>Checks if a date is ahead of another date.</summary>
        /// <param name="lastModified">A DateTime instance to be checked.</param>
        /// <param name="creation">A DateTime instance to be checked against (cannot pass this date).</param>
        /// <exception cref="ArgumentNullException">Thrown when the `lastmodified` date is ahead of the `creation` date</exception>
        public static void CheckLastModifiedAheadOfCreation(DateTime lastModified, DateTime creation)
        {
            CheckNull(lastModified);
            CheckNull(creation);
            int res = DateTime.Compare(creation, lastModified);
            if (res > 0) // Creation date is more than last modified date
            {
                throw new ArgumentException("Creation date is more than last modified date!");
            }
        }


        /// <summary>Constructs the path to the Entry MetaData folder.</summary>
        /// <param name="entryPath">Path to the Entry Folder.</param>
        /// <return>Path to the Entry MetaData folder.</return> 
        public static string ConstEntryMetaDataPath(string entryPath)
        {
            CheckIfEmptyString(entryPath);
            return Path.Combine(entryPath, "EntryMetaData.json");
        }


        /// <summary>Constructs the path to the Records folder.</summary>
        /// <param name="entryPath">Path to the Entry Folder.</param>
        /// <return>Path to the Records folder.</return> 
        public static string ConstRecordsFdrPath(string entryPath)
        {
            CheckIfEmptyString(entryPath);
            return Path.Combine(entryPath, "Records.fdr");
        }


        /// <summary>Constructs the path to the Records MetaData folder.</summary>
        /// <param name="entryPath">Path to the Entry Folder.</param>
        /// <return>Path to the Records MetaData folder.</return> 
        public static string ConstRecordsMetaDataPath(string entryPath)
        {
            CheckIfEmptyString(entryPath);
            return Path.Combine(entryPath, "Records.fdr", "RecordJSON.fdr");
        }


        /// <summary>Constructs the path to the Image Backup folder (Exist in Root Directory).</summary>
        /// <return>Path to the Image Backup folder.</return> 
        public static string ConstImageBackupPath()
        {
            return Path.Combine(Database.Instance.Settings.GetRootDirectory(), "Image_Backup");
        }
    }
}