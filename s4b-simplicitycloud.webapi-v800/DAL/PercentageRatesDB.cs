using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class PercentageRatesDB : MainDB
    {

        public PercentageRatesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<PercentageRates> selectPercentageRateOfType(string type,out int count)
        {
            List<PercentageRates> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PercentageRatesQueries.selectPercentageRatesByType(this.DatabaseType, type), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        count = da.Fill(new DataSet("temp"));
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<PercentageRates>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_PercentageRates(row));
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

        private PercentageRates Load_PercentageRates(DataRow row)
        {
            PercentageRates rates = null;
            try
            {
                if (row != null)
                {

                    rates = new PercentageRates();
                    rates.Sequence = DBUtil.GetLongValue(row, "sequence");
                    rates.Flg_Multi_Schedule = DBUtil.GetBooleanValue(row, "flg_multi_schedule");
                    rates.GroupId = DBUtil.GetLongValue(row, "group_id");
                    rates.PcentId = DBUtil.GetStringValue(row, "pcent_id");
                    rates.PcentRate = DBUtil.GetDoubleValue(row, "pcent_rate");
                    rates.PcentType = DBUtil.GetStringValue(row, "pcent_type");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rates;
        }

    }
}