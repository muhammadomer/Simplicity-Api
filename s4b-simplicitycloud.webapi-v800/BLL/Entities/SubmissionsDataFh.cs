using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class SubmissionsDataFh : ResponseModel
    {
        public long? Sequence { get; set; }
        public string SubmitNo { get; set; }
        public string SubmitTs { get; set; }
        public DateTime? DateSubmit { get; set; }
        public bool FlgAmended { get; set; }
        public bool FlgResubmission { get; set; }
        public int CreatedPDFCount { get; set; }
        public string FileCabId { get; set; }
        public string P1ContractNo { get; set; }
        public string P1SiteAddress { get; set; }
        public string P1ClientName { get; set; }
        public string P1TankType { get; set; }
        public string P1TankSize { get; set; }
        public DateTime? P1ReportDate { get; set; }
        public string P1InspectedBy { get; set; }
        public string P2Location { get; set; }
        public DateTime? P2InstallationDate { get; set; }
        public string P2InstallationDateUnknown { get; set; }
        public string P2ContactName { get; set; }
        public string P2ContactTelNo { get; set; }
        public string P2ContacText { get; set; }
        public string P2ContactMobile { get; set; }
        public string P2LContacts { get; set; }
        public string P2VisitStatus { get; set; }
        public string P2ManufacturerDetails { get; set; }
        public string P2VisitPurpose { get; set; }
        public string P2VisitPurpose2 { get; set; }
        public string P3TankStatus { get; set; }
        public string P3TankDiameter { get; set; }
        public string P3TankShape { get; set; }
        public string P3Tanksize { get; set; }
        public string P3TankManufacturer { get; set; }
        public string P3PanelDimensions { get; set; }
        public string P3TankType { get; set; }
        public string P3ToppanelDimensions { get; set; }
        public DateTime? P3TankInstallationDate { get; set; }
        public string P3Hozboltseam { get; set; }
        public string P3TankHeight { get; set; }
        public string P3ActualCapacity { get; set; }
        public string P3TankDetails { get; set; }
        public string P3TankShellDetails { get; set; }
        public string P3TankShellDetails2 { get; set; }
        public string P4RoofTankShellDetails { get; set; }
        public string P4TestReturn { get; set; }
        public string P4DrainValve { get; set; }
        public string P4Suction { get; set; }
        public string P4OverFlows { get; set; }
        public string P4InletValve { get; set; }
        public string P4ImmersionHeater { get; set; }
        public string P4LowLevelManWay { get; set; }
        public string P6AncillaryItems { get; set; }
        public string P6ExternalLadder { get; set; }
        public string P6InletValveHousing { get; set; }
        public string P7Observations01 { get; set; }
        public string P7Observations02 { get; set; }
        public string P7Observations03 { get; set; }
        public string P7Observations04 { get; set; }
        public string P7Observations05 { get; set; }
        public string P7Observations06 { get; set; }
        public string P7Observations07 { get; set; }
        public string P7Observations08 { get; set; }
        public string P7Observations09 { get; set; }
        public string P7Observations10 { get; set; }
        public string P7Conclusions { get; set; }
        public string P7SignatureImageFile { get; set; }
        public string P8ContentsGauge { get; set; }
        public string P8LevelSwitches { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

        public string CreatedByUserName { get; set; }
        public string LastAmendedByUserName { get; set; }

        public List<SubmissionsDataFhi> TankConnections { get; set; }
        public List<SubmissionsDataFhi> AncillaryItems { get; set; }
        public List<SubmissionsImagesFh> SiteInspectionImages { get; set; }

        internal Dictionary<string, string> GetFieldMappingDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("FIELD_VAR_JOB_REFERENCE", this.P1ContractNo);
            dictionary.Add("FIELD_PG1_SITE_ADDRESS", this.P1SiteAddress);
            dictionary.Add("FIELD_PG1_CLIENT_NAME", this.P1ClientName);
            dictionary.Add("FIELD_PG1_TANK_TYPE", P1TankType);
            dictionary.Add("FIELD_PG1_TANK_SIZE", P1TankSize);
            dictionary.Add("FIELD_PG1_REPORT_DATE", P1ReportDate.ToString());
            dictionary.Add("FIELD_PG1_INSPECTION_BY", P1InspectedBy);
            dictionary.Add("FIELD_PG2_LOCATION", P2Location);
            dictionary.Add("FIELD_PG2_INSTALLATION_DATE", P2InstallationDate.ToString());
            dictionary.Add("FIELD_PG2_INSTALLATION_DATE_UNKNOWN", P2InstallationDateUnknown.ToString());
            dictionary.Add("FIELD_PG2_CONTACT_NAME", P2ContactName);
            dictionary.Add("FIELD_PG2_CONTACT_TEL_NO", P2ContactTelNo);
            dictionary.Add("FIELD_PG2_CONTACT_EXT", P2ContacText);
            dictionary.Add("FIELD_PG2_CONTACT_MOBILE_NO", P2ContactMobile);
            dictionary.Add("FIELD_PG2_CONTACTS", P2LContacts);
            dictionary.Add("FIELD_PG2_VISIT_STATUS", P2VisitStatus);
            dictionary.Add("FIELD_PG2_MANUFACTURER_DETAILS", P2ManufacturerDetails);
            dictionary.Add("FIELD_PG2_VISIT_PURPOSE", P2VisitPurpose);
            dictionary.Add("FIELD_PG2_VISIT_PURPOSE2", P2VisitPurpose2);
            dictionary.Add("FIELD_PG3_TANK_STATUS", P3TankStatus);
            dictionary.Add("FIELD_PG3_TANK_DIAMETER", P3TankDiameter);
            dictionary.Add("FIELD_PG3_TANK_SHAPE", P3TankShape);
            dictionary.Add("FIELD_PG3_TANK_SIZE", P3Tanksize);
            dictionary.Add("FIELD_PG3_TANK_MANUFACRURER", P3TankManufacturer);
            dictionary.Add("FIELD_PG3_PANEL_DIMENSIONS", P3PanelDimensions);
            dictionary.Add("FIELD_PG3_TANK_TYPE", P3TankType);
            dictionary.Add("FIELD_PG3_TOP_PANEL_DIMENSIONS", P3ToppanelDimensions);
            dictionary.Add("FIELD_PG3_TANK_INSTALLATION_DATE", P3TankInstallationDate.ToString());
            dictionary.Add("FIELD_PG3_HOZ_BOLT_SEAM", P3Hozboltseam);
            dictionary.Add("FIELD_PG3_TANK_HEIGHT", P3TankHeight);
            dictionary.Add("FIELD_PG3_ACTUAL_CAPACITY", P3ActualCapacity);
            dictionary.Add("FIELD_PG3_TANK_DETAILS", P3TankDetails);
            dictionary.Add("FIELD_PG3_TANK_SHELL_DETAILS", P3TankShellDetails);
            dictionary.Add("FIELD_PG3_TANK_SHELL_EXTERNAL2", P3TankShellDetails2);
            dictionary.Add("FIELD_PG4_ROOF_TANK_SHELL_DETAILS", P4RoofTankShellDetails);
            dictionary.Add("FIELD_PG4_TEST_RETURN", P4TestReturn);
            dictionary.Add("FIELD_PG4_DRAIN_VALVE", P4DrainValve);
            dictionary.Add("FIELD_PG4_SUCTION", P4Suction);
            dictionary.Add("FIELD_PG4_OVERFLOWS", P4OverFlows);
            dictionary.Add("FIELD_PG4_INLET_FLOAT_VALVE", P4InletValve);
            dictionary.Add("FIELD_PG4_IMMERSION_HEATER", P4ImmersionHeater);
            dictionary.Add("FIELD_PG4_LOW_LEVEL_MANWAY", P4LowLevelManWay);
            dictionary.Add("FIELD_PG6_ANCILLARY_ITEMS", P6AncillaryItems);
            dictionary.Add("FIELD_PG6_EXTERNAL_LADDER", P6ExternalLadder);
            dictionary.Add("FIELD_PG6_INLET_VALVE_HOUSING", P6InletValveHousing);
            dictionary.Add("FIELD_PG7_ROW01_OBSERVATIONS", P7Observations01);
            dictionary.Add("FIELD_PG7_ROW02_OBSERVATIONS", P7Observations02);
            dictionary.Add("FIELD_PG7_ROW03_OBSERVATIONS", P7Observations03);
            dictionary.Add("FIELD_PG7_ROW04_OBSERVATIONS", P7Observations04);
            dictionary.Add("FIELD_PG7_ROW05_OBSERVATIONS", P7Observations05);
            dictionary.Add("FIELD_PG7_ROW06_OBSERVATIONS", P7Observations06);
            dictionary.Add("FIELD_PG7_ROW07_OBSERVATIONS", P7Observations07);
            dictionary.Add("FIELD_PG7_ROW08_OBSERVATIONS", P7Observations08);
            dictionary.Add("FIELD_PG7_ROW09_OBSERVATIONS", P7Observations09);
            dictionary.Add("FIELD_PG7_ROW10_OBSERVATIONS", P7Observations10);
            dictionary.Add("FIELD_PG7_RECOMMENDATIONS", P7Conclusions);
            dictionary.Add("FIELD_PG8_CONTENTS_GAUGE", P8ContentsGauge);
            dictionary.Add("FIELD_PG8_LEVEL_SWITCHES", P8LevelSwitches);
            //dictionary.Add("FIELD_PG7_SIGNATURE", P7SignatureImageFile);

            // Add Site inspection images to the dictionary
            if (SiteInspectionImages != null && SiteInspectionImages.Count > 0)
            {
                foreach (SubmissionsImagesFh imageItem in SiteInspectionImages)
                {
                    if (!dictionary.ContainsKey(imageItem.FieldId.ToUpper()))
                    {
                        dictionary.Add(imageItem.FieldId.ToUpper(), imageItem.FilePath);
                    }
                }
            }

            // Add tank connections
            if (TankConnections != null && TankConnections.Count > 0)
            {
                foreach (SubmissionsDataFhi dataFHi in TankConnections)
                {
                    dataFHi.AddMapping(dictionary);
                }
            }

            // Add ancillary items
            if (AncillaryItems != null && AncillaryItems.Count > 0)
            {
                foreach (SubmissionsDataFhi dataFHi in AncillaryItems)
                {
                    dataFHi.AddMapping(dictionary);
                }
            }

            return dictionary;
        }
    }

    public class SubmissionsDataFhi
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public int PageNo { get; set; }
        public int RowNo { get; set; }
        public string RowSize { get; set; }
        public double RowQty { get; set; }
        public string RowCondition { get; set; }
        public string RowComments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

        public void AddMapping(Dictionary<string, string> dictionary)
        {
            string sizeItemKey = string.Format("FIELD_PG{0}_ROW{1}_SIZE", PageNo.ToString(), RowNo.ToString());
            if (!dictionary.ContainsKey(sizeItemKey))
            {
                dictionary.Add(sizeItemKey, RowSize);
            }
            string quantityItemKey = string.Format("FIELD_PG{0}_ROW{1}_QTY", PageNo.ToString(), RowNo < 10 ? "0" + RowNo.ToString() : RowNo.ToString());
            if (!dictionary.ContainsKey(quantityItemKey))
            {
                dictionary.Add(quantityItemKey, RowQty.ToString());
            }
        }
    }

    public class SubmissionsImagesFh
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public bool FixedImage { get; set; }
        public int PageNo { get; set; }
        public string FieldId { get; set; }
        public string FileDisplayName { get; set; }
        public string FilePath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string FileWWWurl { get; internal set; }

        public string SubmitNo;
        public bool IsBase64;
        public string Base64File;
        public string FileName;
    }



}
