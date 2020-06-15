using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefPropertyTypeQueries
    {

        public static string getSelectAllBytypeId(string databaseType, long typeId)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " SELECT * " +
                                      "  FROM    un_ref_property_type" +
                                      " WHERE type_id = " + typeId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getAllPropertyTypes(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " SELECT * FROM    un_ref_property_type";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string insert(string databaseType, long typeId, string typeDesc)
        {
            string returnValue = "";
            try
            {

               
                        returnValue = "INSERT INTO   un_ref_property_type( type_id,  type_desc)" +
                                      "VALUES ( " + typeId + ",   '" + typeDesc + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long typeId, string typeDesc)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " UPDATE   un_ref_property_type" +
                                      "   SET  type_desc =  '" + typeDesc + "',  " +
                                      " WHERE type_id = " + typeId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long typeId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = " DELETE FROM   un_ref_property_type" +
                                      " WHERE type_id = " + typeId;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

