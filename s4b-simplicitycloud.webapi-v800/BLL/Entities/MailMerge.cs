namespace SimplicityOnlineBLL.Entities
{
    public class RefMailMergeCodes
    {
        public string MergeCode { get; set; }
        public string MergeDesc { get; set; }
        public string OldTableName { get; set; }
        public string OldColumnName { get; set; }
        public string NewTableName { get; set; }
        public string NewColumnName { get; set; }
        public string MergeFormula { get; set; }
    }

    public class RefMailMergeCodesMin
    {
        public string MergeCode { get; set; }
        public string MergeDesc { get; set; }
    }

    public class MailMergeRequest
    {
        public string TemplateId { get; set; }
        public long? JobSequence { get; set; }
    }

    public class OrderMailMergeCodes
    {
        public string MergeCode { get; set; }
        public string MergeValue { get; set; }
    }
}