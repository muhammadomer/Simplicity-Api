using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL

{
    public class EntityDetailsCoreDB: MainDB
    {
        public EntityDetailsCoreDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertEntityDetailsCore(out long entityId, bool flgDeleted, bool flgEntityOnHold, bool flgContactManager, long clientType, bool flgInvoicingClient, bool flgEntityJoin,
                                            long entityJoinId, long entityApprovedStatus, long entityPymtType, bool flgEformsPreferred, string nameShort, string nameLong, long sageId,
                                            bool flgSageTurnOn, string nameSage, string nameTitle, string nameInitilas, string nameForename, string nameSurname, string addressNo,
                                            string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                                            string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                                            bool flgSupAddressHeld, bool flgClientCheck, long userListId, long userListId2, long userListId3, object userNumericField1, bool flgUserField1,
                                            bool flgUserField2, bool flgUserField3, bool flgUserField4, string userTextField1, string userTextField2, string userTextField3, string userTextField4,
                                            bool flgUserDateField1, DateTime? dateUserDateField1, bool flgUserDateField2, DateTime? dateUserDateField2, bool flgUserDateField3, DateTime? dateUserDateField3,
                                            bool flgUserDateField4, DateTime? dateUserDateField4, bool flgEntityApproved, long createdBy, DateTime? dateCreated, long LastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            entityId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string sql = "select max(entity_id) from un_entity_details_core";
                    using (OleDbCommand objCommand =
                        new OleDbCommand(sql, conn))
                    {
                        OleDbDataReader dr = objCommand.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            entityId = long.Parse(dr[0].ToString()) + 1;
                        }
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(Entity_Details_CoreQueries.insert(this.DatabaseType,entityId, flgDeleted,  flgEntityOnHold,  flgContactManager,  clientType,  flgInvoicingClient,  flgEntityJoin,
                                                                        entityJoinId,  entityApprovedStatus,  entityPymtType,  flgEformsPreferred,  nameShort,  nameLong,  sageId,
                                                                        flgSageTurnOn,  nameSage,  nameTitle,  nameInitilas,  nameForename,  nameSurname,  addressNo,
                                                                        addressLine1,  addressLine2,  addressLine3,  addressLine4,  addressLine5,  addressPostCode,  addressFull,
                                                                        telephone,  telExt,  telFax,  telMobile,  telWork,  email,  PropertyEpn,  PropertyUpn,  entityDetails,
                                                                        flgSupAddressHeld,  flgClientCheck,  userListId,  userListId2,  userListId3,  userNumericField1,  flgUserField1,
                                                                        flgUserField2,  flgUserField3,  flgUserField4,  userTextField1,  userTextField2,  userTextField3,  userTextField4,
                                                                        flgUserDateField1, dateUserDateField1,  flgUserDateField2, dateUserDateField2,  flgUserDateField3, dateUserDateField3,
                                                                        flgUserDateField4, dateUserDateField4,flgEntityApproved,  createdBy, dateCreated,  LastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting into Entity Details Core. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool insertAddress(out long entityId, bool flgDeleted,  bool flgEntityJoin,
                                            long entityJoinId,  string addressNo,
                                            string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                                            string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                                            long createdBy, DateTime? dateCreated, long LastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            entityId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string sql = "select max(entity_id) from un_entity_details_core";
                    using (OleDbCommand objCommand =
                        new OleDbCommand(sql, conn))
                    {
                        OleDbDataReader dr = objCommand.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            entityId = long.Parse(dr[0].ToString()) + 1;
                        }
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(Entity_Details_CoreQueries.insert(this.DatabaseType, entityId, false, false, false, 0, false, flgEntityJoin,
                                                                        entityJoinId, 0, 0, false, "N/A", "N/A", 0,
                                                                        false, "N/A", "", "", "", "", addressNo,
                                                                        addressLine1, addressLine2, addressLine3, addressLine4, addressLine5, addressPostCode, addressFull,
                                                                        telephone, telExt, telFax, telMobile, telWork, email, PropertyEpn, PropertyUpn, entityDetails,
                                                                        false, false, 0, 0, 0, 0, false,
                                                                        false, false, false, null, null, null, null,
                                                                        false, null, false, null, false, null,
                                                                        false, null,false, createdBy, dateCreated, LastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting into Entity Details Core. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        public  List<EntityDetailsCore> selectAllEntity_Details_CoreByEntityId(long entityId)
        {
            List<EntityDetailsCore> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllByEntityId(this.DatabaseType, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCore>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Entity_Details_Core(dr));
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

     public EntityDetailsCore getEntityByEntityid(long? entityId)
        {
            EntityDetailsCore returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllByEntityId(this.DatabaseType, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Entity_Details_Core(dr);
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
     public EntityDetailsCore getEntityByShortName(string shortName)
        {
            EntityDetailsCore returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllByShortName(this.DatabaseType, shortName), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Entity_Details_Core(dr);
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
        public EntityDetailsCore GetEntityByLongName(string longName)
        {
            EntityDetailsCore returnValue = null;
            string qry = @"SELECT * FROM un_entity_details_core edc " +
                        " left outer join un_cld_edc cld on edc.entity_id = cld.entity_id " +
                        " WHERE Lower(name_long) = '" + longName.ToLower() +
                        "' or Lower(cld.rossum_contact_name)='" + longName.ToLower() + "'";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Entity_Details_Core(dr);
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
        public string generateShortName(string shortName)
        {
            string returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getLastShortNameValue(this.DatabaseType, shortName), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            
                            if (dr.HasRows)
                            {
                                dr.Read();
                                string value = dr["short_name"].ToString();
                                returnValue = shortName + (Convert.ToInt32(value) + 1).ToString();
                                if (returnValue.Length > 32)
                                    returnValue = returnValue.Substring(0, 31);
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
        internal bool updateClientInfo(EntityDetailsCore edc, string infoType)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(Entity_Details_CoreQueries.UpdateClientInfo(this.DatabaseType, edc, infoType), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }

            return returnValue;
        }
     public bool update(long entityId, bool flgDeleted, bool flgEntityOnHold, bool flgContactManager, long clientType, bool flgInvoicingClient, bool flgEntityJoin,
                           long entityJoinId, long entityApprovedStatus, long entityPymtType, bool flgEformsPreferred,  string nameLong, long sageId,
                           bool flgSageTurnOn, string nameSage, string nameTitle, string nameInitilas, string nameForename, string nameSurname, string addressNo,
                           string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                           string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                           bool flgSupAddressHeld, bool flgClientCheck, long userListId, long userListId2, long userListId3, object userNumericField1, bool flgUserField1,
                           bool flgUserField2, bool flgUserField3, bool flgUserField4, string userTextField1, string userTextField2, string userTextField3, string userTextField4,
                           bool flgUserDateField1, DateTime? dateUserDateField1, bool flgUserDateField2, DateTime? dateUserDateField2, bool flgUserDateField3, DateTime? dateUserDateField3,
                           bool flgUserDateField4, DateTime? dateUserDateField4, bool flgEntityApproved, long LastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Entity_Details_CoreQueries.update(this.DatabaseType, entityId, flgDeleted, flgEntityOnHold, flgContactManager, clientType, flgInvoicingClient, flgEntityJoin,
                                                                           entityJoinId, entityApprovedStatus, entityPymtType, flgEformsPreferred,  nameLong, sageId,
                                                                           flgSageTurnOn, nameSage, nameTitle, nameInitilas, nameForename, nameSurname, addressNo,
                                                                           addressLine1, addressLine2, addressLine3, addressLine4, addressLine5, addressPostCode, addressFull,
                                                                           telephone, telExt, telFax, telMobile, telWork, email, PropertyEpn, PropertyUpn, entityDetails,
                                                                           flgSupAddressHeld, flgClientCheck, userListId, userListId2, userListId3, userNumericField1, flgUserField1,
                                                                           flgUserField2, flgUserField3, flgUserField4, userTextField1, userTextField2, userTextField3, userTextField4,
                                                                           flgUserDateField1, dateUserDateField1, flgUserDateField2, dateUserDateField2, flgUserDateField3, dateUserDateField3,
                                                                           flgUserDateField4, dateUserDateField4,flgEntityApproved, LastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Updating Entity. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool updateEntityJoinId(long entityId,long entityJoinId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Entity_Details_CoreQueries.updateEntityJoinId(this.DatabaseType, entityId,entityJoinId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Updating Entity. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool updateAddress(long entityId, bool flgDeleted,bool flgEntityJoin,long entityJoinId,  string addressNo,
                          string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                          string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                         long LastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Entity_Details_CoreQueries.updateAddress(this.DatabaseType, entityId, flgDeleted, flgEntityJoin,
                                                                           entityJoinId,  addressNo,
                                                                           addressLine1, addressLine2, addressLine3, addressLine4, addressLine5, addressPostCode, addressFull,
                                                                           telephone, telExt, telFax, telMobile, telWork, email, PropertyEpn, PropertyUpn, entityDetails,
                                                                           LastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Updating Entity. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal bool updateEDCInfo(EntityDetailsCore edc, string infoType)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(Entity_Details_CoreQueries.UpdateEntityDetailsCoreInfo(this.DatabaseType, edc, infoType), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }
            return returnValue;
        }

        public bool deleteByentityId(long entityId)
        {
            bool returnValue = false;
            string qry = "DELETE FROM un_cld_edc WHERE entity_id = " + entityId;

            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate = new OleDbCommand(qry, conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        objCmdUpdate.CommandText = "DELETE FROM un_entity_details_core WHERE entity_id = " + entityId;
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

        public   bool deleteByFlgDeleted(long entityId)
        {
            bool returnValue = false;
            int rowsEffected = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Entity_Details_CoreQueries.deleteFlagDeleted(this.DatabaseType, entityId), conn))
                    {
                        rowsEffected = objCmdUpdate.ExecuteNonQuery();
                    }
                }
                if (rowsEffected > 0) {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private EntityDetailsCoreMin LoadEDCMin(OleDbDataReader dr)
        {
            EntityDetailsCoreMin edcMin = null;
            try
            {
                if (dr != null)
                {
                    edcMin = new EntityDetailsCoreMin();
                    edcMin.EntityId = long.Parse(dr["entity_id"].ToString());
                    edcMin.EntityJoinId = this.ColumnExists(dr, "entity_join_id") ? long.Parse(dr["entity_join_id"].ToString()) : -1;
                    edcMin.NameShort = this.ColumnExists(dr, "name_short") ? dr["name_short"].ToString() : null;
                    edcMin.NameLong = this.ColumnExists(dr, "name_long") ? dr["name_long"].ToString() : null;
                    if(edcMin.EntityJoinId<=0 || edcMin.EntityId==edcMin.EntityJoinId)
                    {
                        edcMin.ParentName = SimplicityConstants.NotAvailable;
                    }
                    else
                    {
                        edcMin.ParentName = this.ColumnExists(dr, "parent_name_long") ? dr["parent_name_long"].ToString() : null;
                    }                    
                    edcMin.AddressFull = this.ColumnExists(dr, "address_full") ? dr["address_full"].ToString() : null;
                    edcMin.AddressPostCode = this.ColumnExists(dr, "address_post_code") ? dr["address_post_code"].ToString() : null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return edcMin;
        }

        private EntityDetailsCoreMin LoadEDCMin(DataRow row)
        {
            EntityDetailsCoreMin edcMin = null;
            try
            {
                if (row != null)
                {
                    edcMin = new EntityDetailsCoreMin();
                    edcMin.EntityId = DBUtil.GetLongValue(row,"entity_id");
                    edcMin.EntityJoinId = DBUtil.GetLongValue(row, "entity_join_id");
                    edcMin.NameShort = DBUtil.GetStringValue(row, "name_short");
                    edcMin.NameLong = DBUtil.GetStringValue(row, "name_long");
                    edcMin.NameForename = DBUtil.GetStringValue(row, "name_forename");
                    edcMin.NameSurname = DBUtil.GetStringValue(row, "name_surname");
                    if (edcMin.EntityJoinId <= 0 || edcMin.EntityId == edcMin.EntityJoinId)
                    {
                        edcMin.ParentName = SimplicityConstants.NotAvailable;
                    }
                    else
                    {
                        edcMin.ParentName = DBUtil.GetStringValue(row, "parent_name_short");
                    }
                    edcMin.AddressFull = DBUtil.GetStringValue(row, "address_full");
                    edcMin.AddressPostCode = DBUtil.GetStringValue(row, "address_post_code");
                    edcMin.Telephone = DBUtil.GetStringValue(row, "telephone");
                    edcMin.TelExt = DBUtil.GetStringValue(row, "tel_ext");
                    edcMin.TelMobile = DBUtil.GetStringValue(row, "tel_mobile");
                    edcMin.TelWork = DBUtil.GetStringValue(row, "tel_work");
                    edcMin.TelFax = DBUtil.GetStringValue(row, "tel_fax");
                    edcMin.Email = DBUtil.GetStringValue(row, "email");
                    edcMin.TransTypeDesc = DBUtil.GetStringValue(row, "trans_type_text");
                    edcMin.FlgEntityApproved = DBUtil.GetBooleanValue(row, "flg_entity_approved") ;
                    edcMin.FlgInvoicingClient = DBUtil.GetBooleanValue(row, "flg_invoicing_client");
					edcMin.FlgEntityOnHold = DBUtil.GetBooleanValue(row, "flg_entity_on_hold");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return edcMin;
        }
        private EntityDetailsCore Load_Entity_Details_Core(OleDbDataReader dr)
        {
            const string METHOD_NAME = "EntityDetailsCoreDB.Load_Entity_Details_Core()";
            EntityDetailsCore entityDetailsCore = null;
            try
            {
                if (dr != null)
                {
                    entityDetailsCore = new EntityDetailsCore();
                    entityDetailsCore.EntityId = long.Parse(dr["entity_id"].ToString());
                    entityDetailsCore.Email = this.ColumnExists(dr, "email") ? (dr["email"] == null || dr["email"] == DBNull.Value) ? "" : dr["email"].ToString() : string.Empty;
                    entityDetailsCore.NameShort = this.ColumnExists(dr, "name_short") ? dr["name_short"].ToString() : string.Empty;
                    entityDetailsCore.ParentNameShort = this.ColumnExists(dr, "parent_name") ? dr["parent_name"].ToString() : string.Empty;
                    entityDetailsCore.NameLong = this.ColumnExists(dr, "name_long") ? dr["name_long"].ToString() : string.Empty;
                    entityDetailsCore.AddressFull = this.ColumnExists(dr, "address_full") ? dr["address_full"].ToString() : string.Empty;
                    entityDetailsCore.FlgDeleted = this.ColumnExists(dr, "flg_deleted") ? bool.Parse(dr["flg_deleted"].ToString()) : false;
                    entityDetailsCore.FlgEntityOnHold = this.ColumnExists(dr, "flg_entity_on_hold") ? bool.Parse(dr["flg_entity_on_hold"].ToString()) : false;
                    entityDetailsCore.FlgEntityApproved = this.ColumnExists(dr, "flg_entity_approved") ? bool.Parse(dr["flg_entity_approved"].ToString()) : false;
                    entityDetailsCore.FlgContactManager = this.ColumnExists(dr, "flg_contact_manager") ? bool.Parse(dr["flg_contact_manager"].ToString()) : false;
                    entityDetailsCore.ClientType = this.ColumnExists(dr, "client_type") ? long.Parse(dr["client_type"].ToString()) : 0;
                    entityDetailsCore.FlgInvoicingClient = this.ColumnExists(dr, "flg_invoicing_client") ? bool.Parse(dr["flg_invoicing_client"].ToString()) : false;
                    entityDetailsCore.FlgEntityJoin = this.ColumnExists(dr, "flg_entity_join") ? bool.Parse(dr["flg_entity_join"].ToString()) : false;
                    entityDetailsCore.EntityJoinId = this.ColumnExists(dr, "entity_join_id") ? long.Parse(dr["entity_join_id"].ToString()) : 0;
                    entityDetailsCore.EntityApprovedStatus = this.ColumnExists(dr, "entity_approved_status") ? long.Parse(dr["entity_approved_status"].ToString()) : 0;
                    entityDetailsCore.EntityPymtType = this.ColumnExists(dr, "entity_pymt_type") ? long.Parse(dr["entity_pymt_type"].ToString()) : 0;
                    entityDetailsCore.EntityPymtTypeDesc = this.ColumnExists(dr, "entity_pymt_desc") ? (dr["entity_pymt_desc"].ToString()) : "";
                    entityDetailsCore.FlgEformsPreferred = this.ColumnExists(dr, "flg_eforms_preferred") ? bool.Parse(dr["flg_eforms_preferred"].ToString()) : false;
                    entityDetailsCore.SageId = this.ColumnExists(dr, "sage_id") ? Utilities.GetDBLong(dr["sage_id"]) : 0;
                    entityDetailsCore.FlgSageTurnOn = this.ColumnExists(dr, "flg_sage_turn_on") ? bool.Parse(dr["flg_sage_turn_on"].ToString()) : false;
                    entityDetailsCore.NameSage = this.ColumnExists(dr, "name_sage") ? dr["name_sage"].ToString() : string.Empty;
                    entityDetailsCore.NameTitle = this.ColumnExists(dr, "name_title") ? dr["name_title"].ToString() : string.Empty;
                    entityDetailsCore.NameInitilas = this.ColumnExists(dr, "name_initilas") ? dr["name_initilas"].ToString() : string.Empty;
                    entityDetailsCore.NameForename = this.ColumnExists(dr, "name_forename") ? dr["name_forename"].ToString() : string.Empty;
                    entityDetailsCore.NameSurname = this.ColumnExists(dr, "name_surname") ? dr["name_surname"].ToString() : string.Empty;
                    entityDetailsCore.AddressNo = this.ColumnExists(dr, "address_no") ? dr["address_no"].ToString() : string.Empty;
                    entityDetailsCore.AddressLine1 = this.ColumnExists(dr, "address_line1") ? dr["address_line1"].ToString() : string.Empty;
                    entityDetailsCore.AddressLine2 = this.ColumnExists(dr, "address_line2") ? dr["address_line2"].ToString() : string.Empty;
                    entityDetailsCore.AddressLine3 = this.ColumnExists(dr, "address_line3") ? dr["address_line3"].ToString() : string.Empty;
                    entityDetailsCore.AddressLine4 = this.ColumnExists(dr, "address_line4") ? dr["address_line4"].ToString() : string.Empty;
                    entityDetailsCore.AddressLine5 = this.ColumnExists(dr, "address_line5") ? dr["address_line5"].ToString() : string.Empty;
                    entityDetailsCore.AddressPostCode = this.ColumnExists(dr, "address_post_code") ? dr["address_post_code"].ToString() : string.Empty;
                    entityDetailsCore.Telephone = this.ColumnExists(dr, "telephone") ? dr["telephone"].ToString() : string.Empty;
                    entityDetailsCore.TelExt = this.ColumnExists(dr, "tel_ext") ? dr["tel_ext"].ToString() : string.Empty;
                    entityDetailsCore.TelFax = this.ColumnExists(dr, "tel_fax") ? dr["tel_fax"].ToString() : string.Empty;
                    entityDetailsCore.TelMobile = this.ColumnExists(dr, "tel_mobile") ? dr["tel_mobile"].ToString() : string.Empty;
                    entityDetailsCore.TelWork = this.ColumnExists(dr, "tel_work") ? dr["tel_work"].ToString() : string.Empty;
                    entityDetailsCore.Email = this.ColumnExists(dr, "email") ? dr["email"].ToString() : string.Empty;
                    entityDetailsCore.PropertyEpn = this.ColumnExists(dr, "property_epn") ? dr["property_epn"].ToString() : string.Empty;
                    entityDetailsCore.PropertyUpn = this.ColumnExists(dr, "property_upn") ? dr["property_upn"].ToString() : string.Empty;                   
                    entityDetailsCore.EntityDetails = this.ColumnExists(dr, "entity_details") ? dr["entity_details"].ToString() : string.Empty;
                    entityDetailsCore.FlgSupAddressHeld = this.ColumnExists(dr, "flg_sup_address_held") ? bool.Parse(dr["flg_sup_address_held"].ToString()) : false;
                    entityDetailsCore.FlgClientCheck = this.ColumnExists(dr, "flg_client_check") ? bool.Parse(dr["flg_client_check"].ToString()) : false;
                    entityDetailsCore.EdcGdpr = new EdcGdpr();
                    entityDetailsCore.EdcGdpr.EntityId = this.ColumnExists(dr, "gdpr_entityId") ? Int16.Parse(dr["gdpr_entityId"].ToString() == "" ? "-1" : dr["gdpr_entityId"].ToString()) : -1;
                    entityDetailsCore.EdcGdpr.UserAccepts = this.ColumnExists(dr, "user_accepts") ? Int16.Parse(dr["user_accepts"].ToString()== "" ? "0" : dr["user_accepts"].ToString()) : 0;
                    entityDetailsCore.EdcGdpr.DateUserAccepts = this.ColumnExists(dr, "date_user_accepts") ? DateTime.Parse(dr["date_user_accepts"].ToString()=="" ? DateTime.Now.ToString() : dr["date_user_accepts"].ToString()) : DateTime.Today;
                    entityDetailsCore.EdcGdpr.AcceptsType = this.ColumnExists(dr, "accepts_type") ? Int16.Parse(dr["accepts_type"].ToString() == "" ? "2" : dr["accepts_type"].ToString()) : 2;
                    entityDetailsCore.EdcGdpr.ContactByEmail = this.ColumnExists(dr, "contact_by_email") ? Int16.Parse(dr["contact_by_email"].ToString() == "" ? "1" : dr["contact_by_email"].ToString()) : 1;
                    entityDetailsCore.EdcGdpr.ContactByPhone = this.ColumnExists(dr, "contact_by_phone") ? Int16.Parse(dr["contact_by_phone"].ToString() == "" ? "1" : dr["contact_by_phone"].ToString()) : 1;
                    entityDetailsCore.EdcGdpr.ContactByPost = this.ColumnExists(dr, "contact_by_post") ? Int16.Parse(dr["contact_by_post"].ToString() == "" ? "1" : dr["contact_by_post"].ToString()) : 1;
                    entityDetailsCore.EdcGdpr.ContactBySms = this.ColumnExists(dr, "contact_by_sms") ? Int16.Parse(dr["contact_by_sms"].ToString() == "" ? "1" : dr["contact_by_sms"].ToString()) : 1;
                    
                    //entityDetailsCore.UserListId = this.ColumnExists(dr, "user_list_id") ? long.Parse(dr["user_list_id"].ToString()) : 0;
                    //entityDetailsCore.UserListId2 = this.ColumnExists(dr, "user_list_id2") ? long.Parse(dr["user_list_id2"].ToString()) : 0;
                    //entityDetailsCore.UserListId3 = this.ColumnExists(dr, "user_list_id3") ? long.Parse(dr["user_list_id3"].ToString()) : 0;                    
                    //entityDetailsCore.FlgUserField1 = this.ColumnExists(dr, "flg_user_field1") ? bool.Parse(dr["flg_user_field1"].ToString()) : false;
                    //entityDetailsCore.FlgUserField2 = this.ColumnExists(dr, "flg_user_field2") ? bool.Parse(dr["flg_user_field2"].ToString()) : false;
                    //entityDetailsCore.FlgUserField3 = this.ColumnExists(dr, "flg_user_field3") ? bool.Parse(dr["flg_user_field3"].ToString()) : false;
                    //entityDetailsCore.FlgUserField4 = this.ColumnExists(dr, "flg_user_field4") ? bool.Parse(dr["flg_user_field4"].ToString()) : false;
                    //entityDetailsCore.UserTextField1 = this.ColumnExists(dr, "user_text_field1") ? dr["user_text_field1"].ToString() : string.Empty;
                    //entityDetailsCore.UserTextField2 = this.ColumnExists(dr, "user_text_field2") ? dr["user_text_field2"].ToString() : string.Empty;
                    //entityDetailsCore.UserTextField3 = this.ColumnExists(dr, "user_text_field3") ? dr["user_text_field3"].ToString() : string.Empty;
                    //entityDetailsCore.UserTextField4 = this.ColumnExists(dr, "user_text_field4") ? dr["user_text_field4"].ToString() : string.Empty;
                    //entityDetailsCore.FlgUserDateField1 = this.ColumnExists(dr, "flg_user_date_field1") ? bool.Parse(dr["flg_user_date_field1"].ToString()) : false;
                    //entityDetailsCore.DateUserDateField1 = this.ColumnExists(dr, "date_user_date_field1") ? dr["date_user_date_field1"].ToString() : string.Empty;
                    //entityDetailsCore.FlgUserDateField2 = this.ColumnExists(dr, "flg_user_date_field2") ? bool.Parse(dr["flg_user_date_field2"].ToString()) : false;
                    //entityDetailsCore.DateUserDateField2 = this.ColumnExists(dr, "date_user_date_field2") ? dr["date_user_date_field2"].ToString() : string.Empty;
                    //entityDetailsCore.FlgUserDateField3 = this.ColumnExists(dr, "flg_user_date_field3") ? bool.Parse(dr["flg_user_date_field3"].ToString()) : false;
                    //entityDetailsCore.DateUserDateField3 = this.ColumnExists(dr, "date_user_date_field3") ? dr["date_user_date_field3"].ToString() : string.Empty;
                    //entityDetailsCore.FlgUserDateField4 = this.ColumnExists(dr, "flg_user_date_field4") ? bool.Parse(dr["flg_user_date_field4"].ToString()) : false;
                    //entityDetailsCore.DateUserDateField4 = this.ColumnExists(dr, "date_user_date_field4") ? dr["date_user_date_field4"].ToString() : string.Empty;
                    entityDetailsCore.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    entityDetailsCore.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    entityDetailsCore.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    entityDetailsCore.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch(Exception ex)
            {
                Utilities.GenerateAndLogMessage(METHOD_NAME, "Excpetion Occured While Loading Entity Details Core. ", ex);

            }
            return entityDetailsCore;
        }

        private EntityDetailsCore Load_Entity_Details_Core(DataRow row)
        {
            const string METHOD_NAME = "EntityDetailsCoreDB.Load_Entity_Details_Core()";
            EntityDetailsCore entityDetailsCore = null;
            try
            {
                if (row != null)
                {
                    entityDetailsCore = new EntityDetailsCore();
                    entityDetailsCore.EntityId = DBUtil.GetLongValue(row, "entity_id"); 
                    entityDetailsCore.Email = DBUtil.GetStringValue(row, "email") ;
                    entityDetailsCore.NameShort = DBUtil.GetStringValue(row, "name_short");
                    //entityDetailsCore.ParentEntityId = DBUtil.GetStringValue(row, "parent_entity_id") ? long.Parse(row["parent_entity_id"].ToString()) : 0;
                    //entityDetailsCore.ParentNameShort = DBUtil.GetStringValue(row, "parent_name_short") ? row["parent_name_short"].ToString() : string.Empty;
                    entityDetailsCore.NameLong = DBUtil.GetStringValue(row, "name_long");
                    entityDetailsCore.AddressFull = DBUtil.GetStringValue(row, "address_full")  ;
                    entityDetailsCore.FlgDeleted = DBUtil.GetBooleanValue(row, "flg_deleted") ;
                    entityDetailsCore.FlgEntityOnHold = DBUtil.GetBooleanValue(row, "flg_entity_on_hold") ;
                    entityDetailsCore.FlgContactManager = DBUtil.GetBooleanValue(row, "flg_contact_manager") ;
                    entityDetailsCore.ClientType = DBUtil.GetLongValue(row, "client_type") ;
                    entityDetailsCore.FlgInvoicingClient = DBUtil.GetBooleanValue(row, "flg_invoicing_client") ;
                    entityDetailsCore.FlgEntityJoin = DBUtil.GetBooleanValue(row, "flg_entity_join") ;
                    entityDetailsCore.EntityJoinId = DBUtil.GetLongValue(row, "entity_join_id") ;
                    entityDetailsCore.EntityApprovedStatus = DBUtil.GetLongValue(row, "entity_approved_status") ;
                    entityDetailsCore.EntityPymtType = DBUtil.GetLongValue(row, "entity_pymt_type") ;
                    entityDetailsCore.FlgEformsPreferred = DBUtil.GetBooleanValue(row, "flg_eforms_preferred") ;
                    entityDetailsCore.SageId = DBUtil.GetLongValue(row, "sage_id") ;
                    entityDetailsCore.FlgSageTurnOn = DBUtil.GetBooleanValue(row, "flg_sage_turn_on") ;
                    entityDetailsCore.NameSage = DBUtil.GetStringValue(row, "name_sage") ;
                    entityDetailsCore.NameTitle = DBUtil.GetStringValue(row, "name_title") ;
                    entityDetailsCore.NameInitilas = DBUtil.GetStringValue(row, "name_initilas") ;
                    entityDetailsCore.NameForename = DBUtil.GetStringValue(row, "name_forename") ;
                    entityDetailsCore.NameSurname = DBUtil.GetStringValue(row, "name_surname") ;
                    entityDetailsCore.AddressNo = DBUtil.GetStringValue(row, "address_no") ;
                    entityDetailsCore.AddressLine1 = DBUtil.GetStringValue(row, "address_line1") ;
                    entityDetailsCore.AddressLine2 = DBUtil.GetStringValue(row, "address_line2") ;
                    entityDetailsCore.AddressLine3 = DBUtil.GetStringValue(row, "address_line3") ;
                    entityDetailsCore.AddressLine4 = DBUtil.GetStringValue(row, "address_line4") ;
                    entityDetailsCore.AddressLine5 = DBUtil.GetStringValue(row, "address_line5") ;
                    entityDetailsCore.AddressPostCode = DBUtil.GetStringValue(row, "address_post_code") ;
                    entityDetailsCore.Telephone = DBUtil.GetStringValue(row, "telephone") ;
                    entityDetailsCore.TelExt = DBUtil.GetStringValue(row, "tel_ext") ;
                    entityDetailsCore.TelFax = DBUtil.GetStringValue(row, "tel_fax") ;
                    entityDetailsCore.TelMobile = DBUtil.GetStringValue(row, "tel_mobile") ;
                    entityDetailsCore.TelWork = DBUtil.GetStringValue(row, "tel_work") ;
                    entityDetailsCore.Email = DBUtil.GetStringValue(row, "email") ;
                    entityDetailsCore.PropertyEpn = DBUtil.GetStringValue(row, "property_epn") ;
                    entityDetailsCore.PropertyUpn = DBUtil.GetStringValue(row, "property_upn") ;
                    entityDetailsCore.PropertyType = DBUtil.GetStringValue(row, "PropertyType");
                    entityDetailsCore.PropertyStatus = DBUtil.GetStringValue(row, "PropertyStatus");
                    entityDetailsCore.EntityDetails = DBUtil.GetStringValue(row, "entity_details") ;
                    entityDetailsCore.FlgSupAddressHeld = DBUtil.GetBooleanValue(row, "flg_sup_address_held") ;
                    entityDetailsCore.FlgClientCheck = DBUtil.GetBooleanValue(row, "flg_client_check") ;
                    entityDetailsCore.Client = DBUtil.GetStringValue(row, "client");
                    entityDetailsCore.CreatedBy = DBUtil.GetIntValue(row, "created_by");
                    entityDetailsCore.DateCreated = DBUtil.GetDateTimeValue(row, "date_created");
                    entityDetailsCore.LastAmendedBy = DBUtil.GetIntValue(row, "last_amended_by");
                    entityDetailsCore.DateLastAmended = DBUtil.GetDateTimeValue(row, "date_last_amended");
                    
                }
            }
            catch (Exception ex)
            {
                Utilities.GenerateAndLogMessage(METHOD_NAME, "Excpetion Occured While Loading Entity Details Core. ", ex);

            }
            return entityDetailsCore;
        }

        private EntityDetailsCoreAddress Load_Entity_Details_Core_address(DataRow row)
        {
            const string METHOD_NAME = "EntityDetailsCoreDB.Load_Entity_Details_Core()";
            EntityDetailsCoreAddress entityDetailsCore = null;
            try
            {
                if (row != null)
                {
                    entityDetailsCore = new EntityDetailsCoreAddress();
                    entityDetailsCore.EntityId = DBUtil.GetLongValue(row, "entity_id");
                    entityDetailsCore.Email = DBUtil.GetStringValue(row, "email");
                    entityDetailsCore.NameShort = DBUtil.GetStringValue(row, "name_short");
                    entityDetailsCore.NameLong = DBUtil.GetStringValue(row, "name_long");
                    entityDetailsCore.AddressFull = DBUtil.GetStringValue(row, "address_full");
                    entityDetailsCore.FlgDeleted = DBUtil.GetBooleanValue(row, "flg_deleted");
                    entityDetailsCore.ClientType = DBUtil.GetLongValue(row, "client_type");
                    entityDetailsCore.EntityJoinId = DBUtil.GetLongValue(row, "entity_join_id");
                    entityDetailsCore.AddressNo = DBUtil.GetStringValue(row, "address_no");
                    entityDetailsCore.AddressLine1 = DBUtil.GetStringValue(row, "address_line1");
                    entityDetailsCore.AddressLine2 = DBUtil.GetStringValue(row, "address_line2");
                    entityDetailsCore.AddressLine3 = DBUtil.GetStringValue(row, "address_line3");
                    entityDetailsCore.AddressLine4 = DBUtil.GetStringValue(row, "address_line4");
                    entityDetailsCore.AddressLine5 = DBUtil.GetStringValue(row, "address_line5");
                    entityDetailsCore.AddressPostCode = DBUtil.GetStringValue(row, "address_post_code");
                    entityDetailsCore.Telephone = DBUtil.GetStringValue(row, "telephone");
                    entityDetailsCore.TelExt = DBUtil.GetStringValue(row, "tel_ext");
                    entityDetailsCore.TelFax = DBUtil.GetStringValue(row, "tel_fax");
                    entityDetailsCore.TelMobile = DBUtil.GetStringValue(row, "tel_mobile");
                    entityDetailsCore.TelWork = DBUtil.GetStringValue(row, "tel_work");
                    entityDetailsCore.Email = DBUtil.GetStringValue(row, "email");
                    entityDetailsCore.PropertyEpn = DBUtil.GetStringValue(row, "property_epn");
                    entityDetailsCore.PropertyUpn = DBUtil.GetStringValue(row, "property_upn");
                    entityDetailsCore.PropertyType = DBUtil.GetStringValue(row, "PropertyType");
                    entityDetailsCore.PropertyStatus = DBUtil.GetStringValue(row, "PropertyStatus");
                    entityDetailsCore.NameTitle = DBUtil.GetStringValue(row, "ContactName");
                    entityDetailsCore.EntityDetails = DBUtil.GetStringValue(row, "entity_details");
                    entityDetailsCore.Client = DBUtil.GetStringValue(row, "client");
                }
            }
            catch (Exception ex)
            {
                Utilities.GenerateAndLogMessage(METHOD_NAME, "Excpetion Occured While Loading Entity Details Core. ", ex);

            }
            return entityDetailsCore;
        }

        public List<EntityDetailsCoreMin> getSelectAllByTransType(string TransType, string qSearch)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllByTransType(this.DatabaseType, TransType, qSearch), conn))
                    {
                        {
                            OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                            DataTable dt = new DataTable();
                            da.Fill( dt);
                            if (dt.Rows != null && dt.Rows.Count > 0)
                            {
                                returnValue = new List<EntityDetailsCoreMin>();
                                foreach (DataRow row in dt.Rows)
                                {
                                    returnValue.Add(LoadEDCMin(row));
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
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

        internal List<EntityDetailsCoreMin> getSelectAllByTransType(ClientRequest clientRequest,string transType, out int count, bool isCountRequired)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllByTransType(this.DatabaseType, clientRequest,transType), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(clientRequest.first, clientRequest.rows, dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<EntityDetailsCoreMin>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadEDCMin(row));
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
                ErrorMessage = "Exception Occured While Getting Client List. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal List<EntityDetailsCoreMin> getSelectAllSuppliers(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllSuppliers(this.DatabaseType, clientRequest), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(clientRequest.first, clientRequest.rows, dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<EntityDetailsCoreMin>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadEDCMin(row));
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
                ErrorMessage = "Exception Occured While Getting Supplier List. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        public List<EntityDetailsCore> GetAllEmailAddresses(long? jobSequence)
        {
            List<EntityDetailsCore> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getAllEmailAddresses(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCore>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Entity_Details_Core(dr));
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


        public List<EntityDetailsCoreMin> getFullAddresses(string address)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getfullAddress(this.DatabaseType,address), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCoreMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadEDCMin(dr));
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

        public List<EntityDetailsCoreMin> GetEntitiesByName(string transType, string name)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.searchEntitiesNamesByTransType(this.DatabaseType, transType, name), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCoreMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadEDCMin(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting Entities By Name. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<EntityDetailsCoreMin> GetEntitiesByTransTypeAndAddress(string transType, string address)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.GetEntitiesByTransTypeAndAddress(this.DatabaseType, transType, address), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCoreMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadEDCMin(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Record Found.";
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

        public List<EntityDetailsCoreAddress> GetAddressByTransType(string transType, ClientRequest requestModel,  out int count, bool isCountRequired)
        {
            List<EntityDetailsCoreAddress> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                        OleDbDataAdapter da = new OleDbDataAdapter();
                        if (isCountRequired)
                        {
                            using (OleDbCommand objCmdSelect =
                                new OleDbCommand(Entity_Details_CoreQueries.GetCountOfAddressByTransType(this.DatabaseType, transType,requestModel), conn))
                            {
                                OleDbDataReader dr = objCmdSelect.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        count = Convert.ToInt32(dr[0].ToString());
                                    }
                                }
                                Utilities.WriteLog("Fill count on :" + DateTime.Now);
                            }
                           
                        }
                        using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.GetAddressByTransType(this.DatabaseType, transType, requestModel), conn))
                        {
                            da.SelectCommand = objCmdSelect;
                            DataTable dt = new DataTable();
                            da.Fill(requestModel.first, requestModel.rows, dt);
                            if (dt.Rows != null && dt.Rows.Count > 0)
                            {
                                returnValue = new List<EntityDetailsCoreAddress>();
                                foreach (DataRow row in dt.Rows)
                                {
                                    returnValue.Add(Load_Entity_Details_Core_address(row));
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
                ErrorMessage = "Exception Occured While Getting Address List. " + ex.Message + " " + ex.InnerException;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<EntityDetailsCoreMin> GetEntitiesByTransTypeAndAddressAndEntityJoinId(string transType, string address, long entityJoinId)
        {
            const string METHOD_NAME = "EntityDetailsCoreDB.GetEntitiesByTransTypeAndAddressAndEntityJoinId()";
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.GetEntitiesByTransTypeAndAddressAndEntityJoinId(this.DatabaseType, transType, address, entityJoinId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCoreMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadEDCMin(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Record Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Entities By TransType And Entity Join Id.", ex);
            }
            return returnValue;
        }

        public List<EntityDetailsCoreMin> getPropertyAddress(string TransType)
        {
            const string METHOD_NAME = "EntityDetailsCoreDB.getPropertyAddress()";
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Entity_Details_CoreQueries.getSelectAllByTransType(this.DatabaseType, TransType,null), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EntityDetailsCoreMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadEDCMin(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Property Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Property Address.", ex);
            }
            return returnValue;
        }

        public bool IsColumnExist(OleDbDataReader reader, string column)
        {
            bool ColumnExists = false;
            try
            {
                int columnOrdinal = reader.GetOrdinal(column);
                ColumnExists = true;
            }
            catch (IndexOutOfRangeException)
            {
                ColumnExists = false;
            }

            return ColumnExists;
        }

        public EntityDetailsCore GetEntityWithCloudFields(long entityId)
        {

            EntityDetailsCore edc = null;
            string qry = @"select * from un_entity_details_core edc " +
                    " left outer join un_cld_edc cld on edc.entity_id = cld.entity_id where edc.entity_id = " + entityId + " and edc.flg_deleted =" + Utilities.GetBooleanForDML(DatabaseType, false);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                edc = new EntityDetailsCore();
                                edc = Load_Entity_Details_Core(dr);
                                if (edc.EdcCloudFields == null) edc.EdcCloudFields = new EdcCloud();
                                edc.EdcCloudFields.ContactInvoiceCabId = DBUtil.GetStringValue(dr, "contact_invoice_cab_id");
                                edc.EdcCloudFields.ContactPOCabId = DBUtil.GetStringValue(dr, "contact_invoice_cab_id");
                                edc.EdcCloudFields.ContactDNCabId = DBUtil.GetStringValue(dr, "contact_invoice_cab_id");
                                edc.EdcCloudFields.RossumContactName = DBUtil.GetStringValue(dr, "rossum_contact_name");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting Entities By Name. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return edc;
        }

        public bool SaveEdcCloudFields(EntityDetailsCore edc)
        {
            bool returnValue = false;
            long entityId = long.Parse(edc.EntityId.ToString());

            try
            {
                string qryExistance = @"select * from un_cld_edc where entity_id = " + entityId;
                string qryInsert = @"insert into un_cld_edc (entity_id, contact_invoice_cab_id, contact_po_cab_id, contact_dn_cab_id, rossum_contact_name) " +
                    " Values (" + edc.EntityId +
                    ", '" + edc.EdcCloudFields.ContactInvoiceCabId + "' " +
                    ", '" + edc.EdcCloudFields.ContactPOCabId + "' " +
                    ", '" + edc.EdcCloudFields.ContactDNCabId + "' " +
                    ", '" + edc.EdcCloudFields.RossumContactName + "')";

                string qryUpdate = @"update un_cld_edc set " +
                    " contact_invoice_cab_id = '" + edc.EdcCloudFields.ContactInvoiceCabId + "' " +
                    ", contact_po_cab_id = '" + edc.EdcCloudFields.ContactPOCabId + "' " +
                    ", contact_dn_cab_id = '" + edc.EdcCloudFields.ContactDNCabId + "' " +
                    ", rossum_contact_name = '" + edc.EdcCloudFields.RossumContactName + "' " +
                    " where entity_id = " + edc.EntityId;

                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCommand = new OleDbCommand(qryExistance, conn))
                    {
                        using (OleDbDataReader dr = objCommand.ExecuteReader())
                        {
                            if (dr.HasRows)
                                objCommand.CommandText = qryUpdate;
                            else
                                objCommand.CommandText = qryInsert;
                        }

                        int resp = objCommand.ExecuteNonQuery();
                        if (resp > 0) returnValue = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting Entities By Name. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        //public bool updateJobAddress(EntityDetailsCore edc)
        //{
        //    bool returnValue = false;

        //    using (OleDbConnection conn = this.getDbConnection())
        //    {
        //        using (OleDbCommand objCmdUpdate =
        //            new OleDbCommand(Entity_Details_CoreQueries.UpdateJobAddress(this.DatabaseType, edc), conn))
        //        {
        //            objCmdUpdate.ExecuteNonQuery();
        //        }
        //        returnValue = true;
        //    }

        //    return returnValue;
        //}
    }
}
