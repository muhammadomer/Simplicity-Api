using System;

namespace SimplicityOnlineWebApi.ClientInvoice.Utills
{
    public class DB_TAG_UTILLS
    {
        internal static string GetTagAsPerDbType(string databaseType, string tag)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = GetMsAccessTageName(tag);
                        break;
                    case "SQLSERVER":
                        returnValue = GetSqlServerTageName(tag);
                        break;
                    default:
                        returnValue = "";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string GetMsAccessTageName(string tag)
        {
            string returnValue = "";
            switch (tag)
            {
                case "CURRENT_DATE_TAG":
                    returnValue = "Now()";
                    break;
                case "DATE_DIFF_DAY_RANGE":
                    returnValue = "'d'";
                    break;
                case "BOOLEAN_TRUE_VALUE":
                    returnValue = "TRUE";
                    break;
                case "BOOLEAN_FALSE_VALUE":
                    returnValue = "FALSE";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        internal static string GetSqlServerTageName(string tag)
        {
            string returnValue = "";
            switch (tag)
            {
                case "CURRENT_DATE_TAG":
                    returnValue = "GETDATE()";
                    break;
                case "DATE_DIFF_DAY_RANGE":
                    returnValue = "day";
                    break;
                case "BOOLEAN_TRUE_VALUE":
                    returnValue = "1";
                    break;
                case "BOOLEAN_FALSE_VALUE":
                    returnValue = "0";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        internal static string GetLikeQuery(string databaseType, string fieldName,string searchValue)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = fieldName + " Like '*" + searchValue.Trim() + "*' ";
                        break;
                    case "SQLSERVER":
                        returnValue = fieldName + " Like '%" + searchValue.Trim() + "%'";
                        break;
                    default:
                        returnValue = "";
                        break;
                }
            }
            catch { }
            return returnValue;
        }
    }
}
