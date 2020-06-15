using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdTendersSpecs
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JobSequence { get; set; }
        public long? MeSequence { get; set; }
        public int PackSequence { get; set; }
        public int StatusSequence { get; set; }
        public bool FlgSpecPublished { get; set; }
        public DateTime? DatSpecPublished { get; set; }
        public DateTime? DatSpecDeadline { get; set; }
        public bool FlgSpecExtended { get; set; }
        public DateTime? DatSpecExtended { get; set; }
        public string SpecNotes { get; set; }
        public bool FlgTenderInProgress { get; set; }
        public bool FlgTendersCompleted { get; set; }
        public bool FlgAwarded { get; set; }
        public DateTime? DatAwarded { get; set; }
        public long? AwardedEntityId { get; set; }
        public OrdTendersTP TenderAwardedTP { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public OrdersMeHeader OrderMeHeader { get; set; }
        public RefOrdTenderPacks RefOrdTenderPacks { get; set; }
        public RefOrdTenderStatus RefOrdTenderStatus { get; set; }
        public List<OrdTendersSpecsFiles> OrdTendersSpecsFiles { get; set; }
        public List<OrdTendersTP> OrdTenders{ get; set; }
    }

    public class OrdTendersSpecsClient
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? MeSequence { get; set; }
        public int PackSequence { get; set; }
        public int StatusSequence { get; set; }
        public bool FlgDeleted { get; set; }
        public bool FlgSpecPublished { get; set; }
        public DateTime? DateSpecPublished { get; set; }
        public DateTime? DateSpecDeadline { get; set; }
        public bool FlgSpecExtended { get; set; }
        public DateTime? DateSpecExtended { get; set; }
        public string SpecNotes { get; set; }
        public bool FlgTenderInProgress { get; set; }
        public bool FlgTendersCompleted { get; set; }
        public bool FlgAwarded { get; set; }
        public DateTime? DateAwarded { get; set; }
        public long? AwardedTenderSequence { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public OrdersMeHeader OrderMeHeader { get; set; }
        public RefOrdTenderPacks RefOrdTenderPacks { get; set; }
        public RefOrdTenderStatus RefOrdTenderStatus { get; set; }
        public List<OrdTendersSpecsFiles> OrdTendersSpecsFiles { get; set; }
        public List<OrdTendersTP> OrdTenders { get; set; }
        public List<OrdTendersTPQS> OrdTenderSpecQAS { get; set; }
    }

    public class RefOrdTenderPacks
    {
        public int PackSequence { get; set; }
        public bool FlgDeleted { get; set; }
        public string PackDesc { get; set; }
        public int PackRowIndex { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
    public class RefOrdTenderStatus
    {
        public int StatusSequence { get; set; }
        public bool FlgDeleted { get; set; }
        public string StatusDesc { get; set; }
        public int StatusRowIndex { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
    public class RefOrdTenderCategories
    {
        public int CategorySequence { get; set; }
        public bool FlgDeleted { get; set; }
        public string CategoryDesc { get; set; }
        public int CategoryRowIndex { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}