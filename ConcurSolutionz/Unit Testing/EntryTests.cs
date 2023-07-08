using ConcurSolutionz.Database;
using System;
using System.Globalization;
using System.Reflection.Metadata;
using Xunit;

namespace Unit_Testing
{
    public class EntryTests
    {
        // Initialize objects to be used for testing
        StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
        StudentProjectClaimMetaData md;
        Receipt.ReceiptBuilder receiptBuilder = new();
        Receipt receipt1;
        Receipt receipt2;
        List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

        [Fact]
        public void BuildEntry_ShouldBuild_UsingBuilder()
        {
            // Arrange
            if (Directory.Exists(@"D:\ConcurTests\EntryTest.fdr\File 1.entry"))
            {
                Directory.Delete(@"D:\ConcurTests\EntryTest.fdr\File 1.entry", true);
            }

            if (!Directory.Exists(@"D:\ConcurTests\EntryTest.fdr"))
            {
                Directory.CreateDirectory(@"D:\ConcurTests\EntryTest.fdr");
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
                .SetImgPath(@"D:\IMG_5669.jpg")
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

            entry = entryBuilder.SetFileName("File 1")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(@"D:\ConcurTests\EntryTest.fdr")
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry);

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


        [Fact]
        public void BuildEntry_ShouldThrowException_ForDuplicateFileName()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<IOException>(() => entryBuilder.SetFileName("File 1")
                .SetCreationDate(DateTime.Now)
                .SetFilePath(@"D:\Folder 1.fdr"));
        }

   
        [Fact]
        public void BuildEntry_ShouldThrowException_ForNullFile()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => entryBuilder.SetFileName(null));
        }

        [Fact]
        public void BuildEntry_ShouldThrowException_ForNullFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<IOException>(() => entryBuilder.SetFileName("File 2")
                .SetFilePath(null));
        }

        [Fact]
        public void BuildEntry_ShouldThrowException_ForInvalidFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();

            // Act & Assert
            Assert.Throws<IOException>(() => entryBuilder.SetFileName("File 3")
                .SetFilePath(@"D:\abc"));
        }

    }
}