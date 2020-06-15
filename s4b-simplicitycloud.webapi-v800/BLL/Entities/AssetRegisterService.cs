using System;

namespace  SimplicityOnlineBLL.Entities
{
	public class AssetRegisterService
		{
			public long? Sequence { get; set; }
			public bool FlgDeleted { get; set; }
			public bool FlgNotActive { get; set; }
			public long? AssetSequence { get; set; }
			public long? JobSequence { get; set; }
			public long? DaSequence { get; set; }
			public long? DaAppType { get; set; }
			public DateTime? DateDaStart { get; set; }
			public DateTime? DateService { get; set; }
			public string ServiceInitials { get; set; }
			public string ServiceNotes { get; set; }
			public long? ConditionSequence { get; set; }
			public long? ServiceBy { get; set; }
			public bool FlgNewJobCreated { get; set; }
			public bool FlgNewApp { get; set; }
			public bool FlgValidated { get; set; }
			public long? ValidatedBy { get; set; }
			public DateTime? DateValidated { get; set; }
            public double ItemCostLabourRate { get; set; }
            public long? CreatedBy { get; set; }
			public DateTime? DateCreated { get; set; }
			public long? LastAmendedBy { get; set; }
			public DateTime? DateLastAmended { get; set; }
		}
}