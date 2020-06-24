using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
//using SimplicityOnlineWebApi.BLL.CustomBinders;

namespace SimplicityOnlineBLL.Entities
{
    //[ModelBinder(BinderType = typeof(OrdersBinder))]
    public class OrdersKpi : ResponseModel
    {
        public long? Sequence { get; set; }
        public long JobSequence { get; set; }
        public string JobRef { get; set; }
        public string OrderType { get; set; }
        public long? JobClientId { get; set; }
        public string JobClientName { get; set; }
        public string JobClientRef { get; set; }
        public string JobCostCentre { get; set; }
        public string JobTradeCode { get; set; }
        public string JobAddress { get; set; }
        public string JobPriorityCode { get; set; }
        public DateTime? JobDate { get; set; }
        public DateTime? JobDateDue { get; set; }
        public bool FlgJobDateStart { get; set; }
        public bool FlgBillPerforma { get; set; }
        public DateTime? JobDateStart { get; set; }
        public bool FlgJobDateFinish { get; set; }
        public DateTime? JobDateFinish { get; set; }
        public string ClientNameLong { get; set; }
        public string ParentNameShort { get; set; }
        public string ParentNameLong { get; set; }
        public string JobManagerName { get; set; }
        public string OrderTypeDescShort { get; set; }
        public string KpiStatus { get; set; }
      
    }

}
