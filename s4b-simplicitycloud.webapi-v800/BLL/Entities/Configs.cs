using Microsoft.AspNetCore.SignalR;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public static class Configs
    {
        public static Dictionary<string, ProjectSettings> settings { get; set; }
        public static EmailSettings EmailSettings { get; set; }
        public static List<RefDocsLogos> DocumentLogos { get; set; }
        public static List<RossumSetting> RossumSettings { get; set; }
    }

    public class ProjectSettings
    {
        public string ConnectionString { get; set; }
        public string LinkServer { get; set; }
        public string HomePath { get; set; }
        public string AttFileOrderDocsPath { get; set; }
        public string AttFileOrderDocsLocationDB { get; set; }
        public List<NaturalForm> Forms { get; set; }
        public string NaturalFormSuccessURL { get; set; }
        public string NaturalFormErrorURL { get; set; }
        public bool IsDiaryAppFormsEnabled { get; set; }
        public int SessionExpiryHours { get; internal set; }
        public string TagImagePath { get; internal set; }
        public string TagImageBaseURL { get; internal set; }
        public string EmailAttachmentsPath { get; internal set; }
        public string FromEmailAddress { get; internal set; }
        public string DatabaseType { get; internal set; }
        public string AttachmentFolderMode { get; internal set; }
        public bool AutoCreateJobRef { get; internal set; }
        public string EmailAccount { get; internal set; }
        public string UserAccount { get; internal set; }
        public string KeyFilePath { get; internal set; }
        public string RootFolder { get; internal set; }
        public string TempUploadFolderPath { get; internal set; }
        public string UploadDocumentFolderName { get; internal set; }
        public bool EnableUploadDocumentFolderName { get; internal set; }
        public string AdminEmailAddress { get; internal set; }
        public int JobRefNumberLength { get; internal set; }
        public bool ManualCreateJobRefForCreateOrder { get; internal set; }
        public string FilingCabinetS4BFormsFolder { get; internal set; }
        public string S4BFormsRootFolderPath { get; internal set; }
        public string S4BFormsSubmissionRootFolderPath { get; internal set; }
        public string S4BFormsSubmissionRootFolderWWW { get; internal set; }
        public string S4BFormsSubmissionDefaultJobRef { get; internal set; }
        public Dictionary<string, DatabaseInfo> SecondaryConnections { get; set; }
        public Dictionary<string, string> SecondaryS4BFormsSubmissionsExportFolder { get; set; }
    }

    public class DatabaseInfo
    {
        public string DatabaseType { get; internal set; }
        public string ConnectionString { get; internal set; }
    }

    public class EmailSettings
    {
        public string HostName;
        public string HostUser;
        public string HostPassword;
        public int HostPort;
        public bool IsEnableSsl;
    }

    public class RossumSetting
    {
        public string ProjectId { get;set; }
        public int InvoicesQueueID { get; set; }
        public int ReceiptsQueueID { get; set; }
        public int DeliveryNotesQueueID { get; set; }
        public int PurchaseOrdersQueueID { get; set; }
        public int UserId { get; set; }
        public bool IsRunScheduler { get; set; }
    }


}
