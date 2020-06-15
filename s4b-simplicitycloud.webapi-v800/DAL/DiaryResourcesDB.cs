using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class DiaryResourcesDB : MainDB
    {
        public DiaryResourcesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertDiaryResources(out long sequence, bool flgDeleted, string resourceName, long resourceStatus, long resourceDisplayOrder, long resourceGroup,
                                     long resourceType, long joinResource, string resourceNotes, long resourceLogOnId, long createdBy,
                                     DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(DiaryResourcesQueries.insert(this.DatabaseType, flgDeleted, resourceName, resourceStatus, resourceDisplayOrder, resourceGroup, resourceType,
                                                                    joinResource, resourceNotes, resourceLogOnId, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<DiaryResources> selectAllDiaryResourcesSequence(long sequence)
        {
            List<DiaryResources> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryResourcesQueries.getSelectAllBySequence(this.DatabaseType,sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryResources>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_DiaryResources(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public DiaryResources selectDiaryResourceByUserId(int userId)
        {
            DiaryResources returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryResourcesQueries.getSelectAllByUserId(this.DatabaseType, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_DiaryResources(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<DiaryResourcesMin> AllDiaryResources()
        {
            List<DiaryResourcesMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryResourcesQueries.getAllDiaryResources(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryResourcesMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadDiaryResourcesMin(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public DiaryResourceContactDetail GetDiaryResourceContactByResourceId(long resourceSequence)
        {
            DiaryResourceContactDetail returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryResourcesQueries.getDiaryResourceContactByResourceId(this.DatabaseType,resourceSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnValue = LoadDiaryResourceContact(dr);
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
            return returnValue;
        }
        public List<DiaryResourceNotes> getResourceNotesByEntityId(long entityId)
        {
            List<DiaryResourceNotes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryResourcesQueries.getResourceNotesByEntityId(this.DatabaseType, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryResourceNotes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_ResourceNotes(dr));
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
            return returnValue;
        }

        public bool updateBySequence(long sequence, bool flgDeleted, string resourceName, long resourceStatus, long resourceDisplayOrder, long resourceGroup,
                                    long resourceType, long joinResource, string resourceNotes, long resourceLogOnId, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryResourcesQueries.update(this.DatabaseType, sequence, flgDeleted, resourceName, resourceStatus, resourceDisplayOrder, resourceGroup,
                                                                    resourceType, joinResource, resourceNotes, resourceLogOnId, createdBy, dateCreated,
                                                                    lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(DiaryResourcesQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(DiaryResourcesQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private DiaryResources Load_DiaryResources(OleDbDataReader dr)
        {
            DiaryResources diaryResources = null;
            try
            {
                if (dr != null)
                {
                    diaryResources = new DiaryResources();
                    diaryResources.Sequence = long.Parse(dr["sequence"].ToString());
                    diaryResources.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    diaryResources.ResourceName = Utilities.GetDBString(dr["resource_name"]);
                    diaryResources.ResourceStatus = long.Parse(dr["resource_status"].ToString());
                    diaryResources.ResourceDisplayOrder = long.Parse(dr["resource_display_order"].ToString());
                    diaryResources.ResourceGroup = long.Parse(dr["resource_group"].ToString());
                    diaryResources.ResourceType = long.Parse(dr["resource_type"].ToString());
                    diaryResources.JoinResource = int.Parse(dr["join_resource"].ToString());
                    diaryResources.ResourceNotes = Utilities.GetDBString(dr["resource_notes"]);
                    diaryResources.ResourceLogOnId = long.Parse(dr["resource_log_on_id"].ToString());
                    diaryResources.CreatedBy = long.Parse(dr["created_by"].ToString());
                    diaryResources.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    diaryResources.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    diaryResources.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                    diaryResources.NameShort = DBUtil.GetStringValue(dr, "name_short");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return diaryResources;
        }

        private DiaryResourcesMin LoadDiaryResourcesMin(OleDbDataReader dr)
        {
            DiaryResourcesMin diaryResources = null;
            try
            {
                if (dr != null)
                {
                    diaryResources = new DiaryResourcesMin();
                    diaryResources.Sequence = long.Parse(dr["sequence"].ToString());
                    diaryResources.ResourceName = Utilities.GetDBString(dr["resource_name"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return diaryResources;
        }

        private DiaryResourceContactDetail LoadDiaryResourceContact(OleDbDataReader dr)
        {
            DiaryResourceContactDetail diaryResources = null;
            try
            {
                if (dr != null)
                {
                    diaryResources = new DiaryResourceContactDetail();
                    diaryResources.Sequence = DBUtil.GetLongValue(dr,"sequence");
                    diaryResources.ResourceName = DBUtil.GetStringValue(dr,"resource_name");
                    diaryResources.EngineerName = DBUtil.GetStringValue(dr, "name_long");
                    diaryResources.Telephone = DBUtil.GetStringValue(dr, "telephone");
                    diaryResources.TelExt = DBUtil.GetStringValue(dr, "tel_ext");
                    diaryResources.TelMobile = DBUtil.GetStringValue(dr, "tel_mobile");
                    diaryResources.TelWork = DBUtil.GetStringValue(dr, "tel_work");
                    diaryResources.TelFax = DBUtil.GetStringValue(dr, "tel_fax");
                    diaryResources.Email = DBUtil.GetStringValue(dr, "email");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return diaryResources;
        }

        private DiaryResourceNotes Load_ResourceNotes(OleDbDataReader dr)
        {
            DiaryResourceNotes notes = null;
            try
            {
                if (dr != null)
                {
                    notes = new DiaryResourceNotes();
                    notes.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    notes.DateAppStart = DBUtil.GetDateTimeValue(dr, "date_app_start");
                    notes.AppLocation = DBUtil.GetStringValue(dr, "app_location");
                    notes.AppSubject = DBUtil.GetStringValue(dr, "app_subject");
                    notes.AppNotes = DBUtil.GetStringValue(dr, "app_notes");
                    notes.ResourceName = DBUtil.GetStringValue(dr, "resource_name");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notes;
        }

        public double GetResourceVAMCostRate(long sequence)
        {
            const string METHOD_NAME = "DiaryResourcesDB.GetResourceVAMCostRate()";
            double returnValue = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryResourcesQueries.SelectResourceVAMCostRateBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = DBUtil.GetDoubleValue(dr, "vam_cost_rate");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While getting VAM Cost Rate.", ex);
            }
            return returnValue;
        }
    }
}