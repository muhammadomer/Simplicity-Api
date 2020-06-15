using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class ApplicationWebPagesQueries
    {
        public static string GetAllApplicationWebPages(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT *   FROM un_application_web_pages where is_visible = " + Utilities.GetBooleanForDML(databaseType, true) +"  and is_toolbar = " + Utilities.GetBooleanForDML(databaseType, true);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
