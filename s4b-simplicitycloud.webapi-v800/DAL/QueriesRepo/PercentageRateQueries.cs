using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class PercentageRatesQueries
    {
        public static string selectPercentageRatesByType(string databaseType,string type)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = @"SELECT *
                        FROM  un_percentage_rates";
                if (type.Length>0)
                {
                    returnValue += " Where un_percentage_rates.pcent_type='"+ type+"'";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        
    }
}

