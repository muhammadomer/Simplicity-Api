namespace SimplicityOnlineBLL.Entities
{
    public class AttfOrdDocsFileCounter
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public bool FlgMasterFile { get; set; }
        public long? LastFileNo { get; set; }
    }
}