using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterSuppToolsQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                        "  FROM un_asset_register_supp_tools " +
                        " WHERE sequence = " + Sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long joinSequence, string assetId, long vehicleSequence, bool flgMinimumAge, long minimumAge, bool flgCertificationReq,
                                    long powerById, long gradingId, long acquiredTypeId, long serviceTypeId, long stateTypeId, bool flgOutOfService,
                                    DateTime? dateOutOfService, bool flgDueInService, DateTime? dateDueInService, string assetNotes, long lastAmendedBy,
                                    DateTime? dateLastAmended)
        {

            string returnValue = "";
            try
            {
                
                returnValue = "INSERT INTO un_asset_register_supp_tools(join_sequence, asset_id, vehicle_sequence, flg_minimum_age, minimum_age, flg_certification_re, powered_by_id, " +
                        "       grading_id, acquired_type_id, service_type_id, state_type_id, flg_out_of_service, date_out_of_service, " +
                        "       flg_due_in_service, date_due_in_service, asset_notes, last_amended_by, date_last_amended) " +
                        "VALUES (" + joinSequence + ", '" + assetId + "', " + vehicleSequence + ", " + Utilities.GetBooleanForDML(databaseType, flgMinimumAge) + ", " + minimumAge + ", " + Utilities.GetBooleanForDML(databaseType, flgCertificationReq) + ", " +
                        powerById + ", " + gradingId + ", " + acquiredTypeId + ", " + serviceTypeId + ", " + stateTypeId + ", " + Utilities.GetBooleanForDML(databaseType, flgOutOfService) + ", " +
                        Utilities.GetDateTimeForDML(databaseType, dateOutOfService,true,true) + ", " + Utilities.GetBooleanForDML(databaseType, flgDueInService) + ", " + Utilities.GetDateTimeForDML(databaseType, dateDueInService,true,true) + ", '" + assetNotes + "', " +
                        lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, long joinSequence, string assetId, long vehicleSequence, bool flgMinimumAge, long minimumAge, bool flgCertificationReq,
                                    long powerById, long gradingId, long acquiredTypeId, long serviceTypeId, long stateTypeId, bool flgOutOfService,
                                    DateTime? dateOutOfService, bool flgDueInService, DateTime? dateDueInService, string assetNotes, long lastAmendedBy, DateTime? dateLastAmended)
        {

            string returnValue = "";
            try
            {
                
                returnValue = "UPDATE un_asset_register_supp_tools " +
                        "   SET join_sequence =  " + joinSequence + ", " +
                        "       asset_id =  '" + assetId + "', " +
                        "       vehicle_sequence =  " + vehicleSequence + ", " +
                        "       flg_minimum_age =  " + Utilities.GetBooleanForDML(databaseType, flgMinimumAge) + ", " +
                        "       minimum_age =  " + minimumAge + ", " +
                        "       flg_certification_req =  " + Utilities.GetBooleanForDML(databaseType, flgCertificationReq) + ", " +
                        "       powered_by_id =  " + powerById + ", " +
                        "       grading_id =  " + gradingId + ", " +
                        "       acquired_type_id =  " + acquiredTypeId + ", " +
                        "       service_type_id =  " + serviceTypeId + ", " +
                        "       state_type_id =  " + stateTypeId + ", " +
                        "       flg_out_of_service =  " + Utilities.GetBooleanForDML(databaseType, flgOutOfService) + ", " +
                        "       date_out_of_service =  " + Utilities.GetDateTimeForDML(databaseType, dateOutOfService,true,true) + ", " +
                        "       flg_due_in_service =  " + Utilities.GetBooleanForDML(databaseType, flgDueInService) + ", " +
                        "       date_due_in_service =  " + Utilities.GetDateTimeForDML(databaseType, dateDueInService,true,true) + ", " +
                        "       asset_notes =  '" + assetNotes + "', " +
                        "       last_amended_by = " + lastAmendedBy + ", " +
                        "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
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
                
                returnValue = "DELETE FROM un_asset_register_supp_tools " +
                    " WHERE sequence = " + sequence;
              
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

