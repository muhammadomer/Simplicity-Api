using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL

{
    public class OrdersTendersDB: MainDB
    {
        public OrdersTendersDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

       public  List<OrdTendersTP> selectAllOrdersTendersByEntityId(long entityId, bool flgFilterByStatus, int statusSequence,bool fliterOutFutureTender, bool flgFilterByFinalised, bool flgTenderFinalised)
        {
            List<OrdTendersTP> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.getOrdersTendersByEntityId(this.DatabaseType, entityId, flgFilterByStatus, statusSequence, fliterOutFutureTender, flgFilterByFinalised, flgTenderFinalised), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTP>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersForTP(dr));
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

        public List<OrdTendersSpecs> selectAllOrdersTendersByJobClientId(long jobClientId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            List<OrdTendersSpecs> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.getOrdersTendersByJobClientId(this.DatabaseType, jobClientId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersSpecs>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersDetails(dr));
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

        public List<OrdTendersSpecs> selectAllOrdersTendersByViewerId(long ViewerId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            List<OrdTendersSpecs> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.getOrdersTendersByViewerId(this.DatabaseType, ViewerId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersSpecs>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersDetails(dr));
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

        public List<OrdTendersSpecsFiles> selectAllOrdersTendersSpecsFilesByTenderSpecSequence(long joinSequence)
        {
            List<OrdTendersSpecsFiles> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderSpecsFileByJoinSequence(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersSpecsFiles>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersSpecsFiles(dr));
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

        public List<OrdTendersTPQS> selectOrdTendersQAByTenderTPSequence(long joinSequence)
        {
            List<OrdTendersTPQS> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrdTendersQAByTenderTPSequence(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTPQS>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersTPQS(dr));
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

        public List<OrdTendersTPQS> selectOrdTendersQAByTenderSpec(long sequence)
        {
            List<OrdTendersTPQS> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrdTendersQAByTenderSpec(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTPQS>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersTPQS(dr));
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
        public OrdTendersTPQS selectOrdTendersQABySequence(long sequence)
        {
            OrdTendersTPQS returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrdTendersQABySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrdersTendersTPQS(dr);
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

        public List<OrdTendersSpecsFiles> selectAllOrdersTendersSpecsFilesByTenderSpecSequenceAndLatestVersion(long joinSequence)
        {
            List<OrdTendersSpecsFiles> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderSpecsFileByJoinSequenceAndLatestVersion(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersSpecsFiles>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersSpecsFiles(dr));
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

        public List<OrdTendersTPFiles> selectAllOrdersTendersTPFilesByGUId(long guId)
        {
            List<OrdTendersTPFiles> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderTPFileByGUId(this.DatabaseType, guId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTPFiles>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersTPFiles(dr));
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

        public List<OrdTendersSpecsFiles> selectAllOrdersTendersSpecsFilesByGUId(long guId)
        {
            List<OrdTendersSpecsFiles> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderSpecsFilesByGUId(this.DatabaseType, guId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersSpecsFiles>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersSpecsFiles(dr));
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

        public List<OrdTendersTPFiles> selectAllOrdersTendersTPFilesByTenderTPSequence(long joinSequence)
        {
            List<OrdTendersTPFiles> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderTPFileByJoinSequence(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTPFiles>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrdersTendersTPFiles(dr));
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

        public OrdTendersTPFiles selectTendersTPFilesByFileSequence(long sequence)
        {
            OrdTendersTPFiles returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderTPFileBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new OrdTendersTPFiles();
                                while (dr.Read())
                                {
                                    returnValue = LoadOrdersTendersTPFiles(dr);
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

        public List<RefOrdTenderStatus> selectAllRefOrderTenderStatus()
        {
            List<RefOrdTenderStatus> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectAllRefOrderTenderStatus(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefOrdTenderStatus>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefOrderTenderStatus(dr));
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

        internal OrdTendersTPQS InsertOrdTendersTPQA(OrdTendersTPQS ordTendersTPQA)
        {
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(OrdersTendersQueries.InsertOrdTendersTPQA(DatabaseType, ordTendersTPQA), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            ordTendersTPQA.Sequence = Utilities.GetDBAutoNumber(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ordTendersTPQA;
        }

        public List<OrdTendersTP> selectTenderTPDetailsBySequence(long sequence, bool loadAll)
        {
            List<OrdTendersTP> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderTPDetailsBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTP>();
                                while (dr.Read())
                                {
                                    OrdTendersTP ordTendersTP = LoadOrdersTendersForTP(dr);
                                    if (ordTendersTP != null)
                                    {
                                        if (loadAll)
                                        {
                                            if (ordTendersTP.OrdersTendersSpecs != null)
                                            {
                                                ordTendersTP.OrdersTendersSpecs.OrdTendersSpecsFiles = selectAllOrdersTendersSpecsFilesByTenderSpecSequenceAndLatestVersion(ordTendersTP.OrdersTendersSpecs.Sequence ?? 0);
                                                DatabaseInfo dbInfo = new DatabaseInfo();
                                                dbInfo.DatabaseType = this.DatabaseType;
                                                dbInfo.ConnectionString = this.connectionString;
                                                ordTendersTP.OrdersTendersSpecs.OrderMeHeader = new OrdersMeHeaderDB(dbInfo).selectAllOrdersMeHeaderSequence(ordTendersTP.OrdersTendersSpecs.MeSequence ?? 0, true);
                                            }
                                            ordTendersTP.OrdTendersTPFiles = selectAllOrdersTendersTPFilesByTenderTPSequence(ordTendersTP.Sequence ?? 0);
                                            ordTendersTP.OrdTendersQAs = selectOrdTendersQAByTenderTPSequence(ordTendersTP.Sequence ?? 0);
                                        }
                                        returnValue.Add(ordTendersTP);
                                    }
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

        public OrdTendersTPNotifications selectTenderTPDetailsBySequenceForNotifications(long sequence)
        {
            OrdTendersTPNotifications returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectTenderTPDetailsBySequenceForNotifications(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new OrdTendersTPNotifications();
                                while (dr.Read())
                                {
                                    returnValue = LoadOrdersTendersTPForNotificatons(dr);
                                    
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

        public List<OrdTendersTP> selectTenderTPDetailsByJoinSequence(long joinSequence)
        {
            List<OrdTendersTP> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrderTenderTPDetailsByJoinSequence(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTP>();
                                while (dr.Read())
                                {
                                    OrdTendersTP ordTendersTP = LoadOrdersTendersForTP(dr);
                                    if (ordTendersTP != null)
                                    {
                                        
                                        returnValue.Add(ordTendersTP);
                                    }
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

        public List<OrdTendersTP> selectTenderTPDetailsByJoinSequence4Client(long joinSequence)
        {
            List<OrdTendersTP> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrderTenderTPByJoinSequence4Client(this.DatabaseType, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdTendersTP>();
                                while (dr.Read())
                                {
                                    OrdTendersTP ordTendersTP = LoadOrdersTendersForClient(dr);
                                    if (ordTendersTP != null)
                                    {
                                        
                                        
                                        returnValue.Add(ordTendersTP);
                                    }
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

        public OrdTendersSpecs selectOrderTenderDetailsBySequence(long sequence, bool loadAll)
        {
            OrdTendersSpecs returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrdersTenderBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrdersTendersDetails(dr);
                                if (returnValue != null)
                                {
                                    if (loadAll)
                                    {
                                        if (returnValue != null)
                                        {
                                            //---Load orders tenders with questions
                                            returnValue.OrdTenders = selectTenderTPDetailsByJoinSequence(returnValue.Sequence ?? 0);
                                        }
                                    }
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

        public OrdTendersSpecsClient selectOrderTenderDetailsBySequence4Client(long sequence, bool loadAll)
        {
            OrdTendersSpecsClient returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersTendersQueries.selectOrdersTenderBySequence4Client(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrdersTendersDetails4Client(dr);
                                if (returnValue != null)
                                {
                                    if (loadAll)
                                    {
                                        //---Load Tender Spec Files
                                        if (returnValue.Sequence>0)
                                        {
                                            returnValue.OrdTendersSpecsFiles = selectAllOrdersTendersSpecsFilesByTenderSpecSequenceAndLatestVersion(returnValue.Sequence ?? 0);
                                        }
                                        //---Load Tender Questions Answers
                                        returnValue.OrdTenderSpecQAS = selectOrdTendersQAByTenderSpec(sequence);
                                        if (returnValue != null)
                                        {
                                            //---Load orders tenders
                                            returnValue.OrdTenders = selectTenderTPDetailsByJoinSequence4Client(returnValue.Sequence ?? 0);
                                        }
                                    }
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

        internal bool UpdateOrdTendersTP(OrdTendersTP ordTendersTP)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(OrdersTendersQueries.UpdateOrdTendersTP(DatabaseType, ordTendersTP), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            if (ordTendersTP.TenderAccept == 1) {
                                OleDbCommand cmdObj2 = new OleDbCommand(OrdersTendersQueries.UpdateOrdTendersSpecFlgInProgress(DatabaseType, ordTendersTP.JoinSequence ?? 0),connection);
                                cmdObj2.ExecuteNonQuery();
                            }
                            if (ordTendersTP.FlgTenderFinalised == true)
                            {
                                OleDbCommand cmdObj3 = new OleDbCommand(OrdersTendersQueries.UpdateOrdTendersSpecFlgCompleted(DatabaseType, ordTendersTP.JoinSequence ?? 0), connection);
                                cmdObj3.ExecuteNonQuery();
                            }
                            returnValue = true;
                            //ordTendersTPFiles.Sequence = Utilities.GetDBAutoNumber(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool UpdateOrdTendersTPFlgTenderUpload(OrdTendersTP ordTendersTP)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(OrdersTendersQueries.UpdateOrdTendersTPFlgTenderUploads(DatabaseType, ordTendersTP), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool UpdateOrdTendersTPFileDeletedFlag(long sequence, bool flgDeleted)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(OrdersTendersQueries.UpdateOrdTendersTPFilesDeletedFlag(DatabaseType, sequence, flgDeleted), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            returnValue = true;
                            //ordTendersTPFiles.Sequence = Utilities.GetDBAutoNumber(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public OrdTendersTPFiles InsertOrdTendersTPFiles(OrdTendersTPFiles ordTendersTPFiles)
        {
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(OrdersTendersQueries.InsertOrdTendersTPFiles(DatabaseType, ordTendersTPFiles), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            ordTendersTPFiles.Sequence = Utilities.GetDBAutoNumber(connection);
                            if (ordTendersTPFiles.GuId <= 0)
                            {
                                ordTendersTPFiles.GuId = ordTendersTPFiles.Sequence;
                                using (OleDbCommand cmdObj1 = new OleDbCommand(OrdersTendersQueries.UpdateOrdTendersTPFiles(DatabaseType, ordTendersTPFiles), connection))
                                {
                                    cmdObj1.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ordTendersTPFiles;
        }

        private OrdTendersSpecs LoadOrdersTendersDetails(OleDbDataReader dr)
        {
            OrdTendersSpecs returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersSpecs();
                    OrdersMeHeader ordersMeHeader = new OrdersMeHeader();
                    Orders order = new Orders();
                    OrdTendersTP awardedOrderTenderTP = new OrdTendersTP();
                    EntityDetailsCore awardedTPEdc = new EntityDetailsCore();
                    RefOrdTenderCategories refOrdTenderCategories = new RefOrdTenderCategories();
                    RefOrdTenderPacks refOrdTenderPacks = new RefOrdTenderPacks();
                    RefOrdTenderStatus refOrdTenderStatus = new RefOrdTenderStatus();

                    //Ref Tables
                    refOrdTenderPacks.PackSequence = this.ColumnExists(dr, "pack_sequence") ? Int32.Parse(dr["pack_sequence"].ToString()) : 0;
                    refOrdTenderPacks.PackDesc = this.ColumnExists(dr, "pack_desc") ? dr["pack_desc"].ToString() : "";
                    refOrdTenderStatus.StatusSequence = this.ColumnExists(dr, "status_sequence") ? Int32.Parse(dr["status_sequence"].ToString()) : 0;
                    refOrdTenderStatus.StatusDesc = this.ColumnExists(dr, "status_desc") ? dr["status_desc"].ToString() : "";
                    refOrdTenderCategories.CategorySequence = this.ColumnExists(dr, "category_sequence") && String.IsNullOrWhiteSpace(dr["category_sequence"].ToString()) == false ? Int32.Parse(dr["category_sequence"].ToString()) : 0;
                    refOrdTenderCategories.CategoryDesc = this.ColumnExists(dr, "category_desc") ? dr["category_desc"].ToString() : "";

                    awardedTPEdc.NameLong = this.ColumnExists(dr, "awarded_tp_name_long") ? dr["awarded_tp_name_long"].ToString() : "";
                    //Awarded Tender TP
                    awardedOrderTenderTP.TenderAmount = this.ColumnExists(dr, "tender_amount") && dr["tender_amount"] != DBNull.Value && dr["tender_amount"]!=null  ? double.Parse(dr["tender_amount"].ToString()) : 0;
                    awardedOrderTenderTP.ThirdPartyEDC = awardedTPEdc;
                    awardedOrderTenderTP.Sequence = this.ColumnExists(dr, "tender_sequence") ? Int32.Parse(dr["tender_sequence"].ToString()) : 0;
                    //Tenders Specs
                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.PackSequence = refOrdTenderPacks.PackSequence;
                    returnValue.StatusSequence = refOrdTenderStatus.StatusSequence;
                    returnValue.MeSequence = this.ColumnExists(dr, "me_sequence") ? long.Parse(dr["me_sequence"].ToString()) : -1;
                    returnValue.JobSequence = this.ColumnExists(dr, "job_sequence") ? long.Parse(dr["job_sequence"].ToString()) : -1;
                    returnValue.FlgSpecPublished = this.ColumnExists(dr, "flg_spec_published") ? bool.Parse(dr["flg_spec_published"].ToString()) : false;
                    returnValue.DatSpecPublished = this.ColumnExists(dr, "date_spec_published") ? Utilities.getDBDate(dr["date_spec_published"]) : DateTime.MinValue;
                    returnValue.DatSpecDeadline = this.ColumnExists(dr, "date_spec_deadline") ? Utilities.getDBDate(dr["date_spec_deadline"]) : DateTime.MinValue;
                    returnValue.FlgSpecExtended = this.ColumnExists(dr, "flg_spec_extended") ? bool.Parse(dr["flg_spec_extended"].ToString()) : false;
                    returnValue.DatSpecExtended = this.ColumnExists(dr, "date_spec_extended") ? Utilities.getDBDate(dr["date_spec_extended"]) : DateTime.MinValue;
                    returnValue.SpecNotes = this.ColumnExists(dr, "tp_notes") ? (dr["tp_notes"] == null || dr["tp_notes"] == DBNull.Value) ? "" : dr["tp_notes"].ToString() : string.Empty;
                    returnValue.FlgAwarded = this.ColumnExists(dr, "flg_awarded") ? bool.Parse(dr["flg_awarded"].ToString()) : false;
                    returnValue.DatAwarded = this.ColumnExists(dr, "date_awarded") ? Utilities.getDBDate(dr["date_awarded"]) : DateTime.MinValue;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;

                    //ME Header
                    ordersMeHeader.CategorySequence = refOrdTenderCategories.CategorySequence;
                    ordersMeHeader.MeProjectTitle = this.ColumnExists(dr, "me_project_title") ? dr["me_project_title"].ToString() : "";
                    ordersMeHeader.FlgSpecShowClient = this.ColumnExists(dr, "flg_spec_show_client") ? bool.Parse(dr["flg_spec_show_client"].ToString()) : false;
                    ordersMeHeader.FlgSpecShowJobAddress = this.ColumnExists(dr, "flg_spec_show_job_address") ? bool.Parse(dr["flg_spec_show_job_address"].ToString()) : false;

                    //Orders 
                    order.JobAddress = this.ColumnExists(dr, "job_address") ? dr["job_address"].ToString() : "";
                    order.JobRef = this.ColumnExists(dr, "job_ref") ? dr["job_ref"].ToString() : "";

                    //Assigning - Order is important
                    ordersMeHeader.RefOrdTenderCategories = refOrdTenderCategories;
                    ordersMeHeader.Order = order;
                    returnValue.RefOrdTenderPacks = refOrdTenderPacks;
                    returnValue.RefOrdTenderStatus = refOrdTenderStatus;
                    returnValue.OrderMeHeader = ordersMeHeader;
                    returnValue.TenderAwardedTP = awardedOrderTenderTP;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersSpecsClient LoadOrdersTendersDetails4Client(OleDbDataReader dr)
        {
            OrdTendersSpecsClient returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersSpecsClient();
                    Orders order = new Orders();
                    OrdersMeHeader ordersMeHeader = new OrdersMeHeader();
                    RefOrdTenderCategories refOrdTenderCategories = new RefOrdTenderCategories();
                    RefOrdTenderPacks refOrdTenderPacks = new RefOrdTenderPacks();
                    RefOrdTenderStatus refOrdTenderStatus = new RefOrdTenderStatus();
                    OrdTendersTP orderTenderTP = new OrdTendersTP();
                    //Ref Tables
                    refOrdTenderPacks.PackSequence = this.ColumnExists(dr, "pack_sequence") ? Int32.Parse(dr["pack_sequence"].ToString()) : 0;
                    refOrdTenderPacks.PackDesc = this.ColumnExists(dr, "pack_desc") ? dr["pack_desc"].ToString() : "";
                    refOrdTenderStatus.StatusSequence = this.ColumnExists(dr, "status_sequence") ? Int32.Parse(dr["status_sequence"].ToString()) : 0;
                    refOrdTenderStatus.StatusDesc = this.ColumnExists(dr, "status_desc") ? dr["status_desc"].ToString() : "";
                    refOrdTenderCategories.CategorySequence = this.ColumnExists(dr, "category_sequence") && String.IsNullOrWhiteSpace(dr["category_sequence"].ToString()) == false ? Int32.Parse(dr["category_sequence"].ToString()) : 0;
                    refOrdTenderCategories.CategoryDesc = this.ColumnExists(dr, "category_desc") ? dr["category_desc"].ToString() : "";
                  
                    //Tenders Specs
                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.PackSequence = refOrdTenderPacks.PackSequence;
                    returnValue.StatusSequence = refOrdTenderStatus.StatusSequence;
                    returnValue.MeSequence = this.ColumnExists(dr, "me_sequence") ? long.Parse(dr["me_sequence"].ToString()) : -1;
                    returnValue.JobSequence = this.ColumnExists(dr, "job_sequence") ? long.Parse(dr["job_sequence"].ToString()) : -1;
                    returnValue.FlgSpecPublished = this.ColumnExists(dr, "flg_spec_published") ? bool.Parse(dr["flg_spec_published"].ToString()) : false;
                    returnValue.DateSpecPublished = this.ColumnExists(dr, "date_spec_published") ? Utilities.getDBDate(dr["date_spec_published"]) : DateTime.MinValue;
                    returnValue.DateSpecDeadline = this.ColumnExists(dr, "date_spec_deadline") ? Utilities.getDBDate(dr["date_spec_deadline"]) : DateTime.MinValue;
                    returnValue.FlgSpecExtended = this.ColumnExists(dr, "flg_spec_extended") ? bool.Parse(dr["flg_spec_extended"].ToString()) : false;
                    returnValue.DateSpecExtended = this.ColumnExists(dr, "date_spec_extended") ? Utilities.getDBDate(dr["date_spec_extended"]) : DateTime.MinValue;
                    returnValue.SpecNotes = this.ColumnExists(dr, "tp_notes") ? (dr["tp_notes"] == null || dr["tp_notes"] == DBNull.Value) ? "" : dr["tp_notes"].ToString() : string.Empty;
                    returnValue.FlgAwarded = this.ColumnExists(dr, "flg_awarded") ? bool.Parse(dr["flg_awarded"].ToString()) : false;
                    returnValue.DateAwarded = this.ColumnExists(dr, "date_awarded") ? Utilities.getDBDate(dr["date_awarded"]) : DateTime.MinValue;
                    returnValue.AwardedTenderSequence = this.ColumnExists(dr, "awarded_tender_seq") ? long.Parse(dr["awarded_tender_seq"].ToString()) : -1;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;

                    //ME Header
                    ordersMeHeader.CategorySequence = refOrdTenderCategories.CategorySequence;
                    ordersMeHeader.MeProjectTitle = this.ColumnExists(dr, "me_project_title") ? dr["me_project_title"].ToString() : "";
                    ordersMeHeader.FlgSpecShowClient = this.ColumnExists(dr, "flg_spec_show_client") ? bool.Parse(dr["flg_spec_show_client"].ToString()) : false;
                    ordersMeHeader.FlgSpecShowJobAddress = this.ColumnExists(dr, "flg_spec_show_job_address") ? bool.Parse(dr["flg_spec_show_job_address"].ToString()) : false;

                    //Orders 
                    long longParam;
                    order.JobAddress = this.ColumnExists(dr, "job_address") ? dr["job_address"].ToString() : "";
                    order.JobClientName = this.ColumnExists(dr, "job_client_name") ? dr["job_client_name"].ToString() : "";
                    order.JobClientRef = this.ColumnExists(dr, "job_client_ref") ? dr["job_client_ref"].ToString() : "";
                    order.JobRef = this.ColumnExists(dr, "job_ref") ? dr["job_ref"].ToString() : "";
                    order.JobManagerId = (dr["job_manager"] == null || dr["job_manager"] == DBNull.Value) ? -1 : long.TryParse(dr["job_manager"].ToString(), out longParam) ? (long)longParam : -1;
                    DatabaseInfo dbInfo = new DatabaseInfo();
                    dbInfo.DatabaseType = this.DatabaseType;
                    dbInfo.ConnectionString = this.connectionString;
                    if (order.JobManagerId > 0)
                    {
                        order.JobManager = new EntityDetailsCoreDB(dbInfo).getEntityByEntityid( order.JobManagerId);
                    }

                    //Assigning - Order is important
                    ordersMeHeader.RefOrdTenderCategories = refOrdTenderCategories;
                    ordersMeHeader.Order = order;
                    returnValue.RefOrdTenderPacks = refOrdTenderPacks;
                    returnValue.RefOrdTenderStatus = refOrdTenderStatus;
                    returnValue.OrderMeHeader = ordersMeHeader;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersTP LoadOrdersTendersForTP(OleDbDataReader dr)
        {
            OrdTendersTP returnValue = null;
            try
            {
                if (dr != null)
                { 
                    returnValue = new OrdTendersTP();
                    OrdTendersSpecs ordTendersSpecs = new OrdTendersSpecs();
                    OrdersMeHeader ordersMeHeader = new OrdersMeHeader();
                    Orders refOrders = new Orders();
                    RefOrdTenderCategories refOrdTenderCategories = new RefOrdTenderCategories();
                    RefOrdTenderPacks refOrdTenderPacks = new RefOrdTenderPacks();
                    RefOrdTenderStatus refOrdTenderStatus = new RefOrdTenderStatus();

                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    returnValue.EntityId = this.ColumnExists(dr, "entity_id") ? long.Parse(dr["entity_id"].ToString()) : -1;
                    returnValue.TenderAccept = this.ColumnExists(dr, "tender_accept") ? Int32.Parse(dr["tender_accept"].ToString()) : -1;
                    returnValue.TPNotes = this.ColumnExists(dr, "tp_notes") ? (dr["tp_notes"] == null || dr["tp_notes"] == DBNull.Value) ? "" : dr["tp_notes"].ToString() : string.Empty;
                    returnValue.FlgTenderFinalised = this.ColumnExists(dr, "flg_tender_finalised") ? bool.Parse(dr["flg_tender_finalised"].ToString()) : false;
                    returnValue.DatTenderFinalised = this.ColumnExists(dr, "date_tender_finalised") ? Utilities.getDBDate(dr["date_tender_finalised"]) : DateTime.MinValue;
                    returnValue.FlgTenderUploads = this.ColumnExists(dr, "flg_tender_uploads") ? bool.Parse(dr["flg_tender_uploads"].ToString()) : false;
                    returnValue.TenderAmount = this.ColumnExists(dr, "tender_amount") && dr["tender_amount"] !=DBNull.Value ? double.Parse(dr["tender_amount"].ToString()) : 0;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;

                    //Ref Tables
                    refOrdTenderPacks.PackSequence = this.ColumnExists(dr, "pack_sequence") ? Int32.Parse(dr["pack_sequence"].ToString()) : 0;
                    refOrdTenderPacks.PackDesc = this.ColumnExists(dr, "pack_desc") ? dr["pack_desc"].ToString() : "";
                    refOrdTenderStatus.StatusSequence = this.ColumnExists(dr, "status_sequence") ? Int32.Parse(dr["status_sequence"].ToString()) : 0;
                    refOrdTenderStatus.StatusDesc = this.ColumnExists(dr, "status_desc") ? dr["status_desc"].ToString() : "";
                    refOrdTenderCategories.CategorySequence = this.ColumnExists(dr, "category_sequence") ? Int32.Parse(dr["category_sequence"].ToString()) : 0;
                    refOrdTenderCategories.CategoryDesc = this.ColumnExists(dr, "category_desc") ? dr["category_desc"].ToString() : "";

                    //Tenders Specs
                    ordTendersSpecs.Sequence = returnValue.JoinSequence;
                    ordTendersSpecs.PackSequence = refOrdTenderPacks.PackSequence;
                    ordTendersSpecs.StatusSequence = refOrdTenderStatus.StatusSequence;
                    ordTendersSpecs.MeSequence = this.ColumnExists(dr, "me_sequence") ? long.Parse(dr["me_sequence"].ToString()) : -1;
                    ordTendersSpecs.JobSequence = this.ColumnExists(dr, "job_sequence") ? long.Parse(dr["job_sequence"].ToString()) : -1;
                    ordTendersSpecs.FlgSpecPublished = this.ColumnExists(dr, "flg_spec_published") ? bool.Parse(dr["flg_spec_published"].ToString()) : false;
                    ordTendersSpecs.DatSpecPublished = this.ColumnExists(dr, "date_spec_published") ? Utilities.getDBDate(dr["date_spec_published"]) : DateTime.MinValue;
                    ordTendersSpecs.DatSpecDeadline = this.ColumnExists(dr, "date_spec_deadline") ? Utilities.getDBDate(dr["date_spec_deadline"]) : DateTime.MinValue;
                    ordTendersSpecs.FlgSpecExtended = this.ColumnExists(dr, "flg_spec_extended") ? bool.Parse(dr["flg_spec_extended"].ToString()) : false;
                    ordTendersSpecs.DatSpecExtended = this.ColumnExists(dr, "date_spec_extended") ? Utilities.getDBDate(dr["date_spec_extended"]) : DateTime.MinValue;
                    ordTendersSpecs.SpecNotes = this.ColumnExists(dr, "tp_notes") ? (dr["tp_notes"] == null || dr["tp_notes"] == DBNull.Value) ? "" : dr["tp_notes"].ToString() : string.Empty;
                    ordTendersSpecs.FlgAwarded = this.ColumnExists(dr, "flg_awarded") ? bool.Parse(dr["flg_awarded"].ToString()) : false;
                    ordTendersSpecs.DatAwarded = this.ColumnExists(dr, "date_awarded") ? Utilities.getDBDate(dr["date_awarded"]) : DateTime.MinValue;

                    //ME Header
                    ordersMeHeader.CategorySequence = refOrdTenderCategories.CategorySequence;
                    ordersMeHeader.MeProjectTitle = this.ColumnExists(dr, "me_project_title") ? dr["me_project_title"].ToString() : "";
                    ordersMeHeader.FlgSpecShowClient = this.ColumnExists(dr, "flg_spec_show_client") ? bool.Parse(dr["flg_spec_show_client"].ToString()) : false;
                    ordersMeHeader.FlgSpecShowJobAddress = this.ColumnExists(dr, "flg_spec_show_job_address") ? bool.Parse(dr["flg_spec_show_job_address"].ToString()) : false;
                    refOrders.JobRef = this.ColumnExists(dr, "job_ref") ? dr["job_ref"].ToString() : "";

                    
                    //Assigning - Order is important
                    ordersMeHeader.Order = refOrders;
                    ordersMeHeader.RefOrdTenderCategories = refOrdTenderCategories;
                    ordTendersSpecs.RefOrdTenderPacks = refOrdTenderPacks;
                    ordTendersSpecs.RefOrdTenderStatus = refOrdTenderStatus;
                    ordTendersSpecs.OrderMeHeader = ordersMeHeader;
                    returnValue.OrdersTendersSpecs = ordTendersSpecs;
                }
            }
            catch(Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersTPNotifications LoadOrdersTendersTPForNotificatons(OleDbDataReader dr)
        {
            OrdTendersTPNotifications returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersTPNotifications();
                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.ContractorName = this.ColumnExists(dr, "contractorName") ? dr["contractorName"].ToString() : "";
                    returnValue.TenderAccept = this.ColumnExists(dr, "tender_accept") ? Int32.Parse(dr["tender_accept"].ToString()) : -1;
                    returnValue.TPNotes = this.ColumnExists(dr, "tp_notes") ? (dr["tp_notes"] == null || dr["tp_notes"] == DBNull.Value) ? "" : dr["tp_notes"].ToString() : string.Empty;
                    returnValue.FlgTenderFinalised = this.ColumnExists(dr, "flg_tender_finalised") ? bool.Parse(dr["flg_tender_finalised"].ToString()) : false;
                    returnValue.DateTenderFinalised = this.ColumnExists(dr, "date_tender_finalised") ? Utilities.getDBDate(dr["date_tender_finalised"]) : DateTime.MinValue;
                    returnValue.FlgTenderUploads = this.ColumnExists(dr, "flg_tender_uploads") ? bool.Parse(dr["flg_tender_uploads"].ToString()) : false;
                    if (returnValue.TenderAccept == 1)
                        returnValue.TenderAcceptanceStatus = "Accepted";
                    else if (returnValue.TenderAccept == 2)
                        returnValue.TenderAcceptanceStatus = "Declined";
                    else
                        returnValue.TenderAcceptanceStatus = "Not Set";

                    //Ref Tables
                    returnValue.PackDesc = this.ColumnExists(dr, "pack_desc") ? dr["pack_desc"].ToString() : "";
                    returnValue.StatusDesc = this.ColumnExists(dr, "status_desc") ? dr["status_desc"].ToString() : "";
                    returnValue.CategoryDesc = this.ColumnExists(dr, "category_desc") ? dr["category_desc"].ToString() : "";

                    //Tenders Specs
                    returnValue.FlgSpecPublished = this.ColumnExists(dr, "flg_spec_published") ? bool.Parse(dr["flg_spec_published"].ToString()) : false;
                    returnValue.DateSpecPublished = this.ColumnExists(dr, "date_spec_published") ? Utilities.getDBDate(dr["date_spec_published"]) : DateTime.MinValue;
                    returnValue.SpecNotes = this.ColumnExists(dr, "spec_notes") ? dr["spec_notes"].ToString() : "";
                    returnValue.ClosingDate = this.ColumnExists(dr, "closing_date") ? Utilities.getDBDate(dr["closing_date"]) : DateTime.MinValue;
                    
                    
                    returnValue.FlgTenderAwarderd = this.ColumnExists(dr, "flg_awarded") ? bool.Parse(dr["flg_awarded"].ToString()) : false;
                    returnValue.DateTenderAwarded = this.ColumnExists(dr, "date_awarded") ? Utilities.getDBDate(dr["date_awarded"]) : DateTime.MinValue;

                    //ME Header
                    returnValue.ProjectTitle = this.ColumnExists(dr, "me_project_title") ? dr["me_project_title"].ToString() : "";
                    returnValue.FlgSpecShowClient = this.ColumnExists(dr, "flg_spec_show_client") ? bool.Parse(dr["flg_spec_show_client"].ToString()) : false;
                    returnValue.FlgSpecShowJobAddress = this.ColumnExists(dr, "flg_spec_show_job_address") ? bool.Parse(dr["flg_spec_show_job_address"].ToString()) : false;
                    //Orders
                    returnValue.JobRef = this.ColumnExists(dr, "job_ref") ? dr["job_ref"].ToString() : "";
                    returnValue.JobAddress = this.ColumnExists(dr, "job_address") ? dr["job_address"].ToString() : "";



                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersTP LoadOrdersTendersForClient(OleDbDataReader dr)
        {
            OrdTendersTP returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersTP();

                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    returnValue.EntityId = this.ColumnExists(dr, "entity_id") ? long.Parse(dr["entity_id"].ToString()) : -1;
                    returnValue.TenderAccept = this.ColumnExists(dr, "tender_accept") ? Int32.Parse(dr["tender_accept"].ToString()) : -1;
                    returnValue.TPNotes = this.ColumnExists(dr, "tp_notes") ? (dr["tp_notes"] == null || dr["tp_notes"] == DBNull.Value) ? "" : dr["tp_notes"].ToString() : string.Empty;
                    returnValue.FlgTenderFinalised = this.ColumnExists(dr, "flg_tender_finalised") ? bool.Parse(dr["flg_tender_finalised"].ToString()) : false;
                    returnValue.DatTenderFinalised = this.ColumnExists(dr, "date_tender_finalised") ? Utilities.getDBDate(dr["date_tender_finalised"]) : DateTime.MinValue;
                    returnValue.FlgTenderUploads = this.ColumnExists(dr, "flg_tender_uploads") ? bool.Parse(dr["flg_tender_uploads"].ToString()) : false;
                    returnValue.TenderAmount = this.ColumnExists(dr, "tender_amount") && dr["tender_amount"] != DBNull.Value ? double.Parse(dr["tender_amount"].ToString()) : 0;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;

                    DatabaseInfo dbInfo = new DatabaseInfo();
                    dbInfo.DatabaseType = this.DatabaseType;
                    dbInfo.ConnectionString = this.connectionString;
                    if (returnValue.EntityId > 0)
                    {
                        returnValue.ContractorName = new EntityDetailsCoreDB(dbInfo).getEntityByEntityid(returnValue.EntityId).NameLong;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersSpecsFiles LoadOrdersTendersSpecsFiles(OleDbDataReader dr)
        {
            OrdTendersSpecsFiles returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersSpecsFiles();
                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    returnValue.FileDesc = this.ColumnExists(dr, "file_desc") ? (dr["file_desc"] == null || dr["file_desc"] == DBNull.Value) ? "" : dr["file_desc"].ToString() : string.Empty;
                    returnValue.FileCabId = this.ColumnExists(dr, "file_cab_id") ? (dr["file_cab_id"] == null || dr["file_cab_id"] == DBNull.Value) ? "" : dr["file_cab_id"].ToString() : string.Empty;
                    returnValue.FileDir = this.ColumnExists(dr, "file_dir") ? (dr["file_dir"] == null || dr["file_dir"] == DBNull.Value) ? "" : dr["file_dir"].ToString() : string.Empty;
                    returnValue.FileName = this.ColumnExists(dr, "file_name") ? (dr["file_name"] == null || dr["file_name"] == DBNull.Value) ? "" : dr["file_name"].ToString() : string.Empty;
                    returnValue.FlgUploadComplete = this.ColumnExists(dr, "flg_upload_complete") ? bool.Parse(dr["flg_upload_complete"].ToString()) : false;
                    returnValue.FileDesc = this.ColumnExists(dr, "file_desc") ? (dr["file_desc"] == null || dr["file_desc"] == DBNull.Value) ? "" : dr["file_desc"].ToString() : string.Empty;
                    returnValue.GuId = this.ColumnExists(dr, "gu_id") ? long.Parse(dr["gu_id"].ToString()) : -1;
                    returnValue.VersionNo = this.ColumnExists(dr, "version_no") ? Int32.Parse(dr["version_no"].ToString()) : -1;
                    returnValue.FlgNotLatestVersion = this.ColumnExists(dr, "flg_not_latest_version") ? bool.Parse(dr["flg_not_latest_version"].ToString()) : false;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersTPQS LoadOrdersTendersTPQS(OleDbDataReader dr)
        {
            OrdTendersTPQS returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersTPQS();
                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.FlgDeleted = this.ColumnExists(dr, "flg_deleted") ? bool.Parse(dr["flg_deleted"].ToString()) : false;
                    returnValue.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    returnValue.TPQuestion = this.ColumnExists(dr, "tp_question") ? (dr["tp_question"] == null || dr["tp_question"] == DBNull.Value) ? "" : dr["tp_question"].ToString() : string.Empty;
                    returnValue.FlgAnswered = this.ColumnExists(dr, "flg_answered") ? bool.Parse(dr["flg_answered"].ToString()) : false;
                    returnValue.OwnerAnswer = this.ColumnExists(dr, "owner_answer") ? (dr["owner_answer"] == null || dr["owner_answer"] == DBNull.Value) ? "" : dr["owner_answer"].ToString() : string.Empty;
                    returnValue.FlgPublicAnswer = this.ColumnExists(dr, "flg_public_answer") ? bool.Parse(dr["flg_public_answer"].ToString()) : false;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private OrdTendersTPFiles LoadOrdersTendersTPFiles(OleDbDataReader dr)
        {
            OrdTendersTPFiles returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new OrdTendersTPFiles();
                    returnValue.Sequence = long.Parse(dr["sequence"].ToString());
                    returnValue.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    returnValue.FileDesc = this.ColumnExists(dr, "file_desc") ? (dr["file_desc"] == null || dr["file_desc"] == DBNull.Value) ? "" : dr["file_desc"].ToString() : string.Empty;
                    returnValue.FileCabId = this.ColumnExists(dr, "file_cab_id") ? (dr["file_cab_id"] == null || dr["file_cab_id"] == DBNull.Value) ? "" : dr["file_cab_id"].ToString() : string.Empty;
                    returnValue.FileDir = this.ColumnExists(dr, "file_dir") ? (dr["file_dir"] == null || dr["file_dir"] == DBNull.Value) ? "" : dr["file_dir"].ToString() : string.Empty;
                    returnValue.FileName = this.ColumnExists(dr, "file_name") ? (dr["file_name"] == null || dr["file_name"] == DBNull.Value) ? "" : dr["file_name"].ToString() : string.Empty;
                    returnValue.FlgUploadComplete = this.ColumnExists(dr, "flg_upload_complete") ? bool.Parse(dr["flg_upload_complete"].ToString()) : false;
                    returnValue.FileDesc = this.ColumnExists(dr, "file_desc") ? (dr["file_desc"] == null || dr["file_desc"] == DBNull.Value) ? "" : dr["file_desc"].ToString() : string.Empty;
                    returnValue.GuId = this.ColumnExists(dr, "gu_id") ? long.Parse(dr["gu_id"].ToString()) : -1;
                    returnValue.VersionNo = this.ColumnExists(dr, "version_no") ? Int32.Parse(dr["version_no"].ToString()) : -1;
                    returnValue.FlgNotLatestVersion = this.ColumnExists(dr, "flg_not_latest_version") ? bool.Parse(dr["flg_not_latest_version"].ToString()) : false;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private RefOrdTenderStatus LoadRefOrderTenderStatus(OleDbDataReader dr)
        {
            RefOrdTenderStatus returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new RefOrdTenderStatus();
                    returnValue.StatusSequence = Int32.Parse(dr["status_sequence"].ToString());
                    returnValue.FlgDeleted = this.ColumnExists(dr, "flg_deleted") ? bool.Parse(dr["flg_deleted"].ToString()) : false;
                    returnValue.StatusDesc = this.ColumnExists(dr, "status_desc") ? (dr["status_desc"] == null || dr["status_desc"] == DBNull.Value) ? "" : dr["status_desc"].ToString() : string.Empty;
                    returnValue.StatusRowIndex = this.ColumnExists(dr, "status_row_index") ? Int32.Parse(dr["status_row_index"].ToString()) : -1;
                    returnValue.CreatedBy = this.ColumnExists(dr, "created_by") ? Int32.Parse(dr["created_by"].ToString()) : -1;
                    returnValue.DateCreated = this.ColumnExists(dr, "date_created") ? Utilities.getDBDate(dr["date_created"]) : DateTime.MinValue;
                    returnValue.LastAmendedBy = this.ColumnExists(dr, "last_amended_by") ? Int32.Parse(dr["last_amended_by"].ToString()) : -1;
                    returnValue.DateLastAmended = this.ColumnExists(dr, "date_last_amended") ? Utilities.getDBDate(dr["date_last_amended"]) : DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

    }
}
