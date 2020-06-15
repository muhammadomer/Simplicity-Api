using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefDiaryAppTypesQueries
    {
        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT *  FROM un_ref_diary_app_types ";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByappTypeCode(string databaseType, long appTypeCode)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                                      "  FROM    un_ref_diary_app_types" +
                                      " WHERE app_type_code = " + appTypeCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string insert(string databaseType, RefDiaryAppTypes obj)
        {
            string returnValue = "";
            try
            {

               
                returnValue = "INSERT INTO   un_ref_diary_app_types( app_type_code,  app_type_sequence,  app_type_desc,  app_type_icon_path,  flg_app_type_accounts,  flg_app_type_certificate,  certificate_sequence,  flg_app_type_sim,  sim_index," +
                "                                    flg_app_type_sim_due_visit,  flg_app_type_sim_revisit,  flg_app_type_veh,  veh_index,  flg_app_type_veh_due_visit,  flg_app_type_veh_due_revisit,  flg_app_type_veh_due_ser, " +
                "                                    flg_app_type_veh_due_pmi,  flg_app_type_veh_due_mot,  flg_app_type_veh_due_ins,  flg_app_type_mnt,  flg_app_type_holiday,  flg_app_type_tool,  tool_index,  flg_app_type_tool_due_visit, " +
                "                                    flg_app_type_plant,  plant_index,  flg_app_type_plant_due_visit,  flg_app_type_plant_due_revisit,  flg_app_type_plant_due_ser )" +
                "VALUES ( " + obj.AppTypeCode + ",  " + obj.AppTypeSequence + ",   '" + obj.AppTypeDesc + "',   '" + obj.AppTypeIconPath + "',   " 
                 + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeAccounts) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeCertificate) + ",  " + obj.CertificateSequence + ",   " 
                 +Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeSim) + ",  " + obj.SimIndex + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeSimDueVisit) + ",   " 
                 + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeSimRevisit) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVeh) 
                 + ",  " + obj.VehIndex + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueVisit) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueRevisit) + ",   " 
                 + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueSer) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDuePmi) 
                 + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueMot) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueIns) 
                 + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeMnt) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeHoliday) 
                 + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeTool) + ",  " + obj.ToolIndex + ",   " 
                 +Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeToolDueVisit) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlant) 
                 + ",  " + obj.PlantIndex + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlantDueVisit) + ",   "
                 + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlantDueRevisit) + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlantDueSer) + ")";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, RefDiaryAppTypes obj)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "UPDATE   un_ref_diary_app_types" +
                                      "   SET  app_type_sequence =  " + obj.AppTypeSequence + ",  " +
                                      " app_type_desc =  '" + obj.AppTypeDesc + "',  " +
                                      " app_type_icon_path =  '" + obj.AppTypeIconPath + "',  " +
                                      " flg_app_type_accounts = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeAccounts) + ",  " +
                                      " flg_app_type_certificate = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeCertificate) + ",  " +
                                      " certificate_sequence =  " + obj.CertificateSequence + ",  " +
                                      " flg_app_type_sim = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeSim) + ",  " +
                                      " sim_index =  " + obj.SimIndex + ",  " +
                                      " flg_app_type_sim_due_visit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeSimDueVisit) + ",  " +
                                      " flg_app_type_sim_revisit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeSimRevisit) + ",  " +
                                      " flg_app_type_veh = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVeh) + ",  " +
                                      " veh_index =  " + obj.VehIndex + ",  " +
                                      " flg_app_type_veh_due_visit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueVisit) + ",  " +
                                      " flg_app_type_veh_due_revisit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueRevisit) + ",  " +
                                      " flg_app_type_veh_due_ser = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueSer) + ",  " +
                                      " flg_app_type_veh_due_pmi = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDuePmi) + ",  " +
                                      " flg_app_type_veh_due_mot = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueMot) + ",  " +
                                      " flg_app_type_veh_due_ins = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeVehDueIns) + ",  " +
                                      " flg_app_type_mnt = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeMnt) + ",  " +
                                      " flg_app_type_holiday = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeHoliday) + ",  " +
                                      " flg_app_type_tool = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeTool) + ",  " +
                                      " tool_index =  " + obj.ToolIndex + ",  " +
                                      " flg_app_type_tool_due_visit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypeToolDueVisit) + ",  " +
                                      " flg_app_type_plant = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlant) + ",  " +
                                      " plant_index =  " + obj.PlantIndex + ",  " +
                                      " flg_app_type_plant_due_visit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlantDueVisit) + ",  " +
                                      " flg_app_type_plant_due_revisit = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlantDueRevisit) + ",  " +
                                      " flg_app_type_plant_due_ser = " + Utilities.GetBooleanForDML(databaseType, obj.FlgAppTypePlantDueSer)  +
                                      "  WHERE app_type_code = " + obj.AppTypeCode;
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long appTypeCode)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "DELETE FROM   un_ref_diary_app_types" +
                                      "WHERE app_type_code = " + appTypeCode;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long appTypeCode)
        {
            string returnValue = "";
            try
            {
                
                        bool flg = true;
                        returnValue = "UPDATE   un_ref_diary_app_types" +
                                      "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                      " WHERE app_type_code = " + appTypeCode;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

