using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class ResponseModel
    {
        public string ProjectId { get; set; }
        public bool IsSucessfull { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public int Count { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
        public bool AutoCreateJobRef { get; set; }
        public List<ApplicationWebPages> ApplicationWebPagesList { get; set; }
        public object TheObject { get; set; }

        public List<S4bFormsAssign> S4bFormsAssignList { get; set; }
        public List<UserDetails> UserDetailsList { get; set; }
    }
}
