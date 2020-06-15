using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class RefS4bForms
    {
        public long? FormSequence { get; set; }
        public bool FlgDeleted { get; set; }
        public bool FlgS4B { get; set; }
        public bool FlgDefault { get; set; }
        public long? DefaultId { get; set; }
        public bool FlgPreferred { get; set; }
        public long? RowIndex { get; set; }
        public string FormId { get; set; }
        public string FormDesc { get; set; }
        public string EmailTo { get; set; }
        public string CCEMailAddress { get; set; }
        public string BCCEmailAddess { get; set; }
        public long? CategorySequence { get; set; }
        public bool FlgClientSpecific { get; set; }
        public long? ClientId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string FileName { get; set; }
        public string Filebasecode { get; set; }
        public bool FlgPrePopulate { get; set; }
        public bool FlgLaunchFromHome { get; set; }
        public bool FlgLaunchFromApps { get; set; }
        public string PrePopulationSql { get; set; }
        public bool FlgAssetRequired { get; set; }
        public bool FlgSupplierRequired { get; set; }
        public bool FlgAddZipPhotos { get; set; }
    }

    public class RefS4bFormsMin
    {
        public long? FormSequence { get; set; }
        public string FormId { get; set; }
        public string FormDesc { get; set; }
        public bool FlgPrePopulate { get; set; }
        public bool FlgLaunchFromHome { get; set; }
        public bool FlgLaunchFromApps { get; set; }
        public bool IsAssetRequired { get; set; }
    }

    public class RefS4BFormsSync
    {
        public List<RefS4bFormsMin> ModifiedTemplates { get; set; }
        public List<long> ActiveTemplates { get; set; }
        public DateTime ServerProcessingTime { get; set; }
    }
}
