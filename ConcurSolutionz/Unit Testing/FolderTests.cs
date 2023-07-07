using System.Globalization;
using ConcurSolutionz.Database;

namespace Unit_Testing
{
    public class FolderTests
    {

        [Fact]
        public void BuildFolder_ShouldBuild_UsingBuilder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();
            Folder folder;

            // Act
            folder = folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("14/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath("D:")
                .Build();

            // Assert
            Assert.True(Directory.Exists(Path.Combine(folder.FileName, folder.FilePath)));

            string Expected1 = "Folder 1.fdr";
            Assert.Equal(Expected1, folder.FileName);

            DateTime Expected2 = DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected2, folder.CreationDate);

            DateTime Expected3 = DateTime.ParseExact("14/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected3, folder.LastModifiedDate);

            string Expected4 = @"D:\Folder 1.fdr";
            Assert.Equal(Expected4, folder.FilePath);

        }

        [Fact]
        public void BuildFolder_ShouldThrowException_ForDuplicateFolderName()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.Throws<IOException>(() => folderBuilder.SetFileName("Folder 1")
                .SetCreationDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetLastModifiedDate(DateTime.ParseExact("14/11/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetFilePath("D:"));
        }

        [Fact]
        public void BuildFolder_ShouldThrowException_ForNullFolder()
        {
            // Arrange
            Folder.FolderBuilder folderBuilder = new();

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() => folderBuilder.SetFileName(null));
        }
    }
}
