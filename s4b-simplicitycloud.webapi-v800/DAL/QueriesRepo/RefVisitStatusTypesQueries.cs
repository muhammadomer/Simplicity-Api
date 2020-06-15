using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefVisitStatusTypesQueries
    {

        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {   
                returnValue = "SELECT * FROM un_ref_visit_status_type ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getVisitTypeById(string databaseType, int Id)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                        "  FROM un_ref_visit_status_type " +
                        " WHERE status_id = " + Id;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByStatusId(string databaseType, int visitStatusId)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                             "  FROM un_ref_visit_status_type " +
                             " WHERE status_id = " + visitStatusId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByVisitStatusDesc(string databaseType,string visitStatusDesc)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM un_ref_visit_status_type " +
                              " WHERE status_desc = '" + visitStatusDesc + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, RefVisitStatusTypes obj)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO un_ref_visit_status_type (status_id, status_desc, status_abbreviation,flg_no_access, flg_visit_complete, flg_create_order_bill, flg_return_reason) " +
                                      "VALUES (" + obj.StatusId + ", '" + obj.StatusDesc + "', '" + obj.StatusAbbreviation + "', " 
                                      + Utilities.GetBooleanForDML(databaseType, obj.FlgNoAccess) + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgVisitComplete) 
                                      + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgCreateOrderBill) + ", " 
                                      + Utilities.GetBooleanForDML(databaseType, obj.FlgReturnReason) + ")";
                               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, RefVisitStatusTypes obj)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_ref_visit_status_type set status_desc = '" + obj.StatusDesc + "', status_abbreviation = '" + obj.StatusAbbreviation + "', " +
                            "       flg_no_access = " + Utilities.GetBooleanForDML(databaseType, obj.FlgNoAccess) 
                            + ", flg_visit_complete = " + Utilities.GetBooleanForDML(databaseType, obj.FlgVisitComplete) 
                            + ", " +" flg_create_order_bill = " + Utilities.GetBooleanForDML(databaseType, obj.FlgCreateOrderBill)
                            + ", " + " flg_return_reason = " + Utilities.GetBooleanForDML(databaseType, obj.FlgReturnReason) 
                            + " WHERE status_id = " + obj.StatusId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string delete(string databaseType, long genericFieldId)
       {
           string returnValue = "";
           try
           {
                returnValue = "DELETE FROM un_ref_visit_status_type " +
                             " WHERE status_id = " + genericFieldId;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }

    }
}

