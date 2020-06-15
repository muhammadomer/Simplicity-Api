using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class PurchaseOrders
    {
        public long? OrderId { get; set; }
        public bool FlgEformsImport { get; set; }
        public long? EformsImportId { get; set; }
        public bool FlgPoPlaced { get; set; }
        public int PoType { get; set; }
        public string OrderRef { get; set; }
        public string CustomerRef { get; set; }
        public long? SupplierId { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierTelephone { get; set; }
        public DateTime? OrderDate { get; set; }
        public string AddressInvoice { get; set; }
        public string AddressDelivery { get; set; }
        public double OrderAmount { get; set; }
        public double OrderDiscountAmount { get; set; }
        public double OrderShippingAmount { get; set; }
        public double OrderSubtotalAmount { get; set; }
        public double OrderVatAmount { get; set; }
        public double OrderTotalAmount { get; set; }
        public long? ContactId { get; set; }
        public string VehicleReg { get; set; }
        public string AdditionInfo { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public bool FlgDispatchDate { get; set; }
        public DateTime? DateDespatchDate { get; set; }
        public string OrderedBy { get; set; }
        public int OrderStatus { get; set; }
        public string UserField01 { get; set; }
        public string UserField02 { get; set; }
        public string UserField03 { get; set; }
        public string UserField04 { get; set; }
        public string UserField05 { get; set; }
        public string UserField06 { get; set; }
        public string UserField07 { get; set; }
        public string UserField08 { get; set; }
        public string UserField09 { get; set; }
        public string UserField10 { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public List<PurchaseOrderItems> POItems { get; set; }
    }
}