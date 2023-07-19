using System.Text.Json;

namespace ConcurSolutionz.Database
{
	public class JSONAdaptor
	{
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

