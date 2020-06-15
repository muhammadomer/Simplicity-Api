using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class PassthroughDB : MainDB
    {
        public PassthroughDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public PassthroughModel getPassthroughByPassthroughString(string passthroughString)
        {
            PassthroughModel returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PassthroughQueries.SelectAllFieldsByPassthroughString(this.DatabaseType, passthroughString), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();                                                                
                                returnValue = LoadAttachmentFoldersPassthrough(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting passthrough model. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public PassthroughModel getPassthroughBySequence(long sequence)
        {
            PassthroughModel returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PassthroughQueries.SelectAllFieldsBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadAttachmentFoldersPassthrough(dr);
                            }
                            else
                            {
                                ErrorMessage = "No Attachment Folder Passthrough Record Found For Sequence '" + sequence + "'";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting passthrough model. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool deletePassthroughBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdDelete =
                        new OleDbCommand(PassthroughQueries.DeleteBySequence(this.DatabaseType, sequence), conn))
                    {
                        objCmdDelete.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while deleting passthrough model. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool insert(out long sequence, string passthroughString, long jobSequence, long jobClientId, long jobAddressId, int entityId, bool flagAdminMode, string componentName, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            sequence = -1;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdInsert = new OleDbCommand(PassthroughQueries.insert(this.DatabaseType, passthroughString, jobSequence, jobClientId, jobAddressId, entityId, flagAdminMode, componentName, createdBy, createdDate), conn))
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
                            returnValue = true;
                        }
                    }
                }
            }

            return returnValue;
        }

        PassthroughModel LoadAttachmentFoldersPassthrough(OleDbDataReader dr)
        {
            PassthroughModel returnValue = null;
            try
            {
                if (dr != null)
                {
                    PassthroughModel passthroughModel = new PassthroughModel();
                    passthroughModel.CreatedBy = dr["created_by"] != null ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    passthroughModel.ComponentName = Utilities.GetDBString(dr["component_name"]);
                    passthroughModel.InternalId = Utilities.GetDBString(dr["internal_id"]);
                    passthroughModel.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    passthroughModel.JobClientId = long.Parse(dr["job_client_id"].ToString());
                    passthroughModel.JobAddressId = long.Parse(dr["job_address_id"].ToString());
                    passthroughModel.EntityId = dr["entity_id"] != null ? Int32.Parse(dr["entity_id"].ToString()) : -1;
                    passthroughModel.PassthroughString = Utilities.GetDBString(dr["passthorugh_string"]);
                    passthroughModel.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue = passthroughModel;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Loading passthrough model. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
    }
}
