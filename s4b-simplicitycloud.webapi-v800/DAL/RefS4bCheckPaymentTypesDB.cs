using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefS4bCheckPaymentTypesDB : MainDB
    {

        public RefS4bCheckPaymentTypesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public List<RefS4bCheckPaymentTypes> selectAll()
        {
            List<RefS4bCheckPaymentTypes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bCheckPaymentTypesQueries.getSelectAllBySequence(this.DatabaseType,0), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefS4bCheckPaymentTypes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefS4bCheckPaymentTypes(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
      
        private RefS4bCheckPaymentTypes Load_RefS4bCheckPaymentTypes(OleDbDataReader dr)

        {
			RefS4bCheckPaymentTypes record = null;
            try
            {
                if (dr != null)
                {
                    record = new RefS4bCheckPaymentTypes();
                    record.PaymentType = int.Parse(dr["pymt_type"].ToString());
                    record.RowIndex = int.Parse(dr["row_index"].ToString());
                    record.PaymentDesc = Utilities.GetDBString(dr["pymt_desc"]);
                    record.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    record.CreatedBy = long.Parse(dr["created_by"].ToString());
                    record.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    record.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    record.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
				Utilities.WriteLog("Error Occured:" + ErrorMessage);
				throw ex;
			}
            return record;
        }

       
    }
}