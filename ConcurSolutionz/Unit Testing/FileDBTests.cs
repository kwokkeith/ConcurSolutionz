using ConcurSolutionz.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Testing
{
    public class FileDBSetup : IDisposable
    {
        public FileDBSetup()
        {
            string testdirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "ConcurTests");

            if (!Directory.Exists(testdirectoryPath))
            {
                Directory.CreateDirectory(testdirectoryPath);
            }

            if (Directory.Exists(Path.Combine(testdirectoryPath, "FileDBTest.fdr")))
            {
                Directory.Delete(Path.Combine(testdirectoryPath, "FileDBTest.fdr"), true);
            }
            Directory.CreateDirectory(Path.Combine(testdirectoryPath, "FileDBTest.fdr"));
        }

        public void Dispose()
        {
            // Do not remove: needed by IDisposable
            // Nothing is done to teardown
        }
    }

    public class FileDBTests: IClassFixture<FileDBSetup>
    {
        [Fact]
        public void CreationDate_FutureDate_ThrowsArgumentException()
        {
            FileDB fileDB = new MockFileDB();
            DateTime futureDate = DateTime.Now.AddDays(1);

            Assert.Throws<ArgumentException>(() => fileDB.CreationDate = futureDate);
        }

        [Fact]
        public void LastModifiedDate_FutureDate_ThrowsArgumentException()
        {
            var fileDB = new MockFileDB();
            DateTime futureDate = DateTime.Now.AddDays(1);

            Assert.Throws<ArgumentException>(() => fileDB.LastModifiedDate = futureDate);
        }

        [Fact]
        public void UpdateModifiedDate_SetsToCurrentDateTime()
        {
            var fileDB = new MockFileDB();

            fileDB.UpdateModifiedDate();

            Assert.Equal(DateTime.Now.Date, fileDB.LastModifiedDate.Date);
        }

        public class MockFileDB : FileDB
        {
            protected override void SetFolder()
            {
                // Mock implementation
            }

            public override void SelectedAction()
            {
                // Mock implementation
            }
        }
    }
}
