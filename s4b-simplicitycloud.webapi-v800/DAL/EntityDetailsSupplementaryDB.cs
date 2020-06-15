using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class EntityDetailsSupplementaryDB : MainDB
    {

        public EntityDetailsSupplementaryDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertEntityDetailsSupplementary(long entityId, string dataType, string data)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(EntityDetailsSupplementaryQueries.insert(entityId, dataType, data), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Inserting Supplementary Data for Entity Id '" + entityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<EntityDetailsSupplementary> GetSelectAllByEntityId(long entityId)
        {
            List<EntityDetailsSupplementary> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EntityDetailsSupplementaryQueries.getSelectAllBySequence(entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsSupplementary>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_EntityDetailsSupplementary(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting Supplementary Data for Entity Id '" + entityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool updateByentityId(long entityId, string dataType, string data)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsSupplementaryQueries.update(entityId, dataType, data), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Updating Supplementary Data for Entity Id '" + entityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool deleteByEntityId(long entityId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsSupplementaryQueries.delete(this.DatabaseType, entityId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Deleting Supplementary Data for Entity Id '" + entityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long entityId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsSupplementaryQueries.deleteFlagDeleted(this.DatabaseType, entityId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Update Supplementary Data Deleted Flag for Entity Id '" + entityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private EntityDetailsSupplementary Load_EntityDetailsSupplementary(OleDbDataReader dr)

        {
            EntityDetailsSupplementary entityDetailsSupplementary = null;
            try
            {
                if (dr != null)
                {
                    entityDetailsSupplementary = new EntityDetailsSupplementary();
                    entityDetailsSupplementary.EntityId = long.Parse(dr["entity_id"].ToString());
                    entityDetailsSupplementary.DataType = Utilities.GetDBString(dr["data_type"]);
                    entityDetailsSupplementary.Data = Utilities.GetDBString(dr["data"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Loading Supplementary Data. " + ex.Message + " " + ex.InnerException;
            }
            return entityDetailsSupplementary;
        }
    }
}
