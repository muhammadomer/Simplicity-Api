using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL
{
    public class SiteInspectionDB : MainDB
    {
        public SiteInspectionDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<string> FindMatchingContractNos(string contactNo)
        {
            List<string> returnValue = new List<string>();

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect = new OleDbCommand(SiteInspectionQueries.SelectMatchingContactNoByContactNo(this.DatabaseType, contactNo), conn))
                {
                    using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                returnValue.Add((dr["p1_contract_no"] == null || dr["p1_contract_no"] == DBNull.Value) ? "" : dr["p1_contract_no"].ToString());
                            }
                        }
                    }
                }

            }

            return returnValue;
        }

        public List<SubmissionsDataFh> GetSubmissionsDataFhList()
        {
            List<SubmissionsDataFh> returnValue = new List<SubmissionsDataFh>();

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect = new OleDbCommand(SiteInspectionQueries.getRecordsList(this.DatabaseType), conn))
                {
                    using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                returnValue.Add(LoadSubmissionsDataFh(dr));
                            }
                            returnValue = returnValue.OrderByDescending(m => m.DateSubmit).ToList();
                        }
                    }
                }

            }

            return returnValue;
        }

        public SubmissionsDataFh GetBySequence(long sequence)
        {
            SubmissionsDataFh returnValue = null;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect = new OleDbCommand(SiteInspectionQueries.SelectAllFieldsOfSubmissionDataFhTableBySequence(this.DatabaseType, sequence), conn))
                {
                    using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            returnValue = LoadSubmissionsDataFh(dr);
                            returnValue.TankConnections = new List<SubmissionsDataFhi>();

                            for (int i = 1; i <= 12; i++)
                            {
                                returnValue.TankConnections.Add(new SubmissionsDataFhi()
                                {
                                    PageNo = 9,
                                    RowNo = i,
                                    JoinSequence = returnValue.Sequence
                                });
                            }


                            returnValue.AncillaryItems = new List<SubmissionsDataFhi>();
                            for (int i = 1; i <= 8; i++)
                            {
                                returnValue.AncillaryItems.Add(new SubmissionsDataFhi()
                                {
                                    PageNo = 11,
                                    RowNo = i,
                                    JoinSequence = returnValue.Sequence
                                });
                            }

                            returnValue.SiteInspectionImages = new List<SubmissionsImagesFh>();
                        }
                    }
                }

                if (returnValue != null)
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(SiteInspectionQueries.SelectAllFieldsOfSubmissionDataFhiTableByJoinSequence(this.DatabaseType, returnValue.Sequence ?? 0), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                SubmissionsDataFhi submissionsDataFhi = null;
                                SubmissionsDataFhi submissionsDataFhi1 = null;
                                while (dr.Read())
                                {
                                    submissionsDataFhi = LoadSubmissionsDataFhi(dr);
                                    if (submissionsDataFhi != null)
                                    {
                                        if (submissionsDataFhi.PageNo == 9)
                                        {
                                            submissionsDataFhi1 = returnValue.TankConnections.First(x => x.RowNo == submissionsDataFhi.RowNo);
                                            submissionsDataFhi1.Sequence = submissionsDataFhi.Sequence;
                                            submissionsDataFhi1.CreatedBy = submissionsDataFhi.CreatedBy;
                                            submissionsDataFhi1.DateCreated = submissionsDataFhi.DateCreated;
                                            submissionsDataFhi1.DateLastAmended = submissionsDataFhi.DateLastAmended;
                                            submissionsDataFhi1.JoinSequence = submissionsDataFhi.JoinSequence;
                                            submissionsDataFhi1.LastAmendedBy = submissionsDataFhi.LastAmendedBy;
                                            submissionsDataFhi1.PageNo = submissionsDataFhi.PageNo;
                                            submissionsDataFhi1.RowComments = submissionsDataFhi.RowComments;
                                            submissionsDataFhi1.RowCondition = submissionsDataFhi.RowCondition;
                                            submissionsDataFhi1.RowNo = submissionsDataFhi.RowNo;
                                            submissionsDataFhi1.RowQty = submissionsDataFhi.RowQty;
                                            submissionsDataFhi1.RowSize = submissionsDataFhi.RowSize;
                                        }
                                        else if (submissionsDataFhi.PageNo == 11)
                                        {
                                            submissionsDataFhi1 = returnValue.AncillaryItems.First(x => x.RowNo == submissionsDataFhi.RowNo);
                                            submissionsDataFhi1.Sequence = submissionsDataFhi.Sequence;
                                            submissionsDataFhi1.CreatedBy = submissionsDataFhi.CreatedBy;
                                            submissionsDataFhi1.DateCreated = submissionsDataFhi.DateCreated;
                                            submissionsDataFhi1.DateLastAmended = submissionsDataFhi.DateLastAmended;
                                            submissionsDataFhi1.JoinSequence = submissionsDataFhi.JoinSequence;
                                            submissionsDataFhi1.LastAmendedBy = submissionsDataFhi.LastAmendedBy;
                                            submissionsDataFhi1.PageNo = submissionsDataFhi.PageNo;
                                            submissionsDataFhi1.RowComments = submissionsDataFhi.RowComments;
                                            submissionsDataFhi1.RowCondition = submissionsDataFhi.RowCondition;
                                            submissionsDataFhi1.RowNo = submissionsDataFhi.RowNo;
                                            submissionsDataFhi1.RowQty = submissionsDataFhi.RowQty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    using (OleDbCommand objCmdSelect = new OleDbCommand(SiteInspectionQueries.SelectAllFieldsOfSubmissionImagesFhTableByJoinSequence(this.DatabaseType, returnValue.Sequence ?? 0), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                SubmissionsImagesFh submissionsImagesFh = null;
                                while (dr.Read())
                                {
                                    submissionsImagesFh = LoadSubmissionImageFh(dr);
                                    if (submissionsImagesFh != null)
                                    {
                                        returnValue.SiteInspectionImages.Add(submissionsImagesFh);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return returnValue;
        }

        internal bool Update(SubmissionsDataFh siteInspection)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.UpdateSubmissionsDataFh(this.DatabaseType, siteInspection), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();

                    foreach (var item in siteInspection.AncillaryItems)
                    {
                        objCmdUpdate.CommandText = string.Empty;
                        if (item.Sequence > 0)
                        {
                            objCmdUpdate.CommandText = SiteInspectionQueries.UpdateSubmissionsDataFhi(this.DatabaseType, item);
                        }
                        else if (String.IsNullOrWhiteSpace(item.RowSize) == false ||
                                String.IsNullOrWhiteSpace(item.RowComments) == false ||
                                String.IsNullOrWhiteSpace(item.RowCondition) == false ||
                                item.RowQty > 0
                                )
                        {
                            objCmdUpdate.CommandText = SiteInspectionQueries.InsertSubmissionsDataFhi(this.DatabaseType, item);
                        }


                        if (String.IsNullOrWhiteSpace(objCmdUpdate.CommandText) == false)
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                    foreach (var item in siteInspection.TankConnections)
                    {
                        objCmdUpdate.CommandText = string.Empty;
                        if (item.Sequence > 0)
                        {
                            objCmdUpdate.CommandText = SiteInspectionQueries.UpdateSubmissionsDataFhi(this.DatabaseType, item);
                        }
                        else if (String.IsNullOrWhiteSpace(item.RowSize) == false ||
                                String.IsNullOrWhiteSpace(item.RowComments) == false ||
                                String.IsNullOrWhiteSpace(item.RowCondition) == false ||
                                item.RowQty > 0
                                )
                        {
                            objCmdUpdate.CommandText = SiteInspectionQueries.InsertSubmissionsDataFhi(this.DatabaseType, item);
                        }
                        if (String.IsNullOrWhiteSpace(objCmdUpdate.CommandText) == false)
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                returnValue = true;
            }
            return returnValue;
        }

        internal bool Insert(S4BSubmissionsData2 submissionData)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmd =
                    new OleDbCommand(SiteInspectionQueries.InsertSubmissionsData2(this.DatabaseType, submissionData), conn))
                {
                    objCmd.ExecuteNonQuery();
                    long JoinSequence = Utilities.GetDBAutoNumber(conn);
                }
                returnValue = true;
            }
            return returnValue;
        }

        internal bool Insert(SubmissionsDataFh siteInspection)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.InsertSubmissionsDataFh(this.DatabaseType, siteInspection), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                    long JoinSequence = Utilities.GetDBAutoNumber(conn);
                    if (siteInspection.AncillaryItems != null)
                    {
                        foreach (var item in siteInspection.AncillaryItems)
                        {
                            item.JoinSequence = JoinSequence;
                            objCmdUpdate.CommandText = SiteInspectionQueries.InsertSubmissionsDataFhi(this.DatabaseType, item);
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                    if (siteInspection.TankConnections != null)
                    {
                        foreach (var item in siteInspection.TankConnections)
                        {
                            item.JoinSequence = JoinSequence;
                            objCmdUpdate.CommandText = SiteInspectionQueries.InsertSubmissionsDataFhi(this.DatabaseType, item);
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                    if (siteInspection.SiteInspectionImages != null)
                    {
                        foreach (var item in siteInspection.SiteInspectionImages)
                        {
                            item.JoinSequence = JoinSequence;
                            objCmdUpdate.CommandText = SiteInspectionQueries.InsertSubmissionsImagesFH(this.DatabaseType, item);
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                returnValue = true;
            }
            return returnValue;
        }

        internal bool InsertSubmissionImages(SubmissionsImagesFh siteInspection)
        {
            bool returnValue = false;
            
            using (OleDbConnection conn = this.getDbConnection())
            {
                siteInspection.FixedImage = true;
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.InsertSubmissionsImagesFH(this.DatabaseType, siteInspection), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                    
                }
                returnValue = true;
            }
            return returnValue;
        }

        internal bool UpdateSubmissionImages(SubmissionsImagesFh siteInspection)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.UpdateSubmissionsImagesFH(this.DatabaseType, siteInspection), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();

                }
                returnValue = true;
            }
            return returnValue;
        }
        internal bool UpdateFileCabIdAndPDFCount(SubmissionsDataFh submissionDataFh)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(SiteInspectionQueries.UpdateFileCabIdAndPdfCountForSubmissionsDataFh(this.DatabaseType, submissionDataFh), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }
            return returnValue;
        }

        SubmissionsDataFh LoadSubmissionsDataFh(OleDbDataReader dr)
        {
            SubmissionsDataFh siteInspetion = null;

            if (dr != null)
            {
                int i;
                int l;
                DateTime? dt;
                siteInspetion = new SubmissionsDataFh();
                siteInspetion.Sequence = long.Parse(dr["sequence"].ToString());
                siteInspetion.P1ContractNo = (dr["p1_contract_no"] == null || dr["p1_contract_no"] == DBNull.Value) ? "" : dr["p1_contract_no"].ToString();
                siteInspetion.SubmitNo = (dr["submit_no"] == null || dr["submit_no"] == DBNull.Value) ? "" : dr["submit_no"].ToString();
                siteInspetion.SubmitTs = (dr["submit_ts"] == null || dr["submit_ts"] == DBNull.Value) ? "" : dr["submit_ts"].ToString();
                siteInspetion.DateSubmit = Utilities.getDBDate(dr["date_submit"]);
                siteInspetion.FileCabId = (dr["file_cab_id"] == null || dr["file_cab_id"] == DBNull.Value) ? "" : dr["file_cab_id"].ToString();
                siteInspetion.CreatedPDFCount = (dr["created_pdf_count"] == null || dr["created_pdf_count"] == DBNull.Value) ? -1 : Int32.TryParse(dr["created_pdf_count"].ToString(), out l) ? l : -1;
                siteInspetion.P1SiteAddress = (dr["p1_site_address"] == null || dr["p1_site_address"] == DBNull.Value) ? "" : dr["p1_site_address"].ToString();
                siteInspetion.P1ClientName = (dr["p1_client_name"] == null || dr["p1_client_name"] == DBNull.Value) ? "" : dr["p1_client_name"].ToString();
                siteInspetion.P1TankType = (dr["p1_tank_type"] == null || dr["p1_tank_type"] == DBNull.Value) ? "" : dr["p1_tank_type"].ToString();
                siteInspetion.P1TankSize = (dr["p1_tank_size"] == null || dr["p1_tank_size"] == DBNull.Value) ? "" : dr["p1_tank_size"].ToString();
                siteInspetion.P1ReportDate = Utilities.getDBDate(dr["p1_report_date"]);
                siteInspetion.P1InspectedBy = (dr["p1_inspected_by"] == null || dr["p1_inspected_by"] == DBNull.Value) ? "" : dr["p1_inspected_by"].ToString();
                siteInspetion.P2Location = (dr["p2_location"] == null || dr["p2_location"] == DBNull.Value) ? "" : dr["p2_location"].ToString();
                siteInspetion.P2InstallationDate = Utilities.getDBDate(dr["p2_installation_date"]);
                siteInspetion.P2InstallationDateUnknown = (dr["p2_installation_date_unkown"] == null || dr["p2_installation_date_unkown"] == DBNull.Value) ? "" : dr["p2_installation_date_unkown"].ToString();
                siteInspetion.P2ContactName = (dr["p2_contact_name"] == null || dr["p2_contact_name"] == DBNull.Value) ? "" : dr["p2_contact_name"].ToString();
                siteInspetion.P2ContactTelNo = (dr["p2_contact_tel_no"] == null || dr["p2_contact_tel_no"] == DBNull.Value) ? "" : dr["p2_contact_tel_no"].ToString();
                siteInspetion.P2ContacText = (dr["p2_contact_ext"] == null || dr["p2_contact_ext"] == DBNull.Value) ? "" : dr["p2_contact_ext"].ToString();
                siteInspetion.P2ContactMobile = (dr["p2_contact_mobile"] == null || dr["p2_contact_mobile"] == DBNull.Value) ? "" : dr["p2_contact_mobile"].ToString();
                siteInspetion.P2LContacts = (dr["p2_lcontacts"] == null || dr["p2_lcontacts"] == DBNull.Value) ? "" : dr["p2_lcontacts"].ToString();
                siteInspetion.P2VisitStatus = (dr["p2_visit_status"] == null || dr["p2_visit_status"] == DBNull.Value) ? "" : dr["p2_visit_status"].ToString();
                siteInspetion.P2ManufacturerDetails = (dr["p2_manufacturer_details"] == null || dr["p2_manufacturer_details"] == DBNull.Value) ? "" : dr["p2_manufacturer_details"].ToString();
                siteInspetion.P2VisitPurpose = (dr["p2_visit_purpose"] == null || dr["p2_visit_purpose"] == DBNull.Value) ? "" : dr["p2_visit_purpose"].ToString();
                siteInspetion.P2VisitPurpose2 = (dr["p2_visit_purpose2"] == null || dr["p2_visit_purpose2"] == DBNull.Value) ? "" : dr["p2_visit_purpose2"].ToString();
                siteInspetion.P3TankStatus = (dr["p3_tank_status"] == null || dr["p3_tank_status"] == DBNull.Value) ? "" : dr["p3_tank_status"].ToString();
                siteInspetion.P3TankDiameter = (dr["p3_tank_diameter"] == null || dr["p3_tank_diameter"] == DBNull.Value) ? "" : dr["p3_tank_diameter"].ToString();
                siteInspetion.P3TankShape = (dr["p3_tank_shape"] == null || dr["p3_tank_shape"] == DBNull.Value) ? "" : dr["p3_tank_shape"].ToString();
                siteInspetion.P3Tanksize = (dr["p3_tank_size"] == null || dr["p3_tank_size"] == DBNull.Value) ? "" : dr["p3_tank_size"].ToString();
                siteInspetion.P3TankManufacturer = (dr["p3_tank_manufacturer"] == null || dr["p3_tank_manufacturer"] == DBNull.Value) ? "" : dr["p3_tank_manufacturer"].ToString();
                siteInspetion.P3PanelDimensions = (dr["p3_panel_dimensions"] == null || dr["p3_panel_dimensions"] == DBNull.Value) ? "" : dr["p3_panel_dimensions"].ToString();
                siteInspetion.P3TankType = (dr["p3_tank_type"] == null || dr["p3_tank_type"] == DBNull.Value) ? "" : dr["p3_tank_type"].ToString();
                siteInspetion.P3ToppanelDimensions = (dr["p3_top_panel_dimensions"] == null || dr["p3_top_panel_dimensions"] == DBNull.Value) ? "" : dr["p3_top_panel_dimensions"].ToString();
                siteInspetion.P3TankInstallationDate = Utilities.getDBDate(dr["p3_tank_installation_date"]);
                siteInspetion.P3Hozboltseam = (dr["p3_hoz_bolt_seam"] == null || dr["p3_hoz_bolt_seam"] == DBNull.Value) ? "" : dr["p3_hoz_bolt_seam"].ToString();
                siteInspetion.P3TankHeight = (dr["p3_tank_height"] == null || dr["p3_tank_height"] == DBNull.Value) ? "" : dr["p3_tank_height"].ToString();
                siteInspetion.P3ActualCapacity = (dr["p3_actual_capacity"] == null || dr["p3_actual_capacity"] == DBNull.Value) ? "" : dr["p3_actual_capacity"].ToString();
                siteInspetion.P3TankDetails = (dr["p3_tank_details"] == null || dr["p3_tank_details"] == DBNull.Value) ? "" : dr["p3_tank_details"].ToString();
                siteInspetion.P3TankShellDetails = (dr["p3_tank_shell_details"] == null || dr["p3_tank_shell_details"] == DBNull.Value) ? "" : dr["p3_tank_shell_details"].ToString();
                siteInspetion.P3TankShellDetails2 = (dr["p3_tank_shell_details2"] == null || dr["p3_tank_shell_details2"] == DBNull.Value) ? "" : dr["p3_tank_shell_details2"].ToString();
                siteInspetion.P4RoofTankShellDetails = (dr["p4_roof_tank_shell_details"] == null || dr["p4_roof_tank_shell_details"] == DBNull.Value) ? "" : dr["p4_roof_tank_shell_details"].ToString();
                siteInspetion.P4TestReturn = (dr["p4_test_return"] == null || dr["p4_test_return"] == DBNull.Value) ? "" : dr["p4_test_return"].ToString();
                siteInspetion.P4DrainValve = (dr["p4_drain_valve"] == null || dr["p4_drain_valve"] == DBNull.Value) ? "" : dr["p4_drain_valve"].ToString();
                siteInspetion.P4Suction = (dr["p4_suction"] == null || dr["p4_suction"] == DBNull.Value) ? "" : dr["p4_suction"].ToString();
                siteInspetion.P4OverFlows = (dr["p4_overflows"] == null || dr["p4_overflows"] == DBNull.Value) ? "" : dr["p4_overflows"].ToString();
                siteInspetion.P4InletValve = (dr["p4_inlet_valve"] == null || dr["p4_inlet_valve"] == DBNull.Value) ? "" : dr["p4_inlet_valve"].ToString();
                siteInspetion.P4ImmersionHeater = (dr["p4_immersion_heater"] == null || dr["p4_immersion_heater"] == DBNull.Value) ? "" : dr["p4_immersion_heater"].ToString();
                siteInspetion.P4LowLevelManWay = (dr["p4_low_level_manway"] == null || dr["p4_low_level_manway"] == DBNull.Value) ? "" : dr["p4_low_level_manway"].ToString();
                siteInspetion.P6AncillaryItems = (dr["p6_ancillary_items"] == null || dr["p6_ancillary_items"] == DBNull.Value) ? "" : dr["p6_ancillary_items"].ToString();
                siteInspetion.P6ExternalLadder = (dr["p6_external_ladder"] == null || dr["p6_external_ladder"] == DBNull.Value) ? "" : dr["p6_external_ladder"].ToString();
                siteInspetion.P6InletValveHousing = (dr["p6_inlet_valve_housing"] == null || dr["p6_inlet_valve_housing"] == DBNull.Value) ? "" : dr["p6_inlet_valve_housing"].ToString();
                siteInspetion.P7Observations01 = (dr["p7_observations01"] == null || dr["p7_observations01"] == DBNull.Value) ? "" : dr["p7_observations01"].ToString();
                siteInspetion.P7Observations02 = (dr["p7_observations02"] == null || dr["p7_observations02"] == DBNull.Value) ? "" : dr["p7_observations02"].ToString();
                siteInspetion.P7Observations03 = (dr["p7_observations03"] == null || dr["p7_observations03"] == DBNull.Value) ? "" : dr["p7_observations03"].ToString();
                siteInspetion.P7Observations04 = (dr["p7_observations04"] == null || dr["p7_observations04"] == DBNull.Value) ? "" : dr["p7_observations04"].ToString();
                siteInspetion.P7Observations05 = (dr["p7_observations05"] == null || dr["p7_observations05"] == DBNull.Value) ? "" : dr["p7_observations05"].ToString();
                siteInspetion.P7Observations06 = (dr["p7_observations06"] == null || dr["p7_observations06"] == DBNull.Value) ? "" : dr["p7_observations06"].ToString();
                siteInspetion.P7Observations07 = (dr["p7_observations07"] == null || dr["p7_observations07"] == DBNull.Value) ? "" : dr["p7_observations07"].ToString();
                siteInspetion.P7Observations08 = (dr["p7_observations08"] == null || dr["p7_observations08"] == DBNull.Value) ? "" : dr["p7_observations08"].ToString();
                siteInspetion.P7Observations09 = (dr["p7_observations09"] == null || dr["p7_observations09"] == DBNull.Value) ? "" : dr["p7_observations09"].ToString();
                siteInspetion.P7Observations10 = (dr["p7_observations10"] == null || dr["p7_observations10"] == DBNull.Value) ? "" : dr["p7_observations10"].ToString();
                siteInspetion.P7Conclusions = (dr["p7_conclusions"] == null || dr["p7_conclusions"] == DBNull.Value) ? "" : dr["p7_conclusions"].ToString();
                siteInspetion.P7SignatureImageFile = (dr["p7_signature_image_file"] == null || dr["p7_signature_image_file"] == DBNull.Value) ? "" : dr["p7_signature_image_file"].ToString();
                siteInspetion.P8ContentsGauge = (dr["p8_contents_gauge"] == null || dr["p8_contents_gauge"] == DBNull.Value) ? "" : dr["p8_contents_gauge"].ToString();
                siteInspetion.P8LevelSwitches = (dr["p8_level_switches"] == null || dr["p8_level_switches"] == DBNull.Value) ? "" : dr["p8_level_switches"].ToString();
                siteInspetion.CreatedBy = (dr["created_by"] == null || dr["created_by"] == DBNull.Value) ? -1 : int.TryParse(dr["created_by"].ToString(), out i) ? i : -1;
                siteInspetion.DateCreated = Utilities.getDBDate(dr["date_created"]);
                siteInspetion.LastAmendedBy = (dr["last_amended_by"] == null || dr["last_amended_by"] == DBNull.Value) ? -1 : int.TryParse(dr["last_amended_by"].ToString(), out i) ? i : -1;
                siteInspetion.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);

            }

            return siteInspetion;
        }

        SubmissionsDataFhi LoadSubmissionsDataFhi(OleDbDataReader dr)
        {
            SubmissionsDataFhi submissionsDataFhi = null;

            if (dr != null)
            {
                int i;
                long l;
                DateTime? dt;
                submissionsDataFhi = new SubmissionsDataFhi();

                submissionsDataFhi.Sequence = long.Parse(dr["sequence"].ToString());
                submissionsDataFhi.JoinSequence = (dr["join_sequence"] == null || dr["join_sequence"] == DBNull.Value) ? -1 : long.TryParse(dr["join_sequence"].ToString(), out l) ? l : -1;
                submissionsDataFhi.PageNo = (dr["page_no"] == null || dr["page_no"] == DBNull.Value) ? -1 : int.TryParse(dr["page_no"].ToString(), out i) ? i : -1;
                submissionsDataFhi.RowNo = (dr["row_no"] == null || dr["row_no"] == DBNull.Value) ? -1 : int.TryParse(dr["row_no"].ToString(), out i) ? i : -1;
                submissionsDataFhi.RowSize = (dr["row_size"] == null || dr["row_size"] == DBNull.Value) ? "" : dr["row_size"].ToString();
                submissionsDataFhi.RowQty = (dr["row_qty"] == null || dr["row_qty"] == DBNull.Value) ? -1 : int.TryParse(dr["row_qty"].ToString(), out i) ? i : -1;
                submissionsDataFhi.RowCondition = (dr["row_condition"] == null || dr["row_condition"] == DBNull.Value) ? "" : dr["row_condition"].ToString();
                submissionsDataFhi.RowComments = (dr["row_comments"] == null || dr["row_comments"] == DBNull.Value) ? "" : dr["row_comments"].ToString();
                submissionsDataFhi.CreatedBy = (dr["created_by"] == null || dr["created_by"] == DBNull.Value) ? -1 : int.TryParse(dr["created_by"].ToString(), out i) ? i : -1;
                submissionsDataFhi.DateCreated = Utilities.getDBDate(dr["date_created"]);
                submissionsDataFhi.LastAmendedBy = (dr["last_amended_by"] == null || dr["last_amended_by"] == DBNull.Value) ? -1 : int.TryParse(dr["last_amended_by"].ToString(), out i) ? i : -1;
                submissionsDataFhi.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
            }
            return submissionsDataFhi;
        }

        SubmissionsImagesFh LoadSubmissionImageFh(OleDbDataReader dr)
        {
            SubmissionsImagesFh submissionsImageFh = null;
            if (dr != null)
            {
                int i;
                long l;
                submissionsImageFh = new SubmissionsImagesFh();
                submissionsImageFh.Sequence = long.Parse(dr["sequence"].ToString());
                submissionsImageFh.JoinSequence = (dr["join_sequence"] == null || dr["join_sequence"] == DBNull.Value) ? -1 : long.TryParse(dr["join_sequence"].ToString(), out l) ? l : -1;
                submissionsImageFh.FixedImage = bool.Parse(dr["flg_fixed_image"].ToString());
                submissionsImageFh.PageNo = (dr["page_no"] == null || dr["page_no"] == DBNull.Value) ? -1 : int.TryParse(dr["page_no"].ToString(), out i) ? i : -1;
                submissionsImageFh.FieldId = Utilities.GetDBString(dr["field_id"]);
                submissionsImageFh.FileDisplayName = Utilities.GetDBString(dr["file_display_name"]);
                submissionsImageFh.FilePath = Utilities.GetDBString(dr["file_path"]);
                submissionsImageFh.CreatedBy = (dr["created_by"] == null || dr["created_by"] == DBNull.Value) ? -1 : int.TryParse(dr["created_by"].ToString(), out i) ? i : -1;
                submissionsImageFh.DateCreated = Utilities.getDBDate(dr["date_created"]);
                submissionsImageFh.LastAmendedBy = (dr["last_amended_by"] == null || dr["last_amended_by"] == DBNull.Value) ? -1 : int.TryParse(dr["last_amended_by"].ToString(), out i) ? i : -1;
                submissionsImageFh.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
            }
            return submissionsImageFh;
        }
    }
}
