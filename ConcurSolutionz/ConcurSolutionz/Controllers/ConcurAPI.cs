using ConcurSolutionz.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ConcurSolutionz
{
    public class ConcurAPI
    {
        public static HttpClient HttpClient { get; private set; }
        private static string UserID, EMPKey;


        public ConcurAPI(string cookieToken)
        {
            UserID = string.Empty;
            EMPKey = string.Empty;

            //Initialize HttpClient for obj
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Add("cookie", cookieToken);
        }


        /// <summary>
        /// Method <c>Initialize</c> retrieves the UserID and EMPKey to run the rest of the API calls
        /// </summary>
        public async Task<string> Initialize()
        {
            /* Code "0": Success
             * Error code "1": Failed to create request | Likely due to expired cookie
             * Error code "2": Failed to retrieve requestId
             * Error code "3": Failed to extract userID or EMPkey, delete request */

            //First query to create a dummy request to retrieve user ID
            string FirstQuery = await CreateRequest();

            //Retrieves requestId from response, if requestId not in response then failed to create request: error code 1
            if (FirstQuery.Contains("id") == false) return "1";
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

            //If RequestID is empty, error code 2
            if (string.IsNullOrEmpty(RequestID)) return "2";

            //Retrieve userId and empKey
            string SecondQuery = await GetUserID(RequestID);
            if (SecondQuery.Contains("id") == false)
            {

            }
            SecondQuery = SecondQuery.Replace("\"", "");
            JsonSplit = SecondQuery.Split(new char[] { ',', '{', '}' });
            for (int i = 0; i < JsonSplit.Length; i++)
            {
                if (JsonSplit[i].Contains("empUUID")) UserID = JsonSplit[i].Split(":")[1];
                else if (JsonSplit[i].Contains("empId")) EMPKey = JsonSplit[i].Split(":")[1];
            }

            //Delete dummy request
            await DeleteRequest(RequestID);

            //If UserId or EMPKey is empty, error code 3
            if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(EMPKey)) { return "3"; }
            else return "0";
        }


        /// <summary>
        /// Method <c>CreateRequest</c> is a helper method for Initialize() to make an API call for making a request to retrieve the userId
        /// </summary>
        private async Task<string> CreateRequest()
        {
            string requestURL = "https://www-us2.api.concursolutions.com/cds/graphql";
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
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// Method <c>GetUserID</c> is a helper method for Initialize() to retrieve the UserID and EMPKey from the request made
        /// </summary>
        private async Task<string> GetUserID(string RequestID = "")
        {
            const string requestURL = "https://www-us2.api.concursolutions.com/travelrequest/graphql";
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
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// Method <c>DeleteRequest</c> is a helper method for Initialize() to clean up the request made earlier
        /// </summary>
        private async Task<string> DeleteRequest(string RequestID = "")
        {
            string requestURL = "https://www-us2.api.concursolutions.com/cds/graphql";
            string content = @"{
                ""operationName"": ""deleteRequest"",
                ""variables"": {
                    ""requestId"": ""<REQUESTID>""
                },
                ""query"": ""mutation deleteRequest($requestId: ID!) {\n  travelRequest {\n    request {\n      delete(requestId: $requestId) {\n        requestId\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}""
            }".Replace("<REQUESTID>", RequestID);

            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            return await response.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// Method <c>GetAllClaims</c> retrieves all of the current user's claims and returns it as a list of Models.Claim
        /// </summary>
        public async Task<List<Models.Claim>> GetAllClaims()
        {
            string requestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
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
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            Models.Claim tempClaim = new Models.Claim();
            string jsonString = await response.Content.ReadAsStringAsync();
            try
            {
                JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(jsonString)!;
                JsonArray jsonArr = jsonObject!["data"]!["employee"]!["reportsForUser"]!["reports"]!.AsArray();

                //If no user created claims are found
                if (jsonArr.Count == 0) return new List<Models.Claim>();
                List<Models.Claim> Claims = new List<Models.Claim>();
                for (int i = 0; i < jsonArr.Count; i++)
                {
                    tempClaim.Id = jsonArr[i]!["reportId"]!.ToString();
                    tempClaim.Key = jsonArr[i]!["rptKey"]!.ToString();
                    tempClaim.Name = jsonArr[i]!["name"]!.ToString();
                    tempClaim.Date = jsonArr[i]!["reportDate"]!.ToString();
                    Claims.Add(tempClaim);
                }
                return Claims;
            }
            catch (Exception ex)
            {
               //Returns an empty list in the case of an error
                return new List<Models.Claim>();
            }
        }


        /// <summary>
        /// Method <c>ClaimCreateDD</c> retrieves all possible claim policies for the user to select when creating a new claim
        /// </summary>
        public async Task<List<ClaimPolicy>> ClaimCreateDD()
        {
            string requestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
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
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            string jsonString = await response.Content.ReadAsStringAsync();
            try
            {
                JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(jsonString)!;
                JsonArray jsonArr = jsonObject!["data"]!["CDS_spend"]!["list"]!["items"]!.AsArray();
                List<ClaimPolicy> Policies = new List<ClaimPolicy>();
                for (int i = 0; i < jsonArr.Count; i++)
                {
                    Policies.Add(item: jsonArr[i].Deserialize<ClaimPolicy>()!);
                }
                return Policies;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve policy claims");
            }
        }


        /// <summary>
        /// Method <c>CreateClaim</c> creates a claim using the object Models.CLaim
        /// </summary>
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
            string jsonString = await response.Content.ReadAsStringAsync();
            if (jsonString.Contains("COMPLETED"))
            {
                JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(jsonString)!;
                return jsonObject["data"]!["CDS_expense"]!["report"]!["create"]!["response"]!["id"]!.ToString();
            }
            else throw new Exception("Failed to create claim");
        }


        /// <summary>
        /// Method <c>CreateExpense</c> create an expense under a claim, taking in a expense and a claim that the expense belongs under as arguments
        /// </summary>
        public async Task<String> CreateExpense(Expense expense, Models.Claim claim)
        {
            string requestURL = "https://www-us2.api.concursolutions.com/nui-expense/graphql";
            string content = @"{""query"":""\n      mutation saveExpense {\n        saveExpense (\n          payload: \""{\\\""foreignOrDomestic\\\"":\\\""HOME\\\"",\\\""hasVat\\\"":\\\""N\\\"",\\\""exceptionCount\\\"":0,\\\""receiptRequired\\\"":\\\""N\\\"",\\\""receiptReceived\\\"":\\\""N\\\"",\\\""lnKey\\\"":22895,\\\""locationName\\\"":\\\""Singapore, SINGAPORE\\\"",\\\""isPersonal\\\"":\\\""N\\\"",\\\""isClearedExceptions\\\"":\\\""N\\\"",\\\""imageRequired\\\"":\\\""N\\\"",\\\""hasExceptions\\\"":\\\""N\\\"",\\\""hasMobileReceipt\\\"":\\\""N\\\"",\\\""locName\\\"":\\\""Singapore, SINGAPORE\\\"",\\\""ccLocationResolved\\\"":\\\""Y\\\"",\\\""exceptionMaxLevel\\\"":0,\\\""exchangeRate\\\"":1,\\\""receiptType\\\"":\\\""T\\\"",\\\""travelAllowance\\\"":\\\""N\\\"",\\\""isBillable\\\"":\\\""N\\\"",\\\""hasTimestamp\\\"":\\\""N\\\"",\\\""allocationState\\\"":\\\""N\\\"",\\\""claimedAmount\\\"":0,\\\""exchangeRateDirection\\\"":\\\""M\\\"",\\\""expKey\\\"":\\\""01128\\\"",\\\""expName\\\"":\\\""Student Project-Material & Supplies\\\"",\\\""formKey\\\"":1228,\\\""patKey\\\"":\\\""CASH\\\"",\\\""transactionCurrencyName\\\"":\\\""SGD\\\"",\\\""rejected\\\"":\\\""N\\\"",\\\""description\\\"":\\\""<Description>\\\"",\\\""transactionDate\\\"":\\\""<Date>\\\"",\\\""hotelCheckoutDate\\\"":\\\""<Date>\\\"",\\\""undefined\\\"":\\\""<Date>\\\"",\\\""vendorDescription\\\"":\\\""<Supplier>\\\"",\\\""transactionAmount\\\"":<Cost>,\\\""postedAmount\\\"":<Cost>,\\\""custom1\\\"":\\\""<ReceiptNo>\\\"",\\\""comment\\\"":\\\""<Comment>\\\"",\\\""parRpeKey\\\"":\\\""\\\"",\\\""empKey\\\"":\\\""<empKey>\\\"",\\\""polKey\\\"":\\\""1056\\\"",\\\""rptKey\\\"":\\\""<rptKey>\\\"",\\\""crnKey\\\"":129,\\\""reportEntryTaxes\\\"":[{\\\""taxAuthKey\\\"":\\\""2045\\\"",\\\""taxFormKey\\\"":\\\""1314\\\"",\\\""custom1\\\"":\\\""103427\\\""}],\\\""hasAttendees\\\"":\\\""N\\\"",\\\""attendees\\\"":{\\\""clearExisting\\\"":\\\""Y\\\"",\\\""list\\\"":[]}}\"",\n          mrusToSave: \""\"",\n          patKey: \""\"",\n          pctKey: \""\"",\n          parRpeKey: \""\"",\n          reportId: \""<reportId>\"",\n          expenseId: \""null\"",\n          shouldRecalculateActualsVsLimitsAllowances: false,\n          userId: \""<userId>\"",\n          contextRole: \""TRAVELER\""\n        ) {\n          status,\n          expense {\n            rptKey\n            rpeKey\n            expenseId\n          }\n        }\n      }\n    ""}"
            .Replace("<userId>", UserID).Replace("<empKey>", EMPKey).Replace("<reportId>", expense.ReportId).Replace("<rptKey>", claim.Key).Replace("<Description>", expense.Description).Replace("<Date>", expense.Date).Replace("<Supplier>", expense.Supplier).Replace("<Cost>", expense.Cost.ToString()).Replace("<ReceiptNo>", expense.ReceiptNo).Replace("<Comment>", expense.Comment);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            string jsonString = await response.Content.ReadAsStringAsync();
            if (jsonString.Contains("SUCCESS!"))
            {
                JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(jsonString)!;
                return jsonObject!["data"]!["saveExpense"]!["expense"]!["rpeKey"]!.ToString();
            }
            else throw new Exception("Failed to create new expense");
        }


        /// <summary>
        /// Method <c>GetReportKey</c> retrieves the key value for a claim
        /// </summary>
        public async Task<String> GetReportKey(string reportId)
        {
            string requestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{""operationName"":""GetReportIdAndKey"",""variables"":{""userId"":""<userId>"",""contextRole"":""TRAVELER"",""rptKey"":null,""reportId"":""<reportId>""},""query"":""query GetReportIdAndKey($userId: String, $contextRole: ContextRoleType, $rptKey: String, $reportId: String) {\n  employee(userId: $userId, contextRole: $contextRole) {\n    userId\n    contextRole\n    expenseReport(reportId: $reportId, rptKey: $rptKey) {\n      reportId\n      rptKey\n      __typename\n    }\n    __typename\n  }\n}\n""}"
            .Replace("<userId>", UserID).Replace("<reportId>", reportId);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            string jsonString = await response.Content.ReadAsStringAsync();
            if (!jsonString.Contains("data")) throw new Exception("Unable to retrieve report key");
            JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(jsonString)!;

            return jsonObject!["data"]!["employee"]!["expenseReport"]!["rptKey"]!.ToString();
        }


        /// <summary>
        /// Method <c>UploadImage</c> uploads an image under the user's account
        /// </summary>
        public async Task<string> UploadImage(string ImagePath, string filename)
        {
            string RequestUrl = "https://us2.concursolutions.com/expense/expenseDotNet/Receipts/UploadLineItemImage.ashx?empKey=" + EMPKey + "&role=TRAVELER";
            byte[] ImageBinary = File.ReadAllBytes(ImagePath);
            ByteArrayContent ImageContent = new ByteArrayContent(ImageBinary);
            MultipartFormDataContent HttpContent = new MultipartFormDataContent();
            HttpContent.Add(ImageContent, "File", filename);
            var response = await HttpClient.PostAsync(RequestUrl, HttpContent);
            string jsonString = await response.Content.ReadAsStringAsync();
            if (!jsonString.Contains("SUCCESS")) throw new Exception("Fail to upload image");
            string imageId = jsonString.Split(',')[1].Split('\'')[1];

            return imageId;
        }


        /// <summary>
        /// Method <c>GetAllExpenses</c> Gets all the expenses under a claim, also used to retrieve the expense IDs for the expense
        /// </summary>
        public async Task<List<Expense>> GetAllExpenses(Models.Claim claim)
        {
            string requestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{
              ""operationName"": ""GetEntriesAndColumns"",
              ""variables"": {
                ""reportId"": ""<reportId>"",
                ""userId"": ""<userId>"",
                ""contextRole"": ""TRAVELER"",
                ""expenseListDetailFormId"": null,
                ""includeDetailItemizations"": true
              },
              ""query"": ""query GetEntriesAndColumns($reportId: String!, $contextRole: ContextRoleType!, $userId: String!, $expenseListDetailFormId: String, $includeDetailItemizations: Boolean) {\n  reportEntriesDetails(\n    userId: $userId\n    reportId: $reportId\n    contextRole: $contextRole\n    expenseListDetailFormId: $expenseListDetailFormId\n    includeDetailItemizations: $includeDetailItemizations\n  ) {\n    reportId\n    columns {\n      formFieldId\n      dataType\n      fieldName\n      __typename\n    }\n    entries {\n      expenseId\n      summary {\n        attendeeCount\n        id\n        expenseType {\n          id\n          code\n          name\n          meta {\n            isJapanPublicTransportation\n            __typename\n          }\n          __typename\n        }\n        meta {\n          canDelete\n          hasAffidavit\n          hasAllocation\n          hasAttendees\n          hasBlockingExceptions\n          hasComments\n          hasExceptions\n          hasItemizations\n          hasReceiptImage\n          hasSource\n          hasSourceCreditCard\n          hasSourceEReceipt\n          hasSourceItinerary\n          hasSourceMobile\n          hasSourcePersonalCard\n          hasValidEbunshoImage\n          __typename\n        }\n        parentExpenseId\n        paymentType {\n          id\n          code\n          name\n          __typename\n        }\n        transactionAmount {\n          value\n          currencyCode\n          __typename\n        }\n        approvedAmount {\n          value\n          currencyCode\n          __typename\n        }\n        claimedAmount {\n          value\n          currencyCode\n          __typename\n        }\n        vendor {\n          id\n          description\n          name\n          __typename\n        }\n        receiptImageId\n        eReceiptImageId\n        isImageRequired\n        isPaperReceiptRequired\n        expenseSourceIdentifiers {\n          creditCardTransactionId\n          eReceiptId\n          expenseCaptureImageId\n          personalCardTransactionId\n          quickExpenseId\n          segmentId\n          segmentTypeId\n          tripId\n          jptRouteId\n          __typename\n        }\n        isPersonalExpense\n        transactionDate\n        location {\n          id\n          name\n          city\n          countrySubDivisionCode\n          __typename\n        }\n        rpeKey\n        __typename\n      }\n      values {\n        fieldName\n        formFieldId\n        value {\n          ... on StringValue {\n            stringValue: value\n            __typename\n          }\n          ... on IntegerValue {\n            integerValue: value\n            __typename\n          }\n          ... on BooleanValue {\n            booleanValue: value\n            __typename\n          }\n          ... on ListValue {\n            listValue: value {\n              code\n              id\n              value\n              __typename\n            }\n            __typename\n          }\n          ... on ListItemValue {\n            code\n            id\n            listItemValue: value\n            __typename\n          }\n          ... on AmountValue {\n            amountValue: value {\n              value\n              currency {\n                code\n                name\n                __typename\n              }\n              __typename\n            }\n            __typename\n          }\n          ... on FloatValue {\n            floatValue: value\n            __typename\n          }\n          ... on DateValue {\n            dateValue: value\n            __typename\n          }\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n  reportExceptions(\n    reportId: $reportId\n    userId: $userId\n    contextRole: $contextRole\n  ) {\n    ...ExceptionsFragment\n    __typename\n  }\n}\n\nfragment ExceptionsFragment on ReportExceptions {\n  countOfExceptions\n  hasBlockingExceptions\n  reportExceptions {\n    allocationId\n    exceptionCode\n    expenseId\n    parentExpenseId\n    isBlocking\n    message\n    parameters {\n      missingFields {\n        missingSpecialParentField\n        fields\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n  entryExceptions {\n    ...entryExceptionsFragment\n    __typename\n  }\n  __typename\n}\n\nfragment entryExceptionsFragment on EntryExceptions {\n  reportId\n  expenseId\n  attendeesExceptionLevel\n  countOfExceptions\n  transactionAmount {\n    value\n    currencyCode\n    __typename\n  }\n  transactionDate\n  expenseType {\n    id\n    code\n    name\n    meta {\n      isJapanPublicTransportation\n      __typename\n    }\n    __typename\n  }\n  entryExceptions {\n    ...entryExceptionFragment\n    __typename\n  }\n  itemizations {\n    reportId\n    expenseId\n    attendeesExceptionLevel\n    countOfExceptions\n    transactionAmount {\n      value\n      currencyCode\n      __typename\n    }\n    transactionDate\n    expenseType {\n      id\n      code\n      name\n      meta {\n        isJapanPublicTransportation\n        __typename\n      }\n      __typename\n    }\n    entryExceptions {\n      ...entryExceptionFragment\n      __typename\n    }\n    itemizations {\n      expenseId\n      __typename\n    }\n    __typename\n  }\n  __typename\n}\n\nfragment entryExceptionFragment on ExpenseEntryException {\n  allocationId\n  exceptionCode\n  expenseId\n  isBlocking\n  message\n  parentExpenseId\n  parameters {\n    missingFields {\n      fields\n      __typename\n    }\n    __typename\n  }\n  __typename\n}\n""
            }".Replace("<userId>", UserID).Replace("<reportId>", claim.Id);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(requestURL, HttpContent);
            string jsonString = await response.Content.ReadAsStringAsync();
            try
            {
                JsonNode jsonObject = JsonSerializer.Deserialize<JsonNode>(jsonString)!;
                JsonArray jsonArr = jsonObject!["data"]!["reportEntriesDetails"]!["entries"]!.AsArray();
                List<Expense> Expenses = new List<Expense>();
                for (int i = 0; i < jsonArr.Count; i++)
                {
                    Expense expense = new Expense();
                    expense.Id = jsonArr[i]!["expenseId"]!.ToString();
                    expense.Cost = Convert.ToDecimal(jsonArr[i]!["summary"]!["transactionAmount"]!["value"]!.ToString());
                    expense.Supplier = jsonArr[i]!["summary"]!["vendor"]!["description"]!.ToString();
                    expense.Date = jsonArr[i]!["summary"]!["transactionDate"]!.ToString();
                    expense.RPEKey = jsonArr[i]!["summary"]!["rpeKey"]!.ToString();
                    Expenses.Add(expense);
                }

                return Expenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve expenses");
            }
        }


        /// <summary>
        /// Method <c>LinkImageToRequest</c> Links the uploaded image to the expense it is assigned under
        /// </summary>
        public async Task<String> LinkImageToRequest(Expense expense)
        {
            string requestURL = "https://www-us2.api.concursolutions.com/spend-graphql/graphql";
            string content = @"{""operationName"":""AttachImageMutation"",""variables"":{""userId"":""<userId>"",""contextRole"":""TRAVELER"",""reportId"":""<reportId>"",""expenseId"":""<expenseId>"",""imageId"":""<imageId>""},""query"":""mutation AttachImageMutation($userId: String!, $contextRole: ContextRoleType!, $reportId: String!, $expenseId: String!, $imageId: String!) {\n  employee(userId: $userId, contextRole: $contextRole) {\n    expenseReport(reportId: $reportId) {\n      entry(expenseId: $expenseId) {\n        attachImage(imageId: $imageId) {\n          id\n          receiptImageId\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n""}"
            .Replace("<userId>", UserID).Replace("<reportId>", expense.ReportId).Replace("<expenseId>", expense.Id).Replace("<imageId>", expense.ImageId);
            StringContent HttpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(requestURL, HttpContent);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
