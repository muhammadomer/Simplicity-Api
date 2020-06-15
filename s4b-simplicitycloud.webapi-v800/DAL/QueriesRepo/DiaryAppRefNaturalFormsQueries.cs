using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class DiaryAppRefNaturalFormsQueries
    {
        internal static string SelectAllFieldsByClientId(string databaseType, long clientId)
        {
            string returnValue = "";
           
            returnValue = @" SELECT  rnf.form_sequence, rnf.row_index, rnf.flg_default, rnf.default_id, rnf.flg_preferred, rnf.form_id, rnf.form_desc,
                rnf.flg_client_specific, rnf.client_id, rfnc.category_sequence, rfnc.flg_compulsory, rfnc.category_desc, rfnc.hyperlink_text,
            FROM un_ref_s4b_forms AS rnf 
                INNER JOIN  un_ref_s4b_forms_categories AS rfnc ON rfnc.category_sequence = rnf.category_sequence
            WHERE rnf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                + " AND rnf.flg_client_specific <> " + Utilities.GetBooleanForDML(databaseType, true)
                + " UNION "
                + @"SELECT rfnc.category_sequence, rfnc.flg_compulsory, rfnc.category_desc,
                rnf.form_sequence, rnf.row_index, rnf.flg_default, rnf.default_id, rnf.flg_preferred, rnf.form_id, rnf.form_desc,
                rnf.hyper_link_label,rnf.flg_client_specific, rnf.client_id
            FROM un_ref_s4b_forms AS rnf 
                INNER JOIN un_ref_s4b_forms_categories AS rfnc ON rfnc.category_sequence = rnf.category_sequence
            WHERE rnf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                + "   AND rnf.flg_client_specific = " + Utilities.GetBooleanForDML(databaseType, true)
                + "   AND rnf.client_id = " + clientId
                + " ORDER BY rfnc.category_desc, rnf.row_index, rnf.form_sequence";
            
            return returnValue;
        }

        internal static string SelectAllFields(string databaseType)
        {
            string returnValue = "";
            returnValue = @"SELECT rnf.form_sequence, rnf.row_index, rnf.flg_default, rnf.default_id, rnf.flg_preferred, rnf.form_id, rnf.form_desc,
                rnf.flg_client_specific, rnf.client_id,rfnc.category_sequence, rfnc.flg_compulsory, rfnc.category_desc, rfnc.hyperlink_text
            FROM un_ref_s4b_forms AS rnf
                INNER JOIN  un_ref_s4b_forms_categories AS rfnc  ON rfnc.category_sequence = rnf.category_sequence
            WHERE rnf.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
            + " ORDER BY rfnc.category_desc, rnf.row_index, rnf.form_sequence";
            return returnValue;
        }
    }
}
