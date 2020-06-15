using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineBLL.Entities
{
    public class UserSessions
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProjectId { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public bool FlgAdmin { get; set; }
    }
}
