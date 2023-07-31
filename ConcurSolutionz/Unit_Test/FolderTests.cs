using System.Globalization;
using ConcurSolutionz.Database;
using Unit_Test;

namespace Unit_Test
{
    public class FolderSetup : IDisposable
    {
        public FolderSetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "FolderTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "FolderTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "FolderTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class FolderTests: IClassFixture<FolderSetup>
    {
        string foldertestpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests", "FolderTest.fdr");

        [Fact(DisplayName = "6.1")]
        public void BuildFolder_ShouldBuild_UsingBuilder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(foldertestpath)
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

            string Expected4 = Path.Combine(foldertestpath, "Folder 1.fdr");
            Assert.Equal(Expected4, folder.FilePath);

        }

        // Fuzzing the values of folder creation
        [Fact(DisplayName = "6.2")]
        public void BuildFolder_ShouldBuild_UsingBuilder_Fuzz()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Fuzz values
            string fileName = Fuzzer.GenerateRandomString(10);
            DateTime creationDate = Fuzzer.GenerateRandomDateTime();
            DateTime modifiedDate = Fuzzer.GenerateRandomDateTime();

            // Assert for creation date and modified date
            if (creationDate > modifiedDate || creationDate > DateTime.Now || modifiedDate > DateTime.Now)
            {
                Assert.Throws<ArgumentException>(() => folderBuilder.SetCreationDate(creationDate).SetLastModifiedDate(modifiedDate));
            }
            else
            {
                // Act
                folder = folderBuilder.SetFileName(fileName)
                    .SetCreationDate(creationDate)
                    .SetLastModifiedDate(modifiedDate)
                    .SetFilePath(foldertestpath)
                    .Build();
                FileCreator.CreateFile(folder);

                // Assert
                Assert.True(Directory.Exists(Path.Combine(folder.FileName, folder.FilePath)));

                string Expected1 = fileName + ".fdr";
                Assert.Equal(Expected1, folder.FileName);

                DateTime Expected2 = creationDate;
                Assert.Equal(Expected2, folder.CreationDate);

                DateTime Expected3 = modifiedDate;
                Assert.Equal(Expected3, folder.LastModifiedDate);

                string Expected4 = Path.Combine(foldertestpath, Expected1);
                Assert.Equal(Expected4, folder.FilePath);
            }
        }

        [Fact(DisplayName = "6.3")]
        public void BuildFolder_ShouldThrowException_ForEmptyFolder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => folderBuilder.SetFileName(""));
        }

        [Fact(DisplayName = "6.4")]
        public void BuildFolder_ShouldThrowException_ForNullFolder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => folderBuilder.SetFileName(null));
        }

        [Fact(DisplayName = "6.5")]
        public void BuildFolder_ShouldThrowException_ForSettingFilePathBeforeFileName()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => folderBuilder.SetFilePath(foldertestpath)
            .SetFileName("Folder 1"));
        }

        [Fact(DisplayName = "6.6")]
        public void BuildFolder_ShouldThrowException_ForDateTimeAheadOfNow()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() => folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2099", "dd/MM/yyyy", CultureInfo.InvariantCulture)));
        }

        [Fact(DisplayName = "6.x")]
        public void BuildFolder_ShouldThrowException_ForCreationDateAheadOfLastModifiedDate()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() => folderBuilder.SetFileName("Folder 1")
                            .SetCreationDate(DateTime.ParseExact("24/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                            .SetLastModifiedDate(DateTime.ParseExact("14/11/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture)));
        }

        [Fact(DisplayName = "6.7")]
        public void BuildFolder_ShouldBuildFolder_WithFolderSetToTrue()
        {
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(foldertestpath)
                .Build();

            // Assert
            Assert.True(folder.Folder);

        }

        [Fact(DisplayName = "6.8")]
        public void StepIntoFolder_ChangesWorkingDirectory_toFolderFilePath()
        {
            Database dbinstance = Database.Instance;
            // Arrange
            if (Directory.Exists(foldertestpath))
            {
                Directory.Delete(foldertestpath, true);
            }
            Directory.CreateDirectory(foldertestpath);

            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFileName("Folder 2")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("30/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath(foldertestpath)
                .Build();
            FileCreator.CreateFile(folder);

            folder.SelectedAction();
            Assert.True(dbinstance.Getwd() == folder.FilePath);
        }

    }
}
