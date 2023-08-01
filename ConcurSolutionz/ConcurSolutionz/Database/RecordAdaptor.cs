using ConcurSolutionz.Models.CustomException;

namespace ConcurSolutionz.Database
{
    public class RecordAdaptor
    {
        public static dynamic ConvertRecord( Record record)
        {
            if (record.SubType == typeof(Receipt).FullName)
            {
                return (Receipt)record;
            }
            else
            { 
                throw new RecordConversionException(
                    "Invalid Record subtype detected, " +
                    "could not convert using RecordSocket!");
            }
        }
    }
}