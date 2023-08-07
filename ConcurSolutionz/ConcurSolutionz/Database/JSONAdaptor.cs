using System.Text.Json;

namespace ConcurSolutionz.Database
{
	public class JSONAdaptor
	{
        /// <summary>Converts a json to its corresponding Record.</summary>
        /// <param name="json">json string to be converted.</param> 
        /// <param name="recordSubtype">Record Subtype to be converted.</param> 
        /// <return>An instance of `recordSubtype`.</return>
        /// /// <exception cref="ArgumentException">Thrown when the record subtype is undetected/incorrect.</exception>
        public static dynamic GetRecordFromJSON(string json, string recordSubtype)
        {
            if (recordSubtype == typeof(Receipt).FullName)
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new Receipt.ReceiptConverter());

                return JsonSerializer.Deserialize<Receipt>(json, options);
            }
            else
            { 
                throw new ArgumentException("Invalid record subtype detected, could not convert using JSONAdaptor!");
            }
        }


        /// <summary>Converts a json to its corresponding Entry MetaData.</summary>
        /// <param name="json">json string to be converted.</param> 
        /// <param name="entryMDSubType">MetaData Subtype to be converted.</param> 
        /// <return>An instance of `entryMDSubType`.</return>
        /// <exception cref="ArgumentException">Thrown when the file subtype is undetected/incorrect.</exception>
        public static dynamic GetEntryMetaDataFromJSON(string json, string entryMDSubType)
        {
            if (entryMDSubType == typeof(StudentProjectClaimMetaData).FullName)
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new StudentProjectClaimMetaData.StudentProjectClaimMetaDataConverter());

                return JsonSerializer.Deserialize<StudentProjectClaimMetaData>(json, options);
            }
            else
            { 
                throw new ArgumentException("Invalid entry metadata subtype detected, could not convert using JSONAdaptor!");
            }
        }
    }
}

