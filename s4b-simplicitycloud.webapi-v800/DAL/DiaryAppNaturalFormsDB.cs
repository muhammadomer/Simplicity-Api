using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Dynamic;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.DAL
{
    public class DiaryAppNaturalFormsDB : MainDB
    {
        
        public DiaryAppNaturalFormsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<DiaryAppNaturalForm> SelectAllDANFByDESequence(long deSequence)
        {
            List<DiaryAppNaturalForm> returnObj = new List<DiaryAppNaturalForm>(); ;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.SelectAllDANFByDESequence(this.DatabaseType, deSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadDANF(dr));
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

        internal List<DiaryAppNaturalForm> SelectUnassignedDANFOfDESequence(long deSequence)
        {
            List<DiaryAppNaturalForm> returnObj = new List<DiaryAppNaturalForm>(); ;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.SelectUnassignedDANFOfDESequence(this.DatabaseType, deSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadDANF(dr));
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
        internal List<DiaryAppNaturalForm> SelectAllDANFByFormSequence(long deSequence,long formSequence)
        {
            List<DiaryAppNaturalForm> returnObj = new List<DiaryAppNaturalForm>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.SelectAllDANFByFormSequence(this.DatabaseType,deSequence,formSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {   
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadDANF(dr));
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

        internal DiaryAppNaturalForm InsertNaturalForms(DiaryAppNaturalForm Object)
        {
            DiaryAppNaturalForm returnValue = new DiaryAppNaturalForm();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.InsertDiaryAppsNaturalForms(this.DatabaseType, Object), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            Object.Sequence = sequence;
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

        internal DiaryAppNaturalForm InsertPasteDiaryAppsNaturalForms(DiaryAppNaturalForm Object)
        {
            DiaryAppNaturalForm returnValue = new DiaryAppNaturalForm();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.InsertPasteDiaryAppsNaturalForms(this.DatabaseType, Object), conn))
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

        internal DiaryAppNaturalForm InsertTFRFromUnscheduled(long deSequence, long deSequenceUnscheduled,long userId)
        {
            DiaryAppNaturalForm returnValue = new DiaryAppNaturalForm();
            try
            {
                
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.InsertTFRFromUnscheduled(this.DatabaseType, deSequence,deSequenceUnscheduled,userId), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            returnValue.FormSequence = sequence;
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

        internal bool DeleteNaturalFormsByDeSequence(long deSequence)
        {
            bool returnValue = false;
            try
            {

                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.DeleteDiaryAppsNaturalFormsbyDeSequence(this.DatabaseType, deSequence), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
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

        internal bool DeleteNaturalFormsBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {

                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppNaturalFormsQueries.DeleteDiaryAppsNaturalFormsbySequence(this.DatabaseType, sequence), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
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

        DiaryAppNaturalForm LoadDANF(OleDbDataReader dr)
        {
            DiaryAppNaturalForm obj = new DiaryAppNaturalForm();
            try
            {
                if (dr != null)
                {
                    obj.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    obj.RefNaturalForm = new RefNaturalForm();
                    obj.RefNaturalForm.FormSequence = long.Parse(dr["form_sequence"].ToString());
                    obj.RefNaturalForm.FormId = dr["form_id"].ToString();
                    obj.RefNaturalForm.FormDesc =  dr["form_desc"].ToString();
                    obj.RefNaturalForm.FlgClientSpecific = Boolean.Parse(dr["flg_client_specific"].ToString());
                    obj.RefNaturalForm.ClientId = long.Parse(dr["client_id"].ToString());
                    obj.RefNaturalForm.CategorySequence = Int32.Parse(dr["category_sequence"].ToString());
                    obj.RefNaturalForm.CategoryDesc = dr["category_desc"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
    }

}

