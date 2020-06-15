using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Dynamic;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL
{
    public class S4BFormTemplateDB : MainDB
    {
        public S4BFormTemplateDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
       
        public ExpandoObject GetTemplateBySequence(long sequence,string templateId,string imagePath)
        {
            ExpandoObject returnValue = null;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect = new OleDbCommand(S4BFormTemplateQueries.SelectAllFieldsOfSubmissionDataBySequence(this.DatabaseType, sequence), conn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        returnValue = LoadTemplateData(dt, templateId, imagePath);
                    }
                    else
                    {
                        ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                    }
                   
                }
            }
            return returnValue;
        }

        internal bool Update(ExpandoObject templateData,long joinSequence,long userId)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = conn;
                objCmd.CommandText = S4BFormTemplateQueries.SelectSubmissionDataByFieldName(this.DatabaseType, "flgShowPriceToClient");
                OleDbDataAdapter da = new OleDbDataAdapter(objCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    objCmd.CommandText = S4BFormTemplateQueries.InsertFlgShowPriceTemplateData(this.DatabaseType, joinSequence, userId, "flgShowPriceToClient", "False");
                    objCmd.ExecuteNonQuery();
                }
                foreach (KeyValuePair<string, object> kvp in templateData)
                {
                    if (kvp.Key != "TemplateImages")
                    {
                        objCmd.CommandText = S4BFormTemplateQueries.UpdateTemplateData(this.DatabaseType, joinSequence, userId, kvp.Key, kvp.Value.ToString());
                        objCmd.ExecuteNonQuery();
                    }
                }
                
                returnValue = true;
            }
            return returnValue;
        }

        internal bool UpdateSubmissionImages(SubmissionsImagesFh siteInspection)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.UpdateSubmissionsImagesFH(this.DatabaseType, siteInspection), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();

                }
                returnValue = true;
            }
            return returnValue;
        }
        internal bool UpdateFileCabIdAndPDFCount(SubmissionsDataFh submissionDataFh)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.UpdateFileCabIdAndPdfCountForSubmissionsDataFh(this.DatabaseType, submissionDataFh), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }
            return returnValue;
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        ExpandoObject LoadTemplateData(DataTable dt,string templateId, string imagePath)
        {   
            dynamic obj = new ExpandoObject();
            List<TemplateImages> templateImages = null;
            templateImages = new List<TemplateImages>();
            AddProperty(obj, "TemplateImages", null);
            foreach (DataRow row in dt.Rows)
            {
                
                if (row["field_type"].ToString() == "signature" || row["field_type"].ToString() == "image") {
                    TemplateImages image = new TemplateImages();
                    image.Sequence = Convert.ToInt64(row["sequence"].ToString());
                    image.FieldName = row["field_name"].ToString();
                    image.FileName = row["field_data"].ToString().Trim();
                    image.PageNo = Convert.ToInt32(row["page_number"].ToString());
                    if (image.FileName.Trim().Length > 0)
                        image.FileWWWurl = string.Format("{0}/{1}/{2}", imagePath, templateId, row["field_data"].ToString().Trim());
                    else
                        image.FileWWWurl = "";
                    image.CreatedBy = Convert.ToInt32( row["created_by"].ToString());
                    image.LastAmendedBy = Convert.ToInt32(row["last_amended_by"].ToString());
                    image.DateCreated = Utilities.getDBDate(row["date_created"].ToString());
                    image.DateLastAmended = Utilities.getDBDate(row["date_last_amended"].ToString());
                    templateImages.Add(image);
                }
                else
                    AddProperty(obj, row["field_name"].ToString(), row["field_data"].ToString());
            }
            obj.TemplateImages=templateImages;
            return obj;
        }

        S4BFormTemplate LoadTemplateData_old(OleDbDataReader dr)
        {
            S4BFormTemplate siteInspetion = null;

            if (dr != null)
            {
                int i;
                siteInspetion = new S4BFormTemplate();
                siteInspetion.Sequence = long.Parse(dr["sequence"].ToString());
                siteInspetion.FieldName = (dr["field_name"] == null || dr["field_name"] == DBNull.Value) ? "" : dr["field_name"].ToString();
                siteInspetion.FieldData = (dr["field_data"] == null || dr["field_data"] == DBNull.Value) ? "" : dr["field_data"].ToString();
                siteInspetion.FieldPosition = (dr["field_position"] == null || dr["field_position"] == DBNull.Value) ? "" : dr["field_position"].ToString();
                siteInspetion.FieldType = (dr["field_type"] == null || dr["field_type"] == DBNull.Value) ? "" : dr["field_type"].ToString();
                siteInspetion.PageNo = (dr["page_number"] == null || dr["page_number"] == DBNull.Value) ? -1 : int.TryParse(dr["page_number"].ToString(), out i) ? i : -1;
                siteInspetion.CreatedBy = (dr["created_by"] == null || dr["created_by"] == DBNull.Value) ? -1 : int.TryParse(dr["created_by"].ToString(), out i) ? i : -1;
                siteInspetion.DateCreated = Utilities.getDBDate(dr["date_created"]);
                siteInspetion.LastAmendedBy = (dr["last_amended_by"] == null || dr["last_amended_by"] == DBNull.Value) ? -1 : int.TryParse(dr["last_amended_by"].ToString(), out i) ? i : -1;
                siteInspetion.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
            }
            return siteInspetion;
        }

        SubmissionsImagesFh LoadSubmissionImageFh(OleDbDataReader dr)
        {
            SubmissionsImagesFh submissionsImageFh = null;
            if (dr != null)
            {
                int i;
                long l;
                submissionsImageFh = new SubmissionsImagesFh();
                submissionsImageFh.Sequence = long.Parse(dr["sequence"].ToString());
                submissionsImageFh.JoinSequence = (dr["join_sequence"] == null || dr["join_sequence"] == DBNull.Value) ? -1 : long.TryParse(dr["join_sequence"].ToString(), out l) ? l : -1;
                submissionsImageFh.FixedImage = bool.Parse(dr["flg_fixed_image"].ToString());
                submissionsImageFh.PageNo = (dr["page_no"] == null || dr["page_no"] == DBNull.Value) ? -1 : int.TryParse(dr["page_no"].ToString(), out i) ? i : -1;
                submissionsImageFh.FieldId = Utilities.GetDBString(dr["field_id"]);
                submissionsImageFh.FileDisplayName = Utilities.GetDBString(dr["file_display_name"]);
                submissionsImageFh.FilePath = Utilities.GetDBString(dr["file_path"]);
                submissionsImageFh.CreatedBy = (dr["created_by"] == null || dr["created_by"] == DBNull.Value) ? -1 : int.TryParse(dr["created_by"].ToString(), out i) ? i : -1;
                submissionsImageFh.DateCreated = Utilities.getDBDate(dr["date_created"]);
                submissionsImageFh.LastAmendedBy = (dr["last_amended_by"] == null || dr["last_amended_by"] == DBNull.Value) ? -1 : int.TryParse(dr["last_amended_by"].ToString(), out i) ? i : -1;
                submissionsImageFh.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
            }
            return submissionsImageFh;
        }
    }
}
