using ConcurSolutionz.Database;
using System;
using System.Globalization;
using System.Diagnostics;
using System.Reflection.Metadata;
using Xunit;

namespace Unit_Test
{
    public class EntrySetup : IDisposable
    {
        public EntrySetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "EntryTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "EntryTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "EntryTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class EntryTests : IClassFixture<EntrySetup>
    {
        // Initialize objects to be used for testing
        StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
        StudentProjectClaimMetaData md;
        Receipt.ReceiptBuilder receiptBuilder = new();
        Receipt receipt1;
        Receipt receipt2;
        List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
        string entrytestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "EntryTest.fdr");
        string picturepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", "IMG_1001.png");

        [Fact(DisplayName = "5.1"), TestPriority(0)]
        public void BuildEntry_ShouldBuild_UsingBuilder()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 1.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 1.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 1")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            // Assert
            Assert.True(Directory.Exists(entry.FilePath));

            string fileContent = File.ReadAllText(Path.Combine(entry.FilePath, "EntryMetaData.json"));

            Assert.Contains(md.EntryName, fileContent);
            Assert.Contains(md.EntryBudget.ToString(), fileContent);
            Assert.Contains(md.Policy, fileContent);
            Assert.Contains(md.ClaimName, fileContent);
            Assert.Contains("2023-02-10T00:00:00", fileContent);
            Assert.Contains(md.Purpose, fileContent);
            Assert.Contains(md.TeamName, fileContent);
            Assert.Contains(md.ProjectClub, fileContent);

        }

        [Fact(DisplayName = "5.2"), TestPriority(1)]
        public void BuildEntry_ShouldThrowException_ForMissingAttributes()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => entryBuilder.Build());
        }

        [Fact(DisplayName = "5.3"), TestPriority(2)]
        public void BuildEntry_ShouldThrowException_ForSettingFilePathBeforeFileName()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => entryBuilder.SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetFileName("Entry 2")
                .SetMetaData(md)
                .SetRecords(records)
                .Build());
        }

        [Fact(DisplayName = "5.4"), TestPriority(3)]
        public void BuildEntry_ShouldThrowException_ForDuplicateFileName()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 3")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);


            // Act & Assert
            Assert.Throws<IOException>(() => Database.CreateFile(entry));

        }


        [Fact(DisplayName = "5."), TestPriority(4)]
        public void BuildEntry_ShouldThrowException_ForNullFile()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => entryBuilder.SetFileName(null));
        }

        [Fact(DisplayName = "5."), TestPriority(5)]
        public void BuildEntry_ShouldThrowException_ForNullFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<IOException>(() => entryBuilder.SetFileName("Entry 4")
                .SetFilePath(null));
        }

        [Fact(DisplayName = "5."), TestPriority(6)]
        public void BuildEntry_ShouldThrowException_ForInvalidFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<IOException>(() => entryBuilder.SetFileName("Entry 5")
                .SetFilePath(Path.Combine(entrytestpath, "jasbnfjnjwn")));
        }

        [Fact(DisplayName = "5."), TestPriority(7)]
        public void BuildEntry_ShouldThrowException_ForSettingFutureCreationDate()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => entryBuilder.SetFileName("Entry 6")
                .SetCreationDate(DateTime.ParseExact("20/02/2043", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build());
        }

        [Fact(DisplayName = "5."), TestPriority(8)]
        public void BuildEntry_ShouldThrowException_ForSettingFutureLastModifiedDate()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => entryBuilder.SetFileName("Entry 7")
                .SetCreationDate(DateTime.ParseExact("20/02/2043", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build());
        }

        [Fact(DisplayName = "5."), TestPriority(9)]
        public void BuildEntry_ShouldReturnFalseFolderVariable()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 8.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 8.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 8")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 8")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);


            Assert.False(entry.Folder);
        }

        [Fact(DisplayName = "5.8"), TestPriority(10)]
        public void AssignRecordID_SetsAUniqueRecordID()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 9.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 9.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 9")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 9")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            entry.AddRecord(receipt2);
            Debug.Write("\n\n RECORDS:" + entry.GetRecords());

            Assert.Equal(0, entry.GetRecords()[0].RecordID);
            Assert.Equal(1, entry.GetRecords()[1].RecordID);

        }

        [Fact(DisplayName = "5.9"), TestPriority(11)]
        public void AddRecord_AddsRecord_ToListOfRecords()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 10.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 10.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 10")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 10")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            entry.AddRecord(receipt2);

            Assert.Equal(2, entry.GetRecords().Count);

        }

        [Fact(DisplayName = "5.10"), TestPriority(12)]
        public void DelRecord_DeletesRecord_FromListOfRecords()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 11.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 11.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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
            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 11")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 11")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);
            entry.AddRecord(receipt2);

            entry.DelRecord(receipt2);

            Assert.Equal(1, entry.GetRecords().Count);
        }

        [Fact(DisplayName = "5.11"), TestPriority(13)]
        public void DelRecord_ThrowsExceptionWhen_DeletingNonExistingRecord()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 12.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 12.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 12")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 12")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            Assert.Throws<ArgumentException>(() => entry.DelRecord(receipt2));

        }

        [Fact(DisplayName = "5.12"), TestPriority(14)]
        public void DelRecordByID_DeletesRecord_FromListOfRecords()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 13.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 13.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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
            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();


            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 13")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 13")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);
            entry.AddRecord(receipt2);
            entry.DelRecordByID(0);

            Assert.Equal(1, entry.GetRecords()[0].RecordID);
        }

        [Fact(DisplayName = "5.13"), TestPriority(15)]
        public void DelRecordByID_ThrowsExceptionWhen_DeletingNonExistingRecord()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 14.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 14.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();                               

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 14")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 14")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            entry.AddRecord(receipt2);

            Assert.Throws<ArgumentException>(() => entry.DelRecordByID(8));

        }

        [Fact(DisplayName = "5.14"), TestPriority(16)]
        public void GetRecords_ReturnsListOfRecords()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 15.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 15.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 15")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 15")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            entry.AddRecord(receipt2);
            List<ConcurSolutionz.Database.Record> record_list= entry.GetRecords();

            Assert.Equal(2, record_list.Count);
            Assert.Equal(receipt1, record_list[0]);
            Assert.Equal(receipt2, record_list[1]);

        }

        [Fact(DisplayName = "5.15"), TestPriority(17)]
        public void GetRecord_ReturnsRecord_WithSpecifiedRecordID()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 16.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 16.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act
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

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("838492170-CFSIFJ33")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath(picturepath)
                .Build();

            records.Add(receipt1);

            md = studentProjMDBuilder
                .SetEntryName("Entry 16")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 16")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            entry.AddRecord(receipt2);

            Assert.Equal(receipt1, entry.GetRecord(0));
            Assert.Equal(receipt2, entry.GetRecord(1));
        }

        [Fact(DisplayName = "5.16"), TestPriority(18)]
        public void GetRecord_ReturnsNull_WhenRetrievingNonExistingRecord()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(entrytestpath + "Entry 17.entry")))
            {
                Directory.Delete(Path.Combine(entrytestpath + "Entry 17.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;

            // Act

            md = studentProjMDBuilder
                .SetEntryName("Entry 17")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFileName("Entry 17")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(entrytestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            Database.CreateFile(entry);

            Assert.Equal(null, entry.GetRecord(8));
        }
    }
}