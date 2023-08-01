using System;
using System.Globalization;
using ConcurSolutionz.Database;
using Newtonsoft.Json.Linq;

namespace Unit_Test
{
    public class MetaDataTest
    {
        [Fact(DisplayName = "4.1")]
        public void Build_MetaDataShouldBuildUsingBuilder()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;

            // Act
            metadata = builder.SetClaimName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetTeamName("RaisinStudios")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
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

            string Expected6 = "ConcurSolutionz.Database.StudentProjectClaimMetaData";
            Assert.Equal(Expected6, MD.SubType);

            Assert.Equal(MDBuilder.DEFAULT_BUDGET, MD.EntryBudget);

            Assert.Equal(MDBuilder.DEFAULT_ENTRYNAME, MD.EntryName);

            string Expected7 = "Student Project Claim";
            Assert.Equal(Expected7, MD.Policy);
        }

        [Fact(DisplayName = "4.2")]
        public void Build_ThrowErrorIfNullValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();

            // Act
            builder.SetClaimName("SCF-MAY2022-001")
            .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
            .SetPurpose("Cohort class bonding for cohort 02")
            .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES");
            // Missing TeamName of metadata

            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact(DisplayName = "4.3")]
        public void SetClaimDate_ThrowErrorIfAhead()
        {
            // Arrange
            DateTime Value = DateTime.Now.AddDays(1);
            StudentProjectClaimMDBuilder builder = new();

            // Assert
            Assert.Throws<ArgumentException>(() => builder.SetClaimName("SCF-MAY2022-001")
                                    .SetClaimDate(Value));
        }

        [Fact(DisplayName = "4.4")]
        public void SetEntryBudget_ThrowErrorForNegValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();

            // Act/Assert
            Assert.Throws<ArgumentException>(() => builder.SetEntryBudget(-232m));
        }

        [Fact(DisplayName = "4.5")]
        public void SetterForMetaData_ReturnCorrectSetValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;
            DateTime updatedTime = DateTime.ParseExact("26/06/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            // Act
            metadata = builder.SetClaimName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetTeamName("RaisinStudios")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetEntryName("Entry 1")
                .SetEntryBudget(100m)
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;

            MD.ClaimName = "Claim1";
            MD.ClaimDate = updatedTime;
            MD.Purpose = "Cohort class bonding for cohort 02";
            MD.TeamName = "ChestnutStudios";
            MD.ProjectClub = "CompStruc";
            MD.EntryName = "Entry1";
            MD.EntryBudget = 100m;
            MD.Policy = "Other Student Claim";

            // Assert
            Assert.Equal("Claim1", MD.ClaimName);
            Assert.Equal("Entry1", MD.EntryName);
            Assert.Equal(100m, MD.EntryBudget);
            Assert.Equal("Other Student Claim", MD.Policy);
            Assert.Equal("CompStruc", MD.ProjectClub);
            Assert.Equal("ChestnutStudios", MD.TeamName);
            Assert.Equal(updatedTime, MD.ClaimDate);
            Assert.Equal("Cohort class bonding for cohort 02", MD.Purpose);

        }

        [Fact(DisplayName = "4.6")]
        public void SetterEntryBudget_ThrowErrorForNegValue()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;

            // Act
            metadata = builder.SetClaimName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetTeamName("RaisinStudios")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetEntryName("SCF-MAY2022-001")
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;

            // Assert
            Assert.Throws<ArgumentException>(() => MD.EntryBudget = -25432.23m);
        }

        [Fact(DisplayName = "4.7")]
        public void SetterClaimDate_ThrowErrorForDateTimeAhead()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;
            DateTime updatedDate = DateTime.Now.AddDays(1);

            // Act
            metadata = builder.SetClaimName("SCF-MAY2022-001")
                .SetClaimDate(DateTime.ParseExact("06/05/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Cohort class bonding for cohort 02")
                .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                .SetTeamName("RaisinStudios")
                .Build();

            StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;


            // Assert
            Assert.Throws<ArgumentException>(() => MD.ClaimDate = updatedDate);
        }

        [Fact(DisplayName = "4.8")]
        public void Build_MetaDataShouldBuildUsingBuilder_Fuzz()
        {
            // Arrange
            StudentProjectClaimMDBuilder builder = new();
            MetaData metadata;
            string claimName = Fuzzer.GenerateRandomString(10);
            string purpose = Fuzzer.GenerateRandomString(20);
<<<<<<< HEAD
            DateTime claimDate = Fuzzer.GenerateRandomDateTime();
=======
            string claimDate = Fuzzer.GenerateRandomDateTime().ToString();
>>>>>>> 954a28e (feat: dark mode updated)
            string teamName = Fuzzer.GenerateRandomString(10);

            // Act
            if (claimDate > DateTime.Now)
            {
                Assert.Throws<ArgumentException>(() => builder.SetClaimDate(claimDate));
            }

            else
            {
                metadata = builder.SetClaimName(claimName)
                    .SetClaimDate(claimDate)
                    .SetPurpose(purpose)
                    .SetTeamName(teamName)
                    .SetProjectClub("(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES")
                    .Build();

                StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)metadata;

                // Assert
                string Expected1 = "(PP-00074-E0901-E0901-002) COMPUTATION STRUCTURES";
                Assert.Equal(Expected1, MD.ProjectClub);

                DateTime Expected2 = claimDate;
                Assert.Equal(Expected2, MD.ClaimDate);

                string Expected3 = claimName;
                Assert.Equal(Expected3, MD.ClaimName);

                string Expected4 = purpose;
                Assert.Equal(Expected4, MD.Purpose);

                string Expected5 = teamName;
                Assert.Equal(Expected5, MD.TeamName);

                string Expected6 = "ConcurSolutionz.Database.StudentProjectClaimMetaData";
                Assert.Equal(Expected6, MD.SubType);

                Assert.Equal(MDBuilder.DEFAULT_BUDGET, MD.EntryBudget);

                Assert.Equal(MDBuilder.DEFAULT_ENTRYNAME, MD.EntryName);

                string Expected7 = "Student Project Claim";
                Assert.Equal(Expected7, MD.Policy);
            }
        }
    }
}