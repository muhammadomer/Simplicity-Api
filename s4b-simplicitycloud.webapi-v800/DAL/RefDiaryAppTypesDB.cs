using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefDiaryAppTypesDB : MainDB
    {

        public RefDiaryAppTypesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public RefDiaryAppTypes insertRefDiaryAppTypes(RefDiaryAppTypes obj)
        {
            RefDiaryAppTypes returnValue = new RefDiaryAppTypes();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string sql = "select MAX(AppTypeCode) from un_ref_diary_app_types";
                    using (OleDbCommand cmdObj = new OleDbCommand(sql, conn))
                    {
                        Object result = cmdObj.ExecuteScalar();
                        obj.AppTypeCode = Convert.ToInt32(result) + 1;
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefDiaryAppTypesQueries.insert(this.DatabaseType, obj), conn))
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


        public List<RefDiaryAppTypes> selectAllRefDiaryAppTypesappTypeCode(long appTypeCode)
        {
            List<RefDiaryAppTypes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefDiaryAppTypesQueries.getSelectAllByappTypeCode(this.DatabaseType, appTypeCode), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefDiaryAppTypes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefDiaryAppTypes(dr));
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


        public List<RefDiaryAppTypes> selectAllRefDiaryAppTypes()
        {
            List<RefDiaryAppTypes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefDiaryAppTypesQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefDiaryAppTypes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefDiaryAppTypes(dr));
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

        public RefDiaryAppTypes updateByappTypeCode(RefDiaryAppTypes obj)
        {
            RefDiaryAppTypes returnValue = new RefDiaryAppTypes();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefDiaryAppTypesQueries.update(this.DatabaseType, obj), conn))
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


        public bool deleteByappTypeCode(long appTypeCode)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefDiaryAppTypesQueries.delete(this.DatabaseType, appTypeCode), conn))
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

        public bool deleteByFlgDeleted(long appTypeCode)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefDiaryAppTypesQueries.deleteFlagDeleted(this.DatabaseType, appTypeCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private RefDiaryAppTypes Load_RefDiaryAppTypes(OleDbDataReader dr)

        {
            RefDiaryAppTypes refDiaryAppTypes = null;
            try
            {
                if (dr != null)
                {

                    refDiaryAppTypes = new RefDiaryAppTypes();
                    refDiaryAppTypes.AppTypeCode = long.Parse(dr["app_type_code"].ToString());
                    refDiaryAppTypes.AppTypeSequence = long.Parse(dr["app_type_sequence"].ToString());
                    refDiaryAppTypes.AppTypeDesc = Utilities.GetDBString(dr["app_type_desc"]);
                    refDiaryAppTypes.AppTypeIconPath = Utilities.GetDBString(dr["app_type_icon_path"]);
                    refDiaryAppTypes.FlgAppTypeAccounts = bool.Parse(dr["flg_app_type_accounts"].ToString());
                    refDiaryAppTypes.FlgAppTypeCertificate = bool.Parse(dr["flg_app_type_certificate"].ToString());
                    refDiaryAppTypes.CertificateSequence = long.Parse(dr["certificate_sequence"].ToString());
                    refDiaryAppTypes.FlgAppTypeSim = bool.Parse(dr["flg_app_type_sim"].ToString());
                    refDiaryAppTypes.SimIndex = long.Parse(dr["sim_index"].ToString());
                    refDiaryAppTypes.FlgAppTypeSimDueVisit = bool.Parse(dr["flg_app_type_sim_due_visit"].ToString());
                    refDiaryAppTypes.FlgAppTypeSimRevisit = bool.Parse(dr["flg_app_type_sim_revisit"].ToString());
                    refDiaryAppTypes.FlgAppTypeVeh = bool.Parse(dr["flg_app_type_veh"].ToString());
                    refDiaryAppTypes.VehIndex = long.Parse(dr["veh_index"].ToString());
                    refDiaryAppTypes.FlgAppTypeVehDueVisit = bool.Parse(dr["flg_app_type_veh_due_visit"].ToString());
                    refDiaryAppTypes.FlgAppTypeVehDueRevisit = bool.Parse(dr["flg_app_type_veh_due_revisit"].ToString());
                    refDiaryAppTypes.FlgAppTypeVehDueSer = bool.Parse(dr["flg_app_type_veh_due_ser"].ToString());
                    refDiaryAppTypes.FlgAppTypeVehDuePmi = bool.Parse(dr["flg_app_type_veh_due_pmi"].ToString());
                    refDiaryAppTypes.FlgAppTypeVehDueMot = bool.Parse(dr["flg_app_type_veh_due_mot"].ToString());
                    refDiaryAppTypes.FlgAppTypeVehDueIns = bool.Parse(dr["flg_app_type_veh_due_ins"].ToString());
                    refDiaryAppTypes.FlgAppTypeMnt = bool.Parse(dr["flg_app_type_mnt"].ToString());
                    refDiaryAppTypes.FlgAppTypeHoliday = bool.Parse(dr["flg_app_type_holiday"].ToString());
                    refDiaryAppTypes.FlgAppTypeTool = bool.Parse(dr["flg_app_type_tool"].ToString());
                    refDiaryAppTypes.ToolIndex = long.Parse(dr["tool_index"].ToString());
                    refDiaryAppTypes.FlgAppTypeToolDueVisit = bool.Parse(dr["flg_app_type_tool_due_visit"].ToString());
                    refDiaryAppTypes.FlgAppTypePlant = bool.Parse(dr["flg_app_type_plant"].ToString());
                    refDiaryAppTypes.PlantIndex = long.Parse(dr["plant_index"].ToString());
                    refDiaryAppTypes.FlgAppTypePlantDueVisit = bool.Parse(dr["flg_app_type_plant_due_visit"].ToString());
                    refDiaryAppTypes.FlgAppTypePlantDueRevisit = bool.Parse(dr["flg_app_type_plant_due_revisit"].ToString());
                    refDiaryAppTypes.FlgAppTypePlantDueSer = bool.Parse(dr["flg_app_type_plant_due_ser"].ToString());

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refDiaryAppTypes;
        }

    }
}
