namespace  SimplicityOnlineBLL.Entities
{
	public class Cld_Ord_Labels_Files_Add_Info
		{
			public long? Sequence { get; set; }
			public long? JobSequence { get; set; }
			public long? OiSequence { get; set; }
			public long? HeaderSequence { get; set; }
			public long? JoinSequence { get; set; }
			public bool FlgDeleted { get; set; }
			public string AddInfo { get; set; }
			public long? CreatedBy { get; set; }
			public string DateCreated { get; set; }
			public long? LastAmendedBy { get; set; }
			public string DateLastAmended { get; set; }		
		}
}