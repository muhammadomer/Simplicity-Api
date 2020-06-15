using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class RequestModel
    {
        public long? PageId { get; set; }
        public object TheObject { get; set; }
        public List<RequestFilters> Filters { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
    }

    public class RequestFilters
    {
        public string SearchText { get; set; }
        public string SearchField { get; set; }
        public string MatchCriteria { get; set; }
    }

    public class RequestHeaderModel
    {
        public string ProjectId {get; set;}
        public int UserId { get; set; }
        public object TheObject { get; set; }
    }
}
