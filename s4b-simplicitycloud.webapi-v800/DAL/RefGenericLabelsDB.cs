using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefGenericLabelsDB: MainDB
    {
        public RefGenericLabelsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefGenericLabels(long genericFieldId, string geneticFieldName, string customisedFieldName)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string MaxId = "select MAX(generic_field_id) from un_ref_generic_labels";
                    using (OleDbCommand cmdObj = new OleDbCommand(MaxId, conn))
                    {
                        Object result = cmdObj.ExecuteScalar();
                        genericFieldId = Convert.ToInt32(result) + 1;
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefGenericLabelsQueries.insert(this.DatabaseType, genericFieldId, geneticFieldName, customisedFieldName), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                ErrorMessage = "Error occured while inserting into Generic Labels " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<RefGenericLabels> selectAllRef_Generic_Labels()
        {
            List<RefGenericLabels> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefGenericLabelsQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefGenericLabels>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Ref_Generic_Labels(dr));
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

        public RefGenericLabels selectAllRef_Generic_LabelsByGenericFieldId(long genericFieldId)
        {
            RefGenericLabels returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefGenericLabelsQueries.getSelectAllByGenericFieldId(this.DatabaseType, genericFieldId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new RefGenericLabels();
                                while (dr.Read())
                                {
                                    returnValue=Load_Ref_Generic_Labels(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                ErrorMessage = "Error occured while inserting into generic labels " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<RefGenericLabels> selectAllRefGenericLabelsByGeneticFieldName(string geneticFieldName)
        {
            List<RefGenericLabels> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefGenericLabelsQueries.getSelectAllByGeneticFieldName(this.DatabaseType, geneticFieldName), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefGenericLabels>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Ref_Generic_Labels(dr));
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

        public bool updateBygenericFieldId(long genericFieldId, string geneticFieldName, string customisedFieldName)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefGenericLabelsQueries.update(this.DatabaseType, genericFieldId, geneticFieldName, customisedFieldName), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                ErrorMessage = "Error occured while inserting into generic labels " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBygenericFieldId(long genericFieldId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefGenericLabelsQueries.delete(this.DatabaseType, genericFieldId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
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

        private RefGenericLabels Load_Ref_Generic_Labels(OleDbDataReader dr)
        {
            RefGenericLabels refGenericLabels = null;
            try
            { 
                if(dr!=null)
                {
                    refGenericLabels = new RefGenericLabels();
                    refGenericLabels.GenericFieldId = long.Parse(dr["generic_field_id"].ToString());
                    refGenericLabels.GeneticFieldName = Utilities.GetDBString(dr["genetic_field_name"]);
                    refGenericLabels.CustomisedFieldName = Utilities.GetDBString(dr["customised_field_name"]);
                }
            }
            catch(Exception ex)
            {
            }
            return refGenericLabels;
        }       
    }
}
