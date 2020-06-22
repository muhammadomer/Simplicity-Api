using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Models;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.DAL
{
    public class RossumFilesDB : MainDB
    {

        protected readonly DatabaseInfo DbInfo;
        public RossumFilesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
            this.DbInfo = dbInfo;
        }

        internal long SaveRossumFile(RossumFile rossumFile)
        {
            long returnValue = -1;
            string qry;
            try
            {
                if ( rossumFile.Sequence <= 0)
                { //BLOCK: Insert entry 
                    qry = @"Insert into un_rossum_files(
                    file_name, file_name_cab_id, doc_type, created_by, date_created, last_amended_by, date_last_amended) 
                    Values('" +
                        rossumFile.FileName + "','" +
                        rossumFile.FileNameCabId + "'," +
                        rossumFile.DocType + "," +
                        rossumFile.CreatedBy + "," +
                        Utilities.GetDateTimeForDML(this.DatabaseType, DateTime.Now, true, true) + "," +
                        rossumFile.LastAmendedBy + "," +
                        Utilities.GetDateTimeForDML(this.DatabaseType, DateTime.Now, true, true) + ")";
                }
                else
                { //BLOCK: Update entry 
                    qry = @"update un_rossum_files set " +
                    " file_name = '" + rossumFile.FileName + "'" +
                    ", file_name_cab_id = '" + rossumFile.FileNameCabId + "'" +
                    ", doc_type = " + rossumFile.DocType +
                    ", rossum_queue_id = " + rossumFile.RossumQueueId +
                    ", date_doc_uploaded = " + Utilities.GetDateTimeForDML(this.DatabaseType, rossumFile.DateDocUploaded, true, true) +
                    ", rossum_document_id = " + rossumFile.RossumDocumentId +
                    ", rossum_annotation_id = " + rossumFile.RossumAnnotationId +
                    ", date_doc_processed = " + Utilities.GetDateTimeForDML(this.DatabaseType, rossumFile.DateDocProcessed, true, true) +
                    ", flg_failed = " + Utilities.GetBooleanForDML(this.DatabaseType, rossumFile.FlgFailed) +
                    ", file_remarks = '" + rossumFile.FileRemarks + "'" +
                    ", date_doc_validated = " + Utilities.GetDateTimeForDML(this.DatabaseType, rossumFile.DateDocValidated, true, true) +
                    ", date_doc_imported = " + Utilities.GetDateTimeForDML(this.DatabaseType, rossumFile.DateDocImported, true, true) +
                    ", rossum_cab_id = '" + rossumFile.RossumCabId + "'" +
                    ", last_amended_by = " + rossumFile.LastAmendedBy +
                    ", date_last_amended = " + Utilities.GetDateTimeForDML(this.DatabaseType, DateTime.Now, true, true) +
                    ", doc_upload_source = " + rossumFile.DocUploadSource +
                    ", debug_data = '" + rossumFile.DebugData + "'" +
                    " where sequence = " + rossumFile.Sequence;
                }
                //Utilities.WriteLog(qry, "SaveRossumFile");

                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert = new OleDbCommand(qry, conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = Utilities.GetDBAutoNumber(conn);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = -1;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging

            }
            return returnValue;
        }

        internal long UpdateDateUploaded(long? sequence)
        {
            return UpdateRossumDate("date_doc_uploaded", (long) sequence);
        }

        internal long UpdateDateProcessed(long? sequence)
        {
            return UpdateRossumDate("date_doc_processed", (long)sequence);
        }

        internal long UpdateDateValidated(long? sequence)
        {
            return UpdateRossumDate("date_doc_validated", (long)sequence);
        }

        internal long UpdateDateImported(long? sequence)
        {
            return UpdateRossumDate("date_doc_imported", (long)sequence);
        }

        private long UpdateRossumDate(string columnName, long sequence)
        {
            long returnValue = -1;
            string qry;
            qry = @"update un_rossum_files set " + columnName + "=" + Utilities.GetDateTimeForDML(this.DatabaseType, DateTime.Now, true, true) + 
                " where sequence = " + sequence + 
                " and " + columnName + " is null";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert = new OleDbCommand(qry, conn))
                    {
                        returnValue = objCmdInsert.ExecuteNonQuery();
                        if (returnValue<1)
                            Utilities.WriteLog("No row affected." + columnName, "UpdateRossumDate-Repository");
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = -1;
                Utilities.WriteLog("Rossum Date "+ columnName + " could not be saved", "UpdateRossumDate-Repository");
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging

            }
            return returnValue;
        }

        internal DateTime GetLastDateValidated()
        {
            DateTime returnValue = DateTime.Now;
            string qry;
            qry = @"select Max(date_doc_validated) as lastDate from un_rossum_files";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                if (!String.IsNullOrEmpty(dr["date_doc_validated"].ToString()))
                                    returnValue = DateTime.Parse(dr["date_doc_validated"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception while getting max validate date ", "GetLastDateValidated");
            }
            return returnValue;
        }

        internal bool UpdateContactName(RossumFile rossFile)
        {
            bool returnValue = false;
            string qry = "update un_rossum_files set rossum_contact_name ='" + rossFile.SupplierName + "' where sequence = "+ rossFile.Sequence;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate = new OleDbCommand(qry, conn))
                    {
                        int resp = objCmdUpdate.ExecuteNonQuery();
                        if (resp > 0 ) returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Could not update contact name. - " + ex.Message, "UpdateContactName");
            }
            return returnValue;
        }

        internal bool UpdateRemarks(RossumFile rossFile)
        {
            bool returnValue = false;
            string qry = "update un_rossum_files set file_remarks ='" + rossFile.FileRemarks + "' where sequence = " + rossFile.Sequence;

            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate = new OleDbCommand(qry, conn))
                    {
                        int resp = objCmdUpdate.ExecuteNonQuery();
                        if (resp > 0) returnValue = true;
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

        internal RossumFile GetBySequence(long sequence)
        {
            RossumFile returnValue = null;
            List<RossumFile> tempList = null;
            string qry = "select * from un_rossum_files files " +
                "inner join un_ref_rossum_doc_types types on types.doc_type = files.doc_type " +
                "where sequence=" + sequence;
            tempList = QryToRossumFiles(qry);
            if (tempList.Count > 0)
                returnValue = tempList[0];
            return returnValue;
        }
        internal RossumFile GetDebugData(long sequence)
        {
            RossumFile rossFile =new RossumFile();
            List<RossumFile> tempList = null;
            string qry = "select debug_data from un_rossum_files files where sequence=" + sequence;
            //tempList = QryToRossumFiles(qry);
            //if (tempList.Count > 0)
            //    rossFile = tempList[0];
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                rossFile.DebugData = dr.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
                                
            return rossFile;
        }

        internal RossumFile GetByFileCabId(string fileCabId)
        {
            RossumFile returnValue = null;
            List<RossumFile> tempList = null;
            string qry = "select * from un_rossum_files where file_name_cab_id='" + fileCabId + "'";
            tempList = QryToRossumFiles(qry);
            if (tempList.Count > 0)
                returnValue = tempList[0];
            return returnValue;
        }

        internal RossumFile GetByAnnotationId(int annotationId)
        {
            RossumFile returnValue = null;
            List<RossumFile> tempList = null;
            string qry = "select * from un_rossum_files where rossum_annotation_id=" + annotationId;
            tempList = QryToRossumFiles(qry);
            if (tempList.Count > 0)
                returnValue = tempList[0];
            return returnValue;
        }

        internal List<RossumFile> GetAllUnConfirmed(DateTime? fromDate, DateTime? toDate)
        {
            string qry = RossumQueries.GetAllUnConfirmed(this.DatabaseType, fromDate, toDate);
            return QryToRossumFiles(qry);
        }

        internal List<RossumFile> GetFilesToUploadOnRossum()
        {
            string qry = @"select * from un_rossum_files where date_doc_uploaded is null and file_name_cab_id is not null and file_name_cab_id<>'' ";
            return QryToRossumFiles(qry);
        }

        internal List<RossumFile> GetFilesToImportFromRossum()
        {
            string qry;
            qry = @"select * from un_rossum_files where date_doc_validated is not null and date_doc_imported is null order by date_doc_validated desc";
            return QryToRossumFiles(qry);
        }

        internal bool DeleteFlgBySequence(long sequence)
        {
            bool returnValue = false;
            string qry = "UPDATE un_rossum_files " +
                     "  SET flg_deleted = " + Utilities.GetBooleanForDML(DbInfo.DatabaseType, true) +
                     " WHERE sequence = " + sequence;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate = new OleDbCommand(qry, conn))
                    {
                        if (objCmdUpdate.ExecuteNonQuery() > 0)
                            returnValue = true;
                        else
                            returnValue = false;
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

        internal List<RossumDocumentType> GetDocTypesAll()
        {
            string qry = @"select * from un_ref_rossum_doc_types";           
            return QryToDocTypes(qry);
        }

        internal long UpdateDocTypeCabIds(RossumDocumentType documentType)
        {
            long returnValue = -1;
            string qry = "";
            try
            {
                if (!string.IsNullOrEmpty(documentType.DocTypeKey))
                { //BLOCK: Update entry 
                    qry = @"update un_ref_rossum_doc_types set " +
                    " doc_type_folder_cab_id = '" + documentType.DocTypeFolderCabId + "'" +
                    ", received_folder_cab_id= '" + documentType.ReceivedFolderCabId + "'" +
                    ", success_folder_cab_id = '" + documentType.SuccessFolderCabId + "'" +
                    ", in_review_folder_cab_id = '" + documentType.InReviewFolderCabId + "'" +
                    ", failed_folder_cab_id  = '" + documentType.FailedFolderCabId + "'" +
                    " where doc_type_key = '" + documentType.DocTypeKey + "'";
                }
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert = new OleDbCommand(qry, conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = Utilities.GetDBAutoNumber(conn);
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

        internal RossumDocumentType GetDocType(string docKey)
        {
            string qry = "select * from un_ref_rossum_doc_types where doc_type_key ='" + docKey + "'";
            return QryToDocTypes(qry).FirstOrDefault();
        }

        internal RossumDocumentType GetDocType(int docType)
        {
            string qry = "select * from un_ref_rossum_doc_types where doc_type =" + docType;
            return QryToDocTypes(qry).FirstOrDefault();
        }

        private List<RossumFile> QryToRossumFiles(string qry)
        {
            List<RossumFile> lstRossumFiles = new List<RossumFile>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    RossumFile file = new RossumFile();
                                    file.Sequence = DBUtil.GetLongValue(dr, "sequence");
                                    file.FileName = DBUtil.GetStringValue(dr, "file_name");
                                    file.FileNameCabId = DBUtil.GetStringValue(dr, "file_name_cab_id");
                                    file.DocType = DBUtil.GetIntValue(dr, "doc_type");
                                    file.DocTypeDesc = DBUtil.GetStringValue(dr, "doc_type_desc");
                                    file.RossumDocumentId = DBUtil.GetIntValue(dr, "rossum_document_id");
                                    file.RossumAnnotationId = DBUtil.GetIntValue(dr, "rossum_annotation_id");
                                    file.RossumQueueId = DBUtil.GetIntValue(dr, "rossum_queue_id");
                                    if (!string.IsNullOrEmpty(dr["date_doc_uploaded"].ToString())) 
                                        file.DateDocUploaded = DateTime.Parse(dr["date_doc_uploaded"].ToString());
                                    if (!string.IsNullOrEmpty(dr["date_doc_processed"].ToString()))
                                        file.DateDocProcessed = DateTime.Parse(dr["date_doc_processed"].ToString());
                                    file.FlgFailed = DBUtil.GetBooleanValue(dr, "flg_failed");
                                    file.FileRemarks = DBUtil.GetStringValue(dr, "file_remarks");
                                    if (!string.IsNullOrEmpty(dr["date_doc_validated"].ToString())) 
                                        file.DateDocValidated = DateTime.Parse(dr["date_doc_validated"].ToString());
                                    if (!string.IsNullOrEmpty(dr["date_doc_imported"].ToString())) 
                                        file.DateDocImported = DateTime.Parse(dr["date_doc_imported"].ToString());
                                    file.RossumCabId = DBUtil.GetStringValue(dr, "rossum_cab_id");
                                    file.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                                    file.CreatedByName = DBUtil.GetStringValue(dr, "user_name");
                                    file.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                                    file.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                                    file.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());
                                    GetDocStatus(ref file);
                                    file.SupplierName = DBUtil.GetStringValue(dr, "supplier_name");
                                    lstRossumFiles.Add(file);
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
            return lstRossumFiles;
        }

        private List<RossumDocumentType> QryToDocTypes(string qry)
        {
            List<RossumDocumentType> lstRossumDocTypes = new List<RossumDocumentType>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                RossumDocumentType docType = new RossumDocumentType();
                                docType.DocType = DBUtil.GetIntValue(dr, "doc_type");
                                docType.DocTypeKey = DBUtil.GetStringValue(dr, "doc_type_key");
                                docType.DocTypeDesc = DBUtil.GetStringValue(dr, "doc_type_desc");
                                docType.DocTypeQueueId = DBUtil.GetIntValue(dr, "doc_type_queue_id");
                                docType.DocTypeFolderName = DBUtil.GetStringValue(dr, "doc_type_folder_name");
                                docType.DocTypeFolderCabId = DBUtil.GetStringValue(dr, "doc_type_folder_cab_id");
                                docType.ReceivedFolderName = DBUtil.GetStringValue(dr, "received_folder_name");
                                docType.ReceivedFolderCabId = DBUtil.GetStringValue(dr, "received_folder_cab_id");
                                docType.InReviewFolderName = DBUtil.GetStringValue(dr, "in_review_folder_name");
                                docType.InReviewFolderCabId = DBUtil.GetStringValue(dr, "in_review_folder_cab_id");
                                docType.SuccessFolderName = DBUtil.GetStringValue(dr, "success_folder_name");
                                docType.SuccessFolderCabId = DBUtil.GetStringValue(dr, "success_folder_cab_id");
                                docType.FailedFolderName = DBUtil.GetStringValue(dr, "failed_folder_name");
                                docType.FailedFolderCabId = DBUtil.GetStringValue(dr, "failed_folder_cab_id");
                                lstRossumDocTypes.Add(docType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstRossumDocTypes;
        }

        private void GetDocStatus(ref RossumFile file)
        {
            long? a = file.Sequence;
            if (file.FlgFailed == true)
            {
                file.DocStatusCode = 0;
                file.DocStatus = "Failed";
            }
            else if (file.DateDocUploaded != null && file.DateDocProcessed == null && file.FlgFailed == false)
            { 
                file.DocStatusCode = 2;
                file.DocStatus = "Uploaded";
            }
            else if (file.DateDocUploaded != null && file.DateDocProcessed != null && file.DateDocValidated == null)
            {
                file.DocStatusCode = 3;
                file.DocStatus = "Ready to Validate";
            }
            else if (file.DateDocUploaded != null && file.DateDocProcessed != null && file.DateDocValidated != null && file.DateDocImported == null)
            {
                file.DocStatusCode = 4;
                file.DocStatus = "Waiting for import";
            }
            else if (file.DateDocUploaded != null && file.DateDocProcessed != null && file.DateDocValidated != null && file.DateDocImported != null)
            {
                file.DocStatusCode = 5;
                file.DocStatus = "Invoice Created";
            }
            return;
        }

        public string GrossData(string qry, bool isUpdate)
        {
            string returnValue = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd = new OleDbCommand(qry, conn))
                    {
                        if (isUpdate)
                        {
                            returnValue = objCmd.ExecuteNonQuery().ToString();
                        }
                        else
                        {
                            using (OleDbDataReader dr = objCmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    var r = Serialize(dr);
                                    returnValue = JsonConvert.SerializeObject(r, Formatting.Indented);
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


        private IEnumerable<Dictionary<string, object>> Serialize(OleDbDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
            {
                var record = new Dictionary<string, object>();
                foreach (var col in cols)
                    record.Add(col, reader[col]);
                results.Add(record);
            }
            return results;
        }
    }


}
#region Archive
//internal bool UpdateFileStatus(RossWebHook hook)
//{
//    bool returnValue = false;
//    /*
//     * When new status = Importing -> update date_doc_updated
//     * When new status = To_Review -> update date_doc_processed
//     * When new status = Exported -> update date_doc_validated
//     * When new status = Exported -> flg_delete=true
//     * After updating date_doc_validated, Get contents , create invoice then update date_doc_imported
//     * */

//    string qry = "update un_rossum_files set ";
//    if (hook.annotation.status == RossumDocStatus.FAILED_IMPORT || hook.annotation.status == RossumDocStatus.FAILED_EXPORT)
//    {
//        qry += "flg_failed=1, file_remarks='" + hook.annotation.messages.FirstOrDefault() + "' where 1=1 ";
//    }
//    if (hook.annotation.status == RossumDocStatus.IMPORTING)
//    {
//        qry += " date_doc_uploaded='" + Utilities.GetDateTimeForDML(this.DatabaseType, DateTime.Now, true, true) +
//        "' where 1=1 and date_doc_uploaded is not null";
//    }
//    if (hook.annotation.status == RossumDocStatus.TO_REVIEW)
//    {
//        qry += " date_doc_processed='" + Utilities.GetDateTimeForDML(this.DatabaseType, DateTime.Now, true, true) +
//        "' where 1=1 and date_doc_processed is not null";
//    }

//    else if (hook.annotation.status == RossumDocStatus.EXPORTED)
//    {
//        // This block should run only once for a document
//        qry += " date_doc_validated='" + Utilities.GetDateTimeForDML(this.DatabaseType, hook.annotation.confirmed_at, true, true) +
//        "' where 1=1 and date_doc_validated is not null";

//    }
//    else if (hook.annotation.status == RossumDocStatus.DELETED)
//    {
//        qry += " flg_delete=1 where 1=1 ";
//    }
//    else
//    {
//        return true;
//    }
//    // where clause for all
//    qry += " and file_name='" + hook.document.original_file_name + "'";

//    try
//    {
//        using (OleDbConnection conn = this.getDbConnection())
//        {
//            using (OleDbCommand objCmdInsert = new OleDbCommand(qry, conn))
//            {
//                objCmdInsert.ExecuteNonQuery();
//                returnValue = true;
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
//        // Requires Logging
//    }
//    return returnValue;
//}
#endregion
