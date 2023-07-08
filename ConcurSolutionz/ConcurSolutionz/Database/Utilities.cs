
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Text;
using System;

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


        public static void CheckIfNegative<T>(T value)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(default(T)) < 0)
            {
                throw new ArgumentException(value + " cannot be negative!");
            }
        }

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

        public static void CheckIfEmptyString(string value){
            if (string.IsNullOrEmpty(value)){
                throw new ArgumentException("Empty string passed!");
            }
        }

        public static void CheckDateTimeAheadOfNow(DateTime date){
            CheckNull(date);
            int res = DateTime.Compare(date, DateTime.Now);
            if (res > 0) // Date passed is more than current date
            {
                throw new ArgumentException("Date passed is ahead of current date!");
            }
        }

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

        public static string ConstEntryMetaDataPath(string entryPath){
            return Path.Combine(entryPath, "EntryMetaData.json");
        }

        // Creating Receipt MetaData path creates Receipts folder as well
        public static string ConstReceiptsFdrPath(string entryPath){
            return Path.Combine(entryPath, "Receipts.fdr");
        }

        public static string ConstReceiptMetaDataPath(string entryPath){
            return Path.Combine(entryPath, "Receipts.fdr", "ReceiptJSON.fdr");
        }


    }
}