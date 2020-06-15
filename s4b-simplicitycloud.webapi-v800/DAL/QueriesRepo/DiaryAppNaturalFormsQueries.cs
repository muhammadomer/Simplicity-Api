using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class DiaryAppNaturalFormsQueries
    {
        internal static string SelectAllDANFByFormSequence(string databaseType,long deSequence,  long formSequence)
        {
            string returnValue = @"SELECT DISTINCT danf.sequence, danf.diary_apps_sequence, rnf.form_sequence, rnf.row_index,
                rnf.flg_default, rnf.default_id, rnf.flg_preferred, rnf.form_id, rnf.form_desc, rnf.flg_client_specific, rnf.client_id,
                rnfc.category_sequence, rnfc.category_desc
            FROM (un_diary_apps_natural_forms AS danf 
                INNER JOIN un_ref_s4b_forms AS rnf ON danf.form_sequence = rnf.form_sequence)
                INNER JOIN un_ref_s4b_forms_categories AS rnfc  ON rnf.category_sequence = rnfc.category_sequence
            WHERE danf.diary_apps_sequence = " + deSequence
                + " AND rnf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + " AND rnfc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                + " AND danf.form_sequence = " + formSequence;
            returnValue += " ORDER BY rnfc.category_desc, rnf.row_index, rnf.form_sequence";
            return returnValue;
        }

        internal static string SelectUnassignedDANFOfDESequence(string databaseType, long deSequence)
        {
         string returnValue = @"SELECT rsf.form_sequence, rsf.form_id, rsf.form_desc, rsf.row_index, rsf.flg_client_specific, rsf.client_id
               , rsf.category_sequence, rsfc.category_desc
            FROM un_ref_s4b_forms AS rsf
               INNER JOIN un_ref_s4b_forms_categories AS rsfc ON rsf.category_sequence = rsfc.category_sequence
            WHERE rsf.form_sequence NOT IN (SELECT danf.form_sequence FROM un_diary_apps_natural_forms AS danf WHERE danf.diary_apps_sequence = " + deSequence + @" )
               And rsf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
           + " ORDER BY rsf.row_index";
            //string returnValue = @"SELECT DISTINCT  rnf.form_sequence, rnf.form_id, rnf.form_desc,rnf.row_index, rnf.flg_client_specific, rnf.client_id,
            //    rnfc.category_sequence, rnfc.category_desc
            //FROM ( un_ref_s4b_forms AS rnf
            //    Left JOIN un_diary_apps_natural_forms AS danf  ON danf.form_sequence = rnf.form_sequence)
            //    INNER JOIN un_ref_s4b_forms_categories AS rnfc  ON rnf.category_sequence = rnfc.category_sequence
            //WHERE  danf.form_sequence not in (Select form_sequence from un_diary_apps_natural_forms where  diary_apps_sequence  = " + deSequence + @" )
            //    AND rnf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + " AND rnfc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);

         //returnValue += " ORDER BY rnfc.category_desc, rnf.row_index, rnf.form_sequence";
         return returnValue;
        }
        internal static string SelectAllDANFByDESequence(string databaseType, long de_Sequence)
        {
            string returnValue = @"SELECT DISTINCT danf.sequence, danf.diary_apps_sequence, rnf.form_sequence, rnf.row_index,
                rnf.flg_default, rnf.default_id, rnf.flg_preferred, rnf.form_id, rnf.form_desc, rnf.flg_client_specific, rnf.client_id,
                rnfc.category_sequence, rnfc.category_desc
            FROM (un_diary_apps_natural_forms AS danf 
                INNER JOIN un_ref_s4b_forms AS rnf ON danf.form_sequence = rnf.form_sequence)
                INNER JOIN un_ref_s4b_forms_categories AS rnfc  ON rnf.category_sequence = rnfc.category_sequence
            WHERE danf.diary_apps_sequence = " + de_Sequence
                + " AND rnf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + " AND rnfc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            returnValue += " ORDER BY rnf.row_index";
            return returnValue;
        }
        internal static string InsertDiaryAppsNaturalForms(string databaseType, DiaryAppNaturalForm naturalForms)
        {
            string returnValue = @"INSERT INTO un_diary_apps_natural_forms( diary_apps_sequence, form_sequence, created_by, date_created)
            Values(" + naturalForms.DeSequence + "," + naturalForms.FormSequence + "," + naturalForms.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, DateTime.Now,true,true) + ")";
            return returnValue;
        }
        internal static string InsertPasteDiaryAppsNaturalForms (string databaseType, DiaryAppNaturalForm naturalForms)
        {
            string returnValue = @"INSERT INTO un_diary_apps_natural_forms(diary_apps_sequence, form_sequence,, created_by, date_created)
                SELECT " + naturalForms.DeSequence + ", AS diary_apps_sequence, form_sequence," + naturalForms.CreatedBy + " AS created_by," + Utilities.GetDateTimeForDML(databaseType, DateTime.Now, true, true) 
                + " FROM un_diary_apps_natural_forms  WHERE diary_apps_sequence = " + naturalForms.DeSequence;
            return returnValue;
        }
        internal static string InsertTFRFromUnscheduled(string databaseType, long de_Sequence,  long de_SequenceUnscheduled, long userId)
        {
            string returnValue = @"INSERT INTO un_diary_apps_natural_forms(diary_apps_sequence, form_sequence,, created_by, date_created)
                SELECT " + de_Sequence + ", AS diary_apps_sequence, daunf.form_sequence," + userId + " AS created_by," + Utilities.GetDateTimeForDML(databaseType, DateTime.Now, true, true) 
                + " FROM un_diary_apps_unsched_nf AS daunf WHERE daunf.unscheduled_de_seq =  " + de_SequenceUnscheduled;
            return returnValue;
        }
        internal static string DeleteDiaryAppsNaturalFormsbyDeSequence(string databaseType, long de_Sequence)
        {
            string returnValue = @"DELETE FROM un_diary_apps_natural_forms
            WHERE diary_apps_sequence = " + de_Sequence;
            return returnValue;
        }
        internal static string DeleteDiaryAppsNaturalFormsbySequence(string databaseType, long sequence)
        {
            string returnValue = @"DELETE FROM un_diary_apps_natural_forms Where sequence = " + sequence;
            return returnValue;
        }

    }
}
