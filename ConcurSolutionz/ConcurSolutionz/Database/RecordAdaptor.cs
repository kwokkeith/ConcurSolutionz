using ConcurSolutionz.Models.CustomException;

namespace ConcurSolutionz.Database
{
    public class RecordAdaptor
    {
        /// <summary>Converts a Record instance to its corresponding subtype.</summary>
        /// <param name="record">Record instance to be converted.</param> 
        /// <return>Instance of the Subtype of the Record instance.</return>
        /// <exception cref="ArgumentException">Thrown when the MetaData subtype is undetected/incorrect.</exception>
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