using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class DiaryAppsReturnedDB : MainDB
    {

        public DiaryAppsReturnedDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertDiaryAppsReturned(out long sequence, long daSequence, long resourceSequence, long jobSequence, DateTime? dateAppStart, DateTime? dateAppEnd, string appSubject,
                                            string appLocation, string appNotes, long appType, long visitStatus, string returnReason, long createdBy, DateTime? dateCreated,
                                            long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(DiaryAppsReturnedQueries.insert(this.DatabaseType, daSequence, resourceSequence, jobSequence, dateAppStart, dateAppEnd, appSubject, appLocation, appNotes,
                                                                         appType, visitStatus, returnReason, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool insertDiaryAppsReturnedByDiaryApp(out long sequence, long daSequence, long visitStatus, string returnReason, 
                                                      long createdBy, DateTime? dateCreated,
                                                      long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(DiaryAppsReturnedQueries.insertByAppointmentSequence(this.DatabaseType, daSequence, visitStatus, returnReason, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                ErrorMessage = "Unable to get Auto Number from Diary Apps Returned Record.\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting into Diary Apps Returned  " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<DiaryAppsReturned> selectAllDiaryAppsReturnedSequence(long sequence)
        {
            List<DiaryAppsReturned> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsReturnedQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryAppsReturned>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_DiaryAppsReturned(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }


        public bool updateBySequence(long sequence, long daSequence, long resourceSequence, long jobSequence, DateTime? dateAppStart, DateTime? dateAppEnd, string appSubject,
                                    string appLocation, string appNotes, long appType, long visitStatus, string returnReason, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsReturnedQueries.update(this.DatabaseType, sequence, daSequence, resourceSequence, jobSequence, dateAppStart, dateAppEnd, appSubject,
                                                                                    appLocation, appNotes, appType, visitStatus, returnReason, createdBy, dateCreated,
                                                                                    lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsReturnedQueries.delete(sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsReturnedQueries.deleteFlagDeleted(this.DatabaseType,sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private DiaryAppsReturned Load_DiaryAppsReturned(OleDbDataReader dr)

        {
            DiaryAppsReturned diaryAppsReturned = null;
            try
            {
                if (dr != null)
                {

                    diaryAppsReturned = new DiaryAppsReturned();
                    diaryAppsReturned.Sequence = long.Parse(dr["sequence"].ToString());
                    diaryAppsReturned.DaSequence = long.Parse(dr["da_sequence"].ToString());
                    diaryAppsReturned.ResourceSequence = long.Parse(dr["resource_sequence"].ToString());
                    diaryAppsReturned.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    diaryAppsReturned.DateAppStart = Utilities.getSQLDate(DateTime.Parse(dr["date_app_start"].ToString()));
                    diaryAppsReturned.DateAppEnd = Utilities.getSQLDate(DateTime.Parse(dr["date_app_end"].ToString()));
                    diaryAppsReturned.AppSubject = Utilities.GetDBString(dr["app_subject"]);
                    diaryAppsReturned.AppLocation = Utilities.GetDBString(dr["app_location"]);
                    diaryAppsReturned.AppNotes = Utilities.GetDBString(dr["app_notes"]);
                    diaryAppsReturned.AppType = long.Parse(dr["app_type"].ToString());
                    diaryAppsReturned.VisitStatus = Int32.Parse(dr["visit_status"].ToString());
                    diaryAppsReturned.ReturnReason = Utilities.GetDBString(dr["return_reason"]);
                    diaryAppsReturned.CreatedBy = long.Parse(dr["created_by"].ToString());
                    diaryAppsReturned.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    diaryAppsReturned.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    diaryAppsReturned.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return diaryAppsReturned;
        }
    }
}
