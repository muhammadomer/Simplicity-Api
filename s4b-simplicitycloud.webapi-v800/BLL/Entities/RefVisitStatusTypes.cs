using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineBLL.Entities
{
    public class RefVisitStatusTypes
    {
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }
        public string StatusAbbreviation { get; set; }
        public bool FlgNoAccess { get; set; }
        public bool FlgVisitComplete { get; set; }
        public bool FlgCreateOrderBill { get; set; }
        public bool FlgReturnReason { get; set; }
    }
}
