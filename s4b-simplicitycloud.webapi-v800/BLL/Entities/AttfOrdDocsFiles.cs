namespace SimplicityOnlineBLL.Entities
{
    public class AttfOrdDocsFiles
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JobSequence { get; set; }
        public long? FileMasterId { get; set; }
        public string FileSubmissonId { get; set; }
        public string FileDescription { get; set; }
        public string FileNotes { get; set; }
        public string FilePathAndName { get; set; }
        public long? CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public string DateLastAmended { get; set; }
    }
}