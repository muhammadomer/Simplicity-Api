using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL
{
    public class AttachmentFilesFolderDB : MainDB
    {
        public AttachmentFilesFolderDB(DatabaseInfo dbInfo) : base(dbInfo)
        {

        }
        public List<AttachmentFilesFolder> GetAttachFolderStructure()
        {
            List<AttachmentFilesFolder>  returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttachmentFilesFolderQuery.GetAttachFolderStructure(DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AttachmentFilesFolder>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AttachmentFilesFolder(dr));
                                    
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
        private AttachmentFilesFolder Load_AttachmentFilesFolder(OleDbDataReader dr)
        {
            AttachmentFilesFolder attachmentFilesFolder = null;
            try
            {
                if (dr != null)
                {
                    attachmentFilesFolder = new AttachmentFilesFolder();
                    attachmentFilesFolder.Id = dr["sequence"].ToString();
                    attachmentFilesFolder.ParentFolderId = dr["parent_folder_id"].ToString();
                    attachmentFilesFolder.Name = dr["folder_desc"].ToString();
                    attachmentFilesFolder.ParentFolderName = dr["ParentFolderName"].ToString();                    
                    //attachmentFilesFolder.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    //attachmentFilesFolder.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    //attachmentFilesFolder.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return attachmentFilesFolder;
        }

        public GDriveMapping insertGDriveFileMapping(GDriveMapping obj)
        {
            GDriveMapping returnValue = new GDriveMapping();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AttachmentFilesFolderQuery.insertGDriveMapping(this.DatabaseType, obj), conn))
                    {
                        int result = objCmdInsert.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            obj.sequence = sequence;
                            returnValue = obj;
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
    }
}
