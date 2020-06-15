namespace  SimplicityOnlineBLL.Entities
{
	public class ApplicationLogOns
		{
			public long? Sequence { get; set; }
			public long? UserId { get; set; }
			public string UserLogOnTime { get; set; }
			public bool FlgUserLogOff { get; set; }
			public bool FlgReset { get; set; }
			public string UserLogOffTime { get; set; }
			public long? UserProcessId { get; set; }
			public string UserIpAddress { get; set; }
		}
}