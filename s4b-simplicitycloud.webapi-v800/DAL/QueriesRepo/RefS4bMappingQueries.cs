using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefS4bMappingQueries
    {
        public static string getSelectAllByFormSequence(string databaseType, long formSequence)
        {
            string returnValue = "";
           
                    returnValue = "SELECT * " +
                                  "  FROM  un_ref_s4b_mapping " +
                                  "  Where form_sequence = " + formSequence;

            return returnValue;
        }
    }
}
