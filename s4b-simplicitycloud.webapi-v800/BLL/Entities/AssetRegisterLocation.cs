namespace  SimplicityOnlineBLL.Entities
{
	public class AssetRegisterLocation
		{
			public long? Sequence { get; set; }
			public long? EntityId { get; set; }
			public bool FlgDeleted { get; set; }
			public bool FlgUseBuilding { get; set; }
			public string AssetLocationBuilding { get; set; }
			public bool FlgUseFloor { get; set; }
			public string AssetLocationFloor { get; set; }
			public bool FlgUseRoom { get; set; }
			public string AssetLocationRoom { get; set; }
			public long? AssetLocationRoomType { get; set; }
			public string AssetLocation { get; set; }
		}
}