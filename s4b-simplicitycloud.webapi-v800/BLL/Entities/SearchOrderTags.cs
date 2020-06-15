using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class SearchOrderTags
    {
        public bool IsJobRef { get; set; }
        public bool IsTagNumber { get; set; }
        public bool IsTagUser { get; set; }
        public bool IsTagCreatedDate { get; set; }
        public string JobRef { get; set; }
        public string TagNumber { get; set; }
        public int TagUser { get; set; }
        public DateTime? TagCreatedDate { get; set; }
    }
    public class FilterOption {
        public long? ClientId { get; set; }
        public long? AddressId { get; set; }
        public long? DeptId { get; set; }
        public long? CateId { get; set; }
    }
}
