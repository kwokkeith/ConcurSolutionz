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
        private string cookie = $"";

        [Fact]
        public void ValidClaim() 
        {
            ConcurAPI concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            if (!init.Equals("0")) Assert.True(false, "Failed to init");

            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-15";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                task.Wait();
                claim.Id = task.Result;
                task = concur.GetReportKey(claim.Id);
                task.Wait();
                claim.Key = task.Result;
                //Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }

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
                task = concur.CreateExpense(expense, claim);
                task.Wait();
                expense.RPEKey = task.Result;
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
            Assert.True(true);
        }

        [Fact]
        public void InvalidClaimDate()
        {
            ConcurAPI concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            if (!init.Equals("0")) Assert.True(false, "Failed to init");

            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2099-06-15";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                task.Wait();
                claim.Id = task.Result;
                task = concur.GetReportKey(claim.Id);
                task.Wait();
                claim.Key = task.Result;
                //Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(true, ex.Message);
            }
        }

        [Fact]
        public void InvalidExpenseDate()
        {
            ConcurAPI concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            if (!init.Equals("0")) Assert.True(false, "Failed to init");

            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-15";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                task.Wait();
                claim.Id = task.Result;
                task = concur.GetReportKey(claim.Id);
                task.Wait();
                claim.Key = task.Result;
                //Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }

            //Create new expense
            Expense expense = new Expense();
            expense.Date = "2099-06-21";
            expense.Cost = (decimal)69.00;
            expense.Description = "Test Description";
            expense.Supplier = "Test Supplier";
            expense.ReceiptNo = "Test Receipt";
            expense.Comment = "Test Comment";
            expense.ReportId = claim.Id;
            try
            {
                task = concur.CreateExpense(expense, claim);
                task.Wait();
                expense.RPEKey = task.Result;
            }
            catch (Exception ex)
            {
                Assert.True(true, ex.Message);
            }
        }

        [Fact]
        public void InvalidPolicy()
        {
            ConcurAPI concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            if (!init.Equals("0")) Assert.True(false, "Failed to init");

            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-15";
            claim.Policy = "SOME INVALID POLICY"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                task.Wait();
                claim.Id = task.Result;
                task = concur.GetReportKey(claim.Id);
                task.Wait();
                claim.Key = task.Result;
                Assert.True(false, "Created claim despite invalid policy");
            }
            catch (Exception ex)
            {
                Assert.True(true);
            }
        }
        [Fact]
        public void FuzzerName()
        {
            ConcurAPI concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            if(!init.Equals("0")) Assert.True(false, "Failed to init");


            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = Fuzzer.GenerateRandomString(10,true);
            claim.Date = "2023-06-15";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                task.Wait();
                claim.Id = task.Result;
                task = concur.GetReportKey(claim.Id);
                task.Wait();
                claim.Key = task.Result;
                //Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, "Invalid claim name from fuzzer");
            }

            //Create new expense
            Expense expense = new Expense();
            expense.Date = "2099-06-21";
            expense.Cost = (decimal)69.00;
            expense.Description = Fuzzer.GenerateRandomString(10, true); 
            expense.Supplier = Fuzzer.GenerateRandomString(10, true);
            expense.ReceiptNo = Fuzzer.GenerateRandomString(10, true);
            expense.Comment = Fuzzer.GenerateRandomString(100, true); ;
            expense.ReportId = claim.Id;
            try
            {
                task = concur.CreateExpense(expense, claim);
                task.Wait();
                expense.RPEKey = task.Result;
            }
            catch (Exception ex)
            {
                Assert.True(false, "Invalid expense inputs from fuzzer");
            }
            Assert.True(true);
        }
        [Fact]
        public void FuzzerAmount()
        {
            ConcurAPI concur = new ConcurAPI(cookie);
            Task<string> task = concur.Initialize();
            task.Wait();
            string init = task.Result;
            if (!init.Equals("0")) Assert.True(false, "Failed to init");

            //Create new claim
            ConcurSolutionz.Models.Claim claim = new ConcurSolutionz.Models.Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-15";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            try
            {
                task = concur.CreateClaim(claim);//Create a claim on concur and obtain the id
                task.Wait();
                claim.Id = task.Result;
                task = concur.GetReportKey(claim.Id);
                task.Wait();
                claim.Key = task.Result;
                //Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }

            //Create new expense
            Expense expense = new Expense();
            expense.Date = "2099-06-21";
            expense.Cost = (decimal)Fuzzer.GenerateRandomDouble(5,2);
            expense.Description = "Test Description";
            expense.Supplier = "Test Supplier";
            expense.ReceiptNo = "Test Receipt";
            expense.Comment = "Test Comment";
            expense.ReportId = claim.Id;
            try
            {
                task = concur.CreateExpense(expense, claim);
                task.Wait();
                expense.RPEKey = task.Result;
            }
            catch (Exception ex)
            {
                Assert.True(true, ex.Message);
            }
        }
    }
}
