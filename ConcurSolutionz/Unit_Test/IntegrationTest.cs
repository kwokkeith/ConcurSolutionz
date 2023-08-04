using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using ConcurSolutionz.Database;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace Unit_Test
{
    public class IntegrationSetup : IDisposable
    {
        public IntegrationSetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");
            string picturepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", "IMG_1000.png");

            if (!File.Exists(picturepath))
            {
                // Create default test image if IMG_1000.png does not exist
                using (FileStream fs = File.Create(picturepath))
                {
                    byte[] imageBytes = new byte[100];
                    fs.Write(imageBytes, 0, imageBytes.Length);
                }
            }

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "IntegrationTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "IntegrationTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "IntegrationTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class IntegrationTest: IClassFixture<IntegrationSetup>
    {
        string inttestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "IntegrationTest.fdr");
        string picturepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", "IMG_1000.png");

        [Fact, TestPriority(0)]
        public void PreTest_TestsAllUtilities()
        {   // 1.1
            string? StringValue = null;
            Assert.Throws<ArgumentNullException>(() => Utilities.CheckNull(StringValue));

            // 1.2
            StringValue = "Hi!";
            var exception = Xunit.Record.Exception(() => Utilities.CheckNull(StringValue));
            Assert.Null(exception);

            // 1.3
            int? IntValue = 324;
            Assert.True(Utilities.IsNumericType(IntValue));
            IntValue = 232242;
            Assert.True(Utilities.IsNumericType(IntValue));

            // 1.3
            StringValue = "324.43";
            decimal? DecimalValue = Convert.ToDecimal(StringValue);
            Assert.True(Utilities.IsNumericType(DecimalValue));
            StringValue = "423532.42";
            DecimalValue = Convert.ToDecimal(StringValue);
            Assert.True(Utilities.IsNumericType(DecimalValue));

            // 1.3
            double? DoubleValue = 324.43;
            Assert.True(Utilities.IsNumericType(DoubleValue));
            DoubleValue = 423532.42;
            Assert.True(Utilities.IsNumericType(DoubleValue));

            // 1.4
            StringValue = "HiEveryone"; Assert.False(Utilities.IsNumericType(StringValue));
            Assert.False(Utilities.IsNumericType(StringValue));

            // 1.4
            bool BoolValue = true;
            Assert.False(Utilities.IsNumericType(BoolValue));

            // 1.5
            DoubleValue = null;
            IntValue = null;
            DecimalValue = null;
            Assert.False(Utilities.IsNumericType(DoubleValue));
            Assert.False(Utilities.IsNumericType(IntValue));
            Assert.False(Utilities.IsNumericType(DecimalValue));

            // 1.6
            decimal Decimalvalue = -1231231.23m;
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfNegative(Decimalvalue));

            // 1.7
            Decimalvalue = 1231231.23m;
            exception = Xunit.Record.Exception(() => Utilities.CheckNull(Decimalvalue));
            Assert.Null(exception);

            // 1.8
            StringValue = "";
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfEmptyString(StringValue));

            // 1.9
            StringValue = null;
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfEmptyString(StringValue));

            // 1.10
            StringValue = "Hi!";
            exception = Xunit.Record.Exception(() => Utilities.CheckIfEmptyString(StringValue));
            Assert.Null(exception);

            // 1.11
            DateTime DateTimeValue = DateTime.Now.AddDays(365);
            Assert.Throws<ArgumentException>(() => Utilities.CheckDateTimeAheadOfNow(DateTimeValue));

            // 1.12
            DateTimeValue = DateTime.Now.AddDays(-365);
            exception = Xunit.Record.Exception(() => Utilities.CheckDateTimeAheadOfNow(DateTimeValue));
            Assert.Null(exception);

            // 1.13
            DateTimeValue = DateTime.Now;
            exception = Xunit.Record.Exception(() => Utilities.CheckDateTimeAheadOfNow(DateTimeValue));
            Assert.Null(exception);

            // 1.14
            DateTime lastModified = new(2023, 01, 23, 0, 0, 0);
            DateTime creation = new(2022, 12, 23, 0, 0, 0);
            Assert.Throws<ArgumentException>(() => Utilities.CheckLastModifiedAheadOfCreation(creation, lastModified));

            // 1.15
            lastModified = new(2022, 12, 23, 0, 0, 0);
            creation = new(2023, 01, 23, 0, 0, 0);
            exception = Xunit.Record.Exception(() => Utilities.CheckLastModifiedAheadOfCreation(creation, lastModified));
            Assert.Null(exception);

            // 1.16
            lastModified = new(2023, 01, 23, 0, 0, 0);
            creation = new(2023, 01, 23, 0, 0, 0);
            exception = Xunit.Record.Exception(() => Utilities.CheckLastModifiedAheadOfCreation(creation, lastModified));
            Assert.Null(exception);

            // 1.17
            string path = "Capstone 2023";
            string entryPath = Path.Combine(path, "EntryMetaData.json");
            Assert.Equal(entryPath, Utilities.ConstEntryMetaDataPath(path));
            path = "HiHongJingTheOneAndOnly";
            entryPath = Path.Combine(path, "EntryMetaData.json");
            Assert.Equal(entryPath, Utilities.ConstEntryMetaDataPath(path));

            // 1.18
            entryPath = "";
            Assert.Throws<ArgumentException>(() => Utilities.ConstEntryMetaDataPath(entryPath));

            // 1.19
            path = "Capstone 2023";
            entryPath = Path.Combine(path, "Records.fdr");
            Assert.Equal(entryPath, Utilities.ConstRecordsFdrPath(path));
            path = "HiAkashJefferson";
            entryPath = Path.Combine(path, "Records.fdr");
            Assert.Equal(entryPath, Utilities.ConstRecordsFdrPath(path));


            // 1.20
            entryPath = "";
            Assert.Throws<ArgumentException>(() => Utilities.ConstRecordsFdrPath(entryPath));

            // 1.21
            path = "Capstone 2023";
            entryPath = Path.Combine(path, "Records.fdr", "RecordJSON.fdr");
            Assert.Equal(entryPath, Utilities.ConstRecordsMetaDataPath(path));
            path = "HiShaunTheJack";
            entryPath = Path.Combine(path, "Records.fdr", "RecordJSON.fdr");
            Assert.Equal(entryPath, Utilities.ConstRecordsMetaDataPath(path));

            // 1.22
            entryPath = "";
            Assert.Throws<ArgumentException>(() => Utilities.ConstRecordsMetaDataPath(entryPath));
        }


        [Fact, TestPriority(1)]
        public void AppInitialization_TestsDatabase_Concur()
        {
            Thread.Sleep(400);
            string settingsfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurSolutionz", "settings.json");

            if (File.Exists(settingsfilePath))
            {
                File.Delete(settingsfilePath);
            }

            // Check for exisiting settings file and tries to get the root directory from it
            Concur concursettings = new();
            string rootDir = concursettings.GetRootDirectory();
            Assert.Null(rootDir); // rootdir should be null if settings file does not exist

            // Instantiate Database
            Database dbinstance = Database.Instance;
            rootDir = inttestpath;
            dbinstance.SetSetting(concursettings);
            ;

            // User sets the root directory
            dbinstance.GetSettings().SetRootDirectory(rootDir);
            dbinstance.Setwd(rootDir);

            // Check that rootDir is set correctly and working directory is set correctly
            Assert.True(dbinstance.GetSettings().GetRootDirectory() == rootDir);
            Assert.True(dbinstance.Getwd() == rootDir);
        }

        [Fact, TestPriority(2)]
        public void CreatingConcurSettings_TestsConcur_Database_CookieStorage()
        {
            // Initialize Settings and cookie storage
            Concur concursettings = new();
            concursettings.SetRootDirectory(inttestpath);
            CookieStorage cookieStorage = new();
            concursettings.CookieStorage = cookieStorage;

            // Set the cookie storage path
            string cookiePath = Directory.CreateDirectory(Path.Combine(inttestpath, "CookiePath")).ToString();
            concursettings.CookieStorage.CookieStoragePath = cookiePath;

            // Set settings in database
            Database dbinstance = Database.Instance;
            dbinstance.SetSetting(concursettings);

            // Check that settings are set correctly
            Assert.True(dbinstance.GetSettings().GetRootDirectory() == inttestpath);
            Concur retrievedSettings = (Concur)dbinstance.GetSettings();
            Assert.True(retrievedSettings.CookieStorage.CookieStoragePath == cookiePath);
        }

        [Fact, TestPriority(3)]
        public void PopulateFileManagement_TestsDatabase()
        {
            // Create some entries and folders in the root directory
            Directory.CreateDirectory(Path.Combine(inttestpath, "Folder 1.fdr", "Subfolder 1.fdr"));
            Directory.CreateDirectory(Path.Combine(inttestpath, "Folder 1.fdr", "Subfolder 2.fdr"));
            Directory.CreateDirectory(Path.Combine(inttestpath, "Entry 1.entry"));

            // Get list of files in working directory
            Database dbinstance = Database.Instance;
            List<string> files = dbinstance.GetFileNamesFromWD();

            // Check that it returns the correct number of files in working directory (Folder 1.fdr, Entry 1.entry)
            Assert.True(files.Count == 2); // Should not have subfolders as they are not in the working directory
            Assert.Contains("Folder 1.fdr", files);
            Assert.Contains("Entry 1.entry", files);
        }

        [Fact, TestPriority(4)]
        public void NavigatingFileManagement_TestsDatabase()
        {
            // Navigate to Folder 1 using filename
            Database dbinstance = Database.Instance;
            dbinstance.FileSelectByFileName("Folder 1.fdr");

            // Check that working directory is set correctly and that it returns the correct number of files in working directory (Subfolder 1.fdr, Subfolder 2.fdr)
            Assert.True(dbinstance.Getwd() == Path.Combine(inttestpath, "Folder 1.fdr"));
            Assert.True(dbinstance.GetFileNamesFromWD().Count == 2); // Should have 2 subfolders

            // Navigate to Subfolder 1 using filepath
            dbinstance.FileSelectByFilePath(Path.Combine(inttestpath, "Folder 1.fdr", "Subfolder 1.fdr"));

            // Check that working directory is set correctly and that it returns the 0 files in working directory
            Assert.True(dbinstance.Getwd() == Path.Combine(inttestpath, "Folder 1.fdr", "Subfolder 1.fdr"));
            Assert.True(dbinstance.GetFileNamesFromWD().Count == 0); // Should have no subfolders
        }

        [Fact, TestPriority(5)]
        public void GoingBackInFileManagement_TestsDatabase()
        {
            // Go back to Folder 1
            Database dbinstance = Database.Instance;
            dbinstance.FileGoBack();

            Assert.True(dbinstance.Getwd() == Path.Combine(inttestpath, "Folder 1.fdr"));
        }

        [Fact, TestPriority(6)]
        public void AddFolderinFileManagement_TestsDatabase_FolderBuilder()
        {
            Database dbinstance = Database.Instance;
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Create another subfolder inside Folder 1
            folder = folderBuilder.SetFileName("SubFolder 3")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(dbinstance.Getwd())
                .Build();
            Database.CreateFile(folder);

            // Check that the subfolder is created
            Assert.True(Directory.Exists(Path.Combine(inttestpath, "Folder 1.fdr", "SubFolder 3.fdr")));
        }

        [Fact, TestPriority(7)]
        public void DeleteFileFromFileManagement_TestsDatabase()
        {
            Database dbinstance = Database.Instance;

            // Delete Subfolder 3
            Database.DeleteDirectoryByFilePath(Path.Combine(inttestpath, "Folder 1.fdr", "SubFolder 3.fdr"));

            // Check that Subfolder 3 is deleted
            Assert.False(Directory.Exists(Path.Combine(inttestpath, "Folder 1.fdr", "SubFolder 3.fdr")));
        }

        [Fact, TestPriority(8)]
        public void CreateNewEntry_TestsDatabase_MDBuilder_EntryBuilder_Entry()
        {
            Database dbinstance = Database.Instance;
            dbinstance.Setwd(inttestpath);

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();


            // Build receipts and entry

            md = studentProjMDBuilder
                .SetEntryName("Entry 2")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 2")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(inttestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            // Check that the entry is created
            Assert.True(Directory.Exists(Path.Combine(inttestpath, "Entry 2.entry")));
            Assert.True(File.Exists(Path.Combine(inttestpath, "Entry 2.entry", "EntryMetaData.json")));
            Assert.True(Directory.Exists(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "RecordJSON.fdr")));

            // Read the metadata from the JSON file
            string json = File.ReadAllText(Path.Combine(inttestpath, "Entry 2.entry", "EntryMetaData.json"));
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement subtypeElement = doc.RootElement.GetProperty("SubType");
            string subType = subtypeElement.GetString();

            // Check that Metadata is written to correctly
            MetaData retrievedMD = JSONAdaptor.GetEntryMetaDataFromJSON(json, subType);
            Assert.Equivalent(md, MDAdaptor.ConvertMetaData(retrievedMD));
        }

        [Fact, TestPriority(9)]
        public void OpenExistingEntryFromFileManagement_TestsDatabase_Receipt_MDAdaptor()
        {
            Database dbinstance = Database.Instance;
            Tuple <MetaData, List <ConcurSolutionz.Database.Record>> fileDetails = dbinstance.getFileDetailFromFileName("Entry 2.entry");
            StudentProjectClaimMetaData md = MDAdaptor.ConvertMetaData(fileDetails.Item1);
            Receipt.ReceiptBuilder receiptBuilder = new();

            // Check that the metadata is read correctly
            Assert.Equal("Entry 2", md.EntryName);
            Assert.Equal(100, md.EntryBudget);
            Assert.Equal("Claim 1", md.ClaimName);
            Assert.Equal(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture), md.ClaimDate);
            Assert.Equal("Purpose 1", md.Purpose);
            Assert.Equal("Team 1", md.TeamName);
            Assert.Equal("Project Club 1", md.ProjectClub);
        }

        [Fact, TestPriority(10)]
        public void AddRecordToExistingEntry_TestsMDBuilder_Receipt_Entry()
        {
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt1;
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Populate existing entry and receipts
            receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            md = studentProjMDBuilder
                .SetEntryName("Entry 2")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            records.Add(receipt1);

            entry = entryBuilder.SetFileName("Entry 2")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(inttestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();

            // Add new receipt
            entry.AddRecord(receipt1);

            // Check that the old receipts exist and new receipt is added correctly
            // Read the records from the JSON files in RecordJSON.fdr
            List<ConcurSolutionz.Database.Record> retrievedRecords = new();
            foreach (string record in Directory.GetFiles(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "RecordJSON.fdr")))
            {
                string json = File.ReadAllText(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "RecordJSON.fdr", record));
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement subtypeElement = doc.RootElement.GetProperty("SubType");
                string subType = subtypeElement.GetString();

                ConcurSolutionz.Database.Record retrievedRecord = JSONAdaptor.GetRecordFromJSON(json, subType);
                retrievedRecords.Add(retrievedRecord);
            }

            // Check that Records are written to correctly
            Assert.True(retrievedRecords.Count == 1);
            Assert.Equivalent(receipt1, retrievedRecords[0]);

            // Check that images are populated correctly
            Assert.True(File.Exists(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "0.png")));

            Assert.Equal(File.ReadAllBytes(picturepath), File.ReadAllBytes(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "0.png")));
        }

        [Fact, TestPriority(11)]
        public void ModifyMetadataOfExistingEntry_TestsMDBuilder_Receipt_Entry()
        {
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt1;
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            StudentProjectClaimMetaData newMD;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Populate existing entry and receipts
            receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            md = studentProjMDBuilder
                .SetEntryName("Entry 2")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            records.Add(receipt1);

            entry = entryBuilder.SetFileName("Entry 2")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(inttestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();


            // Edit Metadata
            newMD = studentProjMDBuilder
                .SetEntryName("Entry 2")
                .SetEntryBudget(90)
                .SetClaimName("Claim ABC")
                .SetClaimDate(DateTime.ParseExact("10/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose ABC")
                .SetTeamName("Team ABC")
                .SetProjectClub("Project Club ABC")
                .Build();

            entry.MetaData = newMD;

            // Read the metadata from the JSON file
            string json = File.ReadAllText(Path.Combine(inttestpath, "Entry 2.entry", "EntryMetaData.json"));
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement subtypeElement = doc.RootElement.GetProperty("SubType");
            string subType = subtypeElement.GetString();

            // Check that Metadata is written to correctly
            MetaData retrievedMD = JSONAdaptor.GetEntryMetaDataFromJSON(json, subType);
            Assert.Equivalent(newMD, MDAdaptor.ConvertMetaData(retrievedMD));
        }

        [Fact, TestPriority(12)]
        public void DeleteRecordFromExistingEntry_TestsMDBuilder_Receipt_Entry()
        {
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt1;
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            StudentProjectClaimMetaData newMD;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Populate existing entry and receipts
            receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "0.png"))
                .Build();

            md = studentProjMDBuilder
                .SetEntryName("Entry 2")
                .SetEntryBudget(90)
                .SetClaimName("Claim ABC")
                .SetClaimDate(DateTime.ParseExact("10/12/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose ABC")
                .SetTeamName("Team ABC")
                .SetProjectClub("Project Club ABC")
                .Build();

            records.Add(receipt1);

            entry = entryBuilder.SetFileName("Entry 2")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(inttestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();

            // Delete Record 1
            entry.DelRecord(receipt1);

            // Check that the Receipt 1 is deleted
            // Read the records from the JSON files in RecordJSON.fdr
            List<ConcurSolutionz.Database.Record> retrievedRecords = new();
            foreach (string record in Directory.GetFiles(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "RecordJSON.fdr")))
            {
                string json = File.ReadAllText(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "RecordJSON.fdr", record));
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement subtypeElement = doc.RootElement.GetProperty("SubType");
                string subType = subtypeElement.GetString();

                ConcurSolutionz.Database.Record retrievedRecord = JSONAdaptor.GetRecordFromJSON(json, subType);
                retrievedRecords.Add(retrievedRecord);
            }

            // Check that Receipt 2 is deleted correctly
            Assert.True(retrievedRecords.Count == 0);

            // Check that images are populated correctly
            Assert.False(File.Exists(Path.Combine(inttestpath, "Entry 2.entry", "Records.fdr", "0.png")));

        }

        [Fact, TestPriority(13)]
        public void TransferToConcur_TestsConcurAPI_SeleniumWrapper_CookieBrowser()
        {
            // TODO
        }

        [Fact, TestPriority(14)]
        public void CreatingCookie_TestsCookie_CookieStorage()
        {
            Cookie.CookieBuilder cookieBuilder = new();
            Cookie cookie;
            string path = Path.Combine(inttestpath, "CookiePath");

            // Act
            cookie = cookieBuilder.SetExpiryDate(DateTime.ParseExact("24/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetBm_sz("abc")
                .SetTAsessionID("123")
                .SetAk_bmsc("def")
                .Set_abck("456")
                .SetOTSESSIONAABQRD("ghi")
                .SetJWT("789")
                .SetBm_sv("jkl")
                .Build();

            CookieStorage storage = new CookieStorage { CookieStoragePath = path };
            storage.StoreCookie(cookie);

            // Check if cookie was stored correctly
            Assert.True(File.Exists(Path.Combine(path, "cookie.json")));

            string json = File.ReadAllText(Path.Combine(path, "cookie.json"));
            var options = new JsonSerializerOptions();
            options.Converters.Add(new Cookie.CookieConverter());

            Cookie assembledCookie = JsonSerializer.Deserialize<Cookie>(json, options);
            Assert.Equivalent(cookie, assembledCookie);
        }

    }
}
