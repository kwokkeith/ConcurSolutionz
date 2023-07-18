using System;
using System.Globalization;
using ConcurSolutionz.Database;
using Newtonsoft.Json.Linq;

namespace Unit_Testing
{
    public class MetaDataTest
    {
        [Fact]
        public void Build_MetaDataShouldBuildUsingBuilder()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;

            // Act
            metadata = builder.SetEntryName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetTeamName("RaisinStudios")
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;

            // Assert
            string Expected1 = "(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES";
            Assert.Equal(Expected1, MD.ProjectClub);

            DateTime Expected2 = DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected2, MD.ClaimDate);

            string Expected3 = "SCF-MAY2022-001";
            Assert.Equal(Expected3, MD.ClaimName);

            string Expected4 = "Cohort class bonding for cohort 02";
            Assert.Equal(Expected4, MD.Purpose);

            string Expected5 = "RaisinStudios";
            Assert.Equal(Expected5, MD.TeamName);

            string Expected6 = "StudentProjectClaimMetaData";
            Assert.Equal(Expected6, MD.SubType);

            Assert.Equal(MDBuilder.DEFAULT_BUDGET, MD.EntryBudget);

            Assert.Equal(MDBuilder.DEFAULT_ENTRYNAME, MD.EntryName);

            string Expected7 = "Student Project Claim";
            Assert.Equal(Expected7, MD.Policy);
        }

        [Fact]
        public void Build_ThrowErrorIfNullValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();

            // Act
            builder.SetEntryName("SCF-MAY2022-001")
            .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
            .SetPurpose("Cohort class bonding for cohort 02")
            .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES");
            // Missing TeamName of metadata

            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void SetClaimDate_ThrowErrorIfAhead()
        {
            // Arrange
            DateTime Value = DateTime.Now;
            Value.AddDays(1);
            StudentProjectClaimMDBuilder builder = new();

            // Assert
            Assert.Throws<ArgumentException>(() => builder.SetClaimDate(Value));
        }

        [Fact]
        public void SetEntryBudget_ThrowErrorForNegValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();

            // Act/Assert
            Assert.Throws<ArgumentException>(() => builder.SetEntryBudget(-232m));
        }

        [Fact]
        public void SetterForMetaData_ReturnCorrectSetValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;
            DateTime updatedTime = DateTime.ParseExact("26/06/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Act
            metadata = builder.SetEntryName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetTeamName("RaisinStudios")
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;

            MD.ClaimName = "Claim1";
            MD.EntryName = "Entry1";
            MD.EntryBudget = 3432.232m;
            MD.Policy = "Other Student Claim";
            MD.ProjectClub = "CompStruc";
            MD.TeamName = "ChestnutStudios";
            MD.ClaimDate = updatedTime;

            // Assert
            Assert.Equal("Claim1", MD.ClaimName);
            Assert.Equal("Entry1", MD.EntryName);
            Assert.Equal(3432.232m, MD.EntryBudget);
            Assert.Equal("Other Student Claim", MD.Policy);
            Assert.Equal("CompStruc", MD.ProjectClub);
            Assert.Equal("ChestnutStudios", MD.TeamName);
            Assert.Equal(updatedTime, MD.ClaimDate);
        }

        [Fact]
        public void SetterEntryBudget_ThrowErrorForNegValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;

            // Act
            metadata = builder.SetEntryName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetTeamName("RaisinStudios")
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;

            // Assert
            Assert.Throws<ArgumentException>(() => MD.EntryBudget = -25432.23m);
        }

        [Fact]
        public void SetterClaimDate_ThrowErrorForDateTimeAhead()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;
            DateTime updatedDate = DateTime.Now;
            updatedDate.AddDays(1);

            // Act
            metadata = builder.SetEntryName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetTeamName("RaisinStudios")
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;


            // Assert
            Assert.Throws<ArgumentException>(() => MD.ClaimDate = updatedDate);
        }
    }
}