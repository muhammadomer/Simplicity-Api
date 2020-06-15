using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersTendersQueries
    {

        public static string InsertOrdTendersTPFiles(string dataBase, OrdTendersTPFiles obj)
        {
            string returnVal = "";
            try
            {
                returnVal = "INSERT INTO un_ord_tenders_tp_files (flg_deleted, join_sequence, file_desc, file_cab_id, file_dir, " +
                                    "       file_name, flg_upload_complete, gu_id, version_no, flg_not_latest_version, " +
                                    "       created_by,date_created,last_amended_by,date_last_amended) " +
                                    "VALUES(" + Utilities.GetBooleanForDML(dataBase, obj.FlgDeleted) + ", " + obj.JoinSequence + ", " +
                                    "    '" + obj.FileDesc + "', '" + obj.FileCabId + "', " +
                                    "    '" + obj.FileDir + "', '" + obj.FileName + "', " + Utilities.GetBooleanForDML(dataBase, obj.FlgUploadComplete) + ", " +
                                    "     " + obj.GuId + ", " +
                                    "    '" + obj.VersionNo + "', " + Utilities.GetBooleanForDML(dataBase, obj.FlgNotLatestVersion) + ", " +
                                    "     " + obj.CreatedBy + "," + Utilities.GetDateTimeForDML(dataBase, obj.DateCreated, true, true) + ", " +
                                    "     " + obj.LastAmendedBy + "," + Utilities.GetDateTimeForDML(dataBase, obj.DateLastAmended, true, true) + ") ";
                
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string UpdateOrdTendersTPFlgTenderUploads(string dataBase, OrdTendersTP obj)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_ord_tenders_tp " +
                                   "   SET flg_tender_uploads = " + Utilities.GetBooleanForDML(dataBase, obj.FlgTenderUploads) + ", " +
                                   "       last_amended_by = " + obj.LastAmendedBy + ", " +
                                   "       date_last_amended = " + Utilities.GetDateTimeForDML(dataBase, obj.DateLastAmended, true, true) + " " +
                                   " WHERE sequence = " + obj.Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string UpdateOrdTendersTP(string dataBase, OrdTendersTP obj)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_ord_tenders_tp " +
                                   "   set tender_accept = " + obj.TenderAccept + ", " +
                                   "       tp_notes = '" + obj.TPNotes + "', " +
                                   "       flg_tender_finalised = " + Utilities.GetBooleanForDML(dataBase, obj.FlgTenderFinalised) + ", " +
                                   "       date_tender_finalised = " + Utilities.GetDateTimeForDML(dataBase, obj.DatTenderFinalised, true, true) + ", " +
                                   "       last_amended_by = " + obj.LastAmendedBy + ", " +
                                   "       date_last_amended = " + Utilities.GetDateTimeForDML(dataBase, obj.DateLastAmended, true, true) + " " +
                                   " WHERE sequence = " + obj.Sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string UpdateOrdTendersSpecFlgInProgress(string dataBase,long sequence)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_ord_tenders_specs " +
                                    "   set flg_tenders_in_progress = " + Utilities.GetBooleanForDML(dataBase, true) +
                                    " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string UpdateOrdTendersSpecFlgCompleted(string dataBase, long sequence)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_ord_tenders_specs " +
                                    "   set flg_tenders_completed = " + Utilities.GetBooleanForDML(dataBase, true)
                                    + " WHERE sequence=" + sequence + " and sequence not in (Select join_sequence From un_ord_tenders_tp where flg_tender_finalised=" + Utilities.GetBooleanForDML(dataBase, false) + " and join_sequence=" + sequence + ") ";
               
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string UpdateOrdTendersTPFiles(string dataBase, OrdTendersTPFiles obj)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_ord_tenders_tp_files " +
                                    "   set gu_id = " + obj.GuId + " " +
                                    "WHERE sequence = " + obj.Sequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string UpdateOrdTendersTPFilesDeletedFlag(string dataBase, long sequence, bool flgDeleted)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_ord_tenders_tp_files " +
                                   "   set flg_deleted = " + Utilities.GetBooleanForDML(dataBase, flgDeleted) + " " +
                                   "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string getOrdersTendersByEntityId(string databaseType, long entityId, bool flgFilterByStatus, int statusSequence,bool fliterOutFutureTender, bool flgFilterByFinalised, bool flgTenderFinalised)
        {
            string returnValue = "";
            returnValue = @"SELECT un_ord_tenders_tp.sequence, un_ord_tenders_tp.join_sequence,
                                             un_ord_tenders_tp.entity_id, un_ord_tenders_tp.tender_accept,
                                             un_ord_tenders_tp.tp_notes, un_ord_tenders_tp.flg_tender_finalised,
                                             un_ord_tenders_tp.date_tender_finalised, un_ord_tenders_tp.flg_tender_uploads,
                                             un_ref_ord_tender_packs.pack_sequence, un_ref_ord_tender_packs.pack_desc, 
                                             un_ref_ord_tender_status.status_sequence, un_ref_ord_tender_status.status_desc, 
                                             un_ref_ord_tender_categories.category_sequence, 
                                             un_ref_ord_tender_categories.category_desc, 
                                             un_ord_tenders_specs.me_sequence, 
                                             un_ord_tenders_specs.flg_spec_published,
                                             un_ord_tenders_specs.date_spec_published, un_ord_tenders_specs.date_spec_deadline, 
                                             un_ord_tenders_specs.flg_spec_extended, un_ord_tenders_specs.date_spec_extended, 
                                             un_orders_me_header.me_project_title, un_orders_me_header.flg_spec_show_client, 
                                             un_orders_me_header.flg_spec_show_job_address ,un_orders.job_ref
                                       FROM (((((un_ord_tenders_tp 
                                          INNER JOIN un_ord_tenders_specs ON un_ord_tenders_tp.join_sequence = un_ord_tenders_specs.sequence) 
                                          INNER JOIN un_orders_me_header  ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) 
                                          INNER JOIN un_ref_ord_tender_packs ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) 
                                          INNER JOIN un_orders ON un_ord_tenders_specs.job_sequence = un_orders.sequence)
                                          LEFT JOIN un_ref_ord_tender_status ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence)
                                          LEFT JOIN un_ref_ord_tender_categories  ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence 
                                      WHERE un_ord_tenders_tp.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                    "   AND un_ord_tenders_specs.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                    "   AND un_ord_tenders_specs.flg_spec_published = " + Utilities.GetBooleanForDML(databaseType, true) +
                                    "   AND un_ord_tenders_tp.entity_id = " + entityId;
            if (flgFilterByFinalised)
            {
                returnValue += " AND un_ord_tenders_tp.flg_tender_finalised = " + Utilities.GetBooleanForDML(databaseType, flgTenderFinalised);
            }
            if (fliterOutFutureTender)
            {
                returnValue += " AND un_ord_tenders_specs.date_spec_published <= " + Utilities.GetDateTimeForDML(databaseType, DateTime.Now, true, true);
            }
            if (flgFilterByStatus)
            {
                returnValue += " AND un_ord_tenders_specs.status_sequence = " + statusSequence;
            }  
            returnValue += " ORDER BY un_ord_tenders_specs.date_spec_deadline DESC ";
            return returnValue;
        }

        public static string getOrdersTendersByJobClientId(string databaseType, long jobClientId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT un_ord_tenders_specs.sequence, un_ord_tenders_specs.me_sequence, " +
                                      "       un_ord_tenders_specs.flg_spec_published, un_ord_tenders_specs.date_spec_published, " +
                                      "       un_ord_tenders_specs.date_spec_deadline, un_ord_tenders_specs.flg_spec_extended, " +
                                      "       un_ord_tenders_specs.spec_notes, un_ord_tenders_specs.flg_awarded, un_ord_tenders_specs.date_awarded, " +
                                      "       un_ord_tenders_specs.date_spec_extended, " + 
                                      "       un_ord_tenders_tp.tender_amount, un_ref_ord_tender_packs.pack_sequence, " +
                                      "       un_ref_ord_tender_packs.pack_desc, un_ref_ord_tender_status.status_sequence, " +
                                      "       un_ref_ord_tender_status.status_desc, un_ref_ord_tender_categories.category_sequence, " +
                                      "       un_ref_ord_tender_categories.category_desc, un_orders_me_header.me_project_title, "+
                                      "       un_orders_me_header.flg_spec_show_client, un_orders_me_header.flg_spec_show_job_address, "+
                                      "       edc_tp.name_long as awarded_tp_name_long, un_orders.job_address, un_orders.job_ref " +
                                      "       FROM ((((((un_ord_tenders_specs INNER JOIN un_orders_me_header "+
                                      "       ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) " +
                                      " INNER JOIN un_ref_ord_tender_packs " +
                                      "    ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) "+
                                      "  LEFT JOIN un_ref_ord_tender_status "+
                                      "    ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) " +
                                      "  LEFT JOIN un_ref_ord_tender_categories " +
                                      "    ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence) " +
                                      "  LEFT JOIN un_ord_tenders_tp " +
                                      "    ON un_ord_tenders_specs.awarded_tender_seq = un_ord_tenders_tp.sequence) " +
                                      "  LEFT JOIN un_entity_details_core AS edc_tp " +
                                      "    ON un_ord_tenders_tp.entity_id = edc_tp.entity_id) " +
                                      " INNER JOIN un_orders ON un_ord_tenders_specs.job_sequence = un_orders.sequence " +
                                      " WHERE un_ord_tenders_specs.flg_spec_published =  " + Utilities.GetBooleanForDML(databaseType, true)+
                                      "   AND un_ord_tenders_specs.flg_deleted <>  "+ Utilities.GetBooleanForDML(databaseType, true)+
                                      "   AND un_orders.job_client_id = " + jobClientId +
                                      "   AND un_orders_me_header.flg_client_can_view_overview = " + Utilities.GetBooleanForDML(databaseType, true) ;
                        if (flgFilterByAwarded)
                        {
                            returnValue += " AND un_ord_tenders_tp.flg_tender_finalised = " + Utilities.GetBooleanForDML(databaseType, flgTenderAwarded);
                        }
                        
                if (flgFilterByStatus)
                {
                    returnValue += " AND un_ord_tenders_specs.status_sequence = " + statusSequence;
                }
                returnValue += " ORDER BY un_ord_tenders_specs.date_spec_deadline DESC ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getOrdersTendersByViewerId(string databaseType, long jobClientId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT un_ord_tenders_specs.sequence, un_ord_tenders_specs.me_sequence, 
                                un_ord_tenders_specs.flg_spec_published, un_ord_tenders_specs.date_spec_published,
                                un_ord_tenders_specs.date_spec_deadline, un_ord_tenders_specs.flg_spec_extended,
                                un_ord_tenders_specs.spec_notes, un_ord_tenders_specs.flg_awarded, un_ord_tenders_specs.date_awarded, 
                                un_ord_tenders_specs.date_spec_extended, 
                                un_ord_tenders_tp.tender_amount, un_ref_ord_tender_packs.pack_sequence, 
                                un_ref_ord_tender_packs.pack_desc, un_ref_ord_tender_status.status_sequence, 
                                un_ref_ord_tender_status.status_desc, un_ref_ord_tender_categories.category_sequence, 
                                un_ref_ord_tender_categories.category_desc, un_orders_me_header.me_project_title, 
                                un_orders_me_header.flg_spec_show_client, un_orders_me_header.flg_spec_show_job_address, 
                                edc_tp.name_long as awarded_tp_name_long, un_orders.job_address, un_orders.job_ref 
                                FROM ((((((un_ord_tenders_specs INNER JOIN un_orders_me_header 
                                ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) 
                                INNER JOIN un_ref_ord_tender_packs 
                                ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) 
                                LEFT JOIN un_ref_ord_tender_status 
                                ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) 
                                LEFT JOIN un_ref_ord_tender_categories 
                                ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence) 
                                LEFT JOIN un_ord_tenders_tp 
                                ON un_ord_tenders_specs.awarded_tender_seq = un_ord_tenders_tp.sequence) 
                                LEFT JOIN un_entity_details_core AS edc_tp 
                                ON un_ord_tenders_tp.entity_id = edc_tp.entity_id) 
                                INNER JOIN un_orders ON un_ord_tenders_specs.job_sequence = un_orders.sequence 
                                WHERE un_ord_tenders_specs.flg_spec_published = " + Utilities.GetBooleanForDML(databaseType,true) 
                                +" AND un_ord_tenders_specs.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                                +" AND un_orders.job_client_id in (Select entity_id From un_web_viewer_assign_to Where viewer_id=" + jobClientId + ")" 
                            + " AND un_orders_me_header.flg_client_can_view_overview = " + Utilities.GetBooleanForDML(databaseType, true);
                if (flgFilterByAwarded)
                {
                    returnValue += " AND un_ord_tenders_tp.flg_tender_finalised = " + Utilities.GetBooleanForDML(databaseType, flgTenderAwarded);
                }
                   
                if (flgFilterByStatus)
                {
                    returnValue += " AND un_ord_tenders_specs.status_sequence = " + statusSequence;
                }
                returnValue += " ORDER BY un_ord_tenders_specs.date_spec_deadline DESC ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectOrdersTenderBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT un_ord_tenders_specs.sequence, un_ord_tenders_specs.me_sequence, 
                            un_ord_tenders_specs.flg_spec_published, un_ord_tenders_specs.date_spec_published, 
                            un_ord_tenders_specs.date_spec_deadline, un_ord_tenders_specs.flg_spec_extended, 
                            un_ord_tenders_specs.spec_notes, un_ord_tenders_specs.flg_awarded, un_ord_tenders_specs.date_awarded, 
                        un_ord_tenders_specs.date_spec_extended, 
                            un_ord_tenders_tp.tender_amount, un_ref_ord_tender_packs.pack_sequence, 
                            un_ref_ord_tender_packs.pack_desc, un_ref_ord_tender_status.status_sequence, 
                            un_ref_ord_tender_status.status_desc, un_ref_ord_tender_categories.category_sequence, 
                            un_ref_ord_tender_categories.category_desc, un_orders_me_header.me_project_title, 
                            un_orders_me_header.flg_spec_show_client, un_orders_me_header.flg_spec_show_job_address, 
                            edc_tp.name_long as awarded_tp_name_long, un_orders.job_address, un_orders.job_ref 
                                            
                            FROM ((((((un_ord_tenders_specs INNER JOIN un_orders_me_header 
                        ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) 
                    INNER JOIN un_ref_ord_tender_packs 
                        ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) 
                    LEFT JOIN un_ref_ord_tender_status 
                        ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) 
                    LEFT JOIN un_ref_ord_tender_categories 
                        ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence) 
                    LEFT JOIN un_ord_tenders_tp 
                        ON un_ord_tenders_specs.awarded_tender_seq = un_ord_tenders_tp.join_sequence) 
                    LEFT JOIN un_entity_details_core AS edc_tp 
                        ON un_ord_tenders_tp.entity_id = edc_tp.entity_id) 
                    INNER JOIN un_orders ON un_ord_tenders_specs.job_sequence = un_orders.sequence 
                    WHERE un_ord_tenders_specs.sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectOrdersTenderBySequence4Client(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = @"SELECT un_ord_tenders_specs.sequence, un_ord_tenders_specs.me_sequence, 
                                             un_ord_tenders_specs.flg_spec_published, un_ord_tenders_specs.date_spec_published, 
                                             un_ord_tenders_specs.date_spec_deadline, un_ord_tenders_specs.flg_spec_extended, 
                                             un_ord_tenders_specs.spec_notes, un_ord_tenders_specs.flg_awarded, un_ord_tenders_specs.date_awarded, 
                                             un_ord_tenders_specs.awarded_tender_seq,
                                             un_ord_tenders_specs.date_spec_extended, 
                                             un_ref_ord_tender_packs.pack_sequence,un_ref_ord_tender_packs.pack_desc, 
                                             un_ref_ord_tender_status.status_sequence, un_ref_ord_tender_status.status_desc,
                                              un_ref_ord_tender_categories.category_sequence, un_ref_ord_tender_categories.category_desc,
                                              un_orders_me_header.me_project_title, 
                                             un_orders_me_header.flg_spec_show_client, un_orders_me_header.flg_spec_show_job_address, 
                                             un_orders.job_address, un_orders.job_ref, un_orders.job_client_name, un_orders.job_client_ref, un_orders.job_manager
                                        FROM ((((
                                            un_ord_tenders_specs INNER JOIN un_orders_me_header 
                                                ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) 
                                            INNER JOIN un_ref_ord_tender_packs 
                                                ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) 
                                            LEFT JOIN un_ref_ord_tender_status 
                                                ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) 
                                            LEFT JOIN un_ref_ord_tender_categories 
                                                ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence) 
                                            INNER JOIN un_orders ON un_ord_tenders_specs.job_sequence = un_orders.sequence 
                                        WHERE un_ord_tenders_specs.sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderTPDetailsBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                        returnValue = @"SELECT un_ord_tenders_tp.sequence, un_ord_tenders_tp.join_sequence, 
                                             un_ord_tenders_tp.entity_id, un_ord_tenders_tp.tender_accept, 
                                             un_ord_tenders_tp.tp_notes, un_ord_tenders_tp.flg_tender_finalised, 
                                             un_ord_tenders_tp.date_tender_finalised, un_ord_tenders_tp.flg_tender_uploads, 
                                             un_ref_ord_tender_packs.pack_sequence, un_ref_ord_tender_packs.pack_desc, 
                                             un_ref_ord_tender_status.status_sequence, un_ref_ord_tender_status.status_desc, 
                                             un_ref_ord_tender_categories.category_sequence, 
                                            un_ref_ord_tender_categories.category_desc, un_ord_tenders_specs.job_sequence, 
                                             un_ord_tenders_specs.me_sequence, 
                                             un_ord_tenders_specs.flg_spec_published, 
                                             un_ord_tenders_specs.date_spec_published, un_ord_tenders_specs.date_spec_deadline, 
                                             un_ord_tenders_specs.flg_spec_extended, un_ord_tenders_specs.date_spec_extended, 
                                             un_orders_me_header.me_project_title, un_orders_me_header.flg_spec_show_client, 
                                             un_orders_me_header.flg_spec_show_job_address 
                                       FROM ((((un_ord_tenders_tp INNER JOIN un_ord_tenders_specs 
                                         ON un_ord_tenders_tp.join_sequence = un_ord_tenders_specs.sequence) 
                                      INNER JOIN un_orders_me_header 
                                         ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) 
                                      INNER JOIN un_ref_ord_tender_packs 
                                         ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) 
                                       LEFT JOIN un_ref_ord_tender_status 
                                         ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) 
                                       LEFT JOIN un_ref_ord_tender_categories 
                                         ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence 
                                      WHERE un_ord_tenders_tp.flg_deleted <> "+ Utilities.GetBooleanForDML(databaseType, true )
                                        +" AND un_ord_tenders_specs.flg_deleted <> "+  Utilities.GetBooleanForDML(databaseType, true )
                                        +" AND un_ord_tenders_specs.flg_spec_published = " + Utilities.GetBooleanForDML(databaseType, true)
                                        + " AND un_ord_tenders_tp.sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderTPDetailsBySequenceForNotifications(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = @"SELECT un_ord_tenders_tp.sequence,un_entity_details_core.name_long as contractorName , un_ord_tenders_tp.tender_accept, un_ord_tenders_tp.tp_notes
                            ,un_ord_tenders_tp.flg_tender_finalised, un_ord_tenders_tp.date_tender_finalised, un_ord_tenders_tp.flg_tender_uploads
                            , un_ref_ord_tender_packs.pack_desc, un_ref_ord_tender_status.status_desc,  un_ref_ord_tender_categories.category_desc
                            ,un_ord_tenders_specs.flg_spec_published, un_ord_tenders_specs.date_spec_published
                            ,un_ord_tenders_specs.spec_notes,un_ord_tenders_specs.flg_awarded, un_ord_tenders_specs.date_awarded
                            ,iif( un_ord_tenders_specs.flg_spec_extended = TRUE , un_ord_tenders_specs.date_spec_extended , un_ord_tenders_specs.date_spec_deadline) as closing_date
                            ,un_orders_me_header.me_project_title, un_orders_me_header.flg_spec_show_client, un_orders_me_header.flg_spec_show_job_address
                            ,un_orders.job_ref,un_orders.job_address
                        FROM ((((((
                            un_ord_tenders_tp 
                            INNER JOIN un_ord_tenders_specs ON un_ord_tenders_tp.join_sequence = un_ord_tenders_specs.sequence) 
                            INNER JOIN un_orders_me_header ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) 
                            INNER JOIN un_ref_ord_tender_packs ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) 
                            INNER JOIN un_Orders ON un_ord_tenders_specs.job_sequence = un_Orders.sequence) 
                            LEFT JOIN un_ref_ord_tender_status ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) 
                            LEFT JOIN un_entity_details_core ON un_ord_tenders_tp.entity_id = un_entity_details_core.Entity_id) 
                            LEFT JOIN un_ref_ord_tender_categories ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence
                        WHERE un_ord_tenders_tp.flg_deleted <> " + +Utilities.GetBooleanForDML(databaseType, true) 
                            +" AND un_ord_tenders_specs.flg_deleted <> " + +Utilities.GetBooleanForDML(databaseType, true) 
                            + " AND un_ord_tenders_specs.flg_spec_published = "+ +Utilities.GetBooleanForDML(databaseType, true)
                            + " AND un_ord_tenders_tp.sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
    }
        public static string selectOrderTenderTPDetailsByJoinSequence(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT un_ord_tenders_tp.sequence, un_ord_tenders_tp.join_sequence, " +
                                      "       un_ord_tenders_tp.entity_id, un_ord_tenders_tp.tender_accept, " +
                                      "       un_ord_tenders_tp.tp_notes, un_ord_tenders_tp.flg_tender_finalised, " +
                                      "       un_ord_tenders_tp.date_tender_finalised, un_ord_tenders_tp.flg_tender_uploads, " +
                                      "       un_ref_ord_tender_packs.pack_sequence, un_ref_ord_tender_packs.pack_desc, " +
                                      "       un_ref_ord_tender_status.status_sequence, un_ref_ord_tender_status.status_desc, " +
                                      "       un_ref_ord_tender_categories.category_sequence, " +
                                      "       un_ref_ord_tender_categories.category_desc, un_ord_tenders_specs.job_sequence, " +
                                      "       un_ord_tenders_specs.me_sequence, " +
                                      "       un_ord_tenders_specs.flg_spec_published, " +
                                      "       un_ord_tenders_specs.date_spec_published, un_ord_tenders_specs.date_spec_deadline, " +
                                      "       un_ord_tenders_specs.flg_spec_extended, un_ord_tenders_specs.date_spec_extended, " +
                                      "       un_orders_me_header.me_project_title, un_orders_me_header.flg_spec_show_client, " +
                                      "       un_orders_me_header.flg_spec_show_job_address " +
                                      " FROM ((((un_ord_tenders_tp INNER JOIN un_ord_tenders_specs " +
                                      "   ON un_ord_tenders_tp.join_sequence = un_ord_tenders_specs.sequence) " +
                                      "INNER JOIN un_orders_me_header " +
                                      "   ON un_ord_tenders_specs.me_sequence = un_orders_me_header.sequence) " +
                                      "INNER JOIN un_ref_ord_tender_packs " +
                                      "   ON un_ord_tenders_specs.pack_sequence = un_ref_ord_tender_packs.pack_sequence) " +
                                      " LEFT JOIN un_ref_ord_tender_status " +
                                      "   ON un_ord_tenders_specs.status_sequence = un_ref_ord_tender_status.status_sequence) " +
                                      " LEFT JOIN un_ref_ord_tender_categories " +
                                      "   ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence " +
                                      " WHERE un_ord_tenders_tp.flg_deleted <> " + +Utilities.GetBooleanForDML(databaseType, true)
                                      + "  AND un_ord_tenders_specs.flg_deleted <> " + +Utilities.GetBooleanForDML(databaseType, true)
                                      + "  AND un_ord_tenders_tp.join_sequence = " + joinSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectOrderTenderTPByJoinSequence4Client(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT un_ord_tenders_tp.sequence, un_ord_tenders_tp.join_sequence, 
                                             un_ord_tenders_tp.entity_id, un_ord_tenders_tp.tender_accept, 
                                             un_ord_tenders_tp.tp_notes, un_ord_tenders_tp.flg_tender_finalised, 
                                             un_ord_tenders_tp.date_tender_finalised, un_ord_tenders_tp.flg_tender_uploads,un_ord_tenders_tp.tender_amount
                                       FROM un_ord_tenders_tp 
                                      WHERE un_ord_tenders_tp.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true )
                                        + " AND un_ord_tenders_tp.join_sequence = " + joinSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderSpecsFileByJoinSequence(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                                     " FROM un_ord_tenders_specs_files " +
                                     "WHERE un_ord_tenders_specs_files.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true) +
                                     "  AND un_ord_tenders_specs_files.flg_upload_complete =  " + Utilities.GetBooleanForDML(databaseType, true) +
                                     "  AND un_ord_tenders_specs_files.join_sequence = " + joinSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectOrdTendersQAByTenderTPSequence(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
               
                    returnValue = "SELECT * " +
                                    " FROM un_ord_tenders_tp_qs " +
                                    "WHERE un_ord_tenders_tp_qs.flg_deleted <>  "+ Utilities.GetBooleanForDML(databaseType, true) +
                                    "  AND un_ord_tenders_tp_qs.join_sequence = " + joinSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string selectOrdTendersQAByTenderSpec(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
                        returnValue = @"SELECT * 
                                       FROM un_ord_tenders_tp_qs 
                                      WHERE un_ord_tenders_tp_qs.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true) 
                                        +" AND un_ord_tenders_tp_qs.join_sequence in (Select sequence From un_ord_tenders_tp  where join_sequence=" + joinSequence + ")";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectOrdTendersQABySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                                      " FROM un_ord_tenders_tp_qs " +
                                      "WHERE un_ord_tenders_tp_qs.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                      "  AND un_ord_tenders_tp_qs.sequence = " + sequence;
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderSpecsFileByJoinSequenceAndLatestVersion(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = @"SELECT * 
                                       FROM un_ord_tenders_specs_files 
                                      WHERE un_ord_tenders_specs_files.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                                        +" AND un_ord_tenders_specs_files.flg_not_latest_version <> " + Utilities.GetBooleanForDML(databaseType, true)
                                        +" AND un_ord_tenders_specs_files.flg_upload_complete = " + +Utilities.GetBooleanForDML(databaseType, true)
                                        + " AND un_ord_tenders_specs_files.join_sequence = " + joinSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        

        public static string selectTenderTPFileBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT * 
                                FROM un_ord_tenders_tp_files 
                                WHERE un_ord_tenders_tp_files.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                               +"  AND un_ord_tenders_tp_files.sequence = " + sequence;
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderTPFileByJoinSequence(string databaseType, long joinSequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      " FROM un_ord_tenders_tp_files " +
                                      "WHERE un_ord_tenders_tp_files.flg_deleted <> " + +Utilities.GetBooleanForDML(databaseType, true)
                                      + "  AND un_ord_tenders_tp_files.join_sequence = " + joinSequence;
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderSpecsFilesByGUId(string databaseType, long guId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                                      " FROM un_ord_tenders_specs_files " +
                                      "WHERE flg_deleted <>  "  +Utilities.GetBooleanForDML(databaseType, true)
                                     + "  AND gu_id = " + guId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectTenderTPFileByGUId(string databaseType, long guId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                                      " FROM un_ord_tenders_tp_files " +
                                      "WHERE un_ord_tenders_tp_files.flg_deleted <>  " + +Utilities.GetBooleanForDML(databaseType, true)
                                      +"  AND un_ord_tenders_tp_files.gu_id = " + guId;
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectAllRefOrderTenderStatus(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      " FROM un_ref_ord_tender_status " +
                                      " WHERE un_ref_ord_tender_status.flg_deleted <>  " + Utilities.GetBooleanForDML(databaseType, true);
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string InsertOrdTendersTPQA(string dataBase, OrdTendersTPQS obj)
        {
            string returnVal = "";
            try
            {
                
                        returnVal = "INSERT INTO un_ord_tenders_tp_qs (flg_deleted, join_sequence, tp_question, flg_answered, owner_answer, " +
                                    "       flg_public_answer, " +
                                    "       created_by,date_created,last_amended_by,date_last_amended) " +
                                    "VALUES(" + Utilities.GetBooleanForDML(dataBase, obj.FlgDeleted) + ", " + obj.JoinSequence + ", " +
                                    "    '" + obj.TPQuestion + "', " + Utilities.GetBooleanForDML(dataBase, obj.FlgAnswered) + ", " +
                                    "    '" + obj.OwnerAnswer + "', " + Utilities.GetBooleanForDML(dataBase, obj.FlgPublicAnswer) + ", " +
                                    "     " + obj.CreatedBy + "," + Utilities.GetDateTimeForDML(dataBase, obj.DateCreated,true,true) + ", " +
                                    "     " + obj.LastAmendedBy + "," + Utilities.GetDateTimeForDML(dataBase, obj.DateLastAmended,true,true) + ") ";
                        
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }
    }
}

