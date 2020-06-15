using System;
namespace SimplicityOnlineBLL.Entities
{
    public class DiaryResources
    {

        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public string ResourceName { get; set; }
        public long? ResourceStatus { get; set; }
        public long? ResourceDisplayOrder { get; set; }
        public long? ResourceGroup { get; set; }
        public long? ResourceType { get; set; }
        public int JoinResource { get; set; }
        public string ResourceNotes { get; set; }
        public long? ResourceLogOnId { get; set; }
        public long? CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public string DateLastAmended { get; set; }

        public string NameShort { get; set; } //EDC Name Short
    }

    public class DiaryResourcesMin
    {
        public long? Sequence { get; set; }
        public string ResourceName { get; set; }
    }

    public class DiaryResourceContactDetail
    {
        public long? Sequence { get; set; }
        public string ResourceName { get; set; }
        public string EngineerName { get; set; }
        public string Telephone { get; set; }
        public string TelExt { get; set; }
        public string TelMobile { get; set; }
        public string TelWork { get; set; }
        public string TelFax { get; set; }
        public string Email { get; set; }
    }

    public class DiaryResourceNotes
    {
        public long? Sequence { get; set; }
        public DateTime? DateAppStart { get; set; }
        public string AppLocation { get; set;}
        public string AppSubject { get; set; }
        public string AppNotes { get; set; }
        public string ResourceName { get; set; }
    }
}