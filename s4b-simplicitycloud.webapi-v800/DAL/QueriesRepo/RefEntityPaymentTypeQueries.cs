using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefEntityPaymentTypeQueries
    {

        
        public static string getAllPaymentTypes(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " SELECT * FROM    un_ref_entity_pymt_type";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       
    }
}

