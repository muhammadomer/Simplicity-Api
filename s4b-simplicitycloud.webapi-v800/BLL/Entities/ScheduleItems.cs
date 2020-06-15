using System;
using System.Collections.Generic;
namespace SimplicityOnlineBLL.Entities
{
    public class ScheduleItems
    {
        public long? GroupId { get; set; }
        public string TransType { get; set; }
        public string ProductCode { get; set; }
        public string ParentCode { get; set; }
        public string ProductDesc { get; set; }
        public string ProductUnits { get; set; }
        public double ProductQuantity { get; set; }
        public double ChgPcentLabour { get; set; }
        public double AmountLabour { get; set; }
        public double AmtLabourSubTotal { get; set; }
        public double ChgPcentMaterials { get; set; }
        public double AmountMaterials { get; set; }
        public double ChgPcentPlant { get; set; }
        public double AmountPlant { get; set; }
        public string AdjCode { get; set; }
        public double ChgPcentAdj { get; set; }
        public string PriorityCode { get; set; }
        public double ChgPcentPriority { get; set; }
        public double ChgPcentValue { get; set; }
        public double AmountValue { get; set; }
        public double AmtQtySubtotal { get; set; }
        public double AmtTotLabour { get; set; }
        public double AmtTotMaterials { get; set; }
        public double AmtTotPlant { get; set; }
        public double AmtTotSubcon { get; set; }
        public double AmtTotSums { get; set; }
        public double AmtTotLabOnly { get; set; }
        public double AmtTotPrelims { get; set; }
        public double AmountTotal { get; set; }
        public double AmountBalance { get; set; }
        public DateTime? ItemVam { get; set; }
        public int AssignedTo { get; set; }
        public int VamCostSequence { get; set; }
        public double VamCostRate { get; set; }
        public int ProductOutsDia { get; set; }
        public int ProductdOrw { get; set; }
        public string ProductCostCentre { get; set; }
        public double ProductWeight { get; set; }
        public double ProductLength { get; set; }
        public double ProductHeight { get; set; }
        public double ProductWidth { get; set; }
        public DateTime? ItemDueDate { get; set; }
        public bool FlgCompleted { get; set; }
        public int GrpOrdTi { get; set; }
        public int MeOiSequence { get; set; }
        public int SupplierId { get; set; }
        public int RciId { get; set; }
        public int RciOiSequence { get; set; }
        public string RciInvNo { get; set; }
        public string RciDdJoin { get; set; }
        public int RelationshipType { get; set; }
        public int OiSectionSequence { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public bool FlgDocsRecd { get; set; }
        public bool IsDelRow { get; set; }
        public Orders Order { get; set; }
    }
    public class ScheduleItemsHierarchy
    {
        public long? GroupId { get; set; }
        public string TransType { get; set; }
        public string ProductCode { get; set; }
        public string ParentCode { get; set; }
        public string ProductDesc { get; set; }
        public List<ScheduleItemsHierarchy> Child { get; set; }
    }
}