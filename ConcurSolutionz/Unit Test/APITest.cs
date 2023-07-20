using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ConcurSolutionz;
using ConcurSolutionz.Controllers;
using ConcurSolutionz.Models;

namespace Unit_Test
{
    [Collection("APITest")]
    public class APITest
    {
        private ConcurAPI concur;
        private ConcurSolutionz.Models.Claim claim;
        private Expense expense;

        [Theory]
        [InlineData("")]//Insert cookie here
        public void Init(string cookie) 
        {
            concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            Assert.True(init.Equals("0"));
        }

        [Fact]
        public void CreateClaim()
        {
            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                Task<string> task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                claim.Id = task.Result;
                this.claim = claim;
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void CreateExpense()
        {
            //Create new expense
            Expense expense = new Expense();
            expense.Date = "2023-06-21";
            expense.Cost = (decimal)69.00;
            expense.Description = "Test Description";
            expense.Supplier = "Test Supplier";
            expense.ReceiptNo = "Test Receipt";
            expense.Comment = "Test Comment";
            expense.ReportId = claim.Id;
            try
            {
                Task<string> task = concur.CreateExpense(expense, claim);
                expense.RPEKey = task.Result;
                this.expense = expense;
                Assert.True(true);
            }
            catch(Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
