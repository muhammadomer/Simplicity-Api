using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefOrderHireDamageTypeDB : MainDB
    {

        public RefOrderHireDamageTypeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<RefOrderHireDamageType> getAllDamageTypes()
        {
            List<RefOrderHireDamageType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefOrdersHireDamageTypeQueries.SelectAll(this.DatabaseType), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<RefOrderHireDamageType>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Type(row));
                            }
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting Order Hire Damage Type. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private RefOrderHireDamageType Load_Type(DataRow row)
        {
            RefOrderHireDamageType types = null;
            try
            {
                if (row != null)
                {

                    types = new RefOrderHireDamageType();
                    types.DamageTypeSequence = DBUtil.GetLongValue(row, "damage_type_sequence");
                    types.DamageTypeDesc = DBUtil.GetStringValue(row, "damage_type_desc");
                    types.RowIndex = DBUtil.GetIntValue(row, "row_index");
                    types.FlgDeleted = DBUtil.GetBooleanValue(row, "flg_deleted");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return types;
        }

    }
}