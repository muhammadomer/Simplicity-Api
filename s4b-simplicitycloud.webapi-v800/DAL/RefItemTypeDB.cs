using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefItemTypeDB : MainDB
    {

        public RefItemTypeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<RefItemTypes> selectItemType(bool isAllItems, out int count)
        {
            List<RefItemTypes> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefItemTypeQueries.selectItemType(this.DatabaseType, isAllItems), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        count = da.Fill(new DataSet("temp"));
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<RefItemTypes>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_ItemType(row));
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

        private RefItemTypes Load_ItemType(DataRow row)
        {
            RefItemTypes ItemTypes = null;
            try
            {
                if (row != null)
                {

                    ItemTypes = new RefItemTypes();
                    ItemTypes.TypeSequence = DBUtil.GetIntValue(row, "type_sequence");
                    ItemTypes.TypeDesc = DBUtil.GetStringValue(row, "type_desc");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemTypes;
        }

    }
}