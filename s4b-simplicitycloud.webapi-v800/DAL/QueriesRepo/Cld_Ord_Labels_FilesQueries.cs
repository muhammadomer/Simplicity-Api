using SimplicityOnlineWebApi.Commons;
using System;
using System.Text;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class Cld_Ord_Labels_FilesQueries
    {
        public static string getSelectAllByJoinSequence(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " SELECT i.sequence, i.job_sequence, i.add_info_sequence, i.oi_sequence, i.header_sequence,  i.join_sequence,  i.flg_deleted, file_name_and_path, date_file_date, file_desc, image_url, logo_url, i.created_by, i.date_created, i.last_amended_by, i.date_last_amended, " +
                                      " (SELECT user_name from un_user_details where user_id = i.created_by) AS image_user_name, info.add_info,i.drive_file_id " +
                                      " FROM un_cld_ord_labels_files i " +
                                      " LEFT JOIN un_cld_ord_labels_files_add_info info ON info.sequence = i.add_info_sequence " +
                                      " WHERE i.join_sequence = " + joinSequence + " AND i.flg_deleted <>  "+ Utilities.GetBooleanForDML(databaseType,true);
                        
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT i.sequence,i.job_sequence,i.oi_sequence,i.add_info_sequence,i.header_sequence,i.join_sequence,i.add_info_sequence,i.flg_deleted,i.file_name_and_path, " +
                                " i.date_file_date,i.file_desc,i.created_by,i.date_created,i.last_amended_by,i.date_last_amended,i.image_url, i.logo_url,i.drive_file_id " +
                                " ,(SELECT user_name from un_user_details where user_id = i.created_by) AS image_user_name " +
                                " , info.add_info FROM un_cld_ord_labels_files i LEFT JOIN un_cld_ord_labels_files_add_info info " +
                                " on info.sequence = i.add_info_sequence WHERE i.sequence = " + sequence;
                        
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long sequence, long jobSequence, long oiSequence, long headerSequence, 
                                    long joinSequence,long addInfoSequence, bool flgDeleted, string fileNameAndPath, string imageURL,
                                    string logoURL, DateTime? dateFileDate, 
                                    string fileDesc, int createdBy, DateTime? dateCreated, int lastAmendedBy, 
                                    DateTime? dateLastAmended,string driveFileId)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "INSERT INTO un_cld_ord_labels_files (job_sequence, oi_sequence, " +
                        "      header_sequence,  join_sequence,add_info_sequence, flg_deleted, file_name_and_path, image_url, " +
                        "      logo_url, date_file_date, file_desc,drive_file_id, created_by, date_created, last_amended_by, date_last_amended) " +
                        "VALUES (" + jobSequence + ", " + oiSequence + ", " + headerSequence + ", " +
                                joinSequence + ", " + addInfoSequence + ", " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", '" + fileNameAndPath + "', '" + imageURL + "'," +
                                "'" + logoURL + "', " + Utilities.GetDateTimeForDML(databaseType, dateFileDate,true,true) + ", " +
                                "'" + fileDesc + "','" + driveFileId + "', " + createdBy + ",  "  + Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + ", " +
                                lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string updateDeleteFlagBySequence(string databaseType, long sequence, bool flgDeleted, int lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "UPDATE un_cld_ord_labels_files " +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + "," +
                        "       last_amended_by =  " + lastAmendedBy + "," +
                        "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) +
                        " WHERE sequence = " + sequence;
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string updateAll(string databaseType, long sequence, long jobSequence, long oiSequence, long headerSequence, 
                                       long joinSequence, long addInfoSequence, bool flgDeleted, string fileNameAndPath, 
                                       DateTime? dateFileDate, string fileDesc,  string imageURL, string logoURL,
                                       int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended,string imageName,string driveFileId)
        {
            string returnValue = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(imageName))
                {
                   
                            sb.Append("UPDATE un_cld_ord_labels_files ");
                            sb.Append("   SET job_sequence = " + jobSequence + ",");
                            sb.Append(" oi_sequence = " + oiSequence + ",");
                            sb.Append("       header_sequence = " + headerSequence + ",");
                            sb.Append("       join_sequence = " + joinSequence + ",");
                            if (addInfoSequence > 0) sb.Append("       add_info_sequence = " + addInfoSequence + ",");
                            sb.Append("       flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",");
                            //"       file_name_and_path = '" + fileNameAndPath + "', " +
                            //"       image_url = '" + imageURL + "', " +                              
                            sb.Append("       file_desc = '" + fileDesc + "',");
                            sb.Append("       drive_file_id = '" + driveFileId + "',");
                            sb.Append("       created_by = " + createdBy + ",");
                            sb.Append("       date_created = " + Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + " ,");
                            sb.Append("       date_file_date = " + Utilities.GetDateTimeForDML(databaseType, dateFileDate,true,true) + ",");
                            sb.Append("       last_amended_by = " + lastAmendedBy + ",");
                            sb.Append("       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true));
                            sb.Append(" WHERE sequence = " + sequence);

                            returnValue = sb.ToString();
                    
                    //edit case when no image changes
                    //returnValue = "UPDATE un_cld_ord_labels_files " +
                    //              "   SET job_sequence = " + jobSequence + "," +
                    //              "       oi_sequence = " + oiSequence + "," +
                    //              "       header_sequence = " + headerSequence + "," +
                    //              "       join_sequence = " + joinSequence + "," +
                    //              "       add_info_sequence = " + addInfoSequence + "," +
                    //              "       flg_deleted = " + Utilities.getSQLBoolean(flgDeleted) + "," +
                    //              //"       file_name_and_path = '" + fileNameAndPath + "', " +
                    //              //"       image_url = '" + imageURL + "', " +                              
                    //              "       file_desc = '" + fileDesc + "'," +
                    //              "       created_by = " + createdBy + "," +
                    //              "       date_created = '" + dateCreated.ToString("yyyy-MM-dd HH:mm:ss") + "' ," +
                    //              "       date_file_date = '" + dateFileDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ," +
                    //              "       last_amended_by = " + lastAmendedBy + "," +
                    //              "       date_last_amended = '" + dateLastAmended.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    //              " WHERE sequence = " + sequence;
                }
                else
                {
                   
                            sb.Append("UPDATE un_cld_ord_labels_files ");
                            sb.Append("   SET job_sequence = " + jobSequence + ",");
                            sb.Append("       oi_sequence = " + oiSequence + ",");
                            sb.Append("       header_sequence = " + headerSequence + ",");
                            sb.Append("       join_sequence = " + joinSequence + ",");
                            if (addInfoSequence > 0) sb.Append("       add_info_sequence = " + addInfoSequence + ",");
                            sb.Append("       flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",");
                            sb.Append("       file_name_and_path = '" + fileNameAndPath + "', ");
                            sb.Append("       image_url = '" + imageURL + "', ");
                            sb.Append("       logo_url = '" + logoURL + "', ");
                            sb.Append("       file_desc = '" + fileDesc + "',");
                            sb.Append("       created_by = " + createdBy + ",");
                            sb.Append("       date_created = " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ",");
                            sb.Append("       last_amended_by = " + lastAmendedBy + ",");
                            sb.Append("       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) );
                            sb.Append(" WHERE sequence = " + sequence);

                            returnValue = sb.ToString();
                            

                    //returnValue = "UPDATE un_cld_ord_labels_files " +
                    //              "   SET job_sequence = " + jobSequence + "," +
                    //              "       oi_sequence = " + oiSequence + "," +
                    //              "       header_sequence = " + headerSequence + "," +
                    //              "       join_sequence = " + joinSequence + "," +
                    //              "       add_info_sequence = " + addInfoSequence + "," +
                    //              "       flg_deleted = " + Utilities.getSQLBoolean(flgDeleted) + "," +
                    //              "       file_name_and_path = '" + fileNameAndPath + "', " +
                    //              "       image_url = '" + imageURL + "', " +                              
                    //              "       file_desc = '" + fileDesc + "'," +
                    //              "       created_by = " + createdBy + "," +
                    //              "       date_created = '" + dateCreated.ToString("yyyy-MM-dd HH:mm:ss") + "' ," +
                    //              "       last_amended_by = " + lastAmendedBy + "," +
                    //              "       date_last_amended = '" + dateLastAmended.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    //              " WHERE sequence = " + sequence;
                }

            }
            catch (Exception)
            {
            }
            return returnValue;
        }

        internal static string getSelectAllByJoinSequenceUserAndDateSearch(string databaseType, long joinSequence, bool isTagCreatedDate, DateTime? tagCreatedDate, bool isTagUser, int tagUser)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT sequence, job_sequence, add_info_sequence, oi_sequence, header_sequence,  join_sequence, " +
                              "      flg_deleted, file_name_and_path, date_file_date, file_desc, image_url, logo_url, created_by, " +
                              "      date_created, last_amended_by, date_last_amended, " +
                             "(SELECT user_name from un_user_details where user_id = un_cld_ord_labels_files.created_by) AS image_user_name,drive_file_id " +
                              " FROM un_cld_ord_labels_files " +
                               "WHERE join_sequence = " + joinSequence +
                               "  AND flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true);
                if(isTagCreatedDate)
                {
                    returnValue = returnValue + "   AND date_file_date = " + Utilities.GetDateValueForDML(databaseType, tagCreatedDate) ;
                }
                if (isTagUser)
                {
                    returnValue = returnValue + "   AND created_by = " + tagUser;
                }
            }
            catch (Exception)
            {
            }
            return returnValue;
        }
    }
}

