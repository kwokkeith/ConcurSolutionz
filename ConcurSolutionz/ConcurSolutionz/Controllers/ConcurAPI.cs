using ESC_HTTP_Call.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ESC_HTTP_Call
{
    internal class ConcurAPI
    {
        public static HttpClient HttpClient { get; private set; }
        private const string GraphQL = "https://www-us2.api.concursolutions.com/cds/graphql";
        private static string UserID, EMPKey;

        public void Initialize(string CookieToken = "")
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Add("cookie", CookieToken);

            //First query to create a dummy request to retrieve user ID
            Task<string> CreateReqTask = CreateRequest();
            CreateReqTask.Wait();
            string FirstQuery = CreateReqTask.Result;
            //Console.WriteLine("First Query: \n" + FirstQuery);
            if (FirstQuery.Contains("id") == false) throw new Exception("First query failed");
            FirstQuery = FirstQuery.Replace("\"", "");
            string RequestID = "";
            string[] JsonSplit = FirstQuery.Split(new char[] { ',', '{', '}' });

            for (int i = 0; i < JsonSplit.Length; i++)
            {
                if (JsonSplit[i].Contains("id"))
                {
                    RequestID = JsonSplit[i].Split(":")[1];
                    break;
                }
            }
            Console.WriteLine("Request ID: \n" + RequestID);

            //Retrieve user ID
            Task<string> UserIDTask = GetUserID(RequestID);
            UserIDTask.Wait();
            //Console.WriteLine("UserID Response: \n" + UserIDTask.Result);

            string SecondQuery = UserIDTask.Result;
            if (SecondQuery.Contains("id") == false) throw new Exception("Second query failed");
            SecondQuery = SecondQuery.Replace("\"", "");
            JsonSplit = SecondQuery.Split(new char[] { ',', '{', '}' });
            for (int i = 0; i < JsonSplit.Length; i++)
            {
                //Console.WriteLine(JsonSplit[i]);
                if (JsonSplit[i].Contains("empUUID"))
                {
                    //Console.WriteLine("empUUID Found");
                    UserID = JsonSplit[i].Split(":")[1];
                }
                else if (JsonSplit[i].Contains("empId"))
                {
                    //Console.WriteLine("empId Found");
                    EMPKey = JsonSplit[i].Split(":")[1];
                }
            }
            Console.WriteLine("User ID: " + UserID + "\n");
            Console.WriteLine("EMP Key: " + EMPKey + "\n");


            //Delete dummy request
            Task<string> DelReqTask = DeleteRequest(RequestID);
            CreateReqTask.Wait();
            Console.WriteLine("Del Response: \n" + DelReqTask.Result);
            Console.WriteLine("End of Init\n\n");
        }

        private async Task<string> CreateRequest()
        {
            string content = @"{
                ""operationName"": ""CreateRequestHeader"",
                ""variables"": {
                ""values"": {
                ""customData"": [
                    {
                      ""id"": ""custom1"",
                      ""value"": """"
                    }
                 ],
                ""policyId"": ""9C614675927C294DB713360F151FEEC2"",
                  ""name"": ""dummyrequest"",
                 ""startDate"": null,
                ""endDate"": null,
                ""businessPurpose"": """",
                ""comment"": """"
                }
             },
             ""query"": ""mutation CreateRequestHeader($values: TravelRequestRequestInput!) {\n  travelRequest {\n    request {\n      create(input: $values) {\n        id\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}""
            }";

            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(GraphQL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetUserID(string RequestID = "")
        {
            const string RequestURL = "https://www-us2.api.concursolutions.com/travelrequest/graphql";
            string content = @"{
                ""operationName"": ""requestDetailsQuery"",
                ""variables"": {
                    ""withAgencyEstimatedFees"": false,
                    ""withInvoiceReports"": false,
                    ""requestId"": ""<REQUESTID>"",
                    ""role"": ""REQ_TRAVELER""
                },
                ""query"": ""query requestDetailsQuery($requestId: ID!, $role: String, $withAgencyEstimatedFees: Boolean = false, $entityId: ID, $withInvoiceReports: Boolean = false) {\n  requestDetails(requestId: $requestId, role: $role) {\n    id\n    key\n    requestType {\n      code\n      name\n      __typename\n    }\n    eventShortId\n    shortId\n    empId\n    firstName\n    lastName\n    empUUID\n    policyId\n    policyKey\n    name\n    purpose\n    totalPostedAmount\n    crnKey\n    alphaCode\n    custom6Code\n    isNotFiled\n    isReceiptImageAvailable\n    imagingRequestId\n    allocationFormId\n    startDate\n    endDate\n    apsKey\n    approvalLimitDate\n    approvalStatus\n    mainPnr\n    tripId\n    tripUuid\n    isUserReviewed\n    hasAgencyProposals\n    containerPnr\n    agencyProposalType\n    displayProposalLink\n    isPrepForAprvl\n    hasSelectedProposal\n    hasLineItemsConfigured\n    isSubmitted\n    isApproved\n    maxRiskLevel\n    noOfParticipants\n    invoiceReportsList @include(if: $withInvoiceReports) {\n      totalCount\n      totalAmount\n      totalCrnKey\n      reports {\n        reportId\n        apvStatusName\n        crnCode\n        invoiceDate\n        name\n        amountApproved\n        totalApprovedAmount\n        amountClaimed\n        totalClaimedAmount\n        __typename\n      }\n      __typename\n    }\n    agencyEstimatedFees(requestId: $requestId) @include(if: $withAgencyEstimatedFees) {\n      upToDate\n      __typename\n    }\n    comments {\n      comment\n      commentByFirstName\n      commentByLastName\n      creationDate\n      isLatest\n      commentKey\n      __typename\n    }\n    workflow {\n      instanceId\n      currentStep\n      __typename\n    }\n    printFormatConfigs {\n      agcBookedFilter\n      alwaysAppendCurrency\n      alwaysAppendCurrencyAsBoolean\n      contentVariable {\n        hasBarCode\n        ptCode\n        __typename\n      }\n      includeReportDetails\n      includeRptDetailsAsEnum\n      pfcCode\n      printFormatConfigLang {\n        instructions\n        name\n        pfcCode\n        __typename\n      }\n      prodCode\n      ptCode\n      __typename\n    }\n    segmentsLocTypes {\n      locTypes {\n        locTypeCode\n        name\n        __typename\n      }\n      segmentTypeKey\n      __typename\n    }\n    totalRemainingAmount\n    approverId\n    __typename\n  }\n  expectedExpenses(\n    requestId: $requestId\n    role: $role\n    entityId: $entityId\n    withAgencyEstimatedFees: $withAgencyEstimatedFees\n  ) {\n    id\n    key\n    segmentIds\n    fromSegment\n    type\n    transactionDate\n    description\n    tripType\n    autoCreated\n    fromLocation {\n      locationName\n      locName\n      iataCode\n      airportName\n      __typename\n    }\n    toLocation {\n      locationName\n      locName\n      iataCode\n      airportName\n      __typename\n    }\n    postedAmount\n    transactionAmount\n    crnKey\n    crnCode\n    comments {\n      entryId\n      segmentId\n      comment {\n        comment\n        commentByFirstName\n        commentByLastName\n        creationDate\n        __typename\n      }\n      __typename\n    }\n    formId\n    customFormId\n    expId\n    formType\n    allocationVersion\n    expName\n    approvedAmount\n    segmentStartDate\n    segmentTypeId\n    dailyAllowanceId\n    isSelfBooked\n    attendeeCount\n    isAutoEstimatedAgencyFees\n    allocationState\n    isSoftDeleted\n    costObjectsForApprover {\n      pendingUser\n      actedOnByUser\n      approvedAmount\n      __typename\n    }\n    __typename\n  }\n}""
            }".Replace("<REQUESTID>", RequestID);

            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> DeleteRequest(string RequestID = "")
        {
            string RequestURL = "https://www-us2.api.concursolutions.com/cds/graphql";
            string content = @"{
                ""operationName"": ""deleteRequest"",
                ""variables"": {
                    ""requestId"": ""<REQUESTID>""
                },
                ""query"": ""mutation deleteRequest($requestId: ID!) {\n  travelRequest {\n    request {\n      delete(requestId: $requestId) {\n        requestId\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}""
            }".Replace("<REQUESTID>", RequestID);

            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllClaims()
        {
            string RequestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{
                ""operationName"": ""GetReportsForUser"",
                ""variables"": {
                    ""dateRange"": null,
                  ""filterByStatus"": ""ACTIVE"",
                   ""paging"": {
                       ""page"": 1,
                       ""size"": 100
                   },
                    ""sortBy"": null,
                     ""sortDirection"": null,
                 ""userId"": ""<userId>"",
                 ""contextRole"": ""TRAVELER""
              },
              ""query"": ""query GetReportsForUser($contextRole: ContextRoleType!, $dateRange: InputDateRange, $filterByStatus: InputFilterBy, $paging: InputPagination, $sortBy: InputSortBy, $sortDirection: InputSortDirection, $userId: String!) {\n  employee(contextRole: $contextRole, userId: $userId) {\n    userId\n    contextRole\n    reportsForUser(\n      input: {dateRange: $dateRange, filterByStatus: $filterByStatus, paging: $paging, sortBy: $sortBy, sortDirection: $sortDirection}\n    ) {\n      reports: list {\n        reportId: id\n        approvedAmount {\n          currencyCode\n          value\n          __typename\n        }\n        approvalStatus\n        approver {\n          firstName: first\n          lastName: last\n          __typename\n        }\n        canAddExpense\n        claimedAmount {\n          currencyCode\n          value\n          __typename\n        }\n        endDate\n        exceptionLevel\n        isMarkedForReviewByDelegate\n        isApproved\n        isPendingApproval\n        isSentBack\n        wasSentForPayment: isSentForPayment\n        isSubmitted\n        isReopened\n        name\n        paymentStatus\n        reportNumber\n        reportDate\n        reportTotal {\n          currencyCode\n          value\n          __typename\n        }\n        reportType\n        sentBackDate\n        startDate\n        submitDate\n        rptKey\n        totalAmountDueEmployee {\n          currencyCode\n          value\n          __typename\n        }\n        __typename\n      }\n      pagination {\n        ...PaginationFragment\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n\nfragment PaginationFragment on PaginationResponse {\n  number\n  size\n  totalElements\n  totalPages\n  __typename\n}""
            }".Replace("<userId>", UserID);

            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();

            //TODO: Parse data into individual claim classes
        }

        public async Task<List<ClaimPolicy>> ClaimCreateDD()
        {
            string RequestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{
            ""operationName"": ""GetFormListItems"",
             ""variables"": {
               ""listInformation"": {
                 ""id"": ""7e509237ae3e8749b8bcffe41239cda8"",
                 ""searchBy"": ""TEXT"",
                 ""searchByCriteria"": ""*"",
                 ""isExternal"": null
                },
                ""sessionId"": null
              },
              ""query"": ""query GetFormListItems($sessionId: ID, $listInformation: CDS_InputListInformation!) {\n  CDS_spend {\n    list(sessionId: $sessionId, listInformation: $listInformation) {\n      isExternal\n      items {\n        code\n        id\n        matchValue\n        serviceVersion\n        code\n        shortCode\n        value\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n""
            }
            ";
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);
            string JsonString = await response.Content.ReadAsStringAsync();

            JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(JsonString);
            JsonArray jsonArr = jsonObject!["data"]!["CDS_spend"]!["list"]!["items"]!.AsArray();
            List<ClaimPolicy> Policies = new List<ClaimPolicy>();
            for (int i = 0; i < jsonArr.Count; i++)
            {
                Policies.Add(item: jsonArr[i].Deserialize<ClaimPolicy>());
            }
            return Policies;


            //TODO: Parse data into individual dropdown classes
        }

        public async Task<string> CreateClaim(Models.Claim claim)
        {
            string requestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";

            string content = @"{
            ""operationName"": ""CreateReportHeader"",
            ""variables"": {
             ""userId"": ""<userId>"",
             ""contextRole"": ""TRAVELER"",
             ""fields"": {
              ""customData"": [
               {
                ""id"": ""custom15"",
                ""value"": ""EE77D2E31E8F0646B31BC8285F0DFC36""
               },
               {
                ""id"": ""custom18"",
                ""value"": ""UGRD(ISTD)(Junior)""
               },
               {
                ""id"": ""custom1"",
                ""value"": ""<Policy>""
               },
               {
                ""id"": ""custom10"",
                ""value"": ""<teamName>""
               },
               {
                ""id"": ""custom2"",
                ""value"": """"
               },
               {
                ""id"": ""custom3"",
                ""value"": """"
               },
               {
                ""id"": ""custom4"",
                ""value"": """"
               },
               {
                ""id"": ""custom5"",
                ""value"": """"
               },
               {
                ""id"": ""custom6"",
                ""value"": ""05E95E772336C64A83545D65E9675DB3""
               },
               {
                ""id"": ""custom7"",
                ""value"": """"
               },
               {
                ""id"": ""custom8"",
                ""value"": """"
               },
               {
                ""id"": ""custom9"",
                ""value"": """"
               },
               {
                ""id"": ""custom12"",
                ""value"": ""4BE7072EC2D59649A155F332FF85D9E7""
               },
               {
                ""id"": ""orgUnit2"",
                ""value"": ""38CB88342DA2C54BB7B69565039089D0""
               },
               {
                ""id"": ""orgUnit3"",
                ""value"": ""0CEB57C34C733F43B5CE3EF67432FDE2""
               },
               {
                ""id"": ""orgUnit4"",
                ""value"": """"
               }
              ],
              ""policyId"": ""B6C67F6554CEB241AA2DF02D8757CBE6"",
              ""name"": ""<claimName>"",
              ""reportDate"": ""<Date>"",
              ""businessPurpose"": ""<Purpose>"",
              ""countryCode"": ""SG"",
              ""reportSource"": ""WEB""
             }
            },
            ""query"": ""mutation CreateReportHeader($contextRole: CDS_SmartExpenseContextTypes, $fields: CDS_InputReportFieldsCreate!, $userId: ID) {\n  CDS_expense {\n    report {\n      create(contextType: $contextRole, fields: $fields, userId: $userId) {\n        status\n        response {\n          id\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n""
            }".Replace("<userId>", UserID).Replace("<teamName>", claim.TeamName).Replace("<claimName>", claim.Name).Replace("<Policy>", claim.Policy).Replace("<Purpose>", claim.Purpose).Replace("<Date>", claim.Date);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<String> CreateExpense(Expense expense)
        {
            string RequestURL = "https://www-us2.api.concursolutions.com/nui-expense/graphql";
            //TODO: Find rptKey based on reportId
            string reportKey = await GetReportKey(expense.ReportId);
            string content = @"{""query"":""\n      mutation saveExpense {\n        saveExpense (\n          payload: \""{\\\""foreignOrDomestic\\\"":\\\""HOME\\\"",\\\""hasVat\\\"":\\\""N\\\"",\\\""exceptionCount\\\"":0,\\\""receiptRequired\\\"":\\\""N\\\"",\\\""receiptReceived\\\"":\\\""N\\\"",\\\""lnKey\\\"":22895,\\\""locationName\\\"":\\\""Singapore, SINGAPORE\\\"",\\\""isPersonal\\\"":\\\""N\\\"",\\\""isClearedExceptions\\\"":\\\""N\\\"",\\\""imageRequired\\\"":\\\""N\\\"",\\\""hasExceptions\\\"":\\\""N\\\"",\\\""hasMobileReceipt\\\"":\\\""N\\\"",\\\""locName\\\"":\\\""Singapore, SINGAPORE\\\"",\\\""ccLocationResolved\\\"":\\\""Y\\\"",\\\""exceptionMaxLevel\\\"":0,\\\""exchangeRate\\\"":1,\\\""receiptType\\\"":\\\""T\\\"",\\\""travelAllowance\\\"":\\\""N\\\"",\\\""isBillable\\\"":\\\""N\\\"",\\\""hasTimestamp\\\"":\\\""N\\\"",\\\""allocationState\\\"":\\\""N\\\"",\\\""claimedAmount\\\"":0,\\\""exchangeRateDirection\\\"":\\\""M\\\"",\\\""expKey\\\"":\\\""01128\\\"",\\\""expName\\\"":\\\""Student Project-Material & Supplies\\\"",\\\""formKey\\\"":1228,\\\""patKey\\\"":\\\""CASH\\\"",\\\""transactionCurrencyName\\\"":\\\""SGD\\\"",\\\""rejected\\\"":\\\""N\\\"",\\\""description\\\"":\\\""<Description>\\\"",\\\""transactionDate\\\"":\\\""<Date>\\\"",\\\""hotelCheckoutDate\\\"":\\\""<Date>\\\"",\\\""undefined\\\"":\\\""<Date>\\\"",\\\""vendorDescription\\\"":\\\""<Supplier>\\\"",\\\""transactionAmount\\\"":<Cost>,\\\""postedAmount\\\"":<Cost>,\\\""custom1\\\"":\\\""<ReceiptNo>\\\"",\\\""comment\\\"":\\\""<Comment>\\\"",\\\""parRpeKey\\\"":\\\""\\\"",\\\""empKey\\\"":\\\""<empKey>\\\"",\\\""polKey\\\"":\\\""1056\\\"",\\\""rptKey\\\"":\\\""<rptKey>\\\"",\\\""crnKey\\\"":129,\\\""reportEntryTaxes\\\"":[{\\\""taxAuthKey\\\"":\\\""2045\\\"",\\\""taxFormKey\\\"":\\\""1314\\\"",\\\""custom1\\\"":\\\""103427\\\""}],\\\""hasAttendees\\\"":\\\""N\\\"",\\\""attendees\\\"":{\\\""clearExisting\\\"":\\\""Y\\\"",\\\""list\\\"":[]}}\"",\n          mrusToSave: \""\"",\n          patKey: \""\"",\n          pctKey: \""\"",\n          parRpeKey: \""\"",\n          reportId: \""<reportId>\"",\n          expenseId: \""null\"",\n          shouldRecalculateActualsVsLimitsAllowances: false,\n          userId: \""<userId>\"",\n          contextRole: \""TRAVELER\""\n        ) {\n          status,\n          expense {\n            rptKey\n            rpeKey\n            expenseId\n          }\n        }\n      }\n    ""}"
            .Replace("<userId>", UserID).Replace("<empKey>", EMPKey).Replace("<reportId>", expense.ReportId).Replace("<rptKey>", reportKey).Replace("<Description>", expense.Description).Replace("<Date>", expense.Date).Replace("<Supplier>", expense.Supplier).Replace("<Cost>", expense.Cost.ToString()).Replace("<ReceiptNo>", expense.ReceiptNo).Replace("<Comment>", expense.Comment);
            //string content = @"{""query"":""\n      mutation saveExpense {\n        saveExpense (\n          payload: \""{\\\""foreignOrDomestic\\\"":\\\""HOME\\\"",\\\""hasVat\\\"":\\\""N\\\"",\\\""exceptionCount\\\"":0,\\\""receiptRequired\\\"":\\\""N\\\"",\\\""receiptReceived\\\"":\\\""N\\\"",\\\""lnKey\\\"":22895,\\\""locationName\\\"":\\\""Singapore, SINGAPORE\\\"",\\\""isPersonal\\\"":\\\""N\\\"",\\\""isClearedExceptions\\\"":\\\""N\\\"",\\\""imageRequired\\\"":\\\""N\\\"",\\\""hasExceptions\\\"":\\\""N\\\"",\\\""hasMobileReceipt\\\"":\\\""N\\\"",\\\""locName\\\"":\\\""Singapore, SINGAPORE\\\"",\\\""ccLocationResolved\\\"":\\\""Y\\\"",\\\""exceptionMaxLevel\\\"":0,\\\""exchangeRate\\\"":1,\\\""receiptType\\\"":\\\""T\\\"",\\\""travelAllowance\\\"":\\\""N\\\"",\\\""isBillable\\\"":\\\""N\\\"",\\\""hasTimestamp\\\"":\\\""N\\\"",\\\""allocationState\\\"":\\\""N\\\"",\\\""claimedAmount\\\"":0,\\\""exchangeRateDirection\\\"":\\\""M\\\"",\\\""expKey\\\"":\\\""01128\\\"",\\\""expName\\\"":\\\""Student Project-Material & Supplies\\\"",\\\""formKey\\\"":1228,\\\""patKey\\\"":\\\""CASH\\\"",\\\""transactionCurrencyName\\\"":\\\""SGD\\\"",\\\""rejected\\\"":\\\""N\\\"",\\\""description\\\"":\\\""testdesc\\\"",\\\""transactionDate\\\"":\\\""2023-04-06\\\"",\\\""hotelCheckoutDate\\\"":\\\""2023-04-06\\\"",\\\""undefined\\\"":\\\""2023-04-06\\\"",\\\""vendorDescription\\\"":\\\""testsupp\\\"",\\\""transactionAmount\\\"":420,\\\""postedAmount\\\"":420,\\\""custom1\\\"":\\\""testreceiptno\\\"",\\\""comment\\\"":\\\""testcomment\\\"",\\\""parRpeKey\\\"":\\\""\\\"",\\\""empKey\\\"":\\\""gWgMdOIxPStLIMTLxuhKY9Vukxi1a\\\"",\\\""polKey\\\"":\\\""1056\\\"",\\\""rptKey\\\"":\\\""gWlZecK$s2NkK5ZklflHv7MO$pslFQSUQ\\\"",\\\""crnKey\\\"":129,\\\""reportEntryTaxes\\\"":[{\\\""taxAuthKey\\\"":\\\""2045\\\"",\\\""taxFormKey\\\"":\\\""1314\\\"",\\\""custom1\\\"":\\\""103427\\\""}],\\\""hasAttendees\\\"":\\\""N\\\"",\\\""attendees\\\"":{\\\""clearExisting\\\"":\\\""Y\\\"",\\\""list\\\"":[]}}\"",\n          mrusToSave: \""\"",\n          patKey: \""\"",\n          pctKey: \""\"",\n          parRpeKey: \""\"",\n          reportId: \""79589DD75811429E8F07\"",\n          expenseId: \""null\"",\n          shouldRecalculateActualsVsLimitsAllowances: false,\n          userId: \""\"",\n          contextRole: \""TRAVELER\""\n        ) {\n          status,\n          expense {\n            rptKey\n            rpeKey\n            expenseId\n          }\n        }\n      }\n    ""}";
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");//
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<String> GetReportKey(string reportId)
        {
            string RequestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{""operationName"":""GetReportIdAndKey"",""variables"":{""userId"":""<userId>"",""contextRole"":""TRAVELER"",""rptKey"":null,""reportId"":""<reportId>""},""query"":""query GetReportIdAndKey($userId: String, $contextRole: ContextRoleType, $rptKey: String, $reportId: String) {\n  employee(userId: $userId, contextRole: $contextRole) {\n    userId\n    contextRole\n    expenseReport(reportId: $reportId, rptKey: $rptKey) {\n      reportId\n      rptKey\n      __typename\n    }\n    __typename\n  }\n}\n""}"
            .Replace("<userId>", UserID).Replace("<reportId>", reportId);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);

            string JsonString = await response.Content.ReadAsStringAsync();
            JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(JsonString);
            Console.WriteLine(jsonObject!["data"]!["employee"]!["expenseReport"]!["rptKey"]!);
            return jsonObject.ToString();
        }

        public async Task<string> UploadImage(string ImagePath)
        {
            string RequestUrl = "https://us2.concursolutions.com/expense/expenseDotNet/Receipts/UploadLineItemImage.ashx?empKey=" + EMPKey + "&role=TRAVELER";
            byte[] ImageBinary = File.ReadAllBytes(ImagePath);
            ByteArrayContent ImageContent = new ByteArrayContent(ImageBinary);
            MultipartFormDataContent HttpContent = new MultipartFormDataContent();
            HttpContent.Add(ImageContent, "File", "Playmaker.png");
            var response = await HttpClient.PostAsync(RequestUrl, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<String> LinkClaimToRequest(Expense expense)
        {
            string RequestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{""operationName"":""AttachImageMutation"",""variables"":{""userId"":""<userId>"",""contextRole"":""TRAVELER"",""reportId"":""<reportId>"",""expenseId"":""<expenseId>"",""imageId"":""<imageId>""},""query"":""mutation AttachImageMutation($userId: String!, $contextRole: ContextRoleType!, $reportId: String!, $expenseId: String!, $imageId: String!) {\n  employee(userId: $userId, contextRole: $contextRole) {\n    expenseReport(reportId: $reportId) {\n      entry(expenseId: $expenseId) {\n        attachImage(imageId: $imageId) {\n          id\n          receiptImageId\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n""}"
            .Replace("<userId>", UserID).Replace("<reportId>", expense.ReportId).Replace("<expenseId>", expense.Id).Replace("<imageId>", expense.ImageId);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(RequestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }


    }
}


/*          Dynamic Obj Ref
          
            //dynamic payload = new JObject();
            //payload.operationName = "GetReportsForUser";
            //payload.variables = new JObject();
            //payload.variables.dateRange = null;
            //payload.variables.filterByStatus = "ACTIVE";
            //payload.variables.paging = new JObject();
            //payload.variables.paging.page = 1;
            //payload.variables.paging.size = 100;
            //payload.variables.sortBy = null;
            //payload.variables.sortDirection = null;
            //payload.variables.userId = "ff242cd1-57c7-47fb-bb33-9ad1edb131c0";
            //payload.variables.contextRole = "TRAVELER";
            //payload.query = @"query GetReportsForUser($contextRole: ContextRoleType!, $dateRange: InputDateRange, $filterByStatus: InputFilterBy, $paging: InputPagination, $sortBy: InputSortBy, $sortDirection: InputSortDirection, $userId: String!) {  employee(contextRole: $contextRole, userId: $userId) {    userId    contextRole    reportsForUser(      input: {dateRange: $dateRange, filterByStatus: $filterByStatus, paging: $paging, sortBy: $sortBy, sortDirection: $sortDirection}    ) {      reports: list {        reportId: id        approvedAmount {          currencyCode          value          __typename        }        approvalStatus        approver {          firstName: first          lastName: last          __typename        }        canAddExpense        claimedAmount {          currencyCode          value          __typename        }        endDate        exceptionLevel        isMarkedForReviewByDelegate        isApproved        isPendingApproval        isSentBack        wasSentForPayment: isSentForPayment        isSubmitted        isReopened        name        paymentStatus        reportNumber        reportDate        reportTotal {          currencyCode          value          __typename        }        reportType        sentBackDate        startDate        submitDate        rptKey        totalAmountDueEmployee {          currencyCode          value          __typename        }        __typename      }      pagination {        ...PaginationFragment        __typename      }      __typename    }    __typename  }}fragment PaginationFragment on PaginationResponse {  number  size  totalElements  totalPages  __typename}";
            //string content = JsonConvert.SerializeObject(payload)
*/