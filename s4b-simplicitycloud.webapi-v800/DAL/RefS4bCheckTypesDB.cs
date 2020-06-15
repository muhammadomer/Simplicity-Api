using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefS4bCheckTypesDB : MainDB
    {

        public RefS4bCheckTypesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

      
        public List<RefS4bCheckTypes> selectAll()
        {
            List<RefS4bCheckTypes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefS4bCheckTypesQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefS4bCheckTypes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefS4bCheckTypes(dr));
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

		public List<RefS4bCheckTypes> selectAllByCheckType(int checkType)
		{
			List<RefS4bCheckTypes> returnValue = null;
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{
					using (OleDbCommand objCmdSelect =
						new OleDbCommand(RefS4bCheckTypesQueries.getSelectAllByCheckType(this.DatabaseType,checkType), conn))
					{
						using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
						{
							if (dr.HasRows)
							{
								returnValue = new List<RefS4bCheckTypes>();
								while (dr.Read())
								{
									returnValue.Add(Load_RefS4bCheckTypes(dr));
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

		private RefS4bCheckTypes Load_RefS4bCheckTypes(OleDbDataReader dr)

        {
			RefS4bCheckTypes record = null;
            try
            {
                if (dr != null)
                {
                    record = new RefS4bCheckTypes();
                    record.Sequence = long.Parse(dr["sequence"].ToString());
                    record.CheckType = int.Parse(dr["check_type"].ToString());
                    record.CheckId = int.Parse(dr["check_id"].ToString());
                    record.RowIndex = long.Parse(dr["row_index"].ToString());
                    record.CheckDesc = Utilities.GetDBString(dr["check_desc"]);
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