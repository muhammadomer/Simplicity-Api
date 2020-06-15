using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class S4BFormRequest
    {
        public bool IsFormBase64 { get; set; }
        public string FormBase64 { get; set; }
        public string FormName { get; set; }
        public string FormId { get; set; }
        public long? FormSequence { get; set; }
        public string SubmissionId { get; set; }
        public bool IsFormResubmitted { get; set; }
        public string JobRef { get; set; }
        public string S4bSubmitNo { get; set; }
    }
    public class SubmittedVideoFile
    {
        public string FileCabId { get; set; }
        public string FileName { get; set; }
    }
}
