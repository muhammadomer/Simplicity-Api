namespace SimplicityOnlineBLL.Entities
{
    public class DiaryAppsReturned
    {
        public long? Sequence { get; set; }
        public long? DaSequence { get; set; }
        public long? ResourceSequence { get; set; }
        public long? JobSequence { get; set; }
        public string DateAppStart { get; set; }
        public string DateAppEnd { get; set; }
        public string AppSubject { get; set; }
        public string AppLocation { get; set; }
        public string AppNotes { get; set; }
        public long? AppType { get; set; }
        public int VisitStatus { get; set; }
        public string ReturnReason { get; set; }
        public long? CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public string DateLastAmended { get; set; }
        public DiaryAppsGPS DaGps { get; set; }
    }
}