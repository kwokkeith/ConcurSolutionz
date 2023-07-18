using System.Text.Json;

namespace ConcurSolutionz.Database
{
	public class JSONAdaptor
	{
        public static dynamic GetRecordFromJSON(string json, string recordSubtype)
        {
            switch (recordSubtype)
            {
                case "Receipt":
                    {
                        var options = new JsonSerializerOptions();
                        options.Converters.Add(new Receipt.ReceiptConverter());

                        return JsonSerializer.Deserialize<Receipt>(json, options); ;
                    }
                default:
                    throw new ArgumentException("Invalid record subtype detected, could not convert using JSONAdaptor!");
            }
        }

        public static dynamic GetEntryMetaDataFromJSON(string json, string entryMDSubType)
        {
            switch (entryMDSubType)
            {
                case "StudentProjectClaimMetaData":
                    {
                        var options = new JsonSerializerOptions();
                        options.Converters.Add(new StudentProjectClaimMetaData.StudentProjectClaimMetaDataConverter());

                        return JsonSerializer.Deserialize<StudentProjectClaimMetaData>(json, options); ;
                    }
                default:
                    throw new ArgumentException("Invalid entry metadata subtype detected, could not convert using JSONAdaptor!");
            }
        }
    }
}

