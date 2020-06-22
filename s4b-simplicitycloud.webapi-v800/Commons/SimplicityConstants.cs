namespace SimplicityOnlineWebApi.Commons
{
    public class SimplicityConstants
    {
        public const string NotSet = "Not Set";
        public const string NotAvailable = "N/A";

        public const string SupplierTransType = "D";
        public const string ClientTransType = "B";
        public const string ContractorTransType = "C";
        public const string PropertyTransType = "P";
        public const string ContactTransType = "F";
        public const string StockTransType = "A";

        public const string EntryTypeSalesInvoice = "SI";

        public const string EntityDetailsSupplementaryDataTypePropertyDetails = "038";
        public const string EntityDetailsSupplementaryDataTypePropertyType = "022";
        public const string EntityDetailsSupplementaryDataTypePropertyStatus = "036";
        public const string EntityDetailsSupplementaryDataTypeNominalCode = "027";
        public const string EntityDetailsSupplementaryDataTypeTaxCode = "028";
        public const string EntityDetailsSupplementaryDataTypeVATNumber = "008";
        public const string EntityDetailsSupplementaryDataTypeWebAddress = "017";
        public const string EntityDetailsSupplementaryDataTypeContactName = "011";

        public const int DashboardViewMaxNumberOfRecords = 10;

        public const char EmailAddressSeparator = ';';

        public const string DONE_FILE_NAME = "Done.txt";

        public const string S4BFormTemplateDirectoryName = "templates";
        public const string S4BFormJsonFileName = "form_contant.json";
        public const string S4BFormTemplateName = "template.pdf";
        public const string S4BFormSubmittedTemplateName = "eform_pdf_with_data.pdf";
        public const string S4BFormReferenceFilesFolderName = "dataSources";
        public const string S4BFormKeywordTemplateId = "{{S4BFormKeywordTemplateId}}";
        public const string S4BFormKeywordTemplateName = "{{S4BFormKeywordTemplateName}}";
        public const string S4BFormKeywordDocumentName = "{{S4BFormKeywordDocumentName}}";
        public const string S4BFormKeywordDocumentTimeStamp = "{{S4BFormKeywordDocumentTimeStamp}}";
        public const string S4BFormKeywordDocumentClientRef = "{{S4BFormKeywordDocumentClientRef}}";
        public const string S4BFormKeywordDocumentPORef = "{{S4BFormKeywordDocumentPORef}}";
        public const string S4BFormKeywordDocumentSubmitNumber = "{{S4BFormKeywordDocumentSubmitNumber}}";
        public const string S4BFormKeywordSubmitterEmail = "{{S4BFormKeywordSubmitterEmail}}";
        public const string S4BFormKeywordDocumentVariable = "{{S4BFormKeywordDocumentVariable}}";
        public const string S4BFormPrepopulationCustomSubmittedUserId = "{{S4BFormPrepopulationCustomSubmittedUserId}}";
        public const string S4BFormPrepopulationCustomPREFCLIENTSTATUS = "{{VAR_PG4_PREF_CLIENT_STATUS}}"; // For Avon Ruby
        public const string S4BFormPrepopulationCustomDISCOUNTPCENT= "{{VAR_PG4_DISCOUNT_PCENT}}"; // For Avon Ruby
        public const string S4BFormPrepopulationCustomListCBSAnnualAssets = "{{CUST_LIST_CBS_ANNUAL_ASSETS}}"; // For CBS Annual Service Assets

        public const string S4BFormFieldNamePageDeclaration = "Field_pg2_declaration"; // This is for High Risk Declarations
        public const string S4BFormFieldValuePageDeclarationHighRisk = "High Risk"; // This is for High Risk Declarations

        public const string CldSettingFilingCabinetMailMergeTemplateName = "FilingCabinetMailMergeTemplateName";
        public const string CldSettingS4BFormSubmissionEmailContent = "S4BFormSubmissionEmailContent";
        public const string CldSettingS4BFormSubmissionEmailContentHighRisk = "S4BFormSubmissionEmailContentHighRisk";
        public const string CldSettingS4BFormSubmissionEmailContentCustom1 = "S4BFormSubmissionEmailContentCustom1";
        public const string CldSettingS4BFormSubmissionEmailContentCustom2 = "S4BFormSubmissionEmailContentCustom2";
        public const string CldSettingS4BFormSubmissionEmailContentCustom3 = "S4BFormSubmissionEmailContentCustom3";
        public const string CldSettingS4BFormSubmissionEmailSubject = "S4BFormSubmissionEmailSubject";
        public const string CldSettingS4BFormSubmissionEmailSubjectHighRisk = "S4BFormSubmissionEmailSubjectHighRisk";
        public const string CldSettingS4BFormSubmissionEmailSubjectCustom1 = "S4BFormSubmissionEmailSubjectCustom1";
        public const string CldSettingS4BFormSubmissionEmailSubjectCustom2 = "S4BFormSubmissionEmailSubjectCustom2";
        public const string CldSettingS4BFormSubmissionEmailSubjectCustom3 = "S4BFormSubmissionEmailSubjectCustom3";
        public const string CldSettingS4BFormDefaultDistributionEmailAddress = "S4BFormDefaultDistributionEmailAddress";
        public const string CldSettingS4BFormDefaultDistributionEmailAddressHighRisk = "S4BFormHighRiskDistributionEmailAddress";
        public const string CldSettingS4BEFormsClientEmailSubjectJobTicket = "S4BEFormsClientEmailSubjectJobTicket";
        public const string CldSettingS4BEFormsClientEmailContentJobTicket = "S4BEFormsClientEmailContentJobTicket";
        public const string CldSettingS4BEFormsClientFromEmailJobTicket = "S4BEFormsClientFromEmailJobTicket";

        public const string CldSettingS4BFormShowDiaryJobNotesBoth = "S4BFormShowDiaryJobNotesBoth";
        public const string CldSettingIsAppointmentsS4BFormsLinked = "IsAppointmentsS4BFormsLinked";
        public const string CldSettingS4BFormsImageQuality = "S4BFormsImageQuality"; //1(Low Quality) to 10(High Quality)
        public const string CldSettingIsSuspendS4BFormsDownloadForUnfinishedSubmissins = "IsSuspendS4BFormsDownloadForUnfinishedSubmissins";
        public const string CldSettingIsOrderStatusUpdateEnabledOnS4BFormVisitStatusUpdate = "IsOrderStatusUpdateEnabledOnS4BFormVisitStatusUpdate";
        public const string CldSettingIsKPICompleteUpdateEnabledOnS4BFormVisitStatusUpdate = "IsKPICompleteUpdateEnabledOnS4BFormVisitStatusUpdate";
        public const string CldSettingIsUserControl1UpdateEnabledOnS4BFormVisitStatusUpdate = "IsUserControl1UpdateEnabledOnS4BFormVisitStatusUpdate";
        public const string CldSettingIsS4BFormJobRefPaddedByZeros = "IsS4BFormJobRefPaddedByZeros";
        public const string CldSettingThirdPartyWebHomePageUrl = "ThirdPartyWebHomePageUrl";

        public const string CldSettingTenderSpecFileCabFolderName = "TenderSpecFileCabFolderName";
        public const string CldSettingTenderSpecFileCabOwnerFolderName = "TenderSpecFileCabOwnerFolderName";
        public const string CldSettingTenderSpecFileCabThirdPartyFolderName = "TenderSpecFileCabThirdPartyFolderName";
        public const string CldSettingTenderAcceptedEmailSubject = "TenderAcceptedEmailSubject";
        public const string CldSettingTenderAcceptedEmailContent = "TenderAcceptedEmailContent";
        public const string CldSettingTenderDeclinedEmailSubject = "TenderDeclinedEmailSubject";
        public const string CldSettingTenderDeclinedEmailContent = "TenderDeclinedEmailContent";
        public const string CldSettingTenderFinalizeEmailSubject = "TenderFinalizedEmailSubject";
        public const string CldSettingTenderFinalizeEmailContent = "TenderFinalizedEmailContent";
        public const string CldSettingTenderPostQuestionEmailSubject = "TenderPostQuestionEmailSubject";
        public const string CldSettingTenderPostQuestionEmailContent = "TenderPostQuestionEmailContent";
        public const string CldSettingTenderFileUploadedEmailSubject = "TenderFileUploadedEmailSubject";
        public const string CldSettingTenderFileUploadedEmailContent = "TenderFileUploadedEmailContent";
        public const string CldSettingEmailHeaderContent = "EmailHeaderContent";
        public const string CldSettingEmailFooterContent = "EmailFooterContent";
        public const string CldSettingIsDemoModeEnabled = "IsDemoModeEnabled";



        public const string CldSettingGoogleDriveAPINew = "GoogleDriveAPINew";
        public const string CldSettingGoogleDriveAPI = "GoogleDriveAPI";
        public const string CldSettingFilingCabinetRootFolder = "FilingCabinetRootFolder";
        public const string CldSettingFilingCabinetRootFolderId = "FilingCabinetRootFolderId";
        public const string CldSettingOldGoogleDriveAPI = "GoogleDriveAPI_OLD";
        public const string CldSettingFirebaseAPI = "FirebaseAPI";
        public const string ApplicationSettingsLastInvoiceNo = "LIN";
        public const string ApplicationSettingsInvoiceNoPrefix = "IPX";
        public const string ApplicationSettingsLastPONo = "LPN";
        public const string ApplicationSettingsPONoPrefix = "PPX";
        internal const string ThirdPartyWebDefaultHomePageUrl = "Appointments";
        public const string FilingCabinetCustomFolderName = "xxxx_custom_folder_name";
        public const string FilingCabinetMailMergeTemplatesDefaultFolderName = "__TEMPLATES__";

        // DB Names
        public const string DB_MSACCESS = "MSACCESS";
        public const string DB_MSSQLSERVER = "SQLSERVER";

        //DB Table Names
        public const string DB_TABLE_ORDERS = "un_orders";
        public const string DB_TABLE_EDC = "un_entity_details_core";

        //DB Fields for Table Orders
        public const string DB_FIELD_ORDERS_OCCUPIER_EMAIL = "occupier_email";

        //DB Fields for Table Entity Details Core
        public const string DB_FIELD_EDC_EMAIL = "email";

        //Enums for CldSettingS4BFormShowDiaryJobNotesBoth
        public enum S4BFormShowDiaryJobNotesBoth { JobNotes=0, DiaryNotes=1, Both=2 };

        //Enums for Search Match Criteria
        public enum SEARCHMATCHCRITERIA { ANY, EXACT, START, END };

        // Order Record Types
        public const string ORDER_RECORD_TYPE_ORD = "Order";
        public const string ORDER_RECORD_TYPE_ENQ = "Enquiry";
        public const string ORDER_RECORD_TYPE_EST = "Estimate";
        public const string ORDER_RECORD_TYPE_TEN = "Tender";
        public const string ORDER_RECORD_TYPE_AP = "Active Project";

        //Messages Constants
        public const string MESSAGE_NO_RECORD_FOUND = "No Record Found.";
        public const string MESSAGE_INVALID_REQUEST = "Invalid Request Parameter.";
        public const string MESSAGE_INVALID_REQUEST_HEADER = "Invalid Request Header.";
        public const string MESSAGE_INVALID_PROJECT_ID = "Invalid Project Id.";

        //Secondary Connections
        public const string DB_CONNECTIONSTRINGS_CAPELREC_ID = "CAPELREC";
        public const string DB_CONNECTIONSTRINGS_CAPELPLANT_ID = "CAPELPLANT";

        //Rossum Settings
        public const string ROSSUM_ROOT_FOLDER_NAME = "RossumRootFolderName";
        public const string ROSSUM_ROOT_FOLDER_ID = "RossumRootFolderId";
        public const string ROSSUM_FOLDER_USER_UPLOADS = "RossumFolderSupplierUserUploads";
        public const string ROSSUM_API_ENDPOINT = "RossumApiEndPoint";
        public const string ROSSUM_USER_ID = "RossumUserId";
        public const string ROSSUM_USER_PASSWORD = "RossumUserPassword";
        public const string ROSSUM_DEBUG_MODE = "RossumDebugMode";
        public const string ROSSUM_LEARNING_MODE = "RossumLearningMode";

        //Scheduler
        public const string SCHEDULER_INTERVAL_ROSSUM= "SchedulerIntervalRossum";
    }
}
