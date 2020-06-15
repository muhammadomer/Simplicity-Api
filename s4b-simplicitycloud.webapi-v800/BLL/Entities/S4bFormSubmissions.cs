using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class S4bFormSubmissions
    {
        public long? Sequence { get; set; }
        public string S4bSubmitNo { get; set; }
        public string TemplateId { get; set; }
        public string S4bSubmitTs { get; set; }
        public DateTime? DateSubmit { get; set; }
        public long? FormSequence { get; set; }
        public string FileCabId { get; set; }
        public string TemplateName { get; set; }
        public string SubmitDetails { get; set; }
        public long? JobSequence { get; set; }
        public bool Flg3rdParty { get; set; }
        public bool FlgCompleted { get; set; }
        public long? Id3rdParty { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public int CreatedPDFCount { get; set; }
        public RefS4bForms RefNatForms { get; set; }
        public Orders Orders { get; set; }
        public string Submitter { get; set; }
        public string JobAddressShort { get; set; }
        public OrdersMin OrderSecondary { get; set; }
        public bool IsInvalidOrderSecondary { get; set; }
        public string clientEmail { get; set; }
        public string ContentPath { get; set; }
        public string ZipFilePath { get; set; }
        public string PdfFilePath { get; set; }
    }
}
