namespace  SimplicityOnlineBLL.Entities
{
	public class AssetRegisterNotes
		{
			public long? Sequence { get; set; }
			public long? JoinSequence { get; set; }
			public long? AssetSequence { get; set; }
			public string AssetNote { get; set; }
			public long? CreatedBy { get; set; }
			public string DateCreated { get; set; }
			public long? LastAmendedBy { get; set; }
			public string DateLastAmended { get; set; }
		}
}