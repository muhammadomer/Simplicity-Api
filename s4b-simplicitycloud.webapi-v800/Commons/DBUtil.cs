using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Data;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.Commons
{
    public class DBUtil
    {
        public static string ErrorMessage { get; set; }

        public static string OrderSearchQueryBuilder(string key, string field, string match)
        {
            string tableName = GetTableName(field);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (key == null || field == null || match == null)
            {
                sb.Append("SELECT * FROM");
                sb.Append(" ");
                sb.Append(GetTableName(field));
                sb.Append(" ");
                sb.Append("ORDER BY job_date DESC ");
                Console.Write(sb.ToString());
                return sb.ToString();
            }
            sb.Append("SELECT * FROM");
            sb.Append(" ");
            sb.Append(GetTableName(field));
            sb.Append(" ");
            sb.Append("WHERE");
            sb.Append(" ");
            sb.Append(field);
            sb.Append(" ");
            sb.Append("LIKE");
            sb.Append(" ");
            sb.Append(buildMatchString(key, match));
            sb.Append(" ");
            sb.Append("ORDER BY job_date DESC ");
            Console.Write(sb.ToString());
            return sb.ToString();
        }

        public static string FilterQueryBuilder(string key, string field, string match)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(" ");
            sb.Append(field);
            sb.Append(" ");
            sb.Append("LIKE");
            sb.Append(" ");
            sb.Append(buildMatchString(key, match));
            sb.Append(" ");
            Console.Write(sb.ToString());
            return sb.ToString();
        }

        public static string S4BFormSubmissionsQueryBuilder(string searchText, string field1, string field2, string field3)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(" ");
            sb.Append(field1);
            sb.Append(" ");
            sb.Append("LIKE");
            sb.Append("'%" + searchText + "%'");
            sb.Append(" ");
            sb.Append("OR");
            sb.Append(" ");
            sb.Append(field2);
            sb.Append(" ");
            sb.Append("LIKE");
            sb.Append("'%" + searchText + "%'");
            sb.Append(" ");
            sb.Append("OR");
            sb.Append(" ");
            sb.Append(field3);
            sb.Append(" ");
            sb.Append("LIKE");
            sb.Append("'%" + searchText + "%'");
            sb.Append(" ");
            Console.Write(sb.ToString());
            return sb.ToString();
        }

        private static string GetTableName(string field)
        {
            return "un_orders";
        }

        private static string buildMatchString(string key, string match)
        {
            string matchString = "'%" + key + "%'";
            switch (match)
            {
                case "any":
                    return matchString;
                case "exact":
                    matchString = "'" + key + "'";
                    return matchString;
                case "start":
                    matchString = "'" + key + "%'";
                    return matchString;
                case "end":
                    matchString = "'%" + key + "'";
                    return matchString;
                default:
                    return matchString;
            }
        }

        internal static long GetLongValue(DataRow row, string colName)
        {
            long returnValue = -1;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = long.Parse(row[colName].ToString());
                }
                catch (Exception ex)
                { }
            }
            return returnValue;
        }

        internal static int GetIntValue(DataRow row, string colName)
        {
            int returnValue = -1;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = int.Parse(row[colName].ToString());
                }
                catch (Exception ex)
                { }
            }
            return returnValue;
        }

        internal static double GetDoubleValue(DataRow row, string colName)
        {
            double returnValue = 0;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = double.Parse(row[colName].ToString());
                }
                catch (Exception ex)
                { }
            }
            return returnValue;
        }

        internal static decimal GetDecimalValue(DataRow row, string colName)
        {
            decimal returnValue = 0;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = decimal.Parse(row[colName].ToString());
                }
                catch (Exception ex)
                { }
            }
            return returnValue;
        }

        internal static string GetStringValue(DataRow row, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetStringValue()";
            ErrorMessage = "";
            string returnValue = null;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = row[colName].ToString();
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting String Value.", ex);
                }
            }
            return returnValue;
        }

        internal static bool GetBooleanValue(DataRow row, string colName)
        {
            bool returnValue = false;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = bool.Parse(row[colName].ToString());
                }
                catch (Exception ex)
                { }
            }
            return returnValue;
        }

        internal static DateTime? GetDateValue(DataRow row, string colName)
        {
            DateTime? returnValue = null;
            if (row != null && IsColumnExists(row, colName))
            {
                if (row[colName] != null && !string.IsNullOrWhiteSpace(row[colName].ToString()))
                {
                    try
                    {
                        returnValue = DateTime.Parse(row[colName].ToString());
                    }
                    catch (Exception ex)
                    { }
                }
            }
            return returnValue;
        }

        internal static DateTime? GetDateTimeValue(DataRow row, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetDateTimeValue()";
            ErrorMessage = "";
            DateTime? returnValue = null;
            if (row[colName] != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = DateTime.Parse(row[colName].ToString());
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting DateTime Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static string GetDateTimeValueStr(DataRow row, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetDateTimeValueStr()";
            ErrorMessage = "";
            string returnValue = null;
            if (row != null && IsColumnExists(row, colName))
            {
                try
                {
                    returnValue = Convert.ToDateTime(row[colName].ToString()).ToString("dd/MM/yyyy");
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting DateTime Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }
        
        static bool IsColumnExists(DataRow row, string colName)
        {
            bool returnValue = false;
            if (row != null)
            {
                returnValue = row.Table.Columns.Contains(colName);
            }
            return returnValue;
        }

        public static bool IsColumnExists(OleDbDataReader reader, string column)
        {
            if (reader == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(column, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                return false;
            }
        }

        internal static string GetStringValue(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetStringValue()";
            ErrorMessage = "";
            string returnValue = null;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    returnValue = (dr[colName] == null || dr[colName] == DBNull.Value) ? "" : dr[colName].ToString(); ;
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting String Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static long GetLongValue(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetLongValue()";
            ErrorMessage = "";
            long returnValue = -1;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    returnValue = (dr[colName] == null || dr[colName] == DBNull.Value) ? -1 : long.Parse(dr[colName].ToString());
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting Long Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static DateTime? GetDateTimeValue(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetDateTimeValue()";
            ErrorMessage = "";
            DateTime? returnValue = null;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    if (dr[colName] == null || dr[colName] == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return DateTime.Parse(dr[colName].ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting DateTime Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }
        
        internal static bool GetBooleanValue(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetBooleanValue()";
            ErrorMessage = "";
            bool returnValue = false;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    returnValue = (dr[colName] == null || dr[colName] == DBNull.Value) ? false : bool.Parse(dr[colName].ToString());
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting Boolean Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static bool GetBooleanValueForSql(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetBooleanValue()";
            ErrorMessage = "";
            bool returnValue = false;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    returnValue = (dr[colName] == null || dr[colName] == DBNull.Value) ? false : bool.Parse(dr[colName].ToString() == "1" ? "True" : "False");
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting Boolean Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static double GetDoubleValue(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetDoubleValue()";
            ErrorMessage = "";
            double returnValue = 0;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    returnValue = (dr[colName] == null || dr[colName] == DBNull.Value) ? 0 : double.Parse(dr[colName].ToString());
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting Double Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static int GetIntValue(OleDbDataReader dr, string colName)
        {
            const string METHOD_NAME = "DBUtil.GetIntValue()";
            ErrorMessage = "";
            int returnValue = -1;
            if (dr != null && IsColumnExists(dr, colName))
            {
                try
                {
                    returnValue = (dr[colName] == null || dr[colName] == DBNull.Value) ? 0 : Int32.Parse(dr[colName].ToString());
                }
                catch (Exception ex)
                {
                    ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting Int Value for Field " + colName, ex);
                }
            }
            return returnValue;
        }

        internal static string GetSearchWildChar(string databaseType)
        {
            string returnValue = "*";
            if (databaseType.Equals(SimplicityConstants.DB_MSSQLSERVER, StringComparison.InvariantCultureIgnoreCase))
            {
                returnValue = "%";
            }
            return returnValue;
        }
    }
}
