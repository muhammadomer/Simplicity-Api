using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{
    public class EdcGdprDB : MainDB
    {

        public EdcGdprDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insert(EdcGdpr obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(EdcGdprQueries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
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

        public bool updateByEntityId(EdcGdpr obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EdcGdprQueries.update(this.DatabaseType, obj), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                        new OleDbCommand(EdcGdprQueries.delete(this.DatabaseType, entityId), conn))
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

        public EdcGdpr selectByEntityId(long entityId)
        {
            EdcGdpr returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EdcGdprQueries.getSelectByEntityId(this.DatabaseType, entityId), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = Load_Gdpr(dt.Rows[0]);
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

        private EdcGdpr Load_Gdpr(DataRow row)
        {
            EdcGdpr obj = null;
            if (row != null)
            {
                obj = new EdcGdpr();
                obj.EntityId = DBUtil.GetLongValue(row, "entity_id");
                obj.UserAccepts = DBUtil.GetIntValue(row, "user_accepts");
                obj.NoReason = DBUtil.GetStringValue(row, "no_resaon");
                obj.DateUserAccepts = DBUtil.GetDateValue(row, "date_user_accepts");
                obj.AcceptsType = DBUtil.GetIntValue(row, "accepts_type");
                obj.ContactByPost = DBUtil.GetIntValue(row,"contact_by_post");
                obj.ContactByEmail = DBUtil.GetIntValue(row, "contact_by_email");
                obj.ContactByPhone = DBUtil.GetIntValue(row, "contact_by_phone");
                obj.ContactBySms = DBUtil.GetIntValue(row, "contact_by_sms");
                obj.CreatedBy = DBUtil.GetIntValue(row,"created_by");
                obj.DateCreated = DBUtil.GetDateValue(row,"date_created");
                obj.LastAmendedBy = DBUtil.GetIntValue(row,"last_amended_by");
                obj.DateLastAmended = DBUtil.GetDateValue(row,"date_last_amended");
            }
            return obj;
        }

        
    }
}
