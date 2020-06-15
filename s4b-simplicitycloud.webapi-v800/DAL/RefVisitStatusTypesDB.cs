using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefVisitStatusTypesDB: MainDB
    {
        public RefVisitStatusTypesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public RefVisitStatusTypes insertRefVisitStatusTypes(RefVisitStatusTypes obj)
        {
            RefVisitStatusTypes returnValue = new RefVisitStatusTypes();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string sql = "select MAX(status_id) from un_ref_visit_status_type";
                    using (OleDbCommand cmdObj = new OleDbCommand(sql, conn))
                    {
                       Object result = cmdObj.ExecuteScalar();
                        obj.StatusId = Convert.ToInt32(result) + 1;
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefVisitStatusTypesQueries.insert(this.DatabaseType, obj), conn))
                    {
                            if (objCmdInsert.ExecuteNonQuery() > 0)
                            {
                                returnValue = obj;
                        }
                        else
                        {
                            returnValue = null;
                        }
                       
                     }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public RefVisitStatusTypes selectAllRefVisitStatusTypesByStatusId(int visitStatus)
        {
            RefVisitStatusTypes returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefVisitStatusTypesQueries.getSelectAllByStatusId(this.DatabaseType, visitStatus), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new RefVisitStatusTypes();
                                dr.Read();
                                returnValue = LoadRefVisitStatusTypes(dr);
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

        public List<RefVisitStatusTypes> selectAllRefVisitStatusTypes()
        {
            List<RefVisitStatusTypes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefVisitStatusTypesQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefVisitStatusTypes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefVisitStatusTypes(dr));
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
        public RefVisitStatusTypes selectAllRefVisitStatusTypesByStatusDesc(string visitStatusDesc)
        {
            RefVisitStatusTypes returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefVisitStatusTypesQueries.getSelectAllByVisitStatusDesc(this.DatabaseType, visitStatusDesc), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadRefVisitStatusTypes(dr);
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

        public RefVisitStatusTypes updateRefVisitStatusTypes(RefVisitStatusTypes obj)
        {
            RefVisitStatusTypes returnValue = new RefVisitStatusTypes();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefVisitStatusTypesQueries.update(this.DatabaseType, obj), conn))
                    {
                        if (objCmdUpdate.ExecuteNonQuery() > 0)
                        {
                            returnValue = obj;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        internal RefVisitStatusTypes GetVisitStatusById(int userId)
        {
            RefVisitStatusTypes returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefVisitStatusTypesQueries.getVisitTypeById(this.DatabaseType, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new RefVisitStatusTypes();
                                while (dr.Read())
                                {
                                    returnValue = LoadRefVisitStatusTypes(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private RefVisitStatusTypes LoadRefVisitStatusTypes(OleDbDataReader dr)
        {
            RefVisitStatusTypes RefVisitStatusTypes = null;
            try
            { 
                if(dr!=null)
                {
                    RefVisitStatusTypes = new RefVisitStatusTypes();
                    RefVisitStatusTypes.FlgCreateOrderBill = bool.Parse(dr["flg_create_order_bill"].ToString());
                    RefVisitStatusTypes.FlgNoAccess = bool.Parse(dr["flg_no_access"].ToString());
                    RefVisitStatusTypes.FlgReturnReason = bool.Parse(dr["flg_return_reason"].ToString());
                    RefVisitStatusTypes.FlgVisitComplete = bool.Parse(dr["flg_visit_complete"].ToString());
                    RefVisitStatusTypes.StatusAbbreviation = Utilities.GetDBString(dr["status_abbreviation"]);
                    RefVisitStatusTypes.StatusDesc = Utilities.GetDBString(dr["status_desc"]);
                    RefVisitStatusTypes.StatusId = Int32.Parse(dr["status_id"].ToString());
                }
            }
            catch(Exception ex)
            {
            }
            return RefVisitStatusTypes;
        }       
    }
}
