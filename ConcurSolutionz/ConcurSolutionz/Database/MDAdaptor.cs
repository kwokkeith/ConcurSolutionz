namespace ConcurSolutionz.Database
{
    public class MDAdaptor
    {
        /// <summary>Converts a MetaData to its corresponding subtype.</summary>
        /// <param name="metaData">MetaData instance to be converted.</param> 
        /// <return>Instance of the Subtype of the MetaData instance.</return>
        /// <exception cref="ArgumentException">Thrown when the MetaData subtype is undetected/incorrect.</exception>
        public static dynamic ConvertMetaData(MetaData metaData)
        {
            if (metaData.SubType == typeof(StudentProjectClaimMetaData).FullName)
            { 
              return (StudentProjectClaimMetaData) metaData;
            }
            else
            { 
                throw new ArgumentException("Invalid MetaData subtype detected, could not convert using MDSocket!");
            }
        }
    }
}