using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class EntityDetailsJoinDB : MainDB
    {

        public EntityDetailsJoinDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertEntityDetailsJoin(long entityId, string transType)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(EntityDetailsJoinQueries.insert(entityId, transType), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }


        public List<EntityDetailsJoin> selectAllEntityDetailsJoinentityId(long entityId)
        {
            List<EntityDetailsJoin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EntityDetailsJoinQueries.getSelectAllBySequence(entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsJoin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_EntityDetailsJoin(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Selecting Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool update(long entityId, string transType)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsJoinQueries.update(entityId, transType), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Updating Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool delete(long entityId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsJoinQueries.delete(entityId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while deleting Entity Details Join " + ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(EntityDetailsJoinQueries.deleteFlagDeleted(this.DatabaseType, entityId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while deleting Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private EntityDetailsJoin Load_EntityDetailsJoin(OleDbDataReader dr)

        {
            EntityDetailsJoin entityDetailsJoin = null;
            try
            {
                if (dr != null)
                {
                    entityDetailsJoin = new EntityDetailsJoin();
                    entityDetailsJoin.EntityId = long.Parse(dr["entity_id"].ToString());
                    //entityDetailsJoin.TransType = Itilities.getDBString(dr["trans_type"]);
                    entityDetailsJoin.TransType = SimplicityConstants.ClientTransType;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Loading Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return entityDetailsJoin;
        }
    }
}
