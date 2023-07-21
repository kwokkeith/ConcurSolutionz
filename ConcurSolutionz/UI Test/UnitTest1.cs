//namespace UI_Test
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        [TestMethod]
//        public void TestMethod1()
//        {
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ConcurSolutionz.Views;
using ConcurSolutionz.Database;
using Newtonsoft.Json.Bson;
using System.Net.Security;

namespace UI_Test
{
    public class MainPageTests
    {
        [Fact]
        public void DoubleTap()
        {
            var mainPage = new ConcurSolutionz.Views.MainPage();
            //mainPage.currentDirectoryPath = "C:/";
            Settings settings = new Settings();
            settings.SetRootDirectory("C:/");
            //object value = mainPage.OnFileDoubleTapped.Execute();
            // Double Tapped into project1 folder
            mainPage.currentDirectoryPath = Path.Combine(settings.GetRootDirectory(), "hello.fdr");
            string path = mainPage.currentDirectoryPath;

            Xunit.Assert.Equal("C:/hello.fdr", path);

        }



        //[Fact]
        //public void SortFiles_Alphabetical_ShouldSortFilesByFileName()
        //{
        //    // Arrange
        //    var mainPage = new ConcurSolutionz.Views.MainPage();
        //    var files = new List<FileItem>
        //    {
        //        new FileItem("File3.txt", false),
        //        new FileItem("File1.txt", false),
        //        new FileItem("File2.txt", false)
        //    };

        //    // Act
        //    mainPage.Files = new ObservableCollection<FileItem>(files);
        //    mainPage.CurrentSortOption = SortOption.Alphabetical;
        //    mainPage.SortFiles();

        //    // Assert
        //    var sortedFiles = mainPage.Files.ToList();
        //    Xunit.Assert.Equal("File1.txt", sortedFiles[0].FileName);
        //    Xunit.Assert.Equal("File2.txt", sortedFiles[1].FileName);
        //    Xunit.Assert.Equal("File3.txt", sortedFiles[2].FileName);
        //}

        //[Fact]
        //public void SortFiles_Date_ShouldSortFilesByCreationDateTimeDescending()
        //{
        //    // Arrange
        //    var mainPage = new MainPage();
        //    var files = new List<FileItem>
        //    {
        //        new FileItem("File1.txt", false),
        //        new FileItem("File2.txt", false),
        //        new FileItem("File3.txt", false)
        //    };

        //    // Set different creation dates for each file
        //    files[0].CreationDateTime = new DateTime(2023, 7, 20, 12, 0, 0);
        //    files[1].CreationDateTime = new DateTime(2023, 7, 21, 8, 0, 0);
        //    files[2].CreationDateTime = new DateTime(2023, 7, 19, 15, 0, 0);

        //    // Act
        //    mainPage.Files = new ObservableCollection<FileItem>(files);
        //    mainPage.CurrentSortOption = SortOption.Date;
        //    mainPage.SortFiles();

        //    // Assert
        //    var sortedFiles = mainPage.Files.ToList();
        //    Xunit.Assert.Equal("File2.txt", sortedFiles[0].FileName);
        //    Xunit.Assert.Equal("File1.txt", sortedFiles[1].FileName);
        //    Xunit.Assert.Equal("File3.txt", sortedFiles[2].FileName);
        //}

        //[Fact]
        //public async Task OnNewEntryClicked_ValidEntryName_ShouldNavigateToEntryPageWithNewName()
        //{
        //    // Arrange
        //    var mainPage = new MainPage();
        //    var newName = "NewEntry.txt";
        //    // Set up the Shell.Current.GoToAsync() mock
        //    var mockGoToAsync = new Func<string, Task>(page =>
        //    {
        //        Xunit.Assert.Equal($"{nameof(EntryPage)}?fileName={newName}&existingFile=False", page);
        //        return Task.CompletedTask;
        //    });

        //    // Act
        //    mainPage.OnNewEntryClicked(null, null);

        //    // Assert
        //    await mockGoToAsync($"{nameof(EntryPage)}?fileName={newName}&existingFile=False");
        //}

        //[Fact]
        //public void OnDeleteClicked_SelectedFileNotNull_ShouldDeleteSelectedFile()
        //{
        //    // Arrange
        //    var mainPage = new MainPage();
        //    var selectedFileName = "ToDelete.txt";
        //    mainPage.Files = new ObservableCollection<FileItem>
        //    {
        //        new FileItem("File1.txt", false),
        //        new FileItem(selectedFileName, false),
        //        new FileItem("File3.txt", false)
        //    };

        //    // Act
        //    mainPage.SelectedFile = mainPage.Files.FirstOrDefault(f => f.FileName == selectedFileName);
        //    mainPage.OnDeleteClicked(null, null);

        //    // Assert
        //    Xunit.Assert.Null(mainPage.SelectedFile);
        //    Xunit.Assert.DoesNotContain(mainPage.Files, f => f.FileName == selectedFileName);
        //}
    }
}
