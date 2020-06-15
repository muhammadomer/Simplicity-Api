namespace  SimplicityOnlineBLL.Entities
{
	public class ApplicationVatPeriods
		{
			public long? Sequence { get; set; }
			public bool FlgDelete { get; set; }
			public long? PeriodYear { get; set; }
			public long? PeriodIndex { get; set; }
			public string DatPeriodStart { get; set; }
			public string DatPeriodEnd { get; set; }
		}
}