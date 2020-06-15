namespace SimplicityOnlineBLL.Entities
{
    public class PercentageRates
    {
        public long? Sequence { get; set; }
        public bool Flg_Multi_Schedule { get; set; }
        public long? GroupId { get; set; }
        public string PcentType { get; set; }
        public string PcentId { get; set; }
        public double PcentRate { get; set; }
    }
}