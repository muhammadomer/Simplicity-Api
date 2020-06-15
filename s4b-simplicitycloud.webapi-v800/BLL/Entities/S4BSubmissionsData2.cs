using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class S4BSubmissionsData2 
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public int PageNumber { get; set; }
        public string FieldName { get; set; }
        public string FieldData { get; set; }
        public int FieldPosition { get; set; }
        public string FieldType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string CreatedByUserName { get; set; }
        public string LastAmendedByUserName { get; set; }
        public List<SubmissionsImagesFh> S4BSubmissionsData2Images { get; set; }
    }
}