using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class Cld_Ord_Labels_FilesDB : MainDB
    {

        public Cld_Ord_Labels_FilesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertCld_Ord_Labels_Files(out long sequence, long jobSequence, long oiSequence, long headerSequence,
                                                     long joinSequence,long addInfoSequence, bool flgDeleted, string fileNameAndPath, 
                                                     string imageURL, string logoURL, DateTime? dateFileDate,
                                                     string fileDesc, int createdBy, DateTime? dateCreated, int lastAmendedBy,
                                                     DateTime? dateLastAmended,string driveFileId)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(Cld_Ord_Labels_FilesQueries.insert(this.DatabaseType, -1, jobSequence, oiSequence, headerSequence,
                                     joinSequence,addInfoSequence, flgDeleted, fileNameAndPath, imageURL, logoURL, dateFileDate,
                                     fileDesc, createdBy, dateCreated, lastAmendedBy,
                                     dateLastAmended,driveFileId), conn))
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
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public List<Cld_Ord_Labels_Files> selectCld_Ord_Labels_FilesByJoinSequence(long joinSequence)
        {
            List<Cld_Ord_Labels_Files> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_FilesQueries.getSelectAllByJoinSequence(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Cld_Ord_Labels_Files>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Cld_Ord_Labels_Files(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        Cld_Ord_Labels_Files Load_Cld_Ord_Labels_Files(OleDbDataReader dr)
        {
            Cld_Ord_Labels_Files returnValue = null;
            try
            { 
                if(dr!=null)
                {
                    Cld_Ord_Labels_Files oiFireProtectionIImages = new Cld_Ord_Labels_Files();
                    oiFireProtectionIImages.Sequence = long.Parse(dr["sequence"].ToString());
                    oiFireProtectionIImages.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    oiFireProtectionIImages.OiSequence = (int)dr["oi_sequence"];
                    oiFireProtectionIImages.HeaderSequence = (int)dr["header_sequence"];
                    oiFireProtectionIImages.JoinSequence = (int)dr["join_sequence"];
                    oiFireProtectionIImages.FlgDeleted = (bool)dr["flg_deleted"];
                    oiFireProtectionIImages.FileDate = Utilities.getDBDate(dr["date_file_date"]);
                    oiFireProtectionIImages.FileDesc = Utilities.GetDBString(dr["file_desc"]);
                    oiFireProtectionIImages.ImageURL = Utilities.GetDBString(dr["image_url"]);
                    if(dr["logo_url"] == DBNull.Value || dr["logo_url"] == null)
                    {
                        oiFireProtectionIImages.LogoURL = Utilities.GetDBString(dr["image_url"]);
                    }
                    else
                    { 
                        oiFireProtectionIImages.LogoURL = Utilities.GetDBString(dr["logo_url"]);
                    }
                    oiFireProtectionIImages.FileNameAndPath = Utilities.GetDBString(dr["file_name_and_path"]);
                    oiFireProtectionIImages.CreatedBy = (int)dr["created_by"];
                    oiFireProtectionIImages.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    oiFireProtectionIImages.LastAmendedBy = (int)dr["last_amended_by"];
                    oiFireProtectionIImages.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
                    oiFireProtectionIImages.ImageUser = Utilities.GetDBString(dr["image_user_name"]);

                    oiFireProtectionIImages.AddInfoSequence = dr["add_info_sequence"].ToString().Equals("") ? 0 : long.Parse(dr["add_info_sequence"].ToString());
                    oiFireProtectionIImages.AddInfo = ColumnExists(dr, "add_info") ? Utilities.GetDBString(dr["add_info"]) : "";
                    oiFireProtectionIImages.DriveFileId = ColumnExists(dr, "drive_file_id") ? Utilities.GetDBString(dr["drive_file_id"]) : "";

                    returnValue = oiFireProtectionIImages;
                }
            }
            catch(Exception ex)
            {
            }
            return returnValue;
        }

        internal Cld_Ord_Labels_Files selectCld_Ord_Labels_FilesBySequence(long sequence)
        {
            Cld_Ord_Labels_Files returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_FilesQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Cld_Ord_Labels_Files(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateAll(long sequence, long jobSequence, long oiSequence, long headerSequence, long joinSequence, long addInfoSequence, 
                                bool flgDeleted, string fileNameAndPath, string imageURL, string logoURL, DateTime? dateFileDate, string fileDesc, 
                                int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended,string imageName,string driveFileId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Cld_Ord_Labels_FilesQueries.updateAll(this.DatabaseType, sequence, jobSequence, oiSequence, headerSequence,
                                                                                   joinSequence, addInfoSequence, flgDeleted, fileNameAndPath, dateFileDate,
                                                                                   fileDesc, imageURL, logoURL, createdBy, dateCreated, lastAmendedBy, dateLastAmended, imageName,driveFileId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateDeleteFlagBySequence(long sequence, bool flgDeleted, int lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Cld_Ord_Labels_FilesQueries.updateDeleteFlagBySequence(this.DatabaseType, sequence, flgDeleted, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal List<Cld_Ord_Labels_Files> selectCld_Ord_Labels_FilesByJoinSequenceUserAndDateSearch(long joinSequence, bool isTagCreatedDate, DateTime? tagCreatedDate, bool isTagUser, int tagUser)
        {
            List<Cld_Ord_Labels_Files> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_FilesQueries.getSelectAllByJoinSequenceUserAndDateSearch(this.DatabaseType, joinSequence, isTagCreatedDate, tagCreatedDate, isTagUser, tagUser), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Cld_Ord_Labels_Files>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Cld_Ord_Labels_Files(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
