using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefProductUnitQueries
    {
        public static string selectProductUnit(string databaseType)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = @"SELECT product_units, product_units_desc FROM un_ref_product_units ";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}

