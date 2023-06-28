
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