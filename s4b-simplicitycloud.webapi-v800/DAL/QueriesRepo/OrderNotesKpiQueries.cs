using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public class OrderNotesKpiQueries
    {
        public static string SelectAllKpiNotes(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT * From un_orders_notes_kpi ";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string SelectByJobSequence(string databaseType,long jobSequence)
        {
            string returnValue = "";
            try
            {       returnValue = @"SELECT uonk.sequence, uonk.job_sequence, uonk.kpi_notes,
                    uonk.created_by, uonk.date_created, uonk.date_last_amended,
                    ud.user_name, ud.user_logon 
                    FROM    un_orders_notes_kpi AS uonk
                    LEFT JOIN un_user_details AS ud ON uonk.created_by = ud.user_id
                    WHERE uonk.job_sequence = " + jobSequence
                    + " ORDER BY uonk.sequence DESC ";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string InsertKpiNotes(string databaseType, OrderNotesKpi NotesKpi)
        {
            string returnValue = "";
            try
            {
                returnValue = "insert into un_orders_notes_kpi(job_sequence, kpi_notes, created_by,  date_created)";
                returnValue += "values('" + NotesKpi.JobSequence + "', '" + NotesKpi.KpiNotes + "', '" + NotesKpi.CreatedBy + "', " + Utilities.GetDateTimeForDML(databaseType, NotesKpi.DateCreated,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string UpdateKpiNotes(string databaseType, OrderNotesKpi NotesKpi)
        {
            string returnValue = "";
            try
            {
                returnValue = "update un_orders_notes_kpi set job_sequence='" + NotesKpi.JobSequence + "',";
                returnValue += "kpi_notes =" + NotesKpi.KpiNotes + ",";
                returnValue += " last_amended_by ='" + NotesKpi.LastAmendedBy + "',";
                returnValue += " date_last_amended =" + Utilities.GetDateTimeForDML(databaseType, NotesKpi.DateLastAmended, true, true) + ",";
                returnValue += " where sequence = '" + NotesKpi.Sequence + "'";
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
    }
}
