using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterServiceQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                        "  FROM un_asset_register_service" +
                        " WHERE sequence = " + Sequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
      
        public static string insert(string databaseType, bool flgDeleted, bool flgNotActive, long assetSequence, long jobSequence, long daSequence, long daAppType, DateTime? dateDaStart,
                                    DateTime? dateService, string serviceInitial, string serviceNotes,long conditionSequence, long serviceBy, bool flgNewJobCreated,
                                    bool flgNewApp, bool flgValidation, long validatedBy, DateTime? dateValidated, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_asset_register_service(flg_deleted, flg_not_active, asset_sequence, job_sequence, "+
                              "       da_sequence, da_app_type, date_da_start, date_service, service_initials, service_notes, " +
                              "       condition_sequence, service_by, flg_new_job_created, flg_new_app, flg_validated, validated_by, " +
                              "       date_validated, created_by, date_created, last_amended_by, date_last_amended) " +
                              "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " + Utilities.GetBooleanForDML(databaseType, flgNotActive) + ", " + 
                              "      " + assetSequence + ", " + jobSequence + ", " + 
                              "      " + daSequence + ", " + daAppType + ", " + Utilities.GetDateTimeForDML(databaseType, dateDaStart, true, true) + ", " + 
                              "      " + Utilities.GetDateTimeForDML(databaseType, dateService, true, true) + ", '" + serviceInitial + "', '" + serviceNotes + "', " +
                              "      " + conditionSequence + ", " + serviceBy + ", " + Utilities.GetBooleanForDML(databaseType, flgNewJobCreated) + ", " + 
                              "      " + Utilities.GetBooleanForDML(databaseType, flgNewApp) + ", " + 
                              "      " + Utilities.GetBooleanForDML(databaseType, flgValidation) + ", " + validatedBy + ", " + 
                              "      " + Utilities.GetDateTimeForDML(databaseType, dateValidated, true, true) + ", " +
                              "      " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                              "      " + lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
     
        public static string update(string databaseType, long sequence, bool flgDeleted, bool flgNotActive, long assetSequence, long jobSequence, long daSequence, long daAppType, DateTime? dateDaStart,
                                    DateTime? dateService, string serviceInitial, string serviceNotes, long conditionSequence, long serviceBy, bool flgNewJobCreated,
                                    bool flgNewApp, bool flgValidation, long validatedBy, DateTime? dateValidated, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_asset_register_service" +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + "," +
                        "       flg_not_active =  " + Utilities.GetBooleanForDML(databaseType, flgNotActive) + "," +
                        "       asset_sequence =  " + assetSequence + "," +
                        "       job_sequence =  " + jobSequence + "," +
                        "       da_sequence =  " + daSequence + "," +
                        "       da_app_type =  " + daAppType + "," +
                        "       date_da_start =  " + Utilities.GetDateTimeForDML(databaseType, dateDaStart,true,true) + "," +
                        "       date_service =  " + Utilities.GetDateTimeForDML(databaseType, dateService,true,true) + "," +
                        "       service_initials =  " + serviceInitial + "," +
                        "       service_notes =  " + serviceNotes + "," +
                        "       condition_sequence =  " + conditionSequence + "," +
                        "       service_by =  " + serviceBy + "," +
                        "       flg_new_job_created =  " + Utilities.GetBooleanForDML(databaseType, flgNewJobCreated) + "," +
                        "       flg_new_app =  " + Utilities.GetBooleanForDML(databaseType, flgNewApp) + "," +
                        "       flg_validated =  " + Utilities.GetBooleanForDML(databaseType, flgValidation)+ "," +
                        "       validated_by =  " + validatedBy + "," +
                        "       date_validated =  " + Utilities.GetDateTimeForDML(databaseType, dateValidated,true,true) + "," +
                        "       created_by = " + createdBy + "," +
                        "       date_created= " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + "," +
                        "       last_amended_by = " + lastAmendedBy + "," +
                        "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + "," +
                        " WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM un_asset_register_service " +
                        " WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                bool flg = true;
                returnValue = "UPDATE un_asset_register_service" +
                    "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                    " WHERE sequence = " + sequence;
             
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }       
    
    }
}

