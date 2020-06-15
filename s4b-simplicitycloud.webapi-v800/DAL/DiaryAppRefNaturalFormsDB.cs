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
    public class DiaryAppRefNaturalFormsDB : MainDB
    {
        private bool IsJobDetails = false;
        public DiaryAppRefNaturalFormsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<RefNaturalForm> SelectAllFields()
        {
            List<RefNaturalForm> returnObj = new List<RefNaturalForm>(); ;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppRefNaturalFormsQueries.SelectAllFields(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {  
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadRefNaturalForm(dr));
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

        internal List<RefNaturalForm> SelectAllFieldsByClientId(long ClientId)
        {
            List<RefNaturalForm> returnObj = new List<RefNaturalForm>(); ;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppRefNaturalFormsQueries.SelectAllFieldsByClientId(this.DatabaseType,ClientId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadRefNaturalForm(dr));
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

        RefNaturalForm LoadRefNaturalForm(OleDbDataReader dr)
        {
            RefNaturalForm obj = new RefNaturalForm();
            try
            {
                if (dr != null)
                {
                    obj.RefNaturalformCategory = new RefNaturalFormCategories();
                    obj.FormSequence = long.Parse(dr["form_sequence"].ToString());
                    obj.RowIndex = long.Parse(dr["row_index"].ToString());
                    obj.FlgDefault = Boolean.Parse(dr["flg_default"].ToString());
                    obj.FlgPreferred = Boolean.Parse(dr["flg_compulsory"].ToString());
                    obj.DefaultId = Int32.Parse(dr["default_id"].ToString());
                    obj.FlgPreferred = Boolean.Parse(dr["flg_preferred"].ToString());
                    obj.FormId = dr["form_id"].ToString();
                    obj.FormDesc =  dr["form_desc"].ToString();
                    obj.FlgClientSpecific = Boolean.Parse(dr["flg_client_specific"].ToString());
                    obj.ClientId = long.Parse(dr["client_id"].ToString());
                    obj.RefNaturalformCategory.CategorySequence = Int32.Parse(dr["category_sequence"].ToString());
                    obj.RefNaturalformCategory.CategoryDesc = dr["category_desc"].ToString();
                    obj.RefNaturalformCategory.FlgCompulsory = Boolean.Parse(dr["flg_compulsory"].ToString());
                    obj.RefNaturalformCategory.HyperlinkText = dr["hyperlink_text"].ToString();
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

