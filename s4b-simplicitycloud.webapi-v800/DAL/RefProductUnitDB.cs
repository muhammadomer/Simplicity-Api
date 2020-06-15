using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefProductUnitDB : MainDB
    {

        public RefProductUnitDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<RefProductUnits> selectProductUnit(out int count)
        {
            List<RefProductUnits> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefProductUnitQueries.selectProductUnit(this.DatabaseType), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        count = da.Fill(new DataSet("temp"));
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<RefProductUnits>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_ProductUnit(row));
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
                ErrorMessage = "Exception Occured While Getting Order Item. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private RefProductUnits Load_ProductUnit(DataRow row)
        {
            RefProductUnits productUnits = null;
            try
            {
                if (row != null)
                {

                    productUnits = new RefProductUnits();
                    productUnits.ProductUnit = DBUtil.GetStringValue(row, "product_units");
                    productUnits.ProductUnitDesc = DBUtil.GetStringValue(row, "product_units_desc");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productUnits;
        }

    }
}