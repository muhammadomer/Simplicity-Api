using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class RefS4bMapping
    {
        public long? Sequence { get; set; }
        public long? FormSequence { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int FieldValueType { get; set; }
    }
}
