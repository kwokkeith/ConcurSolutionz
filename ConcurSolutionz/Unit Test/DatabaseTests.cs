using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ConcurSolutionz.Database;

namespace Unit_Test
{

    public class DBSetup : IDisposable
    {
        public DBSetup()
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

            if (Directory.Exists(Path.Combine(testdirectoryPath, "DatabaseTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "DatabaseTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "DatabaseTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class DatabaseTests : IClassFixture<DBSetup>
    {
        string dbtestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "DatabaseTest.fdr");
        string picturepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Pictures", "IMG_1000.png");

        [Fact]
        public void InstanceTest_ShouldReturnSameInstance()
        {
            // Arrange & Act
            Database instance1 = Database.Instance;
            Database instance2 = Database.Instance;

            // Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void SetSettings_AndGetSettings_ReturnsSameDirectory()
        {
            // Arrange
            Database dbinstance = Database.Instance;
            Settings settings = new();
            string path = dbtestpath;
            settings.SetRootDirectory(path);


            // Act
            dbinstance.SetSetting(settings);

            // Assert
            Settings retrievedSettings = dbinstance.GetSettings();
            Assert.Equal(settings, retrievedSettings);
            Assert.Equal(path, settings.GetRootDirectory());

        }

        [Fact]
        public void SetsWorkingDirectory_AndGetWorkingDirectory_ReturnsSameWorkingDirectory()
        {
            // Arrange
            Database dbinstance = Database.Instance;
            string path = dbtestpath;

            // Act
            dbinstance.Setwd(path);

            // Assert
            Assert.Equal(path, dbinstance.Getwd());
        }

        [Fact]
        public void CreateFile_ForEntryType_CreatesEntry()
        {
            // Arrange
            Database dbinstance = Database.Instance;
            string path = dbtestpath;
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt1;


            // Act

            receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                    .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetDescription("Pizza Hut for bonding activities")
                    .SetSupplierName("Pizza Hut")
                    .SetCityOfPurchase("Singapore, SINGAPORE")
                    .SetReqAmount(104.5m)
                    .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                    .SetReceiptStatus("Tax Receipt")
                    .SetImgPath("D:/IMG_5669.jpg")
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
                    .SetFilePath(path)
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

        [Fact]
        public void CreateFile_ForFolderType_CreatesFolder()
        {
            // Arrange
            Database dbinstance = Database.Instance;
            string path = dbtestpath;
            List<string> files = new List<string> { };
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            if (Directory.Exists(Path.Combine(dbtestpath, "Folder 1.fdr")))
            {
                Directory.Delete(Path.Combine(dbtestpath, "Folder 1.fdr"), true);
            }

            folder = folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(dbtestpath)
                .Build();
            Database.CreateFile(folder);

            // Assert
            Assert.True(Directory.Exists(Path.Combine(folder.FileName, folder.FilePath)));

            string Expected1 = "Folder 1.fdr";
            Assert.Equal(Expected1, folder.FileName);

            DateTime Expected2 = DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected2, folder.CreationDate);

            DateTime Expected3 = DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected3, folder.LastModifiedDate);

            string Expected4 = Path.Combine(dbtestpath, "Folder 1.fdr");
            Assert.Equal(Expected4, folder.FilePath);

        }

        [Fact]
        public void DeleteFile_RemovesFolderOrEntryFromSystem()
        {
            // Arrange
            string path = Path.Combine(dbtestpath, "Folder 2.fdr");
            if (Directory.Exists(Path.Combine(dbtestpath, "Folder 2.fdr")))
            {
                Directory.Delete(Path.Combine(dbtestpath, "Folder 2.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(dbtestpath, "Folder 2.fdr"));

            // Act
            Database.DeleteDirectoryByFilePath(path);

            // Assert
            Assert.False(Directory.Exists(path));

        }

        [Fact]

        public void GetFilesFromWD_ReturnsBothFilesAndFolders()
        {
            // Arrange
            string path = Path.Combine(dbtestpath, "Folder 3.fdr");
            Database dbinstance = Database.Instance;
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt1;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }


            // Act
            folder = folderBuilder.SetFileName("Folder 3")
                    .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetFilePath(dbtestpath)
                    .Build();
            Database.CreateFile(folder);

            folder = folderBuilder.SetFileName("Folder A")
                    .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetFilePath(Path.Combine(dbtestpath, "Folder 3.fdr"))
                    .Build();
            Database.CreateFile(folder);


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

            entry = entryBuilder.SetFileName("File 1")
                    .SetCreationDate(DateTime.Now)
                    .SetFilePath(path)
                    .SetMetaData(md)
                    .SetRecords(records)
                    .Build();

            Database.CreateFile(entry);

            dbinstance.Setwd(path);
            var files = dbinstance.GetFilePathsFromWD();

            // Assert - should contain both folder and entry
            Assert.Contains(Path.Combine(path, "File 1.entry"), files);
            Assert.Contains(Path.Combine(path, "Folder A.fdr"), files);

        }

        [Fact]

        public void GetFilesFromWD_ReturnsFolders()
        {
            // Arrange
            string path = Path.Combine(dbtestpath, "Folder 4.fdr");
            Database dbinstance = Database.Instance;
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            // Act
            folder = folderBuilder.SetFileName("Folder 4")
                    .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetFilePath(dbtestpath)
                    .Build();
            Database.CreateFile(folder);

            folder = folderBuilder.SetFileName("Folder A")
                    .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetFilePath(Path.Combine(dbtestpath, "Folder 4.fdr"))
                    .Build();
            Database.CreateFile(folder);


            dbinstance.Setwd(path);
            var files = dbinstance.GetFilePathsFromWD();

            // Assert - should contain both folder and entry
            Assert.Contains(Path.Combine(path, "Folder A.fdr"), files);

        }


        [Fact]
        public void GetFilesFromWD_ReturnsEntries()
        {
            // Arrange
            string path = Path.Combine(dbtestpath, "Folder 5.fdr");
            Database dbinstance = Database.Instance;
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt1;

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }


            // Act
            folder = folderBuilder.SetFileName("Folder 5")
                    .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetFilePath(dbtestpath)
                    .Build();
            Database.CreateFile(folder);

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

            entry = entryBuilder.SetFileName("File 1")
                    .SetCreationDate(DateTime.Now)
                    .SetFilePath(path)
                    .SetMetaData(md)
                    .SetRecords(records)
                    .Build();

            Database.CreateFile(entry);

            dbinstance.Setwd(path);
            var files = dbinstance.GetFilePathsFromWD();

            // Assert
            Assert.Contains(Path.Combine(path, "File 1.entry"), files);
        }



        [Fact]
        public void FileSelectByFileName_SelectsFolderWithGivenName()
        {

            // Arrange
            string path1 = Path.Combine(dbtestpath, "Folder 6.fdr");
            string path2 = dbtestpath;
            string fileName = "Folder 6.fdr";
            Database dbinstance = Database.Instance;

            if (Directory.Exists(path1))
            {
                Directory.Delete(path1, true);
            }
            Directory.CreateDirectory(path1);
            dbinstance.Setwd(path2);

            // Act 
            dbinstance.FileSelectByFileName(fileName);

            // Assert  
            Assert.Equal(path1, dbinstance.Getwd());
        }

        [Fact]
        public void FileSelectByFileName_SelectsNothingWithGivenEntryName()
        {

            // Arrange
            string path1 = Path.Combine(dbtestpath, "Folder 7.fdr");
            string path2 = dbtestpath;
            Database dbinstance = Database.Instance;
            if (Directory.Exists(path1))
            {
                Directory.Delete(path1, true);
            }
            Directory.CreateDirectory(path1);
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            Entry entry2;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;



            string fileName = "Entry 1";
            dbinstance.Setwd(path1);

            // Act 
            md = studentProjMDBuilder
                    .SetEntryName(fileName)
                    .SetEntryBudget(100)
                    .SetClaimName("Claim 1")
                    .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetPurpose("Purpose 1")
                    .SetTeamName("Team 1")
                    .SetProjectClub("Project Club 1")
                    .Build();

            entry = entryBuilder.SetFileName(fileName)
                    .SetCreationDate(DateTime.Now)
                    .SetFilePath(path1)
                    .SetMetaData(md)
                    .SetRecords(records)
                    .Build();

            entry2 = entryBuilder.SetFileName("Entry 2")
                    .SetCreationDate(DateTime.Now)
                    .SetFilePath(path1)
                    .SetMetaData(md)
                    .SetRecords(records)
                    .Build();

            Database.CreateFile(entry);
            Database.CreateFile(entry2);


            dbinstance.FileSelectByFileName("Entry 2.entry");

            // Assert  
            Assert.Equal(Path.Combine(path1), dbinstance.Getwd());
        }

        [Fact]
        public void FileSelectByFilePath_SelectsFolderWithGivenPath()
        {

            // Arrange
            string path1 = Path.Combine(dbtestpath, "Folder 8.fdr");
            string path2 = dbtestpath;
            Database dbinstance = Database.Instance;
            if (Directory.Exists(path1))
            {
                Directory.Delete(path1, true);
            }
            Directory.CreateDirectory(path1);

            string fileName = "Folder 8.fdr";
            dbinstance.Setwd(path2);

            // Act 
            dbinstance.FileSelectByFilePath(path1);

            // Assert  
            Assert.Equal(path1, dbinstance.Getwd());
        }

        [Fact]
        public void FileSelectByFilePath_SelectsNothingWithGivenEntryPath()
        {

            // Arrange
            string path1 = Path.Combine(dbtestpath, "Folder 9.fdr");
            string path2 = dbtestpath;
            Database dbinstance = Database.Instance;
            if (Directory.Exists(path1))
            {
                Directory.Delete(path1, true);
            }
            Directory.CreateDirectory(path1);
            Entry.EntryBuilder entryBuilder = new();
            Entry entry;
            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;

            string fileName = "Entry 1";
            dbinstance.Setwd(path2);

            // Act 
            md = studentProjMDBuilder
                    .SetEntryName(fileName)
                    .SetEntryBudget(100)
                    .SetClaimName("Claim 1")
                    .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .SetPurpose("Purpose 1")
                    .SetTeamName("Team 1")
                    .SetProjectClub("Project Club 1")
                    .Build();

            entry = entryBuilder.SetFileName(fileName)
                    .SetCreationDate(DateTime.Now)
                    .SetFilePath(path1)
                    .SetMetaData(md)
                    .SetRecords(records)
                    .Build();
            Database.CreateFile(entry);

            // Act 

            dbinstance.FileSelectByFilePath(Path.Combine(path1, fileName + ".entry"));

            // Assert  
            Assert.Equal(Path.Combine(path2), dbinstance.Getwd());
        }


        [Fact]
        public void FileSelectByFileName_ThrowsException_IfFileNotExists()
        {

            // Arrange
            string fileName = "not_present.txt";
            Database dbinstance = Database.Instance;

            // Assert  
            Assert.Throws<Exception>(() =>
            {
                // Act  
                dbinstance.FileSelectByFileName(fileName);
            });
        }

        [Fact]
        public void FileGoBack_UpdatesWorkingDirectory()
        {

            // Arrange
            Thread.Sleep(100);
            string path1 = Path.Combine(dbtestpath, "Folder 9.fdr");
            string path2 = dbtestpath;

            Database dbinstance = Database.Instance;
            Settings settings = new();
            settings.SetRootDirectory(path2);
            dbinstance.SetSetting(settings);

            if (Directory.Exists(path1))
            {
                Directory.Delete(path1, true);
            }
            Directory.CreateDirectory(path1);

            dbinstance.Setwd(path1);

            // Act 
            dbinstance.FileGoBack();
            string newWd = Database.Instance.Getwd();

            // Assert
            Assert.Equal(path2, newWd);
        }

        [Fact]
        public void FileGoBack_ReturnsSameDirectory_WhenInRootDirectory()
        {
            // Arrange
            string path = dbtestpath;
            Database dbinstance = Database.Instance;

            Settings settings = new();
            settings.SetRootDirectory(path);
            dbinstance.SetSetting(settings);

            dbinstance.Setwd(path);

            // Act 
            dbinstance.FileGoBack();
            string newWd = Database.Instance.Getwd();

            // Assert
            Assert.Equal(path, newWd);

        }

    }
}
