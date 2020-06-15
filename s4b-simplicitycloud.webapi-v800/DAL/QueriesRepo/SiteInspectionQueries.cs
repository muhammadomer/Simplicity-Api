using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class SiteInspectionQueries
    {

        internal static string SelectMatchingContactNoByContactNo(string datebaseType, string contactNo)
        {
            string returnValue = "";
           
            returnValue = "SELECT p1_contact_no " +
                    "  FROM un_s4b_submissions_data_fh" +
                    " WHERE p1_contact_no like '%" + contactNo + "%'";
            return returnValue;
        }

        internal static string SelectAllFieldsOfSubmissionDataFhTableBySequence(string datebaseType, long sequence)
        {
            string returnValue = "";
            returnValue = "SELECT * " +
                    "  FROM un_s4b_submissions_data_fh" +
                    " WHERE sequence = " + sequence;
            return returnValue;
        }

        internal static string SelectAllFieldsOfSubmissionImagesFhTableByJoinSequence(string datebaseType, long joinSequence)
        {
            string returnValue = "";
            returnValue = "SELECT * " +
                            "  FROM un_s4b_submissions_images_fh " +
                            " WHERE join_sequence = " + joinSequence;
            return returnValue;
        }

        internal static string SelectAllFieldsOfSubmissionDataFhiTableByJoinSequence(string datebaseType, long joinSequence)
        {
            string returnValue = "";
            returnValue = "SELECT * " +
                    "  FROM un_s4b_submissions_data_fhi" +
                    " WHERE join_sequence = " + joinSequence;
            return returnValue;
        }

        internal static string UpdateSubmissionsDataFh(string databaseType, SubmissionsDataFh siteInspection)
        {
            string returnValue = "";
            returnValue = "UPDATE un_s4b_submissions_data_fh" +
                "  SET submit_no  = '" + siteInspection.SubmitNo + "', " +
                " submit_ts = '" + siteInspection.SubmitTs + "', " +
                //" date_submit = " + (siteInspection.DateSubmit.HasValue ? ("#" + siteInspection.DateSubmit.Value.ToString("MM/dd/yyyy") + "#") : "NULL") + " , " +
                " date_submit = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.DateSubmit,true,true) + " , " +
                " p1_contract_no = '" + siteInspection.P1ContractNo + "', " +
                " p1_site_address = '" + siteInspection.P1SiteAddress + "', " +
                " p1_client_name = '" + siteInspection.P1ClientName + "', " +
                " p1_tank_type = '" + siteInspection.P1TankType + "', " +
                " p1_tank_size = '" + siteInspection.P1TankSize + "', " +
                //" p1_report_date = " + (siteInspection.P1ReportDate.HasValue ? ("#" + siteInspection.P1ReportDate.Value.ToString("MM/dd/yyyy") + "#") : "NULL") + " , " +
                " p1_report_date = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.P1ReportDate,true,true) + " , " +
                // " p1_inspected_by = '" + siteInspection.P1InspectedBy + "', " +
                " p2_location = '" + siteInspection.P2Location + "', " +
                //" p2_installation_date = " + (siteInspection.P2InstallationDate.HasValue ? ("#" + siteInspection.P2InstallationDate.Value.ToString("MM/dd/yyyy") + "#") : "NULL") + " , " +
                " p2_installation_date = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.P2InstallationDate,true,true) + " , " +
                " p2_installation_date_unkown = '" + siteInspection.P2InstallationDateUnknown +"', " +
                " p2_contact_name = '" + siteInspection.P2ContactName + "', " +
                " p2_contact_tel_no = '" + siteInspection.P2ContactTelNo + "', " +
                " p2_contact_ext = '" + siteInspection.P2ContacText  + "', " +
                " p2_contact_mobile = '" + siteInspection.P2ContactMobile + "', " +
                " p2_lcontacts = '" + siteInspection.P2LContacts + "', " +
                " p2_visit_status = '" + siteInspection.P2VisitStatus + "', " +
                " p2_manufacturer_details = '" + siteInspection.P2ManufacturerDetails + "', " +
                " p2_visit_purpose = '" + siteInspection.P2VisitPurpose + "', " +
                " p2_visit_purpose2 = '" + siteInspection.P2VisitPurpose2 + "', " +
                " p3_tank_status = '" + siteInspection.P3TankStatus + "', " +
                " p3_tank_diameter = '" + siteInspection.P3TankDiameter + "', " +
                " p3_tank_shape = '" + siteInspection.P3TankShape + "', " +
                // " p3_tank_size = '" + siteInspection.P3Tanksize + "', " +
                " p3_tank_manufacturer = '" + siteInspection.P3TankManufacturer + "', " +
                " p3_panel_dimensions = '" + siteInspection.P3PanelDimensions + "', " +
                // " p3_tank_type = '" + siteInspection.P3TankType + "', " +
                " p3_top_panel_dimensions = '" + siteInspection.P3ToppanelDimensions + "', " +
                //" p3_tank_installation_date = " + (siteInspection.P3TankInstallationDate.HasValue ? ("#" + siteInspection.P3TankInstallationDate.Value.ToString("MM/dd/yyyy") + "#") : "NULL") + " , " +
                " p3_tank_installation_date = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.P3TankInstallationDate ,true,true) + " , " +
                " p3_hoz_bolt_seam = '" + siteInspection.P3Hozboltseam + "', " +
                " p3_tank_height = '" + siteInspection.P3TankHeight + "', " +
                " p3_actual_capacity = '" + siteInspection.P3ActualCapacity + "', " +
                " p3_tank_details = '" + siteInspection.P3TankDetails + "', " +
                " p3_tank_shell_details = '" + siteInspection.P3TankShellDetails + "', " +
                " p3_tank_shell_details2 = '" + siteInspection.P3TankShellDetails2 + "', " +
                "  p4_roof_tank_shell_details = '" + siteInspection.P4RoofTankShellDetails +"'," +
                " p4_test_return = '" + siteInspection.P4TestReturn + "', " +
                " p4_drain_valve = '" + siteInspection.P4DrainValve + "', " +
                " p4_suction = '" + siteInspection.P4Suction + "', " +
                " p4_overflows = '" + siteInspection.P4OverFlows + "', " +
                " p4_inlet_valve = '" + siteInspection.P4InletValve + "', " +
                " p4_immersion_heater = '" + siteInspection.P4ImmersionHeater + "', " +
                " p4_low_level_manway = '" + siteInspection.P4LowLevelManWay + "', " +
                " p6_ancillary_items = '" + siteInspection.P6AncillaryItems + "', " +
                " p6_external_ladder = '" + siteInspection.P6ExternalLadder + "', " +
                " p6_inlet_valve_housing = '" + siteInspection.P6InletValveHousing + "', " +
                " p7_observations01 = '" + siteInspection.P7Observations01 + "', " +
                " p7_observations02 = '" + siteInspection.P7Observations02 + "', " +
                " p7_observations03 = '" + siteInspection.P7Observations03 + "', " +
                " p7_observations04 = '" + siteInspection.P7Observations04 + "', " +
                " p7_observations05 = '" + siteInspection.P7Observations05 + "', " +
                " p7_observations06 = '" + siteInspection.P7Observations06 + "', " +
                " p7_observations07 = '" + siteInspection.P7Observations07 + "', " +
                " p7_observations08 = '" + siteInspection.P7Observations08 + "', " +
                " p7_observations09 = '" + siteInspection.P7Observations09 + "', " +
                " p7_observations10 = '" + siteInspection.P7Observations10 + "', " +
                " p7_conclusions = '" + siteInspection.P7Conclusions + "', " +
                " p7_signature_image_file = '" + siteInspection.P7SignatureImageFile + "', " +
                //   " created_by = " + siteInspection.CreatedBy + ", " +
                //   " date_created = " + (siteInspection.DateCreated.HasValue ? ("#" + siteInspection.DateCreated.Value.ToString("MM/dd/yyyy") + "#") : "NULL") + " , " +
                " last_amended_by = " + siteInspection.LastAmendedBy + ", " +
                //" date_last_amended = " + (siteInspection.DateLastAmended.HasValue ? ("#" + siteInspection.DateLastAmended.Value.ToString("MM/dd/yyyy") + "#") : "NULL") +
                " date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.DateLastAmended ,true,true) + "," +
                " p8_contents_gauge = '" + siteInspection.P8ContentsGauge + "'," +
                " p8_level_switches = '" + siteInspection.P8LevelSwitches + "'" +
                " WHERE sequence = " + siteInspection.Sequence;
               
            return returnValue;
        }

        internal static string UpdateFileCabIdAndPdfCountForSubmissionsDataFh(string databaseType, SubmissionsDataFh siteInspection)
        {
            string returnValue = "";
            returnValue = "UPDATE un_s4b_submissions_data_fh" +
                            "   SET file_cab_id  = '" + siteInspection.FileCabId + "', " +
                            "       created_pdf_count = " + siteInspection.CreatedPDFCount + ", " +
                            "       last_amended_by = " + siteInspection.LastAmendedBy + ", " +
                            "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.DateLastAmended ,true,true) +
                            " WHERE sequence = " + siteInspection.Sequence;
                   
            return returnValue;
        }

        internal static string UpdateSubmissionsDataFhi(string databaseType, SubmissionsDataFhi siteInspection)
        {
            
                string    returnValue = "UPDATE un_s4b_submissions_data_fhi" +
                         "  SET page_no  = " + siteInspection.PageNo + ", " +
                         " row_no = " + siteInspection.RowNo + ", " +
                         " row_size = '" + siteInspection.RowSize + "' , " +
                         " row_qty = " + siteInspection.RowQty + ", " +
                         " row_condition = '" + siteInspection.RowCondition + "', " +
                         " row_comments = '" + siteInspection.RowComments + "', " +
                         " last_amended_by = " + siteInspection.LastAmendedBy + ", " +
                         //" date_last_amended = " + (siteInspection.DateLastAmended.HasValue ? ("#" + siteInspection.DateLastAmended.Value.ToString("MM/dd/yyyy") + "#") : "NULL") +
                         " date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, siteInspection.DateLastAmended ,true,true) +
                         " WHERE sequence = " + siteInspection.Sequence;
                   
            return returnValue;
        }

        internal static string InsertSubmissionsImagesFH(string databaseType, SubmissionsImagesFh submissionImagesFh)
        {
            string returnValue = "";
            returnValue = "INSERT INTO un_s4b_submissions_images_fh(join_sequence, flg_fixed_image, page_no, field_id, file_display_name, file_path, " +
                            "       created_by, date_created) " +
                            "VALUES (" + submissionImagesFh.JoinSequence + ", " + 
                            "      " + submissionImagesFh.FixedImage + ", " + 
                            "      " + submissionImagesFh.PageNo + "," +
                            "     '" + submissionImagesFh.FieldId + "'," +
                            "     '" + Utilities.replaceSpecialChars(submissionImagesFh.FileDisplayName) + "'," +
                            "     '" + submissionImagesFh.FilePath + "'," +
                            "      " + submissionImagesFh.CreatedBy + ", " + 
                            "      " + Utilities.GetDateTimeForDML(databaseType, (submissionImagesFh.DateCreated.HasValue ? submissionImagesFh.DateCreated : DateTime.Now),true,true)   + ")";
                   
            return returnValue;
        }

        internal static string UpdateSubmissionsImagesFH(string databaseType, SubmissionsImagesFh submissionImagesFh)
        {
            string returnValue = "";
            returnValue = "Update  un_s4b_submissions_images_fh Set "
                    + " page_no = " + submissionImagesFh.PageNo
                    + " , field_id = '" + submissionImagesFh.FieldId +"'"
                    + ", file_display_name= '" + Utilities.replaceSpecialChars(submissionImagesFh.FileDisplayName) + "'" 
                    + ", file_path = '" + submissionImagesFh.FilePath +"'"
                    + ", last_amended_by = " + submissionImagesFh.LastAmendedBy
                    + ", date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, (submissionImagesFh.DateLastAmended.HasValue ? submissionImagesFh.DateLastAmended.Value : DateTime.Now),true,true) 
                    + " Where Sequence=" + submissionImagesFh.Sequence;
                   
            return returnValue;
        }

        internal static string InsertSubmissionsDataFh(string databaseType, SubmissionsDataFh siteInspection)
        {
            string returnValue = "";
            returnValue = "INSERT INTO un_s4b_submissions_data_fh(submit_no, submit_ts, date_submit, created_pdf_count, file_cab_id, p1_contract_no, p1_site_address, p1_client_name, p1_tank_type, " +
                    "       p1_tank_size, p1_report_date, p1_inspected_by, p2_location, p2_installation_date, p2_installation_date_unkown, p2_contact_name, " +
                    "       p2_contact_tel_no, p2_contact_ext, p2_contact_mobile, p2_lcontacts, p2_visit_status, " +
                    "       p2_manufacturer_details, p2_visit_purpose, p2_visit_purpose2, p3_tank_status, p3_tank_diameter, " +
                    "       p3_tank_shape, p3_tank_size, p3_tank_manufacturer, p3_panel_dimensions, p3_tank_type, " +
                    "       p3_top_panel_dimensions, p3_tank_installation_date, p3_hoz_bolt_seam, p3_tank_height, p3_actual_capacity, " +
                    "       p3_tank_details, p3_tank_shell_details, p3_tank_shell_details2 , p4_roof_tank_shell_details, p4_test_return, p4_drain_valve, " +
                    "       p4_suction, p4_overflows, p4_inlet_valve, p4_immersion_heater, p4_low_level_manway, " +
                    "       p6_ancillary_items, p6_external_ladder, p6_inlet_valve_housing, p7_observations01, p7_observations02, " +
                    "       p7_observations03, p7_observations04, p7_observations05, p7_observations06, p7_observations07, " +
                    "       p7_observations08, p7_observations09, p7_observations10, p7_conclusions, p7_signature_image_file, " +
                    "       p8_contents_gauge, p8_level_switches, created_by, date_created) " +
                    " VALUES ('" + Utilities.replaceSpecialChars(siteInspection.SubmitNo) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.SubmitTs) + "', " + 
                    Utilities.GetDateValueForDML(databaseType, siteInspection.DateSubmit) + "," + 
                    siteInspection.CreatedPDFCount + ", '" +
                    Utilities.replaceSpecialChars(siteInspection.FileCabId) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P1ContractNo) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P1SiteAddress) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P1ClientName) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P1TankType) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P1TankSize) + "', " + 
                    Utilities.GetDateValueForDML(databaseType, siteInspection.P1ReportDate) + ", '" +
                    Utilities.replaceSpecialChars(siteInspection.P1InspectedBy) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P2Location) + "', " +
                    Utilities.GetDateValueForDML(databaseType, siteInspection.P2InstallationDate) + ", '" +
                    Utilities.replaceSpecialChars(siteInspection.P2InstallationDateUnknown) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P2ContactName) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P2ContactTelNo) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P2ContacText) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P2ContactMobile) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P2LContacts) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P2VisitStatus) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P2ManufacturerDetails) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P2VisitPurpose) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P2VisitPurpose2) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3TankStatus) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P3TankDiameter) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3TankShape) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3Tanksize) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3TankManufacturer) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P3PanelDimensions) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3TankType) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3ToppanelDimensions) + "', " +
                    Utilities.GetDateValueForDML(databaseType,  siteInspection.P3TankInstallationDate) + ", '" +
                    Utilities.replaceSpecialChars(siteInspection.P3Hozboltseam) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3TankHeight) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3ActualCapacity) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P3TankDetails) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P3TankShellDetails) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P3TankShellDetails2) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P4RoofTankShellDetails) + "','" +
                    Utilities.replaceSpecialChars(siteInspection.P4TestReturn) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P4DrainValve) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P4Suction) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P4OverFlows) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P4InletValve) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P4ImmersionHeater) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P4LowLevelManWay) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P6AncillaryItems) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P6ExternalLadder) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P6InletValveHousing) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P7Observations01) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations02) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations03) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations04) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P7Observations05) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations06) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations07) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations08) + "', '" +
                    Utilities.replaceSpecialChars(siteInspection.P7Observations09) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Observations10) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7Conclusions) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P7SignatureImageFile) + "','" +
                    Utilities.replaceSpecialChars(siteInspection.P8ContentsGauge) + "', '" + 
                    Utilities.replaceSpecialChars(siteInspection.P8LevelSwitches) + "', " +
                    siteInspection.CreatedBy + ", " +
                    Utilities.GetDateTimeForDML(databaseType,siteInspection.DateCreated ,true,true) + ")";
                    
            return returnValue;
        }

        internal static string InsertSubmissionsDataFhi(string databaseType, SubmissionsDataFhi siteInspection)
        {
            string returnValue = "";
            returnValue = "INSERT Into un_s4b_submissions_data_fhi" +
                    "(join_sequence, page_no,row_no,row_size,row_qty,row_condition,row_comments,last_amended_by,date_last_amended) " +
                    "Values (" + siteInspection.JoinSequence + ",  " + siteInspection.PageNo + ", " + siteInspection.RowNo + ", '" + siteInspection.RowSize + "', " +
                    siteInspection.RowQty + ", '" + siteInspection.RowCondition + "', '" + siteInspection.RowComments + "', " +
                    siteInspection.LastAmendedBy + ", " 
                    + Utilities.GetDateTimeForDML(databaseType, siteInspection.DateLastAmended,true,true) + ")";

            return returnValue;
        }

        internal static string InsertSubmissionsData2(string databaseType, S4BSubmissionsData2 siteInspection)
        {
            string returnValue = "";
            returnValue = @"INSERT INTO un_s4b_submissions_data_2(join_sequence, page_number, field_name, field_data, field_position, field_type ,created_by, date_created) " +
                " VALUES (" + siteInspection.JoinSequence
                        + ", " + siteInspection.PageNumber
                        + ",'" + Utilities.replaceSpecialChars(siteInspection.FieldName) + "'"
                        + ", '" + Utilities.replaceSpecialChars(siteInspection.FieldData) + "'"
                        + ", " + siteInspection.FieldPosition
                        + ",'" + Utilities.replaceSpecialChars(siteInspection.FieldType) + "'"
                        + "," + siteInspection.CreatedBy
                        + ", " + Utilities.GetDateTimeForDML(databaseType,siteInspection.DateCreated,true,true) + ")";
            return returnValue;
        }
        public static string getRecordsList(string databaseType)
        {
            string returnValue = "";
            returnValue = "SELECT *  FROM un_s4b_submissions_data_fh";
            return returnValue;
        }
    }
}
