using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class DiaryApps
    {
        public long? Sequence { get; set; }
        public string TransType { get; set; }
        public long? JoinResource { get; set; }
        public bool FlgUseClientId { get; set; }
        public long? ClientId { get; set; }
        public long? JobSequence { get; set; }
        public long? JobAddressId { get; set; }
        public bool FlgBookingRequired { get; set; }
        public long? ResourceSequence { get; set; }
        public DateTime? DateAppStart { get; set; }
        public DateTime? DateAppEnd { get; set; }
        public bool FlgAppAllDay { get; set; }
        public string AppPostCode { get; set; }
        public string AppSubject { get; set; }
        public string AppLocation { get; set; }
        public bool FlgAppReminder { get; set; }
        public string AppReminderSound { get; set; }
        public long? AppReminderMins { get; set; }
        public string AppNotes { get; set; }
        public string AppCategory { get; set; }
        public string AppAttachmentPath { get; set; }
        public bool FlgOnlineMeeting { get; set; }
        public bool FlgUnavailable { get; set; }
        public long? RepeatSequence { get; set; }
        public long? MultiResourceSequence { get; set; }
        public long? AppType { get; set; }
        public bool FlgAppDeleted { get; set; }
        public bool FlgAppCompleted { get; set; }
        public bool FlgAppBroken { get; set; }
        public long? AppBrokenReason { get; set; }
        public bool FlgNoAccess { get; set; }
        public bool FlgAppConfirmed { get; set; }
        public DateTime? DateAppConfirmed { get; set; }
        public string AppConfirmedBy { get; set; }
        public long? CertSequence { get; set; }
        public long? VisitStatus { get; set; }
        public DateTime? VisitVam { get; set; }
        public bool FlgAppFixed { get; set; }
        public bool FlgPrint { get; set; }
        public long? PrintUserId { get; set; }
        public long? UnscheduledDeSeq { get; set; }
        public long? RateSequence { get; set; }
        public bool ClientSMSEnabled { get; set; }
        public bool DiaryResourceSMSEnabled{get;set;}
        public bool SMSReminderEnabled {get;set;}
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string AssignToUserName{ get; set; }
        public Orders Order { get; set; }
        public DiaryAppsWebAssign DiaryAppWebAssign { get; set; }
    }
    public class DiaryAppsGPS
    {
        public DateTime? DateUserStart { get; set; }
        public string UserStartGPSLong { get; set; }
        public string UserStartGPSLat { get; set; }
        public DateTime? DateUserEnd { get; set; }
        public string UserEndGPSLong { get; set; }
        public string UserEndGPSLat { get; set; }
    }
    public class DiaryAppsKendoUi
    {
        public long? Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
    }
    public class DiaryAppsAssets
    {
        public long? AppSequence { get; set; }
        public long? AssetSequence { get; set; }
        public long? DiaryAssetSequence { get; set; }
        public string ItemLocation { get; set; }
        public string ItemModel { get; set; }
        public string ItemManufacturer { get; set; }
        public string ItemSerialRef { get; set; }
        public string ItemExtraInfo { get; set; }
        public string ItemUserField1 { get; set; }
        public string AssetCategoryDetails { get; set; }
    }

    public class DiaryAppsHistory
    {
        public long? AppSequence { get; set; }
        public DateTime? DateAppStart { get; set; }
        public DateTime? DateAppEnd { get; set; }
        public string AppSubject { get; set; }
        public string AppNotes { get; set; }
        public string AppCategory { get; set; }
        public string ResourceName { get; set; }
        public string JobClientName { get; set; }
        public string JobClientAddress { get; set; }
        public string JobDescription { get; set; }
    }

    public class DiaryAppsSmart
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public DateTime? DateAppStart { get; set; }        
        public string AppNotes { get; set; }
        public long? VisitStatus { get; set; }
        public OrdersSmart Order { get; set; }
        public List<long> S4BFormsIds { get; set; }
        public DiaryAppsGPS DAGPS { get; set; }
    }
    public class DiaryResponse
    {
        public long? Sequence { get; set; }
    }

    public class DiaryAppsMobileNo
    {
        public string MobileNo { get; set; }
    }
}