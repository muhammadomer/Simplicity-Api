using SimplicityOnlineWebApi.Commons;
using System;
using Microsoft.VisualBasic;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderCancelAuditQueries
    {
       
        public static string insert(string databaseType, OrderCancelAudit orderCancel)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_order_cancel_audit (job_sequence, cancel_reference,cancel_notes, created_by, date_created) 
                             VALUES ('" + orderCancel.JobSequence + "','" + orderCancel.CancelReference + "','" + (String.IsNullOrEmpty(orderCancel.CancelNotes) ? " " : orderCancel.CancelNotes) +"'" 
                             +"," +  orderCancel.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, orderCancel.CreatedDate,true,true) + ")";
               
            }
            catch (Exception ex)
            {
            throw ex;
            }
            return returnValue;
        }
      
    }
}
