using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.BLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefS4bFormsQueries
    {

        public static string getSelectAllByformSequence(string databaseType, long formSequence)
        {
            string returnValue = "";
            try
            {                   
                      returnValue = " SELECT * " +
                                    "  FROM    un_ref_s4b_forms" +
                                    " WHERE form_sequence = " + formSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByFormId(string databaseType, string formId)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      "  FROM  un_ref_s4b_forms" +
                                      " WHERE form_id = '" + formId + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                                "  FROM  un_ref_s4b_forms " +
                                "  Where flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + " ORDER BY row_index ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getDeletTemplate(string databaseType)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                                      "  FROM  un_ref_s4b_forms " +
                                      "  Where flg_deleted = " + Utilities.GetBooleanForDML(databaseType, true) + " ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        //public static string insert(string databaseType, bool flgDeleted, bool flgDefault, long defaultId,  bool flgPreferred, long rowIndex,  
        //                            string formId,  string formDesc, string EmailTo, string CCEMailAddress, string BCCEmailAddess, bool Flgaddzipphotos, long categorySequence,  bool flgClientSpecific, long clientId,  long createdBy,  DateTime? dateCreated, 
        //                            long lastAmendedBy,  DateTime? dateLastAmended,bool flgPrePopulet,bool flgLaunchFromHome,bool flgLaunchFromApps, bool isAssetRequired, bool isSupplierRequired, string prePopulationSql)
        //{
        //    string returnValue = "";
        //    try
        //    {

        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "INSERT INTO   un_ref_s4b_forms(flg_deleted,  flg_default,  default_id,  flg_preferred,  row_index,  form_id," +
        //                            "                                   form_desc, category_sequence,  flg_client_specific,  client_id,  created_by,  date_created," +
        //                            "                                   last_amended_by,  date_last_amended,flg_pre_populate,flg_launch_from_home,flg_launch_from_apps,flg_asset_required,flg_supplier_required,pre_population_sql, email_to, email_copy, email_bcc,flg_add_zip_photos)" +
        //                            "VALUES (" + flgDeleted + ",   " + flgDefault + ",   " + defaultId + ",   " + flgPreferred + ",   " +
        //                            rowIndex + ",   '" + formId + "',   '" + formDesc + "',  " + categorySequence + ",   " + flgClientSpecific + ",  " + clientId + ",  " +
        //                            createdBy + ",   " + Utilities.getAccessDate(DateTime.Now) + ",  " + lastAmendedBy + ",   " + Utilities.getAccessDate(DateTime.Now) + "," + flgPrePopulet + ", " + flgLaunchFromHome + "," +flgLaunchFromApps + "," + isAssetRequired + "," + isSupplierRequired + ",'" + Utilities.getDBSqlString(prePopulationSql) + "' ,   '" + EmailTo + "',   '" + BCCEmailAddess + "',   '" + CCEMailAddress + "'," + Utilities.getSQLBoolean(Flgaddzipphotos) + ")";
        //                break;

        //            case "SQLSERVER":
        //            default:
        //                returnValue = "INSERT INTO   un_ref_s4b_forms(flg_deleted,  flg_default,  default_id,  flg_preferred,  row_index,  form_id," +
        //                            "                                   form_desc, category_sequence,  flg_client_specific,  client_id,  created_by,  date_created," +
        //                            "                                   last_amended_by,  date_last_amended,flg_pre_populate,flg_launch_from_home,flg_launch_from_apps,flg_asset_required,flg_supplier_required,pre_population_sql, email_to, email_copy, email_bcc,flg_add_zip_photos)" +
        //                            "VALUES (" + Utilities.getSQLBoolean(flgDeleted) + ",   " + Utilities.getSQLBoolean(flgDefault) + ",   " + defaultId + ",   " + Utilities.getSQLBoolean(flgPreferred) + ",   " +
        //                            rowIndex + ",   '" + formId + "',   '" + formDesc + "'," + categorySequence + ",   " + Utilities.getSQLBoolean(flgClientSpecific) + ",  " + clientId + ",  " +
        //                            createdBy + ",   " + Utilities.getSQLDate(dateCreated) + ",  " + lastAmendedBy + ",   " + Utilities.getSQLDate(dateLastAmended) + "," + Utilities.getSQLBoolean(flgPrePopulet) + ", " + Utilities.getSQLBoolean(flgLaunchFromHome) + "," + Utilities.getSQLBoolean(flgLaunchFromApps) + "," + Utilities.getSQLBoolean(isAssetRequired) + "," + Utilities.getSQLBoolean(isSupplierRequired) + ",'" + Utilities.getDBSqlString(prePopulationSql) + "' ,   '" + EmailTo + "',   '" + BCCEmailAddess + "',   '" + CCEMailAddress + "'," + Utilities.getSQLBoolean(Flgaddzipphotos) + ")";
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}

        public static string insert(string databaseType, RefS4bForms obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO   un_ref_s4b_forms(  default_id,    row_index,  form_id
                    ,form_desc, category_sequence,  flg_client_specific,  client_id
                    ,pre_population_sql, email_to, email_copy, email_bcc
                    ,flg_deleted,  flg_default,flg_preferred
                   ,flg_pre_populate,flg_launch_from_home,flg_launch_from_apps,flg_asset_required,flg_supplier_required
                   ,flg_add_zip_photos 
                   ,  created_by,  date_created,last_amended_by,  date_last_amended
                  )
                VALUES (" + obj.DefaultId
                + ", " + obj.RowIndex
                + ", '" + obj.FormId + "'" 
                + ", '" + obj.FormDesc + "'"
                + ",  " + obj.CategorySequence
                + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgClientSpecific)
                + ",  " + obj.ClientId
                + ",'" + Utilities.getDBSqlString(obj.PrePopulationSql) + "'"
                + " ,  '" + obj.EmailTo + "'"
                + ",   '" + obj.CCEMailAddress + "'"
				+ ",   '" + obj.BCCEmailAddess + "'"
				+ ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted)
                + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgDefault)
                + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgPreferred)
                + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgPrePopulate)
                + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgLaunchFromHome)
                + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgLaunchFromApps)
                + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgAssetRequired)
                + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgSupplierRequired)
                + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgAddZipPhotos)
                + ",  " + obj.CreatedBy 
                + ",   " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true)
                + ",  " + obj.LastAmendedBy 
                + ",   " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true)
                + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string update(string databaseType, RefS4bForms obj)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_ref_s4b_forms" +
                    " SET flg_deleted = " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ",  " +
                    " flg_default = " + Utilities.GetBooleanForDML(databaseType, obj.FlgDefault) + ",  " +
                    " default_id =  " + obj.DefaultId + ",  " +
                    " flg_preferred = " + Utilities.GetBooleanForDML(databaseType, obj.FlgPreferred) + ",  " +
                    " row_index =  " + obj.RowIndex + ",  " +
                    " form_id =  '" + obj.FormId + "',  " +
                    " form_desc =  '" + obj.FormDesc + "',  " +
                    " category_sequence =  " + obj.CategorySequence + ",  " +
                    " flg_client_specific = " + Utilities.GetBooleanForDML(databaseType, obj.FlgClientSpecific) + ",  " +
                    " client_id =  " + obj.ClientId + ",  " +
                    " last_amended_by =  " + obj.LastAmendedBy + ",  " +
                    " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + "," +
                    " flg_pre_populate =  " + Utilities.GetBooleanForDML(databaseType, obj.FlgPrePopulate) + "," +
                    " flg_launch_from_home =  " + Utilities.GetBooleanForDML(databaseType, obj.FlgLaunchFromHome) + "," +
                    " flg_launch_from_apps =  " + Utilities.GetBooleanForDML(databaseType, obj.FlgLaunchFromApps) + ", " +
                    " flg_asset_required =  " + Utilities.GetBooleanForDML(databaseType, obj.FlgAssetRequired) + ", " +
                    " flg_supplier_required =  " + Utilities.GetBooleanForDML(databaseType, obj.FlgSupplierRequired) + ", " +
                    " pre_population_sql =  '" + Utilities.getDBSqlString(obj.PrePopulationSql) + "', " +
                    " email_to =  '" + obj.EmailTo + "',  " +
                    " email_copy =  '" + obj.CCEMailAddress + "',  " +
                    " email_bcc =  '" + obj.BCCEmailAddess + "',  " +
                    " flg_add_zip_photos =  '" + Utilities.GetBooleanForDML(databaseType, obj.FlgAddZipPhotos) + "'  " +
                    " WHERE form_sequence = " + obj.FormSequence;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public static string delete(string databaseType, long formSequence)
        {
            string returnValue = "";
            try
            {
              switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
             
                       returnValue = " DELETE FROM   un_ref_s4b_forms" +
                                     " WHERE form_sequence = " + formSequence;
               
                    break;
                }
            } 
       
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long formSequence)
        {
            string returnValue = "";
            try
            {
               
                    bool flg = true;
                    returnValue = " UPDATE   un_ref_s4b_forms" +
                                  "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg )+ " " +
                                  " WHERE form_sequence = " + formSequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string undeleteFlagDeleted(string databaseType, long formSequence)
        {
            string returnValue = "";
            try
            {
                
                        bool flg = false;
                        returnValue = " UPDATE   un_ref_s4b_forms" +
                                      "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + " " +
                                      " WHERE form_sequence = " + formSequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string setTempDefault(string databaseType, long formSequence)
        {
            string returnValue = "";
            try
            {
                
                        bool flg = true;
                        returnValue = " UPDATE   un_ref_s4b_forms" +
                                      "   SET flg_default =  " + Utilities.GetBooleanForDML(databaseType, flg) + " " +
                                      " WHERE form_sequence = " + formSequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string getSelectById(string databaseType, long Id)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      "  FROM  un_ref_s4b_forms" +
                                      " WHERE form_sequence = " + Id + " ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByUserIdAndAmendedDate(string databaseType, long userId, DateTime? lastSynDate)
        {
            return @"SELECT un_ref_s4b_forms.*
                       FROM (un_ref_s4b_forms 
                      INNER JOIN un_s4b_forms_assign ON un_s4b_forms_assign.form_sequence = un_ref_s4b_forms.form_sequence)
                      INNER JOIN un_user_details ON un_s4b_forms_assign.user_id  = un_user_details.user_id
                      WHERE un_s4b_forms_assign.user_id = " + userId +
                    "   AND un_ref_s4b_forms.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +                    
                    "   AND (un_ref_s4b_forms.date_last_amended > " + Utilities.GetDateTimeForDML(databaseType, lastSynDate, true, true) +
                    "    OR un_s4b_forms_assign.date_last_amended > " + Utilities.GetDateTimeForDML(databaseType, lastSynDate, true, true) + " )";
        }

        public static string SelectAllByUserId(string databaseType, long userId)
        {
            return @"SELECT un_ref_s4b_forms.* 
                       FROM (un_ref_s4b_forms
                      INNER JOIN un_s4b_forms_assign ON un_s4b_forms_assign.form_sequence = un_ref_s4b_forms.form_sequence)
                      INNER JOIN un_user_details ON un_s4b_forms_assign.user_id  = un_user_details.user_id
                      WHERE un_s4b_forms_assign.user_id = " + userId +
                    "   AND un_ref_s4b_forms.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
        }

        public static string updateLastAmendedDate(string databaseType, long formSequence, DateTime? dateLastAmended)
        {
            string returnValue = "";
            
                    returnValue = "UPDATE un_ref_s4b_forms" +
                            "   SET date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) +
                            " WHERE form_sequence = " + formSequence;

            return returnValue;
        }

        public static string insertMapping(string databaseType, long formSequence, string fieldName, int fieldValueType, string fieldValue, long createdBy, DateTime? dateCreated)
        {
            string returnValue = "";
            
                    returnValue = "INSERT INTO   un_ref_s4b_mapping(form_sequence,field_name, field_value_type,field_value,created_by,date_created)" +
                                "VALUES (" + formSequence + ",   '" + fieldName + "',   " + fieldValueType + ",   '" + fieldValue + "',   " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ")";
                  

            return returnValue;
        }

        public static string updateMapping(string databaseType, long sequence, long formSequence, string fieldName, int fieldValueType, string fieldValue, long amendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";

                    returnValue = "UPDATE un_ref_s4b_mapping " + 
                        " SET form_sequence = " + formSequence +
                        " ,field_name = '" + fieldName + "'" +
                        " , field_value_type = " + fieldValueType + 
                        ",field_value = '" + fieldValue + "'" +
                        " ,last_amended_by = " + amendedBy +
                        " ,date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + 
                        " where sequence = " + sequence;

            return returnValue;
        }

        public static string deleteMapping(string databaseType, long sequence)
        {
            string returnValue = "";
            returnValue = "DELETE from un_ref_s4b_mapping Where sequence = " + sequence;
            return returnValue;
        }
    }
}

