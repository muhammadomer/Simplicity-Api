using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrderNotesKpiDB : MainDB
    {
        public OrderNotesKpiDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<OrderNotesKpi> GetAllNotesKpi()
        {
            List<OrderNotesKpi> returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderNotesKpiQueries.SelectAllKpiNotes(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnObj = new List<OrderNotesKpi>();
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadOrderNotesKpi(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnObj;
        }

        internal List<OrderNotesKpi> getByJobSequence(long jobSequence)
        {
            List<OrderNotesKpi> returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderNotesKpiQueries.SelectByJobSequence(this.DatabaseType,jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnObj = new List<OrderNotesKpi>();
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadOrderNotesKpi(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnObj;
        }

        public OrderNotesKpi CreateNotesKpi(OrderNotesKpi Obj)
        {
            OrderNotesKpi returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderNotesKpiQueries.InsertKpiNotes(this.DatabaseType, Obj), conn))
                    {

                        objCmdSelect.ExecuteNonQuery();
                        string LatestId = "select @@IDENTITY";
                        using (OleDbCommand NewQuery = new OleDbCommand(LatestId, conn))
                        {
                            OleDbDataReader dr = NewQuery.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnObj = Obj;
                                returnObj.Sequence = Int32.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.                                                 '" + METHOD_NAME + "'\n";
                            }
                        }
                        returnObj = Obj;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return returnObj;
        }
        public OrderNotesKpi UpdateNotesKpi(OrderNotesKpi NotesKpiObj)
        {
            OrderNotesKpi returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderNotesKpiQueries.UpdateKpiNotes(this.DatabaseType, NotesKpiObj), conn))
                    {

                        int result = objCmdSelect.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnObj = NotesKpiObj;
                        }
                        else
                        {
                            returnObj = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnObj = null;
            }
            return returnObj;
        }

        OrderNotesKpi LoadOrderNotesKpi(OleDbDataReader dr)
        {
            OrderNotesKpi returnValue = null;
            try
            {
                if (dr != null)
                {
                    OrderNotesKpi NotesKpi = new OrderNotesKpi();
                    NotesKpi.Sequence =DBUtil.GetLongValue(dr,"sequence");
                    NotesKpi.JobSequence = DBUtil.GetLongValue(dr,"job_sequence");
                    NotesKpi.KpiNotes = DBUtil.GetStringValue(dr, "kpi_notes");
                    NotesKpi.CreatedBy = DBUtil.GetIntValue(dr,"created_by");
                    NotesKpi.DateCreated = DBUtil.GetDateTimeValue(dr,"date_created");
                    NotesKpi.LastAmendedBy = DBUtil.GetIntValue(dr,"last_amended_by");
                    NotesKpi.DateLastAmended = DBUtil.GetDateTimeValue(dr,"date_last_amended");
                    NotesKpi.UserName = DBUtil.GetStringValue(dr, "user_name");
                    NotesKpi.UserLogOn = DBUtil.GetStringValue(dr, "user_logon");
                    returnValue = NotesKpi;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
