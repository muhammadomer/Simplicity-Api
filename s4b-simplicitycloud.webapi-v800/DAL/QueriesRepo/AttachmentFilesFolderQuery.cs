using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AttachmentFilesFolderQuery
    {
        public static string GetAttachFolderStructure(string databaseType)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT afff.sequence, afff.flg_deleted, afff.flg_folder_admin, afff.key_1, afff.key_2, " +
                                      " afff.key_3, afff.key_4, afff.key_5,affj.sequence AS join_sequence, affj.flg_folder,  " +
                                      " affj.parent_folder_id, affj.folder_files_sequence,afff.folder_desc, " +
                                      " (select folder_desc from un_attach_folder_files_folders a, un_attach_folder_files_join b " +
                                      " where a.sequence = b.folder_files_sequence and a.sequence = affj.parent_folder_id) as ParentFolderName " +
                                      " FROM un_attach_folder_files_folders AS afff, un_attach_folder_files_join AS affj " +
                                      " WHERE afff.sequence = affj.folder_files_sequence AND affj.flg_folder = True AND afff.flg_folder_admin = True " +
                                      " ORDER BY affj.parent_folder_id, affj.folder_files_sequence ";
                        break;

                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT afff.sequence, afff.flg_deleted, afff.flg_folder_admin, afff.key_1, afff.key_2, " +
                                      " afff.key_3, afff.key_4, afff.key_5,affj.sequence AS join_sequence, affj.flg_folder,  " +
                                      " affj.parent_folder_id, affj.folder_files_sequence,afff.folder_desc, " +
                                      " (select folder_desc from un_attach_folder_files_folders a, un_attach_folder_files_join b " +
                                      " where a.sequence = b.folder_files_sequence and a.sequence = affj.parent_folder_id) as ParentFolderName " +
                                      " FROM un_attach_folder_files_folders AS afff, un_attach_folder_files_join AS affj " +
                                      " WHERE afff.sequence = affj.folder_files_sequence AND affj.flg_folder = 1 AND afff.flg_folder_admin = 1 " +
                                      " ORDER BY affj.parent_folder_id, affj.folder_files_sequence "; 
                            break;                    
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insertGDriveMapping(string databaseType, GDriveMapping obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO  un_gdrive_file_mapping(file_Name,old_file_id,new_file_id)
                                Values ('" + obj.fileName + "', '" + obj.oldFileId + "','" + obj.newFileId + "')";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
    }
}
