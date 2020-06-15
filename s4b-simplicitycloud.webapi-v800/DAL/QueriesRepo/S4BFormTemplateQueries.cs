using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class S4BFormTemplateQueries
    {
        internal static string SelectAllFieldsOfSubmissionDataBySequence(string datebaseType, long sequence)
        {
            string returnValue = @"SELECT s4b_data2.sequence, s4b_data2.join_sequence, s4b_data2.page_number, s4b_data2.field_name, s4b_data2.field_data
                , s4b_data2.field_position, s4b_data2.field_type,created_by,date_created,last_amended_by, date_last_amended
            FROM un_s4b_submissions_data_2 AS s4b_data2
            WHERE s4b_data2.join_sequence=" + sequence
             +" ORDER BY s4b_data2.page_number, s4b_data2.field_position";
            return returnValue;
        }

        internal static string SelectSubmissionDataByFieldName(string datebaseType, string fieldName)
        {
            string returnValue = @"SELECT s4b_data2.sequence, s4b_data2.join_sequence, s4b_data2.page_number, s4b_data2.field_name, s4b_data2.field_data
                , s4b_data2.field_position, s4b_data2.field_type,created_by,date_created,last_amended_by, date_last_amended
            FROM un_s4b_submissions_data_2 AS s4b_data2
            WHERE s4b_data2.field_name='" + fieldName + "'";
            return returnValue;
        }
        internal static string UpdateTemplateData(string datebaseType,long joinSequence,long userId, string fieldName,string value)
        {

            string returnValue = @"Update un_s4b_submissions_data_2 Set
                field_data = '" + value + "'"
                + ",last_amended_by=" + userId
                + ", date_last_amended=" + Utilities.GetDateTimeForDML(datebaseType, DateTime.Now ,true,true)
                + " WHERE field_name ='" + fieldName + "' And join_sequence=" + joinSequence;
            return returnValue;
        }
        internal static string InsertFlgShowPriceTemplateData(string datebaseType, long joinSequence, long userId, string fieldName, string value)
        {

            string returnValue = @"INSERT INTO un_s4b_submissions_data_2(join_sequence, page_number, field_name, field_data, field_position, field_type ,created_by, date_created) " +
                           " VALUES (" + joinSequence
                                   + ", 4" 
                                   + ",'" + fieldName + "'"
                                   + ", 'False'" 
                                   + ", 3"  
                                   + ",'checkbox'"
                                   + "," + userId
                                   + ", " + Utilities.GetDateTimeForDML(datebaseType, DateTime.Now,true,true) + ")";
            return returnValue;
        }
    }
}
