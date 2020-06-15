using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class S4BFormPrepopulationDataRequest
    {
        public List<Appointment> Appointments { get; set; }
    }

    public class Appointment
    {
        public long? Sequence { get; set; }
        public List<Template> Templates { get; set; }
    }

    public class Template
    {
        public long? Sequence { get; set; }
        public Dictionary<string, string> Variables { get; set; }
    }
}
