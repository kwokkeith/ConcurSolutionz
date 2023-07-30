using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using ConcurSolutionz.Database;

namespace Unit_Test
{
    public class FileCreatorSetup : IDisposable
    {
        public FileCreatorSetup()
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

            if (Directory.Exists(Path.Combine(testdirectoryPath, "FileCreatorTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "FileCreatorTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "FileCreatorTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class FileCreatorTest : IClassFixture<FileCreatorSetup>
    {
        string picturesfolderpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures");
        string picturepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", "IMG_1000.png");
        string filecreatortestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "FileCreatorTest.fdr");

        [Fact, TestPriority(0)]
        public void CreateFile_ShouldCreateFolder_UsingFileCreator()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(filecreatortestpath, "Folder 1.fdr")))
            {
                Directory.Delete(Path.Combine(filecreatortestpath, "Folder 1.fdr"), true);
            }

            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(filecreatortestpath)
                .Build();
            FileCreator.CreateFile(folder);

            // Assert
            Assert.True(Directory.Exists(Path.Combine(folder.FileName, folder.FilePath)));

            string Expected1 = "Folder 1.fdr";
            Assert.Equal(Expected1, folder.FileName);

            DateTime Expected2 = DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected2, folder.CreationDate);

            DateTime Expected3 = DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected3, folder.LastModifiedDate);
        }

        [Fact, TestPriority(1)]
        public void CreateFile_ShouldCreateEntry_UsingFileCreator()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(filecreatortestpath, "Entry 1.entry")))
            {
                Directory.Delete(Path.Combine(filecreatortestpath, "Entry 1.entry"), true);
            }

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            Receipt.ReceiptBuilder receiptBuilder = new();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt receipt1;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Act
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

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


            entry = entryBuilder.SetFileName("Entry 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(filecreatortestpath)
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry);

            // Assert
            Assert.True(Directory.Exists(Path.Combine(entry.FileName, entry.FilePath)));
        }

        [Fact, TestPriority(2)]
        public void CreateFile_ShouldThrowException_ForExistingFile()
        {

            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(filecreatortestpath)
                .Build();

            //Assert
            Assert.Throws<IOException>(() => FileCreator.CreateFile(folder));

        }


        [Fact, TestPriority(3)]
        public void CopyFile_ShouldCopy_NonExistingFile()
        {
            // Arrange
            if (Directory.Exists(Path.Combine(filecreatortestpath, "CopyFileTest")))
            {
                Directory.Delete(Path.Combine(filecreatortestpath, "CopyFileTest"), true);
            }
            Directory.CreateDirectory(Path.Combine(filecreatortestpath, "CopyFileTest"));

            // Act
            FileCreator.CopyFile(picturepath, Path.Combine(filecreatortestpath, "CopyFileTest", "IMG_1000.png"));

            // Assert
            Assert.True(File.Exists(Path.Combine(filecreatortestpath, "CopyFileTest", "IMG_1000.png")));
        }

        [Fact, TestPriority(4)]
        public void CopyFile_ShouldOverwrite_ExistingFile()
        {
            // Act
            FileCreator.CopyFile(picturepath, Path.Combine(filecreatortestpath, "CopyFileTest", "IMG_1000.png"));

            // Assert
            Assert.True(File.Exists(Path.Combine(filecreatortestpath, "CopyFileTest", "IMG_1000.png")));
        }

        [Fact, TestPriority(5)]
        public void CopyFile_ShouldThrowException_ForIncorrectFilePath()
        {
            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => FileCreator.CopyFile(picturepath, Path.Combine(filecreatortestpath, "skfjdjaibsnf", "IMG_1000.png")));
        }



        [Fact, TestPriority(6)]
        public void PopulateReceiptFolder_ShouldPopulateFolder_ForNonExistingReceipts()
        {
            if (Directory.Exists(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest")))
            {
                Directory.Delete(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"), true);
            }
            Directory.CreateDirectory(Path.Combine(filecreatortestpath,"PopulateReceiptFolderTest", "ReceiptJSON"));

            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            Receipt.ReceiptBuilder receiptBuilder = new();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt receipt1;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Act
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

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


            entry = entryBuilder.SetFileName("Entry 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"))
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.PopulateReceiptFolder(entry, Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"), Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "ReceiptJSON"));
           
            // Assert
            Assert.True(File.Exists(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "Receipt 0.png")));
            Assert.True(File.Exists(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "ReceiptJSON", "0.json")));

        }

        [Fact, TestPriority(7)]
        public void PopulateReceiptFolder_ShouldPopulateFolder_ForExistingReceipts()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            Receipt.ReceiptBuilder receiptBuilder = new();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt receipt1;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Act
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

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


            entry = entryBuilder.SetFileName("Entry 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"))
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.PopulateReceiptFolder(entry, Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"), Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "ReceiptJSON"));

            // Assert
            Assert.True(File.Exists(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "Receipt 0.png")));
            Assert.True(File.Exists(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "ReceiptJSON", "0.json")));

        }

        [Fact, TestPriority(8)]
        public void PopulateReceiptFolder_ShouldThrowException_ForIncorrectFilePath()
        {
            // Arrange
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            Receipt.ReceiptBuilder receiptBuilder = new();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt receipt1;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            // Act
            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

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


            entry = entryBuilder.SetFileName("Entry 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"))
                .SetMetaData(md)
                .SetRecords(records)
                .Build();

            // Assert
            Assert.Throws<DirectoryNotFoundException>(() => FileCreator.PopulateReceiptFolder(entry, Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest"), Path.Combine(filecreatortestpath, "PopulateReceiptFolderTest", "fsafas", "ReceiptJSON")));

        }



    }
}
