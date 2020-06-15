using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
namespace SimplicityOnlineWebApi.DAL
{
    public class Cld_Ord_Labels_DB : MainDB
    {
        public Cld_Ord_Labels_DB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertCld_Ord_Labels(out long sequence, long jobSequence, string tagNo, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert = 
                        new OleDbCommand(Cld_Ord_Labels_Queries.insert(this.DatabaseType, -1, jobSequence, tagNo, createdBy, createdDate, createdBy, createdDate), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record. '" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("insertCld_Ord_Labels:" + ex.Message);
            }
            return returnValue;
        }

        public Cld_Ord_Labels selectCld_Ord_LabelsByJobSequenceAndTag(long jobSequence, string tagNo)
        {
            Cld_Ord_Labels returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Queries.getSelectAllByJobSequenceAndTagNo(this.DatabaseType, jobSequence, tagNo), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Cld_Ord_Labels(dr);
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

        internal List<Cld_Ord_Labels> selectAllCld_Ord_LabelsByJobSequence(long jobSequence)
        {
            List<Cld_Ord_Labels> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Queries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Cld_Ord_Labels>();
                                while (dr.Read())
                                { 
                                    returnValue.Add(Load_Cld_Ord_Labels(dr));
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

        private Cld_Ord_Labels Load_Cld_Ord_Labels(OleDbDataReader dr)
        {
            Cld_Ord_Labels oiFireProtectionI = null;
            try
            { 
                if(dr!=null)
                { 
                    oiFireProtectionI = new Cld_Ord_Labels();
                    oiFireProtectionI.Sequence = long.Parse(dr["sequence"].ToString());
                    oiFireProtectionI.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    oiFireProtectionI.TagNo = (string)dr["tag_no"];
                }
            }
            catch(Exception ex)
            {

            }
            return oiFireProtectionI;
        }

        internal List<Cld_Ord_Labels> selectAllCld_Ord_LabelsByJobSequenceAndTagNumberSearch(long jobSequence, string tagNo)
        {
            List<Cld_Ord_Labels> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Queries.getSelectAllByJobSequenceAndTagNoSearch(this.DatabaseType,jobSequence, tagNo), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Cld_Ord_Labels>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Cld_Ord_Labels(dr));
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

        internal List<Cld_Ord_Labels> selectAllOtherCld_Ord_LabelsByJobSequenceAndTagNumberSearch(long jobSequence,long sequence, string tagNo)
        {
            List<Cld_Ord_Labels> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Queries.GetAllOtherTagNoByJobSequenceAndTagNo(this.DatabaseType, jobSequence, sequence, tagNo), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Cld_Ord_Labels>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Cld_Ord_Labels(dr));
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

        internal Cld_Ord_Labels selectAllCld_Ord_LabelsBySequence(long sequence)
        {
            Cld_Ord_Labels returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Queries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Cld_Ord_Labels(dr);
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

        public bool updateCld_Ord_Labels(long sequence, string tagNo, int modifiedBy, DateTime? modifiedDate)
        {
            bool returnValue = false;
            //sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(Cld_Ord_Labels_Queries.updateTagNo(this.DatabaseType, sequence, tagNo, modifiedBy, modifiedDate), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        //string sql = "select @@IDENTITY";
                        //using (OleDbCommand objCommand =
                        //    new OleDbCommand(sql, conn))
                        //{
                        //    OleDbDataReader dr = objCommand.ExecuteReader();
                        //    if (dr.HasRows)
                        //    {
                        //        dr.Read();
                        //        sequence = long.Parse(dr[0].ToString());
                        //    }
                        //    else
                        //    {
                        //        //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record. '" + METHOD_NAME + "'\n";
                        //    }
                        //}
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
    }
}
