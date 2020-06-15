using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.Models.Repositories;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefS4bFormsDB : MainDB
    {

        public RefS4bFormsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        //public bool insertRefNaturalForms(out long formSequence, bool flgDeleted, bool flgDefault, long defaultId, bool flgPreferred, long rowIndex, string formId, string formDesc,
        //                                  string EmailTo, string CCEMailAddress, string BCCEmailAddess, bool Flgaddzipphotos, long categorySequence, bool flgClientSpecific, long clientId, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended, bool flgPrePopulate, bool flgLaunchFromHome, bool flgLaunchFromApps, bool isAssetRequired, bool isSupplierRequired, string prePopulationSql)
        //{
        //    bool returnValue = false;
        //    formSequence = -1;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdInsert =
        //                new OleDbCommand(RefS4bFormsQueries.insert(this.DatabaseType, flgDeleted, flgDefault, defaultId, flgPreferred, rowIndex, formId, formDesc, EmailTo, CCEMailAddress, BCCEmailAddess, Flgaddzipphotos,
        //                                                               categorySequence, flgClientSpecific, clientId, createdBy, dateCreated, lastAmendedBy, dateLastAmended, flgPrePopulate, flgLaunchFromHome, flgLaunchFromApps, isAssetRequired, isSupplierRequired, prePopulationSql), conn))
        //            {
        //                int result = objCmdInsert.ExecuteNonQuery();
        //                if (result > 0)
        //                {
        //                    formSequence = Utilities.GetDBAutoNumber(conn);
        //                    returnValue = true;
        //                }
        //                else
        //                {
        //                    returnValue = false;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnValue = false;
        //        //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
        //        // Requires Logging
        //    }
        //    return returnValue;
        //}

        //public bool insertRefNaturalForms(out long formSequence, bool flgDeleted, bool flgDefault, long defaultId, bool flgPreferred, long rowIndex, string formId, string formDesc,
        //                                  string EmailTo, string CCEMailAddress, string BCCEmailAddess, bool Flgaddzipphotos, long categorySequence, bool flgClientSpecific, long clientId, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended, bool flgPrePopulate, bool flgLaunchFromHome, bool flgLaunchFromApps, bool isAssetRequired, bool isSupplierRequired, string prePopulationSql)
        //{
        //    bool returnValue = false;
        //    formSequence = -1;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdInsert =
        //                new OleDbCommand(RefS4bFormsQueries.insert(this.DatabaseType, flgDeleted, flgDefault, defaultId, flgPreferred, rowIndex, formId, formDesc, EmailTo, CCEMailAddress, BCCEmailAddess, Flgaddzipphotos,
        //                                                               categorySequence, flgClientSpecific, clientId, createdBy, dateCreated, lastAmendedBy, dateLastAmended, flgPrePopulate, flgLaunchFromHome, flgLaunchFromApps, isAssetRequired, isSupplierRequired, prePopulationSql), conn))
        //            {
        //                int result = objCmdInsert.ExecuteNonQuery();
        //                if (result > 0)
        //                {
        //                    formSequence = Utilities.GetDBAutoNumber(conn);
        //                    returnValue = true;
        //                }
        //                else
        //                {
        //                    returnValue = false;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnValue = false;
        //        //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
        //        // Requires Logging
        //    }
        //    return returnValue;
        //}

        public RefS4bForms insertRefNaturalForms(RefS4bForms Object)
        {
            RefS4bForms returnValue = new RefS4bForms();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(RefS4bFormsQueries.insert(this.DatabaseType, Object), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            Object.FormSequence = sequence;
                            returnValue = Object;
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

        public List<RefS4bForms> selectAllRefNaturalFormsformSequence(long formSequence)
        {
            List<RefS4bForms> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.getSelectAllByformSequence(this.DatabaseType, formSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefS4bForms>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefS4BForms(dr));
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

        public RefS4bForms getByFormId(string formId)
        {
            RefS4bForms returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.getSelectAllByFormId(this.DatabaseType, formId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new RefS4bForms();
                                dr.Read();
                                returnValue = LoadRefS4BForms(dr);
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

        public List<RefS4bForms> getAllForms()
        {
            List<RefS4bForms> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefS4bForms>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefS4BForms(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public RefS4bForms getRecordById(long Id)
        {
            RefS4bForms returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.getSelectById(this.DatabaseType, Id), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new RefS4bForms();
                                dr.Read();
                                returnValue = LoadRefS4BForms(dr);
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

        public bool updateByformSequence(RefS4bForms obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.update(this.DatabaseType, obj), conn))
                    {
                        int result = objCmdUpdate.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnValue = true;
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

        public bool deleteByformSequence(long formSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.delete(this.DatabaseType, formSequence), conn))
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

        public bool deleteByFlgDeleted(long formSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.deleteFlagDeleted(this.DatabaseType, formSequence), conn))
                    {
                        if (objCmdUpdate.ExecuteNonQuery() > 0)
                        {
                            returnValue = true;
                        }
                        else
                        {
                            returnValue = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool UndeleteByFlgDeleted(long formSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.undeleteFlagDeleted(this.DatabaseType, formSequence), conn))
                    {
                        if (objCmdUpdate.ExecuteNonQuery() > 0)
                        {
                            returnValue = true;
                        }
                        else
                        {
                            returnValue = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool SetTemplateAsDefault(long formSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string query = " UPDATE   un_ref_s4b_forms" +
                                      "   SET flg_default =  " + false + " ";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.setTempDefault(this.DatabaseType, formSequence), conn))
                            {
                                if (objCmdUpdate.ExecuteNonQuery() > 0)
                                {
                                    returnValue = true;
                                }
                                else
                                {
                                    returnValue = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<RefS4bForms> getDeleteTemplate()
        {
            List<RefS4bForms> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.getDeletTemplate(this.DatabaseType), conn))
                    {
                        OleDbDataReader dr = objCmdUpdate.ExecuteReader();
                        if (dr.HasRows)
                        {
                            returnValue = new List<RefS4bForms>();
                            while (dr.Read())
                            {
                                returnValue.Add(LoadRefS4BForms(dr));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private RefS4bForms LoadRefS4BForms(OleDbDataReader dr)
        {
            const string METHOD_NAME = "RefS4BFormsDB.LoadRefS4BForms()";
            RefS4bForms refS4BForms = null;
            try
            {
                if (dr != null)
                {
                    refS4BForms = new RefS4bForms();
                    refS4BForms.FormSequence = DBUtil.GetLongValue(dr, "form_sequence");
                    refS4BForms.FlgDeleted = DBUtil.GetBooleanValue(dr, "flg_deleted");
                    refS4BForms.FlgDefault = DBUtil.GetBooleanValue(dr, "flg_default");
                    refS4BForms.DefaultId = DBUtil.GetLongValue(dr, "default_id");
                    refS4BForms.FlgPreferred = DBUtil.GetBooleanValue(dr, "flg_preferred");
                    refS4BForms.RowIndex = DBUtil.GetLongValue(dr, "row_index");
                    refS4BForms.FormId = DBUtil.GetStringValue(dr, "form_id");
                    refS4BForms.FormDesc = DBUtil.GetStringValue(dr, "form_desc");
                    refS4BForms.CategorySequence = DBUtil.GetLongValue(dr, "category_sequence");
                    refS4BForms.FlgClientSpecific = DBUtil.GetBooleanValue(dr, "flg_client_specific");
                    refS4BForms.ClientId = DBUtil.GetLongValue(dr, "client_id"); refS4BForms.FlgPrePopulate = DBUtil.GetBooleanValue(dr, "flg_pre_populate");
                    refS4BForms.FlgLaunchFromHome = DBUtil.GetBooleanValue(dr, "flg_launch_from_home");
                    refS4BForms.FlgLaunchFromApps = DBUtil.GetBooleanValue(dr, "flg_launch_from_apps");
                    refS4BForms.PrePopulationSql = DBUtil.GetStringValue(dr, "pre_population_sql");
                    refS4BForms.FlgAssetRequired = DBUtil.GetBooleanValue(dr, "flg_asset_required");
                    refS4BForms.FlgSupplierRequired = DBUtil.GetBooleanValue(dr, "flg_supplier_required");
                    refS4BForms.EmailTo = DBUtil.GetStringValue(dr, "email_to");
                    refS4BForms.CCEMailAddress = DBUtil.GetStringValue(dr, "email_copy");
                    refS4BForms.BCCEmailAddess = DBUtil.GetStringValue(dr, "email_bcc");
                    refS4BForms.FlgAddZipPhotos = DBUtil.GetBooleanValue(dr, "flg_add_zip_photos");
                    refS4BForms.CreatedBy = DBUtil.GetLongValue(dr, "created_by");
                    refS4BForms.DateCreated = Utilities.getDBDate(dr["date_created"].ToString());
                    refS4BForms.LastAmendedBy = DBUtil.GetLongValue(dr, "last_amended_by");
                    refS4BForms.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"].ToString());
                    
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Loading RefS4BForms.", ex);
            }
            return refS4BForms;
        }

        private RefS4bFormsMin LoadRefS4BFormsMin(OleDbDataReader dr)
        {
            const string METHOD_NAME = "RefS4BFormsDB.LoadRefS4BFormsMin()";
            RefS4bFormsMin refS4BForms = null;
            try
            {
                if (dr != null)
                {
                    refS4BForms = new RefS4bFormsMin();
                    refS4BForms.FormSequence = DBUtil.GetLongValue(dr, "form_sequence");
                    refS4BForms.FormId = DBUtil.GetStringValue(dr, "form_id");
                    refS4BForms.FormDesc = DBUtil.GetStringValue(dr, "form_desc");
                    refS4BForms.FlgPrePopulate = DBUtil.GetBooleanValue(dr, "flg_pre_populate");
                    refS4BForms.FlgLaunchFromHome = DBUtil.GetBooleanValue(dr, "flg_launch_from_home");
                    refS4BForms.FlgLaunchFromApps = DBUtil.GetBooleanValue(dr, "flg_launch_from_apps");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Loading RefS4BFormsMin.", ex);
            }
            return refS4BForms;
        }

        public List<RefS4bForms> selectUserChangedRefForms(DateTime? lastSynDate, long userId)
        {
            const string METHOD_NAME = "RefS4bFormsDB.selectUserChangedRefForms()";
            List<RefS4bForms> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.getSelectAllByUserIdAndAmendedDate(this.DatabaseType, userId, lastSynDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefS4bForms>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefS4BForms(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Entities By TransType And Entity Join Id.", ex);
            }
            return returnValue;
        }

        public List<RefS4bFormsMin> selectUserChangedRefFormsMin(DateTime? lastSynDate, long userId)
        {
            const string METHOD_NAME = "RefS4bFormsDB.selectUserChangedRefFormsMin()";
            List<RefS4bFormsMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.getSelectAllByUserIdAndAmendedDate(this.DatabaseType, userId, lastSynDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefS4bFormsMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefS4BFormsMin(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Entities By TransType And Entity Join Id.", ex);
            }
            return returnValue;
        }

        public List<long> GetFormSequencesByUserId(long userId)
        {
            const string METHOD_NAME = "RefS4bFormsDB.GetFormSequencesByUserId()";
            List<long> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bFormsQueries.SelectAllByUserId(this.DatabaseType, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<long>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Utilities.GetDBLong(dr["form_sequence"]));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Template Found for User Id '" + userId + "'";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Templates By User.", ex);
            }
            return returnValue;
        }

        public Dictionary<string, string> executePrepopulationQuery(HttpRequest request, string sql, long templateSeq)
        {
            const string METHOD_NAME = "RefS4bFormsDB.executePrepopulationQuery()";
            Dictionary<string, string> returnValue = new Dictionary<string, string>();
            List<RefS4bMapping> mapping = null;
            string value = string.Empty;
            Dictionary<string, string> list = null;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            value = string.Empty;
                            mapping = GetS4bMapping(templateSeq);
                            if (mapping != null)
                            {
                                mapping.ForEach(map =>
                               {
                                   value = string.Empty;
                                   bool customListFound = false;
                                   try
                                   {

                                       switch (map.FieldValueType)
                                       {
                                           case 0: // Not Set
                                               value = SimplicityConstants.NotSet;
                                               break;
                                           case 1: //DB Field
                                               value = Utilities.GetDBString(dr[map.FieldValue]);
                                               Type fieldType = dr.GetFieldType(dr.GetOrdinal(map.FieldValue));
                                               if (!string.IsNullOrEmpty(value) && fieldType.Equals(typeof(DateTime)))
                                               {
                                                   DateTime? dtValue = Utilities.getDBDate(dr[map.FieldValue]);
                                                   value = ((DateTime)dtValue).ToString("dd/MM/yyyy");
                                               }
                                               break;
                                           case 2: // Constant
                                               value = map.FieldValue;
                                               break;
                                           case 3: //Custom Value
                                               value = getS4BFormPrePopulationCustomValue(request, dr, map.FieldValue);
                                               break;
                                           case 4: //Custom List
                                               customListFound = true;
                                               list = GetS4BFormPrePopulationCustomList(request, dr, map.FieldValue);
                                               break;
                                          case 5: //Custom List For Scheduled Items
                                             customListFound = true;
                                             Utilities.WriteLog("Enter Custom List For Scheduled Items:5");
                                             list = GetS4BFormPrePopulationScheduleItemList(request, dr, map.FieldValue);
                                             break;
                                          default:
                                               break;
                                       }
                                       if (customListFound)
                                       {
                                           if (list != null && list.Count > 0)
                                           {
                                               foreach(KeyValuePair<string, string> item in list)
                                               {
                                                   returnValue.Add(item.Key, item.Value);
                                               }
                                           }
                                       }
                                       else
                                       {
                                           returnValue.Add(map.FieldName, value);
                                       }
                                   }
                                   catch (Exception ex)
                                   {
                                       ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while executing Prepopulation Query", ex);
                                   }
                               });
                            }
                        }
                    }
                }
            }
            return returnValue;
        }

        private string getS4BFormPrePopulationCustomValue(HttpRequest request, OleDbDataReader dr, string fieldValue)
        {
            string returnValue = SimplicityConstants.NotSet;
            switch (fieldValue)
            {
                case SimplicityConstants.S4BFormPrepopulationCustomSubmittedUserId:
                    returnValue = Utilities.GetUserIdFromRequest(request).ToString();
                    break;
                case SimplicityConstants.S4BFormPrepopulationCustomPREFCLIENTSTATUS:
                    returnValue = getCustomPrefClientStatusByUserListId(DBUtil.GetStringValue(dr, "user_list_id"), true);
                    break;
                case SimplicityConstants.S4BFormPrepopulationCustomDISCOUNTPCENT:
                    returnValue = getCustomPrefClientStatusByUserListId(DBUtil.GetStringValue(dr, "user_list_id"), false);
                    break;
            }
            return returnValue;
        }

        private Dictionary<string, string> GetS4BFormPrePopulationCustomList(HttpRequest request, OleDbDataReader dr, string fieldValue)
        {
            Dictionary<string, string> returnValue = null;
            switch (fieldValue)
            {
                case SimplicityConstants.S4BFormPrepopulationCustomListCBSAnnualAssets:
                    returnValue = GetCustomListForCBSAnnualServiceAssets(request, DBUtil.GetLongValue(dr, "sequence"));
                    break;
            }
            return returnValue;
        }

      private Dictionary<string, string> GetS4BFormPrePopulationScheduleItemList(HttpRequest request, OleDbDataReader dr, string fieldValue)
      {
         Dictionary<string, string> returnValue = null;
         Utilities.WriteLog("Enter GetS4BFormPrePopulationScheduleItemList");
         returnValue = GetCustomListForCWDScheduleItems(request, DBUtil.GetIntValue(dr, "job_sequence"), fieldValue);
         return returnValue;
      }
      //For CBS Annual Service
      private Dictionary<string, string> GetCustomListForCBSAnnualServiceAssets(HttpRequest request, long sequence)
        {
            const string METHOD_NAME = "DiaryAppsRepository.GetCustomListForCBSAnnualServiceAssets()";
            Dictionary<string, string> returnValue = null;
            try
            {
                DiaryAppsRepository diaryAppsRepository = new DiaryAppsRepository();
                List<DiaryAppsAssets> diaryAppsAssets = diaryAppsRepository.GetDiaryAppsAssets(request, sequence);
                if(diaryAppsAssets!=null)
                {
                    returnValue = new Dictionary<string, string>();
                    int pageCounter = 1;
                    int rowCounter = 0;
                    foreach (DiaryAppsAssets item in diaryAppsAssets)
                    {                        
                        rowCounter++;
                        if(pageCounter==1 && rowCounter>9)
                        {
                            pageCounter = 2;
                            rowCounter = 1;
                        }
                        else if(pageCounter == 2 && rowCounter > 16)
                        {
                            pageCounter = 3;
                            rowCounter = 1;
                        }
                        else if (pageCounter == 3 && rowCounter > 16)
                        {
                            break;
                        }
                        string location = "VAR_PG" + pageCounter + "_ROW" + rowCounter.ToString("00") + "_LOCATION";
                        returnValue.Add(location, item.ItemLocation);
                        string makeModel = "VAR_PG" + pageCounter + "_ROW" + rowCounter.ToString("00") + "_MAKE_MODEL";
                        string makeModelValue = item.ItemManufacturer + " " + item.ItemModel;
                        returnValue.Add(makeModel, makeModelValue);
                        string serialNo = "VAR_PG" + pageCounter + "_ROW" + rowCounter.ToString("00") + "_SERIAL_NO";
                        returnValue.Add(serialNo, item.ItemSerialRef);
                        string assetNo = "VAR_PG" + pageCounter + "_ROW" + rowCounter.ToString("00") + "_ASSET_NO";
                        returnValue.Add(assetNo, item.ItemUserField1);
                        string assetId = "VAR_PG" + pageCounter + "_ROW" + rowCounter.ToString("00") + "_ASSET_ID";
                        returnValue.Add(assetId, item.DiaryAssetSequence.ToString());
                        string catId = "VAR_PG" + pageCounter + "_ROW" + rowCounter.ToString("00") + "_CAT";
                        returnValue.Add(catId, item.AssetCategoryDetails.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting CBS Anual Service List.", ex);
            }
            return returnValue;
        }

      //For CWD Schedule Items, sequence will be jobsequence
      private Dictionary<string, string> GetCustomListForCWDScheduleItems(HttpRequest request, int jobSequence, string listType)
      {
         const string METHOD_NAME = "OrderItemsRepository.GetCustomListForCWDScheduleItems()";
         Dictionary<string, string> returnValue = null;
         try
         {
            OrderItemsRepository orderItemsRepository = new OrderItemsRepository();
            List<OrderItems> orderItems = null;
            // listType  value will be like NT_80 or ST_50
            Utilities.WriteLog("List Type:" + listType);
            int numberOfLines = Convert.ToInt32(listType.Substring(3, 2));
            Utilities.WriteLog("Number Of Lines:" + numberOfLines);
            if (listType.Substring(0, 2) == "NT") //Do not show text lines
               orderItems = orderItemsRepository.GetOrderItemsNT(jobSequence, request);
            else if (listType.Substring(0, 2) == "ST") // Show All
               orderItems = orderItemsRepository.GetAllOrderItems(jobSequence, request);

            if (orderItems != null)
            {
               returnValue = new Dictionary<string, string>();
               int rowCounter = 0;
               foreach (OrderItems item in orderItems)
               {
                  rowCounter++;
                  if (rowCounter <= numberOfLines)
                  {
                     string desc = "VAR_OI_ROW" + rowCounter.ToString("00") + "_DESC";
                     returnValue.Add(desc, item.ItemDesc);
                     string units = "VAR_OI_ROW" + rowCounter.ToString("00") + "_UNIT";
                     returnValue.Add(units, item.ItemUnits);
                     string qty = "VAR_OI_ROW" + rowCounter.ToString("00") + "_QTY";
                     returnValue.Add(qty, item.ItemQuantity.ToString());
                     string value = "VAR_OI_ROW" + rowCounter.ToString("00") + "_VALUE";
                     returnValue.Add(value, item.AmountValue.ToString());
                     string sequence = "VAR_OI_ROW" + rowCounter.ToString("00") + "_SEQ";
                     returnValue.Add(sequence, item.Sequence.ToString());
                  }
               }
            }
            else { Utilities.WriteLog("No Order items return"); }
         }
         catch (Exception ex)
         {
            ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting CWD Schedule Items.", ex);
         }
         return returnValue;
      }

      //For AvonRuby
      private string getCustomPrefClientStatusByUserListId(string userListId, bool isPrefClientStatus)
        {
            string returnValue = "";
            string prefClientStatus = "";
            string discountPercent = "0.0";
            if (userListId.ToString() == "0")
            {
                prefClientStatus = "";
                discountPercent = "0.0";
            }
            else if (userListId.ToString() == "1")
            {
                prefClientStatus = "P";
                discountPercent = ".1";
            }
            else if (userListId.ToString() == "2")
            {
                prefClientStatus = "G";
                discountPercent = "0.075";
            }
            else if (userListId.ToString() == "3")
            {
                prefClientStatus = "S";
                discountPercent = "0.05";
            }
            if (isPrefClientStatus)
            {
                returnValue = prefClientStatus;
            }
            else
            {
                returnValue = discountPercent;
            }
            return returnValue;
        }

        public List<RefS4bMapping> GetS4bMapping(long formSequence)
        {
            List<RefS4bMapping> returnValue = null;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(RefS4bMappingQueries.getSelectAllByFormSequence(this.DatabaseType, formSequence), conn))
                {
                    OleDbDataReader dr = objCmdUpdate.ExecuteReader();
                    if (dr.HasRows)
                    {
                        returnValue = new List<RefS4bMapping>();
                        while (dr.Read())
                        {
                            returnValue.Add(Load_S4bMappings(dr));
                        }
                    }
                }
            }
            return returnValue;
        }

        private RefS4bMapping Load_S4bMappings(OleDbDataReader dr)
        {
            RefS4bMapping refS4bMapping = null;
            if (dr != null)
            {
                refS4bMapping = new RefS4bMapping();
                refS4bMapping.Sequence = long.Parse(dr["sequence"].ToString());
                refS4bMapping.FormSequence = long.Parse(dr["form_sequence"].ToString());
                refS4bMapping.FieldName = Utilities.GetDBString(dr["field_name"]);
                refS4bMapping.FieldValueType = int.Parse(dr["field_value_type"].ToString());
                refS4bMapping.FieldValue = Utilities.GetDBString(dr["field_value"]);
            }
            return refS4bMapping;
        }

        public bool updateLastAmendedDateByformSequence(long formSequence, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefS4bFormsQueries.updateLastAmendedDate(this.DatabaseType, formSequence, dateLastAmended), conn))
                    {
                        int result = objCmdUpdate.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public long insertMapping(long formSequence, string fieldName, int fieldValueType, string fieldValue, long createdBy, DateTime? dateCreated)
        {
            long returnValue = 0;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmd =
                    new OleDbCommand(RefS4bFormsQueries.insertMapping(this.DatabaseType, formSequence, fieldName, fieldValueType, fieldValue, createdBy, dateCreated), conn))
                {
                    int result = objCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        returnValue = Utilities.GetDBAutoNumber(conn);
                    }
                }
            }

            return returnValue;
        }

        public bool updateMapping(long sequence, long formSequence, string fieldName, int fieldValueType, string fieldValue, long amendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmd =
                    new OleDbCommand(RefS4bFormsQueries.updateMapping(this.DatabaseType, sequence, formSequence, fieldName, fieldValueType, fieldValue, amendedBy, dateLastAmended), conn))
                {
                    int result = objCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }

        public bool deleteMapping(long sequence)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmd =
                    new OleDbCommand(RefS4bFormsQueries.deleteMapping(this.DatabaseType, sequence), conn))
                {
                    int result = objCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }
    }
}