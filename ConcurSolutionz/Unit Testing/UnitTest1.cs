using ConcurSolutionz.Database;
using System.Globalization;

namespace Unit_Testing
{
    public class UnitTest1
    {
        // Initialize objects to be used for testing
        StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
        StudentProjectClaimMetaData md;
        Receipt.ReceiptBuilder receiptBuilder = new();
        Receipt receipt1;
        Receipt receipt2;
        List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

        [Fact]
        public void CreateFileTest_ShouldCreateFolder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFilePath("D:")
                .SetFileName("Folder 1")
                .SetCreationDate(DateTime.Now)
                .Build();
            FileCreator.CreateFile(folder);

            // Assert
            Assert.True(Directory.Exists(folder.FilePath));
        }

        [Fact]
        public void CreateFileTest_ShouldCreateFileasEntry()
        {
            // Arrange
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
                .SetImgPath("IMG_5669.JPG")
                .Build();

            receipt2 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("10/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("DTI items")
                .SetSupplierName("Popular")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(20.5m)
                .SetReceiptNumber("183774544")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_0294.JPG")
                .Build();

            records.Add(receipt1);
            records.Add(receipt2);

            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetPolicy("Policy A")
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.Now)
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            entry = entryBuilder.SetFilePath(@"D:\TestFolder")
                .SetFileName("File 1")
                .SetCreationDate(DateTime.Now)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();

            FileCreator.CreateFile(entry);

            // Assert
            Assert.True(Directory.Exists(entry.FilePath));
            Assert.True(File.Exists(Path.Combine(entry.FilePath, entry.FileName)));

            string fileContent = File.ReadAllText(Path.Combine(entry.FilePath, entry.FileName));

            Assert.Contains(md.EntryName, fileContent);
            Assert.Contains(md.EntryBudget.ToString(), fileContent);
            Assert.Contains(md.Policy, fileContent);
            Assert.Contains(md.ClaimName, fileContent);
            Assert.Contains(md.ClaimDate.ToString(), fileContent);
            Assert.Contains(md.Purpose, fileContent);
            Assert.Contains(md.TeamName, fileContent);
            Assert.Contains(md.ProjectClub, fileContent);

        }


        [Fact]
        public void CreateFileTest_ShouldThrowException_ForDuplicateFileName()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry1;


            // Act
            entry1 = entryBuilder.SetFilePath(@"D:\TestFolder")
                .SetFileName("File 1")
                .SetCreationDate(DateTime.Now)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry1);

            // Assert
            Assert.Throws<Exception>(() => FileCreator.CreateFile(entry1));
        }

        [Fact]
        public void CreateFileTest_ShouldThrowException_ForDuplicateFolderName()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();
            Folder folder1;

            // Act
            folder1 = folderBuilder.SetFilePath(@"D:")
                .SetFileName("Folder 1")
                .SetCreationDate(DateTime.Now)
                .Build();
            FileCreator.CreateFile(folder1);

            // Assert
            Assert.Throws<Exception>(() => FileCreator.CreateFile(folder1));
        }

        [Fact]
        public void CreateFileTest_ShouldThrowException_ForNullFile()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry2;

            // Act
            entry2 = entryBuilder.SetFilePath(@"D:\TestFolder")
                .SetFileName(null)
                .SetCreationDate(DateTime.Now)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry2);

            // Assert
            Assert.Throws<Exception>(() => FileCreator.CreateFile(entry2));
        }

        [Fact]
        public void CreateFileTest_ShouldThrowException_ForNullFolder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();
            Folder folder2;

            // Act
            folder2 = folderBuilder.SetFilePath(@"D:")
                .SetFileName(null)
                .SetCreationDate(DateTime.Now)
                .Build();
            FileCreator.CreateFile(folder2);

            // Assert
            Assert.Throws<Exception>(() => FileCreator.CreateFile(folder2));
        }

        [Fact]
        public void CreateFileTest_ShouldThrowException_ForNullFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry3;

            // Act
            entry3 = entryBuilder.SetFilePath(null)
                .SetFileName("File 2")
                .SetCreationDate(DateTime.Now)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry3);

            // Assert
            Assert.Throws<Exception>(() => FileCreator.CreateFile(entry3));
        }

        [Fact]
        public void CreateFileTest_ShouldThrowException_ForInvalidFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry4;

            // Act
            entry4 = entryBuilder.SetFilePath(@"D:\abc")
                .SetFileName("File 3")
                .SetCreationDate(DateTime.Now)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry4);

            // Assert
            Assert.Throws<Exception>(() => FileCreator.CreateFile(entry4));
        }


        [Fact]
        public void PopulateReceiptFolder_CopiesReceiptImageAndMetadata()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry5;



            entry5 = entryBuilder.SetFilePath(@"D:\TestFolder")
                .SetFileName("File 5")
                .SetCreationDate(DateTime.Now)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();



            string receiptFolderPath = $@"{entry5.FilePath}\Receipts";
            string receiptJSONFolder = $@"{entry5.FilePath}\MetaData";

            // Act
            FileCreator.PopulateReceiptFolder(entry5, receiptFolderPath, receiptJSONFolder);

            // Assert
            Assert.True(File.Exists($@"{receiptFolderPath}\{receipt1.RecordID}.json"));
            Assert.True(File.Exists($@"{receiptFolderPath}\{receipt2.RecordID}.json"));
        }

    }
}