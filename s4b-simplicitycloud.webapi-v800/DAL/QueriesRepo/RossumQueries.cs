using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RossumQueries
    {
      public static string GetAllUnConfirmed(string databaseType, DateTime? fromDate, DateTime? toDate)
      {
         string returnValue = "";
         try
         {
            switch (databaseType) { 
            case "MSACCESS":
                        returnValue = @"select top 100 F.*, doc_type_desc, user_name From " +
                        " (un_rossum_files F inner join un_ref_rossum_doc_types t on f.doc_type = t.doc_type) " +
                        " left outer  join un_user_details u on u.user_id = f.created_by " +
                        " where F.flg_deleted=" + Utilities.GetBooleanForDML(databaseType, false);
                        if (!string.IsNullOrEmpty(fromDate.ToString()))
                            returnValue += " and F.date_created  >= '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'";
                        if (!string.IsNullOrEmpty(toDate.ToString()))
                            returnValue += " and F.date_created  <='" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59'";
                        if (string.IsNullOrEmpty(fromDate.ToString()) && string.IsNullOrEmpty(toDate.ToString()))
                            returnValue += " and (F.date_doc_imported is null " +
                                " or F.flg_failed=" + Utilities.GetBooleanForDML(databaseType, true) + ")";
                        returnValue += " order by F.date_created desc";
                        break;
            case "SQLSERVER":
                        returnValue = @"select top 100 F.*, doc_type_desc, u.user_name from un_rossum_files F " +
                        "inner join un_ref_rossum_doc_types t on f.doc_type = t.doc_type " +
                        "left outer  join un_user_details u on u.user_id = f.created_by " +
                        " where (F.flg_deleted is null or F.flg_deleted=" + Utilities.GetBooleanForDML(databaseType, false) + ")";
                    if (!string.IsNullOrEmpty(fromDate.ToString()))
                        returnValue += " and F.date_created  >= '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'";
                    if (!string.IsNullOrEmpty(toDate.ToString()))
                        returnValue += " and F.date_created  <='" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59'";
                    if (string.IsNullOrEmpty(fromDate.ToString()) && string.IsNullOrEmpty(toDate.ToString()))
                        returnValue += " and (F.date_doc_imported is null " +
                            " or F.flg_failed=" + Utilities.GetBooleanForDML(databaseType, true) + ")";
                    returnValue += " order by F.date_created desc";

               break;
            }
         }
         catch (Exception ex)
         {
            Utilities.WriteLog("Error Occured in Query");
         }
         return returnValue;
      }
    }
}
