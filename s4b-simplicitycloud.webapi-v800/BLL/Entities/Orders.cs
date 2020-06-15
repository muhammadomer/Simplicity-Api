using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
//using SimplicityOnlineWebApi.BLL.CustomBinders;

namespace SimplicityOnlineBLL.Entities
{
    //[ModelBinder(BinderType = typeof(OrdersBinder))]
    public class Orders : ResponseModel
    {
        public long? Sequence { get; set; }
        public string JobRef { get; set; }
        public int? OrderType { get; set; }
        public long? JobClientId { get; set; }
        public string JobClientName { get; set; }
        public string ParentClientName { get; set; }
        public string EntityDetails { get; set; }
        public long? JobManagerId { get; set; }
        public string JobManagerName { get; set; }
        public string JobDesc { get; set; }
        public int? JobAddressId { get; set; }
        public int? JobStatusId { get; set; }
        public string JobAddress { get; set; }
        public string AddressPostCode { get; set; }
        public string JobClientRef { get; set; }
             
        public string StatusDescription { get; set; }
        public string JobPriorityCode { get; set; }
        public DateTime? JobDate { get; set; }
        public DateTime? JobDateDue { get; set; }
        public bool FlgUser1 { get; set; }
        public DateTime? DateUser1 { get; set; }
        public bool FlgUser2 { get; set; }
        public DateTime? DateUser2 { get; set; }
        public string JobAddressDetail { get; set; }
        public string JobCostCentre { get; set; }
        public string OccupierName { get; set; }
        public string OccupierTelHome { get; set; }
        public string OccupierTelWork { get; set; }
        public string OccupierTelWorkExt { get; set; }
        public string OccupierTelMobile { get; set; }
        public string OccupierEmail { get; set; }
        public string JobOriginator { get; set; }
        public string JobResolution { get; set; }
        public string JobShortDesc { get; set; }
        public bool FlgClient { get; set; }
        public DateTime? DateClient { get; set; }
        public bool FlgJT { get; set; }
        public DateTime? DateJT { get; set; }
        public DateTime? JobDueDate { get; set; }
        public bool FlgJobSlaTimerStop { get; set; }
        public DateTime? DateJobSlaTimerStop { get; set; }
        public bool FlgJobDateStart { get; set; }
        public DateTime? JobDateStart { get; set; }
        public bool FlgJobDateFinish { get; set; }
        public DateTime? JobDateFinish { get; set; }
        public bool FlgJobCompleted { get; set; }
        public bool FlgJobCancelled { get; set; }
       public DateTime? DateCancelled { get; set; }
        public bool FlgNoAccess { get; set; }
        public double RetentionPcent { get; set; }
        public double SalesDiscountPcent { get; set; }
        public string JobTradeCode { get; set; }
        public string JobTradeCodeDesc { get; set; }
        public bool FlgBillProforma { get; set; }
        public string RecordType { get; set; }
        public string JobEstDetails { get; set; }
        public string JobVODetails { get; set; }
         public string CancelNotes { get; set; }
         public EntityDetailsCore EntityJobAddress { get; set; }
        public EntityDetailsCore JobManager { get; set; }
        public RefJobStatusType JobStatus { get; set; }
        public RefOrderType OrderTypeDesc { get; set; }
        public List<OrdersNotes> OrderNote { get; set; }
        public List<OrderNotesKpi> OrderNoteKPI { get; set; }
        public List<Cld_Ord_Labels> OI_FireProtection_I { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? LastAmendedDate { get; set; }
        public string ClientParent { get; set; }
    }

    public class OrdersMin
    {
        public long? Sequence { get; set; }
        public string JobRef { get; set; }
    }

    public class OrdersJobAddress
    {
        public long? Sequence { get; set; }
        public long? EntityId { get; set; }
        public string JobAddress { get; set; }
        public string NameTitle { get; set; }
        public string Telephone { get; set; }
        public string TelMobile { get; set; }
        public string TelExt { get; set; }
        public string TelWork { get; set; }
        public string Email { get; set; }

    }

    public class OrdersMinWithJobAddress
    {
        public long? Sequence { get; set; }
        public string JobRef { get; set; }
        public string JobAddress { get; set; }
        public string ContactName { get; set; }
        public string JobClientName { get; set; }
        public DateTime? jobDate { get; set; }
    }

    public class OrdersMinWithJobAddressClientName
    {
        public long? Sequence { get; set; }
        public string JobRef { get; set; }
        public string JobAddress { get; set; }
        public string JobClientName { get; set; }
		public string InvoiceNo { get; set; }

	}

    public class OrdersSmart
    {
        public long? Sequence { get; set; }
        public string JobRef { get; set; }
        public string JobClientName { get; set; }
        public string JobAddress { get; set; }
        public string OccupierName { get; set; }
        public string OccupierTelHome { get; set; }
        public string OccupierTelMobile { get; set; }
        public string OccupierEmail { get; set; }
        public string JobClientRef { get; set; }
        public string JobDesc { get; set; }
        public DateTime? JobDateDue { get; set; }    
    }

    public class OrdersList
    {
        public long? Sequence { get; set; }
        public string JobRef { get; set; }
        public string RecordType { get; set; }
        public int? OrderType { get; set; }
        public DateTime? JobDate { get; set; }
        public string AddressPostCode { get; set; }
        public string JobAddress { get; set; }
        public string JobClientRef { get; set; }
        public string JobClientName { get; set; }
        public string StatusDescription { get; set; }
        public string JobTradeCode { get; set; }
        public bool FlgJT { get; set; }
        public DateTime? DateJT { get; set; }
        public bool FlgClient { get; set; }
        public bool FlgJobCancelled { get; set; }
        public bool FlgJobCompleted { get; set; }
        public DateTime? DateClient { get; set; }
        public RefOrderType OrderTypeDesc { get; set; }
    }
}
