using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefEntityPaymentTypeDB : MainDB
    {

        public RefEntityPaymentTypeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<RefEntityPaymentType> getAllRefEntityPaymentType()
        {
            List<RefEntityPaymentType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefEntityPaymentTypeQueries.getAllPaymentTypes(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefEntityPaymentType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefEntityPaymentType(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }


       
        private RefEntityPaymentType Load_RefEntityPaymentType(OleDbDataReader dr)

        {
            RefEntityPaymentType refType = null;
            try
            {
                if (dr != null)
                {

                    refType = new RefEntityPaymentType();
                    refType.EntityPaymentId = long.Parse(dr["entity_pymt_id"].ToString());
                    refType.EntityPaymentDesc = Utilities.GetDBString(dr["entity_pymt_desc"]);
                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refType;
        }

    }
}