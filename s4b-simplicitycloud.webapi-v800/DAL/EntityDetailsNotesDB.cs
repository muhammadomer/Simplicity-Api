using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class EntityDetailsNotesDB : MainDB
    {

        public EntityDetailsNotesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        
        public List<EntityDetailsNotes> getByEntityId(long entityId)
        {
            List<EntityDetailsNotes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EntityDetailsNotesQueries.getAllByEntityId(this.DatabaseType, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsNotes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_EntityNotes(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool insertEntityNotes(out long sequence, EntityDetailsNotes obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                           new OleDbCommand(EntityDetailsNotesQueries.insert(this.DatabaseType, obj.EntityId ?? 0, obj.EntityNotes,
                                            obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool updateBySequence(EntityDetailsNotes obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsNotesQueries.update(this.DatabaseType, obj.Sequence ?? 0, obj.EntityId ?? 0, obj.EntityNotes,
                                         obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EntityDetailsNotesQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private EntityDetailsNotes Load_EntityNotes(OleDbDataReader dr)
        {
            EntityDetailsNotes notes = null;
            try
            {
                if (dr != null)
                {
                    notes = new EntityDetailsNotes();
                    notes.Sequence =DBUtil.GetLongValue(dr,"sequence");
                    notes.EntityId = DBUtil.GetLongValue(dr,"entity_id");
                    notes.EntityNotes = DBUtil.GetStringValue(dr,"entity_notes");
                    notes.CreatedBy = DBUtil.GetLongValue(dr,"created_by");
                    notes.DateCreated = DBUtil.GetDateTimeValue(dr,"date_created");
                    notes.LastAmendedBy = DBUtil.GetLongValue(dr,"last_amended_by");
                    notes.DateLastAmended = DBUtil.GetDateTimeValue(dr,"date_last_amended");
                    notes.UserName =  DBUtil.GetStringValue(dr, "user_name");
                    notes.UserLogon = DBUtil.GetStringValue(dr, "user_logon");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notes;
        }
    }
}
