using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class S4BFormPrepopulationDataModel : ResponseModel
    {
        public List<Appointment> Appointments { get; set; }
    }
}
