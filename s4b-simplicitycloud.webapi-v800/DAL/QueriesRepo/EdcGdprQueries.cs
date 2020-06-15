using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class EdcGdprQueries
    {

        public static string getSelectByEntityId(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT un_edc_gdpr.*
                    FROM un_edc_gdpr  
                Where entity_id=" + entityId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
       
       
        public static string insert(string databaseType,EdcGdpr gdpr )
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_edc_gdpr(entity_id,user_accepts,  date_user_accepts,  accepts_type,  no_resaon,   " +
                                "      contact_by_post, contact_by_email,  contact_by_phone,  contact_by_sms, " + 
                                "      created_by,  date_created, last_amended_by,  date_last_amended) " +
                                "VALUES (" + gdpr.EntityId + ", " + gdpr.UserAccepts 
                                + ", " + Utilities.GetDateTimeForDML(databaseType, gdpr.DateUserAccepts, true, false)
                                + ", " + gdpr.AcceptsType + ", '" + (String.IsNullOrEmpty(gdpr.NoReason) ? " " : gdpr.NoReason) + "'" 
                                + ", " + gdpr.ContactByPost + ", " + gdpr.ContactByEmail + ", " + gdpr.ContactByPhone + ", " + gdpr.ContactBySms 
                                + ", " + gdpr.CreatedBy
                                + ", " + Utilities.GetDateTimeForDML(databaseType, gdpr.DateCreated, true, true)
                                + ", " + (gdpr.LastAmendedBy??0) 
                                + ", " + Utilities.GetDateTimeForDML(databaseType, gdpr.DateLastAmended, true, true)  + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string update(string databaseType, EdcGdpr gdpr)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_edc_gdpr" +
                                 "   SET  user_accepts =  " + gdpr.UserAccepts + ",  " +
                                 "        date_user_accepts =  " + Utilities.GetDateTimeForDML(databaseType, gdpr.DateUserAccepts, true, false) + ",  " +
                                 "        accepts_type =  " + gdpr.AcceptsType + ",  " +
                                 "        no_resaon =  '" + (String.IsNullOrEmpty(gdpr.NoReason) ? " " : gdpr.NoReason) +  "',  " +
                                 "        contact_by_post =  " + gdpr.ContactByPost + ",  " +
                                 "        contact_by_email =  " + gdpr.ContactByEmail + ",  " +
                                 "        contact_by_phone =  " + gdpr.ContactByPhone + ",  " +
                                 "        contact_by_sms =  " + gdpr.ContactBySms + ",  " +
                                 "        last_amended_by =  " + gdpr.LastAmendedBy + ",  " +
                                 "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, gdpr.DateLastAmended,true,true) + 
                                 "  WHERE entity_id = " + gdpr.EntityId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

       
        public static string delete(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_edc_gdpr" +
                              "WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        
    }
}

