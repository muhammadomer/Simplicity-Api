using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdTendersTP
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JoinSequence { get; set; } // Sequence of OrdersTendersSpecs
        public long? EntityId { get; set; }
        public string ContractorName { get; set; }
        public int TenderAccept { get; set; }
        public string TPNotes { get; set; }
        public bool FlgTenderUploads { get; set; }
        public bool FlgTenderFinalised { get; set; }
        public DateTime? DatTenderFinalised { get; set; }
        public double TenderAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public OrdTendersSpecs OrdersTendersSpecs { get; set; }
        public List<OrdTendersTPFiles> OrdTendersTPFiles { get; set; }
        public List<OrdTendersTPQS> OrdTendersQAs { get; set; }
        public EntityDetailsCore ThirdPartyEDC { get; set; } //Entity Details Core Third Party
    }

    public class OrdTendersTPQS
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JoinSequence { get; set; } //  un_ord_tenders_tp.sequence
        public string TPQuestion { get; set; }
        public bool FlgAnswered { get; set; }
        public string OwnerAnswer { get; set; }
        public bool FlgPublicAnswer { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class OrdTendersTPNotifications
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; } // Sequence of OrdersTendersSpecs
        public string ContractorName { get; set; }
        public string JobManager { get; set; }
        public string UserName { get; set; }
        public int TenderAccept { get; set; }
        public string TPNotes { get; set; }
        public bool FlgTenderUploads { get; set; }
        public bool FlgTenderFinalised { get; set; }
        public DateTime? DateTenderFinalised { get; set; }
        public bool FlgTenderAwarderd { get; set; }
        public DateTime? DateTenderAwarded { get; set; }
        public string TenderAcceptanceStatus { get; set; }
        public string PackDesc { get; set; }
        public string StatusDesc { get; set; }
        public string CategoryDesc { get; set; }
        public bool FlgSpecPublished { get; set; }
        public DateTime? DateSpecPublished { get; set; }
        public string SpecNotes { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string ProjectTitle { get; set; }
        public bool FlgSpecShowClient { get; set; }
        public bool FlgSpecShowJobAddress { get; set; }
        public string JobRef { get; set; }
        public string JobAddress { get; set; }
        public string QuestionPosted { get; set; }
        public string QuestionAnswerPosted { get; set; }
        public DateTime? DateQuestionPosted { get; set; }
        public string UploadedFileName { get; set; }
        public string UploadedFileDesc { get; set; }
        public string UploadedFileVersion { get; set; }
        public DateTime? DateFileUploaded { get; set; }
    }

}